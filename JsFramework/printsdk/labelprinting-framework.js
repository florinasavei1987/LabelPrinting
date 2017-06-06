var labelprinting = labelprinting || {};

labelprinting.model = labelprinting.model || {};

/** Configuration */
labelprinting.config = {
	printServiceEndpoint : 'http://localhost:50742/api/labelprinting/',
	getPrintersEndpoint : 'getprinters/',
	getPrinterLabelEndpoint : 'getprinterlabel'
};

/** Printer class */
labelprinting.model.Printer = function (){
	this.id = 'printerId';
	this.name = 'printerName';
};
labelprinting.model.Printer.prototype.toString = function () {
	return this.name;
}

/** PrintingLabel class */
labelprinting.model.PrintingLabel = function() {
	this.printerId = 'printerId';
	this.textToPrint = 'text to be print';
};

labelprinting.app = function () {
	var current = null;
	/** init label printing */
	function init() {

	}

	function getPrinters() {
		// Return a new promise.
		return new Promise(function (resolve, reject) {
			// Do the usual XHR stuff
			var req = new XMLHttpRequest();
			var url = labelprinting.config.printServiceEndpoint + labelprinting.config.getPrintersEndpoint,
			getPrintersEndpoint
			req.open('GET', url);

			req.onload = function () {
				// This is called even on 404 etc
				// so check the status
				if (req.status == 200) {
					// Resolve the promise with the response text
					resolve(req.response);
				} else {
					// Otherwise reject with the status text
					// which will hopefully be a meaningful error
					var jsonObj = JSON.parse(req.response);
					resolve(jsonObj);
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

	function getPrinterLabel() {
		// Return a new promise.
		return new Promise(function (resolve, reject) {
			// Do the usual XHR stuff
			var req = new XMLHttpRequest();
			var url = labelprinting.config.printServiceEndpoint + labelprinting.config,
			getPrintersEndpoint
			req.open('GET', url);

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

	function print(objToPrint) {
		// Return a new promise.
		return new Promise(function (resolve, reject) {
			// Do the usual XHR stuff
			var req = new XMLHttpRequest();
			var url = labelprinting.config.printServiceEndpoint + labelprinting.config,
			getPrintersEndpoint

			req.open("POST", url);
			req.setRequestHeader("Content-Type", "application/json");

			req.onload = function () {
				// This is called even on 404 etc
				// so check the status
				if (req.status == 200) {
					debugger;
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

	return {
		init : init,
		getPrinters : getPrinters,
		getPrinterLabel : getPrinterLabel,
		print : print
	}
}
();
