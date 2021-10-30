'use strict';
/**
 * Clip-Two Main Controller
 */
app.controller('AppCtrl', ['$rootScope', '$scope', '$state', '$translate', '$localStorage', '$window', '$document', '$timeout', 'cfpLoadingBar', '$cookieStore', '$http', '$location', '$modal', '$filter',
    function ($rootScope, $scope, $state, $translate, $localStorage, $window, $document, $timeout, cfpLoadingBar, $cookieStore, $http, $location, $modal, $filter) {
        $scope.menuTypeId = $localStorage.menuType;

        $rootScope.setMenuCookie = function (isLanguageSet = false) {
            //$scope.link = $cookieStore.get('Menu');
            let menu = $localStorage.Menu;
            if (isLanguageSet) {
                angular.forEach(menu, function (value, key) {
                    var filterKey = 'menu.' + value.MenuCode;
                    value.MenuName = $filter('translate')(filterKey);
                    if (value.Lastlevel == false) {
                        angular.forEach(value.submenu, function (subvalue, key) {
                            var subfilterKey = 'menu.' + subvalue.MenuCode;
                            subvalue.MenuName = $filter('translate')(subfilterKey);
                        });
                    }
                });
            }
            $scope.link = menu;
        }
        $rootScope.setMenuCookie();
        $rootScope.validateAuthCookie = function () {
            var tasCookie = $cookieStore.get('TASToken');
            $http({
                method: 'POST',
                url: '/TAS.Web/api/User/CookieLogin',
                data: { "cookieToken": tasCookie },
                async: false
            }).success(function (response) {

                if (response == "Invalid") {
                    throw "error";
                } else if (response == "Error") {
                    throw "error";
                } else {
                    $localStorage.jwt = response.JsonWebToken;
                    $rootScope.LoggedInUserId = response.LoggedInUserId;
                    $rootScope.UserType = response.UserType;

                    $localStorage.LoggedInUserId = response.LoggedInUserId;
                    $localStorage.UserType = response.UserType;


                    $localStorage.menuType = 2;
                    $localStorage.tpaID = $scope.tpaID;

                    $cookieStore.put('jwt', response.JsonWebToken);
                    $cookieStore.put('LoggedInUserId', response.LoggedInUserId);
                    $cookieStore.put('UserType', response.UserType);

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/User/GetMenu',
                        headers: { 'Authorization': $localStorage.jwt },
                        async: false
                    }).success(function (data, status, headers, config) {
                        $localStorage.Menu = data;
                        $rootScope.setMenuCookie();
                    }).error(function (data, status, headers, config) {
                        throw "error";
                    });


                    if ($rootScope.UserType == "IU") {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/User/GetUsersById',
                            data: { "Id": $rootScope.LoggedInUserId },
                            headers: { 'Authorization': $localStorage.jwt }
                        }).success(function (data, status, headers, config) {

                            $location.path("app/dashboard");
                        }).error(function (data, status, headers, config) {
                            throw "error";
                        });
                    }
                    else {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Customer/GetCustomerById',
                            data: { "Id": $rootScope.LoggedInUserId },
                            headers: { 'Authorization': $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $localStorage.user = data;
                            $location.path("app/dashboard");
                        }).error(function (data, status, headers, config) {
                            throw "error";
                        });
                    }

                    ///////////////////SignarR Registration//////////////////////////

                    var data = {
                        loggedInUserId: response.LoggedInUserId,
                        tpaId: $scope.tpaID,
                        hubId: $.connection.hub.id
                    }

                    $rootScope.tasNotificationHub.server.subscribeToHub(data);
                    var data = {
                        hubId: $.connection.hub.id
                    }
                    $rootScope.tasNotificationHub.server.getShortNotificationsByHostId(data);

                    ///////////////////End SignarR Registration//////////////////////////
                }
                return false;
            }).error(function (data, status, headers, config) {
                $cookieStore.remove('TASToken');
                throw "error";
            });
        }


        // Loading bar transition
        // -----------------------------------
        var $win = $($window);

        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
            //start loading bar on stateChangeStart
            cfpLoadingBar.start();
            //added by ranga on cookie setting
            if (toState.name == 'login.signin') {
                var tasCookie = $cookieStore.get('TASToken');
                if (tasCookie) {
                    try {

                        $rootScope.validateAuthCookie();
                        event.preventDefault();
                    } catch (e) {
                        // event.con
                    }

                }
            } else {
                $location.url($location.path())
            }
        });
        $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {

            //stop loading bar on stateChangeSuccess
            event.targetScope.$watch("$viewContentLoaded", function () {
                $rootScope.LoggedInUserId = $localStorage.LoggedInUserId;
                $rootScope.UserType = $localStorage.UserType;
                if ($localStorage.user != undefined) {
                    $rootScope.user = $localStorage.user;
                    $rootScope.user.name = $rootScope.user.FirstName + " " + $rootScope.user.LastName;
                }
                //signalr
                if (typeof $.connection.hub.id === 'undefined') {
                    $rootScope.initialzeSignalR();
                    $rootScope.subscribeToSignalrHub();
                } else {
                    $rootScope.subscribeToSignalrHub();
                }
                cfpLoadingBar.complete();
            });

            // scroll top the page on change state

            $document.scrollTo(0, 0);

            if (angular.element('.email-reader').length) {
                angular.element('.email-reader').animate({
                    scrollTop: 0
                }, 0);
            }

            // Save the route title
            $rootScope.previousState = fromState.name;
            $rootScope.currTitle = $state.current.title;

        });

        // State not found
        $rootScope.$on('$stateNotFound', function (event, unfoundState, fromState, fromParams) {
            //$rootScope.loading = false;
            console.log(unfoundState.to);
            // "lazy.state"
            console.log(unfoundState.toParams);
            // {a:1, b:2}
            console.log(unfoundState.options);
            // {inherit:false} + default options
        });

        $rootScope.pageTitle = function () {
            return $rootScope.app.name + ' - ' + ($rootScope.currTitle || $rootScope.app.description);

        };

        // save settings to local storage
        if (angular.isDefined($localStorage.layout)) {
            $scope.app.layout = $localStorage.layout;

        } else {
            $localStorage.layout = $scope.app.layout;
        }
        $scope.$watch('app.layout', function () {
            // save to local storage
            $localStorage.layout = $scope.app.layout;
        }, true);

        //global function to scroll page up
        $scope.toTheTop = function () {

            $document.scrollTopAnimated(0, 600);

        };

        // angular translate
        // ----------------------

        $scope.language = {
            // Handles language dropdown
            listIsOpen: false,
            // list of available languages
            available: {
                'en': 'English',
                'it_IT': 'Italiano',
                'de_DE': 'Deutsch'
            },
            // display always the current ui language
            init: function () {
                var proposedLanguage = $translate.proposedLanguage() || $translate.use();
                var preferredLanguage = $translate.preferredLanguage();
                // we know we have set a preferred one in app.config
                $scope.language.selected = $scope.language.available[(proposedLanguage || preferredLanguage)];
            },
            set: function (localeId, ev) {
                $translate.use(localeId);
                $scope.language.selected = $scope.language.available[localeId];
                $scope.language.listIsOpen = !$scope.language.listIsOpen;
            }
        };

        $scope.language.init();

        // Function that find the exact height and width of the viewport in a cross-browser way
        var viewport = function () {
            var e = window, a = 'inner';
            if (!('innerWidth' in window)) {
                a = 'client';
                e = document.documentElement || document.body;
            }
            return {
                width: e[a + 'Width'],
                height: e[a + 'Height']
            };
        };
        // function that adds information in a scope of the height and width of the page
        $scope.getWindowDimensions = function () {
            return {
                'h': viewport().height,
                'w': viewport().width
            };
        };
        // Detect when window is resized and set some variables
        $scope.$watch($scope.getWindowDimensions, function (newValue, oldValue) {
            $scope.windowHeight = newValue.h;
            $scope.windowWidth = newValue.w;
            if (newValue.w >= 992) {
                $scope.isLargeDevice = true;
            } else {
                $scope.isLargeDevice = false;
            }
            if (newValue.w < 992) {
                $scope.isSmallDevice = true;
            } else {
                $scope.isSmallDevice = false;
            }
            if (newValue.w <= 768) {
                $scope.isMobileDevice = true;
            } else {
                $scope.isMobileDevice = false;
            }
        }, true);
        // Apply on resize
        $win.on('resize', function () {
            $scope.$apply();
        });

        $scope.ToGB = function (date) {
            //alert(date);
            var dateOnly = date.split('T')[0];
            var year = dateOnly.split('-')[0];
            var date = dateOnly.split('-')[2];
            var month = getMMMbyMM(dateOnly.split('-')[1]);

            return date + '-' + month + '-' + year;

        }

        var getMMMbyMM = function (monthNumber) {
            var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
                "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            return monthNames[parseInt((monthNumber) - 1)]
        }

        var jwt = $cookieStore.get('jwt');
        var tasjwt = $cookieStore.get('tasjwt');

        $scope.signOut = function () {
            $localStorage.menuType = 0;
            $localStorage.CommodityType = "";
            if ($localStorage.tpaID != "" && $localStorage.jwt != "") {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/User/LoginOut',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $rootScope.LoggedInUserId }
                }).success(function (data, status, headers, config) {
                    $cookieStore.remove('TASToken');
                    $localStorage.jwt = "";
                    $rootScope.LoggedInUserId = "";
                    if (data !== "error") {
                        if ($localStorage.tpaName == "perkinsdemo" || $localStorage.tpaName == "Perkinsdemo" || $localStorage.tpaName == "PERKINSDEMO") {
                            window.location.href = '/TAS.Webtest/STANDARD/index.html#/login/signin/' + data;
                        } else {
                            window.location.href = '/TAS.Web/STANDARD/index.html#/login/signin/' + data;
                        }

                    } else {
                        if ($localStorage.tpaName == "perkinsdemo" || $localStorage.tpaName == "Perkinsdemo" || $localStorage.tpaName == "PERKINSDEMO") {
                            window.location.href = '/TAS.Webtest/STANDARD/index.html#/login/signin/' + $localStorage.tpaName;
                        } else {
                            window.location.href = '/TAS.Web/STANDARD/index.html#/login/signin/' + $localStorage.tpaName;
                        }

                    }

                }).error(function (data, status, headers, config) {
                    $cookieStore.remove('TASToken');
                    $localStorage.jwt = "";
                    $rootScope.LoggedInUserId = "";
                    window.location.href = '/TAS.Web/STANDARD/index.html#/login/signin/' + $localStorage.tpaName;

                });

            }
            else {

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/User/TASLoginOut',
                    headers: { 'Authorization': $localStorage.tasjwt == undefined ? tasjwt : $localStorage.tasjwt },
                    data: { "Id": $rootScope.LoggedInUserId }
                }).success(function (data, status, headers, config) {
                    //  if (data == "OK") {
                    $localStorage.tasjwt = "";
                    window.location.href = '/TAS.Web/STANDARD/index.html#/login/tas';
                    // }

                }).error(function (data, status, headers, config) {
                    $localStorage.tasjwt = "";
                    window.location.href = '/TAS.Web/STANDARD/index.html#/login/tas';
                });

            }
            return false;
        }

        $scope.started = false;

        function closeModals() {
            if ($scope.warning) {
                $scope.warning.close();
                $scope.warning = null;
            }

            if ($scope.timedout) {
                $scope.timedout.close();
                $scope.timedout = null;
            }
        }

        $scope.$on('IdleStart', function () {
            closeModals();

            $scope.warning = $modal.open({
                templateUrl: 'warning-dialog.html',
                windowClass: 'modal-danger'
            });
        });

        $scope.$on('IdleEnd', function () {
            closeModals();
        });

        $scope.$on('IdleTimeout', function () {
            closeModals();
            $scope.timedout = $modal.open({
                templateUrl: 'timedout-dialog.html',
                windowClass: 'modal-danger',
            });
            if (!$state.includes('login.signin') && !$state.includes('login.tas')
                && !$state.includes('login.forgot') && !$state.includes('login.changepasword')
                && !$state.includes('home.products') && !$state.includes('home.homePremium')
                && !$state.includes('home.buyingprocess') && !$state.includes('home.login')) {
                $scope.signOut();
            }


        });


        $scope.$on('Keepalive', function () {
            var JWT = null;
            if ($location.path() == '/app/tastpa') {
                JWT = $localStorage.tasjwt != undefined ? $localStorage.tasjwt : tasjwt;
            } else {
                JWT = $localStorage.jwt != undefined ? $localStorage.jwt : jwt;
            }

            if (!$state.includes('login.signin') && !$state.includes('login.tas')
                && !$state.includes('login.forgot') && !$state.includes('login.changepasword')
                && !$state.includes('home.products') && !$state.includes('home.homePremium')
                && !$state.includes('home.buyingprocess') && !$state.includes('home.login')) {
                if (($localStorage.jwt != undefined || $localStorage.tasjwt != undefined || jwt.length > 0 || tasjwt.length > 0) && $location.path() != '/home/products') {

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/User/KeepAlivePing',
                        headers: { 'Authorization': JWT }
                    }).success(function (data, status, headers, config) {
                        //  if (data == "OK") {

                        // }

                    }).error(function (data, status, headers, config) {

                    });

                }
            }
        });

        $scope.start = function () {
            closeModals();
            Idle.watch();
            $scope.started = true;
        };

        $scope.stop = function () {
            closeModals();
            Idle.unwatch();
            $scope.started = false;

        };

        $scope.validateCharacter = function (val, isNumericOnlyAllow) {
            if (isNumericOnlyAllow == undefined || isNumericOnlyAllow == null || isNumericOnlyAllow == false) {
                return (val != "") ? ((val.match(/[a-z]/i)) ? true : false) : false;
            }
            else {
                return (val != "") ? ((val.match(/[0-9] || [a-z]/i)) ? true : false) : false;
            }
        }
        $scope.notificationsInAllView = [];
        $scope.currentNotificationPage = 1;
        $scope.totalNotifications = 0;
        $scope.notificationPageSize = 10;
        $scope.isNextPageAvailable = false;
        $scope.isPrevPageAvailable = false;
        $scope.isNotificationLoading = false;
        $scope.startPage = 0;
        $scope.endPage = 0;
        $scope.refresStartandEndPages = function () {
            $scope.startPage = ($scope.notificationPageSize * ($scope.currentNotificationPage - 1)) + 1;
            if ($scope.startPage + $scope.notificationPageSize - 1 < $scope.totalNotifications) {
                $scope.endPage = ($scope.currentNotificationPage * $scope.notificationPageSize);

            } else {
                $scope.endPage = ($scope.notificationPageSize * ($scope.currentNotificationPage - 1)) + $scope.notificationPageSize -
                    (($scope.currentNotificationPage * $scope.notificationPageSize) - $scope.totalNotifications);

            }
            if ($scope.endPage < $scope.totalNotifications)
                $scope.isNextPageAvailable = true;
            else
                $scope.isNextPageAvailable = false;

            if ($scope.currentNotificationPage > 1)
                $scope.isPrevPageAvailable = true;
            else
                $scope.isPrevPageAvailable = false;


        }
        $scope.getNextNotificationPage = function () {
            $scope.currentNotificationPage++;
            $scope.getAllNotifications();
        }

        $scope.getPrevNotificationPage = function () {
            $scope.currentNotificationPage--;
            $scope.getAllNotifications();
        }

        $scope.getAllNotifications = function () {
            var userId = $rootScope.LoggedInUserId;
            var tpaId = $localStorage.tpaID;
            var pageSize = $scope.notificationPageSize;
            var page = $scope.currentNotificationPage;
            $scope.notificationsInAllView = [];
            $scope.isNotificationLoading = true;
            $http({
                method: 'POST',
                url: 'http://leftfield.xyz/TASNotification/TasNotification/GetPagedNotifications',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: {
                    "userId": userId,
                    "tpaId": tpaId,
                    "pageSize": pageSize,
                    "page": page
                }
            }).success(function (data, status, headers, config) {
                if (data != null) {
                    $scope.totalNotifications = data.TotalRecords;
                    $scope.notificationsInAllView = data.messages;
                    $scope.refresStartandEndPages();
                }
            }).error(function (data, status, headers, config) {


            }).finally(function () {
                $scope.isNotificationLoading = false;
            });

        }
        $scope.msgPopUpodel;
        $scope.viewAllNotifications = function () {
            $scope.notificationsInAllView = [];
            $scope.currentNotificationPage = 1;
            $scope.totalNotifications = 0;
            $scope.notificationPageSize = 10;
            $scope.isNextPageAvailable = false;
            $scope.isPrevPageAvailable = false;
            $scope.isNotificationLoading = false;
            $scope.getAllNotifications();

            $scope.msgPopUpodel = $modal.open({
                templateUrl: 'msgViewPopup.html',
                scope: $scope,
                appendTo: angular.element(document).find('aside')
            });
        }
        $scope.navigateToNotification = function (link) {
            if (link !== "#") {
                window.location.href = link;
                $scope.msgPopUpodel.close();
            }
        }



    }]);
