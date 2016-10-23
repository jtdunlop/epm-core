angular.module('app').controller('materialMaintenanceController', ['$scope', '$http', function ($scope, $http) {
	$scope.model = window.materialMaintenanceModel;

	$scope.cancelEditItem = function (item) {
		window._.extend(item, item.saved);
		item.isEditing = false;
	};

	$scope.beginEditItem = function (item) {
		item.saved = window._.clone(item);
		item.isEditing = true;
	};

	$scope.saveItem = function (item) {
		var url = $('#materialMaintenance').data('save-item-url');
		item.isSaving = true;
		$http.post(url, item)
			.success(function (response) {
				item.LastModified = response.LastModified;
				item.isSaving = false;
				item.isEditing = false;
			});
	};

}]).filter('toTrusted', ['$sce', function ($sce) {
	return function (text) {
		return $sce.trustAsHtml(text);
	};
}]);






