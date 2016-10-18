function JS_UpLoadFiles(clientid, guid, folderpath, addbuttonid , itemWidth,filecss) {
    var instance = this;
    var currentFileObj;
    var currentDisplayObj;
    var addButton;
    var divObj = document.getElementById("div_" + clientid);
    if (!divObj)
        return;
    var uploading = false;
    var moveleft = 0;
    var movetop = 0;
    this.onUploadFinished = false;
    this.onFileChanged = false;//function (filecount)


    this.moveLocation = function (left, top) {
        //moveleft = left;
        //movetop = top;
    }

    function hasFixedParent(e) {
        if (e.style.position == "fixed")
            return true;

        var p = e.offsetParent;
        
        while (p != null && p) {
            if (p.style.position == "fixed")
                return true;
            p = p.offsetParent;
        }
        return false;
    }


    this.resetLocation = function () {
        currentFileObj.style.left = addButton.offset().left + "px";
        currentFileObj.style.top = addButton.offset().top + "px";
        currentFileObj.style.width = addButton.outerWidth() + "px";
        currentFileObj.style.height = addButton.outerHeight() + "px";
    }

    function addButton_MouseOver() {
        try {
            instance.resetLocation();
        }
        catch (err) {
            alert(err.message + "\r\naddButton_MouseOver");
        }
    }

   

    function invokerCompleted(json, text, error) {
        try {
            if (!error) {
                if (json == null) {
                    window.setTimeout(getProgress, 1000);
                    return;
                }
                var percent = json.Percent;
                var serverErr = json.Error;
                if (serverErr && serverErr.length > 0) {
                    alert(serverErr);

                    while (divObj.children.length > 0) {
                        var divitem = divObj.children[0];
                        var fileobj = divitem._fileobj;
                        fileobj.parentElement.removeChild(fileobj);
                        divObj.removeChild(divitem);
                    }
                    uploading = false;
                    document.getElementsByName("xml_" + clientid)[0].value = text;
                    divObj.innerHTML = "请重新选择文件上传！";
                    instance.resetLocation();

                    return;
                }
                if (percent == 1000) {
                    while (divObj.children.length > 0) {
                        var divitem = divObj.children[0];
                        var fileobj = divitem._fileobj;
                        fileobj.parentElement.removeChild(fileobj);
                        divObj.removeChild(divitem);
                    }
                    uploading = false;
                    document.getElementsByName("xml_" + clientid)[0].value = text;
                    divObj.innerHTML = "文件上传完成！";
                    try{
                        
                        if (instance.onUploadFinished) {
                            instance.onUploadFinished();
                        }
                        instance.resetLocation();
                    }
                    catch (e22) {
                        alert(e22.message);
                    }

                    return;
                }
                else {
                    var divitem = divObj.children[0];
                    var c = "<div style='position:relative;left:0px;top:0px;width:" + percent + "%;height:100%;background-color:#1d95c7;'>&nbsp;</div>";
                    var d = "<div style='position:relative;color:green;left:0px;top:0px;width:100%;height:100%;'>正在上传 " + json.UploadingFileName + "<Br>" + percent + "%</div>";
                    divitem.innerHTML = c + d;
                }
            }
        }
        catch (e) {
            alert(e.message);
        }
        window.setTimeout(getProgress, 1000);
    }

    var invoker = new Invoker();
    invoker.async = false;
    invoker.setCompleted(invokerCompleted);
    var arg1 = new Array();
    arg1[0] = "____UpLoadFiles_getProgress";
    var arg2 = new Array();
    arg2[0] = guid;
    //invoker.invokeArgs(arg1 , arg2);



    var iframeObj = document.createElement("IFRAME");
    iframeObj.name = "IFRAME_" + clientid;
    iframeObj.style.display = "none";
    

    var p = document.createElement("DIV");
    p.innerHTML = "<form enctype=\"multipart/form-data\" method=\"post\"></form>";
    var myform = p.children[0];
    p.removeChild(myform);
    myform.action = location.href;
    if (myform.action.indexOf("?") >= 0)
        myform.action += "&______uploadfiles=1";
    else
        myform.action += "?______uploadfiles=1";
    myform.target = "IFRAME_" + clientid;

    var extLimitObject = document.createElement("INPUT");
    extLimitObject.type = "hidden";
    extLimitObject.name = "extLimit";
    myform.appendChild(extLimitObject);

    this.addLimit = function (ext, size)
    {
        extLimitObject.value += ext + "&" + size + ";";
    }

    var guidHidden = document.createElement("INPUT");
    guidHidden.type = "hidden";
    guidHidden.name = "guid";
    guidHidden.value = guid;
    myform.appendChild(guidHidden);

    guidHidden = document.createElement("INPUT");
    guidHidden.type = "hidden";
    guidHidden.name = "______uploadfiles__submit";
    guidHidden.value = folderpath;
    myform.appendChild(guidHidden);

    function deleteFile(fileObj, displayObj) {
        return function () {
            if (uploading == false && confirm("确定取消此文件的上传吗？")) {
                fileObj.parentElement.removeChild(fileObj);
                displayObj.parentElement.removeChild(displayObj);
                instance.resetLocation();

                if (instance.onFileChanged)
                    instance.onFileChanged(divObj.children.length);
            }
        }
    }

    function selectedFile() {

        try{
            if (divObj.children.length == 0) {
                divObj.innerHTML = "";
            }
            currentFileObj.style.display = "none";

            currentDisplayObj = document.createElement("DIV");
            if (filecss && filecss.length > 0) {
                currentDisplayObj.className = filecss;
                currentDisplayObj.setAttribute("style", "background-image:url(/inc/imgs/delete_icon.png);background-position:right;background-repeat:no-repeat;");
            }
            else {
                currentDisplayObj.setAttribute("style", "border:1px solid black;padding:5px;width:" + itemWidth + ";margin-top:2px;background-image:url(/inc/imgs/delete_icon.png);background-position:right;background-repeat:no-repeat;");
            }
            
            var filename = currentFileObj.value;
            filename = filename.substr(filename.lastIndexOf("\\") + 1);
            currentDisplayObj.innerHTML = filename
            currentDisplayObj.title = filename;
            divObj.appendChild(currentDisplayObj);
            currentDisplayObj._fileobj = currentFileObj;
            currentDisplayObj.onclick = deleteFile(currentFileObj, currentDisplayObj);

            addfileobj();
        }
        catch (e) {
            alert(e.message);
        }
    }

    function addfileobj() {
        currentFileObj = document.createElement("INPUT");
        currentFileObj.type = "file";
        currentFileObj.name = "f";
        currentFileObj.style.position = "absolute";
        currentFileObj.style.cursor = "pointer";
        instance.resetLocation();
        if ("MozOpacity" in currentFileObj.style) {
            currentFileObj.style.MozOpacity = 0;
        }
        if ("opacity" in currentFileObj.style) {
            currentFileObj.style.opacity = 0;
        }
        if ("filter" in currentFileObj.style) {
            currentFileObj.style.filter = "alpha(opacity=0)";
        }

        currentFileObj.style.zIndex = "99999";
        myform.appendChild(currentFileObj);

        currentFileObj.onchange = selectedFile;
        if (instance.onFileChanged)
            instance.onFileChanged(divObj.children.length);
    }

    

    function getProgress() {
        try {
            invoker.invokeArgs(arg1, arg2);
        }
        catch (e) {
            alert(e.message);
        }
    }

    this.hasFile = function () {
        return divObj.children.length > 0;
    }

    //this.addFile = function () {
    //    if (uploading == false)
    //        currentFileObj.click();
    //}
    this.upload = function () {
        if (instance.hasFile() == false) {
            if (instance.onUploadFinished)
                instance.onUploadFinished();
            return;
        }
        if (uploading)
            return;
        if (divObj.children.length == 0) {
            alert("请先添加需要上传的文件");
            return;
        }
        try {

            var invoker2 = new Invoker();
            invoker2.async = false;
            var arg_1 = new Array();
            arg_1[0] = "____UpLoadFiles_init_";
            var arg_2 = new Array();
            arg_2[0] = guid;
            invoker2.setCompleted(function (_doc, _text, _err) {
                if (!_err) {
                    myform.submit();
                    uploading = true;
                    window.setTimeout(getProgress, 1000);
                }
                else {
                    alert(_err);
                }
            });
            invoker2.invokeArgs(arg_1, arg_2);

           
        }
        catch (e) {
            alert(e.message);
        }
        //
    }

    function _windowLoad() {
        addButton = $('#' + addbuttonid);
        if (addButton.length == 0) {
            if (addbuttonid.length > 0)
                alert("无法找到id为" + addbuttonid + "的按钮");
            return;
        }

        addButton.mouseover(addButton_MouseOver);

        document.body.appendChild(iframeObj);
        document.body.appendChild(myform);
        addfileobj();
    }

    $(window).load(_windowLoad);
}