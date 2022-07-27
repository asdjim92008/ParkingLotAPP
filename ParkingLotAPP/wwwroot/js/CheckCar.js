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
    //頁數存取
    sessionStorage.setItem('page', 1);
    let page = sessionStorage.getItem('page');
    sessionStorage.setItem('parkingGuid', theRequest.parkingGuid);
};

function Editval() {
    window.location = "/Discount/Index";
}


/*更新數據*/
//function GetFile(num) {
//    //更新頁數
//    let page = parseInt(sessionStorage.getItem('page'));
//    sessionStorage.removeItem('page');
//    sessionStorage.setItem('page', (page + parseInt(num)));

//    page = parseInt(sessionStorage.getItem('page'));
//    let parkingGuid = sessionStorage.getItem('parkingGuid');

//    let SearchCar = document.getElementById("SearchCar").value;
//    let date = document.getElementById("date").value;

//    $.ajax({
//        type: 'GET',
//        url: "/api/Data_SQL/FtpFile",
//        data: { 'parkingGuid': parkingGuid, 'page': page, 'plateNum': SearchCar, 'searchTime': date },
//        dataType: 'json',
//        async: true,
//        success: function (response) {
//            let code = JSON.stringify(response.code);
//            let errmsg = JSON.stringify(response.errMsg)
//            if (code == '"402"') {
//                alert(errmsg);
//                window.location = "/Login/Index";
//            }
//            else {
//                for (let i = 0; i < response.data.length; i++) {

//                    let pattern = /(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})/;
//                    let dateString = response.data[i].ymdhm;
//                    let formatedDate = dateString.replace(pattern, '$1/$2/$3 $4:$5:$6');

//                    if (response.data[i].jpg == "no such file") {
//                        text = "<img src='https://fakeimg.pl/125x90/' width='125' height='90'>";
//                        PL_info = "<time class='PL_time'>" + formatedDate + "</time>";
//                    }
//                    else {
//                        text = "<img src='data:image/jpg;base64," + response.data[i].jpg + "'>";
//                        PL_info = "<time class='PL_time'>" + formatedDate + "</time>";
//                    }

//                    if (response.data[i].tickno == "") {
//                        carnum = "<br><br><br><span class='nocarnum'>無辨識車牌號</span>";
//                    }
//                    else {
//                        carnum = "<span>車牌號：" + response.data[i].platenum + "<br>票號：" + response.data[i].tickno + "<br>車道編號：" + response.data[i].rid + "</span>";
//                    }

//                    $("#PL_image" + i).html(text);
//                    $("#carnum" + i).html(carnum);
//                    $("#nocarnum" + i).html(carnum);
//                    $("#PL_info" + i).html(PL_info);

//                    let LNbt = "<div class='Lbt_div' id='Lbt'><button type = 'button' class='btn btn-link Lbt' value = '-1' onclick = 'GetFile(this.value)'> 上一頁</button></div><a>" + page + "</a>";
//                    LNbt += "<div class='Nbt_div' id = 'Nbt'><button type='button' class='btn btn-link Nbt' value='1' onclick='GetFile(this.value)'>下一頁</button></div>";

//                    $("#add_img_box").html(LNbt);
//                }

//                if (response.data.length < 5) {
//                    let pagerow = "<div class='Lbt_div' id='Lbt'><button type = 'button' class='btn btn-link Lbt' style='margin-left: 125px;' value = '-1' onclick = 'GetFile(this.value)' > 上一頁</button></div><a>" + page + "</a>";
//                    $("#add_img_box").html(pagerow);

//                    for (let i = 0; (response.data.length) + (i + 1) < 6; i++) {
//                        let row = (response.data.length - 1) + (i + 1);

//                        $("#PL_image" + row).html(null);
//                        $("#carnum" + row).html(null);
//                        $("#PL_info" + row).html(null);

//                        let PL_news = "<div><span id = 'PL_image" + row + "' class='PL_image" + row + "'></span ><div class='carnum' id='carnum" + row + "'></div><div class='PL_info' id='PL_info" + row + "'></div></div>";

//                        $("#PL_news" + row).html(PL_news);
//                    }
//                }

//                if (page == 1) {
//                    let pagerow = "<a style='margin-left: 150px;'>" + page + "</a>";

//                    pagerow += "<div class='Nbt_div' id='Nbt' style='position: relative;display: inline-block;margin-left: 5px;'><button type='button' class='btn btn-link Nbt' value='1' onclick='GetFile(this.value)'>下一頁</button></div>";

//                    $("#add_img_box").html(pagerow);
//                }
//            }
//        },
//        error: function () {

//        }
//    });
//}