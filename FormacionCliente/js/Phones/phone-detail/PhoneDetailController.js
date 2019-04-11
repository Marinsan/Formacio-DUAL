(function () {
    'use strict';

    angular.module('routerApp')
        .controller('PhoneDetailController',
        ['$scope', '$stateParams',
            function ($scope, $stateParams) {
        
                $scope.phone = $stateParams.telefonSeleccionat;

            }])
})();