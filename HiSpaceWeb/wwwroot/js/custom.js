//document ready function
$(function () {
	// We can attach the `fileselect` event to all file inputs on the page
	$(document).on('change', ':file', function () {
		var input = $(this),
			numFiles = input.get(0).files ? input.get(0).files.length : 1,
			label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
		input.trigger('fileselect', [numFiles, label]);
	});

	// We can watch for our custom `fileselect` event like this
	$(document).ready(function () {
		$(':file').on('fileselect', function (event, numFiles, label) {
			var input = $(this).parents('.input-group').find(':text'),
				log = numFiles > 1 ? numFiles + ' files selected' : label;
			if (input.length) {
				input.val(log);
			} else {
				if (log);
			}
		});
	});

	//upload function on employee
	$('#empUpload').on('click', function () {
		var fileExtension = ['xls', 'xlsx'];
		var filename = $('#fUpload').val();
		if (filename.length == 0) {
			alert("Please select a file.");
			return false;
		}
		else {
			var extension = filename.replace(/^.*\./, '');
			if ($.inArray(extension, fileExtension) == -1) {
				alert("Please select only excel files.");
				return false;
			}
		}
		var fdata = new FormData();
		console.log(fdata);
		var fileUpload = $("#fUpload").get(0);
		console.log(fileUpload);
		var files = fileUpload.files;
		console.log(files);
		fdata.append(files[0].name, files[0]);
		console.log(fdata);
		$.ajax({
			type: "POST",
			url: "/Employee/OnPostImport",
			beforeSend: function (xhr) {
				xhr.setRequestHeader("XSRF-TOKEN",
					$('input:hidden[name="__RequestVerificationToken"]').val());
			},
			data: fdata,
			contentType: false,
			processData: false,
			success: function (response) {
				if (response.length == 0)
					alert('Some error occured while uploading');
				else {
					$('#dvData').html(response);
				}
				$('#bulkUploadAlert').modal('show');
			},
			error: function (e) {
				$('#dvData').html(e.responseText);
			}
		});
	})

	//upload function on Attendance
	$('#attenUpload').on('click', function () {
		var fileExtension = ['xls', 'xlsx'];
		var filename = $('#fUpload').val();
		if (filename.length == 0) {
			alert("Please select a file.");
			return false;
		}
		else {
			var extension = filename.replace(/^.*\./, '');
			if ($.inArray(extension, fileExtension) == -1) {
				alert("Please select only excel files.");
				return false;
			}
		}
		var fdata = new FormData();
		console.log(fdata);
		var fileUpload = $("#fUpload").get(0);
		console.log(fileUpload);
		var files = fileUpload.files;
		console.log(files);
		fdata.append(files[0].name, files[0]);
		console.log(fdata);
		$.ajax({
			type: "POST",
			url: "/Attendance/OnPostImport",
			beforeSend: function (xhr) {
				xhr.setRequestHeader("XSRF-TOKEN",
					$('input:hidden[name="__RequestVerificationToken"]').val());
			},
			data: fdata,
			contentType: false,
			processData: false,
			success: function (response) {
				if (response.length == 0)
					alert('Some error occured while uploading');
				else {
					$('#dvData').html(response);
				}
				$('#bulkUploadAlert').modal('show');
			},
			error: function (e) {
				$('#dvData').html(e.responseText);
			}
		});
	})

	//$(document).on('change', '#EmpID', function () {
	//	//console.log($(this).val())
	//	var EmpID = $(this).val();

	//           success: function (response) {
	//           },
	//           error: function (e) {
	//employee is check for relieving
	$('.relieving_date_check input[type=checkbox]').each(function () {
		if ($(this).is(":checked")) {
			$(this).parents('.relieving_date_check').siblings('.relieving_date').css('display', 'block');
		}
		else if ($(this).is(":not(:checked)")) {
			$(this).parents('.relieving_date_check').siblings('.relieving_date').css('display', 'none');
		}
	});

	//amenities for each function
	$('#l_floor_facilities input[type=checkbox]').each(function () {
		if ($(this).is(":checked")) {
			$(this).parents('.am_check').siblings('.am_price').css('display', 'block');
		}
		else if ($(this).is(":not(:checked)")) {
			$(this).parents('.am_check').siblings('.am_price').css('display', 'none');
		}
	});

	//form wizard start

	var navListItems = $('div.setup-panel div a'),
		allWells = $('.setup-content'),
		allNextBtn = $('.nextBtn');

	allWells.hide();

	navListItems.click(function (e) {
		e.preventDefault();
		var $target = $($(this).attr('href')),
			$item = $(this);

		if (!$item.hasClass('disabled')) {
			navListItems.removeClass('btn-primary').addClass('btn-default field-filled');
			$item.addClass('btn-primary');
			allWells.hide();
			$target.show();
			$target.find('input:eq(0)').focus();
		}
	});

	allNextBtn.click(function () {
		var curStep = $(this).closest(".setup-content"),
			curStepBtn = curStep.attr("id"),
			nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
			//curInputs = curStep.find("input[type='text'],input[type='url']"),
			isValid = true;

		$(".form-group").removeClass("has-error");
		//for(var i=0; i<curInputs.length; i++){
		//    if (!curInputs[i].validity.valid){
		//        isValid = false;
		//        $(curInputs[i]).closest(".form-group").addClass("has-error");
		//    }
		//}

		if (isValid)
			nextStepWizard.removeAttr('disabled').trigger('click');
	});

	$('div.setup-panel div a.btn-primary').trigger('click');
	//form wizard end

	//login section for the client and user changin image and text
	$("input[name=login-type]:radio").click(function () {
		if ($('input[name=login-type]:checked').val() == "client") {
			$('.login-image').css("background-image", "linear-gradient(to bottom, rgba(0, 0, 0, 0), rgb(0, 0, 0)), url(../img/bg/login-bg2.jpg)");
			$(".login-head span").html(" - Client");
			$(".login-desc").html("Community to share, combine, offering your workspace for client... Lorem Ipsum is simply dummy text of the printing and typesetting industry.");
		} else if ($('input[name=login-type]:checked').val() == "user") {
			$('.login-image').css("background-image", "linear-gradient(rgba(0, 0, 0, 0) 30%, rgba(0, 0, 0, 0.90) 80%), url(../img/bg/login-bg3.jpg)");
			$(".login-head span").html(" - User");
			$(".login-desc").html("Community to share, combine, offering your workspace for user... Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,");
		}
	});

	/******************datatable section inside the document ready********************/
	//client section datatable
	$('#clientTable').DataTable({
		"bPaginate": true,
		"bSortable": true
	});
	//ClientLocation section datatable
	$('#clientLocationTable').DataTable({
		"bPaginate": true,
		"bSortable": true
	});
	//ClientFloor section datatable
	$('#clientFloorTable').DataTable({
		"bPaginate": true,
		"bSortable": true
	});
	//ClientSpace section datatable
	$('#clientSpaceTable').DataTable({
		"bPaginate": true,
		"bSortable": true
	});
	//User section datatable
	$('#clientAdminTable, #clientUserTable, #clientMemberTable, #clientAllTable').DataTable({
		"bPaginate": true,
		"bSortable": true
	});
	//Member section datatable
	$('#memberTable').DataTable({
		"bPaginate": true,
		"bSortable": true
	});
	//Member section datatable
	$('#SpaceBookingTable').DataTable({
		"bPaginate": true,
		"bSortable": true
	});

	//MembershipPlan section datatable
	$('#membershipTable').DataTable({
		"bPaginate": true,
		"bSortable": true
	});

	//Employee section datatable
	$('#employeeTable').DataTable({
		"bPaginate": true,
		"bSortable": true
	});

	//Attendance section datatable
	$('#attendanceTable').DataTable({
		"bPaginate": true,
		"bSortable": true
	});

	//Lead section datatable
	$('#LeadTable').DataTable({
		"bPaginate": true,
		"bSortable": true
	});

	//Start BMW scripts

	$("#matrixpreview").hide();
	$("#matrixpreviewRight").hide();
	var arrSeatIDS = [];
	var seatList = [];

	$("#ApplyToID").change(function () {
		//console.log($("#matrixDisplay").val());
		//if ($("#matrixDisplay").val() == "2") {
		//    console.log($("#matrixDisplay").val());
		//    if (seatPreviousStatus == "Available") {
		//        $("#SeatStatusID").val(2);
		//        console.log($("#SeatStatusID").val());
		//    }
		//}

		if ($("#SeatStatusID").val() == 0) {
			$("#ApplyToID").val(0);
			alert('Please select seat status');
			return;
		}

		var NoRows = $("#SpaceFloorPlan_NumberOfRows").val();
		var NoColumns = $("#SpaceFloorPlan_NumberOfColumns").val();
		var SelectedRow = selectedSeat.split('_')[1];
		var SelectedColumn = selectedSeat.split('_')[2];
		var SelectedSeatStatus = $("#" + selectedSeat).attr('title');
		var SelectedSeatNewStatus;
		console.log(SelectedSeatStatus);

		if ($("#SeatStatusID").val() == 1) {
			SelectedSeatNewStatus = "Available";
		}
		else if ($("#SeatStatusID").val() == 2) {
			SelectedSeatNewStatus = "Occupied";
		}
		else if ($("#SeatStatusID").val() == 3) {
			SelectedSeatNewStatus = "Blocked";
		}

		console.log(SelectedSeatNewStatus);
		console.log($("#SeatPrice").val());

		if ($("#SeatPrice").val() == '' && SelectedSeatStatus == 'Occupied') {
			$("#ApplyToID").val(0);
			alert('Please enter seat price');
			return;
		}
		if ($("#SeatDescription").val() == '' && SelectedSeatStatus == 'Occupied') {
			$("#ApplyToID").val(0);
			alert('Please enter seat description');
			return;
		}
		console.log($("#SeatStatusID").val());

		arrSeatIDS = [];

		if ($("#ApplyToID").val() == 1) {
			arrSeatIDS.push("#" + selectedSeat);
		}
		else if ($("#ApplyToID").val() == 2) {
			for (j = 1; j <= NoColumns; j++) {
				arrSeatIDS.push("#seat_" + SelectedRow + "_" + j);
			}
		}
		else if ($("#ApplyToID").val() == 3) {
			for (i = 1; i <= NoRows; i++) {
				arrSeatIDS.push("#seat_" + i + "_" + SelectedColumn);
			}
		}
		else if ($("#ApplyToID").val() == 4) {
			for (i = 1; i <= NoRows; i++) {
				for (j = 1; j <= NoColumns; j++) {
					arrSeatIDS.push("#seat_" + i + "_" + j);
				}
			}
		}
		console.log(arrSeatIDS);

		//for (i = 1; i <= NoRows; i++) {
		//    for (j = 1; j <= NoColumns; j++) {
		//        //var seat = {};
		//        //seat.SeatStatus = SelectedSeatNewStatus;
		//        //seat.SeatPrice = $("#SeatPrice").val();
		//        //seat.SeatDescription = $("#SeatDescription").val();

		//        //if (true) {
		//        //    seat.SeatStatus = "Available";
		//        //    seat.SeatPrice = "";
		//        //    seat.SeatDescription = "";
		//        //}
		//        //else {
		//            seat.SeatStatus = SelectedSeatNewStatus;
		//            seat.SeatPrice = $("#SeatPrice").val();
		//            seat.SeatDescription = $("#SeatDescription").val();
		//        //}

		//        seat.SeatXCoord = seatId.split('_')[1];
		//        seat.SeatYCoord = seatId.split('_')[2];
		//        seatList.push(seat);

		//    }
		//}

		//if (SelectedSeatStatus == 'Occupied') {
		$.each(arrSeatIDS, function (index, seatId) {
			var seat = {};
			seat.SeatStatus = SelectedSeatNewStatus;
			seat.SeatPrice = $("#SeatPrice").val();
			seat.SeatDescription = $("#SeatDescription").val();
			seat.SeatXCoord = seatId.split('_')[1];
			seat.SeatYCoord = seatId.split('_')[2];
			seatList.push(seat);
		});

		console.log("--abcd--");
		console.log(seatList);

		var urlAction = "";
		console.log("" + SelectedSeatNewStatus);
		//if (SelectedSeatNewStatus == 'Occupied' || SelectedSeatNewStatus == 'Blocked')
		//    urlAction = "/ClientSpace/RemoveSeats/";
		//else if (SelectedSeatStatus == 'Available')
		urlAction = "/ClientSpace/AddSeats/";
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
						console.log(seatId + SelectedSeatNewStatus);
						if (SelectedSeatNewStatus == "Available") {
							$(seatId).css("background-color", "Green");
							$(seatId).attr('title', 'Available');
						}
						else if (SelectedSeatNewStatus == "Occupied") {
							$(seatId).css("background-color", "Orange");
							$(seatId).attr('title', 'Occupied');
						}
						else if (SelectedSeatNewStatus == "Blocked") {
							$(seatId).css("background-color", "Gray");
							$(seatId).attr('title', 'Blocked');
						}
					});
					document.getElementById("matrixpreview").disabled = false;
					document.getElementById("matrixpreviewRight").disabled = true;
					$("#matrixpreview :button").prop("disabled", false);
					$("#matrixpreview").css('opacity', '10.0');
					$("#matrixpreviewRight").hide();
					$("#SeatStatusID").val(0);
					$("#ApplyToID").val(0);
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
				$("#ApplyToID").val(0);
				$("#SeatPrice").val('');
				$("#SeatDescription").val('');
				//$("#matrixpreview").css('opacity', '10.0');
				alert(xhr.responseText);
			}
		});

		//console.log(arrSeatIDS[0]);
	});

	$("#closeButton").on('click', function () {
		document.getElementById("matrixpreview").disabled = false;
		document.getElementById("matrixpreviewRight").disabled = true;
		$("#matrixpreview :button").prop("disabled", false);
		$("#matrixpreviewRight").hide();
		$("#SeatStatusID").val(0);
		$("#ApplyToID").val(0);
		$("#SeatPrice").val('');
		$("#SeatDescription").val('');
		$("#matrixpreview").css('opacity', '10.0');

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
	$("#butPreview").on('click', function () {
		var MatrixBodyWidth = $("#matrixBody").width();
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
			html += "<th><div class='matrixHeader'>" + j + "</div></th>";
		}
		html += "</tr>";
		html += "</thead>";

		seatList = [];

		for (i = 1; i <= NoRows; i++) {
			html += "<tr>";
			html += "<td><div class='matrixHeader'>" + i + "</div></td>";
			for (j = 1; j <= NoColumns; j++) {
				//var seat = {};
				//seat.SeatStatus = "Available";
				//seat.SeatPrice = $("#SeatPrice").val();
				//seat.SeatDescription = $("#SeatDescription").val();
				//seat.SeatXCoord = i;
				//seat.SeatYCoord = j;
				//seatList.push(seat);

				html += "<td>";
				html += "<input ondblclick='BlockSeat(this);' onclick='SelectSeat(this);' style='width:" + seatSize + ";height:" + seatSize + ";' title='Available' id='seat_" + i + "_" + j + "' type='button' class='matrixButton' />";
				html += "</td>";
			}
			html += "</tr>";
		}
		html += "</table>";

		$("#matrixpreview").html(html);
		$("#matrixpreview").show();

		$.ajax({
			type: "GET",
			url: "/ClientSpace/GetClientSeats/",
			data: {},
			dataType: "json",
			success: function (response) {
				var data = JSON.stringify(response);
				//alert("Hello : " + data);
				$.each(response, function (index, seat) {
					var seatid = "#seat_" + seat.seatXCoord + "_" + seat.seatYCoord;
					//alert(seatid + " : " + seat.seatStatus);
					$(seatid).attr("title", seat.seatStatus);
					if (seat.seatStatus == "Available") {
						$(seatid).css("background-color", "Green");
					}
					if (seat.seatStatus == "Occupied") {
						$(seatid).css("background-color", "Orange");
					}
					if (seat.seatStatus == "Blocked") {
						$(seatid).css("background-color", "Gray");
						$(seatid).attr("disabled", "true");
					}
					//alert(index + "_" + seat.seatXCoord + "_" + seat.seatYCoord + "_" + seat.seatStatus);
				});
			},
			error: function (xhr, ajaxOptions, thrownError) {
				alert("Hi : " + xhr.responseText);
			}
		});
	});

	//attendance section
});

