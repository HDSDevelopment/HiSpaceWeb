/*--------seat booking section web start-----------*/

$(".admin_matrixpreview").hide();
$("#matrixpreviewRight").hide();
var arrSeatIDS = [];
var seatList = [];
var cartCount = 0;
var seatDetailsArray = [];

$(window).load(function () {
	$('.wsb-floors__space .space-tab:first-child').click();
	allPageCartAdd()
});

$(document).on('click', '.level2 li a', function () {
	$(this).parents('td').removeClass('open');
	$(this).parents('.level2').css('display', 'none');
});

function PreviewWebsiteSpaceMatrix(ClientSpaceFloorPlanID, NumberOfRows, NumberOfColumns, NumberOfSeats) {
	console.log(ClientSpaceFloorPlanID + '-' + NumberOfRows + '-' + NumberOfColumns + '-' + NumberOfSeats);
	//document.getElementById('btn2').oncontextmenu = function () {
	//    alert('right click!')
	//}

	//seatDetailsArray = [];
	//alert('a');
	allPageCartAdd()
	//$('#cart_count').css('display', 'none');
	//$('.ws-cart .dropdown-menu').css('display', 'none');
	//var MatrixBodyWidth = 100;
	//var MatrixBodyWidth = $(".wsb-floors__space-floor").width();
	//var NoRows = $('#ClientSpaceFloorPlan_NumberOfRows').val();
	var NoRows = NumberOfRows;
	//var NoColumns = $('#ClientSpaceFloorPlan_NumberOfColumns').val();
	var NoColumns = NumberOfColumns;
	//var seatSize = MatrixBodyWidth / NoColumns;
	var seatSize = 35;
	////console.log(NoRows, NoColumns, seatSize)
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
			html += "<td class='seat-not-allow dropdown'>";
			//            html += "<input onClick='seatVariables(this);wsAction(this,true);' onMouseover='seatVariables(this);' style='width:" + seatSize + "px;height:" + seatSize + "px;' title='' seatstatus='Floorspace' seatmatrix='" + i + ":" + j + "' id='seat_" + i + "_" + j + "' data-toggle='dropdown' type='button' class='matrixButton dropbtn dropdown-toggle singleMatrixCell' name='seat_" + i + "_" + j + "' userGivenName='name' description='test' price='123' /><i class='flaticon-furniture-and-household'></i></input>";
			html += "<input disabled onClick='seatVariables(this);wsAction(this,true);' onMouseover='seatVariables(this);' style='width:" + seatSize + "px;height:" + seatSize + "px;' title='Floorspace' seatstatus='Floorspace' seatmatrix='" + i + ":" + j + "' id='seat_" + i + "_" + j + "' data-toggle='dropdown' type='button' class='matrixButton dropbtn dropdown-toggle singleMatrixCell' name='seat_" + i + "_" + j + "' userGivenName='' description='' price='' aria-haspopup='true' aria-expanded='true' /><i class=''></i></input>";
			html += "<ul class='dropdown-menu level1'>" +

				//select section sub dropdown level
				"<li class='dropdown-submenu dropdown-content ws-dropdown'>" +
				"<a tabindex='-1' href='javascript:void(0);' seatstatus='Available' seatmatrix='" + i + ":" + j + "' class=' ws-seathint__selected subLevel'><span><input type='button' class='matrixButton'><i class='flaticon-furniture-and-household'></i></span><span class='ws-seathint__heading'>Select</span><i class='fa fa-caret-right' aria-hidden='true'></i></a>" +
				"<ul class='dropdown-menu level2'>" +
				//"<li class='dropdown-content ws-dropdown'><a onClick='wsAction(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Booked' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='flaticon-furniture-and-household'></i></span><span class='ws-seathint__heading'>Single</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsSelectRow(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Booked Row' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-arrows-h' aria-hidden='true'></i></span><span class='ws-seathint__heading'>Row</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsSelectColumn(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Booked Column' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-arrows-v' aria-hidden='true'></i></span><span class='ws-seathint__heading'>Column</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsCompleteSpace(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Booked Complete Space' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-th' aria-hidden='true'></i></span><span class='ws-seathint__heading'>All</span></a></li>" +
				"</ul>" +
				"</li>" +

				//unselect section sub dropdown level
				"<li class='dropdown-submenu dropdown-content ws-dropdown'>" +
				"<a tabindex='-1' href='javascript:void(0);' seatstatus='Unselect' seatmatrix='" + i + ":" + j + "'  class='ws-seathint__available subLevel' ><span><input type='button' class='matrixButton'><i class='flaticon-furniture-and-household'></i></span><span class='ws-seathint__heading'>Unselect</span><i class='fa fa-caret-right' aria-hidden='true'></i></a>" +
				"<ul class='dropdown-menu level2'>" +
				//"<li class='dropdown-content ws-dropdown'><a onClick='wsAction(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Available' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='flaticon-furniture-and-household'></i></span><span class='ws-seathint__heading'>Single</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsSelectRow(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Available Row' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-arrows-h' aria-hidden='true'></i></span><span class='ws-seathint__heading'>Row</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsSelectColumn(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Available Column' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-arrows-v' aria-hidden='true'></i></span><span class='ws-seathint__heading'>Column</span></a></li>" +
				"<li class='dropdown-content ws-dropdown'><a onClick='wsCompleteSpace(this)' tabindex='-1' href='javascript:void(0);' seatstatus='Seat Available Complete Space' seatmatrix='" + i + ":" + j + "' class='ws-seathint__row subLevel'><span><i class='fa fa-th' aria-hidden='true'></i></span><span class='ws-seathint__heading'>All</span></a></li>" +
				"</ul>" +
				"</li>" +

				"</ul>";
			html += "</td>";
		}
		html += "</tr>";
	}
	html += "</table>";

	$("#admin_matrixpreview_" + ClientSpaceFloorPlanID).html(html);
	$("#admin_matrixpreview_" + ClientSpaceFloorPlanID).show();

	wsClientSetupSeats(ClientSpaceFloorPlanID);

	setTimeout(function () {
		PreviewTempSeat();
	}, 1000);
}

