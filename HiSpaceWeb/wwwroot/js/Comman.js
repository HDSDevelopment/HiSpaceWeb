/*-----------------------------------------*/
/*Apllicable for both the admin and website*/
/*-----------------------------------------*/

/*-----------------------------------------------------------------------------------*/
/*------------------------------seat booking section start---------------------------*/
/*-----------------------------------------------------------------------------------*/

function ApplyPicker() {
	$('#DOJ, #DOR, #FromDate, #ToDate').datetimepicker({
		//debug: true,
		format: 'YYYY-MM-DD',
	});
}

$(document).ready(function () {
	//website media query

	//ready event
	if ($(window).width() <= 470) {
		$('.need').html("<i class='flaticon-work-table'></i>");
		$('.share').html("<i class='flaticon-work-table'></i>");
	} else {
		$('.need').html("Workspace");
		$('.share').html("MySpace");
	}

	if ($(window).width() <= 422)
		$('.nav-left .nav-link.link-to-website').html("<i class='ti ti-world pr-10'></i> Website")
	else
		$('.nav-left .nav-link.link-to-website').html("<i class='ti ti-world pr-10'></i> Book My Workspace")

	//resize event
	$(window).resize(function () {
		if ($(window).width() <= 470) {
			$('.need').html("<i class='flaticon-work-table'></i>");
			$('.share').html("<i class='flaticon-work-table'></i>");
		} else {
			$('.need').html("Workspace");
			$('.share').html("MySpace");
		}
		if ($(window).width() <= 422)
			$('.nav-left .nav-link.link-to-website').html("<i class='ti ti-world pr-10'></i> Website")
		else
			$('.nav-left .nav-link.link-to-website').html("<i class='ti ti-world pr-10'></i> Book My Workspace")
	});

	ApplyPicker();

	$('#from_date').datetimepicker({
		//debug: true,
		//format: 'L',
		//format: 'DD-MM-YYYY hh:mm A',
		format: 'DD/MM/YYYY',
		minDate: moment()
	});
	$('#to_date').datetimepicker({
		useCurrent: false,
		//format: 'L',
		//debug: true,
		format: 'DD/MM/YYYY',
	});
	$("#from_date").on("dp.change", function (e) {
		$('#to_date').data("DateTimePicker").minDate(e.date);
	});
	$("#to_date").on("dp.change", function (e) {
		$('#from_date').data("DateTimePicker").maxDate(e.date);
	});
	$('.selectpicker').selectpicker("refresh");
	// Initialize Editor
	$('.textarea-editor').summernote({
		//height: 200, // set editor height
		minHeight: 150 // set minimum height of editor
		//maxHeight: null, // set maximum height of editor
		//focus: true // set focus to editable area after initializing summernote
	});

	LoadNotification();
	//tab navigation section
	if ($('.hi-tab .active.show').attr('data-id')) {
		$('.tab-back-btn, .tab-submit').css('display', 'none');
	}

	// Select and loop the container element of the elements you want to equalise
	$('.even-div').each(function () {
		// Cache the highest
		var highestBox = 0;
		// Select and loop the elements you want to equalise
		$('.even-column', this).each(function () {
			// If this box is higher than the cached highest then store it
			if ($(this).height() > highestBox) {
				highestBox = $(this).height();
			}
		});
		// Set the height of all those children to whichever was highest
		$('.even-column', this).height(highestBox);
	});
});

/*--------seat booking section admin start---------*/
var oldState = "";
var activeInputTd = "";
var activeTr = "";

var SubMenuSingle = "Single";
var SubMenuRow = "Row";
var SubMenuColumn = "Column";
var SubMenuAll = "All";

