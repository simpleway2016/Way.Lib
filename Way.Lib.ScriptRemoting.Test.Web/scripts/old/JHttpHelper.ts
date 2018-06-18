
class JHttpHelper {

    static writePage(url: string): void {
        document.write(JHttpHelper.downloadUrl(url));
    }

    static downloadUrl(url: string): string {
        var invoker = new WayScriptInvoker(url);
        invoker.async = false;
        var errcount = 0;
        var result;
        invoker.onCompleted = (ret, err) => {
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
        }
        invoker.Get();
        return result;
    }

}