function wsClientSetupSeats(ClientSpaceFloorPlanID) {
	//alert(ClientSpaceFloorPlanID)
	var html = "";
	urlAction = "/Website/ReadClientSeats/";
	$.ajax({
		type: "GET",
		url: urlAction,
		dataType: "json",
		data: { ClientSpaceFloorPlanID: ClientSpaceFloorPlanID },
		success: function (response) {
			//console.log(response)
			$.each(response, function (index, item) {
				var id = 'seat_' + item['seatXCoord'] + '_' + item['seatYCoord'];
				//console.log(id)
				var idCol = item['seatXCoord'] + ':' + item['seatYCoord'];
				//console.log(idCol)
				var seatId = '#admin_matrixpreview_' + ClientSpaceFloorPlanID + ' ' + '#' + id;
				$(seatId).attr('seatstatus', item['seatStatus']);
				$(seatId).attr('title', item['seatStatus']);
				$(seatId).attr('name', id);
				$(seatId).attr('price', item['seatPrice']);
				$(seatId).attr('seatmatrix', idCol);
				$(seatId).attr('description', item['seatDescription']);
				$(seatId).attr('clientSpaceSeatID', item['clientSpaceSeatID']);
				$(seatId).attr('memberBookingSpaceID', item['clientSpaceFloorPlanID']);

				if (item['seatStatus'] == "Available") {
					$(seatId).parents('td').addClass('ws-seathint__available');
					$(seatId).parents('td').removeClass('seat-not-allow');
					$(seatId).siblings('i').addClass('flaticon-furniture-and-household');
					$(seatId).attr('disabled', false);
				}
				else if (item['seatStatus'] == "Booked") {
					$(seatId).parents('td').addClass('ws-seathint__booked');
					$(seatId).siblings('i').addClass('flaticon-furniture-and-household');
					$(seatId).attr('disabled', true);
				}
				else if (item['seatStatus'] == "Unavailable") {
					$(seatId).parents('td').addClass('ws-seathint__unavailable');
					$(seatId).siblings('i').addClass('flaticon-furniture-and-household');
					$(seatId).attr('disabled', true);
				}
				else {
					$(seatId).parents('td').addClass('ws-seathint__floorspace');
					$(seatId).siblings('i').addClass('flaticon-furniture-and-household');
					$(seatId).attr('disabled', true);
				}
				wsCountTotal();
			});
		},
		error: function (xhr, ajaxOptions, thrownError) {
			//console.log("--" + 6 + "--");
			alert(xhr.responseText);
		}
	});
}

//seat variables declaration
var ws_seatId, ws_seatMatrix, ws_tr, ws_td, ws_tdIndex, ws_seatstatus, ws_name, ws_price, ws_description, ws_subMenu, ws_ParentMenu, ws_seatDetailsDiv;
var windowHeight = $(window).height();
var windowHeightSelect = windowHeight - 200;
//var ws_seatDropdownLevel1 = "";
//var ws_seatDropdownLevel1Status = "";

//seat variables
function seatVariables(obj) {
	ws_activeInput = $(obj);
	ws_seatId = $(obj).attr('id');
	ws_seatMatrix = $(obj).attr('seatmatrix');
	ws_seatstatus = $(obj).attr('seatstatus');
	ws_name = $(obj).attr('name');
	ws_price = $(obj).attr('price');
	ws_description = $(obj).attr('description');
	ws_tr = $(obj).parents('tr');
	ws_trIndex = $(obj).parents('tr').index() + 1;
	ws_tdIndex = $(obj).parents('td').index();
	ws_seatDetailsDiv = $(obj).parents('.wsb-floors__space-sec1').find('.ws-seatDetailsMaster').find('.ws-seat__details');
	////console.log(ws_seatDetailsDiv.html())
	//ws_seatDropdownLevel1 = $(obj).siblings('ul').find('.level2').siblings('a');
	//ws_seatDropdownLevel1Status = ws_seatDropdownLevel1.attr('seatstatus');
	////console.log(ws_seatDropdownLevel1Status);
	////console.log(ws_seatId);
	////console.log(ws_seatMatrix);
}

