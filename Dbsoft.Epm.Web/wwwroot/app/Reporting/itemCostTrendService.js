(function () {
	angular.module('app')
		.factory('itemCostTrendService', service);

	service.$inject = ['$resource'];
	function service($resource) {
		return {
			load: load,
			loadMaterials: loadMaterials
		}

		function load(callback) {
			$resource('/api/reporting/itemcosttrends').query(success);

			function success(detail) {
				callback(detail);
			}
		}

		function loadMaterials(id, callback) {
			$resource('/api/reporting/itemmaterialcosttrends?itemId=' + id).query(success);

			function success(detail) {
				callback(detail);
			}
		}
	}
})();
