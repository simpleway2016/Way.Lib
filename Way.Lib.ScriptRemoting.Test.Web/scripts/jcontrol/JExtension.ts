
(<any>HTMLElement).prototype.getContainer = function () {
    var parent = this;
    while (parent) {
        if (parent.JControl)
            return parent.JControl;
        else
            parent = parent.parentElement;
    }
    return null;
};

//往上查找具有datacontext的JControl
(<any>HTMLElement).prototype.getDataContextControl = function () {
    var parent = this;
    while (parent) {
        if (parent.JControl && parent.JControl.datacontext)
            return parent.JControl;
        else
            parent = parent.parentElement;
    }
    return null;
};
