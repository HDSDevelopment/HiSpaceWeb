/* like-icon (index-1) **/
if ($('.ws-like-icon').length) {
	$('.ws-like-icon').on('click', function (e) {
		e.preventDefault();
		$(this).toggleClass('liked');
		$(this).children('.ws-like-icon').toggleClass('liked');
	});
}
//testimonial call
$('.testimonial_slider').carousel();

//workspace details carousel
$('.wsd-carousel').owlCarousel({
	loop: true,
	margin: 0,
	nav: true,
	items: 1,
	autoplay: true,
	autoplayHoverPause: true,
	animateOut: 'zoomOut',
	animateIn: 'slideInUp',
	dots: true
});
//google map section
function displayMap(lat, lon) {
	document.getElementById("ws-map").src = 'https://www.google.com/maps/embed/v1/place?q=' + lat + ',' + lon + '&key=AIzaSyB59thq3ND2fzB_9zq5IZa3Ko2antbwRGw';
}
var latitude = "12.973350";
var longitude = "80.250280";
displayMap(latitude, longitude);

//Remove the row
function removeRow(obj) {
	//alert('a')
	//$(obj).html();
	//console.log($(obj).parent("tr").html());
	$(obj).parent("tr").remove();
}

//website index amenities section more action
function allAmenities(obj, ClientSpaceFloorPlanID) {
	urlAction = "/Website/GetFacilitiesBySpace/";
	$.ajax({
		type: "GET",
		url: urlAction,
		dataType: 'json',
		data: { ClientSpaceFloorPlanID: ClientSpaceFloorPlanID },
		success: function (response) {
			console.log("--1--");
			var availableFacilities = '';
			$.each(response, function (index, item) {
				console.log(item['facilityName']);
				availableFacilities += "<li><i class='far fa-check-circle'></i> " + item['facilityName'] + "</li>";
			});
			$('.amenities-more__details').hide();
			$(obj).parent().find('.alert').remove();
			$(obj).parent().append(
				'<div class="amenities-more__details animated zoomIn alert alert-success alert-dismissible ws-seatdetail" role="alert">' +
				'<button type="button" class="close" onclick="seatClose(this)" aria-label="Close">' +
				'<span aria-hidden="true">&times;</span>' +
				'</button>' +
				'<div class="row">' +
				'<div class="col-md-12 text-left">' +
				'<ul class="ul-list-vertical">' +
				availableFacilities +
				'</ul>' +
				'</div>' +
				'</div>' +
				'</div>'
			);
		},
		error: function (xhr, ajaxOptions, thrownError) {
			console.log("--" + 2 + "--");
			alert(xhr.responseText);
		}
	});
}

/*********tab navigation section end*********/

$('.ws-image-lightbox-1, .ws-image-lightbox-2, .ws-image-lightbox-3').cubeportfolio({
	layoutMode: 'mosaic',
	animationType: 'quicksand',
	gapHorizontal: 0,
	gapVertical: 0,
	gridAdjustment: 'responsive',
	mediaQueries: [{
		width: 1500,
		cols: 5
	}, {
		width: 1100,
		cols: 4
	}, {
		width: 800,
		cols: 3
	}, {
		width: 480,
		cols: 2
	}, {
		width: 320,
		cols: 1
	}],
	caption: 'zoom',
	displayTypeSpeed: 100,
});

//media query
//$(window).resize(function () {
//	alert('a')
//});

//$(window).load(function () {
//	//tooltip section
//	var winWidth = $(window).width();
//	if (winWidth > 991) {
//	}
//});