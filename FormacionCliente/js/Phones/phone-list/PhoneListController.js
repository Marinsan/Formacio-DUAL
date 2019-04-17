(function () {
    'use strict';

    angular.module('routerApp')
        .controller('PhoneListController',
        ['$scope', '$stateParams', 'TelefonoService',
            function ($scope, $stateParams, TelefonoService) {


                // Controlador llista telefons

                TelefonoService.getTelefonos()
                    .then(function (data) {
                        $scope.phones = data;
                        
                    },
                          function (error) {
                              alert("Error obteniendo los telefonos.")
                              console.log(error)
                          });

                $scope.orderProp = 'age';
                $scope.phone = $stateParams.telefonSeleccionat;

            }])
})();