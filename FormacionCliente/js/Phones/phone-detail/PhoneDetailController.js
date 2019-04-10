(function () {
    'use strict';

    angular.module('routerApp')
        .controller('PhoneDetailController',
        ['$scope', '$stateParams',
            function ($scope, $stateParams) {
                // Controlador vista detalls telefons 

                $scope.phone = $stateParams.telefonSeleccionat;

            }])
})();