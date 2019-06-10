String.prototype.controller = function () {
    return WayScriptRemoting.createRemotingController(this);
};
