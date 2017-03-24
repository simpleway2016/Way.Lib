(function () {
    window.lowAndroidCustomScrolls = [];
    var lastClickEvent = null;
    var lastLongTouchEvent = null;
    var LONGCLICKACTIVETIME = 600;//长按触发时间
    var CLICKACTIVETIME = 300;//click点击有效按下时间
    var androidVersion = 5;
    if (navigator.userAgent) {
        var userAgent = navigator.userAgent;
        var index = userAgent.indexOf("Android")
        if (index >= 0) {
            androidVersion = parseFloat(userAgent.slice(index + 8));
        }
    }

    function simulateClick(el) {
        el.focus();
        if (document.createEvent) {
            var theEvent = lastClickEvent;//使用上一个event对象，这样，才能让e.stopPropagation()有作用
            if (!theEvent) {
                theEvent = document.createEvent('MouseEvents');
                theEvent.initEvent('click', true, true);
                lastClickEvent = theEvent;
            }
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

    function setTouchForScroll(element) {
        if (element._setedTouchForScroll || element.style.overflow == "hidden" || element.style.overflowY == "hidden")
            return;
        element._setedTouchForScroll = true;

        var touchPoint;
        element.addEventListener("touchstart", function (e) {
            e.stopPropagation();
            touchPoint = {
                x: e.touches[0].clientX,
                y: e.touches[0].clientY,
                scrollTop: element.scrollTop,
            };
        });
        element.addEventListener("touchmove", function (e) {

            if (touchPoint) {
                e.stopPropagation();
                var y = e.touches[0].clientY;
                var scrolltop = touchPoint.y - y + touchPoint.scrollTop;
                if (screenTop < 0)
                    screenTop = 0;
                element.scrollTop = scrolltop;
            }
        });
        element.addEventListener("touchend", function (e) {
            e.stopPropagation();
            touchPoint = null;
        });
    }

    function TouchHandler(element) {
        if (!("ontouchstart" in element))
            return;

        var maxMoveDistance = Math.max(window.innerWidth, window.innerHeight) / 10;
        
        var modeclass = element.getAttribute("touchmode");
        var modeclassElement = element;
        if (!modeclass || modeclass.length == 0)
            modeclass = null;
        if (modeclass) {
            if (modeclass.indexOf("[") == 0) {
                var expression = modeclass.substr(1, modeclass.indexOf("]") - 1);
                modeclass = modeclass.substr(modeclass.indexOf("]") + 1);
                modeclassElement = eval(expression.replace("{0}", "element"));
            }
        }
        var touchPoint;
        var timeoutflag;

        if (androidVersion < 5 && window.lowAndroidCustomScrolls.length > 0) {
            //如果android版本小于5，必须禁止它的滚动功能，否则1px的滚动都会导致touchend事件无法触发
            //查找上级element是否需要滚动，然后用touch帮它实现滚动
            var parentEle = element.parentElement;
            while (true) {
                if (!parentEle || parentEle.tagName == "BODY")
                    break;
                var found = false;
                for (var i = 0 ; i < window.lowAndroidCustomScrolls.length ; i++) {
                    if (window.lowAndroidCustomScrolls[i] == parentEle) {
                        element._shouldPreventDefaultOnTouchStart = true;
                        setTouchForScroll(parentEle);
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
                parentEle = parentEle.parentElement;

            }
        }

        var longTouchAttr = element.getAttribute("onlongtouch");
        if (longTouchAttr && longTouchAttr.length > 0) {
            element.addEventListener("longtouch", function (e) {
                eval(longTouchAttr);
            }, false);
        }
        else {
            longTouchAttr = null;
        }

        element.addEventListener("touchstart", function (e) {
            lastClickEvent = null;
            if (element._shouldPreventDefaultOnTouchStart) {
                e.preventDefault();
            }
            if (modeclass) {
                addCls(modeclassElement, modeclass);
            }
            touchPoint = {
                x: e.touches[0].clientX,
                y: e.touches[0].clientY,
                time: new Date().getTime()
            };

            
            if (longTouchAttr) {
                timeoutflag = setTimeout(function () {
                    timeoutflag = null;
                    if (touchPoint) {
                        if (modeclass) {
                            removeCls(modeclassElement, modeclass);
                        }
                        touchPoint = null;
                        
                        var theEvent = lastLongTouchEvent;
                        if (!theEvent) {
                            theEvent = document.createEvent('MouseEvents');
                            theEvent.initEvent('longtouch', true, true);
                            lastLongTouchEvent = theEvent;
                        }
                        element.dispatchEvent(theEvent);
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

                var x = e.touches[0].clientX;
                var y = e.touches[0].clientY;
                if (Math.abs(x - touchPoint.x) > maxMoveDistance || Math.abs(y - touchPoint.y) > maxMoveDistance) {
                    if (modeclass) {
                        removeCls(modeclassElement, modeclass);
                    }
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
                removeCls(modeclassElement, modeclass);
            }

            if (touchPoint) {
                if ((new Date().getTime() - touchPoint.time) < CLICKACTIVETIME) {
                    e.preventDefault();
                    simulateClick(element);
                }
                touchPoint = null;
            }
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


    if (document.addEventListener && "ontouchstart" in document.documentElement) {
        document.addEventListener('DOMContentLoaded', parseTouchHandler, false);
    }
})();
