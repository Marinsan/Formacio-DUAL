(function () {
    'use strict';

    angular.module('MainApp', [ 'ui.router', 
                                'ngMaterial',
                                'disi.authentication',
                                'ConstantsModule',
                                'LocalStorageModule',
                                'md.data.table'

    ])
   .run(['$rootScope', '$trace', '$transitions', '$state', 'Authentication',  
     function ($rootScope, $trace, $transitions, $state, Authentication) {

                moment.locale('es');
            
                

            //cabecera
                
                $rootScope.user = Authentication.getCurrentUser();
                
                $rootScope.logOut = function () {
                    Authentication.logOut();
                    $rootScope.user = undefined;
                    $rootScope.sede = undefined;
                    $rootScope.auxSedeDescripcion = undefined;
                    $state.go("Login", {}, { reload: true });
                }
                

               $rootScope.openMenu = function (menuProvider, e) {
                    menuProvider.open();
                }
         //FI cabecera

                //$trace.enable('TRANSITION');
                //https://ui-router.github.io/ng1/docs/latest/interfaces/transition.hookmatchcriteria.htmlç

                $transitions.onStart(
                    { to: function(state){
                            return state.data != null && state.data.seguro === true;
                          }
                    }, 
                    function (trans) {
                   
                        if (!Authentication.isAuthenticated()) {
                            return trans.router.stateService.target('Login');
                        }
                    }
                );

                //$transitions.onStart({ to: 'Menu.**' }, 
                //    function (trans) {
                //    var auth = trans.injector().get('Authentication');
                //    if (!auth.isAuthenticated()) {
                //        return trans.router.stateService.target('Login');
                //    }
                //});


            }])

   .config(['$stateProvider', '$urlRouterProvider', '$mdDateLocaleProvider', function ($stateProvider, $urlRouterProvider, $mdDateLocaleProvider) {

       $mdDateLocaleProvider.formatDate = function (date) {
           return moment(date).format('DD/MM/YYYY');
       };
       $mdDateLocaleProvider.firstDayOfWeek = 1;

       $stateProvider
           .state('Login', {
               url: '/Login',
               data: { seguro: false },
               views: {
                   'principal': {
                       templateUrl: 'script/controler/login/login.html',
                       controller: 'LoginControler'
                       
                   }
               }
           })
            .state('Menu', {
                url: '/Menu',
                data: { seguro: true },
                views: {
                    'principal': {
                        templateUrl: 'script/controler/menu/menu.html',
                        controller: 'MenuControler'
                    }
                }
            })
           .state('Logout', {
               url: '/Logout',
               data: { seguro: true },
               views: {
                   'principal': {
                       controller: 'LogoutControler'
                   }
               }
           })
           .state('AltaUsuario', {
               url: '/AltaUsuario',
               data: { seguro: true },
               views: {
                   'principal': {
                       templateUrl: 'script/controler/altaUsuario/altaUsuario.html',
                       controller: 'AltaUsuarioControler'
                   }
               }
           })
           .state('CheckIn', {
               url: '/CheckIn',
               data: { seguro: true },
               views: {
                   'principal': {
                       templateUrl: 'script/controler/CheckIn/CheckIn.html',
                       controller: 'CheckInControler'
                   }
               }
           })
            .state('CheckOut', {
                url: '/CheckOut',
                data: { seguro: true },
                views: {
                    'principal': {
                        templateUrl: 'script/controler/CheckOut/CheckOut.html',
                        controller: 'CheckOutControler'
                    }
                }
            })
           .state('GestionSaldo', {
               url: '/GestionSaldo/{cliente:json}',
               data: { seguro: true },
               params: { cliente: null },
               views: {
                   'principal': {
                       templateUrl: 'script/controler/recargaSaldo/recargaSaldo.html',
                       controller: 'recargaSaldoControler'
                   }
               }
           })
           .state('Venta', {
               url: '/Venta',
               data: { seguro: true },
               views: {
                   'principal': {
                       templateUrl: 'script/controler/venta/venta.html',
                       controller: 'ventaControler'
                   }
               }
           })
           .state('CrearGrupo', {
               url: '/CrearGrupo',
               data: { seguro: true },
               views: {
                   'principal': {
                       templateUrl: 'script/controler/crearGrupo/crearGrupo.html',
                       controller: 'crearGrupoControler'
                   }
               }
           })
           .state('Servir', {
               url: '/Servir',
               data: { seguro: true },
               views: {
                   'principal': {
                       templateUrl: 'script/controler/servir/servir.html',
                       controller: 'servirControler'
                   }
               }
           })
           .state('GestionStock', {
               url: '/GestionStock',
               data: { seguro: true },
               views: {
                   'principal': {
                       templateUrl: 'script/controler/gestionStock/gestionStock.html',
                       controller: 'gestionStockControler'
                   }
               }
           })
           .state('GestionLotes', {
               url: '/GestionLotes',
               data: { seguro: true },
               views: {
                   'principal': {
                       templateUrl: 'script/controler/gestionLotes/gestionLotes.html',
                       controller: 'gestionLotesControler'
                   }
               }
           })
           .state('ListasCreadas', {
               url: '/ListasCreadas',
               data: { seguro: true },
               views: {
                   'principal': {
                       templateUrl: 'script/controler/listaLotes/listaLotes.html',
                       controller: 'listaLotesControler'
                   }
               }
           })
           .state('Picking', {
               url: '/Picking/:idLista/:codigosLotes',
               data: { seguro: true },
               views: {
                   'principal': {
                       templateUrl: 'script/controler/picking/picking.html',
                       controller: 'pickingControler'
                   }
               }
           })
           .state('Packing', {
               url: '/Packing/:idLista/:codigosLotes',
               data: { seguro: true },
               views: {
                   'principal': {
                       templateUrl: 'script/controler/packing/packing.html',
                       controller: 'packingControler'
                   }
               }
           })
           .state('CierreLote', {
               url: '/CierreLote/:idLista/:codigosLotes',
               data: { seguro: true },
               views: {
                   'principal': {
                       templateUrl: 'script/controler/cierreLote/cierreLote.html',
                       controller: 'cierreLoteControler'
                   }
               }
           })

       $urlRouterProvider.otherwise('/Menu');

   }])
        /*
   .config(function ($mdThemingProvider) {

            var XmasColor = {
                 '50': '#e9797e',
                 '100': '#e56369',
                 '200': '#e24e54',
                 '300': '#de383f',
                 '400': '#d9242b',
                 '500': '#C32027',
                 '600': '#ad1c23',
                 '700': '#97191e',
                 '800': '#81151a',
                 '900': '#6b1215',
                 'A100': '#ed8f93',
                 'A200': '#C32027',
                 'A400': '#f4bbbe',
                 'A700': '#550e11',
                 'contrastDefaultColor': 'dark',
                 'contrastLightColors': []
             };

             $mdThemingProvider.definePalette('XMAS_Palete', XmasColor)

       $mdThemingProvider.theme('XMAS_Palete')
                 .primaryPalette('XMAS_Palete');
                 //.accentPalette('XMAS_Palete');
             //    .warnPalette('orange')
             //    .backgroundPalette('red');
         })
        */

        //.config(function($mdThemingProvider) {
        //    $mdThemingProvider.definePalette('amazingPaletteName', {
        //        '50': 'ffebee',
            
        //      'A700': 'd50000',
        //        'contrastDefaultColor': 'light',    // whether, by default, text (contrast)
        //        // on this palette should be dark or light
        //        'contrastDarkColors': ['50', '100', //hues which contrast should be 'dark' by default
        //         '200', '300', '400', 'A100'],
        //        'contrastLightColors': undefined    // could also specify this if default was 'dark'
        //    });
        //    $mdThemingProvider.theme('default')
        //      .primaryPalette('amazingPaletteName')

    .config(['localStorageServiceProvider', 'STORAGE_NAME_TOKEN',
            function (localStorageServiceProvider, STORAGE_NAME_TOKEN) {

                localStorageServiceProvider.setPrefix(STORAGE_NAME_TOKEN);

            }])

    .directive('loading', ['$http', function ($http) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                scope.isLoading = function () {
                    return $http.pendingRequests.length > 0;
                };
                scope.$watch(scope.isLoading, function (value) {
                    if (value) {
                        element.removeClass('ng-hide');
                    } else {
                        element.addClass('ng-hide');
                    }
                });
            }
        };
    }])

  

  
})();
