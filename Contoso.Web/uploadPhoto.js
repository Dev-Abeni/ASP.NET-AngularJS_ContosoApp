angular.module('contosoApp').directive("contosoFiles", function ($parse) {
    return function link(scope, element, attrs) {
        var onChnage = $parse(attrs.contosoFiles);
        element.on("change", function (event) {
            onchange(scope, { $files: event.target.files });
        });
    }
});