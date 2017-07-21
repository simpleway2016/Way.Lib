HTMLElement.prototype.getContainer = function () {
    var parent = this;
    while (parent) {
        if (parent.JControl)
            return parent.JControl;
        else
            parent = parent.parentElement;
    }
    return null;
};
HTMLElement.prototype.getDataContextControl = function () {
    var parent = this;
    while (parent) {
        if (parent.JControl && parent.JControl.datacontext)
            return parent.JControl;
        else
            parent = parent.parentElement;
    }
    return null;
};
String.prototype.controller = function () {
    return WayScriptRemoting.createRemotingController(this);
};
//# sourceMappingURL=JExtension.js.map