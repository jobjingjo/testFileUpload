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

(function() {
    "use strict";

    function fileUploadController($rootScope, $scope, $timeout, $window, $q, $http) {
        var vm = this;

        vm.startUploading = startUploading;

        function startUploading() {
            if ($scope.transaction_file && $scope.transaction_file.length>0)
            uploadFile($scope.transaction_file[0]).then(() => {
                console.log("OK");
            }).catch(err => {
                console.log(err);
            });
        }


        function uploadFile(file) {
            var deferred = $q.defer();
            if (file.size <= 1000000) {
                const fd = new FormData();
                fd.append('file', file);
                $http.post("upload",
                        fd,
                        {
                            transformRequest: angular.identity,
                            headers: { 'Content-Type': undefined }
                        })
                    .then(function(response) {
                        deferred.resolve(true);
                    })["catch"](function(reason) {
                        deferred.reject(reason.Message);
                    });
            } else {
                deferred.reject('file too large');
            }

            return deferred.promise;
        }
    }

    fileUploadController.$inject = ['$rootScope', '$scope', '$timeout', '$window', '$q', '$http'];
    angular.module('myApp').controller('fileUploadController', fileUploadController);

}());