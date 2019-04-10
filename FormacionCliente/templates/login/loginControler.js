(function () {
    'use strict';

    angular.module('routerApp')

    .controller('LoginControler', ['$rootScope', '$scope', '$state', 'Authentication',
                      function ($rootScope, $scope, $state, Authentication) { 

    $rootScope.pantallaDesc = "Login";

    $scope.Model = {};

  

    $scope.logIn = function (form) {

        if (form.$valid) {
            
            Authentication.logIn($scope.Model)
                .success(function () {

                    
                        $rootScope.user = Authentication.getCurrentUser();
                        $rootScope.sede = Authentication.getCurrentUserSede();
                        $rootScope.auxSedeDescripcion = " - ";
                        //$rootScope.user.user_sedeId
                        //$rootScope.user.sede = $scope.sede;

                        $state.go("home", {}, { reload: true });
                        
                }).error(function(error){
                    console.error("El usuario o la contraseña son incorrectos.");
                });
       }
    }
                 
                              
}])         
  
    .controller('LogoutControler', ['$scope', 'Authentication', '$state', 
                          function ($scope, Authentication, $state) {
                              Authentication.logOut();
                              $state.go('login')
                          }])

    .run(function ($state, $rootScope) {
        $rootScope.$state = $state;
    })

})();

 //   