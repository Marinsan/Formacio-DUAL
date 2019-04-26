var routerApp = angular.module('routerApp',
    ['ui.router',
     'core',
     'ngAnimate',
     'ngMaterial',
     'ngMessages',
     'dx',
     'ngStorage',
     'ngCookies',
     'ngMap',
     'disi.authentication',
     'ConstantsModule',
     'LocalStorageModule',
     'ngMdIcons',
    ]);

routerApp.config(function ($stateProvider, $urlRouterProvider) {

     $urlRouterProvider.otherwise('/home');

    $stateProvider

         .state('Login', {
             url: '/login',
             data: { seguro: false },
             templateUrl: 'js/Login/login.html',
             controller: 'LoginControler'
         })
    
        .state('phone', {
            url: '/phone',
            data: { seguro: true },
            views: {
                "": {
                    templateUrl: 'js/Phones/partial-phone.html',
                   
                },

                'Phones@phone':
                {
                    templateUrl: 'js/Phones/phone-list/partial-phone-list.html',
                    controller: 'PhoneListController',
                },
                
                'Details@phone': {
                    
                    templateUrl: 'js/Phones/phone-detail/partial-phone-detail.html',
                    controller: 'PhoneDetailController',
                   
                }
            },
            params: {
                telefonSeleccionat: {}
            },
            
        })

    
    .state('form', {
        url: '/form',
        data: { seguro: true },
        templateUrl: 'js/Forms/partial-form.html',
        
       
    })

    .state('formng', {
        url: '/formNgMessages',
        data: { seguro: true },
        templateUrl: 'js/Forms/partial-form-ngmessages.html',
      
        
    })
    .state('formMaterial', {
        url: '/formMaterial',
        data: { seguro: true },
        templateUrl: 'js/Forms/partial-form-material.html',
        
        })
    .state('help', {
        url: '/help',
        data: { seguro: true },
        templateUrl: 'js/Help/help.html',
       
        })
    .state('home', {
        url: '/home',
        data: { seguro: true },
        templateUrl: 'js/Home/home.html',
        
         })
     .state('contact', {
         url: '/contact',
         data: { seguro: true },
         templateUrl: 'js/Contact/contact.html',
         
        })
    .state('taula', {
        url: '/taula',
        data: { seguro: true },
        templateUrl: 'js/Data-devextrem/data-grid.html',
        controller: 'GridController',
        
    })
        .state('taulaapi', {
            url: '/taula_api',
            data: { seguro: true },
            templateUrl: 'js/Data-devextrem/data-grid-api.html',
            controller: 'GridController2',
           
        })
    .state('Taula_db', {
        url: '/Employee',
        data: { seguro: true },
        templateUrl: 'js/Taula-db/taula-db.html',
        controller: 'TaulaController'
       
    })

    .state('todo', {
        url: '/todo',
        data: { seguro: true },
        templateUrl: 'js/Todo/todo.html',

    })
    .state('Customers', {
        url: '/Customers',
        data: { seguro: true },
        templateUrl: 'js/Data-db/data-customers.cshtml',

    })

     .state('logout', {
         url: '/logout',
         views: {
             'primary': {
                 controller: 'logoutCtrl',
                 data: { seguro: true }
             }
         }
     })
     .state('empleats', {
         url: '/empleats',
         data: { seguro: true },
         templateUrl: 'js/Empleats/empleats.html',
         controller: 'EmpleatController',

     })

});

routerApp.run(['$rootScope', '$trace', '$transitions', '$state', 'Authentication',
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
           {
               to: function (state) {
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


   }]);