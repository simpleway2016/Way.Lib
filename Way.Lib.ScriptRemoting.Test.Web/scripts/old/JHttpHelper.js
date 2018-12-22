var JHttpHelper = (function () {
    function JHttpHelper() {
    }
    JHttpHelper.writePage = function (url) {
        document.write(JHttpHelper.downloadUrl(url));
    };
    JHttpHelper.downloadUrl = function (url) {
        var invoker = new WayScriptInvoker(url);
        invoker.async = false;
        var errcount = 0;
        var result;
        invoker.onCompleted = function (ret, err) {
            if (err) {
                errcount++;
                if (errcount <= 1) {
                    invoker.Get();
                }
                else {
                    throw "无法打开网页：" + url;
                }
            }
            else {
                result = ret;
            }
        };
        invoker.Get();
        return result;
    };
    return JHttpHelper;
}());
