
window.onerror = (errorMessage, scriptURI, lineNumber) => {
    alert(errorMessage + "\r\nuri:" + scriptURI + "\r\nline:" + lineNumber);
}

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

var AllJBinders: JBinder[] = [];


class JElementHelper {
    static SystemTemplateContainer: HTMLElement;

    //把dst换成source
    static replaceElement(source: HTMLElement, dst: HTMLElement) {
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
    }

    static getControlTypeName(tagname: string): string {
        if (tagname == "OPTION")
            return null;
        for (var name in window) {
            if (name.toUpperCase() == tagname) {
                return name;
            }
        }
        return null;
    }

    static getElement(html: string): HTMLElement {
        var div = document.createElement("DIV");
        div.innerHTML = html;
        return <HTMLElement>div.children[0];
    }


    static initElements(container: HTMLElement, bind: boolean) {
        if (!container || !container.children)//防止#text
            return;

        var classType = JElementHelper.getControlTypeName(container.tagName);
        if (classType) {
            eval("new " + classType + "(container)");
            return;
        }

        for (var i = 0; i < container.children.length; i++) {
            //子元素不用绑定，binder自动绑定子元素
            JElementHelper.initElements(<HTMLElement>container.children[i], bind);
        }

        if (bind)
        {
            //如果是htmlelement
            if ((<any>container).JControl) {
                var jcontrol = <JControl>(<any>container).JControl;
                if (jcontrol.datacontext) {
                   
                    new JDatacontextBinder(container);
                    new JDatacontextExpressionBinder(container);
                }
                new JControlBinder(container);
                new JControlExpressionBinder(container);
            }
            else {
                //查找parent
                var parent = container.parentElement;
                var jcontrol: JControl;
                while (parent) {
                    if ((<any>parent).JControl) {
                        jcontrol = <JControl>(<any>parent).JControl;
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
            if ((<any>container).JControl)
            {
                var jcontrol = <JControl>(<any>container).JControl;
                jcontrol.resetParentJControl();
            }
        }
    }
}


if (document.addEventListener) {
    function removeElement(element) {
        if (!element.children)
            return;//可能是#text

        if (element.JControl)
        {
            (<JControl>element.JControl).dispose();
            
        }

        for (var i = 0; i < element.children.length; i++) {
            removeElement(element.children[i]);
        }
    }

    document.addEventListener('DOMContentLoaded', function () {
        var windowController = <any>document.body.getAttribute("controller");
        if (windowController && windowController.length > 0) {
            (<any>window).controller = windowController.controller();
        }


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

      

        //监视document.body子元素变动事件，新加入的element，转换JControl，这个监听是个异步事件模式
        var MutationObserver = (<any>window).MutationObserver ||
            (<any>window).WebKitMutationObserver ||
            (<any>window).MozMutationObserver;

        if (MutationObserver) {
            try {
                var options = {
                    'childList': true,
                    subtree: true,
                };
                var callback = function (records) {//MutationRecord
                    records.map(function (record) {
                        if (record.addedNodes) {
                            for (var i = 0; i < record.addedNodes.length; i++) {
                                //转换JControl
                                JElementHelper.initElements(record.addedNodes[i], true);
                            }
                        }

                        if (record.removedNodes) {
                            for (var i = 0; i < record.removedNodes.length; i++) {
                                //dispose相关JControl
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
        else {
            //throw "浏览器不支持MutationObserver";

            function nodeAddedCallback(e) {
                //转换JControl
                JElementHelper.initElements(e.target, true);
            }
            function nodeRemovedCallback(e) {
                //dispose相关JControl
                removeElement(e.target);
            }
            document.body.addEventListener("DOMNodeInserted", nodeAddedCallback, false);
            document.body.addEventListener("DOMNodeRemoved", nodeRemovedCallback, false);
        }


        JElementHelper.initElements(document.body, false);
        

    }, false);
}