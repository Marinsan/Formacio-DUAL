(function () {
    'use strict';

    angular.module('routerApp')

        .factory('TelefonoService', ['$http', '$q', 'SERVER_API_URL', function ($http, $q, SERVER_API_URL) {
      
            var _getTelefonos = function () {

                var defered = $q.defer();
                var promise = defered.promise;

                $http({
                    method: 'GET',
                    url: SERVER_API_URL + '/api/telefono/telefonos',
                    cache: true
                   
                }).then(function successCallback(response) {

                    defered.resolve(response.data);
                    //console.log("data: ", response.data[0].so.versionSo)

                }, function errorCallback(response) {
                    defered.reject(response);
                });

                return promise;

            };

            var _getTelefono = function (codigo) {

                var defered = $q.defer();
                var promise = defered.promise;

                $http({
                    method: 'GET',
                    url: SERVER_API_URL + '/api/telefono/' + codigo,
                    cache: true


                }).then(function successCallback(response) {

                    defered.resolve(response.data);

                }, function errorCallback(response) {
                    defered.reject(response);
                });

                return promise;

            };

            return {

                getTelefonos: function () {

                    var defered = $q.defer();
                    var promise = defered.promise;

                    _getTelefonos().then(function (telefonos) {
                        defered.resolve(telefonos);
                    }, function (error) {
                    });

                    return promise;
                },

                getTelefono: function (codigo) {

                    var defered = $q.defer();
                    var promise = defered.promise;

                    _getTelefono(codigo).then(function (telefonos) {
                        defered.resolve(telefonos);
                    }, function (error) {
                    });

                    return promise;
                }
            }

        }])
})();