var selectedSeat;
var seatPreviousStatus;

function BlockSeat(obj) {
	if ($(obj).attr('title') == "Blocked") {
		$(obj).css("background-color", "Green");
		$(obj).attr('title', 'Available');

		//document.getElementById("matrixpreview").disabled = true;
		//document.getElementById("matrixpreviewRight").disabled = false;
		//$("#matrixpreview :button").prop("disabled", true);
		//$("#matrixpreview").css('opacity', '0.5');
		$("#matrixpreviewRight").show();
	}
	else {
		$(obj).css("background-color", "Gray");
		$(obj).attr('title', 'Blocked');

		//document.getElementById("matrixpreview").disabled = true;
		//document.getElementById("matrixpreviewRight").disabled = false;
		//$("#matrixpreview :button").prop("disabled", true);
		//$("#matrixpreview").css('opacity', '0.5');
		$("#matrixpreviewRight").show();
	}
}

function SelectSeat(obj) {
	selectedSeat = obj.id;
	seatPreviousStatus = $(obj).attr('title');
	$(obj).css("background-color", "Blue");
	$(obj).attr('title', 'Selected');
	var selectedSeatStatus = $(obj).attr('title');
	if (selectedSeatStatus == 'Selected') {
		document.getElementById("matrixpreview").disabled = true;
		document.getElementById("matrixpreviewRight").disabled = false;
		$("#matrixpreview :button").prop("disabled", true);
		$("#matrixpreview").css('opacity', '0.5');
		$("#SeatStatusID").val(0);
		$("#ApplyToID").val(0);
		$("#SeatPrice").val('');
		$("#SeatDescription").val('');
		$("#matrixpreviewRight").show();
	}
	$("#seatID").html("(Selected Seat Row " + selectedSeat.split('_')[1] + ", Column " + selectedSeat.split('_')[2] + ")");
}

