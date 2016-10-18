
function JTextBoxList(TextLengthToSelect , clientID, jsdataSouceClientId, pagesize) {
           var instance = this;
           var postbackScript = false;
          
           var textObj = document.getElementById(clientID);
           textObj.setAttribute("autocomplete", "off");
           var textObj_jq = $("#" + clientID);
           var disabled = textObj_jq.attr("disabled");
           var readonly = textObj_jq.attr("readonly");
           textObj_jq.attr("disabled", false);
           if (disabled) {
               textObj_jq.attr("readonly", true);
           }

            var iframeObj = false;
           try{
               iframeObj = document.getElementsByName(clientID + "_iframe")[0];
           }
           catch(e){
           }
           var divObj = document.getElementById(clientID + "_div");
           divObj.parentElement.removeChild(divObj);
           document.body.appendChild(divObj);

            var dataSource = false;
            var dataSourcePageIndex = 0;
            var formObj = textObj.parentElement;
            while (formObj.tagName != "FORM") {
                formObj = formObj.parentElement;
            }
            
            var loading = false;
            this.valueChanged = false;
            this.setPostBack = function (script)
            {
                postbackScript = script;
            }

            function createLoadingObject() {
                if (loading)
                    return;
                loading = document.createElement("IMG");
                loading.src = "/inc/imgs/loading.gif";
                loading.style.position = "absolute";
                loading.style.display = "none";
                loading.style.zIndex = "99999";
                document.body.appendChild(loading);
            }

            var lastkey;
            var gettingData = false;
            if (divObj) {
                divObj.onclick = function (event) {
                    var _event = event ? event : window.event;
                    _event.cancelBubble = true;
                    if (_event.stopPropagation) {
                        _event.stopPropagation();
                    }
                }
            }

            this.frameLoaded = function () {
                gettingData = false;
                var divitem = iframeObj.contentWindow.document.body.children[0];
                iframeObj.contentWindow.document.body.removeChild(divitem);
                //divObj.innerHTML = (iframeObj.contentWindow.document.body.children[0].innerHTML);
                while (divObj.children.length > 0)
                    divObj.removeChild(divObj.children[0]);
                divObj.appendChild(divitem);
                instance.resetLocation();
                if (loading) {
                    loading.style.display = "none";
                }
            }

            function _showLoading(){
                if(gettingData)
                {
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

            this.go = function (pageindex) {
                //lastkey = textObj.value;
                showLoading();
                iframeObj.contentWindow.location.href = divObj.getAttribute("_src") + "&p=" + pageindex + "&key=" + escape(lastkey);
            }

            function submitForm() {
                
                if (formObj.fireEvent) {
                    formObj.fireEvent("onsubmit");
                    formObj.submit();
                }
                else {
                    var evt = document.createEvent('HTMLEvents');
                    evt.initEvent('submit', true, true);
                    formObj.dispatchEvent(evt);
                    formObj.submit();
                }
            }
            this.getText = function () {
                return textObj.value;
            }
            this.setValue = function (v) {
                if (textObj.value != v) {
                    textObj.value = v;
                    if (instance.valueChanged) {
                        instance.valueChanged(v);
                    }
                    
                }
                if (postbackScript) {
                    eval(postbackScript);
                }
                divObj.style.display = "none";
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
            window._way_pop_divObjs.push(divObj);
           //隐藏其他控件的popup
            function hideExceptMe() {
                for (var i = 0 ; i < window._way_pop_divObjs.length ; i++) {
                    if (window._way_pop_divObjs[i] != divObj)
                    window._way_pop_divObjs[i].style.display = "none";
                }
            }



            this.resetLocation = function () {
                if (divObj.parentElement != document.body) {
                    //把divObj移到body下面,否则absolute不能保证正确定位
                    divObj.parentElement.removeChild(divObj);
                    document.body.appendChild(divObj);
                }
                    divObj.style.display = "";
                    var windowHeight = document.documentElement.clientHeight;
                    var height = divObj.offsetHeight;
                    divObj.style.left = textObj_jq.offset().left + "px";
                    var top = textObj_jq.offset().top + (textObj.offsetHeight + 2);
                    if ( top - $(document).scrollTop() + height > windowHeight) {
                        top = textObj_jq.offset().top - height - 2;
                    }
                    if (top < 0)
                        top = 0;
                    divObj.style.top = top + "px";
                    
            }

            function nextPageDataSouce() {
                dataSourcePageIndex++;
                showDataSource();
            }
            function prePageDataSouce() {
                dataSourcePageIndex--;
                showDataSource();
            }
            function dataClick(event) {
                var _event = event ? event : window.event;
                var srcObj;
                if (_event.target) {
                    srcObj = _event.target;
                }
                else {
                    srcObj = _event.srcElement;
                }
                instance.setValue(srcObj._data.Text);
            }
            function showDataSource() {
                divObj.innerHTML = "";

                var count = 0;
                var souce;
                if (lastkey.length > 0) {
                    souce = [];
                    for (var i = 0 ; i < dataSource.length ; i++) {
                        if (dataSource[i].Text.indexOf(lastkey) >= 0) {
                            souce.push(dataSource[i]);
                            count++;
                        }
                    }
                }
                else {
                    souce = dataSource;
                    count = dataSource.length;
                }

                var index;
                var startindex = pagesize*dataSourcePageIndex;
                for (var i = 0 ; i < pagesize ; i++) {
                    index = startindex + i;
                    if (index < count) {
                        var dataitem = souce[index];
                        var div = document.createElement("DIV");
                        div.className = "_textBoxList_div1";
                        div.onclick = dataClick;
                        if (dataitem.DisplayText != null && dataitem.DisplayText.length > 0)
                            div.innerHTML = dataitem.DisplayText;
                        else
                            div.innerHTML = dataitem.Text;
                        div._data = dataitem;
                        if (dataitem.Description != null)
                        div.title = dataitem.Description;
                        divObj.appendChild(div);
                    }
                }

                if (dataSourcePageIndex > 0 || index < count) {
                    var div = document.createElement("DIV");
                    div.className = "_textBoxList_div1";
                    if (dataSourcePageIndex > 0) {
                        var divbtn = document.createElement("DIV");
                        divbtn.className = "_textBoxList_btn";
                        divbtn.innerHTML = "上一页";
                        divbtn.onclick = prePageDataSouce;
                        div.appendChild(divbtn);
                    }
                    else {
                        var divbtn = document.createElement("DIV");
                        divbtn.className = "_textBoxList_btn2";
                        divbtn.innerHTML = "上一页";
                        div.appendChild(divbtn);
                    }

                    if (index < dataSource.length) {
                        var divbtn = document.createElement("DIV");
                        divbtn.className = "_textBoxList_btn";
                        divbtn.innerHTML = "下一页";
                        divbtn.onclick = nextPageDataSouce;
                        div.appendChild(divbtn);
                    }
                    else {
                        var divbtn = document.createElement("DIV");
                        divbtn.className = "_textBoxList_btn2";
                        divbtn.innerHTML = "下一页";
                        div.appendChild(divbtn);
                    }
                    divObj.appendChild(div);
                }
            }

            if (!disabled && !readonly)
            textObj.onclick = function (event) {
                if (textObj.value.length < TextLengthToSelect)
                    return;

                try{
                    if (jsdataSouceClientId && !dataSource)
                        eval("dataSource=" + jsdataSouceClientId + ";");

                }
                catch (e) {
                    alert(e.message);
                }

                try {
                    hideExceptMe();
                    var _event = event ? event : window.event;
                    _event.cancelBubble = true;
                    if (_event.stopPropagation) {
                        _event.stopPropagation();
                    }

                    if (divObj.style.display != "") {
                        //lastkey = textObj.value;
                        if (TextLengthToSelect == 0)
                            lastkey = "";
                        else
                            lastkey = textObj.value;
                        if (dataSource) {
                            showDataSource();
                        }
                        else {
                            showLoading();
                            iframeObj.contentWindow.location.href = divObj.getAttribute("_src") + "&p=0&key=" + escape(lastkey);
                        }
                        $(divObj).outerWidth(textObj_jq.outerWidth());
                        divObj.style.display = "";
                        if (dataSource) {
                            instance.resetLocation();
                        }
                    }
                }
                catch (e) {
                    alert("页面没有加载完毕，请稍后再试！" + e.message);
                }
            }

            if (!disabled && !readonly)
                textObj.onkeydown = function (ev) {
                    ev = ev || event;

                    if (ev.keyCode == 13) {
                        ev.cancelBubble = true;
                        if (ev.stopPropagation) {
                            ev.stopPropagation();
                        }
                        return false;
                    }
                    
                }
                textObj.onkeyup = function (ev) {
                    ev = ev || event;
                   
                    if (textObj.value.length < TextLengthToSelect) {
                        divObj.style.display = "none";
                        return;
                    }

                try {
                    if (ev.keyCode == 13) {
                        ev.cancelBubble = true;
                        if (ev.stopPropagation) {
                            ev.stopPropagation();
                        }

                        lastkey = textObj.value;
                        if (dataSource) {
                            dataSourcePageIndex = 0;
                            showDataSource();
                        }
                        else {
                            showLoading();
                            iframeObj.contentWindow.location.href = divObj.getAttribute("_src") + "&p=0&key=" + escape(lastkey);
                        }
                        if (divObj.style.display != "") {
                            divObj.style.display = "";
                        }

                        if (dataSource) {
                            instance.resetLocation();
                        }
                        return false;
                    }

                }
                catch (e) {
                    alert("页面没有加载完毕，请稍后再试！");
                }
            }
        }