$('#btnMatrixPreview').on('click', function () {
	var NoRows = $('#ClientSpaceFloorPlan_NumberOfRows').val();
	//var NoRows = 8;
	var NoColumns = $('#ClientSpaceFloorPlan_NumberOfColumns').val();
	//var NoColumns = 8;
	var seatSize = 35;
	//console.log(NoRows, NoColumns, seatSize)
	var html = "";

	html += "<table class='hs_matrixTable'>";
	html += "<thead>";
	html += "<tr>";
	html += "<th><div class='hs_matrixTable__header'></div></th>";
	for (j = 1; j <= NoColumns; j++) {
		html += "<th><div class='hs_matrixTable__header'>" + j + "</div></th>";
	}
	html += "</tr>";
	html += "</thead>";

	for (i = 1; i <= NoRows; i++) {
		html += "<tr>";
		html += "<td><div class='hs_matrixTable__header'>" + i + "</div></td>";
		for (j = 1; j <= NoColumns; j++) {
			html += "<td class='dropdown'>";
			html += "<input ondblclick='wsSeatDetails(this);' onClick='wsState(this)' style='width:" + seatSize + "px;height:" + seatSize + "px;' title='Floorspace' seatstatus='Floorspace' seatmatrix='" + i + ":" + j + "' id='seat_" + i + "_" + j + "' data-toggle='dropdown' type='button' class='matrixButton dropbtn dropdown-toggle singleMatrixCell' name='seat_" + i + "_" + j + "' userGivenName='' description='' price='' /><i class=''></i></input>";
			html += "<ul class='dropdown-menu level1'>" +

				//Available section sub dropdown level
				"<li class='dropdown-submenu dropdown-content ws-dropdown'>" +
				"<a tabindex='-1' href='javascript:void(0);' seatstatus='Available' seatmatrix='" + i + ":" + j + "'  class='ws-seathint__available subLevel' ><span><input type='button' class='matrixButton'><i class='flaticon-furniture-and-household'></i></span><span class='ws-seathint__heading'>Available</span><i class='fa fa-caret-right' aria-hidden='true'></i></a>" +
				"<ul class='dropdown-menu level2'>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsAction(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Available' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='flaticon-furniture-and-household'></i></span><span class='ws-seathint__heading'>" + SubMenuSingle + "</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsSelectRow(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Available Row' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-arrows-h' aria-hidden='true'></i></span><span class='ws-seathint__heading'>" + SubMenuRow + "</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsSelectColumn(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Available Column' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-arrows-v' aria-hidden='true'></i></span><span class='ws-seathint__heading'>" + SubMenuColumn + "</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsCompleteSpace(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Available Complete Space' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-th' aria-hidden='true'></i></span><span class='ws-seathint__heading'>" + SubMenuAll + "</span></a></li>" +
				"</ul>" +
				"</li>" +

				//Booked section sub dropdown level
				"<li class='dropdown-submenu dropdown-content ws-dropdown'>" +
				"<a tabindex='-1' href='javascript:void(0);' seatstatus='Booked' seatmatrix='" + i + ":" + j + "' class='ws-seathint__booked subLevel'><span><input type='button' class='matrixButton'><i class='flaticon-furniture-and-household'></i></span><span class='ws-seathint__heading'>Booked</span><i class='fa fa-caret-right' aria-hidden='true'></i></a>" +
				"<ul class='dropdown-menu level2'>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsAction(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Booked' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='flaticon-furniture-and-household'></i></span><span class='ws-seathint__heading'>" + SubMenuSingle + "</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsSelectRow(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Booked Row' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-arrows-h' aria-hidden='true'></i></span><span class='ws-seathint__heading'>" + SubMenuRow + "</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsSelectColumn(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Booked Column' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-arrows-v' aria-hidden='true'></i></span><span class='ws-seathint__heading'>" + SubMenuColumn + "</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsCompleteSpace(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Booked Complete Space' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-th' aria-hidden='true'></i></span><span class='ws-seathint__heading'>" + SubMenuAll + "</span></a></li>" +
				"</ul>" +
				"</li>" +

				//Unavailable section sub dropdown level
				"<li class='dropdown-submenu dropdown-content ws-dropdown'>" +
				"<a tabindex='-1' href='javascript:void(0);' seatstatus='Unavailable' seatmatrix='" + i + ":" + j + "' class='ws-seathint__unavailable subLevel'><span><input type='button' class='matrixButton'><i class='flaticon-furniture-and-household'></i></span><span class='ws-seathint__heading'>Unavailable</span><i class='fa fa-caret-right' aria-hidden='true'></i></a>" +
				"<ul class='dropdown-menu level2'>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsAction(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Unavailable' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='flaticon-furniture-and-household'></i></span><span class='ws-seathint__heading'>" + SubMenuSingle + "</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsSelectRow(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Unavailable Row' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-arrows-h' aria-hidden='true'></i></span><span class='ws-seathint__heading'>" + SubMenuRow + "</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsSelectColumn(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Unavailable Column' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-arrows-v' aria-hidden='true'></i></span><span class='ws-seathint__heading'>" + SubMenuColumn + "</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsCompleteSpace(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Unavailable Complete Space' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-th' aria-hidden='true'></i></span><span class='ws-seathint__heading'>" + SubMenuAll + "</span></a></li>" +
				"</ul>" +
				"</li>" +

				//Floorspace section sub dropdown level
				"<li class='dropdown-submenu dropdown-content ws-dropdown'>" +
				"<a tabindex='-1' href='javascript:void(0);' seatstatus='Floorspace' seatmatrix='" + i + ":" + j + "' class='ws-seathint__blocked subLevel'><span><input type='button' class='matrixButton'></span><span class='ws-seathint__heading'>De-Select</span><i class='fa fa-caret-right' aria-hidden='true'></i></a>" +
				"<ul class='dropdown-menu level2'>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsAction(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat FloorSpace' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='flaticon-furniture-and-household'></i></span><span class='ws-seathint__heading'>" + SubMenuSingle + "</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsSelectRow(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat FloorSpace Row' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-arrows-h' aria-hidden='true'></i></span><span class='ws-seathint__heading'>" + SubMenuRow + "</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsSelectColumn(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat FloorSpace Column' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-arrows-v' aria-hidden='true'></i></span><span class='ws-seathint__heading'>" + SubMenuColumn + "</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsCompleteSpace(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat FloorSpace Complete Space' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-th' aria-hidden='true'></i></span><span class='ws-seathint__heading'>" + SubMenuAll + "</span></a></li>" +
				"</ul>" +
				"</li>" +

				"</ul>";

			html += "</td>";
		}
		html += "</tr>";
	}
	html += "</table>";

	//overlay fade section
	html += "<div id='tableNoDrop' class=''><div style='display:none;'>Please fill the details</div></div>";

	$("#admin_matrixpreview").html(html);
	$("#admin_matrixpreview").show();
	wsReadSeats();
});

function wsReadSeats() {
	var html = "";
	urlAction = "/ClientSpace/ReadClientSeats/";
	$.ajax({
		type: "GET",
		url: urlAction,
		dataType: "json",
		success: function (response) {
			if (response) {
				$.each(response, function (index, item) {
					var id = 'seat_' + item['seatXCoord'] + '_' + item['seatYCoord'];
					var idCol = item['seatXCoord'] + ':' + item['seatYCoord'];
					$('#' + id).attr('seatstatus', item['seatStatus']);
					$('#' + id).attr('title', item['seatStatus']);
					$('#' + id).attr('name', id);
					$('#' + id).attr('price', item['seatPrice']);
					$('#' + id).attr('seatmatrix', idCol);
					$('#' + id).attr('description', item['seatDescription']);

					if (item['seatStatus'] == "Available") {
						$('#' + id).parents('td').addClass('ws-seathint__available');
						$('#' + id).siblings('i').addClass('flaticon-furniture-and-household');
					}
					else if (item['seatStatus'] == "Booked") {
						$('#' + id).parents('td').addClass('ws-seathint__booked');
						$('#' + id).siblings('i').addClass('flaticon-furniture-and-household');
					}
					else if (item['seatStatus'] == "Unavailable") {
						$('#' + id).parents('td').addClass('ws-seathint__unavailable');
						$('#' + id).siblings('i').addClass('flaticon-furniture-and-household');
					}
					else {
						$('#' + id).parents('td').addClass('ws-seathint__floorspace');
						$('#' + id).siblings('i').addClass('flaticon-furniture-and-household');
					}
				});
			}
		},
		error: function (xhr, ajaxOptions, thrownError) {
			console.log("--" + 6 + "--");
			alert(xhr.responseText);
		}
	});
}
//state maintaines
function wsState(obj) {
	oldState = $(obj).attr('seatstatus');
	activeTr = $(obj).parents('tr').index() + 1;
	activeInputTd = $(obj).parents('td').index();
	//console.log(oldState);
	//console.log(activeTr);
	//console.log(activeInputTd);
}