//End BMW scripts

//section for the password hide and show style
$(function () {
	$('[type="password"].input-password').password()
})
function applyStyle(e) {
	$(e).closest('.input-group').next('.md-password').css({ "color": "#8e54e9", "-webkit-transition": "all 0.28s ease", "transition": "all 0.28s ease" });
	$(e).closest('.form-group').find('.bar').addClass('input-bar');
}
function removeStyle(e) {
	$(e).closest('.input-group').next('.md-password').css({ "color": "#5c5c5c", "-webkit-transition": "all 0.28s ease", "transition": "all 0.28s ease" });
	$(e).closest('.form-group').find('.bar').removeClass('input-bar');
}

//$('input[type="checkbox"]').click(function () {
//	if ($(this).prop("checked") == true) {
//		alert("Checkbox is checked.");
//	}
//	else if ($(this).prop("checked") == false) {
//		alert("Checkbox is unchecked.");
//	}
//})

//Action on the client creation submit button
$('input[type="checkbox"]#terms').click(function () {
	if ($(this).prop("checked") == true) {
		$('input[type="submit"]').attr("disabled", false);
		$('input[type="submit"]').css("cursor", "pointer");
	}
	else if ($(this).prop("checked") == false) {
		$('input[type="submit"]').attr("disabled", true);
		$('input[type="submit"]').css("cursor", "no-drop");
	}
})

