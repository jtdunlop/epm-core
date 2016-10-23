(function () {
	angular.module('app')
		.controller('ItemCostTrendController', Controller);

	Controller.$inject = ['$scope', 'itemCostTrendService'];
	function Controller($scope, itemCostTrendService) {
		$scope.toggleDetail = toggleDetail;
		activate();

		function activate() {
			$scope.loading = true;
			itemCostTrendService.load(success);

			function success(result) {
				$scope.model = result;
				$scope.loading = false;
			}
		};

		function toggleDetail(item) {
			item.visible = !item.visible;
			if (!item.detail) {
				itemCostTrendService.loadMaterials(item.itemId, success);
			}

			function success(result) {
				item.detail = result;
			}
		}
	}
})();











