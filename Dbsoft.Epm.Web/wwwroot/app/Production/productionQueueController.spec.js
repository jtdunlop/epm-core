/// <reference path="../../../scripts/testReferences.js" />
/// <reference path="../../../wwwroot/app/Production/productionQueueController.js" />

describe("productionQueueController", function () {
	// ReSharperReporter.prototype.jasmineDone = function () { };
	beforeEach(module('app'));

	var $controller;

	beforeEach(inject(function(_$controller_) {
		// The injector unwraps the underscores (_) from around the parameter names when matching
		$controller = _$controller_;
	}));
	describe("getQuantity", function () {
		it("Builds 8 when quantity: 1, batch: 8, blueprints:1", function () {
			var $scope = {};
			$controller('productionQueueController', { $scope: $scope });
			var item = {
				Quantity: 1,
				AvailableBlueprints: 1,
				MinimumStock: 8
			};
			expect($scope.getQuantity(item)).toBe(8);
		});
		it("Builds 8 when quantity: 9, batch: 8, blueprints:1", function () {
			var $scope = {};
			$controller('productionQueueController', { $scope: $scope });
			var item = {
				Quantity: 9,
				AvailableBlueprints: 1,
				MinimumStock: 8
			};
			expect($scope.getQuantity(item)).toBe(8);
		});
		it("Builds 16 when quantity: 9, batch: 8, blueprints:2", function () {
			var $scope = {};
			$controller('productionQueueController', { $scope: $scope });
			var item = {
				Quantity: 9,
				AvailableBlueprints: 2,
				MinimumStock: 8
			};
			expect($scope.getQuantity(item)).toBe(16);
		});
		it("Builds 16 when quantity: 17, batch: 8, blueprints:2", function () {
			var $scope = {};
			$controller('productionQueueController', { $scope: $scope });
			var item = {
				Quantity: 17,
				AvailableBlueprints: 2,
				MinimumStock: 8
			};
			expect($scope.getQuantity(item)).toBe(16);
		});
	});
});