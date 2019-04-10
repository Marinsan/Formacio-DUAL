(function () {

    'use strict';

    angular.module('ConstantsModule', [])

        //GENERALS



        .constant('APP_VERSION', "1.00")

        .constant('SERVER_API_URL', 'http://localhost:57211')



        //disi.authentication

           .constant('AUTENTICATION_URL', '/oauth/token')

           .constant('STORAGE_NAME_TOKEN', 'APP_XMAS_MARKET')

        .constant('JWT_WHITE_LIST', ["www.xmarket.com", "demos.disisl.com", "localhost"])



})();