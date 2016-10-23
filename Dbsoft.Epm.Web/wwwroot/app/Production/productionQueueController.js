angular.module('app')
	.run([
		"$http", function($http) {
			$http.defaults.headers.common.Authorization = 'Bearer ' + window.token;
		}
	])
	.controller('productionQueueController', [
		'$scope', '$timeout', '$resource', function($scope, $timeout, $resource) {
			var Item = $resource('/api/item/:id/batchsize', null,
			{
				'$update': { method: 'PUT' }
			});

			$scope.model = window.model;

			$scope.getClass = function(item) {
				return item.Markup < item.MinimumMarkup ? "error" : "";
			};

			var roundup = function(num, multiple) {
				return Math.ceil(num / multiple) * multiple;
			};

			$scope.getQuantity = function(item) {
				var quantity = Math.min(item.Quantity, item.MinimumStock * item.AvailableBlueprints);
				quantity = roundup(quantity, item.MinimumStock);
				return quantity;
			};

			$scope.updateBatchSize = function(item) {
				item.status = 'saving';
				Item.$update({
					id: item.ItemId
				}, { batchSize: item.MinimumStock }, function(success) {
					item.status = 'saved';
					$timeout(function() {
						item.status = '';
					}, 2000);
				});
			};
		}
	])
	.filter('showItems', function() {
		return function(items, model) {
			var visible = [];
			_.each(items, function(item) {
				if (item.Quantity > 0 && item.MinimumStock > 0 || model.showAll) {
					visible.push(item);
				}
			});
			return visible;
		}
	});