//function for all available, booked, unavailable, floorspace, row, column, complete.....
function wsAction(obj) {
	var seatTd = $(obj).parents('td');
	var seat = seatTd.children('input');
	var seatCellIndex = seatTd.index();
	var seatStatus = seat.attr('seatstatus');
	var subMenu = $(obj).parents('.level2');
	var ParentMenu = $(obj).parents('.level1');
	var seatDropdownStatus = $(obj).parents('.dropdown-submenu').children('a').attr('seatstatus');

	//hide the dropdown menus
	subMenu.hide();
	ParentMenu.removeClass('show');
	//seat status is Available

	if (seatDropdownStatus == "Available") {
		seatTd.removeClass('ws-seathint__unavailable ws-seathint__booked');
		seat.removeClass('ws-seatblocked');
		seat.next(i).addClass('flaticon-furniture-and-household');
		seat.attr('seatstatus', 'Available');
		seat.attr('title', 'Available');
		$('.hs_matrixTable input').removeClass('selected-cell-border');
		seat.addClass('selected-cell-border');
		//seatDetails called
		seatDetails(obj);
	}

	//seat status is Booked
	else if (seatDropdownStatus == "Booked") {
		seatTd.removeClass('ws-seathint__unavailable');
		seatTd.addClass('ws-seathint__booked');
		seat.removeClass('ws-seatblocked');
		seat.next(i).addClass('flaticon-furniture-and-household');
		seat.attr('title', 'Booked');
		seat.attr('seatstatus', 'Booked');
		$('.hs_matrixTable input').removeClass('selected-cell-border');
		seat.addClass('selected-cell-border');
		//seatDetails called
		seatDetails(obj);
	}

	//seat status is Unavailable
	else if (seatDropdownStatus == "Unavailable") {
		seatTd.removeClass('ws-seathint__booked');
		seatTd.addClass('ws-seathint__unavailable');
		seat.removeClass('ws-seatblocked');
		seat.next(i).addClass('flaticon-furniture-and-household');
		seat.attr('title', 'Unavailable');
		seat.attr('seatstatus', 'Unavailable');
		$('.hs_matrixTable input').removeClass('selected-cell-border');
		seat.addClass('selected-cell-border');
		//seatDetails called
		seatDetails(obj);
	}

	//seat status is Floorspace
	else if (seatDropdownStatus == "Floorspace") {
		seatTd.removeClass('ws-seathint__unavailable ws-seathint__booked');
		seat.next(i).removeClass('flaticon-furniture-and-household');
		seat.addClass('ws-seatblocked');
		seat.attr('title', 'Floorspace');
		seat.attr('seatstatus', 'Floorspace');
		$('.hs_matrixTable input').removeClass('selected-cell-border');
		seat.addClass('selected-cell-border');
		//seatDetails called
		//seatDetails(obj);
		console.log('-a-');
		spaceDetailsApply(obj, 'Single', seatDropdownStatus);
		console.log('-b-');
	}
}
//select row section
function wsSelectRow(obj) {
	//console.log(dropdownSublevel);

	var seatDropdownLevel1 = $(obj).parents('.level2').siblings('a');
	var seat = $(obj).parents('.level1').siblings('input');
	var seatStatus = seatDropdownLevel1.attr('seatstatus');
	console.log(seatStatus);
	var seatMatrix = $(obj).parents('.level1').siblings('input').attr('seatmatrix');
	//console.log(seat.attr('seatstatus'));
	//console.log(seatMatrix);

	var seatList = $(obj).parents().closest('tr').find('input');
	var seatListRow = $(obj).parents().closest('tr').find('td');
	var seatListIcon = $(obj).parents().closest('tr').find('input.singleMatrixCell~i');
	var subMenu = $(obj).parents('.level2');
	var ParentMenu = $(obj).parents('.level1');
	//console.log(seatList.html());
	//console.log(seatStatus);

	$('.hs_matrixTable input').removeClass('selected-cell-border');
	seat.addClass('selected-cell-border');
	//console.log(seatList.html());

	console.log(seatStatus);

	if (seatStatus == "Floorspace") {
		//alert('floor');
		seatList.attr('title', 'Floorspace');
		seatList.attr('seatstatus', 'Floorspace');
		seatListIcon.removeClass('flaticon-furniture-and-household');
		seatListRow.removeClass('ws-seathint__unavailable ws-seathint__booked');
		subMenu.hide();
		ParentMenu.removeClass('show');

		console.log('-a-');
		spaceDetailsApply(obj, seatStatus, 'row', 'Floorspace');
		console.log('-b-');
	}
	else if (seatStatus == "Available") {
		//alert('Available');
		seatList.attr('title', 'Available');
		seatList.attr('seatstatus', 'Available');
		seatListIcon.addClass('flaticon-furniture-and-household');
		seatList.addClass('ws-seathint__available');
		seatListRow.removeClass('ws-seathint__unavailable ws-seathint__booked');
		subMenu.hide();
		ParentMenu.removeClass('show');
	}
	else if (seatStatus == "Booked") {
		//alert('Booked')
		seatList.attr('title', 'Booked');
		seatList.attr('seatstatus', 'Booked');
		seatListIcon.addClass('flaticon-furniture-and-household');
		seatListRow.removeClass('ws-seathint__unavailable');
		seatListRow.addClass('ws-seathint__booked');
		subMenu.hide();
		ParentMenu.removeClass('show');
	}
	else if (seatStatus == "Unavailable") {
		//alert('Unavailable')
		seatList.attr('title', 'Unavailable');
		seatList.attr('seatstatus', 'Unavailable');
		seatListIcon.addClass('flaticon-furniture-and-household');
		seatListRow.removeClass('ws-seathint__booked');
		seatListRow.addClass('ws-seathint__unavailable');
		subMenu.hide();
		ParentMenu.removeClass('show');
	}

	seatDetails(obj, seatMatrix, "row");
}

