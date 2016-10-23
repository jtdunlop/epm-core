angular.module('app')
	.controller('dailySalesController', [
		'$scope', 'salesSummaryService', function ($scope, salesSummaryService) {
			$scope.refresh = refresh;
			$scope.itemSales = itemSales;
			$scope.refresh();

			function refresh() {
				$scope.loading = true;
				var url = '/api/reporting/DailySales';
				salesSummaryService.load(url, success);

				function success(result) {
					$scope.model = result;
					$scope.loading = false;
				}
			};

			function itemSales(item) {
				var url = "/Reporting/ItemSales?fromDate=" + item.dateTime + "&toDate=" + item.dateTime;
				window.location = url;
			}
		}
	]);









