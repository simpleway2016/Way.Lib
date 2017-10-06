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
        this.lineElement.setAttribute('style', 'stroke:#aaaaaa;stroke-width:5;cursor:pointer;');
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
        this.rectElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;cursor:pointer;');
        this.startx = position.x;
        this.starty = position.y;
        svgContainer.appendChild(this.rectElement);
    };
    ToolBox_Rect.prototype.mousemove = function (x, y) {
        this.rectElement.setAttribute('width', Math.max(0, x - this.startx));
        this.rectElement.setAttribute('height', Math.max(0, y - this.starty));
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
        this.rootElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;cursor:pointer;');
        this.startx = position.x;
        this.starty = position.y;
        svgContainer.appendChild(this.rootElement);
    };
    ToolBox_Ellipse.prototype.mousemove = function (x, y) {
        this.rootElement.setAttribute('rx', Math.max(0, x - this.startx));
        this.rootElement.setAttribute('ry', Math.max(0, y - this.starty));
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
        this.rootElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;cursor:pointer;');
        this.startx = position.x;
        this.starty = position.y;
        svgContainer.appendChild(this.rootElement);
    };
    ToolBox_Circle.prototype.mousemove = function (x, y) {
        this.rootElement.setAttribute('r', Math.max(0, x - this.startx));
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
    }
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
    return Editor;
}());
//# sourceMappingURL=Editor.js.map