//close action
function seatClose(obj) {
	var seatCloseId = $(obj).parent('.ws-seatdetail').attr('seatId');
	console.log("closeid: " + seatCloseId);
	$(obj).parent('.ws-seatdetail').addClass('animated zoomOut');
	setTimeout(
		function () {
			$(obj).parent('.ws-seatdetail').remove();
			wsCountTotal();
		},
		300);
	$('#' + seatCloseId).removeClass('ws-seatselected');
	$('#' + seatCloseId).attr('title', 'Available');
	$('#' + seatCloseId).attr('seatstatus', 'Available');
	checkSelectedCount();

	//no seat count on close hint
	seatCount -= 1;
	$('.seatSelectedCount').html(seatCount);
	////console.log(seatCount);
}

var seatCount = 0;
var seatTotal = 0;

//count section
function wsCountTotal() {
	seatCount = 0;
	seatTotal = 0;
	seatCount = $('.ws-seat__details:visible').children('.ws-seatdetail_show').length;

	////console.log(seatCount);

	$('.ws-seatDetailsMaster:visible .seatSelectedCount').html(seatCount);
	////console.log(seatCount);
	$('.ws-seat__details:visible').children('.ws-seatdetail_show').each(function () {
		//alert('a');
		////console.log($(this).find('.wd_price').attr('wdprice'));
		////console.log($(this).html())
		seatTotal += parseFloat($(this).find('.wd_price').attr('wdprice'));
		////console.log(seatTotal);
	});

	////console.log(seatTotal);

	$('.sd_subtotal').html('' + seatTotal);
}

//single seat select
function wsAction(obj, isSingle = false) {
	var ws_seatDropdownLevel1 = '';
	var ws_seatDropdownLevel1Status = '';
	var seatList_clientSpaceSeatID = $(obj).attr('clientspaceseatid');
	var seatList_memberBookingSpaceID = $(obj).attr('memberBookingSpaceID');

	if (isSingle) {
		ws_seatDropdownLevel1Status = $(obj).attr('seatstatus');
		//alert(ws_seatDropdownLevel1Status);
		if (ws_seatDropdownLevel1Status == 'Selected') {
			ws_seatDropdownLevel1Status = 'Unselect';
			//$(obj).removeClass('ws-seatselected');
			//$(obj).attr('title', 'Available');
			//$(obj).attr('seatstatus', 'Available');
		}
	}
	else {
		ws_seatDropdownLevel1 = $(obj).parents('.level2').siblings('a');
		ws_seatDropdownLevel1Status = ws_seatDropdownLevel1.attr('seatstatus');
	}

	////console.log(ws_seatDropdownLevel1Status)

	//available seat action
	if ($(obj).attr('seatstatus') == "Available") {
		ws_activeInput.addClass('ws-seatselected');
		ws_activeInput.attr('title', 'Selected');
		ws_activeInput.attr('seatstatus', 'Selected');

		ws_seatDetailsDiv.prepend('<div memberBookingSpaceID="' + seatList_memberBookingSpaceID + '" seatmatrix="' + ws_seatMatrix + '" id="detail-' + ws_seatId + '" seatId=' + ws_seatId + ' class="alert alert-success alert-dismissible ws-seatdetail ws-seatdetail_show animated zoomIn" role="alert">' +
			'<button type="button" class="close" onclick="seatClose(this)"  aria-label="Close" >' +
			'<span aria-hidden="true">&times;</span>' +
			'</button>' +
			'<div class="row">' +
			'<div class="col-md-12">' +
			'<span class="ws-seatdetail__head">Name:</span>' +
			'<span class="wd_name">' + ws_name + '</span>' +
			'</div>' +
			'<div class="col-md-12 hide">' +
			'<span class="ws-seatdetail__head">ClientSpaceSeatID:</span>' +
			'<span class="wd_clientspaceseatid">' + seatList_clientSpaceSeatID + '</span>' +
			'</div>' +
			'<div class="col-md-12">' +
			'<span class="ws-seatdetail__head">Price:</span>' +
			'<span class="text-primary wd_price" wdprice=' + ws_price + '>' + ws_price + '</span>' +
			'</div>' +
			'<div class="col-md-12">' +
			'<span class="ws-seatdetail__head">Description: </span>' +
			'<span class="wd_desc">' + ws_description + '</span>' +
			'</div>' +
			'</div>' +
			'</div>').fadeIn(1000);

		////console.log(windowHeight);
		ws_seatDetailsDiv.css({ "max-height": windowHeightSelect + "px", "overflow": "auto" });

		//opacity and event action on seat details
		$('.ws-seatDetailsMaster').css({ 'opacity': '1', 'pointer-events': 'all' });
		$('#sampleSeatDetail').addClass('animated zoomOut');
		setTimeout(
			function () {
				$('#sampleSeatDetail').remove();
			},
			300);

		//seat count and total
		wsCountTotal();
	}

	//selected seat action
	else if ($(obj).attr('seatstatus') == "Selected") {
		ws_activeInput.removeClass('ws-seatselected');
		ws_activeInput.attr('title', 'Available');
		ws_activeInput.attr('seatstatus', 'Available');
		//remove details alert
		var seatCloseDetailId = ws_seatDetailsDiv.find('#detail-' + ws_seatId);
		seatCloseDetailId.addClass('animated zoomOut');
		setTimeout(
			function () {
				seatCloseDetailId.remove();
				//seat count and total
				wsCountTotal();
				//function to check the details to show or disabled
				checkSelectedCount();
			},
			300);
	}
}

