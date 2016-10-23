angular.module('app').controller('marketResearchController', ['$scope', function ($scope) {
	$scope.model = window.marketResearchModel;
}]).filter('showItems', function () {
	return function (items, model) {
		var visible = [];
		window._.each(items, function (item) {
			if ((!item.IsMine || model.showMine) && (item.Markup < 100 || model.showOutliers)) {
				visible.push(item);
			}
		});
		return visible;
	};
});









