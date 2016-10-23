var app = angular.module('app', ['ui.date', 'submit', 'validated.input', 'ui.bootstrap']);
app.controller('configurationController', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {
	$scope.model = window.configurationModel;

	$scope.getFactoryStation = function (stationId) {
		var url = $('#configuration').data('station-lookup-url');
		return $http.get(url + "?stationID=" + stationId).then(function (response) {
			$scope.model.factoryStationName = response.data.model ? response.data.model.stationName : '';
		});
	};

	$scope.getMarketStation = function (stationId) {
		var url = $('#configuration').data('station-lookup-url');
		return $http.get(url + "?stationID=" + stationId).then(function (response) {
			$scope.model.sellStationName = response.data.model ? response.data.model.stationName : '';
		});
	};

	$scope.getBuyStation = function (stationId) {
		var url = $('#configuration').data('station-lookup-url');
		return $http.get(url + "?stationID=" + stationId).then(function (response) {
			$scope.model.buyStationName = response.data.model ? response.data.model.stationName : '';
		});
	};

	$scope.getPosSolarSystem = function (solarSystemId) {
		var url = $('#configuration').data('solar-system-lookup-url');
		return $http.get(url + "?solarSystemID=" + solarSystemId).then(function (response) {
			$scope.model.posLocationName = response.data.model ? response.data.model.solarSystemName : '';
		});
	};

	$scope.getStations = function (stationPartial) {
		var url = $('#configuration').data('station-list-url');

		return $http.get(url + "?stationPartial=" + stationPartial).then(function (response) {
			$scope.allStations = response.data.model;
			return _.pluck(response.data.model, "stationName");
		});
	};

	$scope.getSolarSystems = function (solarSystemPartial) {
		var url = $('#configuration').data('solar-system-list-url');

		return $http.get(url + "?systemPartial=" + solarSystemPartial).then(function (response) {
			$scope.allSolarSystems = response.data.Model;
			return _.pluck(response.data.Model, "SolarSystemName");
		});
	};

	$scope.selectSellMarket = function (stationName) {
		$scope.model.MarketSellID = _.find($scope.allStations, function (station) {
			var result = station.stationName === stationName;
			return result;
		}).stationID;
	};

	$scope.selectBuyMarket = function (stationName) {
		$scope.model.MarketBuyID = _.find($scope.allStations, function (station) {
			return station.stationName === stationName;
		}).stationID;
	};

	$scope.selectFactory = function (stationName) {
		$scope.model.FactoryID = _.find($scope.allStations, function (station) {
			return station.stationName === stationName;
		}).stationID;
	};

	$scope.selectPosLocation = function (systemName) {
		$scope.model.PosLocationID = _.find($scope.allSolarSystems, function (system) {
			return system.solarSystemName === systemName;
		}).solarSystemID;
	};

	$scope.saveConfiguration = function () {
		var url = $('#configuration').data('save-configuration-url');
		$scope.model.isSaving = true;
		if (!$scope.model.PosEnabled) {
			$scope.model.PosLocationID = null;
		}
		$http.post(url, $scope.model)
			.success(function () {
				$scope.model.saveResult = "Configuration saved";
				$timeout(function () {
					$scope.model.saveResult = "";
				}, 3000);
			})
			.error(function (response) {
				$scope.model.saveResult = "Error: " + response.Message;
			});
		$scope.model.isSaving = false;
	};

	$scope.getMarketStation($scope.model.MarketSellID);
	$scope.getFactoryStation($scope.model.FactoryID);
	$scope.getPosSolarSystem($scope.model.PosLocationID);
	$scope.getBuyStation($scope.model.MarketBuyID);
}])