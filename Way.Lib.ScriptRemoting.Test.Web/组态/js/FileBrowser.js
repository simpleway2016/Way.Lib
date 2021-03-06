var FileBrowser = (function () {
    function FileBrowser() {
        var _this = this;
        this.initdata = false;
        this.parentid = 0;
        this.uploading = false;
        this.backgroundElement = document.createElement("DIV");
        this.backgroundElement.className = "FileBrowserBg";
        this.backgroundElement.style.visibility = "hidden";
        this.backgroundElement.style.width = "100%";
        this.backgroundElement.style.height = "100%";
        this.backgroundElement.style.position = "absolute";
        this.backgroundElement.style.zIndex = "899";
        document.body.appendChild(this.backgroundElement);
        this.rootElement = document.createElement("DIV");
        this.rootElement.className = "FileBrowser";
        this.rootElement.style.zIndex = "900";
        this.rootElement.style.visibility = "hidden";
        document.body.appendChild(this.rootElement);
        this.rootElement.style.left = (window.innerWidth - this.rootElement.offsetWidth) / 2 + "px";
        this.rootElement.style.top = (window.innerHeight - this.rootElement.offsetHeight) / 2 + "px";
        var toolDiv = document.createElement("DIV");
        toolDiv.style.width = "100%";
        toolDiv.style.height = "30px";
        toolDiv.style.position = "absolute";
        toolDiv.style.borderBottom = "1px solid #ccc";
        var html = "<div style='position:absolute;right:70px;top:5px;cursor:pointer;background-repeat:no-repeat;background-image:url(/images/browser/folder.png);background-size:16px 16px;padding-left:20px;font-size:12px;'>添加文件夹</div>";
        html += "<label style='font-size:12px;background:#e1e1e1;border:1px solid #adadad;padding:2px;margin-left:3px;' for='xFile_1234889'>选择文件</label><div style='width:130px;height:22px;position:absolute;left:60px;top:4px;font-size:12px;'></div><input style='width:0px;' id='xFile_1234889' type='file'><input style='margin-left:135px;' type='button' value='上传'>";
        html += "<div style='position:absolute;right:10px;top:5px;cursor:pointer;background-repeat:no-repeat;background-image:url(/images/browser/close.png);background-size:16px 16px;padding-left:20px;font-size:12px;'>关闭</div>";
        html += "<div style='position:absolute;right:180px;top:5px;cursor:pointer;background-repeat:no-repeat;background-image:url(/images/browser/clear.png);background-size:16px 16px;padding-left:20px;font-size:12px;'>选择空图片</div>";
        toolDiv.innerHTML = html;
        this.fileElement = toolDiv.children[3];
        this.fileElement.addEventListener("change", function () {
            toolDiv.children[2].innerHTML = (_this.fileElement.value);
        }, false);
        toolDiv.children[0].addEventListener("click", function () { _this.addFolderDialog(); }, false);
        toolDiv.children[4].addEventListener("click", function () { _this.updateFile(); }, false);
        toolDiv.children[5].addEventListener("click", function () {
            _this.hide();
        }, false);
        toolDiv.children[6].addEventListener("click", function () { if (_this.onSelectFile) {
            _this.onSelectFile("");
        } _this.hide(); }, false);
        this.rootElement.appendChild(toolDiv);
        this.container = document.createElement("DIV");
        this.container.style.width = "100%";
        this.container.style.top = "30px";
        this.container.style.bottom = "0px";
        this.container.style.position = "absolute";
        this.container.style.overflowX = "hidden";
        this.container.style.overflowY = "auto";
        this.rootElement.appendChild(this.container);
        this.loadingElement = document.createElement("IMG");
        this.loadingElement.src = "/images/browser/loading.gif";
        this.loadingElement.style.width = "80px";
        this.loadingElement.style.height = "80px";
        this.loadingElement.style.position = "absolute";
        this.hideLoading();
        this.rootElement.appendChild(this.loadingElement);
        this.loadingElement.style.left = (this.rootElement.offsetWidth - this.loadingElement.offsetWidth) / 2 + "px";
        this.loadingElement.style.top = (this.rootElement.offsetHeight - this.loadingElement.offsetHeight) / 2 + "px";
        this.serverController = "SunRizServer.Controllers.ImageFileManager".controller();
        document.body.addEventListener("click", function () {
            var menuEle = document.body.querySelector(".FileBrowserFolderMenu");
            menuEle.style.visibility = "hidden";
        }, false);
    }
    FileBrowser.prototype.updateFile = function () {
        var _this = this;
        if (this.uploading)
            return;
        if (this.fileElement.files.length == 0) {
            alert("请选择文件");
            return;
        }
        this.uploading = true;
        this.showLoading();
        this.serverController.server.GetNewFileName(function (ret, err) {
            if (err) {
                _this.hideLoading();
                alert(err);
            }
            else {
                var filename = ret;
                _this.serverController.uploadFile(_this.fileElement, filename, function (ret, totalSize, uploaded, err) {
                    if (ret == "ok") {
                        _this.hideLoading();
                        _this.addFile(filename);
                        _this.fileElement.value = "";
                        _this.uploading = false;
                    }
                });
            }
        });
    };
    FileBrowser.prototype.addFile = function (filename) {
        var _this = this;
        this.showLoading();
        this.serverController.server.AddFileByUpload(this.parentid, function (ret, err) {
            _this.hideLoading();
            if (err) {
                alert(err);
            }
            else {
                var ele = _this.additem(ret.Name, false, "/ImageFiles/" + ret.FileName);
                ele._data = ret;
            }
        });
    };
    FileBrowser.prototype.addFolderDialog = function () {
        var _this = this;
        var name = window.prompt("请输入文件夹名称", "");
        if (name && name.length > 0) {
            this.showLoading();
            this.serverController.server.AddFolder(name, this.parentid, function (ret, err) {
                _this.hideLoading();
                if (err)
                    alert(err);
                else {
                    var ele = _this.additem(name, true);
                    ele._data = ret;
                    if (_this.container.children.length > 1) {
                        _this.container.removeChild(ele);
                        _this.container.insertBefore(ele, _this.container.children[0]);
                    }
                }
            });
        }
    };
    FileBrowser.prototype.additem = function (name, isFolder, imgPath) {
        if (imgPath === void 0) { imgPath = null; }
        var itemEle = document.createElement("DIV");
        itemEle.style.float = "left";
        itemEle.className = "FileBrowserItem";
        var iconEle = document.createElement("DIV");
        if (isFolder) {
            iconEle.style.backgroundImage = "url(/images/browser/folder.png)";
            iconEle.style.backgroundSize = "64px 64px";
            iconEle.addEventListener("click", function () { }, false);
        }
        else {
            iconEle.style.backgroundImage = "url(" + imgPath + ")";
            iconEle.style.backgroundSize = "90% auto";
        }
        iconEle.style.backgroundPosition = "center";
        iconEle.style.backgroundRepeat = "no-repeat";
        iconEle.style.width = "100px";
        iconEle.style.height = "80px";
        itemEle.appendChild(iconEle);
        var lableEle = document.createElement("DIV");
        lableEle.innerHTML = name;
        lableEle.style.width = "100%";
        lableEle.style.textAlign = "center";
        itemEle.appendChild(lableEle);
        this.container.appendChild(itemEle);
        itemEle._browser = this;
        itemEle.oncontextmenu = this.itemContextMenu;
        itemEle.addEventListener("click", this.itemClick, false);
        itemEle.updateName = function (n) {
            itemEle._data.Name = n;
            lableEle.innerHTML = n;
        };
        return itemEle;
    };
    FileBrowser.prototype.itemContextMenu = function (e) {
        var itemEle = e.target;
        while (!itemEle._browser) {
            itemEle = itemEle.parentElement;
        }
        var self = itemEle._browser;
        var menuEle;
        menuEle = document.body.querySelector(".FileBrowserFolderMenu");
        menuEle.style.left = e.clientX + "px";
        menuEle.style.top = e.clientY + "px";
        menuEle.style.visibility = "visible";
        menuEle._data = itemEle._data;
        menuEle._obj = self;
        menuEle.ele = itemEle;
    };
    FileBrowser.prototype.itemClick = function (e) {
        var itemEle = e.target;
        while (!itemEle._browser) {
            itemEle = itemEle.parentElement;
        }
        var self = itemEle._browser;
        if (itemEle._data.IsFolder) {
            self.loadImages(itemEle._data.id);
        }
        else {
            if (self.onSelectFile) {
                self.onSelectFile("/ImageFiles/" + itemEle._data.FileName);
            }
        }
    };
    FileBrowser.prototype.rename = function (ele, id) {
        var newname = window.prompt("请输入名称", ele._data.Name);
        if (newname && newname.length > 0) {
            this.serverController.server.ChangeName(newname, ele._data.id, function (ret, err) {
                if (err)
                    alert(err);
                else {
                    ele.updateName(newname);
                }
            });
        }
    };
    FileBrowser.prototype.copy = function (ele, id) {
        if (ele._data.FileName) {
            window.copyToClipboard("/ImageFiles/" + ele._data.FileName);
        }
    };
    FileBrowser.prototype.deleteFile = function (ele, id) {
        var _this = this;
        if (window.confirm("确定删除吗？")) {
            this.showLoading();
            this.serverController.server.DeleteFile(id, function (ret, err) {
                _this.hideLoading();
                if (err)
                    alert(err);
                else {
                    ele.parentElement.removeChild(ele);
                }
            });
        }
    };
    FileBrowser.prototype.showLoading = function () {
        this.loadingElement.style.visibility = "visible";
    };
    FileBrowser.prototype.hideLoading = function () {
        this.loadingElement.style.visibility = "hidden";
    };
    FileBrowser.prototype.show = function () {
        this.backgroundElement.style.visibility = "visible";
        if (!this.initdata) {
            this.initdata = true;
            this.loadImages(0);
        }
        this.rootElement.style.visibility = "visible";
    };
    FileBrowser.prototype.hide = function () {
        this.backgroundElement.style.visibility = "hidden";
        this.rootElement.style.visibility = "hidden";
        if (this.onHide) {
            this.onHide();
        }
    };
    FileBrowser.prototype.loadImages = function (parentid) {
        var _this = this;
        this.showLoading();
        this.serverController.server.GetFiles(parentid, function (ret, err) {
            _this.hideLoading();
            if (err)
                alert(err);
            else {
                _this.container.innerHTML = "";
                if (parentid != 0) {
                    var ele = _this.additem("...", true);
                    ele._data = {
                        IsFolder: true,
                        id: _this.parentid
                    };
                }
                _this.parentid = parentid;
                for (var i = 0; i < ret.length; i++) {
                    var data = ret[i];
                    var ele = _this.additem(data.Name, data.IsFolder, "/ImageFiles/" + data.FileName);
                    ele._data = data;
                }
            }
        });
    };
    return FileBrowser;
}());
//# sourceMappingURL=FileBrowser.js.map