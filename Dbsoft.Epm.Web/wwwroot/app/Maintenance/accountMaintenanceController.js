/// <reference path="../../underscore.js" />
/// <reference path="../app.js" />
angular.module('app').controller('accountMaintenanceController', ['$scope', '$http', function ($scope, $http) {
	$scope.model = window.accountMaintenanceModel;

	$scope.hasVisibleAccount = function () {
		return window._.some($scope.model.Accounts, function (account) {
			return !account.DeletedFlag;
		});
	};

	$scope.beginEditAccount = function (account) {
		$scope.model.saved = window._.clone(account);
		account.isEditing = true;
		account.isError = false;
	};

	$scope.saveAccount = function (account) {
		var url = $('#accountMaintenance').data('save-account-url');
		account.isSaving = true;
		account.isError = false;
		$http.post(url, account)
			.success(function (response) {
				account.isError = !response.success;
				account.errorMessage = response.message;
				account.isSaving = false;
				account.isEditing = !response.success;
				if (response.success) {
					account.ApiKeyType = response.model.apiKeyType;
					account.ApiAccessMask = response.model.apiAccessMask;
				}
			});
	};

	$scope.addAccount = function () {
		$scope.model.Accounts.push({ isEditing: true, isAdding: true });
	};

	$scope.cancelEditAccount = function (account) {
		window._.extend(account, $scope.model.saved);
		if (account.isAdding) {
			account.isRemoved = true;
		}
		account.isEditing = false;
	};

	$scope.maskEnablesTransactions = function (mask, keyType) {
		return keyType == 0 ? (mask & 0x400000) != 0 : (mask & 0x200000) != 0;
	};

	$scope.maskEnablesOrders = function (mask) {
		return (mask & 0x1000) != 0;
	};

	$scope.maskEnablesBalance = function (mask) {
		return (mask & 0x01) != 0;
	};

	$scope.maskEnablesAssets = function (mask) {
		return (mask & 0x02) != 0;
	};

	$scope.maskEnablesIndustryJobs = function (mask) {
		return (mask & 0x80) != 0;
	};
}]).filter('filterAccounts', function () {
	return function (accounts, showDeleted) {
		var visibleAccounts = [];
		window._.each(accounts, function (account) {
			if (!account.isRemoved && (showDeleted || !account.DeletedFlag || account.isEditing)) {
				visibleAccounts.push(account);
			}
		});
		return visibleAccounts;
	};
});


