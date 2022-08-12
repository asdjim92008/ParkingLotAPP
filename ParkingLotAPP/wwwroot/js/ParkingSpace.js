const url = location.search; //獲取url中"?"符號後的字串
let theRequest = new Object();
if (url.indexOf("?") != -1) {
    let str = url.substr(1);
    strs = str.split("&");
    for (let i = 0; i < strs.length; i++) {
        theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
    }
}
window.onload = function () {
    ResetData();
};

function ResetData() {
    let num, getnum = "";
    let text = "";
    let row = "";
    let newnum = new Array();
    $.ajax({
        type: 'GET',
        url: "/api/UDP/GetCarCount",
        data: { 'parkingGuid': theRequest.parkingGuid },
        dataType: 'json',
        async: true,
        success: function (Response) {
            let code = JSON.stringify(Response.code);
            let errmsg = JSON.stringify(Response.errMsg)
            let error = JSON.stringify(Response.data[0].state)
            if (code == '"402"') {
                alert(errmsg);
                window.location = "/Login/Index";
            }
            else if (code == '"404"') {
                text = "<div class='row listbox_ct' id='listbox_ct'><div class='pkdata' id='parkingName' >連接失敗未取得資料</div>";
                text += "<div class='SevenSegment' id='SevenSegment'>";
                text += "<span class='seven-segment' id='num0' data-number='0'></span>";
                text += "<span class='seven-segment' id='num1' data-number='0'></span>";
                text += "<span class='seven-segment' id='num2' data-number='0'></span>";
                text += "<span class='seven-segment' id='num3' data-number='0'></span></div></div>";
                $("#listbox").html(text);
                alert(errmsg);
            }
            else if (error == '"失敗"') {
                text = "<div class='row listbox_ct' id='listbox_ct'><div class='pkdata' id='parkingName' >連接失敗未取得資料</div>";
                text += "<div class='SevenSegment' id='SevenSegment'>";
                text += "<span class='seven-segment' id='num0' data-number='0'></span>";
                text += "<span class='seven-segment' id='num1' data-number='0'></span>";
                text += "<span class='seven-segment' id='num2' data-number='0'></span>";
                text += "<span class='seven-segment' id='num3' data-number='0'></span></div></div>";
                $("#listbox").html(text);
                alert("連接失敗!未取得資料!!");
            }
            else {
                for (i = 0; i < Response.data.length; i++) {
                    num = Response.data[i].led_CarCount;
                    row = num.length;
                    for (a = 0; a < row; a++) {
                        newnum.push(num.charAt(0));
                        num = num.slice(1);
                    }

                    text += "<div class='row listbox_ct'><div class='pkdata'>" + Response.data[i].parkingName + Response.data[i].area + "</div>";
                    text += "<div class='SevenSegment'>";

                    if (newnum.length < 4) {
                        const repair = newnum.length;
                        switch (repair) {
                            case 1: {
                                newnum.splice(0, 0, 0);
                                newnum.splice(0, 0, 0);
                                newnum.splice(0, 0, 0);
                                break;
                            }
                            case 2: {
                                newnum.splice(0, 0, 0);
                                newnum.splice(0, 0, 0);
                                break;
                                break;
                            }
                            case 3: {
                                newnum.splice(0, 0, 0);
                                break;
                            }
                            default: {
                                break;
                            }
                        }
                    }
                    for (b = 0; b <= (newnum.length + 2); b++) {
                        getnum = newnum.shift();
                        text += "<span class='seven-segment' data-number='" + getnum + "'></span>";
                    }
                    text += "</div><div class='sbbox'><ul class='btn-numbox'><li><ul class='count'>";
                    text += "<li><span id='num-jian' class='num-jian' onclick='num_jian()'>-</span></li>";
                    text += "<li><input type='text' name='" + Response.data[i].led_no + "' class='input-num' id='input-num' value='0'/></li>";
                    text += "<li><span id='num-jia' class='num-jia' onclick='num_jia()'>+</span></li></ul></li></ul>";
                    text += "<button id='" + Response.data[i].led_no + "' type='button' class='submit' onclick='javascript: return Edata(this);'>Submit</button></div></div>";
                }
                $("#listbox").html(text);
            }
        },
        error: function () {

        }
    })
}

function Edata(myObj) {
    let text = "";
    let carCount = $("input[name='" + myObj.id + "']").val();
    $.ajax({
        type: 'GET',
        url: "/api/UDP/SetCarCount",
        data: { 'parkingGuid': theRequest.parkingGuid, 'carCount': carCount, 'num': myObj.id },
        dataType: 'json',
        async: true,
        success: function (Response) {
            let code = JSON.stringify(Response.code);
            let errmsg = JSON.stringify(Response.errMsg)
            let error = JSON.stringify(Response.data[0].state)
            if (code == '"402"') {
                alert(errmsg);
                window.location = "/Login/Index";
            }
            else if (code == '"404"') {
                text = "<div class='row listbox_ct' id='listbox_ct'><div class='pkdata' id='parkingName' ></div>";
                text += "<div class='SevenSegment' id='SevenSegment'>";
                text += "<span class='seven-segment' id='num0' data-number='0'></span>";
                text += "<span class='seven-segment' id='num1' data-number='0'></span>";
                text += "<span class='seven-segment' id='num2' data-number='0'></span>";
                text += "<span class='seven-segment' id='num3' data-number='0'></span></div></div>";
                $("#listbox").html(text);
                alert(errmsg);
            }
            else if (error == '"失敗"') {
                text = "<div class='row listbox_ct' id='listbox_ct'><div class='pkdata' id='parkingName' ></div>";
                text += "<div class='SevenSegment' id='SevenSegment'>";
                text += "<span class='seven-segment' id='num0' data-number='0'></span>";
                text += "<span class='seven-segment' id='num1' data-number='0'></span>";
                text += "<span class='seven-segment' id='num2' data-number='0'></span>";
                text += "<span class='seven-segment' id='num3' data-number='0'></span></div></div>";
                $("#listbox").html(text);
                alert("連接失敗!資料修改未完成!!");
            }
            else {
                alert("資料修改成功!!");
                ResetData();
            }
        },
        error: function () {

        }
    })
}

/** 數值加減btn**/
function num_jia() {
    var input_num = document.getElementById("input-num");
    input_num.value = parseInt(input_num.value) + 1;
}

function num_jian() {
    var input_num = document.getElementById("input-num");
    if (input_num.value <= 0) {
        input_num.value = 0;
    } else {
        input_num.value = parseInt(input_num.value) - 1;
    }
}