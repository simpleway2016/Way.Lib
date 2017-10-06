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
        this._selected = false;
        this._moveAllSelectedControl = false;
        this.element = element;
        element._editorControl = this;
        this.element.addEventListener("dragstart", function (e) {
            e.preventDefault();
        }, false);
        this.element.addEventListener("click", function (e) {
            e.stopPropagation();
        }, false);
        this.element.addEventListener("dblclick", function (e) {
            e.stopPropagation();
            _this.showProperty();
        }, false);
        this.element.addEventListener("mousedown", function (e) {
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
var SVGContainerControl = (function (_super) {
    __extends(SVGContainerControl, _super);
    function SVGContainerControl(element) {
        var _this = _super.call(this, element) || this;
        _this.rootElement = element;
        return _this;
    }
    Object.defineProperty(SVGContainerControl.prototype, "colorBG", {
        get: function () {
            return this.rootElement.style.backgroundColor;
        },
        set: function (v) {
            if (v == "")
                v = "#FFFFFF";
            this.rootElement.style.backgroundColor = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SVGContainerControl.prototype, "imgBg", {
        get: function () {
            var url = this.rootElement.style.backgroundImage;
            if (url && url.length > 0) {
                url = url.substr(4, url.length - 5);
            }
            return url;
        },
        set: function (v) {
            if (v == "") {
                this.rootElement.style.backgroundImage = "";
            }
            else {
                this.rootElement.style.backgroundImage = "url(" + v + ")";
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SVGContainerControl.prototype, "bgWidth", {
        get: function () {
            var size = this.rootElement.style.backgroundSize.split(' ');
            return size[0];
        },
        set: function (v) {
            if (v.indexOf("%") < 0 && v.indexOf("px") < 0)
                v += "px";
            var size = this.rootElement.style.backgroundSize.split(' ');
            this.rootElement.style.backgroundSize = v + " " + size[1];
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SVGContainerControl.prototype, "bgHeight", {
        get: function () {
            var size = this.rootElement.style.backgroundSize.split(' ');
            return size[1];
        },
        set: function (v) {
            if (v.indexOf("%") < 0 && v.indexOf("px") < 0)
                v += "px";
            var size = this.rootElement.style.backgroundSize.split(' ');
            this.rootElement.style.backgroundSize = size[0] + " " + v;
        },
        enumerable: true,
        configurable: true
    });
    SVGContainerControl.prototype.getPropertiesCaption = function () {
        return ["底色", "背景图", "背景图宽", "背景图高"];
    };
    SVGContainerControl.prototype.getProperties = function () {
        return ["colorBG", "imgBg", "bgWidth", "bgHeight"];
    };
    Object.defineProperty(SVGContainerControl.prototype, "rect", {
        get: function () {
            return { x: 10, y: 10, width: 0, height: 0 };
        },
        enumerable: true,
        configurable: true
    });
    return SVGContainerControl;
}(EditorControl));
var LineControl = (function (_super) {
    __extends(LineControl, _super);
    function LineControl(element) {
        var _this = _super.call(this, element) || this;
        _this.pointEles = [];
        _this.moving = false;
        _this.lineElement = element;
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
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
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
        var _this = _super.call(this, element) || this;
        _this.moving = false;
        _this.startX = 0;
        _this.startY = 0;
        _this.rectElement = element;
        return _this;
    }
    Object.defineProperty(RectControl.prototype, "rect", {
        get: function () {
            var myrect = this.rectElement.getBBox();
            return myrect;
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
        this.pRightBottom.setAttribute("cx", (rect.x + rect.width));
        this.pRightBottom.setAttribute("cy", (rect.y + rect.height));
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
    function EllipseControl(element) {
        var _this = _super.call(this, element) || this;
        _this.pointEles = [];
        _this.moving = false;
        _this.startX = 0;
        _this.startY = 0;
        _this.rootElement = element;
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
            var myrect = this.rect;
            var x = parseInt(this.rootElement.getAttribute("cx")) + (v.x - myrect.x);
            this.rootElement.setAttribute("cx", x);
            var y = parseInt(this.rootElement.getAttribute("cy")) + (v.y - myrect.y);
            this.rootElement.setAttribute("cy", y);
            var rx = parseInt(this.rootElement.getAttribute("rx")) + (v.width - myrect.width) / 2;
            this.rootElement.setAttribute("rx", rx);
            var ry = parseInt(this.rootElement.getAttribute("ry")) + (v.height - myrect.height) / 2;
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
    function CircleControl(element) {
        var _this = _super.call(this, element) || this;
        _this.pointEles = [];
        _this.moving = false;
        _this.startX = 0;
        _this.startY = 0;
        _this.rootElement = element;
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
            var myrect = this.rect;
            var x = parseInt(this.rootElement.getAttribute("cx")) + (v.x - myrect.x);
            this.rootElement.setAttribute("cx", x);
            var y = parseInt(this.rootElement.getAttribute("cy")) + (v.y - myrect.y);
            this.rootElement.setAttribute("cy", y);
            var r = parseInt(this.rootElement.getAttribute("r")) + (v.width - myrect.width) / 2;
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
    function ImageControl(element) {
        var _this = _super.call(this, element) || this;
        _this.imgElement = element;
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
    function TextControl(element) {
        var _this = _super.call(this, element) || this;
        _this.textElement = element;
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
            var myrect = this.rectElement.getBBox();
            return myrect;
        },
        set: function (v) {
            this.rectElement.setAttribute("x", v.x);
            this.rectElement.setAttribute("y", v.y);
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
//# sourceMappingURL=EditorControls.js.map