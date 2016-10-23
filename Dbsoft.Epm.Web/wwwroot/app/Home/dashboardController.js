angular.module('app').controller('dashboardController', ['$scope', '$resource', function ($scope, $resource) {
	$scope.model = window.model;
	$scope.model.loading = true;

	var url = '/api/dashboard';
	$resource(url, {token: window.token}).get(function (success) {
		$scope.model = success;
		$scope.model.loading = false;
	});
}]);

