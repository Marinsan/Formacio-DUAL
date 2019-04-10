/**
 * Modul principal d'autenticacio mitjançant JWT.
 *
 * @author DISI - Antonio
 * 
 *  @params - SERVER_API_URL    (obligatoria)-> adreça deñ server    
 *  @params - AUTENTICATION_URL (obligatoria)-> servei d'autenticacio
 *	@params - STORAGE_NAME_TOKEN (obligatoria)-> namespace del web storage de la app
 *
 *  @dependencies 
 *      js/moment.js 
 *  	js/constant/constants.js
 * 		lib/angular-jwt/dist/angular-jwt.min.js
 * 
 */

(function () {
    'use strict';

    angular.module('disi.authentication', ['angular-jwt','LocalStorageModule', 'ConstantsModule'])

   .config(['jwtOptionsProvider','localStorageServiceProvider', 'STORAGE_NAME_TOKEN', 'JWT_WHITE_LIST',
    function (jwtOptionsProvider, localStorageServiceProvider, STORAGE_NAME_TOKEN, JWT_WHITE_LIST) {
 	
	 		localStorageServiceProvider.setPrefix(STORAGE_NAME_TOKEN);
	 		jwtOptionsProvider.config({ whiteListedDomains: JWT_WHITE_LIST });
	}])
  
})();
