window.onerror = function (errorMessage, scriptURI, lineNumber) {
    alert(errorMessage + "\r\nuri:" + scriptURI + "\r\nline:" + lineNumber);
};
if (true) {
    try {
        var obj = {};
        Object.defineProperty(obj, "test", {
            get: function () {
                return null;
            },
            set: function (value) {
            },
            enumerable: true,
            configurable: true
        });
    }
    catch (e) {
        throw "浏览器不支持defineProperty";
    }
}
var AllJBinders = [];
var JElementHelper = (function () {
    function JElementHelper() {
    }
    JElementHelper.replaceElement = function (source, dst) {
        if (dst == dst.parentElement.children[dst.parentElement.children.length - 1]) {
            var parent = dst.parentElement;
            parent.removeChild(dst);
            parent.appendChild(source);
        }
        else {
            var nextlib = dst.nextElementSibling;
            var parent = dst.parentElement;
            parent.removeChild(dst);
            parent.insertBefore(source, nextlib);
        }
    };
    JElementHelper.getControlTypeName = function (tagname) {
        if (tagname == "OPTION")
            return null;
        for (var name in window) {
            if (name.toUpperCase() == tagname) {
                return name;
            }
        }
        return null;
    };
    JElementHelper.getElement = function (html) {
        var div = document.createElement("DIV");
        div.innerHTML = html;
        return div.children[0];
    };
    JElementHelper.initElements = function (container, bind) {
        if (!container || !container.children)
            return;
        var classType = JElementHelper.getControlTypeName(container.tagName);
        if (classType) {
            eval("new " + classType + "(container)");
            return;
        }
        for (var i = 0; i < container.children.length; i++) {
            JElementHelper.initElements(container.children[i], bind);
        }
        if (bind) {
            if (container.JControl) {
                var jcontrol = container.JControl;
                if (jcontrol.datacontext) {
                    new JDatacontextBinder(container);
                    new JDatacontextExpressionBinder(container);
                }
                new JControlBinder(container);
                new JControlExpressionBinder(container);
            }
            else {
                var parent = container.parentElement;
                var jcontrol;
                while (parent) {
                    if (parent.JControl) {
                        jcontrol = parent.JControl;
                        break;
                    }
                    else {
                        parent = parent.parentElement;
                    }
                }
                if (jcontrol) {
                    if (jcontrol.datacontext) {
                        new JDatacontextBinder(container);
                        new JDatacontextExpressionBinder(container);
                    }
                    new JControlBinder(container);
                    new JControlExpressionBinder(container);
                }
            }
        }
        else {
            if (container.JControl) {
                var jcontrol = container.JControl;
                jcontrol.resetParentJControl();
            }
        }
    };
    return JElementHelper;
}());
if (document.addEventListener) {
    function removeElement(element) {
        if (!element.children)
            return;
        if (element.JControl) {
            element.JControl.dispose();
        }
        for (var i = 0; i < element.children.length; i++) {
            removeElement(element.children[i]);
        }
    }
    document.addEventListener('DOMContentLoaded', function () {
        var bodytemplate = document.body.getAttribute("template");
        if (!bodytemplate || bodytemplate.length == 0) {
            bodytemplate = "/templates/system.html";
        }
        var templateHtml = JHttpHelper.downloadUrl(bodytemplate);
        JElementHelper.SystemTemplateContainer = document.createElement("DIV");
        JElementHelper.SystemTemplateContainer.innerHTML = templateHtml;
        var style = JElementHelper.SystemTemplateContainer.querySelector("style");
        if (style) {
            document.head.appendChild(style);
        }
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
                var callback = function (records) {
                    records.map(function (record) {
                        if (record.addedNodes) {
                            for (var i = 0; i < record.addedNodes.length; i++) {
                                JElementHelper.initElements(record.addedNodes[i], true);
                            }
                        }
                        if (record.removedNodes) {
                            for (var i = 0; i < record.removedNodes.length; i++) {
                                removeElement(record.removedNodes[i]);
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
        JElementHelper.initElements(document.body, false);
    }, false);
}
//# sourceMappingURL=JInit.js.map