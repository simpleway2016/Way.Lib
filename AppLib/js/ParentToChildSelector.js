function JS_ParentToChildSelectorByDataSource(clientid, dataSourceName, defaultValue) {
    var instance = this;
    var containerDIV = document.getElementById(clientid + "_div");
    containerDIV.style.zIndex = 999999;
    var textObj = document.getElementsByName(clientid + "_text")[0];
    var textObj_jq = $(textObj);
    var dataSource = false;

    this.getValue = function()
    {
        var selects = document.getElementsByName(clientid);
        var value = "";
        for (var i = 0 ; i < selects.length ; i++) {
            if (selects[i].value == "")
                break;
            if (value.length > 0)
                value += ",";
            value += selects[i].value;
        }
        return value;
    }
   
    function window_load()
    {
        if (!dataSource)
            eval("dataSource=" + dataSourceName + ";");

        if (defaultValue && defaultValue.length > 0) {
            var text = "";
            var datas = dataSource;
            for (var i = 0 ; i < defaultValue.length ; i++) {
                var finded = false;
                for (var j = 0 ; j < datas.length ; j++) {
                    if (datas[j].Value == defaultValue[i]) {
                        
                        finded = true;
                        if (text.length > 0)
                            text += "/";
                        text += datas[j].Text;
                        datas = datas[j].Items;
                        break;
                    }
                }
                if (!finded)
                    break;
            } 
            textObj.value = text;
        }
    }
    if (window.attachEvent) {
        window.attachEvent("onload", window_load);
    }
    else {
        window.addEventListener("load", window_load, false);
    }

    containerDIV.onclick = function (event) {
        var _event = event ? event : window.event;
        _event.cancelBubble = true;
        if (_event.stopPropagation) {
            _event.stopPropagation();
        }
    }


    if (!window._way_pop_divObjs) {
        window._way_pop_divObjs = [];

        function windowclick() {
            for (var i = 0 ; i < window._way_pop_divObjs.length ; i++) {
                window._way_pop_divObjs[i].style.display = "none";
            }
        }
        if (document.attachEvent) {
            document.attachEvent("onclick", windowclick);
        }
        else {
            document.addEventListener("click", windowclick, false);
        }
    }
    window._way_pop_divObjs.push(containerDIV);
    //隐藏其他控件的popup
    function hideExceptMe() {
        for (var i = 0 ; i < window._way_pop_divObjs.length ; i++) {
            if (window._way_pop_divObjs[i] != containerDIV)
                window._way_pop_divObjs[i].style.display = "none";
        }
    }

    function textonclick(event) {
        hideExceptMe();
        var _event = event ? event : window.event;
        _event.cancelBubble = true;
        if (_event.stopPropagation) {
            _event.stopPropagation();
        }
        if (containerDIV.style.display != "") {
            ShowData();
        }
        instance.resetLocation();
    }
    if (textObj.attachEvent) {
        textObj.attachEvent("onclick", textonclick);
    }
    else {
        textObj.addEventListener("click", textonclick, false);
    }

    function addSelect(datas, layernumber, value) {
        if (!datas)
            return false;

        while (containerDIV.children.length >= layernumber)
            containerDIV.removeChild(containerDIV.children[containerDIV.children.length - 1]);

        if (datas.length == 0)
            return false;
        var select = document.createElement("SELECT");
        select.size = 12;
        select._layernumber = layernumber;
        select.name = clientid;
        containerDIV.appendChild(select);

        for (var i = 0 ; i < datas.length ; i++) {
            var option = document.createElement("OPTION");
            var dataitem = datas[i];
            if (dataitem.DisplayText != null && dataitem.DisplayText.length > 0)
                option.innerHTML = dataitem.DisplayText;
            else
                option.innerHTML = dataitem.Text;
            option.value = dataitem.Value;
            option._data = dataitem;
            select.appendChild(option);
        }
        var result = false;
        if (value) {
            select.value = value;
            result = select.children[select.selectedIndex]._data.Items;
        }

        select.onchange = select_change;
        return result;
    }

    function select_change(event) {
        var _event = event ? event : window.event;
        var srcObj;
        if (_event.target) {
            srcObj = _event.target;
        }
        else {
            srcObj = _event.srcElement;
        }
        var option = srcObj.children[srcObj.selectedIndex];
        addSelect(option._data.Items, srcObj._layernumber + 1);
  

        var text = "";
        defaultValue = [];
        for (var i = 0 ; i < containerDIV.children.length ; i++) {
            if (containerDIV.children[i].selectedIndex < 0)
                break;
            if (text.length > 0)
                text += "/";
            text += containerDIV.children[i].children[containerDIV.children[i].selectedIndex]._data.Text;
            defaultValue.push(containerDIV.children[i].value);
        }
        textObj.value = text;
    }

    function showDefault() {
        var datas = dataSource;
        for (var i = 0 ; i < defaultValue.length ; i++) {
            datas = addSelect(datas, i + 1, defaultValue[i]);
        }

        if (containerDIV.children.length > 0) {
            srcObj = containerDIV.children[containerDIV.children.length - 1];
            if (srcObj.selectedIndex >= 0) {
                var option = srcObj.children[srcObj.selectedIndex];
                addSelect(option._data.Items, srcObj._layernumber + 1);
            }
        }
    }

    function ShowData() {
        try{
            if (!dataSource)
                eval("dataSource=" + dataSourceName + ";");
            containerDIV.innerHTML = "";
            if (defaultValue && defaultValue.length > 0) {
                showDefault();
            }
            else {
                addSelect(dataSource, 1);
            }
            containerDIV.style.display = "";
        }
        catch (e) {
            alert(e.message);
        }
    }


    this.resetLocation = function () {
        if (containerDIV.parentElement != document.forms[0]) {
            //把containerDIV移到document.forms[0]下面,否则absolute不能保证正确定位
            containerDIV.parentElement.removeChild(containerDIV);
            document.forms[0].appendChild(containerDIV);
        }

        containerDIV.style.display = "";
        var windowHeight = document.documentElement.clientHeight;
        var height = containerDIV.offsetHeight;
        containerDIV.style.left =textObj_jq.offset().left + "px";
        var top = textObj_jq.offset().top + (textObj.offsetHeight + 2);
        if (top - $(document).scrollTop() + height > windowHeight) {
            top = textObj_jq.offset().top - height - 2;
        }
        if (top < 0)
            top = 0;
        containerDIV.style.top = top + "px";

    }




}

