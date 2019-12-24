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
    function withdrawController($rootScope, $scope, accountService, $timeout, $window) {

        $scope.doWithdraw = function () {
            accountService.withdraw($scope.amount).then(x => {
                $rootScope.balance = x;
                $rootScope.error = false;
                console.log(x);
                $('.toast').toast('show');

                $timeout(function () { $window.location.href = 'https://localhost:44318/Account/Index'; }, 1000);

            }).catch(err => {
                $rootScope.error = true;
                $rootScope.errorMessage = err;
                $('.toast').toast('show');
                console.log(err);
            });
        };
    }
    withdrawController.$inject = ['$rootScope', '$scope', 'accountService', '$timeout', '$window'];
    angular.module('myApp').controller('withdrawController', withdrawController);

}());