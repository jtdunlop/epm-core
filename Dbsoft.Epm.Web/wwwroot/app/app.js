angular.module('app', ['ngResource', 'ui.date', 'submit', 'validated.input', 'ui.bootstrap']);

if (typeof CCPEVE != 'undefined') {
	CCPEVE.requestTrust('http://' + document.location.hostname + '/')();
}