//select column section
function wsSelectColumn(obj) {
	var seatDropdownLevel1 = $(obj).parents('.level2').siblings('a');
	var seat = $(obj).parents('.level1').siblings('input');
	var seatStatus = seatDropdownLevel1.attr('seatstatus');
	var seatCellIndex = $(obj).parents('td').index();
	var seatMatrix = $(obj).parents('.level1').siblings('input').attr('seatmatrix');
	//console.log(seatCellIndex);

	$(".hs_matrixTable tr").each(function (index) {
		var seatList = $(this).find('td').eq(seatCellIndex).find('input');

		//console.log(seatList.val());
		var seatListColumn = $(this).find('td').eq(seatCellIndex)
		var seatListIcon = $(this).find('td').eq(seatCellIndex).find('input.singleMatrixCell~i');
		var subMenu = $(obj).parents('.level2');
		var ParentMenu = $(obj).parents('.level1');

		$('.hs_matrixTable input').removeClass('selected-cell-border');
		seat.addClass('selected-cell-border');
		//console.log(seatList.html());

		if (seatStatus == "Floorspace") {
			//alert('floor');
			seatList.attr('title', 'Floorspace');
			seatList.attr('seatstatus', 'Floorspace');
			seatListIcon.removeClass('flaticon-furniture-and-household');
			seatListColumn.removeClass('ws-seathint__unavailable ws-seathint__booked');
			subMenu.hide();
			ParentMenu.removeClass('show');

			console.log('-a-');
			spaceDetailsApply(obj, 'column', seatStatus);
			console.log('-b-');
		}
		else if (seatStatus == "Available") {
			//alert('Available');
			seatList.attr('title', 'Available');
			seatList.attr('seatstatus', 'Available');
			seatListIcon.addClass('flaticon-furniture-and-household');
			seatList.addClass('ws-seathint__available');
			seatListColumn.removeClass('ws-seathint__unavailable ws-seathint__booked');
			subMenu.hide();
			ParentMenu.removeClass('show');
		}
		else if (seatStatus == "Booked") {
			//alert('Booked')
			seatList.attr('title', 'Booked');
			seatList.attr('seatstatus', 'Booked');
			seatListIcon.addClass('flaticon-furniture-and-household');
			seatListColumn.removeClass('ws-seathint__unavailable');
			seatListColumn.addClass('ws-seathint__booked');
			subMenu.hide();
			ParentMenu.removeClass('show');
		}
		else if (seatStatus == "Unavailable") {
			//alert('Unavailable')
			seatList.attr('title', 'Unavailable');
			seatList.attr('seatstatus', 'Unavailable');
			seatListIcon.addClass('flaticon-furniture-and-household');
			seatListColumn.removeClass('ws-seathint__booked');
			seatListColumn.addClass('ws-seathint__unavailable');
			subMenu.hide();
			ParentMenu.removeClass('show');
		}
	});
	seatDetails(obj, seatCellIndex, "column");
}

//select all complete space
function wsCompleteSpace(obj) {
	var seatDropdownLevel1 = $(obj).parents('.level2').siblings('a');
	var seat = $(obj).parents('.level1').siblings('input');
	var seatStatus = seatDropdownLevel1.attr('seatstatus');
	var seatList = $(obj).parents().find('.hs_matrixTable tr').find('input');
	var seatListRow = $(obj).parents().find('.hs_matrixTable tr').find('td');
	var seatListIcon = $(obj).parents().find('.hs_matrixTable tr').find('input.singleMatrixCell~i');
	var subMenu = $(obj).parents('.level2');
	var ParentMenu = $(obj).parents('.level1');
	var seatMatrix = $(obj).parents('.level1').siblings('input').attr('seatmatrix');
	//console.log(seatDropdownLevel1.html());

	$('.hs_matrixTable input').removeClass('selected-cell-border');
	seat.addClass('selected-cell-border');
	//console.log(seatList.html());

	if (seatStatus == "Floorspace") {
		//alert('floor');
		seatList.attr('title', 'Floorspace');
		seatList.attr('seatstatus', 'Floorspace');
		seatListIcon.removeClass('flaticon-furniture-and-household');
		seatListRow.removeClass('ws-seathint__unavailable ws-seathint__booked');
		subMenu.hide();
		ParentMenu.removeClass('show');

		console.log('-a-');
		spaceDetailsApply(obj, 'complete', seatStatus);
		console.log('-b-');
	}
	else if (seatStatus == "Available") {
		//alert('Available');
		seatList.attr('title', 'Available');
		seatList.attr('seatstatus', 'Available');
		seatListIcon.addClass('flaticon-furniture-and-household');
		seatList.addClass('ws-seathint__available');
		seatListRow.removeClass('ws-seathint__unavailable ws-seathint__booked');
		subMenu.hide();
		ParentMenu.removeClass('show');
	}
	else if (seatStatus == "Booked") {
		//alert('Booked')
		seatList.attr('title', 'Booked');
		seatList.attr('seatstatus', 'Booked');
		seatListIcon.addClass('flaticon-furniture-and-household');
		seatListRow.removeClass('ws-seathint__unavailable');
		seatListRow.addClass('ws-seathint__booked');
		subMenu.hide();
		ParentMenu.removeClass('show');
	}
	else if (seatStatus == "Unavailable") {
		//alert('Unavailable')
		seatList.attr('title', 'Unavailable');
		seatList.attr('seatstatus', 'Unavailable');
		seatListIcon.addClass('flaticon-furniture-and-household');
		seatListRow.removeClass('ws-seathint__booked');
		seatListRow.addClass('ws-seathint__unavailable');
		subMenu.hide();
		ParentMenu.removeClass('show');
	}
	seatDetails(obj, seatMatrix, "complete");
}

