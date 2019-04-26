(function () {
    'use strict';

    angular.module('routerApp')

        .factory('EmpleatService', ['$http', '$q', 'SERVER_API_URL', function ($http, $q, SERVER_API_URL) {
      
            var _getEmpleats = function () {

                var defered = $q.defer();
                var promise = defered.promise;

                $http({
                    method: 'GET',
                    url: SERVER_API_URL + '/api/empleat/empleats',
                    cache: true
                   
                }).then(function successCallback(response) {

                    defered.resolve(response.data);
                    //console.log("data: ", response.data[0])

                }, function errorCallback(response) {
                    defered.reject(response);
                });

                return promise;

            };

            var _getEmpleat = function (codi) {

                var defered = $q.defer();
                var promise = defered.promise;

                $http({
                    method: 'GET',
                    url: SERVER_API_URL + '/api/empleat/' + codi,
                    cache: true


                }).then(function successCallback(response) {

                    defered.resolve(response.data);

                }, function errorCallback(response) {
                    defered.reject(response);
                });

                return promise;

            };

            return {

                getEmpleats: function () {

                    var defered = $q.defer();
                    var promise = defered.promise;

                    _getEmpleats().then(function (empleats) {
                        defered.resolve(empleats);
                    }, function (error) {
                    });

                    return promise;
                },

                getEmpleat: function (codi) {

                    var defered = $q.defer();
                    var promise = defered.promise;

                    _getEmpleat(codi).then(function (empleats) {
                        defered.resolve(empleats);
                    }, function (error) {
                    });

                    return promise;
                }
            }

        }])
})();