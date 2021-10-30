app.controller('InternalHomeCtrl',
    function ($scope, $rootScope, $http, $location) {
        $scope.loadInitData = function () {
            if ($rootScope.AuthToken == null || $rootScope.AuthToken == "") {
                alert('The login session is expired!');
                //window.location.href = window.location.hostpathname + '/index.html#/internal/';
                $location.path('/internal');
            } else {
                document.getElementById("fixedNavBar").style.display = 'block';
                document.getElementById("breadcrumb").style.display = 'block';
                document.getElementById("fixedFooter").style.display = 'block';
                document.getElementById("regDropdown").style.display = 'block';
                document.getElementById("btnRegister").style.display = 'none';
                document.getElementById("mainmenu-content").style.marginLeft = "225px";
                document.getElementById("mainmenu").style.display = 'block';
                
            }
        }
    });