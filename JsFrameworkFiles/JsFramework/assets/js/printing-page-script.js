
var labelprinting = labelprinting || {};

labelprinting.printingpage = function () {
	printersList = [];
	printerLabels = [];
	// current selected printer, assigned when a printer is selected.
	currentSelectedPrinter = null;
	currentSelectedPrinterLabel = null;

	$(document).ready(function () {

	
		labelprinting.app.init('');
		
		initPrintersListTab();


		labelprinting.app.getPrinters().then(function (returnedPrintersArray) {
			printerList = returnedPrintersArray;
			addPrintersToPage();
		});
	});

	function initPrintersListTab() {
		$("#printerLabelsList_tab").fadeOut();
		$("#printLabel_tab").fadeOut();
		$(".back").click(function () {
			$(".pages").fadeOut();
			$("#printerList_tab").fadeIn();
			$('#printerList_tab_left').addClass('animated slideInLeft');
			$('#printerList_tab_right').addClass('animated slideInRight');
			currentSelectedPrinter = null;
			currentSelectedPrinterLabel = null;
		});
	}

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
		$('#printersContainer').html(printersListHtml);

		currentSelectedPrinterLabel = null;
	}

	function onPrinterClick(printerId) {
		debugger;
		$("#printerList_tab").fadeOut();
		$("#printerLabelsList_tab").fadeIn();
		$('#printerLabelsList_tab_left').addClass('animated slideInLeft');
		$('#printerLabelsList_tab_right').addClass('animated slideInRight');

		var printer = printerList.filter(item => item.Id == printerId)[0];
		currentSelectedPrinter = printer;
		
		labelprinting.app.getPrinterLabels(printerId).then(function (returnedPrinterLabelsArray) {
			printerLabels = returnedPrinterLabelsArray;
			addPrinteLabelsToPage();

		});
	}

	function addPrinteLabelsToPage() {
		// printers list (same list the getPrinters() returns)
		var printerLabelsHtml = '';
		if (printerLabels && printerLabels.length > 0) {
			for (var i = 0; i < printerLabels.length; i++) {
				printerLabelsHtml += '<div id="btnPrintLabel_' + printerLabels[i].Id + '" onClick="labelprinting.printingpage.onPrinterLabelClick(\'' + printerLabels[i].Id + '\')" class="btn btn-printer printerButton">' + printerLabels[i].Name + '</div>     ';
			}
		} else {
			printerLabelsHtml = "<p>No printer labels found</p>"
		}
		$('#printerLabelsContainer').html(printerLabelsHtml);
	}

	function onPrinterLabelClick(printerLabelId) {
		debugger;
		var printerLabel = printerLabels.filter(item => item.Id == printerLabelId)[0];
		currentSelectedPrinterLabel = printerLabel;

		$("#printerLabelsList_tab").fadeOut();
		$("#printLabel_tab").fadeIn();
		$('#printLabel_tab_left').addClass('animated slideInLeft');
		$('#printLabel_tab_right').addClass('animated slideInRight');
	}

	function onPrintClick() {
		if (!currentSelectedPrinter) {
			alert("You have to select a printer")
			return;
		}

		if (!currentSelectedPrinterLabel) {
			alert("You have to select a printer label")
			return;
		}

		var materialInfo = $('#materialInfo').val();
		var textToPrint = $('#textToPrint').val();

		// get image as base64
		labelprinting.app.base64FileInputEncoder('imageToPrint').then(function (imageToPrint) {
			// update print label object
			currentSelectedPrinterLabel.MaterialInfo = materialInfo;
			currentSelectedPrinterLabel.TextToPrint = textToPrint;
			currentSelectedPrinterLabel.ImageToPrint = imageToPrint;

			labelprinting.app.printLabel(currentSelectedPrinterLabel).then(function (result) {
				if (result) {
					alert('Printed succesfully');
					window.location.href = window.location.href.split('#')[0];
					return;
				}
			});

		});
	}

	return {
		onPrinterClick : onPrinterClick,
		onPrinterLabelClick : onPrinterLabelClick,
		onPrintClick : onPrintClick
	}
}
();