//double click
function wsSeatDetails(obj) {
	$('.hs_matrixTable input').removeClass('selected-cell-border');
	$('.dropdown-content').removeClass("show");
	$(obj).addClass('selected-cell-border');
	//alert('a');
	seatDetails(obj);
}

function seatDetails(obj, seatMatrix, type) {
	//console.log(type);
	//console.log(seatMatrix);
	//console.log(oldState);
	var selectedSeatDetails = $(obj).parents('td').find('input');
	var seatmatrixname = $(obj).attr('seatmatrix');
	var seatStatus = $(obj).attr('seatstatus');

	//console.log(selectedSeatDetails.html());
	var seat_Name = selectedSeatDetails.attr('usergivenname');
	var seat_Price = selectedSeatDetails.attr('price');
	var seat_Description = selectedSeatDetails.attr('description');
	var seat_status = selectedSeatDetails.attr('title');
	$('#ws-seatinput').css('display', 'block');
	$('#ws-seatinput').empty();

	$('#ws-seatinput').append(
		'<button type="button" class="close" onclick="seatDeatilsClose(this)"  aria-label="Close" >' +
		'<span aria-hidden="true">&times;</span>' +
		'</button>' +
		'<h5 class="card-title text-center ws-seatinput__title animated zoomIn">seat (<span id="selectSeat">' + seatmatrixname + '</span>) - <span class="seatStatus">' + seatStatus + '</span></h5>' +
		'<div class="row animated zoomIn">' +
		'<div class="offset-lg-2 col-lg-4 col-md-6 col-sm-6 col-12 ">' +
		'<div class="form-group">' +
		'<div class="form-group">' +
		'<input id="seat_name" name="seat_name" type="text" placeholder="Seat name" seatSelectedId="' + seatmatrixname + '" vlaue="" />' +
		'<label for="seat_name" class="control-label">Seat Name</label>' +
		'<i class="bar"></i>' +
		'</div>' +
		'</div>' +
		'</div>' +
		'<div class="col-lg-4 col-md-6 col-sm-6 col-12">' +
		'<div class="form-group">' +
		'<div class="form-group">' +
		'<input id="seat_price" name="seat_price" type="number" placeholder="123" value="' + seat_Price + '"/>' +
		'<label for="seat_price" class="control-label">Seat Price</label>' +
		'<i class="bar"></i>' +
		'</div>' +
		'</div>' +
		'</div>' +
		'</div>' +

		'<div class="row animated zoomIn">' +
		'<div class="offset-lg-2 col-lg-8 col-md-12 col-sm-12 col-12">' +
		'<div class="form-group">' +
		'<div class="form-group">' +
		'<input id="seat_description" name="seat_description" type="text" placeholder="Enter here..." value="' + seat_Description + '"/>' +
		'<label for="seat_description" class="control-label">Seat Description</label>' +
		'<i class="bar"></i>' +
		'</div>' +
		'</div>' +
		'</div>' +
		'</div>' +

		'<div class="row animated zoomIn">' +
		'<div class="offset-lg-2 col-lg-8 col-md-12 col-sm-12 col-12">' +
		'<div class="form-group">' +
		'<div class="form-group">' +
		'<input id="seat_status" name="seat_status" type="text" placeholder="Enter here..." value="' + seat_status + '"/>' +
		'<label for="seat_status" class="control-label">Status (display in tooltip)</label>' +
		'<i class="bar"></i>' +
		'</div>' +
		'</div>' +
		'</div>' +
		'</div>' +
		'<div class="row">' +
		'<div class="offset-lg-2 col-10 d-flex justify-content-between">' +
		'<div>' +
		'<a href="javascript:void(0);" onClick="spaceDetailsApply(this,\'' + seatMatrix + '\',\'' + type + '\')" class="btn btn-success text-uppercase">Add Seat(s)</a>' +
		'</div>' +
		'</div>' +
		'</div>'
	);

	if (type == "complete") {
		//alert('a')
		$('#seat_name').val("Complete");
		$('#selectSeat').html("Complete");
	}
	else if (type == "row") {
		$('#seat_name').val("Row");
		$('#selectSeat').html("Row");
	}
	else if (type == "column") {
		$('#seat_name').val("Column");
		$('#selectSeat').html("Column");
	}
	else {
		//alert('b')
		$('#seat_name').val(seatmatrixname);
	}

	if (seatStatus == "Seat Available" || seatStatus == "Seat Available Row" || seatStatus == "Seat Available Column" || seatStatus == "Seat Available Complete Space") {
		$('.seatStatus').css('color', '#33b433');
	}
	else if (seatStatus == "Seat FloorSpace" || seatStatus == "Seat FloorSpace Row" || seatStatus == "Seat FloorSpace Column" || seatStatus == "Seat FloorSpace Complete Space") {
		$('.seatStatus').css('color', '#2c2e3e');
	}
	else if (seatStatus == "Seat Unavailable" || seatStatus == "Seat Unavailable Row" || seatStatus == "Seat Unavailable Column" || seatStatus == "Seat Unavailable Complete Space") {
		$('.seatStatus').css('color', '#ff9800');
	}
	else if (seatStatus == "Seat Booked" || seatStatus == "Seat Booked Row" || seatStatus == "Seat Booked Column" || seatStatus == "Seat Booked Complete Space") {
		$('.seatStatus').css('color', '#f62459');
	}

	if (seat_Price == "") {
		$('#tableNoDrop').addClass('no-drop__div');
		$('#tableNoDrop div').show();
	}
}