//row select
function wsSelectRow(obj) {
	var ws_seatDropdownLevel1 = $(obj).parents('.level2').siblings('a');
	var ws_seatDropdownLevel1Status = ws_seatDropdownLevel1.attr('seatstatus');
	////console.log(ws_seatDropdownLevel1Status);

	if (ws_seatDropdownLevel1Status == "Available") {
		ws_tr.find('td').find('input[seatstatus="Available"]').each(function () {
			$(this).addClass('ws-seatselected');
			$(this).attr('title', 'Selected');
			$(this).attr('seatstatus', 'Selected');

			var seatList_id = $(this).attr('id');
			var seatList_seatMatrix = $(this).attr('seatmatrix');
			//var seatList_seatstatus = $(this).attr('seatstatus');
			var seatList_name = $(this).attr('name');
			var seatList_price = $(this).attr('price');
			var seatList_description = $(this).attr('description');
			var seatList_clientSpaceSeatID = $(this).attr('clientSpaceSeatID');
			var seatList_memberBookingSpaceID = $(this).attr('memberBookingSpaceID');

			//console.log(seatList_clientSpaceSeatID + '----' + seatList_memberBookingSpaceID);

			ws_seatDetailsDiv.prepend('<div memberBookingSpaceID="' + seatList_memberBookingSpaceID + '" seatmatrix="' + seatList_seatMatrix + '" id="detail-' + seatList_id + '" seatId=' + seatList_id + ' class="alert alert-success alert-dismissible ws-seatdetail ws-seatdetail_show animated zoomIn" role="alert">' +
				'<button type="button" class="close" onclick="seatClose(this)"  aria-label="Close" >' +
				'<span aria-hidden="true">&times;</span>' +
				'</button>' +
				'<div class="row">' +
				'<div class="col-md-12">' +
				'<span class="ws-seatdetail__head">Name:</span>' +
				'<span class="wd_name">' + seatList_name +
				'</div>' +
				'<div class="col-md-12 hide">' +
				'<span class="ws-seatdetail__head">ClientSpaceSeatID:</span>' +
				'<span class="wd_clientspaceseatid">' + seatList_clientSpaceSeatID + '</span>' +
				'</div>' +
				'<div class="col-md-12">' +
				'<span class="ws-seatdetail__head">Price:</span>' +
				'<span class="text-primary wd_price" wdprice="' + seatList_price + '">' + seatList_price + '</span>' +
				'</div>' +
				'<div class="col-md-12">' +
				'<span class="ws-seatdetail__head">Description: </span>' +
				'<span class="wd_desc">' + seatList_description + '</span>' +
				'</div>' +
				'</div>' +
				'</div>').fadeIn(1000);
			ws_seatDetailsDiv.css({ "max-height": windowHeightSelect + "px", "overflow": "auto" });

			//opacity and event action on seat details
			$('.ws-seatDetailsMaster').css({ 'opacity': '1', 'pointer-events': 'all' });
			$('#sampleSeatDetail').addClass('animated zoomOut');
			setTimeout(
				function () {
					$('#sampleSeatDetail').remove();
				},
				300);
		});
		//seat count and total
		wsCountTotal();
	}

	else if (ws_seatDropdownLevel1Status == "Unselect") {
		//remove details alert

		var arrSeatIDS = [];
		var seatList = [];
		var inputs = ws_tr.find('td').find('input[seatstatus="Selected"]');

		for (i = 0; i < inputs.length; i++) {
			//console.log(inputs[i]);
			var seat = {};
			seat.MemberBookingSpaceID = $(inputs[i]).attr('memberbookingspaceid');
			seat.ClientSpaceSeatID = $(inputs[i]).attr('clientspaceseatid');
			seatList.push(seat);
		}

		AddRemoveBookingsSeats(seatList, false);

		ws_tr.find('td').find('input[seatstatus="Selected"]').each(function () {
			$(this).removeClass('ws-seatselected');
			$(this).attr('title', 'Available');
			$(this).attr('seatstatus', 'Available');
			var seatList_id = $(this).attr('id');
			var seatCloseDetailId = ws_seatDetailsDiv.find('#detail-' + seatList_id);

			seatCloseDetailId.addClass('animated zoomOut');
			////console.log(seatCloseDetailId);
			setTimeout(
				function () {
					seatCloseDetailId.remove();
					//seat count and total
					wsCountTotal();
					checkSelectedCount();
				},
				300);
		});
		//function to check the details to show or disabled

		//console.log('-a-' + seatList);
		//removeCartItems('row', ws_seatDropdownLevel1Status);
		//console.log('-b-');
	}
}

