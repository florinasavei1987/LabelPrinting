
var labelprinting = labelprinting || {};


$(document).ready(function () {
	debugger;

	labelprinting.app.init();

	labelprinting.app.getPrinters().then(function (printers) {
		// printers list (same list the getPrinters() returns)
		debugger;
	});
});

function initPageButtonHandlers() {
	$("#printer_pane_scroll").fadeOut();

	$(".printerButton").click(function () {
		$("#index").fadeOut();
		$("#printer_pane_scroll").fadeIn();
		$('#printer_pane_left').addClass('animated slideInLeft');
		$('#printer_pane_right').addClass('animated slideInRight');
	});

	$(".back").click(function () {
		$(".pages").fadeOut();
		$("#index").fadeIn();
		$('#index_left').addClass('animated slideInLeft');
		$('#index_right').addClass('animated slideInRight');
	});
}