$('input[type="submit"].bmw-submit').attr("disabled", true);

$(document).ready(function () {
	$("#butPreview").click();
	if ($("#matrixDisplay").val() == '1')
		$("#matrixpreview :button").prop("disabled", true);
	if ($("#matrixDisplay").val() == '2') {
	}
});

//***********************Member section start*****************************//

//discount value calculation
$(".disc-number").bind('change keyup blur', function (e) {
	var actualPrice = parseFloat($('input[name="MemberMaster.MembershipID"]:checked').attr("text"));
	var discount = parseFloat($(this).val());
	var discountedPrice = (actualPrice - (actualPrice * discount / 100)).toFixed(2);
	$('input[id="MemberMaster_DiscountedPrice"]').val(discountedPrice);
});
//radio buttton on change event
$('input[type=radio][name="MemberMaster.MembershipID"]').change(function () {
	$("#MemberMaster_DiscountPercent").val('');
	$("#MemberMaster_DiscountedPrice").val('');
});
//allow only two numbers into the field
$(".disc-number").bind('keydown', function (e) {
	var targetValue = $(this).val();
	if (e.which === 8 || e.which === 13 || e.which === 37 || e.which === 39 || e.which === 46) { return; }

	if (e.which > 47 && e.which < 58 && targetValue.length < 2) {
		var c = String.fromCharCode(e.which);
		var val = parseInt(c);
		var textVal = parseInt(targetValue || "0");
		var result = textVal + val;

		if (result < 0 || result > 99) {
			e.preventDefault();
		}

		if (targetValue === "0") {
			$(this).val(val);
			e.preventDefault();
		}
	}
	else {
		e.preventDefault();
	}
});

