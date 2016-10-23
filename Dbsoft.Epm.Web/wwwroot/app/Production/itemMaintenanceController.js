angular.module('app').controller('itemMaintenanceController', ['$scope', '$http', function ($scope, $http) {
	$scope.model = window.itemMaintenanceModel;

	$scope.cancelEditItem = function (item) {
		window._.extend(item, item.saved);
		item.isEditing = false;
	};

	$scope.beginEditItem = function (item) {
		item.saved = window._.clone(item);
		item.isEditing = true;
	};

	$scope.saveItem = function (item) {
		var url = $('#itemMaintenance').data('save-item-url');
		item.isSaving = true;
		$http.post(url, item)
			.success(function (response) {
				item.isSaving = false;
				item.isEditing = false;
			});
	};

}]).filter('toTrusted', ['$sce', function ($sce) {
	return function (text) {
		return $sce.trustAsHtml(text);
	};
}]);

