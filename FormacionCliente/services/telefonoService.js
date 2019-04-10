(function () {
    'use strict';

    /**
    * @ngdoc factory
    * @name app.PhonesService
    * 
    * @description
    * Simula una api de phones, retorna json de la carpeta "assets/json/".
    *
    * @example
    * <pre>PhonesService.getAll()
    * .then(succesFn())
    * .catch(errorFn());
    * <pre>
    **/
    angular
    .module('routerApp')
    .factory('PhonesService', PhonesService);

    PhonesService.$inject = ['$http', '$q', 'SERVER_API_URL'];

    function PhonesService($http, $q, SERVER_API_URL) {
        var service = {
            getAll: _getAll,
            getPhone: _getPhone,
        };

        return service;


        function _getPhone(id) {
            var defered = $q.defer();
            var promise = defered.promise;

            $http.get('assets/json/phones/' + id + '.json')
            .then(function (response) {
                defered.resolve(response.data);
            })
            .catch(function (err) {
                defered.reject(err)
            });

            return promise;
        }

        function _getAll() {

            var defered = $q.defer();
            var promise = defered.promise;

            $http.get(SERVER_API_URL + 'api/telefonos')
            .then(function (response) {
                defered.resolve(response.data);
            })
            .catch(function (err) {
                defered.reject(err)
            });

            return promise;
        }
    }
})();