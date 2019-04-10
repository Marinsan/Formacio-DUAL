
/**
 * Modul de Configuracio d'autenticacio mitjançant JWT.
 *
 * @author DISI - Antonio
 * 
 */

(function () {  
    'use strict';

angular
    .module('disi.authentication')
    .config(function Config($httpProvider, jwtInterceptorProvider) {

        jwtInterceptorProvider.tokenGetter = ['jwtHelper', 'Token', 'Authentication' , function (jwtHelper, Token, Authentication) {
            
                    var idToken = Token.get();
                    
                    if (idToken == null) {
                        return null;
                    }

                    try {
                        if (jwtHelper.isTokenExpired(idToken)) {

                            //    return auth.refreshIdToken(refreshToken).then(function(idToken) {
                            //        localStorage.setItem(nameTokenSecurity, idToken);
                            //        return idToken;
                            //    })
                            return null;
                        }
                        else {

                            var minuts_fins_caducar = (moment(JSON.parse(Token.decodeToken(idToken)).exp * 1000).unix()  - moment().unix()) / 60;
                            var minuts_assignats = (moment(JSON.parse(Token.decodeToken(idToken)).exp * 1000).unix() - moment(JSON.parse(Token.decodeToken(idToken)).nbf * 1000).unix()) / 60;

                            if (minuts_fins_caducar < (minuts_assignats / 2)) {
                                console.log('Actualizo Token');
                                Authentication.RefreshToken();
                                
                            }
                        }
                    }catch (ex) {
                      Token.remove();
                      return null;
                    }

                    return idToken;

         }];
         $httpProvider.interceptors.push('jwtInterceptor');
    })

})();
