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
var AllSelectedControls = [];
function documentElementMouseDown(e) {
}
document.documentElement.addEventListener("mousedown", documentElementMouseDown, false);
var EditorControl = (function () {
    function EditorControl(element) {
        var _this = this;
        this.ctrlKey = false;
        this.isInGroup = false;
        this._selected = false;
        this._moveAllSelectedControl = false;
        this.element = element;
        element._editorControl = this;
        this.element.addEventListener("dragstart", function (e) {
            if (_this.isInGroup || !_this.container)
                return;
            e.preventDefault();
        }, false);
        this.element.addEventListener("click", function (e) {
            if (_this.isInGroup || !_this.container)
                return;
            e.stopPropagation();
        }, false);
        this.element.addEventListener("dblclick", function (e) {
            if (_this.isInGroup || !_this.container)
                return;
            e.stopPropagation();
            _this.showProperty();
        }, false);
        this.element.addEventListener("mousedown", function (e) {
            if (_this.isInGroup || !_this.container)
                return;
            if (e.button == 2)
                return;
            _this._moveAllSelectedControl = _this.selected;
            e.stopPropagation();
            _this.ctrlKey = e.ctrlKey;
            if (_this.ctrlKey)
                _this.selected = !_this.selected;
            else
                _this.selected = true;
            _this.mouseDownX = e.clientX;
            _this.mouseDownY = e.clientY;
            if (_this._moveAllSelectedControl) {
                for (var i = 0; i < AllSelectedControls.length; i++) {
                    AllSelectedControls[i].onBeginMoving();
                }
            }
            else {
                _this.onBeginMoving();
            }
        }, false);
        document.body.addEventListener("mousemove", function (e) {
            if (_this.isInGroup || !_this.container)
                return;
            if (_this.mouseDownX >= 0) {
                e.stopPropagation();
                if (_this._moveAllSelectedControl) {
                    for (var i = 0; i < AllSelectedControls.length; i++) {
                        AllSelectedControls[i].onMoving(_this.mouseDownX, _this.mouseDownY, e.clientX, e.clientY);
                    }
                }
                else {
                    _this.onMoving(_this.mouseDownX, _this.mouseDownY, e.clientX, e.clientY);
                }
            }
        }, false);
        document.body.addEventListener("mouseup", function (e) {
            if (_this.isInGroup || !_this.container)
                return;
            if (_this.mouseDownX >= 0) {
                e.stopPropagation();
                _this.onEndMoving();
                _this.mouseDownX = -1;
            }
        }, false);
    }
    Object.defineProperty(EditorControl.prototype, "selected", {
        get: function () {
            return this._selected;
        },
        set: function (value) {
            if (this._selected !== value) {
                this._selected = value;
                if (value) {
                    if (!this.ctrlKey) {
                        while (AllSelectedControls.length > 0) {
                            AllSelectedControls[0].selected = false;
                        }
                    }
                    AllSelectedControls.push(this);
                }
                else {
                    var index = AllSelectedControls.indexOf(this);
                    AllSelectedControls.splice(index, 1);
                    if (this.propertyDialog)
                        this.propertyDialog.hide();
                }
                this.onSelectedChange();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditorControl.prototype, "rect", {
        get: function () {
            return null;
        },
        set: function (v) {
        },
        enumerable: true,
        configurable: true
    });
    EditorControl.prototype.getPropertiesCaption = function () {
        return null;
    };
    EditorControl.prototype.getProperties = function () {
        return null;
    };
    EditorControl.prototype.run = function () {
    };
    EditorControl.prototype.getJson = function () {
        var obj = {
            rect: this.rect,
            constructorName: this.constructor.name
        };
        var properites = this.getProperties();
        for (var i = 0; i < properites.length; i++) {
            obj[properites[i]] = this[properites[i]];
        }
        return obj;
    };
    EditorControl.prototype.isIntersectWith = function (rect) {
        return false;
    };
    EditorControl.prototype.isIntersect = function (rect1, rect) {
        return rect.x < rect1.x + rect1.width && rect1.x < rect.x + rect.width && rect.y < rect1.y + rect1.height && rect1.y < rect.y + rect.height;
    };
    EditorControl.prototype.showProperty = function () {
        if (!this.propertyDialog)
            this.propertyDialog = new PropertyDialog(this);
        this.propertyDialog.show();
    };
    EditorControl.prototype.onSelectedChange = function () {
    };
    EditorControl.prototype.onBeginMoving = function () {
    };
    EditorControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
    };
    EditorControl.prototype.onEndMoving = function () {
    };
    return EditorControl;
}());
var LineControl = (function (_super) {
    __extends(LineControl, _super);
    function LineControl() {
        var _this = _super.call(this, document.createElementNS('http://www.w3.org/2000/svg', 'g')) || this;
        _this.pointEles = [];
        _this.moving = false;
        _this.lineElement = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        _this.element.appendChild(_this.lineElement);
        _this.lineElement.setAttribute('style', 'stroke:#aaaaaa;stroke-width:5;');
        _this.virtualLineElement = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        _this.element.appendChild(_this.virtualLineElement);
        _this.virtualLineElement.setAttribute('style', 'stroke:red;stroke-opacity:0;stroke-width:10;cursor:pointer;');
        return _this;
    }
    Object.defineProperty(LineControl.prototype, "rect", {
        get: function () {
            return {
                x: Math.min(parseInt(this.lineElement.getAttribute("x1")), parseInt(this.lineElement.getAttribute("x2"))),
                y: Math.min(parseInt(this.lineElement.getAttribute("y1")), parseInt(this.lineElement.getAttribute("y2"))),
                width: Math.abs(parseInt(this.lineElement.getAttribute("x1")) - parseInt(this.lineElement.getAttribute("x2"))),
                height: Math.abs(parseInt(this.lineElement.getAttribute("y1")) - parseInt(this.lineElement.getAttribute("y2"))),
            };
        },
        set: function (v) {
            var x = Math.min(parseInt(this.lineElement.getAttribute("x1")), parseInt(this.lineElement.getAttribute("x2")));
            var y = Math.min(parseInt(this.lineElement.getAttribute("y1")), parseInt(this.lineElement.getAttribute("y2")));
            this.lineElement.setAttribute("x1", (parseInt(this.lineElement.getAttribute("x1")) + v.x - x));
            this.lineElement.setAttribute("x2", (parseInt(this.lineElement.getAttribute("x2")) + v.x - x));
            this.lineElement.setAttribute("y1", (parseInt(this.lineElement.getAttribute("y1")) + v.y - y));
            this.lineElement.setAttribute("y2", (parseInt(this.lineElement.getAttribute("y2")) + v.y - y));
            var height = Math.abs(parseInt(this.lineElement.getAttribute("y1")) - parseInt(this.lineElement.getAttribute("y2")));
            if (parseInt(this.lineElement.getAttribute("y1")) < parseInt(this.lineElement.getAttribute("y2"))) {
                var y2 = parseInt(this.lineElement.getAttribute("y2")) + v.height - height;
                this.lineElement.setAttribute("y2", y2);
            }
            else {
                var y1 = parseInt(this.lineElement.getAttribute("y1")) + v.height - height;
                this.lineElement.setAttribute("y1", y1);
            }
            var width = Math.abs(parseInt(this.lineElement.getAttribute("x1")) - parseInt(this.lineElement.getAttribute("x2")));
            if (parseInt(this.lineElement.getAttribute("x1")) < parseInt(this.lineElement.getAttribute("x2"))) {
                var x2 = parseInt(this.lineElement.getAttribute("x2")) + v.width - width;
                this.lineElement.setAttribute("x2", x2);
            }
            else {
                var x1 = parseInt(this.lineElement.getAttribute("x1")) + v.width - width;
                this.lineElement.setAttribute("x1", x1);
            }
            this.virtualLineElement.setAttribute("x1", this.lineElement.getAttribute("x1"));
            this.virtualLineElement.setAttribute("x2", this.lineElement.getAttribute("x2"));
            this.virtualLineElement.setAttribute("y1", this.lineElement.getAttribute("y1"));
            this.virtualLineElement.setAttribute("y2", this.lineElement.getAttribute("y2"));
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineControl.prototype, "point", {
        get: function () {
            return {
                x1: parseInt(this.lineElement.getAttribute("x1")),
                x2: parseInt(this.lineElement.getAttribute("x2")),
                y1: parseInt(this.lineElement.getAttribute("y1")),
                y2: parseInt(this.lineElement.getAttribute("y2")),
            };
        },
        set: function (v) {
            this.lineElement.setAttribute("x1", v.x1);
            this.lineElement.setAttribute("x2", v.x2);
            this.lineElement.setAttribute("y1", v.y1);
            this.lineElement.setAttribute("y2", v.y2);
            this.virtualLineElement.setAttribute("x1", v.x1);
            this.virtualLineElement.setAttribute("x2", v.x2);
            this.virtualLineElement.setAttribute("y1", v.y1);
            this.virtualLineElement.setAttribute("y2", v.y2);
        },
        enumerable: true,
        configurable: true
    });
    LineControl.prototype.getJson = function () {
        var obj = _super.prototype.getJson.call(this);
        obj.point = this.point;
        return obj;
    };
    Object.defineProperty(LineControl.prototype, "lineWidth", {
        get: function () {
            return this.lineElement.style.strokeWidth;
        },
        set: function (v) {
            this.lineElement.style.strokeWidth = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineControl.prototype, "color", {
        get: function () {
            return this.lineElement.style.stroke;
        },
        set: function (v) {
            this.lineElement.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    LineControl.prototype.getPropertiesCaption = function () {
        return ["线宽", "颜色"];
    };
    LineControl.prototype.getProperties = function () {
        return ["lineWidth", "color"];
    };
    LineControl.prototype.isIntersectWith = function (rect) {
        var myrect = this.rect;
        return this.isIntersect(myrect, rect);
    };
    LineControl.prototype.onBeginMoving = function () {
        this.lineElement._x1 = parseInt(this.lineElement.getAttribute("x1"));
        this.lineElement._y1 = parseInt(this.lineElement.getAttribute("y1"));
        this.lineElement._x2 = parseInt(this.lineElement.getAttribute("x2"));
        this.lineElement._y2 = parseInt(this.lineElement.getAttribute("y2"));
    };
    LineControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x1 = (this.lineElement._x1 + nowX - downX);
        var y1 = (this.lineElement._y1 + nowY - downY);
        var x2 = (this.lineElement._x2 + nowX - downX);
        var y2 = (this.lineElement._y2 + nowY - downY);
        this.lineElement.setAttribute("x1", x1);
        this.lineElement.setAttribute("y1", y1);
        this.lineElement.setAttribute("x2", x2);
        this.lineElement.setAttribute("y2", y2);
        this.virtualLineElement.setAttribute("x1", x1);
        this.virtualLineElement.setAttribute("y1", y1);
        this.virtualLineElement.setAttribute("x2", x2);
        this.virtualLineElement.setAttribute("y2", y2);
        if (this.selected) {
            this.pointEles[0].setAttribute("cx", x1);
            this.pointEles[0].setAttribute("cy", y1);
            this.pointEles[1].setAttribute("cx", x2);
            this.pointEles[1].setAttribute("cy", y2);
        }
    };
    LineControl.prototype.onEndMoving = function () {
    };
    LineControl.prototype.onSelectedChange = function () {
        if (this.selected) {
            var pointEle1 = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            pointEle1.setAttribute("r", "5");
            pointEle1.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:ew-resize;');
            pointEle1.xName = "x1";
            pointEle1.yName = "y1";
            this.lineElement.parentElement.appendChild(pointEle1);
            var pointEle2 = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            pointEle2.setAttribute("r", "5");
            pointEle2.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:ew-resize;');
            pointEle2.xName = "x2";
            pointEle2.yName = "y2";
            this.lineElement.parentElement.appendChild(pointEle2);
            this.pointEles.push(pointEle1);
            this.pointEles.push(pointEle2);
            this.resetPointLocation();
            for (var i = 0; i < this.pointEles.length; i++) {
                this.setEvent(this.pointEles[i], "x" + (i + 1), "y" + (i + 1));
            }
        }
        else {
            for (var i = 0; i < this.pointEles.length; i++) {
                this.lineElement.parentElement.removeChild(this.pointEles[i]);
            }
            this.pointEles = [];
        }
    };
    LineControl.prototype.resetPointLocation = function () {
        this.pointEles[0].setAttribute("cx", this.lineElement.x1.animVal.value);
        this.pointEles[0].setAttribute("cy", this.lineElement.y1.animVal.value);
        this.pointEles[1].setAttribute("cx", this.lineElement.x2.animVal.value);
        this.pointEles[1].setAttribute("cy", this.lineElement.y2.animVal.value);
    };
    LineControl.prototype.setEvent = function (pointEle, xName, yName) {
        var _this = this;
        pointEle.addEventListener("click", function (e) { e.stopPropagation(); }, false);
        pointEle.addEventListener("mousedown", function (e) { _this.pointMouseDown(e, pointEle, xName, yName); }, false);
        pointEle.addEventListener("mousemove", function (e) { _this.pointMouseMove(e, pointEle, xName, yName); }, false);
        pointEle.addEventListener("mouseup", function (e) { _this.pointMouseUp(e, pointEle); }, false);
    };
    LineControl.prototype.pointMouseDown = function (e, pointEle, xName, yName) {
        e.stopPropagation();
        this.moving = true;
        this.startX = e.clientX;
        this.startY = e.clientY;
        this.valueX = parseInt(this.lineElement.getAttribute(xName));
        this.valueY = parseInt(this.lineElement.getAttribute(yName));
        if (pointEle.setCapture)
            pointEle.setCapture();
        else if (window.captureEvents)
            window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);
    };
    LineControl.prototype.pointMouseMove = function (e, pointEle, xName, yName) {
        if (this.moving) {
            e.stopPropagation();
            pointEle.setAttribute("cx", this.valueX + e.clientX - this.startX);
            pointEle.setAttribute("cy", this.valueY + e.clientY - this.startY);
            this.lineElement.setAttribute(xName, (this.valueX + e.clientX - this.startX));
            this.lineElement.setAttribute(yName, (this.valueY + e.clientY - this.startY));
            this.virtualLineElement.setAttribute(xName, (this.valueX + e.clientX - this.startX));
            this.virtualLineElement.setAttribute(yName, (this.valueY + e.clientY - this.startY));
        }
    };
    LineControl.prototype.pointMouseUp = function (e, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            this.moving = false;
            pointEle.releaseCapture();
        }
    };
    return LineControl;
}(EditorControl));
var RectControl = (function (_super) {
    __extends(RectControl, _super);
    function RectControl(element) {
        var _this = _super.call(this, element ? element : document.createElementNS('http://www.w3.org/2000/svg', 'rect')) || this;
        _this.moving = false;
        _this.startX = 0;
        _this.startY = 0;
        _this.rectElement = _this.element;
        _this.rectElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;');
        return _this;
    }
    Object.defineProperty(RectControl.prototype, "rect", {
        get: function () {
            var myrect = this.rectElement.getBBox();
            return {
                x: myrect.x,
                y: myrect.y,
                width: myrect.width,
                height: myrect.height
            };
        },
        set: function (v) {
            this.rectElement.setAttribute("x", v.x);
            this.rectElement.setAttribute("y", v.y);
            this.rectElement.setAttribute("width", v.width);
            this.rectElement.setAttribute("height", v.height);
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RectControl.prototype, "strokeWidth", {
        get: function () {
            return this.rectElement.style.strokeWidth;
        },
        set: function (v) {
            this.rectElement.style.strokeWidth = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RectControl.prototype, "colorStroke", {
        get: function () {
            return this.rectElement.style.stroke;
        },
        set: function (v) {
            this.rectElement.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RectControl.prototype, "colorFill", {
        get: function () {
            return this.rectElement.style.fill;
        },
        set: function (v) {
            this.rectElement.style.fill = v;
        },
        enumerable: true,
        configurable: true
    });
    RectControl.prototype.getPropertiesCaption = function () {
        return ["边框大小", "边框颜色", "填充颜色"];
    };
    RectControl.prototype.getProperties = function () {
        return ["strokeWidth", "colorStroke", "colorFill"];
    };
    RectControl.prototype.isIntersectWith = function (rect) {
        return this.isIntersect(this.rect, rect);
    };
    RectControl.prototype.onSelectedChange = function () {
        if (this.selected) {
            this.pRightBottom = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            this.pRightBottom.setAttribute("r", "5");
            this.pRightBottom.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:nwse-resize;');
            this.rectElement.parentElement.appendChild(this.pRightBottom);
            this.setEvent(this.pRightBottom);
            this.resetPointLocation();
        }
        else {
            this.rectElement.parentElement.removeChild(this.pRightBottom);
        }
    };
    RectControl.prototype.resetPointLocation = function () {
        var rect = this.rect;
        if (this.pRightBottom) {
            this.pRightBottom.setAttribute("cx", (rect.x + rect.width));
            this.pRightBottom.setAttribute("cy", (rect.y + rect.height));
        }
    };
    RectControl.prototype.setEvent = function (pointEle) {
        var _this = this;
        pointEle.addEventListener("click", function (e) { e.stopPropagation(); }, false);
        pointEle.addEventListener("mousedown", function (e) { _this.pointMouseDown(e, pointEle); }, false);
        pointEle.addEventListener("mousemove", function (e) { _this.pointMouseMove(e, pointEle); }, false);
        pointEle.addEventListener("mouseup", function (e) { _this.pointMouseUp(e, pointEle); }, false);
    };
    RectControl.prototype.pointMouseDown = function (e, pointEle) {
        e.stopPropagation();
        this.moving = true;
        this.startX = e.clientX;
        this.startY = e.clientY;
        pointEle._valueX = parseInt(this.rectElement.getAttribute("x"));
        pointEle._valueY = parseInt(this.rectElement.getAttribute("y"));
        pointEle._valueWidth = parseInt(this.rectElement.getAttribute("width"));
        pointEle._valueHeight = parseInt(this.rectElement.getAttribute("height"));
        if (pointEle.setCapture)
            pointEle.setCapture();
        else if (window.captureEvents)
            window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);
    };
    RectControl.prototype.pointMouseMove = function (e, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            this.rectElement.setAttribute("width", Math.max(15, pointEle._valueWidth + (e.clientX - this.startX)));
            this.rectElement.setAttribute("height", Math.max(15, pointEle._valueHeight + (e.clientY - this.startY)));
            this.resetPointLocation();
        }
    };
    RectControl.prototype.pointMouseUp = function (e, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            this.moving = false;
            pointEle.releaseCapture();
        }
    };
    RectControl.prototype.onBeginMoving = function () {
        this.rectElement._x = parseInt(this.rectElement.getAttribute("x"));
        this.rectElement._y = parseInt(this.rectElement.getAttribute("y"));
    };
    RectControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x = (this.rectElement._x + nowX - downX);
        var y = (this.rectElement._y + nowY - downY);
        this.rectElement.setAttribute("x", x);
        this.rectElement.setAttribute("y", y);
        if (this.selected) {
            this.resetPointLocation();
        }
    };
    RectControl.prototype.onEndMoving = function () {
    };
    return RectControl;
}(EditorControl));
var EllipseControl = (function (_super) {
    __extends(EllipseControl, _super);
    function EllipseControl() {
        var _this = _super.call(this, document.createElementNS('http://www.w3.org/2000/svg', 'ellipse')) || this;
        _this.pointEles = [];
        _this.moving = false;
        _this.startX = 0;
        _this.startY = 0;
        _this.rootElement = _this.element;
        _this.rootElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;');
        return _this;
    }
    Object.defineProperty(EllipseControl.prototype, "rect", {
        get: function () {
            var myrect = {
                x: parseInt(this.rootElement.getAttribute("cx")) - parseInt(this.rootElement.getAttribute("rx")),
                y: parseInt(this.rootElement.getAttribute("cy")) - parseInt(this.rootElement.getAttribute("ry")),
                width: parseInt(this.rootElement.getAttribute("rx")) * 2,
                height: parseInt(this.rootElement.getAttribute("ry")) * 2,
            };
            return myrect;
        },
        set: function (v) {
            var rx = v.width / 2;
            var ry = v.height / 2;
            this.rootElement.setAttribute("cx", v.x + rx);
            this.rootElement.setAttribute("cy", v.y + ry);
            this.rootElement.setAttribute("rx", rx);
            this.rootElement.setAttribute("ry", ry);
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EllipseControl.prototype, "strokeWidth", {
        get: function () {
            return this.rootElement.style.strokeWidth;
        },
        set: function (v) {
            this.rootElement.style.strokeWidth = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EllipseControl.prototype, "colorStroke", {
        get: function () {
            return this.rootElement.style.stroke;
        },
        set: function (v) {
            this.rootElement.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EllipseControl.prototype, "colorFill", {
        get: function () {
            return this.rootElement.style.fill;
        },
        set: function (v) {
            this.rootElement.style.fill = v;
        },
        enumerable: true,
        configurable: true
    });
    EllipseControl.prototype.getPropertiesCaption = function () {
        return ["边框大小", "边框颜色", "填充颜色"];
    };
    EllipseControl.prototype.getProperties = function () {
        return ["strokeWidth", "colorStroke", "colorFill"];
    };
    EllipseControl.prototype.isIntersectWith = function (rect) {
        return this.isIntersect(this.rect, rect);
    };
    EllipseControl.prototype.onSelectedChange = function () {
        var _this = this;
        if (this.selected) {
            var pRightBottom = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            pRightBottom.setAttribute("r", "5");
            pRightBottom.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:nwse-resize;');
            pRightBottom._moveFunc = function (ele, x, y) {
                _this.rootElement.setAttribute("rx", Math.max(5, ele._value_rx + (x - ele._startX)));
                _this.rootElement.setAttribute("ry", Math.max(5, ele._value_ry + (y - ele._startY)));
            };
            this.rootElement.parentElement.appendChild(pRightBottom);
            this.pointEles.push(pRightBottom);
            var pCenterBottom = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            pCenterBottom.setAttribute("r", "5");
            pCenterBottom.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:ns-resize;');
            pCenterBottom._moveFunc = function (ele, x, y) {
                _this.rootElement.setAttribute("ry", Math.max(5, ele._value_ry + (y - ele._startY)));
            };
            this.rootElement.parentElement.appendChild(pCenterBottom);
            this.pointEles.push(pCenterBottom);
            for (var i = 0; i < this.pointEles.length; i++) {
                this.setEvent(this.pointEles[i]);
            }
            this.resetPointLocation();
        }
        else {
            for (var i = 0; i < this.pointEles.length; i++) {
                this.rootElement.parentElement.removeChild(this.pointEles[i]);
            }
            this.pointEles = [];
        }
    };
    EllipseControl.prototype.resetPointLocation = function () {
        this.pointEles[0].setAttribute("cx", (this.rootElement.cx.animVal.value + this.rootElement.rx.animVal.value));
        this.pointEles[0].setAttribute("cy", (this.rootElement.cy.animVal.value + this.rootElement.ry.animVal.value));
        this.pointEles[1].setAttribute("cx", (this.rootElement.cx.animVal.value));
        this.pointEles[1].setAttribute("cy", (this.rootElement.cy.animVal.value + this.rootElement.ry.animVal.value));
    };
    EllipseControl.prototype.setEvent = function (pointEle) {
        var _this = this;
        pointEle.addEventListener("click", function (e) { e.stopPropagation(); }, false);
        pointEle.addEventListener("mousedown", function (e) { _this.pointMouseDown(e, pointEle); }, false);
        pointEle.addEventListener("mousemove", function (e) { _this.pointMouseMove(e, pointEle); }, false);
        pointEle.addEventListener("mouseup", function (e) { _this.pointMouseUp(e, pointEle); }, false);
    };
    EllipseControl.prototype.pointMouseDown = function (e, pointEle) {
        e.stopPropagation();
        this.moving = true;
        pointEle._startX = e.clientX;
        pointEle._startY = e.clientY;
        pointEle._value_cx = this.rootElement.cx.animVal.value;
        pointEle._value_cy = this.rootElement.cy.animVal.value;
        pointEle._value_rx = this.rootElement.rx.animVal.value;
        pointEle._value_ry = this.rootElement.ry.animVal.value;
        if (pointEle.setCapture)
            pointEle.setCapture();
        else if (window.captureEvents)
            window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);
    };
    EllipseControl.prototype.pointMouseMove = function (e, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            pointEle._moveFunc(pointEle, e.clientX, e.clientY);
            this.resetPointLocation();
        }
    };
    EllipseControl.prototype.pointMouseUp = function (e, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            this.moving = false;
            pointEle.releaseCapture();
        }
    };
    EllipseControl.prototype.onBeginMoving = function () {
        this.rootElement._cx = parseInt(this.rootElement.getAttribute("cx"));
        this.rootElement._cy = parseInt(this.rootElement.getAttribute("cy"));
    };
    EllipseControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x = (this.rootElement._cx + nowX - downX);
        var y = (this.rootElement._cy + nowY - downY);
        this.rootElement.setAttribute("cx", x);
        this.rootElement.setAttribute("cy", y);
        if (this.selected) {
            this.resetPointLocation();
        }
    };
    EllipseControl.prototype.onEndMoving = function () {
    };
    return EllipseControl;
}(EditorControl));
var CircleControl = (function (_super) {
    __extends(CircleControl, _super);
    function CircleControl() {
        var _this = _super.call(this, document.createElementNS('http://www.w3.org/2000/svg', 'circle')) || this;
        _this.pointEles = [];
        _this.moving = false;
        _this.startX = 0;
        _this.startY = 0;
        _this.rootElement = _this.element;
        _this.rootElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;');
        return _this;
    }
    Object.defineProperty(CircleControl.prototype, "rect", {
        get: function () {
            var myrect = {
                x: parseInt(this.rootElement.getAttribute("cx")) - parseInt(this.rootElement.getAttribute("r")),
                y: parseInt(this.rootElement.getAttribute("cy")) - parseInt(this.rootElement.getAttribute("r")),
                width: parseInt(this.rootElement.getAttribute("r")) * 2,
                height: parseInt(this.rootElement.getAttribute("r")) * 2,
            };
            return myrect;
        },
        set: function (v) {
            var r = v.width / 2;
            this.rootElement.setAttribute("cx", v.x + r);
            this.rootElement.setAttribute("cy", v.y + r);
            this.rootElement.setAttribute("r", r);
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CircleControl.prototype, "strokeWidth", {
        get: function () {
            return this.rootElement.style.strokeWidth;
        },
        set: function (v) {
            this.rootElement.style.strokeWidth = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CircleControl.prototype, "colorStroke", {
        get: function () {
            return this.rootElement.style.stroke;
        },
        set: function (v) {
            this.rootElement.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CircleControl.prototype, "colorFill", {
        get: function () {
            return this.rootElement.style.fill;
        },
        set: function (v) {
            this.rootElement.style.fill = v;
        },
        enumerable: true,
        configurable: true
    });
    CircleControl.prototype.getPropertiesCaption = function () {
        return ["边框大小", "边框颜色", "填充颜色"];
    };
    CircleControl.prototype.getProperties = function () {
        return ["strokeWidth", "colorStroke", "colorFill"];
    };
    CircleControl.prototype.isIntersectWith = function (rect) {
        return this.isIntersect(this.rect, rect);
    };
    CircleControl.prototype.onSelectedChange = function () {
        var _this = this;
        if (this.selected) {
            var pRightBottom = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            pRightBottom.setAttribute("r", "5");
            pRightBottom.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:nwse-resize;');
            pRightBottom._moveFunc = function (ele, x, y) {
                _this.rootElement.setAttribute("r", Math.max(5, ele._value_r + (x - ele._startX)));
            };
            this.rootElement.parentElement.appendChild(pRightBottom);
            this.pointEles.push(pRightBottom);
            for (var i = 0; i < this.pointEles.length; i++) {
                this.setEvent(this.pointEles[i]);
            }
            this.resetPointLocation();
        }
        else {
            for (var i = 0; i < this.pointEles.length; i++) {
                this.rootElement.parentElement.removeChild(this.pointEles[i]);
            }
            this.pointEles = [];
        }
    };
    CircleControl.prototype.resetPointLocation = function () {
        this.pointEles[0].setAttribute("cx", (this.rootElement.cx.animVal.value + this.rootElement.r.animVal.value));
        this.pointEles[0].setAttribute("cy", (this.rootElement.cy.animVal.value + this.rootElement.r.animVal.value));
    };
    CircleControl.prototype.setEvent = function (pointEle) {
        var _this = this;
        pointEle.addEventListener("click", function (e) { e.stopPropagation(); }, false);
        pointEle.addEventListener("mousedown", function (e) { _this.pointMouseDown(e, pointEle); }, false);
        pointEle.addEventListener("mousemove", function (e) { _this.pointMouseMove(e, pointEle); }, false);
        pointEle.addEventListener("mouseup", function (e) { _this.pointMouseUp(e, pointEle); }, false);
    };
    CircleControl.prototype.pointMouseDown = function (e, pointEle) {
        e.stopPropagation();
        this.moving = true;
        pointEle._startX = e.clientX;
        pointEle._startY = e.clientY;
        pointEle._value_cx = this.rootElement.cx.animVal.value;
        pointEle._value_cy = this.rootElement.cy.animVal.value;
        pointEle._value_r = this.rootElement.r.animVal.value;
        if (pointEle.setCapture)
            pointEle.setCapture();
        else if (window.captureEvents)
            window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);
    };
    CircleControl.prototype.pointMouseMove = function (e, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            pointEle._moveFunc(pointEle, e.clientX, e.clientY);
            this.resetPointLocation();
        }
    };
    CircleControl.prototype.pointMouseUp = function (e, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            this.moving = false;
            pointEle.releaseCapture();
        }
    };
    CircleControl.prototype.onBeginMoving = function () {
        this.rootElement._cx = parseInt(this.rootElement.getAttribute("cx"));
        this.rootElement._cy = parseInt(this.rootElement.getAttribute("cy"));
    };
    CircleControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x = (this.rootElement._cx + nowX - downX);
        var y = (this.rootElement._cy + nowY - downY);
        this.rootElement.setAttribute("cx", x);
        this.rootElement.setAttribute("cy", y);
        if (this.selected) {
            this.resetPointLocation();
        }
    };
    CircleControl.prototype.onEndMoving = function () {
    };
    return CircleControl;
}(EditorControl));
var ImageControl = (function (_super) {
    __extends(ImageControl, _super);
    function ImageControl() {
        var _this = _super.call(this, document.createElementNS('http://www.w3.org/2000/svg', 'image')) || this;
        _this.imgElement = _this.element;
        return _this;
    }
    Object.defineProperty(ImageControl.prototype, "imgDefault", {
        get: function () {
            return this.imgElement.href.baseVal;
        },
        set: function (v) {
            this.imgElement.href.baseVal = v;
        },
        enumerable: true,
        configurable: true
    });
    ImageControl.prototype.getPropertiesCaption = function () {
        return ["图片"];
    };
    ImageControl.prototype.getProperties = function () {
        return ["imgDefault"];
    };
    return ImageControl;
}(RectControl));
var TextControl = (function (_super) {
    __extends(TextControl, _super);
    function TextControl() {
        var _this = _super.call(this, document.createElementNS('http://www.w3.org/2000/svg', 'text')) || this;
        _this.textElement = _this.element;
        _this.textElement.textContent = "Text";
        _this.textElement.setAttribute('style', 'fill:#111111;cursor:default;-moz-user-select:none;');
        _this.textElement.setAttribute('font-size', "16");
        return _this;
    }
    Object.defineProperty(TextControl.prototype, "text", {
        get: function () {
            return this.textElement.textContent;
        },
        set: function (v) {
            if (v != this.textElement.textContent) {
                this.textElement.textContent = v;
                this.resetPointLocation();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TextControl.prototype, "size", {
        get: function () {
            return parseInt(this.textElement.getAttribute("font-size"));
        },
        set: function (v) {
            this.textElement.setAttribute("font-size", v);
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TextControl.prototype, "colorFill", {
        get: function () {
            return this.textElement.style.fill;
        },
        set: function (v) {
            this.textElement.style.fill = v;
        },
        enumerable: true,
        configurable: true
    });
    TextControl.prototype.getPropertiesCaption = function () {
        return ["文字", "大小", "颜色"];
    };
    TextControl.prototype.getProperties = function () {
        return ["text", "size", "colorFill"];
    };
    Object.defineProperty(TextControl.prototype, "rect", {
        get: function () {
            var myrect = this.textElement.getBBox();
            return {
                x: myrect.x,
                y: myrect.y,
                width: myrect.width,
                height: myrect.height
            };
        },
        set: function (v) {
            var y = parseInt(this.textElement.getAttribute("y"));
            var otherY = this.textElement.getBBox().y;
            this.rectElement.setAttribute("x", v.x);
            this.rectElement.setAttribute("y", (v.y + y - otherY));
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    TextControl.prototype.onSelectedChange = function () {
        if (this.selected) {
            var myrect = this.rect;
            this.selectingElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            this.selectingElement.setAttribute('x', (myrect.x - 5));
            this.selectingElement.setAttribute('y', (myrect.y - 5));
            this.selectingElement.setAttribute('width', (myrect.width + 10));
            this.selectingElement.setAttribute('height', (myrect.height + 10));
            this.selectingElement.setAttribute('style', 'fill:none;stroke:black;stroke-width:1;stroke-dasharray:2;stroke-dashoffset:2;');
            this.selectingElement.onmousedown = function (e) {
                e.stopPropagation();
            };
            this.rectElement.parentElement.appendChild(this.selectingElement);
        }
        else {
            this.rectElement.parentElement.removeChild(this.selectingElement);
        }
    };
    TextControl.prototype.resetPointLocation = function () {
        var myrect = this.rect;
        this.selectingElement.setAttribute('x', (myrect.x - 5));
        this.selectingElement.setAttribute('y', (myrect.y - 5));
        this.selectingElement.setAttribute('width', (myrect.width + 10));
        this.selectingElement.setAttribute('height', (myrect.height + 10));
    };
    return TextControl;
}(RectControl));
var CylinderControl = (function (_super) {
    __extends(CylinderControl, _super);
    function CylinderControl() {
        var _this = _super.call(this, document.createElementNS('http://www.w3.org/2000/svg', 'g')) || this;
        _this._value = 50;
        _this._max = 100;
        _this._min = 0;
        _this.moving = false;
        _this.startX = 0;
        _this.startY = 0;
        _this.rectElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
        _this.rectElement.setAttribute("rx", "200000");
        _this.rectElement.setAttribute("ry", "20");
        _this.rectElement.setAttribute('width', "0");
        _this.rectElement.setAttribute('height', "0");
        _this.rectElement.setAttribute('style', 'fill:#ffffff;stroke:#aaaaaa;stroke-width:1;');
        _this.cylinderElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
        _this.cylinderElement.setAttribute("rx", "6");
        _this.cylinderElement.setAttribute("ry", "6");
        _this.cylinderElement.setAttribute('style', 'fill:#00BF00;stroke:none;');
        _this.element.appendChild(_this.rectElement);
        _this.element.appendChild(_this.cylinderElement);
        return _this;
    }
    Object.defineProperty(CylinderControl.prototype, "value", {
        get: function () {
            return this._value;
        },
        set: function (v) {
            v = parseFloat(v);
            if (v != this._value) {
                this._value = v;
                this.resetCylinder(this.rect);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CylinderControl.prototype, "max", {
        get: function () {
            return this._max;
        },
        set: function (v) {
            this._max = parseFloat(v);
            if (this._max <= this._min)
                this._max = this._min + 1;
            this.resetCylinder(this.rect);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CylinderControl.prototype, "min", {
        get: function () {
            return this._min;
        },
        set: function (v) {
            this._min = parseFloat(v);
            if (this._min >= this._max)
                this._max = this._max - 1;
            this.resetCylinder(this.rect);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CylinderControl.prototype, "rect", {
        get: function () {
            var myrect = this.rectElement.getBBox();
            return {
                x: myrect.x,
                y: myrect.y,
                width: myrect.width,
                height: myrect.height
            };
        },
        set: function (v) {
            this.rectElement.setAttribute("x", v.x);
            this.rectElement.setAttribute("y", v.y);
            this.rectElement.setAttribute("width", v.width);
            this.rectElement.setAttribute("height", v.height);
            this.resetCylinder(v);
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CylinderControl.prototype, "strokeWidth", {
        get: function () {
            return this.rectElement.style.strokeWidth;
        },
        set: function (v) {
            this.rectElement.style.strokeWidth = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CylinderControl.prototype, "colorStroke", {
        get: function () {
            return this.rectElement.style.stroke;
        },
        set: function (v) {
            this.rectElement.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CylinderControl.prototype, "colorFill", {
        get: function () {
            return this.cylinderElement.style.fill;
        },
        set: function (v) {
            this.cylinderElement.style.fill = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CylinderControl.prototype, "colorBg", {
        get: function () {
            return this.rectElement.style.fill;
        },
        set: function (v) {
            this.rectElement.style.fill = v;
        },
        enumerable: true,
        configurable: true
    });
    CylinderControl.prototype.getPropertiesCaption = function () {
        return ["边框大小", "边框颜色", "底色", "填充颜色", "值", "最大值", "最小值"];
    };
    CylinderControl.prototype.getProperties = function () {
        return ["strokeWidth", "colorStroke", "colorBg", "colorFill", "value", "max", "min"];
    };
    CylinderControl.prototype.resetCylinder = function (rect) {
        var ctrlHeight = rect.height - 40;
        var myheight = parseInt((((this.value - this.min) * ctrlHeight) / (this.max - this.min)));
        if (myheight < 0)
            myheight = 0;
        myheight = Math.min(ctrlHeight, myheight);
        this.cylinderElement.setAttribute("x", rect.x + 10);
        this.cylinderElement.setAttribute("y", (rect.y + 20 + ctrlHeight - myheight));
        this.cylinderElement.setAttribute("width", (rect.width - 20));
        this.cylinderElement.setAttribute("height", myheight);
    };
    CylinderControl.prototype.isIntersectWith = function (rect) {
        return this.isIntersect(this.rect, rect);
    };
    CylinderControl.prototype.onSelectedChange = function () {
        if (this.selected) {
            this.pRightBottom = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            this.pRightBottom.setAttribute("r", "5");
            this.pRightBottom.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:nwse-resize;');
            this.rectElement.parentElement.appendChild(this.pRightBottom);
            this.setEvent(this.pRightBottom);
            this.resetPointLocation();
        }
        else {
            this.rectElement.parentElement.removeChild(this.pRightBottom);
        }
    };
    CylinderControl.prototype.resetPointLocation = function () {
        var rect = this.rect;
        if (this.pRightBottom) {
            this.pRightBottom.setAttribute("cx", (rect.x + rect.width));
            this.pRightBottom.setAttribute("cy", (rect.y + rect.height));
        }
    };
    CylinderControl.prototype.setEvent = function (pointEle) {
        var _this = this;
        pointEle.addEventListener("click", function (e) { e.stopPropagation(); }, false);
        pointEle.addEventListener("mousedown", function (e) { _this.pointMouseDown(e, pointEle); }, false);
        pointEle.addEventListener("mousemove", function (e) { _this.pointMouseMove(e, pointEle); }, false);
        pointEle.addEventListener("mouseup", function (e) { _this.pointMouseUp(e, pointEle); }, false);
    };
    CylinderControl.prototype.pointMouseDown = function (e, pointEle) {
        e.stopPropagation();
        this.moving = true;
        this.startX = e.clientX;
        this.startY = e.clientY;
        pointEle._valueX = parseInt(this.rectElement.getAttribute("x"));
        pointEle._valueY = parseInt(this.rectElement.getAttribute("y"));
        pointEle._valueWidth = parseInt(this.rectElement.getAttribute("width"));
        pointEle._valueHeight = parseInt(this.rectElement.getAttribute("height"));
        if (pointEle.setCapture)
            pointEle.setCapture();
        else if (window.captureEvents)
            window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);
    };
    CylinderControl.prototype.pointMouseMove = function (e, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            var width = Math.max(15, pointEle._valueWidth + (e.clientX - this.startX));
            var height = Math.max(15, pointEle._valueHeight + (e.clientY - this.startY));
            this.rect = {
                x: pointEle._valueX,
                y: pointEle._valueY,
                width: width,
                height: height
            };
            this.resetPointLocation();
        }
    };
    CylinderControl.prototype.pointMouseUp = function (e, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            this.moving = false;
            pointEle.releaseCapture();
        }
    };
    CylinderControl.prototype.onBeginMoving = function () {
        this.rectElement._rect = this.rect;
    };
    CylinderControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x = (this.rectElement._rect.x + nowX - downX);
        var y = (this.rectElement._rect.y + nowY - downY);
        this.rectElement.setAttribute("x", x);
        this.rectElement.setAttribute("y", y);
        this.resetCylinder({ x: x, y: y, width: this.rectElement._rect.width, height: this.rectElement._rect.height });
        if (this.selected) {
            this.resetPointLocation();
        }
    };
    CylinderControl.prototype.onEndMoving = function () {
    };
    return CylinderControl;
}(EditorControl));
var TrendControl = (function (_super) {
    __extends(TrendControl, _super);
    function TrendControl() {
        var _this = _super.call(this, document.createElementNS('http://www.w3.org/2000/svg', 'g')) || this;
        _this.values = [];
        _this._value = 50;
        _this._max = 100;
        _this._min = 0;
        _this.moving = false;
        _this.startX = 0;
        _this.startY = 0;
        _this.rectElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
        _this.element.appendChild(_this.rectElement);
        _this.rectElement.setAttribute('style', 'fill:#000000;stroke:none;');
        _this.line_left_Ele = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        _this.line_left_Ele.setAttribute('style', 'stroke:#ffffff;stroke-width:1;');
        _this.element.appendChild(_this.line_left_Ele);
        _this.line_bottom_Ele = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        _this.line_bottom_Ele.setAttribute('style', 'stroke:#ffffff;stroke-width:1;');
        _this.element.appendChild(_this.line_bottom_Ele);
        _this.pathElement = document.createElementNS('http://www.w3.org/2000/svg', 'path');
        _this.pathElement.setAttribute('style', 'stroke:#ffffff;stroke-width:1;fill:none;');
        _this.pathElement.setAttribute("transform", "translate(0 0)");
        _this.element.appendChild(_this.pathElement);
        return _this;
    }
    Object.defineProperty(TrendControl.prototype, "value", {
        get: function () {
            return this._value;
        },
        set: function (v) {
            v = parseFloat(v);
            if (v != this._value) {
                this._value = v;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "max", {
        get: function () {
            return this._max;
        },
        set: function (v) {
            this._max = parseFloat(v);
            if (this._max <= this._min)
                this._max = this._min + 1;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "min", {
        get: function () {
            return this._min;
        },
        set: function (v) {
            this._min = parseFloat(v);
            if (this._min >= this._max)
                this._max = this._max - 1;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "rect", {
        get: function () {
            var myrect = this.rectElement.getBBox();
            return {
                x: myrect.x,
                y: myrect.y,
                width: myrect.width,
                height: myrect.height
            };
        },
        set: function (v) {
            this.rectElement.setAttribute("x", v.x);
            this.rectElement.setAttribute("y", v.y);
            this.rectElement.setAttribute("width", v.width);
            this.rectElement.setAttribute("height", v.height);
            this.pathElement.setAttribute("transform", "translate(" + v.x + " " + v.y + ")");
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorFill", {
        get: function () {
            return this.rectElement.style.fill;
        },
        set: function (v) {
            this.rectElement.style.fill = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorLineLeftBottom", {
        get: function () {
            return this.line_left_Ele.style.stroke;
        },
        set: function (v) {
            this.line_left_Ele.style.stroke = v;
            this.line_bottom_Ele.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorLine", {
        get: function () {
            return this.pathElement.style.stroke;
        },
        set: function (v) {
            this.pathElement.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    TrendControl.prototype.getPropertiesCaption = function () {
        return ["背景颜色", "值", "量程线颜色", "趋势颜色"];
    };
    TrendControl.prototype.getProperties = function () {
        return ["colorFill", "value", "colorLineLeftBottom", "colorLine"];
    };
    TrendControl.prototype.isIntersectWith = function (rect) {
        return this.isIntersect(this.rect, rect);
    };
    TrendControl.prototype.onSelectedChange = function () {
        if (this.selected) {
            this.pRightBottom = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            this.pRightBottom.setAttribute("r", "5");
            this.pRightBottom.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:nwse-resize;');
            this.rectElement.parentElement.appendChild(this.pRightBottom);
            this.setEvent(this.pRightBottom);
            this.resetPointLocation();
        }
        else {
            this.rectElement.parentElement.removeChild(this.pRightBottom);
        }
    };
    TrendControl.prototype.run = function () {
        var _this = this;
        if (this.values.length > 0)
            this.value = this.values[this.values.length - 1].value;
        else
            this.value = this.min;
        this.reDrawTrend();
        this.element._interval = setInterval(function () { return _this.checkValueChange(); }, 1000);
    };
    TrendControl.prototype.checkValueChange = function () {
        this.values.push({
            value: this.value,
            time: new Date().getTime()
        });
        this.reDrawTrend();
    };
    TrendControl.prototype.reDrawTrend = function () {
        var rect = this.rect;
        var width = rect.width - 20 - 2;
        var now = new Date().getTime();
        var dataStr = "";
        var deleteToIndex = -1;
        for (var i = this.values.length - 1; i >= 0; i--) {
            var x = rect.width - 10 - ((now - this.values[i].time) / 1000) * 2;
            if (x < 10) {
                deleteToIndex = i;
                break;
            }
            var percent = 1 - (this.values[i].value - this.min) / (this.max - this.min);
            var y = 10 + (rect.height - 20) * percent;
            if (y < 10)
                y = 10;
            else if (y > rect.height - 10)
                y = rect.height - 10;
            if (dataStr.length == 0)
                dataStr += "M";
            else
                dataStr += "L";
            dataStr += x + " " + y + " ";
        }
        if (deleteToIndex >= 0) {
            this.values.splice(0, deleteToIndex + 1);
        }
        this.pathElement.setAttribute("d", dataStr);
    };
    TrendControl.prototype.resetPointLocation = function () {
        var rect = this.rect;
        if (this.pRightBottom) {
            this.pRightBottom.setAttribute("cx", (rect.x + rect.width));
            this.pRightBottom.setAttribute("cy", (rect.y + rect.height));
        }
        this.line_left_Ele.setAttribute("x1", (rect.x + 10));
        this.line_left_Ele.setAttribute("y1", (rect.y + 10));
        this.line_left_Ele.setAttribute("x2", (rect.x + 10));
        this.line_left_Ele.setAttribute("y2", (rect.y + rect.height - 10));
        this.line_bottom_Ele.setAttribute("x1", (rect.x + 10));
        this.line_bottom_Ele.setAttribute("y1", (rect.y + rect.height - 10));
        this.line_bottom_Ele.setAttribute("x2", (rect.x + rect.width - 10));
        this.line_bottom_Ele.setAttribute("y2", (rect.y + rect.height - 10));
    };
    TrendControl.prototype.setEvent = function (pointEle) {
        var _this = this;
        pointEle.addEventListener("click", function (e) { e.stopPropagation(); }, false);
        pointEle.addEventListener("mousedown", function (e) { _this.pointMouseDown(e, pointEle); }, false);
        pointEle.addEventListener("mousemove", function (e) { _this.pointMouseMove(e, pointEle); }, false);
        pointEle.addEventListener("mouseup", function (e) { _this.pointMouseUp(e, pointEle); }, false);
    };
    TrendControl.prototype.pointMouseDown = function (e, pointEle) {
        e.stopPropagation();
        this.moving = true;
        this.startX = e.clientX;
        this.startY = e.clientY;
        pointEle._valueX = parseInt(this.rectElement.getAttribute("x"));
        pointEle._valueY = parseInt(this.rectElement.getAttribute("y"));
        pointEle._valueWidth = parseInt(this.rectElement.getAttribute("width"));
        pointEle._valueHeight = parseInt(this.rectElement.getAttribute("height"));
        if (pointEle.setCapture)
            pointEle.setCapture();
        else if (window.captureEvents)
            window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);
    };
    TrendControl.prototype.pointMouseMove = function (e, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            this.rectElement.setAttribute("width", Math.max(15, pointEle._valueWidth + (e.clientX - this.startX)));
            this.rectElement.setAttribute("height", Math.max(15, pointEle._valueHeight + (e.clientY - this.startY)));
            this.resetPointLocation();
        }
    };
    TrendControl.prototype.pointMouseUp = function (e, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            this.moving = false;
            pointEle.releaseCapture();
        }
    };
    TrendControl.prototype.onBeginMoving = function () {
        this.rectElement._x = parseInt(this.rectElement.getAttribute("x"));
        this.rectElement._y = parseInt(this.rectElement.getAttribute("y"));
    };
    TrendControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x = (this.rectElement._x + nowX - downX);
        var y = (this.rectElement._y + nowY - downY);
        this.rectElement.setAttribute("x", x);
        this.rectElement.setAttribute("y", y);
        this.pathElement.setAttribute("transform", "translate(" + x + " " + y + ")");
        if (this.selected) {
            this.resetPointLocation();
        }
    };
    TrendControl.prototype.onEndMoving = function () {
    };
    return TrendControl;
}(EditorControl));
var GroupControl = (function (_super) {
    __extends(GroupControl, _super);
    function GroupControl(element) {
        var _this = _super.call(this, element) || this;
        _this.controls = [];
        _this.moving = false;
        _this.startX = 0;
        _this.startY = 0;
        _this.contentWidth = 0;
        _this.contentHeight = 0;
        element.setAttribute("transform", "translate(0 0) scale(1 1)");
        _this.groupElement = element;
        return _this;
    }
    GroupControl.prototype.removeControl = function (ctrl) {
        for (var i = 0; i < this.controls.length; i++) {
            if (this.controls[i] == ctrl) {
                this.groupElement.removeChild(ctrl.element);
                ctrl.isInGroup = false;
                ctrl.container = null;
                this.controls.splice(i, 1);
                break;
            }
        }
    };
    GroupControl.prototype.addControl = function (ctrl) {
        ctrl.isInGroup = true;
        ctrl.container = this;
        this.groupElement.appendChild(ctrl.element);
        this.controls.push(ctrl);
    };
    Object.defineProperty(GroupControl.prototype, "rect", {
        get: function () {
            var transform = this.groupElement.getAttribute("transform");
            var result = /translate\(([0-9]+) ([0-9]+)\)/.exec(transform);
            var myrect = {};
            myrect.x = parseInt(result[1]);
            myrect.y = parseInt(result[2]);
            result = /scale\(([0-9|\.]+) ([0-9|\.]+)\)/.exec(transform);
            var scalex = parseFloat(result[1]);
            var scaley = parseFloat(result[2]);
            this.contentWidth = 0;
            this.contentHeight = 0;
            for (var i = 0; i < this.controls.length; i++) {
                var ctrl = this.controls[i];
                var _rect = ctrl.rect;
                if (_rect.x + _rect.width > this.contentWidth)
                    this.contentWidth = _rect.x + _rect.width;
                if (_rect.y + _rect.height > this.contentHeight)
                    this.contentHeight = _rect.y + _rect.height;
            }
            if (!this.virtualRectElement) {
                this.virtualRectElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
                this.groupElement.appendChild(this.virtualRectElement);
                this.virtualRectElement.setAttribute('x', "0");
                this.virtualRectElement.setAttribute('y', "0");
                this.virtualRectElement.setAttribute('style', 'fill:#ffffff;fill-opacity:0.1;stroke:#cccccc;stroke-width:1;stroke-dasharray:2;stroke-dashoffset:2;');
            }
            this.virtualRectElement.setAttribute('width', this.contentWidth);
            this.virtualRectElement.setAttribute('height', this.contentHeight);
            myrect.width = parseInt((this.contentWidth * scalex));
            myrect.height = parseInt((this.contentHeight * scaley));
            this.lastRect = myrect;
            return myrect;
        },
        set: function (v) {
            if (this.contentWidth == 0) {
                var r = this.rect;
            }
            var scalex = parseFloat(v.width) / this.contentWidth;
            var scaley = parseFloat(v.height) / this.contentHeight;
            this.groupElement.setAttribute("transform", "translate(" + v.x + " " + v.y + ") scale(" + scalex + " " + scaley + ")");
            this.lastRect = v;
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    GroupControl.prototype.getPropertiesCaption = function () {
        return [];
    };
    GroupControl.prototype.getProperties = function () {
        return [];
    };
    GroupControl.prototype.isIntersectWith = function (rect) {
        return this.isIntersect(this.rect, rect);
    };
    GroupControl.prototype.onSelectedChange = function () {
        if (this.selected) {
            this.pRightBottom = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            this.pRightBottom.setAttribute("r", "5");
            this.pRightBottom.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:nwse-resize;');
            this.groupElement.parentElement.appendChild(this.pRightBottom);
            this.setEvent(this.pRightBottom);
            this.resetPointLocation();
        }
        else {
            this.groupElement.parentElement.removeChild(this.pRightBottom);
        }
    };
    GroupControl.prototype.resetPointLocation = function () {
        var rect = this.lastRect;
        if (this.pRightBottom) {
            this.pRightBottom.setAttribute("cx", (rect.x + rect.width));
            this.pRightBottom.setAttribute("cy", (rect.y + rect.height));
        }
    };
    GroupControl.prototype.setEvent = function (pointEle) {
        var _this = this;
        pointEle.addEventListener("click", function (e) { e.stopPropagation(); }, false);
        pointEle.addEventListener("mousedown", function (e) { _this.pointMouseDown(e, pointEle); }, false);
        pointEle.addEventListener("mousemove", function (e) { _this.pointMouseMove(e, pointEle); }, false);
        pointEle.addEventListener("mouseup", function (e) { _this.pointMouseUp(e, pointEle); }, false);
    };
    GroupControl.prototype.pointMouseDown = function (e, pointEle) {
        e.stopPropagation();
        this.moving = true;
        this.startX = e.clientX;
        this.startY = e.clientY;
        var rect = this.rect;
        pointEle._x = rect.x;
        pointEle._y = rect.y;
        pointEle._width = rect.width;
        pointEle._height = rect.height;
        if (pointEle.setCapture)
            pointEle.setCapture();
        else if (window.captureEvents)
            window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);
    };
    GroupControl.prototype.pointMouseMove = function (e, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            this.rect = {
                x: pointEle._x,
                y: pointEle._y,
                width: Math.max(15, pointEle._width + (e.clientX - this.startX)),
                height: Math.max(15, pointEle._height + (e.clientY - this.startY))
            };
            this.resetPointLocation();
        }
    };
    GroupControl.prototype.pointMouseUp = function (e, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            this.moving = false;
            pointEle.releaseCapture();
        }
    };
    GroupControl.prototype.onBeginMoving = function () {
        var rect = this.rect;
        this.groupElement._x = rect.x;
        this.groupElement._y = rect.y;
        this.groupElement._width = rect.width;
        this.groupElement._height = rect.height;
    };
    GroupControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x = (this.groupElement._x + nowX - downX);
        var y = (this.groupElement._y + nowY - downY);
        this.rect = { x: x, y: y, width: this.groupElement._width, height: this.groupElement._height };
        if (this.selected) {
            this.resetPointLocation();
        }
    };
    GroupControl.prototype.onEndMoving = function () {
    };
    return GroupControl;
}(EditorControl));
//# sourceMappingURL=EditorControls.js.map