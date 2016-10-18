Date.prototype.format = function (format) {
    /* 
    * format="yyyy-MM-dd hh:mm:ss"; 
    */
    var o = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "H+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S": this.getMilliseconds()
    }

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4
        - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1
            ? o[k]
            : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}

String.prototype.getDateString = function() {
    return eval("new " + this.substr(1, this.length - 2)).format("yyyy-MM-dd hh:mm:ss");
}

String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

function myParseInt(myint) {
    try{
        while (myint.indexOf("0") == 0 && myint.length > 1) {
            myint = myint.substr(1);
        }
        return parseInt(myint);
    }
    catch (e) {
        return myint;
    }
}
 
Date.prototype.format = function (format) //author: meizz
{
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "h+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
    (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1,
      RegExp.$1.length == 1 ? o[k] :
        ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}


function doPrintFrame(name) {
    if (window.frames[name] == null) {
        document.getElementById(name).focus();
        document.getElementById(name).contentWindow.print();
    }
    else {
        window.frames[name].focus();
        print();
    }
}

function doPrintViewFrame(name) {
    if (window.frames[name] == null) {
        document.getElementById(name).focus();
        WebBrowser1.ExecWB(7, 1);
    }
    else {
        window.frames[name].focus();
        WebBrowser1.ExecWB(7, 1);
    }
}

function getLocationParams() {
    var index = location.href.indexOf("?") + 1;
    if(index > 0)
        return "&" + location.href.substr(index);
    return "";
}

function DX(n) {
    var strOutput = "";
    var strUnit = '仟佰拾亿仟佰拾万仟佰拾元角分';
    n += "00";
    var intPos = n.indexOf('.');
    if (intPos >= 0)
        n = n.substring(0, intPos) + n.substr(intPos + 1, 2);
    strUnit = strUnit.substr(strUnit.length - n.length);
    for (var i = 0; i < n.length; i++)
        strOutput += '零壹贰叁肆伍陆柒捌玖'.substr(n.substr(i, 1), 1) + strUnit.substr(i, 1);


    while (strOutput.substr(strOutput.length - 2, 1) == "零") {
        strOutput = strOutput.substr(0, strOutput.length - 2);
    }
    return strOutput;
}

function createXMLHttp() {
    var request = false;
    // Microsoft browsers
    if (window.XMLHttpRequest) {
        request = new XMLHttpRequest();
    }
    else if (window.ActiveXObject) {

        try {
            
            //Internet Explorer
            request = new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (e1) {
            try {
                //Internet Explorer
                request = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e2) {
                request = false;
            }
        }
    }

    return request;
}

var PageStateObject = null;


function LoadingObject(_element,txtEle) {
    this.divState = _element;
    this.textElement = txtEle;
    this.setStatus = function (x, y) {
        this.divState.style.left = x;
        this.divState.style.top = y;
        if (arguments.length > 2) {
            this.divState.children[2].innerHTML = arguments[2];
        }
    }

    this.hide = function () {
        
        this.divState.style.display = "none";
    }

    this.show = function () {
        this.divState.style.display = "";
        this.divState.style.left = (document.body.clientWidth - this.divState.offsetWidth) / 2;
        this.divState.style.top = (document.body.clientHeight - this.divState.offsetHeight) / 2 - 50;
        
    }
}

function Invoker(URL) {
    var lasatname;
    var lastargs;
    var intance = this;

    var completedHandler = null;
    this.setCompleted = function (c) {
        completedHandler = c;
    }

    this.async = true;
    var jsonObj;
    var finished = function (obj) {
        return function () {
            if (xmlHttp.readyState == 4) {

                if (xmlHttp.status == 200  || xmlHttp.status == 0) {
                    //服务器必须Response.ContentType = "text/xml";否则responseXML is null
                    if (xmlHttp.responseText.indexOf("{") == 0 || xmlHttp.responseText.indexOf("[") == 0) {
                        //json对象
                        if (completedHandler != null) {
                            eval("jsonObj=" + xmlHttp.responseText);
                            completedHandler(jsonObj, xmlHttp.responseText, false, intance , lasatname , lastargs);
                        }
                        
                    }
                    else {
                        var doc = xmlHttp.responseXML;
                        if (completedHandler != null) {
                            completedHandler(doc, xmlHttp.responseText, false, intance, lasatname, lastargs);
                        }
                    }

                }
                else {
                    if (completedHandler != null) {
                        completedHandler(null, xmlHttp.responseText, "与服务器通讯中断", intance, lasatname, lastargs);
                    }
                }

            }
        }
    }

    var aborted = false;
    function _about() {
        aborted = true;
        xmlHttp.abort();
    }

    function _escape(s) {
        if (s == "")
            return s;
        return encodeURIComponent(s);
    }

    var xmlHttp = null;
    this.invokeArgs = function (names, args) {
        aborted = false;
        lasatname = names;
        lastargs = args;

        var p = "";
        for (var i = 0; i < args.length; i++) {
            p += names[i] + "=" + (args[i] == null ? "" : _escape(args[i]));
            if (i < args.length - 1)
                p += "&";
        }

        if (xmlHttp == null)
            xmlHttp = createXMLHttp(); //delete xmlHttp;


        //xmlHttp.__completed = this.completed;
        xmlHttp.onreadystatechange = finished(this);
        var url = URL ? URL : location.href;
        //if (url.indexOf("?") > 0)
        //    url += "&";
        //else
        //    url += "?";

        //url += "post=1";
        var flag = url.indexOf("#");
        if (flag > 0) {
            url = url.substr(0, flag);
        }
        xmlHttp.open("POST", url, this.async);
        xmlHttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        xmlHttp.send(p); //null,对ff浏览器是必须的

    }

}