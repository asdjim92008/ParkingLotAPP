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
    setday();
    ResetData();
};

function setday() {
    let date = new Date();
    const dayName = ["", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]
    let MM = (date.getMonth() + 1);
    let day = `${dayName[MM]}`;
    let today = day + " " + date.getDate() + "," + date.getFullYear();
    document.getElementById("setday").innerText = today;
}

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
            console.log(Response);
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
                    text += "</div><div class='sbbox'><input type='text' name='" + Response.data[i].led_no + "' class='ecarnum' placeholder='修改車位數'><button id='" + Response.data[i].led_no + "' type='button' class='submit' onclick='javascript: return Edata(this);'>Submit</button></div></div>";
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