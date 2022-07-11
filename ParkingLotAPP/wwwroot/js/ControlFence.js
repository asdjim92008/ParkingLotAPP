window.onload = function(){
    const url = location.search; //獲取url中"?"符號後的字串
    let theRequest = new Object();
    if (url.indexOf("?") != -1) {
        let str = url.substr(1);
        strs = str.split("&");
        for (let i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
        }
    }

    let ParkingType = new Array()
    ParkingType = JSON.parse(localStorage.getItem('ParkingType'));
    if (ParkingType == "CB") {
        ParkingType = "車辨場";
    }
    else if (ParkingType == "CD") {
        ParkingType = "車擋板場";
    }
    else if (ParkingType == "CT") {
        ParkingType = "晶片場";
    }

    let text = "";

    $.ajax({
        type: 'GET',
        url: "/api/Data_SQL/GetFence",
        data: { 'parkingGuid': theRequest.parkingGuid },
        dataType: 'json',
        async: true,
        success: function (response) {
            let code = JSON.stringify(response.code);
            let errmsg = JSON.stringify(response.errMsg)
            if (code == '"402"') {
                alert(errmsg);
                window.location = "/Login/Index";
            }
            else {
                for (let i = 0; i < response.data.length; i++) {
                    text += "<div class='row listbox_ct' id='listbox_ct" + i + "'><div class='pkdata' id='parkingNam" + i + "'>";
                    text += response.data[i].remark + response.data[i].area;
                    text += "</div><div class='ParkingType'>" + ParkingType + "柵欄編號" + response.data[i].fence_no + "</div>";
                    text += "<div class='loading-circles'><div class='circle hold control_bt' data-toggle='modal' data-target='#staticBackdrop' onclick='javascript: return CheckData(" + response.data[i].fence_no + ");'>OPEN</div>";
                    text += "<div class='circle first'></div><div class='circle'></div><div class='circle'></div></div></div>";
                    $("#listbox").html(text);
                }
            }
        },
        error: function () {
        }
    });
}

function CheckData(checknum) {
    var i = (checknum + "").length;
    while (i++ < 3) checknum = "0" + checknum;

    localStorage.setItem('Fence_no', JSON.stringify(checknum));
}

function OpenData() {
    const url = location.search; //獲取url中"?"符號後的字串
    let theRequest = new Object();
    if (url.indexOf("?") != -1) {
        let str = url.substr(1);
        strs = str.split("&");
        for (let i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
        }
    }

    let Fence_no = JSON.parse(localStorage.getItem('Fence_no'));

    $.ajax({
        type: 'GET',
        url: "/api/UDP/OpenGate",
        data: { 'parkingGuid': theRequest.parkingGuid, 'num': Fence_no },
        dataType: 'json',
        async: true,
        success: function (response) {
            let code = JSON.stringify(response.code);
            let data = JSON.stringify(response.data)
            if (code == '"402"') {
                alert(data);
                window.location = "/Login/Index";
                $('#staticBackdrop').modal('hide')
            }
            else if (code == '"200"') {
                alert(data);
                $('#staticBackdrop').modal('hide')
            }
            else {
                alert(data);
                $('#staticBackdrop').modal('hide')
            }
        },
        error: function () {
        }
    });
}