function ApproveClient() {
	return confirm("Are you sure want to approve this client verification.");
}

function ApproveSpace() {
	return confirm("Are you sure want to approve this work space verification.");
}

function ApproveMemberBooking() {
	return confirm("Are you sure want to approve this member request.");
}

//***********************Member section end****************************//

//***********************user section start****************************//

function userTypeFilter(obj, type) {
	if (type == 1) {
		//$('.client-admin, .client-user, .client-member').parent().parent().css('display','none');
	}
	else if (type == 2) {
		$(obj).addClass('btn-visited');
		$('#clientUserTable, #clientMemberTable, #clientAllTable, #clientUserTable_wrapper, #clientMemberTable_wrapper, #clientAllTable_wrapper').css('display', 'none');
		$('#clientAdminTable').css('display', 'table');
		$('#clientAdminTable_wrapper').css('display', 'block');
	}
	else if (type == 3) {
		$('#clientAdmin').removeClass('btn-visited');
		$('#clientAdminTable, #clientMemberTable, #clientAllTable, #clientAdminTable_wrapper, #clientMemberTable_wrapper, #clientAllTable_wrapper').css('display', 'none');
		$('#clientUserTable').css('display', 'table');
		$('#clientUserTable_wrapper').css('display', 'block');
	}
	else if (type == 4) {
		$('#clientAdmin').removeClass('btn-visited');
		$('#clientAdminTable, #clientUserTable, #clientAllTable, #clientAdminTable_wrapper, #clientUserTable_wrapper, #clientAllTable_wrapper').css('display', 'none');
		$('#clientMemberTable').css('display', 'table');
		$('#clientMemberTable_wrapper').css('display', 'block');
	}
	else if (type == 5) {
		$('#clientAdmin').removeClass('btn-visited');
		$('#clientAdminTable, #clientUserTable, #clientMemberTable, #clientAdminTable_wrapper, #clientUserTable_wrapper, #clientMemberTable_wrapper').css('display', 'none');
		$('#clientAllTable').css('display', 'table');
		$('#clientAllTable_wrapper').css('display', 'block');
	}
}