//column select
function wsSelectColumn(obj) {
	var ws_seatDropdownLevel1 = $(obj).parents('.level2').siblings('a');
	var ws_seatDropdownLevel1Status = ws_seatDropdownLevel1.attr('seatstatus');
	if (ws_seatDropdownLevel1Status == "Available") {
		//alert('a');
		$(".hs_matrixTable tr").each(function (index) {
			var seatListStatus = $(this).find('td').eq(ws_tdIndex).find('input').attr('seatstatus');
			var seatList = $(this).find('td').eq(ws_tdIndex).find('input[seatstatus="Available"]');
			var seatList_id = seatList.attr('id');
			//alert(seatListStatus);
			var seatList_seatMatrix = seatList.find('input').attr('seatmatrix');
			//var seatList_seatstatus = seatList.find('input').attr('seatstatus');
			var seatList_name = seatList.attr('name');
			var seatList_price = seatList.attr('price');
			var seatList_description = seatList.attr('description');
			var seatList_clientSpaceSeatID = $(this).attr('clientSpaceSeatID');
			var seatList_memberBookingSpaceID = $(this).attr('memberBookingSpaceID');
			if (seatListStatus == "Available") {
				seatList.addClass('ws-seatselected');
				seatList.attr('title', 'Selected');
				seatList.attr('seatstatus', 'Selected');
				//$('#ws-seat__details').empty();
				ws_seatDetailsDiv.prepend('<div memberBookingSpaceID="' + seatList_memberBookingSpaceID + '" seatmatrix="' + seatList_seatMatrix + '" id="detail-' + seatList_id + '" seatId=' + seatList_id + ' class="alert alert-success alert-dismissible ws-seatdetail ws-seatdetail_show animated zoomIn" role="alert">' +
					'<button type="button" class="close" onclick="seatClose(this)"  aria-label="Close" >' +
					'<span aria-hidden="true">&times;</span>' +
					'</button>' +
					'<div class="row">' +
					'<div class="col-md-12">' +
					'<span class="ws-seatdetail__head">Name:</span>' +
					'<span class="wd_name">' + seatList_name + '</span>' +
					'</div>' +
					'<div class="col-md-12 hide">' +
					'<span class="ws-seatdetail__head">ClientSpaceSeatID:</span>' +
					'<span class="wd_clientspaceseatid">' + seatList_clientSpaceSeatID + '</span>' +
					'</div>' +
					'<div class="col-md-12">' +
					'<span class="ws-seatdetail__head">Price:</span>' +
					'<span class="text-primary wd_price" wdprice="' + seatList_price + '">' + seatList_price + '</span>' +
					'</div>' +
					'<div class="col-md-12">' +
					'<span class="ws-seatdetail__head">Description: </span>' +
					'<span class="wd_desc">' + seatList_description + '</span>' +
					'</div>' +
					'</div>' +
					'</div>').fadeIn(1000);
				ws_seatDetailsDiv.css({ "max-height": windowHeightSelect + "px", "overflow": "auto" });

				//opacity and event action on seat details
				$('.ws-seatDetailsMaster').css({ 'opacity': '1', 'pointer-events': 'all' });
				$('#sampleSeatDetail').addClass('animated zoomOut');
				setTimeout(
					function () {
						$('#sampleSeatDetail').remove();
					},
					300);
			}
		});
		//seat count and total
		wsCountTotal();
	}
	else if (ws_seatDropdownLevel1Status == "Unselect") {
		//remove details alert
		$(".hs_matrixTable tr").each(function (index) {
			var seatListStatus = $(this).find('td').eq(ws_tdIndex).find('input').attr('seatstatus');
			var seatList = $(this).find('td').eq(ws_tdIndex).find('input[seatstatus="Selected"]');
			var seatList_id = seatList.attr('id');
			var seatCloseDetailId = ws_seatDetailsDiv.find('#detail-' + seatList_id);
			////console.log(seatList_id)

			if (seatListStatus == "Selected") {
				seatList.removeClass('ws-seatselected');
				seatList.attr('title', 'Available');
				seatList.attr('seatstatus', 'Available');
				seatCloseDetailId.addClass('animated zoomOut');
				////console.log(seatCloseDetailId);
				setTimeout(
					function () {
						seatCloseDetailId.remove();
						//seat count and total
						wsCountTotal();
					},
					300);
			}
		});
		//function to check the details to show or disabled
		checkSelectedCount();
	}
}

