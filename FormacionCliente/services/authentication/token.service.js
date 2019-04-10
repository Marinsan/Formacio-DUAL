/**
 * Servei de gestio del Token d'autenticacio (JWT)
 *
 * @author DISI - Antonio
 * 
 */

(function () {
    'use strict';

    
    function Token(localStorageService, jwtHelper ) { 
       
        var _tokenStorageKey = 'token';
        var _cachedToken = '';

       
        var set = function(token) {
            _cachedToken = token;
            localStorageService.set(_tokenStorageKey, token)
        };
       
        var get = function() {
            if (!_cachedToken) {
                _cachedToken = localStorageService.get(_tokenStorageKey);
            }
            return _cachedToken;
        };
       
        var remove = function() {
            _cachedToken = null;
            localStorageService.remove(_tokenStorageKey);
        };
       
        var decodeToken = function(token) {
            
             return JSON.stringify(jwtHelper.decodeToken(token));

        };

        return {
            set: set,
            get: get,
            remove: remove,
            decodeToken: decodeToken
        }
    }

    angular
        .module('disi.authentication')
        .factory('Token', Token);
})();