$(function () {
	$('#clientAdmin').click();
	//checkbox check function for scheduling

	//console.log($('#AllTimeCheck').val());
	//console.log($('#MonToFriCheck').val());
	//console.log($('#MonToSatCheck').val());
	//console.log($('#CustomCheck').val());

	if ($('#AllTimeCheck option:selected').text() == "True") {
		$('#AllTimeCheck').parents('label').find('[type=radio]').prop("checked", true);
		$('#AllTimeCheck').parents('label').addClass('radio-active');
	}
	else if ($('#MonToFriCheck option:selected').text() == "True") {
		$('#MonToFriCheck').parents('label').find('[type=radio]').prop("checked", true);
		$('#MonToFriCheck').parents('.radio').siblings().removeClass('cursor-no-drop');
		$('#MonToFriCheck').parents('.radio').siblings().children().removeClass('pointer-event-none');
		$('#MonToFriCheck').parents('.radio').siblings().find('input').prop("checked", true);
		$('#MonToFriCheck').parents('.radio').siblings().find('input').parents('.sch-checkbox').siblings('.sch-time').css('display', 'block');
		$('#MonToFriCheck').parents('.radio').siblings().find('input').siblings('strong').html('Open');
		$('#MonToFriCheck').parents('label').addClass('radio-active');
	}
	else if ($('#MonToSatCheck option:selected').text() == "True") {
		$('#MonToSatCheck').parents('label').find('[type=radio]').prop("checked", true);
		$('#MonToSatCheck').parents('.radio').siblings().removeClass('cursor-no-drop');
		$('#MonToSatCheck').parents('.radio').siblings().children().removeClass('pointer-event-none');
		$('#MonToSatCheck').parents('.radio').siblings().find('input').prop("checked", true);
		$('#MonToSatCheck').parents('.radio').siblings().find('input').parents('.sch-checkbox').siblings('.sch-time').css('display', 'block');
		$('#MonToSatCheck').parents('.radio').siblings().find('input').siblings('strong').html('Open');
		$('#MonToSatCheck').parents('label').addClass('radio-active');
	}
	else if ($('#CustomCheck option:selected').text() == "True") {
		$('#CustomCheck').parents('label').find('[type=radio]').prop("checked", true);
		$('#CustomCheck').parents('.radio').siblings().removeClass('cursor-no-drop');
		$('#CustomCheck').parents('.radio').siblings().children().removeClass('pointer-event-none');
		$('#CustomCheck').parents('.radio').siblings().find('input').prop("checked", true);
		$('#CustomCheck').parents('.radio').siblings().find('input').parents('.sch-checkbox').siblings('.sch-time').css('display', 'block');
		$('#CustomCheck').parents('.radio').siblings().find('input').siblings('strong').html('Open');
		$('#CustomCheck').parents('label').addClass('radio-active');
	}

	//display view in the scheduler section
	if ($('#Display_view #AllTimeCheck option:selected').text() == "True") {
		$('.sch-2, .sch-3, .sch-4').css('display', 'none');
		$('.sch-1').css('display', 'block');
	}
	else if ($('#Display_view #MonToFriCheck option:selected').text() == "True") {
		$('.sch-1, .sch-3, .sch-4').css('display', 'none');
		$('.sch-2').css('display', 'block');
	}
	else if ($('#Display_view #MonToSatCheck option:selected').text() == "True") {
		$('.sch-1, .sch-2, .sch-4').css('display', 'none');
		$('.sch-3').css('display', 'block');
	}
	else if ($('#Display_view #CustomCheck option:selected').text() == "True") {
		$('.sch-1, .sch-2, .sch-3').css('display', 'none');
		$('.sch-4').css('display', 'block');
	}
});
//***********************user section end****************************//

