// Variable + Controllador per a la validacio del formulari partial-form.html

var validationApp = angular.module('validationApp', []);


validationApp.controller('mainController', function ($scope) {

    $scope.submitForm = function (isValid) {

        if (isValid) {
            alert('our form is amazing');
        }

    };

});

// Controlador Formulari Material simple partial-form-material.html

routerApp.controller('FormController', function ($scope) {
    $scope.user = {
        name: 'Cristian Marin',
        email: '',
        phone: ''
    };
});

// Configuracio per als missatges arial-label deshabilitar

routerApp.config(['$mdAriaProvider', function ($mdAriaProvider) {
    $mdAriaProvider.disableWarnings();
}]);


// Controlador Missatge al premer boto Enviar al formulari

routerApp.controller('MessageController', function ($scope, $mdToast, $mdDialog) {
    $scope.showCustomToast = function () {
        $mdToast.show({
            hideDelay: 3000,
            position: 'top right',
            templateUrl: 'templates/toast-template.html'
        });
    };
    $scope.closeToast = function () {
        if (routerApp) return;

        $mdToast
          .hide()
          .then(function () {
              dialogApp = false;
          });
    };

});

// Controlador Imatge Carta contact

routerApp.controller('CardController', function ($scope) {
    $scope.imagePath = 'img/logo.jpg';
});
