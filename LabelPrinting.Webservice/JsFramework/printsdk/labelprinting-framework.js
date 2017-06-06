var labelprinting = labelprinting || {};

labelprinting.model = labelprinting.model || {};

/** Configuration */
labelprinting.config = {
	printServiceEndpoint : 'http://localhost:50742/api/labelprinting/',
	getPrintersEndpoint : 'getprinters/',
	getPrinterLabelEndpoint : 'getprinterlabel',
	printEndpoint : 'print'
};

/** Printer class */
labelprinting.model.Printer = function () {
	this.id = 'printerId';
	this.name = 'printerName';
};
labelprinting.model.Printer.prototype.toString = function () {
	return this.name;
}

/** PrintingLabel class */
labelprinting.model.PrintingLabel = function () {
	this.printerId = 'printerId';
	this.textToPrint = 'text to be print';
};

labelprinting.app = function () {
	var current = null;
	/** init label printing */
	function init() {}

	function getPrinters() {
		// Return a new promise.
		return new Promise(function (resolve, reject) {
			// Do the usual XHR stuff
			var req = new XMLHttpRequest();

			var url = labelprinting.config.printServiceEndpoint + labelprinting.config.getPrintersEndpoint;
			req.open('GET', url);
			req.setRequestHeader("Content-Type", "application/json");
			req.onload = function () {
				// This is called even on 404 etc
				// so check the status
				if (req.status == 200) {
					// Resolve the promise with the response text
					var jsonObj = JSON.parse(req.response);
					resolve(jsonObj);
				} else {
					// Otherwise reject with the status text
					// which will hopefully be a meaningful error
					reject(Error(req.statusText));
				}
			};

			// Handle network errors
			req.onerror = function () {
				reject(Error("Network Error"));
			};

			// Make the request
			req.send();
		});
	}

	function getPrinterLabel(printerId) {
		// Return a new promise.
		return new Promise(function (resolve, reject) {
			// Do the usual XHR stuff
			var req = new XMLHttpRequest();
			var url = labelprinting.config.printServiceEndpoint + labelprinting.config.getPrinterLabelEndpoint + '/' + printerId;
			req.open('GET', url);

			req.onload = function () {
				debugger;
				// This is called even on 404 etc
				// so check the status
				if (req.status == 200) {
					// Resolve the promise with the response text
					var jsonObj = JSON.parse(req.response);
					resolve(jsonObj);
				} else {
					// Otherwise reject with the status text
					// which will hopefully be a meaningful error
					reject(Error(req.statusText));
				}
			};

			// Handle network errors
			req.onerror = function () {
				reject(Error("Network Error"));
			};

			// Make the request
			req.send();
		});
	}

	function printLabel(objToPrint) {

		// Return a new promise.
		return new Promise(function (resolve, reject) {
			// Do the usual XHR stuff
			var req = new XMLHttpRequest();
			var url = labelprinting.config.printServiceEndpoint + labelprinting.config.printEndpoint;

			req.open("POST", url, true);

			req.setRequestHeader("Content-Type", "application/json");

			req.onload = function () {
				debugger;
				// This is called even on 404 etc
				// so check the status
				if (req.status == 200) {
					resolve(true);
					// Resolve the promise with the response text
				} else {
					// Otherwise reject with the status text
					// which will hopefully be a meaningful error
					reject(Error(req.statusText));
				}
			};

			// Handle network errors
			req.onerror = function () {
				reject(Error("Network Error"));
			};

			// Make the request
			req.send(JSON.stringify(objToPrint));
		});
	}

	function print(printerId, textToPrint) {
		return new Promise(function (resolve, reject) {
			// get printer label
			getPrinterLabel(printerId).then(function (printerLabel) {
				printerLabel.TextToPrint = textToPrint;
				debugger;
				// print label
				printLabel(printerLabel).then(function (result) {
					debugger;

					if (result)
						resolve(true);
					else
						reject('Error on printing');
				});
			});
		});
	}

	return {
		init : init,
		getPrinters : getPrinters,
		print : print
	}
}
();