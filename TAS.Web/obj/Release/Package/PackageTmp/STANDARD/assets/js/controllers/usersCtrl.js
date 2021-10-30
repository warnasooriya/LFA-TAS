app.controller('UsersCtrl',
    function ($compile, $scope, $rootScope, $http, $location, DTOptionsBuilder, DTColumnBuilder, DTInstances) {
        var vm = this;
        vm.dtOptions = DTOptionsBuilder.newOptions()
            .withOption('ajax', {
                url: 'api/User/GetAllUsers',
                type: 'POST',
               // contentType: "application/json"
            })
         .withDataProp('data')
            .withOption('processing', true)
            .withOption('serverSide', true)
            .withOption('createdRow', createdRow)
            .withPaginationType('full_numbers');

        vm.dtColumns = [
            DTColumnBuilder.newColumn('Id').withTitle('ID'),
            DTColumnBuilder.newColumn('FirstName').withTitle('First name'),
            DTColumnBuilder.newColumn('LastName').withTitle('Last name'),
            DTColumnBuilder.newColumn('LastName').withTitle('Last name'),
            DTColumnBuilder.newColumn('Id').withTitle('Actions').notSortable()
        .renderWith(function (data, type, full, meta) {
            var html = '<button class="btn btn-warning" ng-click="edit(\'' + data + '\');">' +
                          '<i class="fa fa-edit"></i>' +
                '</button>&nbsp;&nbsp;' +

                '<button class="btn btn-danger" ng-click="delete(\'' + data + '\');">' +
                                  '<i class="fa fa-trash-o"></i>' +
                        '</button>';
            return html;


        })
        ];
        function createdRow(row, data, dataIndex) {
            $compile(angular.element(row).contents())($scope);
        }
    $scope.showPopup = function () {
        var dialog = ngDialog.open({
            template: 'popUpUserMgmt',
            className: 'ngdialog-theme-default ',
            scope: $rootScope
        });
        $rootScope.systemUser = {
            firstName: '',
            lastName:'',
            dateOfBirth: '',
            userName: '',
            password: ''
        };

        $rootScope.ModalName = "Add New User";
        $rootScope.action = "Save";
       
        $rootScope.confirm = function () {
            // var data = JSON.stringify({ 'systemUser': $rootScope.systemUser });
            var request = $http({
                method: "post",
                url: "/api/User/AddUser",
                data: $rootScope.systemUser,
            });
            request.success(function () {
                dialog.close();
                ServerSideProcessingCtrl();
                
            });
        }
        
    };
    $scope.edit = function (id) {
        $rootScope.ModalName = "Update User";
        $rootScope.action = "Update";
        ngDialog.open({
            template: 'popUpUserMgmt',
            className: 'ngdialog-theme-default '
        });
        $rootScope.confirm = function () {
            alert('a');
        }
    };
    $scope.delete = function (id) {
        alert(id);
    };
    $rootScope.resetPopup = function () {
        $rootScope.firstName,
        $rootScope.lastName,
        $rootScope.dateOfBirth,
        $rootScope.userName,
        $rootScope.password = "";
    };
    $rootScope.functionThatReturnsPromise = function () {
        return $timeout(angular.noop, 1000);
    }
   //$scope.ServerSideProcessingCtrl = function () {
        
   // }
});


//function ServerSideProcessingCtrl($compile, $scope, DTOptionsBuilder, DTColumnBuilder, DTInstances) {
//    var vm = this;
//    vm.dtOptions = DTOptionsBuilder.newOptions()
//        .withOption('ajax', {
//            url: 'api/User/GetAllUsers',
//            type: 'GET'
//        })
//     .withDataProp('data')
//        .withOption('processing', true)
//        .withOption('serverSide', true)
//        .withOption('createdRow', createdRow)
//        .withPaginationType('full_numbers');

//    vm.dtColumns = [
//        DTColumnBuilder.newColumn('Id').withTitle('ID'),
//        DTColumnBuilder.newColumn('FirstName').withTitle('First name'),
//        DTColumnBuilder.newColumn('LastName').withTitle('Last name'),
//        DTColumnBuilder.newColumn('LastName').withTitle('Last name'),
//        DTColumnBuilder.newColumn('Id').withTitle('Actions').notSortable()
//    .renderWith(function (data, type, full, meta) {
//        var html = '<button class="btn btn-warning" ng-click="edit(\'' + data + '\');">' +
//                      '<i class="fa fa-edit"></i>' +
//            '</button>&nbsp;&nbsp;' +

//            '<button class="btn btn-danger" ng-click="delete(\'' + data + '\');">' +
//                              '<i class="fa fa-trash-o"></i>' +
//                    '</button>';
//        return html;


//    })
//    ];
//    function createdRow(row, data, dataIndex) {
//        $compile(angular.element(row).contents())($scope);
//    }
//}