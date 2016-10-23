(function () {
	angular.module('app')
		.factory('salesSummaryService', salesSummaryService);

	salesSummaryService.$inject = ['$resource'];
	function salesSummaryService($resource) {
		return {
			load: load
		}

		function load(endpoint, callback) {
			$resource(endpoint).query(success);

			function success(detail) {
				var result = {
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
				result.totals.margin = result.totals.profit / result.totals.gross * 100;
				callback(result);
			}
		}
	}
})();
