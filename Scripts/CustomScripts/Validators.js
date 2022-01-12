function ValidAlphabet() {

    var code = (event.which) ? event.which : event.keyCode;

    if ((code >= 65 && code <= 90) ||
        (code >= 97 && code <= 122) || (code == 32))
    { return true; }
    else
    { return false; }
}

function AlphaNumeric(e) {
    var regex = new RegExp("^[a-zA-Z0-9]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    }

    e.preventDefault();
    return false;
}

function OnlyDotsAndNumbers(txt, event) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode == 46) {
        if (txt.value.indexOf(".") < 0)
            return true;
        else
            return false;
    }

    if (txt.value.indexOf(".") > 0) {
        var txtlen = txt.value.length;
        var dotpos = txt.value.indexOf(".");
        //Change the number here to allow more decimal points than 2
        if ((txtlen - dotpos) > 2)
            return false;
    }

    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}


