function TouchHandler(element) {
    if (!("ontouchstart" in element))
        return;

    function simulateClick(el) {
        var evt;
        if (document.createEvent) {
            var theEvent = document.createEvent('MouseEvents');
            theEvent.initEvent('click', true, true);
            el.dispatchEvent(theEvent);
        } else if (el.fireEvent) {
            el.fireEvent('onclick');
        }
    }

    var addCls = function (el, cls) {
        if ('classList' in el) {
            el.classList.add(cls);
        } else {
            var preCls = el.className;
            var newCls = preCls + ' ' + cls;
            el.className = newCls;
        }
        return el;
    };
    var removeCls = function (el, cls) {
        if ('classList' in el) {
            el.classList.remove(cls);
        } else {
            var allcls = el.className.split(' ');
            var str = "";
            for (var i = 0 ; i < allcls.length ; i++) {
                if (allcls[i] != cls && allcls[i].length > 0) {
                    str += allcls[i] + " ";
                }
            }
            el.className = str;
        }
        return el;
    };

    var LONGCLICKACTIVETIME = 600;//长按触发时间
    var CLICKACTIVETIME = 300;//click点击有效按下时间
    var modeclass = element.getAttribute("touchmode");
    if (!modeclass || modeclass.length == 0)
        modeclass = null;
    var touchPoint;
    var timeoutflag;

    element.addEventListener("touchstart", function (e) {
        e.preventDefault();
        if (modeclass) {
            addCls(element, modeclass);
        }
        touchPoint = {
            x: e.touches[0].clientX,
            y: e.touches[0].clientY,
            time: new Date().getTime()
        };

        var longClickEvent = element.onlongclick;
        if (!longClickEvent) {
            longClickEvent = element.getAttribute("onlongclick");
            if (longClickEvent && longClickEvent.length == 0)
                longClickEvent = null;
        }
        if (longClickEvent) {
            timeoutflag = setTimeout(function () {
                timeoutflag = null;
                if (touchPoint) {
                    if (modeclass) {
                        removeCls(element, modeclass);
                    }
                    touchPoint = null;
                    if (typeof longClickEvent == "function")
                        longClickEvent();
                    else if (typeof longClickEvent == "string") {
                        eval(longClickEvent);
                    }
                }
            }, LONGCLICKACTIVETIME);
        }
    });

    element.addEventListener("touchmove", function (e) {
        if (timeoutflag) {
            clearTimeout(timeoutflag);
            timeoutflag = null;
        }

        if (touchPoint) {
            if (modeclass) {
                removeCls(element, modeclass);
            }
            var x = e.touches[0].clientX;
            var y = e.touches[0].clientY;
            if (Math.abs(x - touchPoint.x) > window.innerWidth / 15 || Math.abs(y - touchPoint.y) > window.innerHeight / 15) {
                touchPoint = null;
            }
        }
    });

    element.addEventListener("touchend", function (e) {
        if (timeoutflag) {
            clearTimeout(timeoutflag);
            timeoutflag = null;
        }
        if (modeclass) {
            removeCls(element, modeclass);
        }
        if (touchPoint && (new Date().getTime() - touchPoint.time) < CLICKACTIVETIME) {

            simulateClick(element);
        }
        touchPoint = null;
    });
}



function parseTouchHandler() {
    var func = function (container) {
        if (container != document && container.nodeType == 3)//3表示#text类型，不是htmlElement
            return;
        if (container != document && container.getAttribute("touchmode") != null) {
            if (!container._touchModeInited) {
                container._touchModeInited = true;
                TouchHandler(container);
            }
        }
        var eles = container.querySelectorAll("*[touchmode]");
        for (var i = 0 ; i < eles.length ; i++) {
            if (!eles[i]._touchModeInited) {
                eles[i]._touchModeInited = true;
                TouchHandler(eles[i]);
            }
        }
    }

    if (!window._touchHandlerInited) {
        window._touchHandlerInited = true;
        //监视document.body子元素变动事件，新加入的element，如果定义touchmode，则自动TouchHandler(element)
        var MutationObserver = window.MutationObserver ||
        window.WebKitMutationObserver ||
        window.MozMutationObserver;

        var mutationObserverSupport = !!MutationObserver;
        if (mutationObserverSupport) {
            try {
                var options = {
                    'childList': true,
                    subtree: true,
                };
                var callback = function (records) {//MutationRecord
                    records.map(function (record) {
                        for (var i = 0 ; i < record.addedNodes.length ; i++) {
                            if (!record.addedNodes[i]._touchModeInited) {
                                func(record.addedNodes[i]);
                            }
                        }
                    });
                };

                var observer = new MutationObserver(callback);
                observer.observe(document.body, options);

            }
            catch (e) {
                alert(e.message);
            }
        }
    }

    func(document);

}


if (document.addEventListener) {
    document.addEventListener('DOMContentLoaded', parseTouchHandler, false);
    //window.addEventListener("load", parseTouchHandler);
}