//complete select
function wsCompleteSpace(obj) {
	var ws_seatDropdownLevel1 = $(obj).parents('.level2').siblings('a');
	var ws_seatDropdownLevel1Status = ws_seatDropdownLevel1.attr('seatstatus');
	var seatList = $(obj).parents().find('.hs_matrixTable tr');
	if (ws_seatDropdownLevel1Status == "Available") {
		//alert('a');
		//$('#ws-seat__details').empty();
		seatList.find('input[seatstatus="Available"]').each(function () {
			$(this).addClass('ws-seatselected');
			$(this).attr('title', 'Selected');
			$(this).attr('seatstatus', 'Selected');

			var seatList_id = $(this).attr('id');
			var seatList_seatMatrix = $(this).attr('seatmatrix');
			//var seatList_seatstatus = $(this).find('input').attr('seatstatus');
			var seatList_name = $(this).attr('name');
			var seatList_price = $(this).attr('price');
			var seatList_description = $(this).attr('description');
			var seatList_clientSpaceSeatID = $(this).attr('clientSpaceSeatID');
			var seatList_memberBookingSpaceID = $(this).attr('memberBookingSpaceID');

			ws_seatDetailsDiv.prepend('<div memberBookingSpaceID="' + seatList_memberBookingSpaceID + '" seatmatrix="' + seatList_seatMatrix + '" id="detail-' + seatList_id + '" seatId=' + seatList_id + ' class="alert alert-success alert-dismissible ws-seatdetail ws-seatdetail_show animated zoomIn" role="alert">' +
				'<button type="button" class="close" onclick="seatClose(this)"  aria-label="Close" >' +
				'<span aria-hidden="true">&times;</span>' +
				'</button>' +
				'<div class="row">' +
				'<div class="col-md-12">' +
				'<span class="ws-seatdetail__head">Name:</span>' +
				'<span class="wd_name">' + seatList_name + '</span>' +
				'</div>' +
				'<div class="col-md-12 hide">' +
				'<span class="ws-seatdetail__head">ClientSpaceSeatID:</span>' +
				'<span class="wd_clientspaceseatid">' + seatList_clientSpaceSeatID + '</span>' +
				'</div>' +
				'<div class="col-md-12">' +
				'<span class="ws-seatdetail__head">Price:</span>' +
				'<span class="text-primary wd_price" wdprice="' + seatList_price + '">' + seatList_price + '</span>' +
				'</div>' +
				'<div class="col-md-12">' +
				'<span class="ws-seatdetail__head">Description: </span>' +
				'<span class="wd_desc">' + seatList_description + '</span>' +
				'</div>' +
				'</div>' +
				'</div>').fadeIn(1000);
			ws_seatDetailsDiv.css({ "max-height": windowHeightSelect + "px", "overflow": "auto" });

			//opacity and event action on seat details
			$('.ws-seatDetailsMaster').css({ 'opacity': '1', 'pointer-events': 'all' });
			$('#sampleSeatDetail').addClass('animated zoomOut');
			setTimeout(
				function () {
					$('#sampleSeatDetail').remove();
				},
				300);
		});
		//seat count and total
		wsCountTotal();
	}
	else if (ws_seatDropdownLevel1Status == "Unselect") {
		//remove details alert
		seatList.find('input[seatstatus="Selected"]').each(function () {
			$(this).removeClass('ws-seatselected');
			$(this).attr('title', 'Available');
			$(this).attr('seatstatus', 'Available');
			var seatList_id = $(this).attr('id');
			var seatCloseDetailId = ws_seatDetailsDiv.find('#detail-' + seatList_id);

			seatCloseDetailId.addClass('animated zoomOut');
			////console.log(seatCloseDetailId);
			setTimeout(
				function () {
					seatCloseDetailId.remove();
					//seat count and total
					wsCountTotal();
				},
				300);
		})
		//function to check the details to show or disabled
		checkSelectedCount();
	}
}

//check the detial to show or disabled
function checkSelectedCount() {
	var availableCount = 0;

	$.each($('.hs_matrixTable input[type="button"]'), function () {
		//console.log(availableCount)
		if ($(this).attr('seatstatus') == 'Selected') {
			availableCount++;
			//console.log(availableCount)
		}
		else {
		}
	});
	////console.log(availableCount);
	if (availableCount >= 1) {
		$('.ws-seatDetailsMaster').css({ 'opacity': '1', 'pointer-events': 'all' });
	}
	else {
		$('.ws-seatDetailsMaster').css({ 'opacity': '0.4', 'pointer-events': 'none' });
		$(ws_seatDetailsDiv).empty();
		ws_seatDetailsDiv.append('<div seatmatrix="" id="sampleSeatDetail" class="animated zoomIn alert alert-success alert-dismissible ws-seatdetail" role="alert">' +
			'<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
			'<span aria-hidden="true">&times;</span>' +
			'</button>' +
			'<div class="row">' +
			'<div class="col-md-12">' +
			'<span class="ws-seatdetail__head">Name:</span>' +
			'<span class="wd_name">Seat Name</span>' +
			'</div>' +
			'<div class="col-md-12">' +
			'<span class="ws-seatdetail__head">Price:</span>' +
			'<span class="text-primary wd_price" wdprice="0">00</span>' +
			'</div>' +
			'<div class="col-md-12">' +
			'<span class="ws-seatdetail__head wd_desc">Description:</span>' +
			'<span>Description' +
			'</span>' +
			'</div>' +
			'</div>' +
			'</div>'
		);
	}
}
/*--------seat booking section web end-----------*/
//cart button action
$('.btn-cart__action').on('click', function () {
	//alert('click event');
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

	var cartBtn = $(this);
	//cartCount += parseInt(seatCount);
	cartCount = parseInt(seatCount);
	////console.log(seatCount);

	$('.ws-cart .dropdown-menu').attr('style', '');
	$('#cart_navBarTotal').html('' + seatTotal);
	$('#cart_subTotal').html('' + seatTotal);
	//$('#cart_total').html('' + seatTotal);

	//seatDetailsArray = [];

	//console.log('Previous : ' + seatDetailsArray)

	SetSeatList();

	setTimeout(function () {
		//alert(cartCount);
		//console.log(seatList)
		//$('#cart_count').html(seatDetailsArray.length);
		$('#cart_count').css('display', 'unset');
		$('#headingOne button').click();
	}, 1000);

	////store the data in temp
	//function tempSeatBooking(seatList){
	//	console.log(seatList);
	//};

	AddRemoveBookingsSeats(seatList, true);

	var cartSubTotal = parseFloat($('.ws-seatList:visible').find('.sd_subtotal').html());
	////console.log(cartSubTotal)
	//var cartTotal = parseFloat($("#cart_total").html());
	////console.log(cartTotal)
	//cartTotal += cartSubTotal;

	//$('#cart_total').html(cartTotal);
	//$('#cart_navBarTotal').html(cartTotal);
});

