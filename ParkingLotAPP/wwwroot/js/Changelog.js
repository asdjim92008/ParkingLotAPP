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

    let text = "<table class='maintable' style='border: solid;' width='100%' border='1' cellspacing='0' cellpadding='3' id='mtable'><tr><td class='colstd' align = 'center'>內容</td><td class='colstd' align='center' width='120'>異動人員</td><td class='colstd' align='center' width='80'>異動時間</td></tr>";

    $.ajax({
        type: 'GET',
        url: "/api/Data_SQL/DBlog",
        data: { 'parkingGuid': theRequest.parkingGuid, 'page': page },
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

                    let pattern = /(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})/;
                    let dateString = response.data[i].time;
                    let formatedDate = dateString.replace(pattern, '$1/$2/$3 $4:$5:$6');

                    text += '<tr><td class="colstd">' + response.data[i].log + '</td><td class="colstd" style="text-align: center;"> ' + response.data[i].manager + '</td><td class="colstd">' + formatedDate + '</td></tr>'
                }

                text += '</table>';

                $("#tbtext").html(text);

                let pagerow = "<a style='margin-left: 150px;color:#FFF;'>" + page + "</a>";

                pagerow += "<div class='Nbt_div' id='Nbt' style='position: relative;display: inline-block;margin-left: 5px;'><button type='button' class='btn btn-link Nbt' value='1' onclick='GetFile(this.value)'>下一頁</button></div>";

                $("#add_img_box").html(pagerow);
            }
        },
        error: function () {
        }
    });
}

/*更新數據*/
function GetFile(num) {
    //更新頁數
    let page = parseInt(sessionStorage.getItem('page'));
    sessionStorage.removeItem('page');
    sessionStorage.setItem('page', (page + parseInt(num)));

    page = parseInt(sessionStorage.getItem('page'));
    let parkingGuid = sessionStorage.getItem('parkingGuid');

    let SearchMng = document.getElementById("SearchMng").value;
    let date = document.getElementById("date").value;

    let text = "<table class='maintable' style='border: solid;' width='100%' border='1' cellspacing='0' cellpadding='3' id='mtable'><tr><td class='colstd' align = 'center'>內容</td><td class='colstd' align='center' width='120'>異動人員</td><td class='colstd' align='center' width='80'>異動時間</td></tr>";


    $.ajax({
        type: 'GET',
        url: "/api/Data_SQL/DBlog",
        data: { 'parkingGuid': parkingGuid, 'page': page, 'manager': SearchMng, 'searchTime': date },
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
                    let pattern = /(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})/;
                    let dateString = response.data[i].time;
                    let formatedDate = dateString.replace(pattern, '$1/$2/$3 $4:$5:$6');

                    text += '<tr><td class="colstd">' + response.data[i].log + '</td><td class="colstd" style="text-align: center;"> ' + response.data[i].manager + '</td><td class="colstd">' + formatedDate + '</td></tr>'
                }

                text += '</table>';
                $("#tbtext").html(text);

                let LNbt = "<div class='Lbt_div' id='Lbt'><button type = 'button' class='btn btn-link Lbt' value = '-1' onclick = 'GetFile(this.value)'> 上一頁</button></div><a style='color: #FFF;'>" + page + "</a>";
                LNbt += "<div class='Nbt_div' id = 'Nbt'><button type='button' class='btn btn-link Nbt' value='1' onclick='GetFile(this.value)'>下一頁</button></div>";
                $("#add_img_box").html(LNbt);

                if (response.data.length < 10) {
                    let pagerow = "<div class='Lbt_div' id='Lbt'><button type = 'button' class='btn btn-link Lbt' style='margin-left: 125px;' value = '-1' onclick = 'GetFile(this.value)' > 上一頁</button></div><a style='color: #FFF;'>" + page + "</a>";
                    $("#add_img_box").html(pagerow);
                }

                if (page == 1) {
                    let pagerow = "<a style='margin-left: 150px;color:#FFF;'>" + page + "</a>";

                    pagerow += "<div class='Nbt_div' id='Nbt' style='position: relative;display: inline-block;margin-left: 5px;'><button type='button' class='btn btn-link Nbt' value='1' onclick='GetFile(this.value)'>下一頁</button></div>";

                    $("#add_img_box").html(pagerow);
                }
            }
        },
        error: function () {

        }
    });
}

/*搜尋功能*/
function SearchMng() {
    let parkingGuid = sessionStorage.getItem('parkingGuid'); //車廠GUID
    let page = 1;
    let SearchMng = document.getElementById("SearchMng").value;
    let date = document.getElementById("date").value;

    date = date.replace(/-/g, '');

    let text = "<table class='maintable' style='border: solid;' width='100%' border='1' cellspacing='0' cellpadding='3' id='mtable'><tr><td class='colstd' align = 'center'>內容</td><td class='colstd' align='center' width='120'>異動人員</td><td class='colstd' align='center' width='80'>異動時間</td></tr>";


    $.ajax({
        type: 'GET',
        url: "/api/Data_SQL/DBlog",
        data: { 'parkingGuid': parkingGuid, 'page': page, 'manager': SearchMng, 'searchTime': date },
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

                    let pattern = /(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})/;
                    let dateString = response.data[i].time;
                    let formatedDate = dateString.replace(pattern, '$1/$2/$3 $4:$5:$6');

                    text += '<tr><td class="colstd">' + response.data[i].log + '</td><td class="colstd" style="text-align: center;"> ' + response.data[i].manager + '</td><td class="colstd">' + formatedDate + '</td></tr>'

                }
                text += '</table>';

                $("#tbtext").html(text);

                let pagerow = "<div style='display: inline-block;margin-left: 170px;'>" + page + "</div>";
                pagerow += "<div class='Nbt_div' id='Nbt' style='position: relative;display: inline-block;margin-left: 5px;'><button type='button' class='btn btn-link Nbt' value='1' onclick='GetFile(this.value)'>下一頁</button></div>";
                $("#add_img_box").html(pagerow);

                if (response.data.length < 10) {
                    let pagerow = "<div style='display: inline-block;margin-left: 170px;'>" + page + "</div>";
                    $("#add_img_box").html(pagerow);
                }

            }
        },
        error: function () {
        }
    });
}