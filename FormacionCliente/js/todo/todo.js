(function () {
    var Task = (function () {
        var counter = 0;
        return function (content, isDone) {
            this.id = counter++;
            this.content = content;
            this.isDone = isDone || false;
        }
    }());
    var app = angular.module('routerApp')
    .service('todosBL', function () {
        this.tasks = [];
        this.add = function (content) {
            this.tasks.push(new Task(content));
        }

        this.update = function (editedTask) {
            var i = this.tasks.findIndex(function (task) {
                return task.id === editedTask.id
            });
            this.tasks[i] = editedTask;
        }

    })
    .controller('todoController', function ($scope, todosBL) {
        $scope.todosBL = todosBL;
        $scope.add = function (task) {
            todosBL.add(task);
            $scope.task = null;
        }
        $scope.editMode = {
            task: {},
            edit: function (task) {
                this.task = task;
            },
            save: function (task) {
                todosBL.update(task);
                this.task = {};
            }
        };
    })

    .controller('showCtrl', function ($scope) {
        $scope.editar = false;
        $scope.toggle = function () {
            $scope.editar = !$scope.editar;
        };
    });
}());