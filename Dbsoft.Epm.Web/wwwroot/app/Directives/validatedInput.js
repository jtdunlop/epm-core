angular.module('validated.input', [])
.directive('validatedInput', ['$compile', function ($compile) {
	var camelToDash = function (str) {
		return str.replace(/\W+/g, '-')
				  .replace(/([a-z\d])([A-Z])/g, '$1-$2');
	};
	var getInput = function (element, attrs, formName) {
		var passThroughAttributes = '';
		for (var attr in attrs) {
			var value = attrs[attr];
			if (attr[0] != "$" && attr != "class" && attr != "ngClass") {
				if (attrs.$attr[attr].substring(0, 3) == "pt-") {
					passThroughAttributes += String.format("{0}=\"{1}\" ", camelToDash(attr.substring(2)), value);

				} else {
					passThroughAttributes += String.format("{0}=\"{1}\" ", camelToDash(attr), value);
				}
			}
		}
		var allClasses = (attrs['class'] || 'validated');
		var validatedClass = allClasses.split(" ")[0];
		var inputClass = String.format("{0}-input", validatedClass);
		var inputInvalidClass = String.format("{0}-invalid", inputClass);
		var ngc = attrs.ngClass || String.format("{'{2}': rc.{0}.needsAttention({0}.{1})}", formName, attrs.name, inputInvalidClass);
		var ngClass = String.format("ng-class=\"{0}\"", ngc);
		var input = String.format("<input class=\"{0} {1}\" {2} {3}/>", allClasses, inputClass, ngClass, passThroughAttributes);
		return input;
	};
	var getLabel = function (element, attrs) {
		var display = attrs.display || attrs.name;
		var allClasses = (attrs['class'] || 'validated');
		var validatedClass = allClasses.split(" ")[0];
		var labelClass = String.format("{0}-label", validatedClass);
		return String.format("<span class=\"{1}\">{0}</span>", display, labelClass);
	};
	var getMessage = function (element, attrs, formName) {
		var allClasses = (attrs['class'] || 'validated');
		var validatedClass = allClasses.split(" ")[0];
		var messageClass = String.format("{0}-message", validatedClass);
		var display = attrs.display || attrs.name;
		var ngShow = String.format("rc.{0}.needsAttention({0}.{1})", formName, attrs.name);
		var requiredSpan = String.format("<span ng-show=\"{0}.{1}.$error.required\">{2} is required</span>", formName, attrs.name, display);
		var numberSpan = String.format("<span ng-show=\"{0}.{1}.$error.number\">{2} must be a number</span>", formName, attrs.name, display);
		return String.format("<div class=\"{3}\" ng-show=\"{0}\">{1}{2}</div>", ngShow, requiredSpan, numberSpan, messageClass);
	};
	var getDiv = function (element, attrs, formName) {
		var allClasses = (attrs['class'] || 'validated');
		var validatedClass = allClasses.split(" ")[0];
		var groupClass = String.format("{0}-input-group", validatedClass);
		return String.format("<div class=\"{3}\">{2}<div class=\"{4}\">{0}{1}</div></div", getInput(element, attrs, formName), getMessage(element, attrs, formName),
			getLabel(element, attrs), validatedClass, groupClass);
	};

	return {
		restrict: 'E',
		scope: true,
		require: ['^submit'],
		compile: function (element) {
			return {
				pre: function (scope, e, a) {
					var div = getDiv(element, a, scope.formName);
					element.removeAttr("popover");
					element.html(div);
					$compile(element.contents())(scope);
				}
			};
		}
	};

}]);


if (!String.format) {
	String.format = function (format) {

		var args = Array.prototype.slice.call(arguments, 1);

		var sprintf = function (match, number) {
			return number in args ? args[number] : match;
		};

		var sprintfRegex = /\{(\d+)\}/g;

		return format.replace(sprintfRegex, sprintf);
	};
}
