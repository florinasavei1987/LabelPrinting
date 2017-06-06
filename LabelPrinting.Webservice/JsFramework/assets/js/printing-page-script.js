
var labelprinting = labelprinting || {};

labelprinting.printingpage = function () {
	printersList = [];
	// current selected printer, assigned when a printer is selected.
	currentSelectedPrinter = null;

	$(document).ready(function () {

		initBasicControls();

		labelprinting.app.init();

		labelprinting.app.getPrinters().then(function (printers) {

			printerList = printers;

			addPrintersToPage();

		});
	});

	function addPrintersToPage() {
		// printers list (same list the getPrinters() returns)
		var printersListHtml = '';
		if (printerList && printerList.length > 0) {
			for (var i = 0; i < printerList.length; i++) {
				printersListHtml += '<div id="btnPrint_' + printerList[i].Id + '" onClick="labelprinting.printingpage.onPrinterClick(\'' + printerList[i].Id + '\')" class="btn btn-printer printerButton">' + printerList[i].Name + '</div>     ';
			}
		} else {
			printersListHtml = "<p>No printers found</p>"
		}
		$('#printersContainer').append(printersListHtml);
	}

	function initBasicControls() {
		$("#printer_pane_scroll").fadeOut();

		$(".back").click(function () {
			$(".pages").fadeOut();
			$("#index").fadeIn();
			$('#index_left').addClass('animated slideInLeft');
			$('#index_right').addClass('animated slideInRight');
			currentSelectedPrinter = null;
		});
	}

	function onPrinterClick(printerId) {
		debugger;
		$("#index").fadeOut();
		$("#printer_pane_scroll").fadeIn();
		$('#printer_pane_left').addClass('animated slideInLeft');
		$('#printer_pane_right').addClass('animated slideInRight');

		var printer = printerList.filter(item => item.Id = printerId)[0];
		currentSelectedPrinter = printer;
	}

	function onPrintClick() {
		if (!currentSelectedPrinter) {
			alert("You have to select a printer")
			return;
		} else {
			var textToPrint = $('#printText').val();

			if (!textToPrint || textToPrint.length <= 0) {
				alert("You have to type text to print");
				return;
			}

			labelprinting.app.print(currentSelectedPrinter.Id, textToPrint).then(function (result) {
				if (result) {
					alert('Printed succesfully');
				}
			});
		}

	}

	return {
		onPrinterClick : onPrinterClick,
		onPrintClick : onPrintClick
	}
}
();