$('input[name="ClientSpaceFloorPlan.Is24"]').click(function () {
	//$('input[name="AllTimeCheck"], input[name="MonToFriCheck"], input[name="MonToSatCheck"], input[name="CustomCheck"]').click(function () {
	//console.log($('#AllTimeCheck option:selected').text());
	$('#AllTimeCheck').val("false");
	$('#MonToFriCheck').val("false");
	$('#MonToSatCheck').val("false");
	$('#CustomCheck').val("false");
	if ($(this).prop("checked")) {
		$(this).siblings('select').val("true");
		//console.log($('#AllTimeCheck option:selected').text());
	}

	//alert($(this).attr('radio-attr'))
	$('.form-radio label').removeClass('radio-active');
	$(this).parents('label').addClass('radio-active');
	if ($(this).attr('radio-attr') == "24/7") {
		$('.sch-custom, .sch-weekdays, .sch-weeksaturday').addClass('cursor-no-drop');
		$('.sch-custom > div, .sch-weekdays > div, .sch-weeksaturday > div').addClass('pointer-event-none');
	}
	else if ($(this).attr('radio-attr') == "weekdays") {
		$('.sch-weekdays').removeClass('cursor-no-drop');
		$('.sch-weekdays > div').removeClass('pointer-event-none');

		$('.sch-custom, .sch-weeksaturday').addClass('cursor-no-drop');
		$('.sch-custom > div, .sch-weeksaturday > div').addClass('pointer-event-none');
	}
	else if ($(this).attr('radio-attr') == "weeksaturday") {
		$('.sch-weeksaturday').removeClass('cursor-no-drop');
		$('.sch-weeksaturday > div').removeClass('pointer-event-none');

		$('.sch-custom, .sch-weekdays').addClass('cursor-no-drop');
		$('.sch-custom > div, .sch-weekdays > div').addClass('pointer-event-none');
	}
	else if ($(this).attr('radio-attr') == "custom") {
		$('.sch-custom').removeClass('cursor-no-drop');
		$('.sch-custom > div').removeClass('pointer-event-none');

		$('.sch-weekdays, .sch-weeksaturday').addClass('cursor-no-drop');
		$('.sch-weekdays > div, .sch-weeksaturday > div').addClass('pointer-event-none');
	}
});