function SetSeatList() {
	//alert('a')
	seatList = [];
	singleSeat = {};
	var temp_memberbookingspaceid = $('.btn-cart__action:visible').parents().siblings(".ws-seat__details").find(".ws-seatdetail_show").attr('memberbookingspaceid');
	//console.log(temp_memberbookingspaceid)
	var temp_ClientSpaceSeatID = $('.btn-cart__action:visible').parents().siblings(".ws-seat__details").find(".ws-seatdetail_show").find('.wd_clientspaceseatid').html();
	//console.log(temp_ClientSpaceSeatID)

	if (temp_ClientSpaceSeatID == '0') {
		$('.btn-cart__action:visible').parents().siblings(".ws-seat__details").find(".ws-seatdetail_show").each(function (index) {
			singleSeat = {};
			singleSeat.sd_id = $(this).attr('id');
			singleSeat.SeatMatrix = $(this).attr('seatMatrix');
			singleSeat.SeatId = $(this).attr('seatid');
			singleSeat.SeatName = $(this).find('.wd_name').html();

			singleSeat.SeatPrice = $(this).find('.wd_price').attr('wdprice');
			singleSeat.SeatDesc = $(this).find('.wd_desc').html();

			singleSeat.MemberBookingSpaceID = temp_memberbookingspaceid;
			singleSeat.ClientSpaceSeatID = $(this).find('.wd_clientspaceseatid').html();
			singleSeat.SeatStatus = 'Requested';

			var rs = $.grep(seatDetailsArray, function (item, index) {
				return (item.sd_id == singleSeat.sd_id && item.MemberBookingSpaceID == singleSeat.MemberBookingSpaceID);
			});
			console.log(rs.length);

			if (rs.length == 0) {
				seatDetailsArray.push(singleSeat);
				console.log(seatDetailsArray);
			}

			seatList.push(singleSeat);
		});
	}
	else {
		$('.btn-cart__action:visible').parents().siblings(".ws-seat__details").find(".ws-seatdetail_show").each(function (index) {
			singleSeat = {};
			//var singleSeatTemp = {};
			////console.log($(this).html());
			singleSeat.sd_id = $(this).attr('id');
			singleSeat.SeatMatrix = $(this).attr('seatMatrix');
			singleSeat.SeatId = $(this).attr('seatid');
			singleSeat.SeatName = $(this).find('.wd_name').html();

			singleSeat.SeatPrice = $(this).find('.wd_price').attr('wdprice');
			singleSeat.SeatDesc = $(this).find('.wd_desc').html();

			singleSeat.MemberBookingSpaceID = temp_memberbookingspaceid;
			singleSeat.ClientSpaceSeatID = $(this).find('.wd_clientspaceseatid').html();
			//console.log(singleSeat.ClientSpaceSeatID);
			//alert($(this).find('.wd_clientspaceseatid').html());
			singleSeat.SeatStatus = 'Requested';

			var rs = $.grep(seatDetailsArray, function (item, index) {
				return (item.sd_id == singleSeat.sd_id && item.MemberBookingSpaceID == singleSeat.MemberBookingSpaceID);
			});

			//console.log(rs.length);

			if (rs.length == 0) {
				seatDetailsArray.push(singleSeat);
				//console.log(seatDetailsArray);
			}

			seatList.push(singleSeat);
		});
	}

	console.log(seatList);
}

function PreviewTempSeat() {
	//alert('b')
	SetSeatList();

	$.each(seatList, function (index, item) {
		//console.log(item.SeatId);
		//console.log($('#' + item.SeatId).attr('clientspaceseatid'))

		//console.log(item.ClientSpaceSeatID);
		//console.log(item.MemberBookingSpaceID);
		//console.log($('#admin_matrixpreview_' + item.MemberBookingSpaceID + ' ' + '#' + item.SeatId).attr('clientspaceseatid'));

		if ((item.SeatId == $('#admin_matrixpreview_' + item.MemberBookingSpaceID + ' ' + '#' + item.SeatId).attr('id')) && (item.ClientSpaceSeatID == $('#admin_matrixpreview_' + item.MemberBookingSpaceID + ' ' + '#' + item.SeatId).attr('clientspaceseatid'))) {
			//alert('test')
			//console.log($('#' + item.SeatId).attr('title'));
			$('#admin_matrixpreview_' + item.MemberBookingSpaceID + ' ' + '#' + item.SeatId).addClass('ws-seatselected');
			$('#admin_matrixpreview_' + item.MemberBookingSpaceID + ' ' + '#' + item.SeatId).attr('title', 'Selected');
			$('#admin_matrixpreview_' + item.MemberBookingSpaceID + ' ' + '#' + item.SeatId).attr('seatstatus', 'Selected');
		} else {
		}
	});
}

