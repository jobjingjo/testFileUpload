// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function () {
    "use strict";
    var app = angular.module('myApp', []);
    app.run(function ($rootScope) {
        $rootScope.balance = "mock";
        $rootScope.errorMessage = "mock";
        $rootScope.error = false;
    });
}());

(function () {
    "use strict";
    angular.module('myApp').directive('fileBinder', function () {
        return function (scope, elm, attrs) {
            elm.bind('change', function (evt) {
                scope.$apply(function () {
                    scope[attrs.name] = evt.target.files;
                    console.log(scope[attrs.name]);
                });
            });
        };
    });
}());

(function () {
    "use strict";
    function fileUploadController($rootScope, $scope, $timeout, $window) {
        $scope.fileSelected = function () {
            console.log($scope.transaction_file);
        };

        $scope.startUploading = function() {
            console.log($scope.transaction_file);
        };
    }
    fileUploadController.$inject = ['$rootScope', '$scope', '$timeout', '$window'];
    angular.module('myApp').controller('fileUploadController', fileUploadController);

}());