angular.module('app').controller('refreshApiController', ['$scope', '$http', function ($scope, $http) {

	$scope.refresh = function () {
		var uri = window.baseUri + 'Production/GetApiStatus';
		$http.get(uri).success(function (result) {
			$scope.items = result;
			$scope.running = _.some($scope.items, function (f) {
				return f.Result === 'Running';
			});
		});
	};

	$scope.refresh();

	var hub = $.connection.refreshHub;
	hub.client.refreshUpdated = function () {
		$scope.refresh();
	};

	$.connection.hub.start();

	$scope.startRefresh = function () {
		if ($scope.running) {
			return;
		}
		$scope.running = true;
		var uri = window.baseUri + 'Production/StartApiRefresh';
		$http.post(uri);
	};

	$scope.getRowClass = function (item) {
		return item.Expired ? 'error' : '';
	};
}]);