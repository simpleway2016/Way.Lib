function PicDropDownList(clientid, cssChild, cssBorder, cssSelected, cssMouseOver, _items, selectedValue, uniqueID, postEval) {
    var mainDiv = $("#" + clientid + "_m");
    var instance = this;
    var valueHidden = document.getElementById(clientid);
    this.value = selectedValue;
    var items = _items;

    this.onchange = false;

    var divBorder = document.createElement("DIV");
    var divBorder_jq = $(divBorder);
    divBorder.className = cssBorder;
    divBorder_jq.css({ "position": "absolute", "z-index": 999 });
    divBorder_jq.hide();
    document.body.appendChild(divBorder);

    var lastSelectItem_jq;
    var lastValue;
    this.setValue = function (v) {
        if (v == lastValue)
            return false;

        if (lastSelectItem_jq) {
            lastSelectItem_jq[0].className = cssChild;
            lastSelectItem_jq.css({ "background-color": "", "color": "" });
        }
        lastSelectItem_jq = false;
        lastValue = false;

        var result = false;
        for (var i = 0 ; i < items.length ; i++) {
            var divItem = divBorder.children[i];
            if (items[i].Value == v) {
                instance.value = v;
                lastValue = v;
                valueHidden.value = v;
                mainDiv.html(items[i].Text);
                lastSelectItem_jq = $(divItem);
                if (cssSelected != "") {
                    divItem.className = cssSelected;
                }
                else {
                    lastSelectItem_jq.css({ "background-color": "blue", "color": "white" });
                }
                if (instance.onchange) {
                    instance.onchange();
                }
                result = true;
                break;
            }
        }
        return result;
    }
    this.clearItems = function () {
        divBorder.innerHTML = "";
        mainDiv.html("");
        instance.value = "";
        items = [];
        lastValue = null;
        lastSelectItem_jq = false;
    }

    function addItem2UI(text, value) {
        var divItem = document.createElement("DIV");
        divItem.innerHTML = text;
        divItem.className = cssChild;
        divItem.setAttribute("_value", value);
        divBorder.appendChild(divItem);

        $(divItem).click(function (event) {
            try {
               
                var target = event.target; 
                while (!target.getAttribute("_value") && target.getAttribute("_value") != "")
                    target = target.parentElement;

                var value = target.getAttribute("_value");

                if (instance.setValue(value)) {
                    if (postEval)
                        eval(postEval);
                }
            }
            catch (e) {
                alert(e.message);
            }
        });

        $(divItem).mouseover(function (event) {
            if (cssMouseOver != "")
                event.target.className = cssMouseOver;
        });

        $(divItem).mouseout(function (event) {
            if (event.target == lastSelectItem_jq[0] && cssSelected != "") {
                event.target.className = cssSelected;
            }
            else
                event.target.className = cssChild;
        });
    }

    this.addItem = function (text, value) {
        items.push({ Text: text, Value: value });
        addItem2UI(text, value);
    }

    for (var i = 0 ; i < items.length ; i++) {
        addItem2UI(items[i].Text , items[i].Value);
    }
    instance.setValue(selectedValue);

    $(document).click(function () {
        divBorder_jq.hide();
        divBorder_jq.css({ "height": "", "overflow-y": "", "overflow-x": "" });
    });

    mainDiv.click(function () {
        var offset = mainDiv.offset();
        //divBorder_jq.outerWidth(mainDiv.outerWidth());
        divBorder_jq.css({ "width": mainDiv.outerWidth() });

        var y = offset.top + mainDiv.outerHeight() + 1;
        var borderheight = divBorder_jq.outerHeight();
       
        if (borderheight > 230) {
            borderheight = 230;
            divBorder_jq.css({ "overflow-y": "auto", "overflow-x": "hide","height":"230px" });
        }
        var winHeight = $(window).height();
        var scrolltop = $(document).scrollTop();
        if (y + borderheight - scrolltop > winHeight) {
            y = offset.top - borderheight - 1;
        }

        divBorder_jq.css({ "left": offset.left, "top": y });
        divBorder_jq.show();
        if (lastSelectItem_jq) {
           
            var c = lastSelectItem_jq.offset().top - divBorder_jq.offset().top;
            if (c > 125) {
                divBorder_jq.scrollTop(divBorder_jq.scrollTop() + c - 125);
            }
            //lastSelectItem_jq[0].scrollIntoView();
        }
        return false;
    });
}