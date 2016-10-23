angular.module('app')
	.controller('monthlySalesController', [
		'$scope', 'salesSummaryService', function ($scope, salesSummaryService) {
			$scope.refresh = refresh;
			$scope.itemSales = itemSales;
			$scope.refresh();

			function refresh() {
				$scope.loading = true;
				var url = '/api/reporting/MonthlySales';
				salesSummaryService.load(url, success);

				function success(result) {
					$scope.model = result;
					$scope.loading = false;
				}
			};

			function itemSales(item) {
				var to = moment(item.dateTime).endOf('month').format('YYYY-MM-D');
				var url = "/Reporting/ItemSales?fromDate=" + item.dateTime + "&toDate=" + to;
				window.location = url;
			}
		}
	]);









