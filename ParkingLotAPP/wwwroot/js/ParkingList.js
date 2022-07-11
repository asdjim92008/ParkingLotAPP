window.onload = function () {
    let retrievedObject = new Array()
    retrievedObject = JSON.parse(localStorage.getItem('PLdataObject'));
    var text = "`<tr>";
    for (var i = 0; i < retrievedObject.data.length; i++) {
        if ((i + 1) % 3 == 0) {
            text += "<td><div style='height: 100px;'><div class='card'><div class='card_image'><button value='" + retrievedObject.data[i].parkingGuid + "' style='width: 110px;height: 98px;background-color: transparent;opacity: 0;' onclick='gohome(this.value)'></button><input type='hidden' id='pkguid' name='pkguid' value='" + retrievedObject.data[i].parkingGuid + "'><img class='icicon' src='/images/caricon.png' /></div><div class='card_title title-white'><p class='card_p'>" + retrievedObject.data[i].parkingNo + "<br>" + retrievedObject.data[i].parkingName + "</p></div></div></div></td></tr>";
            text += "<tr>";
        }
        else {
            text += "<td><div style='height: 100px;'><div class='card'><div class='card_image'><button value='" + retrievedObject.data[i].parkingGuid + "' style='width: 110px;height: 98px;background-color: transparent;opacity: 0;' onclick='gohome(this.value)'></button><input type='hidden' id='pkguid' value='" + retrievedObject.data[i].parkingGuid + "'><img class='icicon' src='/images/caricon.png' /></div><div class='card_title title-white'><p class='card_p'>" + retrievedObject.data[i].parkingNo + "<br>" + retrievedObject.data[i].parkingName + "</p></div></div></div></td>";
        }

        if (i == retrievedObject.data.length - 1) {
            text += "</tr><tr>"
            text = text.substring(0, (text.length - 4))
            text += "`";
        }
    }
    $("#PLlist").html(text);

    var companyname = retrievedObject.companyName;
    $("#companyname").html(companyname);
};

function gohome(parkingGuid) {
    var pkguid = parkingGuid;
    $.ajax({
        type: 'GET',
        url: "/api/Data_SQL/AuthorizeData",
        data: { 'parkingGuid': pkguid },
        dataType: 'json',
        async: true,
        success: function (response) {
            localStorage.setItem('ParkingType', JSON.stringify(response.data.parkingType));
            window.location = "/Home/Index?parkingGuid=" + pkguid;
        },
        error: function () {
            alert("error");
        }
    });

}