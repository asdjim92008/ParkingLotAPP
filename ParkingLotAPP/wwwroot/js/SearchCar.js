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

function SearchCar() {
    Randompage(1);//替代html內容
}

/*Enter submit*/
document.getElementById('SearchCar')
    .addEventListener('keyup', function (event) {
        if (event.code === 'Enter') {
            SearchCar();
        }
    });


/*title-font*/
const signs = document.querySelectorAll('x-sign')
const randomIn = (min, max) => (
    Math.floor(Math.random() * (max - min + 1) + min)
)

const mixupInterval = el => {
    const ms = randomIn(2000, 4000)
    el.style.setProperty('--interval', `${ms}ms`)
}

signs.forEach(el => {
    mixupInterval(el)
    el.addEventListener('webkitAnimationIteration', () => {
        mixupInterval(el)
    })
})

/*前往折抵頁面*/
function Discount() {
    window.location = "/Discount/Index";
}

/*更新頁數*/
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
        ary.push({ val: num, class: classList });
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
                Randompage(el.dataset.val)//按下頁數取得此頁資料
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
                Randompage(el.dataset.val)//按下頁數取得此頁資料
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
    return nowPage === 1
        ? "first"
        : nowPage <= totalPage / 2
            ? "front"
            : nowPage !== totalPage
                ? "back"
                : "last";
}
function over5PageGenerateData(pageStatus, nowPage, totalPage) {
    const map = {
        first: [
            { val: -2, class: "pre unclick" },
            { val: nowPage, class: "active" },
            { val: nowPage + 1, class: "" },
            { val: nowPage + 2, class: "" },
            { val: -1, class: "unclick" },
            { val: totalPage, class: "" },
            { val: -2, class: "next" }
        ],
        front: [
            { val: -2, class: "pre" },
            { val: nowPage - 1, class: "" },
            { val: nowPage, class: "active" },
            { val: nowPage + 1, class: "" },
            { val: -1, class: "unclick" },
            { val: totalPage, class: "" },
            { val: -2, class: "next" }
        ],
        back: [
            { val: -2, class: "pre" },
            { val: 1, class: "" },
            { val: -1, class: "unclick" },
            { val: nowPage - 1, class: "" },
            { val: nowPage, class: "active" },
            { val: nowPage + 1, class: "" },
            { val: -2, class: "next" }
        ],
        last: [
            { val: -2, class: "pre" },
            { val: 1, class: "" },
            { val: -1, class: "unclick" },
            { val: nowPage - 2, class: "" },
            { val: nowPage - 1, class: "" },
            { val: nowPage, class: "active" },
            { val: -2, class: "next unclick" }
        ]
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

function Randompage(page) {
    let pagenum = 5;
    let getstartpage = ((page - 1) * pagenum) + 1;
    let getendpage = page * pagenum;
    //api取回來的值
    let dataobj = [
        {
            image: '2022_07_01_10_22_54939_9050057be02b4374b85de3f4e2aa0b95_full',
            carnum: 'AV-1547',
            time: '2022/06/21 04:15:34',
        },
        {
            image: '2022_07_01_10_14_56884_bae7c3330cc64cf4bfef40a5824e43c2_full',
            carnum: 'AV-2831',
            time: '2022/06/21 11:52:59',
        },
        {
            image: '2022_07_01_10_22_54939_9050057be02b4374b85de3f4e2aa0b95_full',
            carnum: 'BT-2647',
            time: '2022/06/21 04:15:34',
        },
        {
            image: '2022_07_01_10_14_56884_bae7c3330cc64cf4bfef40a5824e43c2_full',
            carnum: 'KC-6873',
            time: '2022/06/21 11:52:59',
        },
        {
            image: '2022_07_01_10_22_54939_9050057be02b4374b85de3f4e2aa0b95_full',
            carnum: 'JP-9567',
            time: '2022/06/21 04:15:34',
        },
        {
            image: '2022_07_01_10_14_56884_bae7c3330cc64cf4bfef40a5824e43c2_full',
            carnum: 'BJ-9831',
            time: '2022/06/21 11:52:59',
        },
    ];

    let retext = "";
    for (var i = getstartpage; i <= getendpage; i++) { //組畫面html字串做替代
        if (dataobj.length >= (i)) {
            retext += "<div id='PL_news' class='PL_news' onclick='javascript: return Discount(this.value);'>";
            retext += "<div class='PL_news_box'><div id='PL_image' class='PL_image'>";
            retext += "<img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS_cMDohC3alvJXzDk4_-hqSF13qemnMq1BiA&usqp=CAU'></div>";
            retext += "<div id='carnum1' class='carnum'><span>車牌號：" + dataobj[i - 1].carnum + "<br>進場時間：";
            retext += dataobj[i - 1].time + "</span></div><div id='PL_info' class='PL_info'>";
            retext += "</div></div></div>";
        }
        else {

        }
    }
    $("#content_box").html(retext);

    var offpage = document.getElementById('offpage');
    offpage.style.top = '0px';
    offpage.style.position = 'relative';
}
/*End*/

/*搜尋功能*/
//function SearchCar() {
//    let parkingGuid = sessionStorage.getItem('parkingGuid'); //車廠GUID
//    let page = 1;
//    let SearchCar = document.getElementById("SearchCar").value;
//    let date = document.getElementById("date").value;

//    date = date.replace(/-/g, '');

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

//                    let pagerow = "<div style='display: inline-block;margin-left: 170px;'>" + page + "</div>";

//                    pagerow += "<div class='Nbt_div' id='Nbt' style='position: relative;display: inline-block;margin-left: 5px;'><button type='button' class='btn btn-link Nbt' value='1' onclick='GetFile(this.value)'>下一頁</button></div>";

//                    $("#add_img_box").html(pagerow);
//                }

//                if (response.data.length < 5) {
//                    let pagerow = "<div style='display: inline-block;margin-left: 170px;'>" + page + "</div>";


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

//            }
//        },
//        error: function () {
//        }
//    });
//}