(function () {
	angular.module('app')
		.factory('materialCostTrendService', service);

	service.$inject = ['$resource'];
	function service($resource) {
		return {
			load: load
		}

		function load(callback) {
			$resource('/api/reporting/materialcosttrends').query(success);

			function success(detail) {
				callback(detail);
			}
		}
	}
})();