$(".sch-status").click(function () {
	if ($(this).prop("checked") == true) {
		$(this).parents('.sch-checkbox').siblings('.sch-time').css('display', 'block');
		$(this).siblings('strong').html('Open');
	}
	else if ($(this).prop("checked") == false) {
		$(this).parents('.sch-checkbox').siblings('.sch-time').css('display', 'none');
		$(this).siblings('strong').html('Closed');
	}
});

//$('.sch_open, .sch_close').on('change', function () {
//    if (this.value == "23:59:59") {
//        $(this).parents('.col-lg-6').siblings('.col-lg-6').css('display', 'none');
//    } else {
//        $(this).parents('.col-lg-6').siblings('.col-lg-6').css('display', 'block');
//    }
//});

$('#l_floor_facilities input[type=checkbox]').change(function () {
	if ($(this).is(":checked")) {
		$(this).parents('.am_check').siblings('.am_price').css('display', 'block');
	}
	else if ($(this).is(":not(:checked)")) {
		$(this).parents('.am_check').siblings('.am_price').css('display', 'none');
	}
})

//employee is check for relieving
$('.relieving_date_check input[type=checkbox]').change(function () {
	if ($(this).is(":checked")) {
		$(this).parents('.relieving_date_check').siblings('.relieving_date').css('display', 'block');
	}
	else if ($(this).is(":not(:checked)")) {
		$(this).parents('.relieving_date_check').siblings('.relieving_date').css('display', 'none');
	}
});