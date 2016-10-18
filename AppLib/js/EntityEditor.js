function _EntityEditor_submit(obj) {
    var form = obj.parentElement;
    while (form.tagName != "FORM") {
        form = form.parentElement;
    }
   
    if (form.fireEvent) {
        form.fireEvent("onsubmit"); 
        form.submit();
    }
    else {
        var evt = document.createEvent('HTMLEvents');
        evt.initEvent('submit', true, true);
        form.dispatchEvent(evt);
        form.submit();
    }
}

        function _EntityEditor_Save(clientID) {
            eval(clientID + "_Save()");
        }
		function _EntityEditor_Cancel(clientID) {
            var obj = document.getElementsByName(clientID + "_$$_command")[0];
            obj.value = "cancel";
            _EntityEditor_submit(obj);
        }
		function _EntityEditor_ModifyData(clientID , dataid) {
            var obj = document.getElementsByName(clientID + "_$$_command")[0];
            obj.value = "m:" + dataid;
            _EntityEditor_submit(obj);
        }
		function _EntityEditor_Insert(clientID) {
            var obj = document.getElementsByName(clientID + "_$$_command")[0];
            obj.value = "insert";
            _EntityEditor_submit(obj);
        }
