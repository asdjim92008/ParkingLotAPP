$(document).ready(function () {
    $('.input-group.date').datetimepicker({
        format: "YYYY-MM-DD",
        defaultDate: new Date(),
        locale: "zh-tw"
    });
});


//Get URL parameter
const url = location.search;
let theRequest = new Object();
if (url.indexOf("?") != -1) {
    let str = url.substr(1);
    strs = str.split("&");
    for (let i = 0; i < strs.length; i++) {
        theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
    }
}

//Pagestorage
sessionStorage.setItem('page', 1);
let page = sessionStorage.getItem('page');
sessionStorage.setItem('parkingGuid', theRequest.parkingGuid);

window.onload = function () {
    $.ajax({ 
        type: 'GET',
        url: "/api/Data_SQL/FtpFile",
        data: {
            'parkingGuid': theRequest.parkingGuid,
            'page': page
        },
        dataType: 'json',
        async: true,
        success: function (response) {
            let code = JSON.stringify(response.code);
            let errmsg = JSON.stringify(response.errMsg)
            if (code == '"402"') {
                alert(errmsg);
                window.location = "/Login/Index";
            } else {
                let retext = "";

                //Alternate text
                for (let i = 0; i < response.data.length; i++) { 
                    if (response.data.length >= (i)) {
                        let pattern = /(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})/;
                        let dateString = response.data[i].ymdhm;
                        let formatedDate = dateString.replace(pattern, '$1/$2/$3 $4:$5:$6');

                        retext += "<div id='PL_news' class='PL_news'>";
                        retext += "<div class='PL_news_box'><button type='button' value='" + response.data[i].platenum + "' class='btn btn-lg editbt' data-toggle='modal' data-target='#myModal' onclick='javascript: return Editval(this.value," + i + ");'></button><div id='PL_image' class='PL_image'>";

                        if (response.data[i].jpg == "no such file") {
                            retext += "<img id='carimg" + i +"' src='https://fakeimg.pl/125x90/' width='125' height='90'></div>";
                        } else {
                            retext += "<img id='carimg" + i +"' src='data:image/jpg;base64," + response.data[i].jpg + "'></div>";
                        }

                        retext += "<div id='platenum" + i + "'  class='platenum'><span>車牌號：" + response.data[i].platenum + "<br>進場時間：";
                        retext += formatedDate + "</span></div><div id='PL_info' class='PL_info'>";
                        retext += "</div></div></div>";
                    } else {

                    }
                }
                $("#content_box").html(retext);
            }
        },
        error: function () { }
    });
};


//Page refresh
function Randompage(page) {
    $.ajax({
        type: 'GET',
        url: "/api/Data_SQL/FtpFile",
        data: {
            'parkingGuid': theRequest.parkingGuid,
            'page': page
        },
        dataType: 'json',
        async: true,
        success: function (response) {
            let code = JSON.stringify(response.code);
            let errmsg = JSON.stringify(response.errMsg)
            if (code == '"402"') {
                alert(errmsg);
                window.location = "/Login/Index";
            } else {
                let retext = "";

                //Alternate text
                for (let i = 0; i < response.data.length; i++) { 
                    if (response.data.length >= (i)) {
                        let pattern = /(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})/;
                        let dateString = response.data[i].ymdhm;
                        let formatedDate = dateString.replace(pattern, '$1/$2/$3 $4:$5:$6');

                        retext += "<div id='PL_news' class='PL_news'>";
                        retext += "<div class='PL_news_box'><button type='button' class='btn btn-lg editbt' value='" + response.data[i].platenum + "' data-toggle='modal' data-target='#myModal' onclick='javascript: return Editval(this.value," + i + ");'></button><div id='PL_image' class='PL_image'>";

                        if (response.data[i].jpg == "no such file") {
                            retext += "<img id='carimg" + i +"' src='https://fakeimg.pl/125x90/' width='125' height='90'></div>";
                        } else {
                            retext += "<img id='carimg" + i +"' src='data:image/jpg;base64," + response.data[i].jpg + "'></div>";
                        }

                        retext += "<div id='platenum" + i + "'  class='platenum'><span>車牌號：" + response.data[i].platenum + "<br>進場時間：";
                        retext += formatedDate + "</span></div><div id='PL_info' class='PL_info'>";
                        retext += "</div></div></div>";
                    } else {

                    }
                }
                $("#content_box").html(retext);
                var offpage = document.getElementById('offpage');
                offpage.style.top = '0px';
                offpage.style.position = 'relative';
            }
        },
        error: function () { }
    });


}



