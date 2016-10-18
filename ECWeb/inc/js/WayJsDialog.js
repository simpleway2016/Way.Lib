document.write('<scr' + 'ipt type="text/javascript" src="/inc/jquery/jquery-1.11.1.min.js"></scr' + 'ipt>');
function WayJsDialog() {
    var instance = this;
    var topDiv;//底层透明div
    var borderDiv;//边框div
    var titleDiv;//标题div
    var frameObj;
    var divcaption;

    this.onClosed = false;
    this.close = function () {
        topDiv.css("display", "none");
        if (currentDiv) {
            currentDiv.css("display", "none");
            currentDiv = false;
        }
        borderDiv.css("display", "none");
        if (instance.onClosed) {
            instance.onClosed();
        }
    }

    var currentDiv;
    this.showDiv = function (div, width, height) {
        currentDiv = $(div);
        try {
            var winWidth = $(window).innerWidth();
            var winheight = $(window).innerHeight();

            if (height > winheight)
                height = winheight;
            if (width > winWidth)
                width = winWidth;

            topDiv.css({ "width": winWidth + "px", "height": winheight + "px", "display": "block" });
            var top = (winheight - height) / 3;
            if (top < 0)
                top = 0;

            currentDiv.css({ "width": width, "height": height, "position": "fixed", "display": "block", "z-index": 999, "left": ($(window).innerWidth() - width) / 2, "top": top });

        }
        catch (e) {
            alert(e.message);
        }
    }

    this.show = function (url, title, width, height) {
        try {
            var winWidth = $(window).innerWidth();
            var winheight = $(window).innerHeight();

            if (height > winheight)
                height = winheight;
            if (width > winWidth)
                width = winWidth;

            divcaption.html(title);
            topDiv.css({ "width": winWidth + "px", "height": winheight + "px", "display": "block" });
            var top = (winheight - height) / 3;
            if (top < 0)
                top = 0;
            frameObj.attr("src", "about:blank");
            borderDiv.css({ "width": width, "height": height, "display": "block", "left": ($(window).innerWidth() - width) / 2, "top": top });
            frameObj.css({ "height": height - titleDiv.outerHeight() });
            frameObj.attr("src", url);
        }
        catch (e) {
            alert(e.message);
        }
    }

    var startx;
    var starty;
    var oldleft;
    var oldtop;
    var mousedowned = false;
    function mouseDown(event) {
        if (titleDiv[0].setCapture) {
            titleDiv[0].setCapture();
        } else if (window.captureEvents) {
            window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);
        }
        oldleft = parseInt(borderDiv.css("left").replace("px", ""));
        oldtop = parseInt(borderDiv.css("top").replace("px", ""));
        startx = event.clientX;
        starty = event.clientY;
        mousedowned = true;
        return false;
    }
    function mouseMove(event) {
        if (mousedowned) {
            borderDiv.css({ "left": oldleft + event.clientX - startx, "top": oldtop + event.clientY - starty });
            return false;
        }
    }
    function mouseUp(event) {
        if (mousedowned) {
            mousedowned = false;
            if (titleDiv[0].releaseCapture) {    // Internet Explorer
                titleDiv[0].releaseCapture();
            } else if (window.captureEvents) {
                window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);
            }
            return false;
        }

    }
    function _windowload() {
        try {
            topDiv = $(document.createElement("DIV"));
            //底层透明样式
            topDiv.css({ "background-color": "#000", "opacity": 0.5, "display": "none", "position": "fixed", "left": 0, "top": 0, "z-index": 998 });
            document.body.appendChild(topDiv[0]);

            borderDiv = $(document.createElement("DIV"));
            document.body.appendChild(borderDiv[0]);

            titleDiv = $(document.createElement("DIV"));
            borderDiv[0].appendChild(titleDiv[0]);
            //边框样式
            borderDiv.css({ "background-color": "#ffffff", "position": "fixed", "display": "none", "left": 0, "top": 0, "z-index": 999, "border": "3px #c7e9ed solid ", "padding-bottom": "20px" });
            titleDiv.css({ "background-color": "#c7e9ed", "height": "28px", "width": "100%" });

            frameObj = $(document.createElement("IFRAME"));
            borderDiv[0].appendChild(frameObj[0]);
            frameObj.css({ "width": "100%", "border": "0", "overflow": "auto" });

            divcaption = $(document.createElement("DIV"));
            divcaption.css({ "float": "left" });
            titleDiv[0].appendChild(divcaption[0]);

            //关闭按钮
            var imgsrc = $(document.createElement("IMG"));
            imgsrc.attr("src", "/inc/imgs/dialog_close.png");
            imgsrc.css({ "float": "right", "cursor": "pointer", "padding-top": "2px", "padding-right": "5px" });

            titleDiv[0].appendChild(imgsrc[0]);
            imgsrc.click(function () {
                instance.close();
                return false;
            });

            titleDiv.mousedown(mouseDown);
            titleDiv.mousemove(mouseMove);
            titleDiv.mouseup(mouseUp);

            topDiv.click(function () {
                instance.close();
                return false;
            });
        }
        catch (e) {
            alert(e.message);
        }
    }

    if (window.addEventListener) {
        window.addEventListener("load", _windowload, false);
    }
    else {
        window.attachEvent("onload", _windowload);
    }
}

