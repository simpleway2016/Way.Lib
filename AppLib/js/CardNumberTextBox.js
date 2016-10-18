function JS_CardNumberTextBox(clientid) {
    var txtObj = $("#" + clientid);
    function checkvalue() {
        var value = txtObj.val();
        var result = "";
        var keycount = 0;
        var startIndex = 0;
        for (var i = 0 ; i < value.length ; i++) {
            if (value[i] != ' ') {
                keycount++;
                if (keycount == 4) {
                    keycount = 0;
                    result += value.substr(startIndex, 4) + " ";
                    startIndex += 4;
                }
            }
            else {
                if (keycount > 0) {
                    keycount = 0;
                    result += value.substr(startIndex, i - startIndex + 1);

                }
                startIndex = i + 1;
            }
        }
        if (keycount > 0) {
            result += value.substr(startIndex, keycount);
        }

        txtObj.val(result);
    }
    txtObj.keyup(checkvalue);

    checkvalue();
}