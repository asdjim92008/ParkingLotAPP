function checkdata() {
    var errmsg = "";
    if (document.form1.account.value == "") {
        errmsg = "請輸入帳號\n";
    }
    if (document.form1.password.value == "") {
        errmsg = errmsg + "請輸入密碼";
    }
    if (errmsg != "") {
        alert(errmsg)
        return false
    }
    else {
        var account = document.getElementById("account").value;
        var password = document.getElementById("password").value;
        $.ajax({
            type: 'POST',
            url: "/api/Data_SQL/Login",
            data: { 'Account': account, 'Password': password },
            dataType: 'json',
            async: true,
            success: function (response) {
                var code = JSON.stringify(response.code);
                var errmsg = JSON.stringify(response.errMsg)
                if (code == '"400"') {
                    alert(errmsg);
                }
                else if (code == '"200"') {
                    localStorage.setItem('PLdataObject', JSON.stringify(response));
                    alert('登入成功!!');
                    window.location = "/ParkingList/Index";
                }
                else if (code == '"403"') {
                    alert(errmsg);
                }
            },
            error: function () {
                alert('登入失敗!!');
            }
        });
    }
}