function seatDeatilsClose(obj) {
	$('#tableNoDrop').removeClass('no-drop__div');
	$('#tableNoDrop div').hide();
	//console.log(seatCloseId);
	//$(obj).parent('.ws-seatinput').addClass('animated zoomOut');
	setTimeout(
		function () {
			$(obj).parent('.ws-seatinput').hide();
		},
		300);

	//old state function
	//if (oldState == "Floorspace") {
	//	var activeInputCell = $('.hs_matrixTable').find('tr:eq('+activeTr+')').find('td:eq('+activeInputTd+')').children('input');
	//	console.log(activeInputCell.attr('seatmatrix'));
	//	//var activeInputTd = $('.hs_matrixTable').find('tr:eq('+activeTr+')').children('td:eq('+activeInputTd+')');
	//	//console.log(activeInputTd.html());
	//	//var activeInputFont = $('.hs_matrixTable').find('tr').eq(activeTr).find('td').eq(activeInputTd).children('i');
	//	//console.log(activeInputFont.attr('class'));
	//	//activeInputCell.attr('title', 'Floorspace');
	//	//activeInputCell.attr('seatstatus', 'Floorspace');
	//	//activeInputFont.removeClass('flaticon-furniture-and-household');
	//	//activeInputTd.removeClass('ws-seathint__unavailable ws-seathint__booked');
	//}
}

function spaceDetailsApply(obj, seatMatrix, type, defParam = '') {
	//var NoRows = $('#ClientSpaceFloorPlan_NumberOfRows').val();
	console.log('apply' + defParam);
	var NoRows = 8;
	//var NoColumns = $('#ClientSpaceFloorPlan_NumberOfColumns').val();
	var NoColumns = 8;
	var urlAction = "";
	var arrSeatIDS = [];
	var seatList = [];
	var selectedSeat = "";
	//alert('a');
	var ws_seatId = $('#seat_name').attr('seatSelectedId');
	var ws_seatName = $('#seat_name').val();
	var ws_seatPrice = $('#seat_price').val();
	var ws_seatDescription = $('#seat_description').val();
	var ws_seatStatus = '';
	if (defParam == '')
		ws_seatStatus = $('#seat_status').val();
	else
		ws_seatStatus = defParam;

	var ws_seatmatrix = $('.hs_matrixTable input[seatmatrix = "' + ws_seatId + '"]');

	//console.log(ws_seatId + '--' + seatMatrix + "--" + ws_seatName + '--' + ws_seatDescription + '--' + '--' + ws_seatStatus + '--' + ws_seatmatrix);
	//getting selected row and loop the inputs
	var ws_rowSelectedInput = ws_seatmatrix.parents('tr');
	var ws_selectAll = ws_seatmatrix.parents('.hs_matrixTable')

	console.log(ws_seatId + 'apply' + type + ws_seatStatus);

	if (type == "row") {
		ws_rowSelectedInput.find('input').attr('price', ws_seatPrice);
		ws_rowSelectedInput.find('input').attr('description', ws_seatDescription);
		ws_rowSelectedInput.find('input').attr('title', ws_seatStatus);

		for (var column = 1; column <= NoColumns; column++) {
			var seat = {};
			var _seatID = "seat_" + ws_seatId.split(':')[0] + "_" + column;
			seat.SeatStatus = ws_seatStatus;
			seat.SeatPrice = parseFloat(ws_seatPrice);
			seat.SeatDescription = ws_seatDescription;
			seat.SeatXCoord = ws_seatId.split(':')[0];
			seat.SeatYCoord = column;
			seatList.push(seat);
		}

		//console.log(seatList);
	}
	else if (type == "column") {
		$(".hs_matrixTable tr").each(function (index) {
			$(this).find('td').eq(seatMatrix).find('input').attr('price', ws_seatPrice);
			$(this).find('td').eq(seatMatrix).find('input').attr('description', ws_seatDescription);
			$(this).find('td').eq(seatMatrix).find('input').attr('title', ws_seatStatus);
		})

		for (var row = 1; row <= NoRows; row++) {
			var seat = {};
			var _seatID = "seat_" + row + "_" + ws_seatId.split(':')[1];
			seat.SeatStatus = ws_seatStatus;
			seat.SeatPrice = parseFloat(ws_seatPrice);
			seat.SeatDescription = ws_seatDescription;
			seat.SeatXCoord = row;
			seat.SeatYCoord = ws_seatId.split(':')[1];
			seatList.push(seat);
		}
	}

	else if (type == "complete") {
		ws_selectAll.find('input').attr('price', ws_seatPrice);
		ws_selectAll.find('input').attr('description', ws_seatDescription);
		ws_selectAll.find('input').attr('title', ws_seatStatus);

		for (var row = 1; row <= NoRows; row++) {
			for (var column = 1; column <= NoColumns; column++) {
				var seat = {};
				var _seatID = "seat_" + row + "_" + column;
				seat.SeatStatus = ws_seatStatus;
				seat.SeatPrice = parseFloat(ws_seatPrice);
				seat.SeatDescription = ws_seatDescription;
				seat.SeatXCoord = row;
				seat.SeatYCoord = column;
				seatList.push(seat);
			}
		}
	}
	else {
		ws_seatmatrix.attr('userGivenName', ws_seatName);
		ws_seatmatrix.attr('price', ws_seatPrice);
		ws_seatmatrix.attr('title', ws_seatStatus);
		ws_seatmatrix.attr('description', ws_seatDescription);

		var seat = {};
		var _seatID = "seat_" + ws_seatId.split(':')[0] + "_" + ws_seatId.split(':')[1];
		seat.SeatStatus = ws_seatStatus;
		seat.SeatPrice = parseFloat(ws_seatPrice);
		seat.SeatDescription = ws_seatDescription;
		seat.SeatXCoord = parseInt(ws_seatId.split(':')[0]);
		seat.SeatYCoord = parseInt(ws_seatId.split(':')[1]);
		seatList.push(seat);
	}

	AddRemoveSeats(seatList, ws_seatStatus, ws_seatId);
	//alert(ws_seatId);
}

