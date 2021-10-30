app.controller('menuBuilderCtrl',
function ($scope, $http, $rootScope, $localStorage) {

    //var menuresult = $('#hidMenu').val();
    //var models = (typeof menuresult) == 'string' ? eval('(' + menuresult + ')') : menuresult;
    $scope.link = $localStorage.link;
    //$scope.getmenu = function (link) {

    //    var mnu;
    //    if (link.submenu) {
    //        mnu = $sce.trustAsHtml(link.text + "<span class='fa arrow'/>");

    //    } else {
    //        mnu = $sce.trustAsHtml(link.text);
    //    }
    //    return mnu;
    //}
    //$scope.SetMenu = function (link) {
    //    $('.fa.arrow').each(function () {
    //        var self = $(this);
    //        $(self).removeClass("fa");
    //        $(self).removeClass("arrow");
    //        $(self).addClass("fa");
    //        $(self).addClass("arrow");
    //    });

    //}

  
    });

