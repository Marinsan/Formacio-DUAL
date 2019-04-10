// Controlador Missatges snackbars crud

routerApp.controller('SnackbarErrorController', function ($scope, $mdToast, $mdDialog) {
    $scope.showCustomToast = function () {
        $mdToast.show({
            hideDelay: 3000,
            position: 'top right',
            templateUrl: 'js/Snackbars/snackbar-error.html'
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