//Search
function SearchCar() {
    let parkingGuid = sessionStorage.getItem('parkingGuid'); //車廠GUID
    let page = 1;
    let SearchCarnum = document.getElementById("SearchCarnum").value;
/*    let date = document.getElementById("date").value;*/
/*    date = date.replace(/-/g, '');*/

    $.ajax({
        type: 'GET',
        url: "/api/Data_SQL/FtpFile",
        data: {
            'parkingGuid': parkingGuid,
            'page': page,
            'plateNum': SearchCarnum,
/*           'searchTime': date*/
        },
        dataType: 'json',
        async: true,
        success: function (response) {
            let code = JSON.stringify(response.code);
            let errmsg = JSON.stringify(response.errMsg)
            if (code == '"402"') {
                alert(errmsg);
                window.location = "/Login/Index";
            } else {
                let retext = "";
                for (let i = 0; i < response.data.length; i++) {

                    let pattern = /(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})/;
                    let dateString = response.data[i].ymdhm;
                    let formatedDate = dateString.replace(pattern, '$1/$2/$3 $4:$5:$6');

                    retext += "<div id='PL_news' class='PL_news'>";
                    retext += "<div class='PL_news_box'><button type='button' value='" + response.data[i].platenum + "' class='btn btn-lg editbt' data-toggle='modal' data-target='#myModal' onclick='javascript: return Editval(this.value," + i + ");'></button><div id='PL_image' class='PL_image'>";

                    if (response.data[i].jpg == "no such file") {
                        retext += "<img id='carimg" + i + "' src='https://fakeimg.pl/125x90/' width='125' height='90'></div>";
                    } else {
                        retext += "<img id='carimg" + i + "' src='data:image/jpg;base64," + response.data[i].jpg + "'></div>";
                    }

                    retext += "<div id='platenum" + i + "'  class='platenum'><span>車牌號：" + response.data[i].platenum + "<br>進場時間：";
                    retext += formatedDate + "</span></div><div id='PL_info' class='PL_info'>";
                    retext += "</div></div></div>";
                }
                $("#content_box").html(retext);
            }
        },
        error: function () { }
    });
}

//Modal
function Editval(val,row) {
    $("#platenumber").val(val);
    var obj = document.getElementById("infobox-img");
    var src = document.getElementById("carimg" + row + "").src;
    obj.src = src;
}

//Edit platenumber
function EditJsonData() {
    let parkingGuid = sessionStorage.getItem('parkingGuid'); //車廠GUID
    let platenumber = document.getElementById("platenumber").value; //原車牌號
    let Eplatenumber = document.getElementById("Eplatenumber").value; //修改之車牌號

    if (Eplatenumber == "" || Eplatenumber == null) {
        alert("請輸入修改之車牌號");
        return false;
    } else {
        $.ajax({
            type: 'POST',
            url: "/api/Data_SQL/ChangePlateNum",
            data: {
                'parkingGuid': parkingGuid,
                'n_PlateNum': platenumber,
                'c_PlateNum': Eplatenumber
            },
            dataType: 'json',
            async: true,
            success: function (response) {
                let code = JSON.stringify(response.code);
                let errmsg = JSON.stringify(response.errMsg)
                if (code == '"404"') {
                    alert(errmsg);
                } else if (code == '"402"') {
                    alert(errmsg);
                    window.location = "/Login/Index";
                } else if (code == '"200"') {
                    alert('修改車號成功!!');
                    EJsonData();
                    $('#myModal').modal('hide')
                }
            },
            error: function () {

            }
        });
    }
}

//Page refresh(After Editing)
function EJsonData() {
    let page = sessionStorage.getItem('page');
    let parkingGuid = sessionStorage.getItem('parkingGuid');

    $.ajax({
        type: 'GET',
        url: "/api/Data_SQL/FtpFile",
        data: {
            'parkingGuid': parkingGuid,
            'page': page
        },
        dataType: 'json',
        async: true,
        success: function (response) {
            let code = JSON.stringify(response.code);
            let errmsg = JSON.stringify(response.errMsg)
            if (code == '"402"') {
                alert(errmsg);
                window.location = "/Login/Index";
            } else {

                let retext = "";
                for (let i = 0; i < response.data.length; i++) { //組畫面html字串做替代
                    if (response.data.length >= (i)) {
                        let pattern = /(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})/;
                        let dateString = response.data[i].ymdhm;
                        let formatedDate = dateString.replace(pattern, '$1/$2/$3 $4:$5:$6');

                        retext += "<div id='PL_news' class='PL_news'>";
                        retext += "<div class='PL_news_box'><button type='button' class='btn btn-lg editbt' value='" + response.data[i].platenum + "' data-toggle='modal' data-target='#myModal' onclick='javascript: return Editval(this.value," + i + ");'></button><div id='PL_image' class='PL_image'>";

                        if (response.data[i].jpg == "no such file") {
                            retext += "<img id='carimg" + i + "' src='https://fakeimg.pl/125x90/' width='125' height='90'></div>";
                        } else {
                            retext += "<img id='carimg" + i + "' src='data:image/jpg;base64," + response.data[i].jpg + "'></div>";
                        }

                        retext += "<div id='platenum" + i + "'  class='platenum'><span>車牌號：" + response.data[i].platenum + "<br>進場時間：";
                        retext += formatedDate + "</span></div><div id='PL_info' class='PL_info'>";
                        retext += "</div></div></div>";
                    } else {

                    }
                }
                $("#content_box").html(retext);
            }
        },
        error: function () { }
    });
}


