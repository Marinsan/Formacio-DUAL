angular.module('routerApp')

.controller('DialogEmployeeDeleteController', function ($scope, $mdDialog, $interval) {

    $scope.showAdvanced_delete_taula = function (ev) {
        $mdDialog.show({
            controller: DialogControllerDeleteEmploye,
            templateUrl: 'templates/dialogs/dialog-delete.html',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: true
        })
    };

    function DialogControllerDeleteEmploye($scope, $mdDialog, $http) {
        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.GetAllData = function () {

            $http({
                method: "get",
                url: "http://localhost:52188/Employee/Get_AllEmployee"

            }).then(function (response) {
                $scope.employees = response.data;
               
            }, function ($scope, $mdToast, $mdDialog) {
                $scope.showCustomToast = function () {
                    $mdToast.show({
                        hideDelay: 3000,
                        position: 'top right',
                        templateUrl: 'templates/snackbar/snackbar-error.html'
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
            })
        };

        $scope.DeleteEmp = function (Emp) {
            $http({
                method: "post",
                url: "http://localhost:52188/Employee/Delete_Employee",
                datatype: "json",
                data: JSON.stringify(Emp)
            }).then(function (response) {
                alert(response.data);
                $scope.GetAllData();
            })
        };
    }
});