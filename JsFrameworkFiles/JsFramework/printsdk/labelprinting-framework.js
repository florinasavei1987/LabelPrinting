/**
How to use this library

Download label.printing framework library.
Link labelprinting-framework.js into your webapplication. Eg: <script src="assets/js/printing-page-script.js"></script>
Start using it.

Namespaces
To be able to use this library you have to use labelprinting namespace.
The practice of namespacing is usually to create an object literal encapsulating your own functions and variables, so as not to collide with those created by other libraries.
Promises
All methods of this library, are asynchronous. So when are called promises have to be used. You can find more about promises in here:https://developers.google.com/web/fundamentals/getting-started/primers/promises 

Label.Printing library functions:

1. labelprinting.app.init()
	Before start using other library functions, you have to call this method.
	Printing webservice endpoints are set in here.
	You can overwrite default webservice url by passing paremeter in here.Eg: labelprinting.app.init('http://webserviceurl');

2. labelprinting.app.getPrinters()
	Api function used to retrieve all printers.
	It returns an array of printers. Returned array looks like this:
	[
		{
		    Id : 'PrinterId',
			Name: 'PrinterName'
		},
		.....
	]
	How to call this function using promises:
	labelprinting.app.getPrinters().then(function (returnedPrintersArray) {
			// add your own logic in here, based on returnedPrintersArray
		});
		
2. labelprinting.app.getPrinterLabels(printerId)
	Api function used to retrieve all labels for a specified printer id.
	It returns an array of printerlabels. Returned array looks like this:
	[
		{
		    Id : 'PrinterId',
			Name: 'PrinterName',
			FontSize: 'FontSize',
			LabelFileName: 'LabelFileName',
			TextToPrint: 'TextToPrint',
			MaterialInfo: 'MaterialInfo',
			ImageToPrint: 'ImageToPrint'
		}
		.....
	]
	How to call this function using promises:
	labelprinting.app.printerlabels('testprinterid').then(function (returnedPrinterLabelsArray) {
			// add your own logic in here, based on returnedPrinterLabelsArray
		});
	
3. labelprinting.app.printLabel(printLabelObject)
	Api function used to print a label. It receive as parameter an object like this. 
	You have to update this object with proper values to print. You can't edit Id, and Name of a label, this changes will be ignored. 
	All other object properties are editable, and printing is based on this ones.
	
		{
		    Id : 'PrinterId',
			Name: 'PrinterName',
			LabelFileName: 'LabelFileName',
			FontSize: 'FontSize',
			TextToPrint: 'TextToPrint',
			MaterialInfo: 'MaterialInfo',
			ImageToPrint: 'ImageToPrint'
		}
	If you want to print an image, image should be send in base64 encoding to server. See point 4, about how to convert an image to base64.
	
	It returns true, or throw an error if printing failed.
	How to call this function using promises:
			testLabelObject.MaterialInfo = materialInfo;
			testLabelObject.TextToPrint = textToPrint;
			labelprinting.app.printLabel(testLabelObject).then(function (result) {
				if (result) {
					// printed succesfully
				}
			});
			
4. labelprinting.app.base64FileInputEncoder('fileInputId')
	Helper method of library. It encoded an image to base64.
	It receive as parameter the id of fileinput control.
	It returns base64 string of image.
	How to call this function using promises:
	
		labelprinting.app.base64FileInputEncoder('imageToPrint').then(function (imageToPrintBase64String) {
			testLabelObject.ImageToPrint = imageToPrintBase64String;
				// in here you can call print after print label image property has been updated.
			labelprinting.app.printLabel(testLabelObject).then(function(){
				......
			}

		});
 */

var labelprinting = labelprinting || {};


/** Configuration */
labelprinting.config = {};

labelprinting.app = function () {
	var current = null;
	/** init label printing */
	function init(url) {
		labelprinting.config = {
			printServiceEndpoint : 'http://localhost:50742/api/labelprinting/',
			getPrintersEndpoint : 'getprinters/',
			getPrinterLabelsEndpoint : 'getprinterlabels',
			printEndpoint : 'print'
		};
		
		if(url && url.length > 0){
			labelprinting.config.printServiceEndpoint = url;
		}
	}

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

	function getPrinterLabels(printerId) {
		// Return a new promise.
		return new Promise(function (resolve, reject) {
			// Do the usual XHR stuff
			var req = new XMLHttpRequest();
			var url = labelprinting.config.printServiceEndpoint + labelprinting.config.getPrinterLabelsEndpoint + '/' + printerId;
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

	/** Read an image from a file input and convert it to a base64 string */
	function base64FileInputEncoder(fileInputId) {
		return new Promise(function (resolve, reject) {
			var files = document.getElementById(fileInputId).files;
			if (files.length > 0) {
				var file = files[0];
				var reader = new FileReader();
				reader.readAsDataURL(file);
				reader.onload = function () {
					resolve(reader.result);
				};
				reader.onerror = function (error) {
					reject('Error on reading uploaded image');
				};
			} else {
				resolve('');
			}
		});
	}

	return {
		init : init,
		getPrinters : getPrinters,
		getPrinterLabels : getPrinterLabels,
		base64FileInputEncoder : base64FileInputEncoder,
		printLabel : printLabel
	}
}
();