
class FileBrowser
{
    backgroundElement: HTMLElement;
    rootElement: HTMLElement;
    container: HTMLElement;
    loadingElement: HTMLElement;
    fileElement: HTMLElement;
    serverController: any;
    initdata = false;
    parentid = 0;
    uploading = false;
    onSelectFile: (filepath: string) => any;
    constructor()
    {
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
        html += "<input type='file'><input type='button' value='上传'>";
        html += "<div style='position:absolute;right:10px;top:5px;cursor:pointer;background-repeat:no-repeat;background-image:url(/images/browser/close.png);background-size:16px 16px;padding-left:20px;font-size:12px;'>关闭</div>";
        html += "<div style='position:absolute;right:180px;top:5px;cursor:pointer;background-repeat:no-repeat;background-image:url(/images/browser/clear.png);background-size:16px 16px;padding-left:20px;font-size:12px;'>选择空图片</div>";

        toolDiv.innerHTML = html;
        this.fileElement = <HTMLElement>toolDiv.children[1];
        toolDiv.children[0].addEventListener("click", () => { this.addFolderDialog(); }, false);
        toolDiv.children[2].addEventListener("click", () => { this.updateFile(); }, false);
        toolDiv.children[3].addEventListener("click", () => { this.hide(); }, false);
        toolDiv.children[4].addEventListener("click", () => { if (this.onSelectFile) { this.onSelectFile(""); } this.hide(); }, false);
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
        (<any>this.loadingElement).src = "/images/browser/loading.gif";
        this.loadingElement.style.width = "80px";
        this.loadingElement.style.height = "80px";
        this.loadingElement.style.position = "absolute";
        this.hideLoading();
        this.rootElement.appendChild(this.loadingElement);
        this.loadingElement.style.left = (this.rootElement.offsetWidth - this.loadingElement.offsetWidth) / 2 + "px";
        this.loadingElement.style.top = (this.rootElement.offsetHeight - this.loadingElement.offsetHeight) / 2 + "px";

        this.serverController = (<any>"SunRizServer.Controllers.ImageFileManager").controller();

        document.body.addEventListener("click", () => {
            var menuEle = <HTMLElement>document.body.querySelector(".FileBrowserFolderMenu");
            menuEle.style.visibility = "hidden"; 
        }, false);
    }
    updateFile()
    {
        if (this.uploading)
            return;
        if ((<any>this.fileElement).files.length == 0)
        {
            alert("请选择文件");
            return;
        }
        this.uploading = true;
        this.showLoading();
        this.serverController.server.GetNewFileName((ret, err) => {
            if (err)
            {
                this.hideLoading();
                alert(err);
            }
            else {
                var filename = ret;
                this.serverController.uploadFile(this.fileElement, filename, (ret, totalSize, uploaded, err) => {
                    if (ret == "ok") {                        
                        this.hideLoading();
                        this.addFile(filename);
                        (<any>this.fileElement).value = "";
                        this.uploading = false;
                    }
                });
            }
        });
       
    }
    addFile(filename)
    {
        this.showLoading();
        this.serverController.server.AddFileByUpload(this.parentid, (ret, err) => {
            this.hideLoading();
            if (err) {
                alert(err);
            }
            else {
                var ele = this.additem(ret.Name, false, "/ImageFiles/" + ret.FileName);
                (<any>ele)._data = ret;
            }
        });
    }

    addFolderDialog()
    {
        var name = window.prompt("请输入文件夹名称", "");
        if (name && name.length > 0)
        {
            this.showLoading();
            this.serverController.server.AddFolder(name, this.parentid, (ret, err) => {
                this.hideLoading();
                if (err)
                    alert(err);
                else {
                    var ele = this.additem(name, true);
                    (<any>ele)._data = ret;
                    if (this.container.children.length > 1) {
                        this.container.removeChild(ele);
                        this.container.insertBefore(ele, this.container.children[0]);
                    }
                }
            });
        }
    }

    additem(name: string, isFolder: boolean,imgPath:string = null) {
        var itemEle = document.createElement("DIV");
        (<any>itemEle).style.float = "left";
        itemEle.className = "FileBrowserItem";

        var iconEle = document.createElement("DIV");
        if (isFolder) {
            iconEle.style.backgroundImage = "url(/images/browser/folder.png)";
            iconEle.style.backgroundSize = "64px 64px";
            iconEle.addEventListener("click", () => { }, false);
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
        (<any>itemEle)._browser = this;
        itemEle.oncontextmenu = this.itemContextMenu;
        itemEle.addEventListener("click", this.itemClick, false);
        (<any>itemEle).updateName = (n) => {
            (<any>itemEle)._data.Name = n;
            lableEle.innerHTML = n;
        };
        return itemEle;
    }

    itemContextMenu(e: PointerEvent)
    {
        var itemEle = <any>e.target;
        while (!itemEle._browser) {
            itemEle = itemEle.parentElement;
        }
        var self = itemEle._browser;
        var menuEle: HTMLElement;

        menuEle = <HTMLElement>document.body.querySelector(".FileBrowserFolderMenu");
        menuEle.style.left = e.clientX + "px";
        menuEle.style.top = e.clientY + "px";
        menuEle.style.visibility = "visible"; 

        (<any>menuEle)._data = itemEle._data;
        (<any>menuEle)._obj = self;
        (<any>menuEle).ele = itemEle;
    }

    itemClick(e: MouseEvent)
    {
        var itemEle = <any>e.target;
        while (!itemEle._browser)
        {
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
    }

    rename(ele, id) {
        var newname = window.prompt("请输入名称", ele._data.Name);
        if (newname && newname.length > 0)
        {
            this.serverController.server.ChangeName(newname, ele._data.id, (ret, err) => {
                if (err)
                    alert(err);
                else
                {
                    ele.updateName(newname);
                }
            });
        }
    }
    copy(ele, id) {
        if (ele._data.FileName) {
            (<any>window).copyToClipboard("/ImageFiles/" + ele._data.FileName);
        }
    }
    deleteFile(ele , id)
    {
        if (window.confirm("确定删除吗？"))
        {
            this.showLoading();
            this.serverController.server.DeleteFile(id, (ret, err) => {
                this.hideLoading();
                if (err)
                    alert(err);
                else {
                    ele.parentElement.removeChild(ele);
                }
            });
        }
    }

    showLoading()
    {
        this.loadingElement.style.visibility = "visible";
    }
    hideLoading()
    {
        this.loadingElement.style.visibility = "hidden";
    }
    show()
    {
        this.backgroundElement.style.visibility = "visible";
        if (!this.initdata)
        {
            this.initdata = true;
            this.loadImages(0);
        }
        this.rootElement.style.visibility = "visible";
    }
    hide()
    {
        this.backgroundElement.style.visibility = "hidden";
        this.rootElement.style.visibility = "hidden";
    }

    loadImages(parentid)
    {
        this.showLoading();
        this.serverController.server.GetFiles(parentid, (ret, err) => {
            this.hideLoading();
            if (err) alert(err);
            else {
               
                this.container.innerHTML = "";
                if (parentid != 0)
                {
                    var ele = this.additem("...",true);
                    (<any>ele)._data = {
                        IsFolder:true,
                        id : this.parentid //old parentid  返回上级
                    };
                }
                this.parentid = parentid;
                for (var i = 0; i < ret.length; i++)
                {
                    var data = ret[i];
                    var ele = this.additem(data.Name, data.IsFolder, "/ImageFiles/" + data.FileName);
                    (<any>ele)._data = data;
                }
            }
        });
    }
}