const Pagination = {
    nowPage: 1,
    totalPage: 2 //api接值後取總頁數計算給totalPage
};

const pagination = document.getElementsByClassName("pagination__group")[0];

function init() {
    Pagination.nowPage = 1;
    if (Pagination.totalPage > 5) {
        over5PageRender();
        over5PageListener();
    } else {
        less5PageRender();
        pageListener();
    }
}

function less5PageRender() {
    let totalPage = Pagination.totalPage;
    let nowPage = Pagination.nowPage;
    let ary = [];
    let num = 1;
    while (num < totalPage + 1) {
        let classList = nowPage === num ? "active" : "";
        ary.push({
            val: num,
            class: classList
        });
        num++;
    }
    listRender(ary);
}

function pageListener() {
    document.querySelectorAll(".pagination__item").forEach(el => {
        el.addEventListener("click", function () {
            if (el.dataset.val > 0) {
                Pagination.nowPage = parseInt(el.dataset.val);
                less5PageRender();
                pageListener();

                //Get content
                Randompage(el.dataset.val);
            }
        });
    });
}

function over5PageListener() {
    nextBtnSet();
    preBtnSet();
    document.querySelectorAll(".pagination__item").forEach(el => {
        el.addEventListener("click", function () {
            if (el.dataset.val > 0) {
                over5PageChange(el.dataset.val);

                //Get content
                Randompage(el.dataset.val);
            }
        });
    });
}

function over5PageChange(num) {
    Pagination.nowPage = parseInt(num);
    over5PageRender();
    over5PageListener();
}

function over5PageRender() {
    let nowPage = Pagination.nowPage;
    let totalPage = Pagination.totalPage;
    let pageStatus = over5PageJudgePageStatus(nowPage, totalPage);
    let ary = over5PageGenerateData(pageStatus, nowPage, totalPage);
    listRender(ary);
}

function preBtnSet() {
    document
        .getElementsByClassName("pre")[0]
        .addEventListener("click", function () {
            if (this.classList.value.indexOf("unclick") === -1) {
                over5PageChange(Pagination.nowPage - 1);
            }
        });
}

function nextBtnSet() {
    document
        .getElementsByClassName("next")[0]
        .addEventListener("click", function () {
            if (this.classList.value.indexOf("unclick") === -1) {
                over5PageChange(Pagination.nowPage + 1);
            }
        });
}

function over5PageJudgePageStatus(nowPage, totalPage) {
    return nowPage === 1 ? "first" : nowPage <= totalPage / 2 ? "front" : nowPage !== totalPage ? "back" : "last";
}

function over5PageGenerateData(pageStatus, nowPage, totalPage) {
    const map = {
        first: [{
            val: -2,
            class: "pre unclick"
        }, {
            val: nowPage,
            class: "active"
        }, {
            val: nowPage + 1,
            class: ""
        }, {
            val: nowPage + 2,
            class: ""
        }, {
            val: -1,
            class: "unclick"
        }, {
            val: totalPage,
            class: ""
        }, {
            val: -2,
            class: "next"
        }],
        front: [{
            val: -2,
            class: "pre"
        }, {
            val: nowPage - 1,
            class: ""
        }, {
            val: nowPage,
            class: "active"
        }, {
            val: nowPage + 1,
            class: ""
        }, {
            val: -1,
            class: "unclick"
        }, {
            val: totalPage,
            class: ""
        }, {
            val: -2,
            class: "next"
        }],
        back: [{
            val: -2,
            class: "pre"
        }, {
            val: 1,
            class: ""
        }, {
            val: -1,
            class: "unclick"
        }, {
            val: nowPage - 1,
            class: ""
        }, {
            val: nowPage,
            class: "active"
        }, {
            val: nowPage + 1,
            class: ""
        }, {
            val: -2,
            class: "next"
        }],
        last: [{
            val: -2,
            class: "pre"
        }, {
            val: 1,
            class: ""
        }, {
            val: -1,
            class: "unclick"
        }, {
            val: nowPage - 2,
            class: ""
        }, {
            val: nowPage - 1,
            class: ""
        }, {
            val: nowPage,
            class: "active"
        }, {
            val: -2,
            class: "next unclick"
        }]
    };
    return map[pageStatus];
}

function listRender(ary) {
    pagination.innerHTML = "";
    ary.forEach(element => {
        let li = document.createElement("li");
        li.setAttribute("data-val", element.val);
        li.setAttribute("class", "pagination__item " + element.class);
        li.innerText =
            element.val >= 0 ? element.val : element.val === -1 ? "..." : "";
        pagination.appendChild(li);
    });
}

init();