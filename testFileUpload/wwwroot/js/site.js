// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function () {
    "use strict";
    const app = angular.module("myApp", []);
    app.run(function($rootScope) {
        $rootScope.balance = "mock";
        $rootScope.errorMessage = "mock";
        $rootScope.error = false;
    });
}());

(function () {
    "use strict";
    angular.module("myApp").directive("fileBinder",
        function() {
            return function(scope, elm, attrs) {
                elm.bind("change",
                    function(evt) {
                        scope.$apply(function() {
                            scope[attrs.name] = evt.target.files;
                            console.log(scope[attrs.name]);
                        });
                    });
            };
        });
}());

(function () {
    "use strict";

    function fileUploadService($q, $http) {

        return {
            uploadFile: function uploadFile(file) {
                var deferred = $q.defer();
                if (file.size <= 1000000) {
                    const fd = new FormData();
                    fd.append("files", file);
                    $http.post("upload",
                            fd,
                            {
                                transformRequest: angular.identity,
                                headers: { "Content-Type": undefined }
                            })
                        .then(function (response) {
                            deferred.resolve(true);
                        })["catch"](function (reason) {
                            deferred.reject(reason.Message);
                        });
                } else {
                    deferred.reject("file too large");
                }

                return deferred.promise;
            }
        };
    }

    fileUploadService.$inject = ["$q", "$http"];
    angular.module("myApp").factory("fileUploadService", fileUploadService);

}());

(function () {
    "use strict";

    function fileUploadController(fileUploadService, $scope, $log) {
        const vm = this;

        function startUploading() {
            if ($scope.transaction_file && $scope.transaction_file.length > 0)
            {
                fileUploadService.uploadFile($scope.transaction_file[0]).then(() => {
                    $log.log("OK");
                }).catch((err) => {
                    $log.log(err);
                });
            }
        }

        vm.startUploading = startUploading;
    }

    fileUploadController.$inject = ["fileUploadService", "$scope", "$log"];
    angular.module("myApp").controller("fileUploadController", fileUploadController);

}());