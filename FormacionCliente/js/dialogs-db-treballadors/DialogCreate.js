angular.module('routerApp')
    
.config(['$mdAriaProvider', function ($mdAriaProvider) {
    $mdAriaProvider.disableWarnings();
}])

.controller('CreateEmployeeDialogController', function ($scope, $mdDialog, $interval) {

    $scope.submitForm = function (isValid) {

        if (isValid) {
            alert('our form is amazing');
        }

    };

    $scope.status = '  ';
    $scope.customFullscreen = true;

    $scope.showAdvanced_create_taula = function (Emp, ev) {
        $mdDialog.show({
            controller: CreateDialogController,
            templateUrl: 'js/Todo/dialogs/dialog-create.html',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: false,
            fullscreen: $scope.customFullscreen
        })
    };

    function CreateDialogController($scope, $mdDialog) {
        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.answer = function (answer) {
            $mdDialog.hide(answer);
        };

    }
});