function JS_ParentToChildSelector(clientid) {
    var instance = this;
    var iframeObj = document.getElementsByName(clientid + "_iframe")[0];
    var containerDIV = document.getElementById(clientid + "_div");
    containerDIV.style.zIndex = 999999;
    var textObj = document.getElementsByName(clientid + "_text")[0];
    var textObj_jq = $(textObj);
    var loading = false;
    var gettingData = false;

    this.getValue = function () {
        var selects = document.getElementsByName(clientid);
        var value = "";
        for (var i = 0 ; i < selects.length ; i++) {
            if (selects[i].value == "")
                break;
            if (value.length > 0)
                value += ",";
            value += selects[i].value;
        }
        return value;
    }

    containerDIV.onclick = function (event) {
        
        var _event = event ? event : window.event;
        _event.cancelBubble = true;
        if (_event.stopPropagation) {
            _event.stopPropagation();
        }
    }
   

    if (!window._way_pop_divObjs) {
        window._way_pop_divObjs = [];

        function windowclick() {
            for (var i = 0 ; i < window._way_pop_divObjs.length ; i++) {
                window._way_pop_divObjs[i].style.display = "none";
            }
        }
        if (document.attachEvent) {
            document.attachEvent("onclick", windowclick);
        }
        else {
            document.addEventListener("click", windowclick, false);
        }
    }
    window._way_pop_divObjs.push(containerDIV);
    //隐藏其他控件的popup
    function hideExceptMe() {
        for (var i = 0 ; i < window._way_pop_divObjs.length ; i++) {
            if (window._way_pop_divObjs[i] != containerDIV)
                window._way_pop_divObjs[i].style.display = "none";
        }
    }

    function textonclick(event) {
        hideExceptMe();
        var _event = event ? event : window.event;
        _event.cancelBubble = true;
        if (_event.stopPropagation) {
            _event.stopPropagation();
        }

        instance.resetLocation();
    }
    if (textObj.attachEvent) {
        textObj.attachEvent("onclick", textonclick);
    }
    else {
        textObj.addEventListener("click", textonclick, false);
    }


    this.resetLocation = function () {
        if (containerDIV.parentElement != document.forms[0]) {
            //把containerDIV移到document.forms[0]下面,否则absolute不能保证正确定位
            containerDIV.parentElement.removeChild(containerDIV);
            document.forms[0].appendChild(containerDIV);
        }

        containerDIV.style.display = "";
        var windowHeight = document.documentElement.clientHeight;
        var height = containerDIV.offsetHeight;
        containerDIV.style.left = textObj_jq.offset().left + "px";
        var top = textObj_jq.offset().top + (textObj.offsetHeight + 2);


        if (top - $(document).scrollTop() + height > windowHeight) {
            top = textObj_jq.offset().top - height - 2;
        }
        if (top < 0)
            top = 0;
        containerDIV.style.top = top + "px";

    }

    function createLoadingObject() {
        if (loading)
            return;
        loading = document.createElement("IMG");
        loading.src = "/inc/imgs/loading.gif";
        loading.style.position = "absolute";
        loading.style.display = "none";
        loading.style.zIndex = "9999990";
        document.body.appendChild(loading);
    }

    function _showLoading() {

        if (gettingData) {
            loading.style.display = "";
            loading.style.left = textObj_jq.offset().left + "px";
            loading.style.top = (textObj_jq.offset().top - loading.offsetHeight - 2) + "px";
        }
    }
    function showLoading() {
        gettingData = true;
        createLoadingObject();
        setTimeout(_showLoading, 500);

    }

    this.select_Change = function (event) {
        try {
            var _event = event ? event : window.event;
            var srcObj;
            if (_event.target) {
                srcObj = _event.target;
            }
            else {
                srcObj = _event.srcElement;
            }
            var showlayer = parseInt(srcObj.getAttribute("_layer"));
            var src = containerDIV.getAttribute("_src");
            var selects = document.getElementsByName(clientid);
            var allids = "";
            for (var i = 0 ; i < selects.length ; i++) {
                allids += selects[i].value + ",";
            }
            src += "&allid=" + allids + "&layer=" + showlayer;
            while (containerDIV.children.length > showlayer) {
                containerDIV.removeChild(containerDIV.children[containerDIV.children.length - 1]);
            }
            showLoading();
            iframeObj.contentWindow.location.href = src;
        }
        catch (e) {
            alert(e.message);
        }
    }

    this.loadData = function () {
        gettingData = false;
        var loadedDiv = iframeObj.contentWindow.document.getElementById("div1");
        var tempDiv = document.createElement("DIV");
        tempDiv.innerHTML = loadedDiv.innerHTML;
        while (tempDiv.children.length > 0) {
            var child = tempDiv.children[0];
            tempDiv.removeChild(child);
            containerDIV.appendChild(child);
            child.onchange = instance.select_Change;
        }
        if (loading)
            loading.style.display = "none";

        var selects = document.getElementsByName(clientid);
        var text = "";
        for (var i = 0 ; i < selects.length ; i++) {
            if (selects[i].selectedIndex < 0)
                break;
            text += selects[i].options[selects[i].selectedIndex].text + "/";
        }
        if (text.length > 0) {
            textObj.value = text.substr(0, text.length - 1);
        }
    }

}