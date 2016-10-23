(function () {
	angular.module('app')
		.controller('MaterialCostTrendController', Controller);

	Controller.$inject = ['$scope', 'materialCostTrendService'];
	function Controller($scope, materialCostTrendService) {
		activate();

		function activate() {
			$scope.loading = true;
			materialCostTrendService.load(success);

			function success(result) {
				$scope.model = result;
				$scope.loading = false;
			}
		};
	}
})();











