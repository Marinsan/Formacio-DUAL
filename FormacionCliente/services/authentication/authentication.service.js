/**
 * Servei d'autenticacio.
 *
 * @author DISI - Antonio
 *
 */

(function () {
    'use strict';


    function AuthenticationProvider() {
        this.$get = function ($http, $q, Token, localStorageService, SERVER_API_URL, AUTENTICATION_URL) {
            var _currentUser = null;

            function _saveUserAndToken(token, refresh_token, sede) {
                // store token to local storage
                Token.set(token);
                // decode user data from payload token
                _currentUser = angular.fromJson(Token.decodeToken(token));
                // save user to locale storage
                localStorageService.set('user', _currentUser);
                localStorageService.set('user_refresh_token', refresh_token);

                if (sede != undefined) {
                    localStorageService.set('user_sede', sede);
                }

            }

            return {

                saveUserAndToken: function (token, refresh_token){
                    _saveUserAndToken(token, refresh_token);
                },

                logIn: function (params) {

                    var defered = $q.defer();
                    var promise = defered.promise;



                    $http({
                        method: 'POST',
                        url: SERVER_API_URL + AUTENTICATION_URL,
                        headers: {'Content-Type': 'application/x-www-form-urlencoded'},
                        transformRequest: function (obj) {
                            var str = [];
                            for (var p in obj)
                                str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                            return str.join("&");
                        },

                        data: {
                            username: params.username,
                            password: params.password,
                            grant_type: 'password'
                        }
                    }).then(
                        function successCallback(response) {
                            _saveUserAndToken(response.data.access_token, response.data.refresh_token, params.sede);

                            defered.resolve();
                        }
                        , function errorCallback(response) {
                            defered.reject(response);
                        }
                    );

                    promise.success = function (fn) {
                        promise.then(fn);
                        return promise;
                    };
                    promise.error = function (fn) {
                        promise.then(null, fn);
                        return promise;
                    };

                    return promise;
                },

                RefreshToken: function () {
                    var deferred = $q.defer();

                    var authData = localStorageService.get('user_refresh_token');


                    if (authData) {

                        var data = "grant_type=refresh_token&refresh_token=" + authData;

                            
                            $http.post(SERVER_API_URL + AUTENTICATION_URL, data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                                _saveUserAndToken(response.access_token, response.refresh_token);

                                deferred.resolve();

                            }).error(function (err, status) {
                                _currentUser = null;
                                Token.remove();
                                localStorageService.remove('user');
                                localStorageService.remove('user_refresh_token');
                                defered.reject(response);
                            });
                        
                    }

                    return deferred.promise;
                },

                logOut: function () {
                    _currentUser = null;
                    Token.remove();
                    localStorageService.remove('user');
                    localStorageService.remove('user_refresh_token');
                    localStorageService.remove('user_sede');
                },

                isAuthenticated: function () {
                    return !!Token.get();
                },

                getCurrentUser: function () {

                    if (!_currentUser) {
                        _currentUser = angular.fromJson(localStorageService.get('user'));

                        
                    }

                    //if (_currentUser) {
                    //    _currentUser.user_sedeId = localStorageService.get('user_sede');
                    //}

                    return _currentUser;
                    //return _currentUser || localStorageService.get('user')
                },

                getCurrentUserSede : function(){
                    return localStorageService.get('user_sede');
                }
            };
        };
    }

    angular
        .module('disi.authentication')
        .provider('Authentication', AuthenticationProvider);
})();
