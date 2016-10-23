(function () {
	angular.module('app')
		.controller('ItemSalesController', Controller);

	Controller.$inject = ['$scope', 'itemSalesService'];

	function Controller($scope, itemSalesService) {
		$scope.fromDate = model.FromDate;
		$scope.toDate = model.ToDate;
		$scope.refresh = activate;
		$scope.toggleDetail = toggleDetail;

		activate();

		function activate() {
			$scope.loading = true;
			itemSalesService.load($scope.fromDate, $scope.toDate, null, success);

			function success(detail) {
				$scope.model = {
					detail: detail,
					totals: {
						gross: _.reduce(detail, function (memo, t) {
							return memo + t.grossAmount;
						}, 0),
						profit: _.reduce(detail, function (memo, t) {
							return memo + t.gpAmt;
						}, 0)
					}
				};
				$scope.model.totals.margin = $scope.model.totals.profit / $scope.model.totals.gross * 100;
				$scope.loading = false;
			}
		};

		function toggleDetail(item) {
			item.visible = !item.visible;
			if (!item.detail) {
				itemSalesService.loadDetail($scope.fromDate, $scope.toDate, item.itemId, success);
			}

			function success(result) {
				item.detail = result;
			}
		}

	}
})();