function AddRemoveSeats(seatList, ws_seatStatus, ws_seatId) {
	console.log(ws_seatStatus);
	if (ws_seatStatus == "Floorspace")
		urlAction = "/ClientSpace/RemoveSeats/";
	else
		urlAction = "/ClientSpace/AddSeats/";
	$.ajax({
		type: "POST",
		url: urlAction,
		data: { SeatList: seatList, SelectedSeatId: ws_seatId },
		dataType: "json",
		success: function (response) {
			console.log("--" + 5 + "--");

			$('#wsSeatNotification').append('<div class="animated zoomIn alert alert-success alert-dismissible ws-seatdetail" role="alert">' +
				'<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
				'<span aria-hidden="true">&times;</span>' +
				'</button>' +
				'<div>Seat Details added successfully</div>' +
				'</div>'
			);
			setTimeout(function () {
				$('#wsSeatNotification .alert').fadeOut();
			}, 2000);

			$('#tableNoDrop').removeClass('no-drop__div');
			$('#tableNoDrop div').hide();
		},
		error: function (xhr, ajaxOptions, thrownError) {
			console.log("--" + 6 + "--");
			//alert(xhr.responseText);
			$('#tableNoDrop').removeClass('no-drop__div');
			$('#tableNoDrop div').hide();
		}
	});
}

$(document).on('click', '.dropdown-submenu a.subLevel', function (e) {
	var clickIndex = $(this).parents('.level1 li').index();
	//console.log(clickIndex);
	//$(this).parents('.level1 li').not(':eq('+clickIndex+')').find('.level2').show();

	$(this).next('ul').toggle();

	e.stopPropagation();
	e.preventDefault();
});

// Close the dropdown if the user clicks outside of it
//window.onclick = function(event) {
//  if (!event.target.matches('.dropbtn')) {
//    var dropdowns = document.getElementsByClassName("dropdown-content");
//    var i;
//    for (i = 0; i < dropdowns.length; i++) {
//      var openDropdown = dropdowns[i];
//      if (openDropdown.classList.contains('show')) {
//        openDropdown.classList.remove('show');
//      }
//    }
//  }
//}
/*--------seat booking section admin end-----------*/

/*-----------------------------------------------------------------------------------*/
/*------------------------------seat booking section end-----------------------------*/
/*-----------------------------------------------------------------------------------*/
// 18. Nivo Lightbox

//function wsLightbox(obj) {
//$('.nivo-lightbox').nivoLightbox({
//		effect: 'slideUp',                             // The effect to use when showing the lightbox
//		theme: 'default',                             // The lightbox theme to use
//		keyboardNav: true,                             // Enable/Disable keyboard navigation (left/right/escape)
//		clickOverlayToClose: true,                    // If false clicking the "close" button will be the only way to close the lightbox
//		onInit: function(){},                         // Callback when lightbox has loaded
//		beforeShowLightbox: function(){},             // Callback before the lightbox is shown
//		afterShowLightbox: function(lightbox){},     // Callback after the lightbox is shown
//		beforeHideLightbox: function(){},             // Callback before the lightbox is hidden
//		afterHideLightbox: function(){},             // Callback after the lightbox is hidden
//		onPrev: function(element){},                 // Callback when the lightbox gallery goes to previous item
//		onNext: function(element){},                 // Callback when the lightbox gallery goes to next item
//		errorMessage: 'The requested content cannot be loaded. Please try again later.' // Error message when content can't be loaded
//	});
//}

//$('#horizontalTab1').easyResponsiveTabs({
//		type: 'default', //Types: default, vertical, accordion
//		width: 'auto', //auto or any width like 600px
//		fit: true, // 100% fit in a container
//		tabidentify: 'hor_1', // The tab groups identifier
//		activate: function(event) { // Callback function if tab is switched
//			var $tab = $(this);
//			var $info = $('#nested-tabInfo');
//			var $name = $('span', $info);
//			$name.text($tab.text());
//			$info.show();
//		}
//	});

//cart button action
$('.btn-cart__action').on('click', function () {
	//alert('a');
	//if ($(window).width() > 576) {
	var cart = $('.cart.ws-cart a');
	//} else {
	//}
	var imgtodrag = $(this).find("i").eq(0);
	if (imgtodrag) {
		var imgclone = imgtodrag.clone()
			.offset({
				top: imgtodrag.offset().top - 50,
				left: imgtodrag.offset().left - 20
			})
			.css({
				'opacity': '0.5',
				'position': 'absolute',
				'font-size': '150px',
				'z-index': '10000'
			})
			.appendTo($('body'))
			.animate({
				'top': cart.offset().top + 10,
				'left': cart.offset().left + 10,
				'font-size': 50,
			}, 1000, 'easeInOutExpo');

		setTimeout(function () {
			cart.effect("bounce", {
				times: 2
			}, 300);
		}, 1500);

		imgclone.animate({
			'font-size': 0,
		}, function () {
			$(this).detach()
		});
	}
});

