(function () {
	angular.module('app')
		.controller('SubscriberSalesController', Controller);

	Controller.$inject = ['$scope', 'itemSalesService'];

	function Controller($scope, itemSalesService) {
		$scope.fromDate = model.FromDate;
		$scope.toDate = model.ToDate;
		$scope.refresh = activate;

		activate();

		function activate() {
			$scope.loading = true;
			itemSalesService.loadSubscriberSales($scope.fromDate, $scope.toDate, success);

			function success(detail) {
				$scope.model = detail;
				$scope.loading = false;
			}
		};
	}
})();
