﻿var oldonload = window.onload;

window.onload = function () {
    const url = location.search; //獲取url中"?"符號後的字串
    let theRequest = new Object();
    if (url.indexOf("?") != -1) {
        let str = url.substr(1);
        strs = str.split("&");
        for (let i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
        }
    }
    sessionStorage.setItem('parkingGuid', theRequest.parkingGuid);

    $.ajax({
        type: 'GET',
        url: "/api/Data_SQL/DefaultInfo",
        data: { 'parkingGuid': theRequest.parkingGuid },
        dataType: 'json',
        async: true,
        success: function (response) {
            let code = JSON.stringify(response.code);
            let errmsg = JSON.stringify(response.errMsg)

            if (code == '"200"') {
                var date = new Date();
                Y = date.getFullYear() + '-';
                M = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '-';
                D = date.getDate();
                h = date.getHours();
                m = date.getMinutes();
                s = date.getSeconds();
                if (s < 10) { s = '0' + s; }

                if (m < 10) { m = '0' + m + ':'; }
                else { m = m + ':'; }

                if (h < 10) { h = '0' + h + ':'; }
                else { h = h + ':'; }

                $("#rid").val(response.data.rid);
                $("#tickno").val(response.data.tickno);
                $("#approachtime").val(Y + M + D + "T" + h + m + s);
                $("#platenum").val('');
                $("#tickclass").val('');
            }
            else if (code == '"402"') {
                alert(errmsg);
                window.location = "/Login/Index";
            }
        },
        error: function () {

        }
    });
}

function AddJsonData() {
    let parkingGuid = sessionStorage.getItem('parkingGuid'); //車廠GUID
    let rid = document.getElementById("rid").value;  //車道編號
    let tickno = document.getElementById("tickno").value;  //票號
    let approachtime = document.getElementById("approachtime").value;  //入場時間
    approachtime = approachtime.replace(/-/g, '');
    approachtime = approachtime.replace(/T/g, '');
    approachtime = approachtime.replace(/:/g, '');

    let platenum = document.getElementById("platenum").value;  //車牌號碼
    let tickclass = document.getElementById("tickclass").value;  //費率別
    let Msg = "";
    if (rid == "" || rid == null) {
        Msg += "請輸入車道編號";
    }
    if (tickno == "" || tickno == null) {
        Msg += "\n請輸入票號";
    }
    if (approachtime == "" || approachtime == null) {
        Msg += "\n請輸入入場時間";
    }
    if (platenum == "" || platenum == null) {
        Msg += "\n請輸入車牌號碼";
    }
    if (tickclass == "" || tickclass == null) {
        Msg += "\n請輸入費率別";
    }

    if (Msg != "") {
        alert(Msg);
        return false;
    }
    else {
        $.ajax({
            type: 'POST',
            url: "/api/Data_SQL/InsertPlateNum",
            data: { 'parkingGuid': parkingGuid, 'plateNum': platenum, 'rid': rid, 'ymhdm': approachtime, 'tickno': tickno, 'tickclass': tickclass },
            dataType: 'json',
            async: true,
            success: function (response) {
                let code = JSON.stringify(response.code);
                let errmsg = JSON.stringify(response.errMsg)

                if (code == '"200"') {
                    alert("新增車牌成功!!");
                    window.onload();
                }
                else if (code == '"404"') {
                    alert(errmsg);
                }
                else if (code == '"402"') {
                    alert(errmsg);
                    window.location = "/Login/Index";
                }
            },
            error: function () {

            }
        });
    }
}