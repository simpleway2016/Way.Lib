var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var windowid = new Date().getTime();
window.onerror = function (errorMessage, scriptURI, lineNumber) {
    alert(errorMessage + "\r\nuri:" + scriptURI + "\r\nline:" + lineNumber);
};
var ToolBoxItem = (function () {
    function ToolBoxItem() {
    }
    Object.defineProperty(ToolBoxItem.prototype, "supportMove", {
        get: function () {
            return true;
        },
        enumerable: true,
        configurable: true
    });
    ToolBoxItem.prototype.begin = function (svgContainer, position) {
    };
    ToolBoxItem.prototype.mousemove = function (x, y) {
    };
    ToolBoxItem.prototype.end = function () {
        return null;
    };
    return ToolBoxItem;
}());
var ToolBox_Line = (function (_super) {
    __extends(ToolBox_Line, _super);
    function ToolBox_Line() {
        return _super.call(this) || this;
    }
    ToolBox_Line.prototype.begin = function (svgContainer, position) {
        this.lineElement = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        this.lineElement.setAttribute('x1', position.x);
        this.lineElement.setAttribute('y1', position.y);
        this.lineElement.setAttribute('x2', position.x);
        this.lineElement.setAttribute('y2', position.y);
        this.lineElement.setAttribute('style', 'stroke:#aaaaaa;stroke-width:5;');
        svgContainer.appendChild(this.lineElement);
    };
    ToolBox_Line.prototype.mousemove = function (x, y) {
        this.lineElement.setAttribute('x2', x);
        this.lineElement.setAttribute('y2', y);
    };
    ToolBox_Line.prototype.end = function () {
        return new LineControl(this.lineElement);
    };
    return ToolBox_Line;
}(ToolBoxItem));
var ToolBox_Rect = (function (_super) {
    __extends(ToolBox_Rect, _super);
    function ToolBox_Rect() {
        return _super.call(this) || this;
    }
    ToolBox_Rect.prototype.begin = function (svgContainer, position) {
        this.rectElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
        this.rectElement.setAttribute('x', position.x);
        this.rectElement.setAttribute('y', position.y);
        this.rectElement.setAttribute('width', "0");
        this.rectElement.setAttribute('height', "0");
        this.rectElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;');
        this.startx = position.x;
        this.starty = position.y;
        svgContainer.appendChild(this.rectElement);
    };
    ToolBox_Rect.prototype.mousemove = function (x, y) {
        this.rectElement.setAttribute('x', Math.min(x, this.startx));
        this.rectElement.setAttribute('y', Math.min(y, this.starty));
        this.rectElement.setAttribute('width', Math.abs(x - this.startx));
        this.rectElement.setAttribute('height', Math.abs(y - this.starty));
    };
    ToolBox_Rect.prototype.end = function () {
        if (this.rectElement.getAttribute("width") == 0 || this.rectElement.getAttribute("height") == 0) {
            return null;
        }
        return new RectControl(this.rectElement);
    };
    return ToolBox_Rect;
}(ToolBoxItem));
var ToolBox_Ellipse = (function (_super) {
    __extends(ToolBox_Ellipse, _super);
    function ToolBox_Ellipse() {
        return _super.call(this) || this;
    }
    ToolBox_Ellipse.prototype.begin = function (svgContainer, position) {
        this.rootElement = document.createElementNS('http://www.w3.org/2000/svg', 'ellipse');
        this.rootElement.setAttribute('cx', position.x);
        this.rootElement.setAttribute('cy', position.y);
        this.rootElement.setAttribute('rx', "0");
        this.rootElement.setAttribute('ry', "0");
        this.rootElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;');
        this.startx = position.x;
        this.starty = position.y;
        svgContainer.appendChild(this.rootElement);
    };
    ToolBox_Ellipse.prototype.mousemove = function (x, y) {
        this.rootElement.setAttribute('rx', Math.abs(x - this.startx));
        this.rootElement.setAttribute('ry', Math.abs(y - this.starty));
    };
    ToolBox_Ellipse.prototype.end = function () {
        if (this.rootElement.getAttribute("rx") == 0 || this.rootElement.getAttribute("ry") == 0) {
            return null;
        }
        return new EllipseControl(this.rootElement);
    };
    return ToolBox_Ellipse;
}(ToolBoxItem));
var ToolBox_Circle = (function (_super) {
    __extends(ToolBox_Circle, _super);
    function ToolBox_Circle() {
        return _super.call(this) || this;
    }
    ToolBox_Circle.prototype.begin = function (svgContainer, position) {
        this.rootElement = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
        this.rootElement.setAttribute('cx', position.x);
        this.rootElement.setAttribute('cy', position.y);
        this.rootElement.setAttribute('r', "0");
        this.rootElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;');
        this.startx = position.x;
        this.starty = position.y;
        svgContainer.appendChild(this.rootElement);
    };
    ToolBox_Circle.prototype.mousemove = function (x, y) {
        this.rootElement.setAttribute('r', Math.abs(x - this.startx));
    };
    ToolBox_Circle.prototype.end = function () {
        if (this.rootElement.getAttribute("r") == 0) {
            return null;
        }
        return new CircleControl(this.rootElement);
    };
    return ToolBox_Circle;
}(ToolBoxItem));
var ToolBox_Image = (function (_super) {
    __extends(ToolBox_Image, _super);
    function ToolBox_Image() {
        return _super.call(this) || this;
    }
    Object.defineProperty(ToolBox_Image.prototype, "supportMove", {
        get: function () {
            return false;
        },
        enumerable: true,
        configurable: true
    });
    ToolBox_Image.prototype.begin = function (svgContainer, position) {
        var _this = this;
        fileBrowser.onSelectFile = function (path) {
            fileBrowser.hide();
            _this.rootElement = document.createElementNS('http://www.w3.org/2000/svg', 'image');
            _this.rootElement.setAttribute('x', position.x);
            _this.rootElement.setAttribute('y', position.y);
            _this.rootElement.setAttribute('width', "200");
            _this.rootElement.setAttribute('height', "200");
            _this.rootElement.href.baseVal = path;
            svgContainer.appendChild(_this.rootElement);
            if (_this.buildDone) {
                _this.buildDone(new ImageControl(_this.rootElement));
            }
        };
        fileBrowser.show();
    };
    return ToolBox_Image;
}(ToolBoxItem));
var ToolBox_Text = (function (_super) {
    __extends(ToolBox_Text, _super);
    function ToolBox_Text() {
        return _super.call(this) || this;
    }
    Object.defineProperty(ToolBox_Text.prototype, "supportMove", {
        get: function () {
            return false;
        },
        enumerable: true,
        configurable: true
    });
    ToolBox_Text.prototype.begin = function (svgContainer, position) {
        this.rootElement = document.createElementNS('http://www.w3.org/2000/svg', 'text');
        this.rootElement.setAttribute('x', position.x);
        this.rootElement.setAttribute('id', "aac");
        this.rootElement.setAttribute('y', position.y);
        this.rootElement.textContent = "Text";
        this.rootElement.setAttribute('style', 'fill:#111111;cursor:default;-moz-user-select:none;');
        this.rootElement.setAttribute('font-size', "16");
        svgContainer.appendChild(this.rootElement);
        this.rootElement.onselectstart = function (e) { e.preventDefault(); e.cancelBubble = true; return false; };
        if (this.buildDone) {
            this.buildDone(new TextControl(this.rootElement));
        }
    };
    return ToolBox_Text;
}(ToolBoxItem));
var Editor = (function () {
    function Editor(id) {
        var _this = this;
        this.beginedToolBoxItem = null;
        this.controls = [];
        var divContainer = document.body.querySelector("#" + id);
        this.svgContainer = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        this.svgContainer.setAttribute('width', '100%');
        this.svgContainer.style.backgroundSize = "100% 100%";
        this.svgContainer.style.backgroundRepeat = "no-repeat";
        this.svgContainer.setAttribute('height', '100%');
        this.svgContainer.style.backgroundColor = "#ffffff";
        divContainer.appendChild(this.svgContainer);
        this.svgContainer.addEventListener("click", function (e) {
            if (_this.svgContainer._notClick) {
                _this.svgContainer._notClick = false;
                return;
            }
            _this.svgContainerClick(e);
            _this.fireBodyEvent("click");
        });
        this.svgContainer.addEventListener("mousedown", function (e) {
            _this.fireBodyEvent("mousedown");
            if (!_this.currentToolBoxItem) {
                _this.svgContainer._notClick = true;
                _this.selectingElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
                _this.selectingElement._startx = e.clientX - divContainer.offsetLeft;
                _this.selectingElement._starty = e.clientY - divContainer.offsetTop;
                _this.selectingElement.setAttribute('x', (e.clientX - divContainer.offsetLeft));
                _this.selectingElement.setAttribute('y', (e.clientY - divContainer.offsetTop));
                _this.selectingElement.setAttribute('width', "0");
                _this.selectingElement.setAttribute('height', "0");
                _this.selectingElement.setAttribute('style', 'fill:none;stroke:black;stroke-width:1;stroke-dasharray:2;stroke-dashoffset:2;');
                _this.svgContainer.appendChild(_this.selectingElement);
                _this.svgContainer.setCapture();
            }
        });
        this.svgContainer.addEventListener("mouseup", function (e) {
            if (_this.selectingElement) {
                _this.svgContainer.releaseCapture();
                var rect = {
                    x: parseInt(_this.selectingElement.getAttribute("x")),
                    y: parseInt(_this.selectingElement.getAttribute("y")),
                    width: parseInt(_this.selectingElement.getAttribute("width")),
                    height: parseInt(_this.selectingElement.getAttribute("height")),
                };
                _this.svgContainer.removeChild(_this.selectingElement);
                _this.selectingElement = null;
                _this.selectControlsByRect(rect, e.ctrlKey);
                setTimeout(function () {
                    _this.svgContainer._notClick = false;
                }, 500);
            }
            else {
                _this.svgContainerMouseUpPosition = {
                    x: e.clientX - divContainer.offsetLeft,
                    y: e.clientY - divContainer.offsetTop
                };
            }
        });
        this.svgContainer.addEventListener("mousemove", function (e) {
            if (_this.selectingElement) {
                var w = e.clientX - divContainer.offsetLeft - _this.selectingElement._startx;
                var h = e.clientY - divContainer.offsetTop - _this.selectingElement._starty;
                if (w < 0) {
                    var x = _this.selectingElement._startx + w;
                    w = -w;
                    _this.selectingElement.setAttribute("x", x);
                }
                if (h < 0) {
                    var y = _this.selectingElement._starty + h;
                    h = -h;
                    _this.selectingElement.setAttribute("y", y);
                }
                _this.selectingElement.setAttribute("width", w);
                _this.selectingElement.setAttribute("height", h);
            }
            else {
                _this.svgContainerMouseMove(e.clientX - divContainer.offsetLeft, e.clientY - divContainer.offsetTop);
            }
        });
        document.body.addEventListener("keydown", function (e) {
            if (e.keyCode == 37) {
                for (var i = 0; i < AllSelectedControls.length; i++) {
                    var control = AllSelectedControls[i];
                    var rect = control.rect;
                    rect.x--;
                    control.rect = rect;
                }
            }
            else if (e.keyCode == 38) {
                for (var i = 0; i < AllSelectedControls.length; i++) {
                    var control = AllSelectedControls[i];
                    var rect = control.rect;
                    rect.y--;
                    control.rect = rect;
                }
            }
            else if (e.keyCode == 39) {
                for (var i = 0; i < AllSelectedControls.length; i++) {
                    var control = AllSelectedControls[i];
                    var rect = control.rect;
                    rect.x++;
                    control.rect = rect;
                }
            }
            else if (e.keyCode == 40) {
                for (var i = 0; i < AllSelectedControls.length; i++) {
                    var control = AllSelectedControls[i];
                    var rect = control.rect;
                    rect.y++;
                    control.rect = rect;
                }
            }
        }, false);
    }
    Editor.prototype.copy = function () {
        var copyitems = [];
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var json = control.getJson();
            copyitems.push(json);
        }
        window.localStorage.setItem("copy", JSON.stringify(copyitems));
        window.localStorage.setItem("windowid", windowid + "");
    };
    Editor.prototype.paste = function () {
        var str = window.localStorage.getItem("copy");
        if (str) {
            while (AllSelectedControls.length > 0)
                AllSelectedControls[0].selected = false;
            var isSameWindow = parseInt(window.localStorage.getItem("windowid")) == windowid;
            var copyItems = JSON.parse(str);
            for (var i = 0; i < copyItems.length; i++) {
                var controlJson = copyItems[i];
                if (isSameWindow) {
                    controlJson.rect.x += 10;
                    controlJson.rect.y += 10;
                }
                var editorctrl;
                var ele = document.createElementNS('http://www.w3.org/2000/svg', controlJson.tagName);
                eval("editorctrl = new " + controlJson.constructorName + "(ele)");
                this.svgContainer.appendChild(ele);
                this.controls.push(editorctrl);
                editorctrl.ctrlKey = true;
                editorctrl.selected = true;
                editorctrl.ctrlKey = false;
                for (var pname in controlJson) {
                    if (pname != "tagName" && pname != "constructorName" && pname != "rect") {
                        editorctrl[pname] = controlJson[pname];
                    }
                }
                editorctrl.rect = controlJson.rect;
            }
        }
    };
    Editor.prototype.fireBodyEvent = function (event) {
        var evt = document.createEvent('HTMLEvents');
        evt.initEvent(event, true, true);
        document.body.dispatchEvent(evt);
    };
    Editor.prototype.selectControlsByRect = function (rect, ctrlKey) {
        for (var i = 0; i < this.controls.length; i++) {
            var original = this.controls[i].ctrlKey;
            this.controls[i].ctrlKey = true;
            var intersect = this.controls[i].isIntersectWith(rect);
            if (intersect) {
                if (ctrlKey && this.controls[i].selected) {
                    this.controls[i].selected = false;
                }
                else {
                    this.controls[i].selected = true;
                }
            }
            else {
                if (!ctrlKey) {
                    this.controls[i].selected = false;
                }
            }
            this.controls[i].ctrlKey = original;
        }
    };
    Editor.prototype.setCurrentToolBoxItem = function (typename) {
        if (!typename) {
            this.currentToolBoxItem = null;
            return;
        }
        var item;
        eval("item = new " + typename + "()");
        this.currentToolBoxItem = item;
    };
    Editor.prototype.svgContainerClick = function (e) {
        var _this = this;
        if (!this.currentToolBoxItem) {
            while (AllSelectedControls.length > 0) {
                AllSelectedControls[0].selected = false;
            }
            return;
        }
        if (!this.beginedToolBoxItem) {
            if (this.currentToolBoxItem.supportMove) {
                this.beginedToolBoxItem = this.currentToolBoxItem;
            }
            else {
                this.currentToolBoxItem.buildDone = function (control) {
                    if (control) {
                        _this.controls.push(control);
                    }
                    if (window.toolboxDone) {
                        window.toolboxDone();
                    }
                };
            }
            this.currentToolBoxItem.begin(this.svgContainer, this.svgContainerMouseUpPosition);
        }
        else {
            var control = this.beginedToolBoxItem.end();
            if (control) {
                this.controls.push(control);
            }
            this.beginedToolBoxItem = null;
            if (window.toolboxDone) {
                window.toolboxDone();
            }
        }
    };
    Editor.prototype.svgContainerMouseMove = function (x, y) {
        if (this.beginedToolBoxItem) {
            this.beginedToolBoxItem.mousemove(x, y);
        }
    };
    Editor.prototype.setting = function (e) {
        var _this = this;
        e.stopPropagation();
        if (window.toolboxDone) {
            window.toolboxDone();
        }
        if (!this.propertyDialog)
            this.propertyDialog = new PropertyDialog(new SVGContainerControl(this.svgContainer));
        this.propertyDialog.show();
        this._svgContainerClickForDialog = function (e) {
            _this.svgContainerClickForDialog(e);
        };
        this.svgContainer.addEventListener("click", this._svgContainerClickForDialog, false);
    };
    Editor.prototype.svgContainerClickForDialog = function (e) {
        var ele = e.target;
        while (ele.tagName != "BODY") {
            if (ele == this.propertyDialog.rootElement) {
                return;
            }
            else {
                ele = ele.parentElement;
            }
        }
        this.propertyDialog.hide();
        this.svgContainer.removeEventListener("click", this._svgContainerClickForDialog, false);
    };
    Editor.prototype.alignLeft = function () {
        var minLeft = 999999;
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            if (rect.x < minLeft)
                minLeft = rect.x;
        }
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            rect.x = minLeft;
            control.rect = rect;
        }
    };
    Editor.prototype.alignRight = function () {
        var maxRight = -99999;
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            if (rect.x + rect.width > maxRight)
                maxRight = rect.x + rect.width;
        }
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            rect.x = maxRight - rect.width;
            control.rect = rect;
        }
    };
    Editor.prototype.alignTop = function () {
        var minTop = 999999;
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            if (rect.y < minTop)
                minTop = rect.y;
        }
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            rect.y = minTop;
            control.rect = rect;
        }
    };
    Editor.prototype.alignBottom = function () {
        var maxBottom = -99999;
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            if (rect.y + rect.height > maxBottom)
                maxBottom = rect.y + rect.height;
        }
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            rect.y = maxBottom - rect.height;
            control.rect = rect;
        }
    };
    Editor.prototype.hSpacing = function () {
        if (AllSelectedControls.length <= 1)
            return;
        var totalspacing = 0;
        var maxRight = -99999;
        var minLeft = 999999;
        var rects = [];
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            rect.control = control;
            rects.push(rect);
            if (rect.x + rect.width > maxRight)
                maxRight = rect.x + rect.width;
            if (rect.x < minLeft)
                minLeft = rect.x;
        }
        totalspacing = maxRight - minLeft;
        for (var i = 0; i < rects.length; i++) {
            var rect = rects[i];
            totalspacing -= rect.width;
        }
        for (var i = 0; i < rects.length - 1; i++) {
            var rect = rects[i];
            var rect2 = rects[i + 1];
            if (rect.x > rect2.x) {
                rects[i] = rect2;
                rects[i + 1] = rect;
                i -= 2;
                if (i < -1)
                    i = -1;
            }
        }
        var interval = totalspacing / (rects.length - 1);
        var left = minLeft;
        for (var i = 0; i < rects.length; i++) {
            var rect = rects[i];
            var ctrl = rect.control;
            rect.control = null;
            rect.x = left;
            left += rect.width + interval;
            ctrl.rect = rect;
        }
    };
    Editor.prototype.vSpacing = function () {
        if (AllSelectedControls.length <= 1)
            return;
        var totalspacing = 0;
        var maxBottom = -99999;
        var minTop = 999999;
        var rects = [];
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            rect.control = control;
            rects.push(rect);
            if (rect.y + rect.height > maxBottom)
                maxBottom = rect.y + rect.height;
            if (rect.y < minTop)
                minTop = rect.y;
        }
        totalspacing = maxBottom - minTop;
        for (var i = 0; i < rects.length; i++) {
            var rect = rects[i];
            totalspacing -= rect.height;
        }
        for (var i = 0; i < rects.length - 1; i++) {
            var rect = rects[i];
            var rect2 = rects[i + 1];
            if (rect.y > rect2.y) {
                rects[i] = rect2;
                rects[i + 1] = rect;
                i -= 2;
                if (i < -1)
                    i = -1;
            }
        }
        var interval = totalspacing / (rects.length - 1);
        var top = minTop;
        for (var i = 0; i < rects.length; i++) {
            var rect = rects[i];
            var ctrl = rect.control;
            rect.control = null;
            rect.y = top;
            top += rect.height + interval;
            ctrl.rect = rect;
        }
    };
    Editor.prototype.hCenter = function () {
        var maxHeight = 0;
        var y;
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            if (rect.height > maxHeight) {
                y = rect.y;
                maxHeight = rect.height;
            }
        }
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            rect.y = y + maxHeight / 2 - rect.height / 2;
            control.rect = rect;
        }
    };
    Editor.prototype.vCenter = function () {
        var maxWidth = 0;
        var x;
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            if (rect.width > maxWidth) {
                x = rect.x;
                maxWidth = rect.width;
            }
        }
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            rect.x = x + maxWidth / 2 - rect.width / 2;
            control.rect = rect;
        }
    };
    Editor.prototype.layerUp = function () {
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var nextEle = control.element.nextElementSibling;
            while (nextEle && !nextEle._editorControl) {
                nextEle = nextEle.nextElementSibling;
            }
            if (nextEle) {
                this.svgContainer.removeChild(nextEle);
                this.svgContainer.insertBefore(nextEle, control.element);
            }
        }
    };
    Editor.prototype.layerDown = function () {
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var preEle = control.element.previousElementSibling;
            while (preEle && !preEle._editorControl) {
                preEle = preEle.previousElementSibling;
            }
            if (preEle) {
                this.svgContainer.removeChild(control.element);
                this.svgContainer.insertBefore(control.element, preEle);
            }
        }
    };
    Editor.prototype.layerFront = function () {
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            this.svgContainer.removeChild(control.element);
            this.svgContainer.appendChild(control.element);
        }
    };
    Editor.prototype.layerBottom = function () {
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            if (this.svgContainer.children[0] != control.element) {
                this.svgContainer.removeChild(control.element);
                this.svgContainer.insertBefore(control.element, this.svgContainer.children[0]);
            }
        }
    };
    Editor.prototype.getIndex = function (element) {
        for (var i = 0; i < this.svgContainer.children.length; i++) {
            if (this.svgContainer.children[i] == element)
                return i;
        }
        return -1;
    };
    return Editor;
}());
//# sourceMappingURL=Editor.js.map