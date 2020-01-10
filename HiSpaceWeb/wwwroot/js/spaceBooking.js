//document ready function
$(function () {

    //form wizard start



    //Start BMW scripts

    $("#matrixpreviewSpaceBook").hide();
    $("#matrixpreviewRightSpaceBook").hide();

    $("#ApplyToIDSpaceBook").change(function () {

        //console.log($("#matrixDisplay").val());
        //if ($("#matrixDisplay").val() == "2") {
        //    console.log($("#matrixDisplay").val());
        //    if (seatPreviousStatus == "Available") {
        //        $("#SeatStatusID").val(2);
        //        console.log($("#SeatStatusID").val());
        //    }
        //}
        console.log('hi hello');
        if ($("#ApplyToIDSpaceBook").val() == 0) {
            return;
        }

        var NoRows = $("#SpaceFloorPlan_NumberOfRows").val();
        var NoColumns = $("#SpaceFloorPlan_NumberOfColumns").val();
        var SelectedRow = selectedSeat.split('_')[1];
        var SelectedColumn = selectedSeat.split('_')[2];
        var SelectedSeatStatus = $("#" + selectedSeat).attr('title');
        var SelectedSeatNewStatus;

        if ($("#SeatPrice").val() == '' && SelectedSeatStatus == 'Occupied') {
            $("#ApplyToIDSpaceBook").val(0);
            alert('Please enter seat price');
            return;
        }
        if ($("#SeatDescription").val() == '' && SelectedSeatStatus == 'Occupied') {
            $("#ApplyToIDSpaceBook").val(0);
            alert('Please enter seat description');
            return;
        }
        console.log($("#SeatStatusID").val());


        var arrSeatIDS = [];
        if ($("#ApplyToIDSpaceBook").val() == 1) {
            arrSeatIDS.push("#" + selectedSeat);
        }
        else if ($("#ApplyToIDSpaceBook").val() == 2) {
            for (j = 1; j <= NoColumns; j++) {
                arrSeatIDS.push("#seat_" + SelectedRow + "_" + j);
            }
        }
        else if ($("#ApplyToIDSpaceBook").val() == 3) {
            for (i = 1; i <= NoRows; i++) {
                arrSeatIDS.push("#seat_" + i + "_" + SelectedColumn);
            }
        }
        else if ($("#ApplyToIDSpaceBook").val() == 4) {
            for (i = 1; i <= NoRows; i++) {
                for (j = 1; j <= NoColumns; j++) {
                    arrSeatIDS.push("#seat_" + i + "_" + j);
                }
            }
        }
        console.log(arrSeatIDS);
        var seatList = [];

        //if (SelectedSeatStatus == 'Occupied') {
        $.each(arrSeatIDS, function (index, seatId) {
            var seat = {};
            seat.SeatStatus = SelectedSeatStatus;
            seat.SeatPrice = $("#SeatPrice").val();
            seat.SeatDescription = $("#SeatDescription").val();
            seat.SeatXCoord = seatId.split('_')[1];
            seat.SeatYCoord = seatId.split('_')[2];
            seatList.push(seat);
        });

        console.log("--abcd--");
        console.log(seatList);

        var urlAction = "";

        //if (SelectedSeatNewStatus == 'Occupied' || SelectedSeatNewStatus == 'Blocked')
        urlAction = "/SpaceBooking/AddSeats/";
        //else if (SelectedSeatStatus == 'Available')
        //  urlAction = "/ClientSpace/RemoveSeats/";
        $.ajax({
            type: "POST",
            url: urlAction,
            data: { SeatList: seatList },
            dataType: "json",
            success: function (response) {
                console.log("--" + 5 + "--");
                //var rs = JSON.stringify(response);
                //alert("Hello : " + rs.Message + response.Message);
                //if (response.responseText == "Added")
                {

                    $.each(arrSeatIDS, function (index, seatId) {
                        if (SelectedSeatStatus == "Available") {
                            $(seatId).css("background-color", "Green");
                            $(seatId).attr('title', 'Available');
                        }
                        else if (SelectedSeatStatus == "Occupied") {
                            $(seatId).css("background-color", "Orange");
                            $(seatId).attr('title', 'Occupied');
                        }
                        else if (SelectedSeatStatus == "Blocked") {
                            $(seatId).css("background-color", "Gray");
                            $(seatId).attr('title', 'Blocked');
                        }
                    });
                    document.getElementById("matrixpreviewSpaceBook").disabled = false;
                    document.getElementById("matrixpreviewRightSpaceBook").disabled = true;
                    $("#matrixpreviewSpaceBook :button").prop("disabled", false);
                    $("#matrixpreviewSpaceBook").css('opacity', '10.0');
                    $("#matrixpreviewRightSpaceBook").hide();
                    $("#SeatStatusID").val(0);
                    $("#ApplyToIDSpaceBook").val(0);
                    $("#SeatPrice").val('');
                    $("#SeatDescription").val('');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log("--" + 6 + "--");
                //document.getElementById("matrixpreview").disabled = false;
                //document.getElementById("matrixpreviewRight").disabled = true;
                //$("#matrixpreview :button").prop("disabled", false);
                //$("#matrixpreviewRight").hide();
                $("#SeatStatusID").val(0);
                $("#ApplyToIDSpaceBook").val(0);
                $("#SeatPrice").val('');
                $("#SeatDescription").val('');
                //$("#matrixpreview").css('opacity', '10.0');        
                alert(xhr.responseText);
            }
        });

        //console.log(arrSeatIDS[0]);

    });

    $("#closeButtonSpaceBook").on('click', function () {
        document.getElementById("matrixpreviewSpaceBook").disabled = false;
        document.getElementById("matrixpreviewRightSpaceBook").disabled = true;
        $("#matrixpreviewSpaceBook :button").prop("disabled", false);
        $("#matrixpreviewRightSpaceBook").hide();
        $("#SeatStatusID").val(0);
        $("#ApplyToIDSpaceBook").val(0);
        $("#SeatPrice").val('');
        $("#SeatDescription").val('');
        $("#matrixpreviewSpaceBook").css('opacity', '10.0');

        if (seatPreviousStatus == "Available") {
            $("#" + selectedSeat).css("background-color", "Green");
            $("#" + selectedSeat).attr('title', 'Available');
        }
        if (seatPreviousStatus == "Occupied") {
            $("#" + selectedSeat).css("background-color", "Orange");
            $("#" + selectedSeat).attr('title', 'Occupied');
        }
        if (seatPreviousStatus == "Blocked") {
            $("#" + selectedSeat).css("background-color", "Gray");
            $("#" + selectedSeat).attr('title', 'Blocked');
        }

    });

    //$("#butPreview").click(function () {
    $("#butPreviewSpaceBook").on('click', function () {
        console.log('asdasd')
        var MatrixBodyWidth = $("#matrixBodySpaceBook").width();
        var FloorLength = $("#SpaceFloorPlan_FloorLength").val();
        var FloorBreadth = $("#SpaceFloorPlan_FloorBreadth").val();
        var NoRows = $("#SpaceFloorPlan_NumberOfRows").val();
        var NoColumns = $("#SpaceFloorPlan_NumberOfColumns").val();

        var seatSize = MatrixBodyWidth / NoColumns;


        var html = "";

        html += "<table class='tblMatrix'>";
        html += "<thead>";
        html += "<tr>";
        html += "<th><div class='matrixHeader'></div></th>";
        for (j = 1; j <= NoColumns; j++) {
            //html += "<th><div class='matrixHeader'>" + j + "</div></th>";
        }
        html += "</tr>";
        html += "</thead>";

        for (i = 1; i <= NoRows; i++) {
            html += "<tr>";
            //html += "<td><div class='matrixHeader'>" + i + "</div></td>";
            for (j = 1; j <= NoColumns; j++) {
                html += "<td>";
                html += "<input ondblclick='BlockSeatSpaceBook(this);' onclick='SelectSeatSpaceBook(this);' style='width:" + seatSize + ";height:" + seatSize + ";' title='Available' id='seat_" + i + "_" + j + "' type='button' class='matrixButton' />";
                html += "</td>";
            }
            html += "</tr>";
        }
        html += "</table>";

        $("#matrixpreviewSpaceBook").html(html);
        $("#matrixpreviewSpaceBook").show();

        $.ajax({
            type: "GET",
            url: "/SpaceBooking/GetClientSeats/",
            data: {},
            dataType: "json",
            success: function (response) {
                var data = JSON.stringify(response);
                //alert("Hello : " + data);
                $.each(response, function (index, seat) {
                    var seatid = "#seat_" + seat.seatXCoord + "_" + seat.seatYCoord;
                    //alert(seatid + " : " + seat.seatStatus);                    
                    if (seat.seatStatus == "Available") {
                        $(seatid).css("background-color", "Green");
                        //$(seatid).attr("title", seat.seatStatus + "\nPrice: " + seat.seatPrice + "\n" + seat.seatDescription);
                        $(seatid).attr("title", seat.seatStatus);
                    }
                    if (seat.seatStatus == "Occupied") {
                        $(seatid).css("background-color", "Orange");
                        //$(seatid).attr("title", seat.seatStatus + "\nPrice: " + seat.seatPrice + "\n" + seat.seatDescription);
                        $(seatid).attr("title", seat.seatStatus);
                    }
                    if (seat.seatStatus == "Blocked") {
                        $(seatid).css("background-color", "Gray");
                        $(seatid).attr("disabled", "true");
                        $(seatid).attr("title", seat.seatStatus);
                    }
                    //alert(index + "_" + seat.seatXCoord + "_" + seat.seatYCoord + "_" + seat.seatStatus);
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert("Hi : " + xhr.responseText);
            }
        });

    });

});

var selectedSeatSpaceBook;
var seatPreviousStatusSpaceBook;

function BlockSeatSpaceBook(obj) {

    if ($(obj).attr('title') == "Blocked") {
        $(obj).css("background-color", "Green");
        $(obj).attr('title', 'Available');

        //document.getElementById("matrixpreview").disabled = true;
        //document.getElementById("matrixpreviewRight").disabled = false;
        //$("#matrixpreview :button").prop("disabled", true);
        //$("#matrixpreview").css('opacity', '0.5');
        $("#matrixpreviewRightSpaceBook").show();
    }
    else {
        $(obj).css("background-color", "Gray");
        $(obj).attr('title', 'Blocked');

        //document.getElementById("matrixpreview").disabled = true;
        //document.getElementById("matrixpreviewRight").disabled = false;
        //$("#matrixpreview :button").prop("disabled", true);
        //$("#matrixpreview").css('opacity', '0.5');
        $("#matrixpreviewRightSpaceBook").show();
    }
}

function SelectSeatSpaceBook(obj) {
    selectedSeat = obj.id;
    seatPreviousStatus = $(obj).attr('title');

    console.log(seatPreviousStatus);

    if (seatPreviousStatus == "Available") {
        $(obj).css("background-color", "Orange");
        $(obj).attr('title', 'Occupied');
    }

    var selectedSeatStatus = $(obj).attr('title');
    if (selectedSeatStatus == 'Occupied') {
        document.getElementById("matrixpreviewSpaceBook").disabled = true;
        document.getElementById("matrixpreviewRightSpaceBook").disabled = false;
        $("#matrixpreviewSpaceBook :button").prop("disabled", true);
        $("#matrixpreviewSpaceBook").css('opacity', '0.5');
        $("#SeatStatusID").val(0);
        $("#ApplyToIDSpaceBook").val(0);
        $("#SeatPrice").val('');
        $("#SeatDescription").val('');
        $("#matrixpreviewRightSpaceBook").show();
    }
    $("#seatIDSpaceBook").html("(Selected Seat Row " + selectedSeat.split('_')[1] + ", Column " + selectedSeat.split('_')[2] + ")");
}

//End BMW scripts


$(document).ready(function () {
    $("#butPreviewSpaceBook").click();
    if ($("#matrixDisplaySpaceBook").val() == '1')
        $("#matrixpreviewSpaceBook :button").prop("disabled", true);
    if ($("#matrixDisplaySpaceBook").val() == '2') {

    }
});