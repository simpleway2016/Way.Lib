var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var ServerUrl;
var windowGuid = new Date().getTime();
var CtrlKey = false;
window.onerror = function (errorMessage, scriptURI, lineNumber) {
    alert("发生错误:\r\n" + errorMessage + "\r\nuri:" + scriptURI + "\r\nline:" + lineNumber);
};
if (true) {
    var index = location.href.indexOf("://");
    var domain = location.href.substr(location.href.indexOf("://") + 3);
    ServerUrl = location.href.substr(0, index) + "://" + domain.substr(0, domain.indexOf("/"));
}
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
        this.control = new LineControl();
        this.control.lineElement.setAttribute('x1', position.x);
        this.control.lineElement.setAttribute('y1', position.y);
        this.control.lineElement.setAttribute('x2', position.x);
        this.control.lineElement.setAttribute('y2', position.y);
        this.control.virtualLineElement.setAttribute('x1', position.x);
        this.control.virtualLineElement.setAttribute('y1', position.y);
        this.control.virtualLineElement.setAttribute('x2', position.x);
        this.control.virtualLineElement.setAttribute('y2', position.y);
        svgContainer.appendChild(this.control.element);
    };
    ToolBox_Line.prototype.mousemove = function (x, y) {
        this.control.lineElement.setAttribute('x2', x);
        this.control.lineElement.setAttribute('y2', y);
        this.control.virtualLineElement.setAttribute('x2', x);
        this.control.virtualLineElement.setAttribute('y2', y);
    };
    ToolBox_Line.prototype.end = function () {
        return this.control;
    };
    return ToolBox_Line;
}(ToolBoxItem));
var ToolBox_Rect = (function (_super) {
    __extends(ToolBox_Rect, _super);
    function ToolBox_Rect() {
        return _super.call(this) || this;
    }
    ToolBox_Rect.prototype.begin = function (svgContainer, position) {
        this.control = new RectControl(null);
        this.control.element.setAttribute('x', position.x);
        this.control.element.setAttribute('y', position.y);
        this.control.element.setAttribute('width', "0");
        this.control.element.setAttribute('height', "0");
        this.startx = position.x;
        this.starty = position.y;
        svgContainer.appendChild(this.control.element);
    };
    ToolBox_Rect.prototype.mousemove = function (x, y) {
        this.control.rect = {
            x: Math.min(x, this.startx),
            y: Math.min(y, this.starty),
            width: Math.abs(x - this.startx),
            height: Math.abs(y - this.starty)
        };
    };
    ToolBox_Rect.prototype.end = function () {
        if (this.control.element.getAttribute("width") == 0 || this.control.element.getAttribute("height") == 0) {
            return null;
        }
        return this.control;
    };
    return ToolBox_Rect;
}(ToolBoxItem));
var ToolBox_Ellipse = (function (_super) {
    __extends(ToolBox_Ellipse, _super);
    function ToolBox_Ellipse() {
        return _super.call(this) || this;
    }
    ToolBox_Ellipse.prototype.begin = function (svgContainer, position) {
        this.control = new EllipseControl();
        this.control.element.setAttribute('cx', position.x);
        this.control.element.setAttribute('cy', position.y);
        this.control.element.setAttribute('rx', "0");
        this.control.element.setAttribute('ry', "0");
        this.startx = position.x;
        this.starty = position.y;
        svgContainer.appendChild(this.control.element);
    };
    ToolBox_Ellipse.prototype.mousemove = function (x, y) {
        this.control.element.setAttribute('rx', Math.abs(x - this.startx));
        this.control.element.setAttribute('ry', Math.abs(y - this.starty));
    };
    ToolBox_Ellipse.prototype.end = function () {
        if (this.control.element.getAttribute("rx") == 0 || this.control.element.getAttribute("ry") == 0) {
            return null;
        }
        return this.control;
    };
    return ToolBox_Ellipse;
}(ToolBoxItem));
var ToolBox_Circle = (function (_super) {
    __extends(ToolBox_Circle, _super);
    function ToolBox_Circle() {
        return _super.call(this) || this;
    }
    ToolBox_Circle.prototype.begin = function (svgContainer, position) {
        this.control = new CircleControl();
        this.control.element.setAttribute('cx', position.x);
        this.control.element.setAttribute('cy', position.y);
        this.control.element.setAttribute('r', "0");
        this.startx = position.x;
        this.starty = position.y;
        svgContainer.appendChild(this.control.element);
    };
    ToolBox_Circle.prototype.mousemove = function (x, y) {
        this.control.element.setAttribute('r', Math.abs(x - this.startx));
    };
    ToolBox_Circle.prototype.end = function () {
        if (this.control.element.getAttribute("r") == 0) {
            return null;
        }
        return this.control;
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
        fileBrowser.onHide = function () {
            if (window.toolboxDone) {
                window.toolboxDone();
            }
        };
        fileBrowser.onSelectFile = function (path) {
            fileBrowser.hide();
            _this.control = new ImageControl();
            _this.control.element.setAttribute('x', position.x);
            _this.control.element.setAttribute('y', position.y);
            _this.control.element.setAttribute('width', "200");
            _this.control.element.setAttribute('height', "200");
            _this.control.element.href.baseVal = path;
            svgContainer.appendChild(_this.control.element);
            if (_this.buildDone) {
                _this.buildDone(_this.control);
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
        this.control = new TextControl();
        this.control.rect = position;
        svgContainer.appendChild(this.control.element);
        this.control.element.onselectstart = function (e) { e.preventDefault(); e.cancelBubble = true; return false; };
        if (this.buildDone) {
            this.buildDone(this.control);
        }
    };
    return ToolBox_Text;
}(ToolBoxItem));
var ToolBox_Cylinder = (function (_super) {
    __extends(ToolBox_Cylinder, _super);
    function ToolBox_Cylinder() {
        return _super.call(this) || this;
    }
    ToolBox_Cylinder.prototype.begin = function (svgContainer, position) {
        this.control = new CylinderControl();
        this.control.rectElement.setAttribute('x', position.x);
        this.control.rectElement.setAttribute('y', position.y);
        this.startx = position.x;
        this.starty = position.y;
        svgContainer.appendChild(this.control.element);
    };
    ToolBox_Cylinder.prototype.mousemove = function (x, y) {
        this.control.rectElement.setAttribute('x', Math.min(x, this.startx));
        this.control.rectElement.setAttribute('y', Math.min(y, this.starty));
        this.control.rectElement.setAttribute('width', Math.abs(x - this.startx));
        this.control.rectElement.setAttribute('height', Math.abs(y - this.starty));
    };
    ToolBox_Cylinder.prototype.end = function () {
        if (this.control.rectElement.getAttribute("width") == 0 || this.control.rectElement.getAttribute("height") == 0) {
            return null;
        }
        this.control.resetCylinder(this.control.rect);
        return this.control;
    };
    return ToolBox_Cylinder;
}(ToolBoxItem));
var ToolBox_Trend = (function (_super) {
    __extends(ToolBox_Trend, _super);
    function ToolBox_Trend() {
        return _super.call(this) || this;
    }
    ToolBox_Trend.prototype.begin = function (svgContainer, position) {
        this.control = new TrendControl();
        this.control.rectElement.setAttribute('x', position.x);
        this.control.rectElement.setAttribute('y', position.y);
        this.control.rectElement.setAttribute('width', "0");
        this.control.rectElement.setAttribute('height', "0");
        this.startx = position.x;
        this.starty = position.y;
        svgContainer.appendChild(this.control.element);
    };
    ToolBox_Trend.prototype.mousemove = function (x, y) {
        this.control.rect = {
            x: Math.min(x, this.startx),
            y: Math.min(y, this.starty),
            width: Math.abs(x - this.startx),
            height: Math.abs(y - this.starty)
        };
    };
    ToolBox_Trend.prototype.end = function () {
        if (this.control.rectElement.getAttribute("width") == 0 || this.control.rectElement.getAttribute("height") == 0) {
            return null;
        }
        return this.control;
    };
    return ToolBox_Trend;
}(ToolBoxItem));
var ToolBox_HistoryTrend = (function (_super) {
    __extends(ToolBox_HistoryTrend, _super);
    function ToolBox_HistoryTrend() {
        return _super.call(this) || this;
    }
    ToolBox_HistoryTrend.prototype.begin = function (svgContainer, position) {
        this.control = new HistoryTrendControl();
        this.control.rectElement.setAttribute('x', position.x);
        this.control.rectElement.setAttribute('y', position.y);
        this.control.rectElement.setAttribute('width', "0");
        this.control.rectElement.setAttribute('height', "0");
        this.startx = position.x;
        this.starty = position.y;
        svgContainer.appendChild(this.control.element);
    };
    ToolBox_HistoryTrend.prototype.mousemove = function (x, y) {
        this.control.rect = {
            x: Math.min(x, this.startx),
            y: Math.min(y, this.starty),
            width: Math.abs(x - this.startx),
            height: Math.abs(y - this.starty)
        };
    };
    ToolBox_HistoryTrend.prototype.end = function () {
        if (this.control.rectElement.getAttribute("width") == 0 || this.control.rectElement.getAttribute("height") == 0) {
            return null;
        }
        return this.control;
    };
    return ToolBox_HistoryTrend;
}(ToolBoxItem));
var ToolBox_ButtonArea = (function (_super) {
    __extends(ToolBox_ButtonArea, _super);
    function ToolBox_ButtonArea() {
        return _super.call(this) || this;
    }
    ToolBox_ButtonArea.prototype.begin = function (svgContainer, position) {
        this.control = new ButtonAreaControl();
        this.control.element.setAttribute('x', position.x);
        this.control.element.setAttribute('y', position.y);
        this.control.element.setAttribute('width', "0");
        this.control.element.setAttribute('height', "0");
        this.startx = position.x;
        this.starty = position.y;
        svgContainer.appendChild(this.control.element);
    };
    ToolBox_ButtonArea.prototype.mousemove = function (x, y) {
        this.control.rect = {
            x: Math.min(x, this.startx),
            y: Math.min(y, this.starty),
            width: Math.abs(x - this.startx),
            height: Math.abs(y - this.starty)
        };
    };
    ToolBox_ButtonArea.prototype.end = function () {
        if (this.control.element.getAttribute("width") == 0 || this.control.element.getAttribute("height") == 0) {
            return null;
        }
        return this.control;
    };
    return ToolBox_ButtonArea;
}(ToolBoxItem));
var Editor = (function () {
    function Editor(id) {
        var _this = this;
        this.name = "";
        this.code = "";
        this.beginedToolBoxItem = null;
        this.controls = [];
        this.changed = false;
        this._customProperties = "";
        this.isWatchingRect = false;
        this.isRunMode = false;
        this.currentScale = 1;
        this.divContainer = document.body.querySelector("#" + id);
        this.undoMgr = new UndoManager();
        this.svgContainer = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        this.svgContainer.style.backgroundSize = "100% 100%";
        this.svgContainer.style.backgroundRepeat = "no-repeat";
        this.svgContainer.style.backgroundColor = "#ffffff";
        this.svgContainer.style.height = "8000px";
        this.svgContainer.style.width = "8000px";
        this.divContainer.appendChild(this.svgContainer);
        this.initDivContainer();
        this.initScaleEvent();
        this.initMoveToScrollEvent();
        this.resetScrollbar();
        this.svgContainer.addEventListener("click", function (e) {
            if (e.button == 0) {
                if (_this.svgContainer._notClick) {
                    _this.svgContainer._notClick = false;
                    return;
                }
                _this.svgContainerClick(e);
            }
        });
        this.svgContainer.addEventListener("mousedown", function (e) {
            if (e.button == 0) {
                if (!_this.currentToolBoxItem) {
                    _this.svgContainer._notClick = true;
                    _this.selectingElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
                    _this.selectingElement._startx = e.layerX;
                    _this.selectingElement._starty = e.layerY;
                    _this.selectingElement.setAttribute('x', (e.layerX));
                    _this.selectingElement.setAttribute('y', (e.layerY));
                    _this.selectingElement.setAttribute('width', "0");
                    _this.selectingElement.setAttribute('height', "0");
                    _this.selectingElement.setAttribute('style', 'fill:none;stroke:black;stroke-width:1;stroke-dasharray:2;stroke-dashoffset:2;');
                    _this.svgContainer.appendChild(_this.selectingElement);
                    _this.svgContainer.setCapture();
                }
            }
        });
        this.svgContainer.addEventListener("mouseup", function (e) {
            if (e.button == 0) {
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
                    CtrlKey = e.ctrlKey;
                    _this.selectControlsByRect(rect);
                    CtrlKey = false;
                    setTimeout(function () {
                        _this.svgContainer._notClick = false;
                    }, 500);
                }
                else {
                    _this.svgContainerMouseUpPosition = {
                        x: e.layerX,
                        y: e.layerY
                    };
                }
            }
        });
        this.svgContainer.addEventListener("mousemove", function (e) {
            if (_this.selectingElement) {
                var w = e.layerX - _this.selectingElement._startx;
                var h = e.layerY - _this.selectingElement._starty;
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
                _this.svgContainerMouseMove(e.layerX, e.layerY);
            }
        });
        this.svgContainer.addEventListener("dragover", function (ev) {
            ev.preventDefault();
        });
        this.svgContainer.addEventListener("drop", function (ev) {
            ev.preventDefault();
            try {
                var data = JSON.parse(ev.dataTransfer.getData("Text"));
                if (data && data.Type == "GroupControl") {
                    var rect = {};
                    rect.x = ev.layerX;
                    rect.y = ev.layerY;
                    rect.width = null;
                    rect.height = null;
                    var groupControl = _this.createGroupControl(data.windowCode, rect);
                }
                else if (data && data.Type == "Point") {
                    if (_this.editingPointTextbox) {
                        _this.editingPointTextbox.value = data.Name;
                        _this.editingPointTextbox.changeFunc();
                    }
                }
            }
            catch (e) {
            }
        });
        var movingUndo = null;
        document.body.addEventListener("keyup", function (e) {
            if (movingUndo) {
                movingUndo.moveFinish();
                _this.undoMgr.addUndo(movingUndo);
                movingUndo = null;
            }
        }, false);
        document.body.addEventListener("keydown", function (e) {
            if (e.keyCode == 37) {
                if (!movingUndo) {
                    movingUndo = new UndoMoveControls(_this, AllSelectedControls);
                }
                for (var i = 0; i < AllSelectedControls.length; i++) {
                    var control = AllSelectedControls[i];
                    var rect = control.rect;
                    rect.x--;
                    control.rect = rect;
                }
            }
            else if (e.keyCode == 38) {
                if (!movingUndo) {
                    movingUndo = new UndoMoveControls(_this, AllSelectedControls);
                }
                for (var i = 0; i < AllSelectedControls.length; i++) {
                    var control = AllSelectedControls[i];
                    var rect = control.rect;
                    rect.y--;
                    control.rect = rect;
                }
            }
            else if (e.keyCode == 39) {
                if (!movingUndo) {
                    movingUndo = new UndoMoveControls(_this, AllSelectedControls);
                }
                for (var i = 0; i < AllSelectedControls.length; i++) {
                    var control = AllSelectedControls[i];
                    var rect = control.rect;
                    rect.x++;
                    control.rect = rect;
                }
            }
            else if (e.keyCode == 40) {
                if (!movingUndo) {
                    movingUndo = new UndoMoveControls(_this, AllSelectedControls);
                }
                for (var i = 0; i < AllSelectedControls.length; i++) {
                    var control = AllSelectedControls[i];
                    var rect = control.rect;
                    rect.y++;
                    control.rect = rect;
                }
            }
            else if (e.ctrlKey && e.keyCode == 90) {
                _this.undo();
            }
            else if (e.ctrlKey && e.keyCode == 89) {
                _this.redo();
            }
            else if (e.ctrlKey && e.keyCode == 67) {
                _this.copy();
            }
            else if (e.ctrlKey && e.keyCode == 88) {
                _this.cut();
            }
            else if (e.ctrlKey && e.keyCode == 86) {
                _this.paste();
            }
            else if (e.ctrlKey && e.keyCode == 83) {
                _this.save();
            }
            else if (e.keyCode == 46) {
                _this.delete();
            }
            else if (e.keyCode == 27) {
                if (_this.isRunMode == false) {
                    window.exitFullScreen();
                }
            }
            else if (e.keyCode == 13) {
                if (AllSelectedControls.length > 0) {
                    AllSelectedControls[0].showProperty();
                }
            }
            else {
            }
        }, false);
    }
    Editor.prototype.getControl = function (id) {
        for (var i = 0; i < this.controls.length; i++) {
            if (this.controls[i].id == id) {
                return this.controls[i];
            }
        }
        return null;
    };
    Editor.prototype.removeControl = function (ctrl) {
        for (var i = 0; i < this.controls.length; i++) {
            if (this.controls[i] == ctrl) {
                ctrl.container = null;
                this.svgContainer.removeChild(ctrl.element);
                this.controls.splice(i, 1);
                break;
            }
        }
    };
    Editor.prototype.addControl = function (ctrl) {
        if (!ctrl.element.parentElement) {
            this.svgContainer.appendChild(ctrl.element);
        }
        if (ctrl.constructor.name == "GroupControl") {
            var groupControl = ctrl;
            var minleft = 9999999;
            var mintop = 9999999;
            for (var i = 0; i < groupControl.controls.length; i++) {
                var child = groupControl.controls[i];
                var rect = child.rect;
                if (minleft > rect.x)
                    minleft = rect.x;
                if (mintop > rect.y)
                    mintop = rect.y;
            }
            for (var i = 0; i < groupControl.controls.length; i++) {
                var child = groupControl.controls[i];
                var rect = child.rect;
                rect.x -= minleft;
                rect.y -= mintop;
                child.rect = rect;
            }
        }
        if (!ctrl.id || ctrl.id.length == 0) {
            var controlId = ctrl.constructor.name;
            var index = 1;
            while (this.isIdExist(controlId + index)) {
                index++;
            }
            ctrl.id = controlId + index;
        }
        this.controls.push(ctrl);
        ctrl.container = this;
    };
    Editor.prototype.writeValue = function (pointName, addr, value) {
        window.writeValue(pointName, addr, value);
    };
    Object.defineProperty(Editor.prototype, "colorBG", {
        get: function () {
            return this.svgContainer.style.backgroundColor;
        },
        set: function (v) {
            if (v == "")
                v = "#FFFFFF";
            this.svgContainer.style.backgroundColor = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Editor.prototype, "imgBg", {
        get: function () {
            var url = this.svgContainer.style.backgroundImage;
            if (url && url.length > 0) {
                url = url.substr(4, url.length - 5);
            }
            return url;
        },
        set: function (v) {
            if (v == "") {
                this.svgContainer.style.backgroundImage = "";
            }
            else {
                this.svgContainer.style.backgroundImage = "url(" + v + ")";
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Editor.prototype, "bgWidth", {
        get: function () {
            var size = this.svgContainer.style.backgroundSize.split(' ');
            return size[0];
        },
        set: function (v) {
            if (v.indexOf("%") < 0 && v.indexOf("px") < 0)
                v += "px";
            var size = this.svgContainer.style.backgroundSize.split(' ');
            this.svgContainer.style.backgroundSize = v + " " + size[1];
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Editor.prototype, "bgHeight", {
        get: function () {
            var size = this.svgContainer.style.backgroundSize.split(' ');
            return size[1];
        },
        set: function (v) {
            if (v.indexOf("%") < 0 && v.indexOf("px") < 0)
                v += "px";
            var size = this.svgContainer.style.backgroundSize.split(' ');
            this.svgContainer.style.backgroundSize = size[0] + " " + v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Editor.prototype, "customProperties", {
        get: function () {
            return this._customProperties;
        },
        set: function (v) {
            this._customProperties = v;
        },
        enumerable: true,
        configurable: true
    });
    Editor.prototype.getPropertiesCaption = function () {
        return ["名称", "编号", "底色", "背景图", "背景图宽", "背景图高", "窗口宽", "窗口高", "自定义变量"];
    };
    Editor.prototype.getProperties = function () {
        return ["name", "code", "colorBG", "imgBg", "bgWidth", "bgHeight", "windowWidth", "windowHeight", "customProperties"];
    };
    Object.defineProperty(Editor.prototype, "scrollTop", {
        get: function () {
            return this.svgContainer.parentElement.scrollTop;
        },
        set: function (v) {
            this.svgContainer.parentElement.scrollTop = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Editor.prototype, "scrollLeft", {
        get: function () {
            return this.svgContainer.parentElement.scrollLeft;
        },
        set: function (v) {
            this.svgContainer.parentElement.scrollLeft = v;
        },
        enumerable: true,
        configurable: true
    });
    Editor.prototype.initMoveToScrollEvent = function () {
        var svg1 = this.svgContainer;
        var scrolling = false;
        var downY;
        var downX;
        var scrollInfo;
        svg1.addEventListener("mousedown", function (e) {
            if (e.button == 2) {
                e.stopPropagation();
                e.preventDefault();
                svg1.setCapture();
                svg1.style.cursor = "-moz-grab";
                scrolling = true;
                downX = e.clientX;
                downY = e.clientY;
                scrollInfo = {
                    x: svg1.parentElement.scrollLeft,
                    y: svg1.parentElement.scrollTop,
                };
            }
        }, false);
        svg1.addEventListener("mouseup", function (e) {
            if (scrolling) {
                e.stopPropagation();
                e.preventDefault();
                scrolling = false;
                svg1.releaseCapture();
                svg1.style.cursor = "default";
            }
        }, false);
        svg1.addEventListener("mousemove", function (e) {
            if (scrolling) {
                e.stopPropagation();
                e.preventDefault();
                svg1.parentElement.scrollTo(Math.max(0, scrollInfo.x + downX - e.clientX), Math.max(0, scrollInfo.y + downY - e.clientY));
            }
        }, false);
    };
    Editor.prototype.initScaleEvent = function () {
        var _this = this;
        var svg1 = this.svgContainer;
        svg1.style.transformOrigin = "0 0";
        var scaleFlag = 1;
        var scaling = false;
        var downY;
        var downX;
        var downClientRect;
        var downScroll;
        var downScaleFlag;
        svg1.addEventListener("mousedown", function (e) {
            if (e.button == 1) {
                e.stopPropagation();
                e.preventDefault();
                svg1.style.cursor = "none";
                svg1.setCapture();
                scaling = true;
                scaleFlag = _this.currentScale;
                downY = e.layerY;
                downX = e.layerX;
                downScroll = {
                    h: svg1.parentElement.scrollLeft,
                    v: svg1.parentElement.scrollTop
                };
                downClientRect = {
                    x: e.clientX,
                    y: e.clientY - svg1.parentElement.offsetTop
                };
                downScaleFlag = scaleFlag;
            }
        }, false);
        svg1.addEventListener("mouseup", function (e) {
            if (scaling) {
                e.stopPropagation();
                e.preventDefault();
                scaling = false;
                svg1.style.cursor = "default";
                svg1.releaseCapture();
            }
        }, false);
        svg1.addEventListener("mousemove", function (e) {
            if (scaling) {
                e.stopPropagation();
                e.preventDefault();
                scaleFlag = downScaleFlag + parseFloat((downY - e.layerY)) / 200;
                if (scaleFlag < 1)
                    scaleFlag = 1;
                _this.scale(scaleFlag);
                var pointX = downX * scaleFlag;
                var pointY = downY * scaleFlag;
                svg1.parentElement.scrollTo(Math.max(0, pointX - downClientRect.x), Math.max(0, pointY - downClientRect.y));
            }
        }, false);
    };
    Editor.prototype.initDivContainer = function () {
        var _this = this;
        this.divContainer.style.position = "relative";
        var border = document.createElement("DIV");
        border.style.borderRight = "1px solid #eee";
        border.style.borderBottom = "1px solid #eee";
        border.style.position = "absolute";
        border.style.left = "0px";
        border.style.top = "0px";
        border.innerHTML = "<div style='position:absolute;right:0;bottom:0;color:#aaa;font-size:12px;'></div>";
        this.divContainer.insertBefore(border, this.divContainer.children[0]);
        var func = function (e) {
            if (_this.isRunMode) {
                _this.divContainer.removeChild(border);
                _this.divContainer.removeEventListener("mousemove", func, false);
                return;
            }
            if (_this.isWatchingRect && e.layerY > 0) {
                border.style.display = "";
                border.style.width = e.layerX + "px";
                border.style.height = (e.layerY) + "px";
                border.children[0].innerHTML = e.layerX + "," + (e.layerY);
            }
            else {
                border.style.display = "none";
            }
        };
        this.divContainer.addEventListener("mousemove", func, false);
    };
    Editor.prototype.run = function () {
        this.isRunMode = true;
    };
    Editor.prototype.createGroupControl = function (windowCode, rect) {
        try {
            var json = JHttpHelper.downloadUrl(ServerUrl + "/Home/GetWindowCode?windowCode=" + encodeURIComponent(windowCode));
            var content;
            eval("content=" + json);
            var groupEle = document.createElementNS('http://www.w3.org/2000/svg', 'g');
            var editor = new GroupControl(groupEle, windowCode);
            eval(content.controlsScript);
            editor.loadCustomProperties(content.customProperties);
            this.addControl(editor);
            editor.rect = rect;
            this.undoMgr.addUndo(new UndoAddControl(this, editor));
            return editor;
        }
        catch (e) {
            alert(e.message);
        }
    };
    Editor.prototype.rebuildControls = function () {
        var newControls = [];
        for (var i = 0; i < this.svgContainer.children.length; i++) {
            var ele = this.svgContainer.children[i];
            if (ele._editorControl && this.controls.indexOf(ele._editorControl) >= 0) {
                newControls.push(ele._editorControl);
            }
        }
        if (newControls.length != this.controls.length)
            alert("rebuildControls失败");
        this.controls = newControls;
    };
    Editor.prototype.getScript = function () {
        var properties = this.getProperties();
        var script = "";
        for (var i = 0; i < properties.length; i++) {
            if (this[properties[i]]) {
                script += "editor." + properties[i] + " = " + JSON.stringify(this[properties[i]]) + ";\r\n";
            }
        }
        return script;
    };
    Editor.prototype.scale = function (_scale) {
        this.currentScale = _scale;
        this.svgContainer.style.transform = "scale(" + this.currentScale + "," + this.currentScale + ")";
    };
    Editor.prototype.undo = function () {
        this.undoMgr.undo();
    };
    Editor.prototype.redo = function () {
        this.undoMgr.redo();
    };
    Editor.prototype.selectAll = function () {
        CtrlKey = true;
        for (var i = 0; i < this.controls.length; i++) {
            this.controls[i].selected = true;
        }
        CtrlKey = false;
    };
    Editor.prototype.selectWebControlByPointName = function (pointName) {
        for (var i = 0; i < this.controls.length; i++) {
            this.controls[i].selected = false;
        }
        CtrlKey = true;
        for (var i = 0; i < this.controls.length; i++) {
            this.controls[i].selectByPointName(pointName);
        }
        CtrlKey = false;
    };
    Editor.prototype.group = function () {
        if (AllSelectedControls.length > 0) {
            var items = [];
            for (var i = 0; i < AllSelectedControls.length; i++) {
                items.push(AllSelectedControls[i]);
            }
            var undoObj = new UndoGroup(this, items);
            undoObj.redo();
            this.undoMgr.addUndo(undoObj);
        }
    };
    Editor.prototype.ungroup = function () {
        if (AllSelectedControls.length > 0) {
            var items = [];
            for (var i = 0; i < AllSelectedControls.length; i++) {
                if (AllSelectedControls[i].constructor.name == "FreeGroupControl") {
                    items.push(AllSelectedControls[i]);
                }
            }
            if (items.length > 0) {
                var undoObj = new UndoUnGroup(this, items);
                undoObj.redo();
                this.undoMgr.addUndo(undoObj);
            }
        }
    };
    Editor.prototype.delete = function () {
        var ctrls = [];
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            ctrls.push(control);
        }
        if (ctrls.length > 0) {
            var undoObj = new UndoRemoveControls(this, ctrls);
            undoObj.redo();
            this.undoMgr.addUndo(undoObj);
        }
    };
    Editor.prototype.resetScrollbar = function () {
        var maxWidth = 0;
        var maxHeight = 0;
        for (var i = 0; i < this.controls.length; i++) {
            var rect = this.controls[i].rect;
            if (rect.x + rect.width > maxWidth)
                maxWidth = rect.x + rect.width;
            if (rect.y + rect.height > maxHeight)
                maxHeight = rect.y + rect.height;
        }
        if (maxWidth < this.svgContainer.parentElement.offsetWidth) {
            this.svgContainer.parentElement.style.overflowX = "hidden";
            this.svgContainer.parentElement.scrollLeft = 0;
        }
        else {
            this.svgContainer.parentElement.style.overflowX = "auto";
        }
        if (maxHeight < this.svgContainer.parentElement.offsetHeight) {
            this.svgContainer.parentElement.style.overflowY = "hidden";
            this.svgContainer.parentElement.scrollTop = 0;
        }
        else {
            this.svgContainer.parentElement.style.overflowY = "auto";
        }
    };
    Editor.prototype.save = function () {
        if (this.name.length == 0) {
            alert("请点击左上角设置图标，设置监视画面的名称");
            return;
        }
        if (this.code.length == 0) {
            alert("请点击左上角设置图标，设置监视画面的编号");
            return;
        }
        if (this.customProperties && this.customProperties.length > 0) {
            var items = this.customProperties.split('\n');
            var reg = /[\W]+/;
            for (var i = 0; i < items.length; i++) {
                var name = items[i].trim();
                if (name.length > 0) {
                    if (reg.exec(name)) {
                        alert("自定义变量“" + name + "”包含特殊符合");
                        return;
                    }
                }
            }
        }
        var scripts = "";
        var windowCodes = [];
        for (var i = 0; i < this.controls.length; i++) {
            scripts += this.controls[i].getScript() + "\r\n";
            if (this.controls[i].constructor.name == "GroupControl") {
                windowCodes.push(this.controls[i].windowCode);
            }
        }
        window.save(this.name, this.code, this.windowWidth, this.windowHeight, this.customProperties, this.getScript(), scripts, windowCodes);
    };
    Editor.prototype.getSaveInfo = function () {
        var scripts = "";
        for (var i = 0; i < this.controls.length; i++) {
            scripts += this.controls[i].getScript();
        }
        return JSON.stringify({ "name": this.name, "code": this.code, "editorScript": this.getScript(), "controlsScript": scripts });
    };
    Editor.prototype.cut = function () {
        try {
            var copyitems = [];
            var ctrls = [];
            for (var i = 0; i < AllSelectedControls.length; i++) {
                var control = AllSelectedControls[i];
                var json = control.getJson();
                copyitems.push(json);
                ctrls.push(control);
            }
            window.localStorage.setItem("copy", "");
            window.localStorage.setItem("cut", JSON.stringify(copyitems));
            window.localStorage.setItem("windowGuid", windowGuid + "");
            var undoObj = new UndoRemoveControls(this, ctrls);
            undoObj.redo();
            this.undoMgr.addUndo(undoObj);
        }
        catch (e) {
            alert(e.message);
        }
    };
    Editor.prototype.copy = function () {
        try {
            var copyitems = [];
            for (var i = 0; i < AllSelectedControls.length; i++) {
                var control = AllSelectedControls[i];
                var json = control.getJson();
                copyitems.push(json);
            }
            window.localStorage.setItem("cut", "");
            window.localStorage.setItem("copy", JSON.stringify(copyitems));
            window.localStorage.setItem("windowGuid", windowGuid + "");
        }
        catch (e) {
            alert(e.message);
        }
    };
    Editor.prototype.paste = function () {
        try {
            var str = window.localStorage.getItem("copy");
            if (!str || str.length == 0) {
                str = window.localStorage.getItem("cut");
            }
            if (str && str.length > 0) {
                while (AllSelectedControls.length > 0)
                    AllSelectedControls[0].selected = false;
                var isSameWindow = parseInt(window.localStorage.getItem("windowGuid")) == windowGuid;
                var container = this;
                var copyItems = JSON.parse(str);
                var undoObj = new UndoPaste(this, copyItems, isSameWindow);
                undoObj.redo();
                this.undoMgr.addUndo(undoObj);
            }
        }
        catch (e) {
            alert(e.message);
        }
    };
    Editor.prototype.isIdExist = function (id) {
        for (var i = 0; i < this.controls.length; i++) {
            if (typeof this.controls[i].isIdExist == "function") {
                var result = this.controls[i].isIdExist(id);
                if (result)
                    return true;
            }
            if (this.controls[i].id == id)
                return true;
        }
        return false;
    };
    Editor.prototype.fireBodyEvent = function (event) {
    };
    Editor.prototype.selectControlsByRect = function (rect) {
        var originalCtrlKey = CtrlKey;
        if (originalCtrlKey == false) {
            for (var i = 0; i < this.controls.length; i++) {
                this.controls[i].selected = false;
            }
        }
        CtrlKey = true;
        for (var i = 0; i < this.controls.length; i++) {
            var intersect = this.controls[i].isIntersectWith(rect);
            if (intersect) {
                if (originalCtrlKey && this.controls[i].selected) {
                    this.controls[i].selected = false;
                }
                else {
                    this.controls[i].selected = true;
                }
            }
        }
        CtrlKey = originalCtrlKey;
    };
    Editor.prototype.setCurrentToolBoxItem = function (typename) {
        if (!typename || typename.length == 0) {
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
                        _this.addControl(control);
                        var undoObj = new UndoAddControl(_this, control);
                        _this.undoMgr.addUndo(undoObj);
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
                this.addControl(control);
                var undoObj = new UndoAddControl(this, control);
                this.undoMgr.addUndo(undoObj);
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
            this.propertyDialog = new PropertyDialog(this);
        this.propertyDialog.show();
        this._svgContainerClickForDialog = function (e) {
            _this.svgContainerClickForDialog(e);
        };
        this.svgContainer.addEventListener("click", this._svgContainerClickForDialog, false);
    };
    Editor.prototype.onPropertyDialogTextChanged = function (propertyName) {
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
        var undo = new UndoMoveControlsLayerUp(this, AllSelectedControls);
        undo.redo();
        this.undoMgr.addUndo(undo);
    };
    Editor.prototype.layerDown = function () {
        var undo = new UndoMoveControlsLayerDown(this, AllSelectedControls);
        undo.redo();
        this.undoMgr.addUndo(undo);
    };
    Editor.prototype.layerFront = function () {
        var undo = new UndoMoveControlsLayerFront(this, AllSelectedControls);
        undo.redo();
        this.undoMgr.addUndo(undo);
    };
    Editor.prototype.layerBottom = function () {
        var undo = new UndoMoveControlsLayerBottom(this, AllSelectedControls);
        undo.redo();
        this.undoMgr.addUndo(undo);
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