function LoadNotification() {
	var html = "";
	urlAction = "/Home/GetNotifications/";
	$.ajax({
		type: "GET",
		url: urlAction,
		dataType: "json",
		success: function (response) {
			var url = "@Url.Action('Index','Website')";
			var cnt = 0;

			if (response) {
				$.each(response, function (index, item) {
					var msg = "GoNotificationAction('" + item['notificationName'] + "');"
					html += "<li><a href='javascript:void(0)'><div class='notification d-flex flex-row align-items-center'>";
					html += "<div class='notify-icon bg-img align-self-center'><div class='bg-type bg-type-md bg-danger'>";
					html += '<span onclick="' + msg + '">' + item['notificationDescription'].substring(0, 2).toUpperCase() + '</span></div></div><div class="notify-message">';
					html += '<p onclick="' + msg + '" class="font-weight-bold">';
					html += item['notificationDescription'];
					html += "</p>";
					html += '<small onclick="' + msg + '">Just now</small></div></div></a></li >';
					cnt++;
				});
			}
			$("#ulNotification").html(html);
			$("#adminNotification").html(cnt + "+");
		},
		error: function (xhr, ajaxOptions, thrownError) {
			console.log("--" + 6 + "--");
			alert(xhr.responseText);
		}
	});
}

function GoNotificationAction(notificationName) {
	urlAction = "/Home/GoNotificationAction/";
	$.ajax({
		type: "POST",
		url: urlAction,
		dataType: 'json',
		data: { NotificationAction: notificationName },
		success: function (data) {
			console.log("--" + 1 + "--");
			window.location.href = data;
		},
		error: function (xhr, ajaxOptions, thrownError) {
			console.log("--" + 2 + "--");
			alert(xhr.responseText);
		}
	});
}

/*********tab navigation section start*********/

//on focus input
$('.required-input').focus(function () {
	$(this).parents('.form-group').siblings('.hi-error').addClass('d-none');
});

//on blur input
$('.required-input').blur(function () {
	if ($(this).val() == "") {
		$(this).parents('.form-group').siblings('.hi-error').removeClass('d-none');
	}
});

function tabNavigation(nav) {
	var tabLength = parseInt($('.hi-tab').find('.nav-link').length);
	var currentTab = parseInt($('.hi-tab .active').attr('data-id'));

	//disable the back btn on first tab
	//if (currentTab == 2) {
	//	//alert('a')
	//	$('.tab-back-btn').css('display', 'none');
	//}
	//else {
	//	$('.tab-next-btn').css('display', 'inline-block');
	//	$('.tab-submit').css('display', 'none');
	//}

	//click back button
	if (nav == 0) {
		$("[data-id='" + (currentTab - 1) + "']").click();
	}
	//click next button
	else if (nav == 1) {
		checkRequiredInput(currentTab);

		//to show submit button on the last tab
		if ((currentTab) == (tabLength - 1)) {
			//alert('a')
			$('.tab-next-btn').css('display', 'none');
			$('.tab-submit').css('display', 'inline-block');
		}
	}
	//click submit button
	else if (nav == 2) {
		//event.preventDefault();
		if ($('input:visible').hasClass('required-input')) {
			//each condition to find the empty inputs
			$('.required-input:visible').each(function (i) {
				if ($(this).val() == "") {
					$(this).parents('.form-group').siblings('.hi-error').removeClass('d-none');
					event.preventDefault();
				}
			});
		}
	}
};

function checkRequiredInput(currentTab) {
	var dataId = $("[data-id='" + (currentTab + 1) + "']");
	//check required class in input
	if ($('input:visible').hasClass('required-input')) {
		//each condition to find the empty inputs
		$('.required-input:visible').each(function (i) {
			if ($(this).val() == "") {
				$(this).parents('.form-group').siblings('.hi-error').removeClass('d-none');
			}
			else {
				$('.tab-back-btn').css('display', 'inline-block');
				dataId.click();
				dataId.removeClass('pointer-event-none');
				dataId.parents('.nav-item').removeClass('cursor-no-drop');
			}
		});
	}
	else {
		$('.tab-back-btn').css('display', 'inline-block');
		dataId.click();
		dataId.removeClass('pointer-event-none');
		dataId.parents('.nav-item').removeClass('cursor-no-drop');
	}
};

$('.hi-tab .nav-link').on('click', function () {
	var tabLength = parseInt($('.hi-tab').find('.nav-link').length);
	var currentTab = parseInt($(this).attr('data-id'));
	if (currentTab == 1) {
		$('.tab-back-btn').css('display', 'none');
		$('.tab-next-btn').css('display', 'inline-block');
		$('.tab-submit').css('display', 'none');
	}
	else if (currentTab == tabLength) {
		$('.tab-back-btn').css('display', 'inline-block');
		$('.tab-next-btn').css('display', 'none');
		$('.tab-submit').css('display', 'inline-block');
	}
	else {
		$('.tab-back-btn').css('display', 'inline-block');
		$('.tab-next-btn').css('display', 'inline-block');
		$('.tab-submit').css('display', 'none');
	}
});

function loadPlanDetails(ClientMembershipPlanID, MembershipName, MembershipDuration, MembershipDurationType, Price, StartDate) {
	//alert('hi hello' + MembershipName + '-' + MembershipDuration + '-' + MembershipDurationType + '-' + Price + '-' + time);
	$('#popupClientMembershipPlanID').html(ClientMembershipPlanID);
	$('#popupMembershipName').html(MembershipName);
	$('#popupMembershipDurationType').html(MembershipDurationType);
	$('#popupMembershipDuration').html(MembershipDuration);
	$('#popupPrice').html(Price);
	$('#popupMembershipStartDate').html(StartDate);
	//$('#popupMembershipEndDate').html(EndDate);
}

function RenewalMembershipPlan() {
	var html = "";
	urlAction = "/MembershipPlan/UpdateMembershipPlan/";
	$.ajax({
		type: "POST",
		url: urlAction,
		data: { ClientMembershipPlanID: $('#popupClientMembershipPlanID').html() },
		dataType: "json",
		success: function (response) {
		},
		error: function (xhr, ajaxOptions, thrownError) {
			//console.log("--" + 6 + "--");
			//alert(xhr.responseText);
		}
	});
}
//$(window).on('load', function () {
//	ApplyPicker();
//});