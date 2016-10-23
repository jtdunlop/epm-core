angular.module('app')
	.run(["$http", function ($http) {
		$http.defaults.headers.common.Authorization = 'Bearer ' + window.token;
	}])
	.controller('productionMaterialController', [
		'$scope', '$timeout', '$resource', function ($scope, $timeout, $resource) {
			var Item = $resource('/api/item/:id/multiplier', null,
			{
				'$update': { method: 'PUT' }
			});

			$scope.model = window.model;

			$scope.itemClass = function(item) {
				return item.Needed > item.Available ? "error" : "";
			};

			$scope.updateMultiplier = function(item) {
				item.status = 'saving';
				Item.$update({
					id: item.ItemId
				}, { bounceFactor: item.BounceFactor}, function(success) {
					item.status = 'saved';
					$timeout(function() {
						item.status = '';
					}, 2000);
				});
			};
		}
	]);