function AddRemoveBookingsSeats(seatList, IsAdd) {
	console.log(seatList)
	if (IsAdd)
		urlAction = "/Website/AddSeats/";
	else
		urlAction = "/Website/RemoveSeats/";

	$.ajax({
		type: "POST",
		url: urlAction,
		data: { SeatList: seatList },
		dataType: "json",
		success: function (response) {
			//console.log("--" + 5 + "--");

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
			allPageCartAdd();
		},
		error: function (xhr, ajaxOptions, thrownError) {
			//console.log("--" + 6 + "--");
			//alert(xhr.responseText);
			$('#tableNoDrop').removeClass('no-drop__div');
			$('#tableNoDrop div').hide();
		}
	});
}

//Remove the row
function removeList(obj) {
	$(obj).parents("li.d-flex").remove();
}

function allPageCartAdd() {
	//alert('allPageCartAdd');

	//if (localStorage.getItem('seatDetailsArray')) {
	//    var retrievedSeatDetailsArray = localStorage.getItem('seatDetailsArray');
	//    var retrivedCartCount = localStorage.getItem('cartCount');
	//    var retrivedSeatTotal = localStorage.getItem('seatTotal');
	//}
	//else {
	//    var retrievedSeatDetailsArray = "[]";
	//    var retrivedCartCount = 0;
	//    var retrivedSeatTotal = 0;
	//}

	var html = "";
	urlAction = "/Website/GetSelectedSeats/";
	$.ajax({
		type: "GET",
		url: urlAction,
		dataType: "json",
		success: function (response) {
			retrievedSeatDetailsArray = response;
			$('#cart_count').html(retrievedSeatDetailsArray.length);
			//console.log(response);
			//console.log(retrievedSeatDetailsArray);
			//retrievedSeatDetailsArray = JSON.parse(retrievedSeatDetailsArray);

			//var retrivedCartCount = retrievedSeatDetailsArray.length;

			//////console.log(retrievedSeatDetailsArray);
			//////console.log(retrivedCartCount);
			//////console.log(retrivedSeatTotal);

			////$('.wsc-table tbody').empty();
			//$('#cart_count').html(retrivedCartCount);
			//$('#wsc_total').html(retrivedSeatTotal);
			//$('#cart_navBarTotal').html(retrivedSeatTotal);
			var seatTotalPrice = 0;
			$('#cart_navBarDetails').empty();
			$('.cartSeatDetails tbody').empty();
			$('#cart_navBarDetails').empty();

			//console.log(retrievedSeatDetailsArray)
			$.each(retrievedSeatDetailsArray, function (index, item) {
				//append the details in cart navbar
				//if (item.SeatName != undefined) {
				$('#cart_navBarDetails').append(
					'<li class="d-flex align-items-center" clientSpaceSeatID="' + item.clientSpaceSeatID + '" memberBookingSpaceID="' + item.memberBookingSpaceID + '" seatMatrix="' + item.seatMatrix + '" seatid="' + item.seatId + '">' +
					'<div class="ws-cart__image">' +
					'<i class="flaticon-chair"></i>' +
					'</div>' +
					'<div class="ws-cart__quantity">' +
					'<span class="d-block">' + item.seatName + '</span>' +
					'<span class="quantity d-block">1 x ' + item.seatPrice + '</span>' +
					'</div>' +
					'<div class="ws-cart__price">' + item.seatPrice + '</div>' +
					//'<div class="ws-cart__price text-danger" onclick="removeList(this)">'+
					//	'<i class="fas fa-trash-alt"></i>'+
					//'</div>'+
					'</li>'
				);
				//}

				//append the details in cart div
				$('.cartSeatDetails tbody').append(
					'<tr seatMatrix="' + item.seatMatrix + '" seatid="' + item.seatId + '">' +
					'<td>' + item.seatName + '</td>' +
					'<td class="text-center">1</td>' +
					'<td class="text-right">' + item.seatPrice + '</td>' +
					//'<td class="removeRow text-center" onclick="removeRow(this)"><i class="fas fa-trash-alt"></i></td>'+
					'</tr>'
				);

				$('.wsc-table tbody').append(
					'<tr>' +
					'<td class="wsc-table__client">' +
					'<i class="flaticon-chair"></i>' +
					'<h3 class="d-inline-block">' + item.seatName + '</h3>' +
					'</td>' +
					'<td class="wsc-table__deta">' +
					'<p>' + item.seatDesc + '</p>' +
					'</td>' +
					'<td class="wsc-table__quantity">1</td>' +
					'<td class="wsc-table__price">' + item.seatPrice + '</td>' +
					'<td class="wsc-table__clientSpaceSeatID hidden">' + item.clientSpaceSeatID + '</td>' +
					//'<td class="removeRow" onclick="removeRow(this)"><i class="fas fa-trash-alt"></i></td>'+
					'</tr>'
				);

				seatTotalPrice += parseFloat(item.seatPrice);
			});

			$('#wsc_total').html(seatTotalPrice);
			$('#cart_navBarTotal').html(seatTotalPrice);
			$('#cart_total').html(seatTotalPrice);
			//$('#cart_navBarTotal').html(cartTotal);
		},
		error: function (xhr, ajaxOptions, thrownError) {
			//console.log("--" + 6 + "--");
			alert(xhr.responseText);
		}
	});
}

$(document).on('click', '.checkout', function () {
	//alert('a');
	// Retrieve the object from storage
});