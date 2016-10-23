(function () {
	angular.module('app')
		.factory('itemSalesService', service);

	service.$inject = ['$resource'];
	function service($resource) {
		return {
			load: load,
			loadDetail: loadDetail,
			loadSubscriberSales: loadSubscriberSales
	}

		function load(fromDate, toDate, id, callback) {
			$resource('/api/reporting/itemsales',
				{
					fromDate: moment(fromDate).format(),
					toDate: moment(toDate).format(),
					itemId: id
				}).query(success);

			function success(detail) {
				callback(detail);
			}
		}

		function loadDetail(fromDate, toDate, id, callback) {
			$resource('/api/reporting/itemsalesdetail',
				{
					fromDate: moment(fromDate).format(),
					toDate: moment(toDate).format(),
					itemId: id
				}).query(success);

			function success(detail) {
				callback(detail);
			}
		}

		function loadSubscriberSales(fromDate, toDate, callback) {
			$resource('/api/reporting/subscribersales',
				{
					fromDate: moment(fromDate).format(),
					toDate: moment(toDate).format()
				}).query(success);

			function success(detail) {
				callback(detail);
			}
		}
	}
})();
