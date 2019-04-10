var app = angular.module('routerApp');
app.controller("TaulaController", function ($scope, $http, $interval) {

    $scope.InsertData = function () {
        var Action = document.getElementById("btnSave").getAttribute("value");
        if (Action == "Submit") {
            $scope.Employe = {};
            $scope.Employe.Emp_Name = $scope.EmpName;
            $scope.Employe.Emp_City = $scope.EmpCity;
            $scope.Employe.Emp_Age = $scope.EmpAge;
            $http({
                method: "post",
                url: "http://localhost:52188/Employee/Insert_Employee",
                datatype: "json",
                data: JSON.stringify($scope.Employe)
            }).then(function (response) {
                alert(response.data);
                $scope.GetAllData();
                $scope.EmpName = "";
                $scope.EmpCity = "";
                $scope.EmpAge = "";
            })
        } else {
            $scope.Employe = {};
            $scope.Employe.Emp_Name = $scope.EmpName;
            $scope.Employe.Emp_City = $scope.EmpCity;
            $scope.Employe.Emp_Age = $scope.EmpAge;
            $scope.Employe.Emp_Id = document.getElementById("EmpID_").value;
            $http({
                method: "post",
                url: "http://localhost:52188/Employee/Update_Employee",
                datatype: "json",
                data: JSON.stringify($scope.Employe)
            }).then(function (response) {
                alert(response.data);
                $scope.GetAllData();
                $scope.EmpName = "";
                $scope.EmpCity = "";
                $scope.EmpAge = "";
                document.getElementById("btnSave").setAttribute("value", "Submit");
                document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
                document.getElementById("spn").innerHTML = "Add New Employee";
            })
        }
    }

    $scope.GetAllData = function () {
        
        $http({
            method: "get",
            url: "http://localhost:52188/Employee/Get_AllEmployee"

        }).then(function (response) {
            $scope.employees = response.data;
            var self = this;

            self.activated = true;
            self.determinateValue = 30;

            $interval(function () {

                self.determinateValue += 1;
                if (self.determinateValue > 100) {
                    self.determinateValue = 30;
                }

            }, 100);
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



    
    //This variable is used to store the original values.
    $scope.EditItem = {};

    //Editing an existing record.
    $scope.EditEmp = function (Emp) {
        //Setting EditMode to TRUE makes the TextBoxes visible for the row.
        $scope.Employe[Emp].EditMode = true;

        //The original values are saved in the variable to handle Cancel case.
        $scope.EditItem.EmpName = $scope.Employe[Emp].EmpName;
        $scope.EditItem.EmpCity = $scope.Employe[Emp].EmpCity;
        $scope.EditItem.EmpAge = $scope.Employe[Emp].EmpAge;
    };

    //Cancelling an Edit.
    $scope.CancelEmp = function (Emp) {
        // The original values are restored back into the Customers Array.
        $scope.Employe[Emp].Name = $scope.EditItem.Name;
        $scope.Employe[Emp].Country = $scope.EditItem.Country;

        //Setting EditMode to FALSE hides the TextBoxes for the row.
        $scope.Employe[Emp].EditMode = false;
        $scope.EditItem = {};
    };

    //Updating an existing record to database.
    $scope.UpdateEmp = function (Emp) {
        var employee = $scope.Employe[Emp];
        var post = $http({
            method: "POST",
            url: "http://localhost:52188/Employee/Delete_Employee",
            data: '{employee:' + JSON.stringify(employee) + '}',
            dataType: 'json',
        });
        post.success(function (data, status) {
            //Setting EditMode to FALSE hides the TextBoxes for the row.
            employee.EditMode = false;
        });
    };
})