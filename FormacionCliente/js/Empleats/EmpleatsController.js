(function () {
    'use strict';

    angular.module('routerApp')
        .controller('EmpleatController',
        ['$scope', '$stateParams', 'EmpleatService',
            function ($scope, $stateParams, EmpleatService) {


                // Controlador llista telefons

                EmpleatService.getEmpleats()
                    .then(function (data) {
                        $scope.empleat = data;
                        
                    },
                          function (error) {
                              alert("Error obteniendo los empleados.")
                              console.log(error)
                          });

                $scope.orderProp = 'id';

            }])
})();