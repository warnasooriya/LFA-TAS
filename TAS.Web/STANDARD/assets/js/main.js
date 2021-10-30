var app = angular.module('clipApp', ['clip-two']);
app.run(['$rootScope', '$state', '$stateParams', 'Idle', '$localStorage',
    function ($rootScope, $state, $stateParams, Idle, $localStorage) {
        var viewNotificationPopup;
        // Attach Fastclick for eliminating the 300ms delay between a physical tap and the firing of a click event on mobile browsers
        FastClick.attach(document.body);
        // Set some reference to access them from any scope
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;
        // GLOBAL APP SCOPE
        // set below basic information

        $rootScope.app = {
            name: 'TAS', // name of your project
            author: 'LFA', // author's name or company name
            description: 'Total Administration System', // brief description
            version: '1.0', // current version
            tpaLogoSrc: $localStorage.TpaLogoSrc,
            year: ((new Date()).getFullYear()), // automatic current year (for copyright information)
            isMobile: (function () {// true if the browser is a mobile device
                var check = false;
                if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                    check = true;
                };
                return check;
            })(),
            layout: {
                isNavbarFixed: true, //true if you want to initialize the template with fixed header
                isSidebarFixed: true, // true if you want to initialize the template with fixed sidebar
                isSidebarClosed: false, // true if you want to initialize the template with closed sidebar
                isFooterFixed: false, // true if you want to initialize the template with fixed footer
                theme: 'theme-6', // indicate the theme chosen for your project
                //logo: 'assets/images/Login_logo.png', // relative path of the project logo
                logo: 'assets/images/logo.png',

            }
        };
        // alert($rootScope.app.layout.tpaLogoSrc);

        $rootScope.user = {
            name: $localStorage.currentUsername,
            job: 'ng-Dev',
            picture: $localStorage.currentProfilePic
        };
        $rootScope.tpaName = $localStorage.tpaName;
        $rootScope.notifications = {};
        Idle.watch();
        /*signalr config */
        $rootScope.initialzeSignalR = function () {
            try {


                    $.connection.hub.start()
                        .done(function () { console.log('Now connected, connection ID=' + $.connection.hub.id); })
                        .fail(function () { console.log('Could not Connect!'); });

            }
            catch (err) {
                console.log('Error occured -' + err);
            }
        }

        $rootScope.initialzeSignalR();

        $rootScope.tasNotificationHub = $.connection.tasNotificationHub;

        $rootScope.tasNotificationHub.client.realtimeNotifications = function (data) {
            $localStorage.notifications = JSON.parse(data);
            $rootScope.notifications = $localStorage.notifications;
            $rootScope.isNoNotifications = $rootScope.notifications.messages.length == 0;
        };

        $rootScope.checkNotifications = function () {
            $rootScope.notifications.IsDataAvailable = false;
            var data = { hubId: $.connection.hub.id }
            $rootScope.tasNotificationHub.server.userCheckNotifications(data);
        }

        $rootScope.checkIfNotificationsAvailable = function () {
            var data = { hubId: $.connection.hub.id }
            $rootScope.tasNotificationHub.server.getShortNotificationsByHostId(data);
        }

        $rootScope.subscribeToSignalrHub = function () {
            if (isGuid($localStorage.LoggedInUserId) &&
                isGuid($localStorage.tpaID) && isGuid($.connection.hub.id)) {
                var data = {
                    loggedInUserId: $localStorage.LoggedInUserId,
                    tpaId: $localStorage.tpaID,
                    hubId: $.connection.hub.id
                }

                $rootScope.tasNotificationHub.server.subscribeToHub(data);
                $rootScope.checkIfNotificationsAvailable();
            }
        }


        $rootScope.numberWithCommas = function (x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }
    }]);

// translate config
app.config(['$translateProvider',
    function ($translateProvider) {

        // prefix and suffix information  is required to specify a pattern
        // You can simply use the static-files loader with this pattern:
        $translateProvider.useStaticFilesLoader({
            prefix: 'assets/i18n/',
            suffix: '.json'
        });

        // Since you've now registered more then one translation table, angular-translate has to know which one to use.
        // This is where preferredLanguage(langKey) comes in.
        $translateProvider.preferredLanguage('en');

        // Store the language in the local storage
        $translateProvider.useLocalStorage();

    }]);

app.config(['cfpLoadingBarProvider',
    function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeBar = true;
        cfpLoadingBarProvider.includeSpinner = false;
    }]);



//ng-Idle Configurations
app.config(['KeepaliveProvider', 'IdleProvider', function (KeepaliveProvider, IdleProvider) {
    IdleProvider.idle(3600);
    IdleProvider.timeout(3600);
    KeepaliveProvider.interval(300);
}]);

app.factory('errorInterceptor', ['$q', '$rootScope', '$location', '$localStorage',
    function ($q, $rootScope, $location, $localStorage) {
        return {
            request: function (config) {
                return config || $q.when(config);
            },
            requestError: function (request) {
                // window.location.href = '/TAS.Web/STANDARD/index.html#/login/signin/' + $localStorage.tpaID
                //+ '?redirectto=' + $location.url();
                return $q.reject(request);
                // alert("reqEr");
            },
            response: function (response) {
                return response || $q.when(response);
            },
            responseError: function (response) {
                // alert($location.absUrl());
                if (response.status === 403) {
                    if ($location.path() == '/app/tastpa') {
                        window.location.href = '/TAS.Web/STANDARD/index.html#/login/tas'
                    }
                    else {
                        window.location.href = '/TAS.Web/STANDARD/index.html#/login/signin/' + $localStorage.tpaName           // $localStorage.tpaID
                            + '?redirectto=' + $location.url();
                    }
                    // alert('here');
                } else if (response.status === 500) {
                    //  window.location.href = '/TAS.Web/STANDARD/index.html#/login/signin/' + $localStorage.tpaID
                    // + '?redirectto=' + $location.url();
                    //alert('500here');

                }
                //return $q.reject(response);
            }
        };
    }]);

app.factory('httpRequestInterceptor', ['$location', '$localStorage',
    function ($location, $localStorage) {
        return {
            request: function (config) {

                config.headers['RequestPage'] = $location.path();
                config.headers['RequestUserId'] = $localStorage.LoggedInUserId;

                return config;
            }
        };
    }]);

app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push('errorInterceptor');
    $httpProvider.interceptors.push('httpRequestInterceptor');
}]);

app.directive('allowPattern', [allowPatternDirective]);

app.directive('allowDecimal', [allowDecimalDirective]);

app.config(['$compileProvider',
    function ($compileProvider) {

        $compileProvider.imgSrcSanitizationWhitelist(/^\s*(local|http|https|app|tel|ftp|file|blob|content|ms-appx|x-wmapp0|cdvfile):|data:image\//);

    }]);
function allowPatternDirective() {
    return {
        restrict: "A",
        compile: function (tElement, tAttrs) {
            return function (scope, element, attrs) {
                // I handle key events
                element.bind("keypress", function (event) {
                    var keyCode = event.which || event.keyCode; // I safely get the keyCode pressed from the event.

                    if (keyCode == 37 || keyCode == 38 || keyCode == 39 || keyCode == 40 || keyCode == 8 || keyCode == 46 || keyCode == 9) { // Left / Up / Right / Down Arrow, Backspace, Delete,tab keyCodes
                        return;
                    }

                    var keyCodeChar = String.fromCharCode(keyCode); // I determine the char from the keyCode.

                    // If the keyCode char does not match the allowed Regex Pattern, then don't allow the input into the field.
                    if (!keyCodeChar.match(new RegExp(attrs.allowPattern, "i"))) {
                        event.preventDefault();
                        return false;
                    }

                });


            };
        }
    };
}


function allowDecimalDirective() {
    return {
        restrict: "A",
        compile: function (tElement, tAttrs) {
            return function (scope, element, attrs) {
                element.bind("focusout", function (event) {
                    if (!parseFloat(element.val())) {
                        element.val(parseFloat(0.00).toFixed(attrs.allowDecimal))
                    } else {
                        element.val(parseFloat(element.val()).toFixed(attrs.allowDecimal));
                    }
                });
            };
        }
    };
}

function GetToday() {
    var m_names = new Array("Jan", "Feb", "Mar",
        "Apr", "May", "Jun", "Jul", "Aug", "Sep",
        "Oct", "Nov", "Dec");

    var d = new Date();
    var curr_date = d.getDate()+'';
    var curr_month = d.getMonth();
    var curr_year = d.getFullYear();
    // document.write(curr_date + "-" + m_names[curr_month]
    //  + "-" + curr_year);
    return curr_date.padStart(2, '0') + "-" + m_names[curr_month] + "-" + curr_year;

}

Number.prototype.round = function (p) {
    p = p || 10;
    return parseFloat(this.toFixed(p));
};

app.directive('datepickerLocaldate', ['$parse', function ($parse) {
    var directive = {
        restrict: 'A',
        require: ['ngModel'],
        link: link
    };
    return directive;

    function link(scope, element, attr, ctrls) {
        var ngModelController = ctrls[0];

        // called with a JavaScript Date object when picked from the datepicker
        ngModelController.$parsers.push(function (viewValue) {
            // alert('x');
            // undo the timezone adjustment we did during the formatting
            viewValue.setMinutes(viewValue.getMinutes() - viewValue.getTimezoneOffset());
            // we just want a local date in ISO format
            return viewValue.toISOString().substring(0, 10);

        });

        // called with a 'yyyy-mm-dd' string to format
        //ngModelController.$formatters.push(function (modelValue) {
        //  alert('y');
        // if (!modelValue) {
        //      return "";
        //  }
        // date constructor will apply timezone deviations from UTC (i.e. if locale is behind UTC 'dt' will be one day behind)
        //  var dt = new Date(modelValue);
        // 'undo' the timezone offset again (so we end up on the original date again)
        //   dt.setMinutes(dt.getMinutes() + dt.getTimezoneOffset());
        //   return dt.toISOString().substring(0, 10);
        //});
    }
}]);

app.directive("limitToMax", function () {
    return {
        link: function (scope, element, attributes) {
            element.on("keydown keyup", function (e) {
                if (Number(element.val()) > Number(attributes.max) &&
                    e.keyCode != 46 // delete
                    &&
                    e.keyCode != 8 // backspace
                ) {
                    e.preventDefault();
                    element.val(attributes.max);
                }
            });
        }
    };
});

app.directive("preventTypingGreater", function () {
    return {
        link: function (scope, element, attributes) {
            var oldVal = null;
            element.on("keydown keyup", function (e) {
                if (Number(element.val()) > Number(attributes.max) &&
                    e.keyCode != 46 // delete
                    &&
                    e.keyCode != 8 // backspace
                ) {
                    e.preventDefault();
                    element.val(oldVal);
                } else {
                    oldVal = Number(element.val());
                }
            });
        }
    }
});

app.directive('clearOnClick', ['$window', function ($window) {
    return function (scope, element, attrs) {
        element.bind('click', function ($event) {
            $event.target.select();
            //if (!$window.getSelection().toString()) {
            //    this.ogValue = this.value;
            //    if (this.value == 0) {
            //        this.value = '';
            //    }
            //}
        });

        //element.on('blur', function ($event) {
            //if (this.value == '' && this.ogValue == '') {
            //    this.value = '';
            //} else if (this.value == 0) {
            //    this.value = 0;
            //}
        //});
    };
}]);

app.directive("focusNextInput", function () {
    return {
        restrict: "A",
        link: function ($scope, element) {
            element.on("input", function (e) {
                if (element.val().length === parseInt(element.attr("val-to-jump"))) {
                    var $nextElement = element.next();

                    if ($nextElement.length) {
                        $nextElement[0].focus();
                    }
                }
            });
        }
    };
});

app.directive('capitalize', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            var capitalize = function (inputValue) {
                if (inputValue == undefined) inputValue = '';
                var capitalized = inputValue.toUpperCase();
                if (capitalized !== inputValue) {
                    // see where the cursor is before the update so that we can set it back
                    var selection = element[0].selectionStart;
                    modelCtrl.$setViewValue(capitalized);
                    modelCtrl.$render();
                    // set back the cursor after rendering
                    element[0].selectionStart = selection;
                    element[0].selectionEnd = selection;
                }
                return capitalized;
            }
            modelCtrl.$parsers.push(capitalize);
            capitalize(scope[attrs.ngModel]); // capitalize initial value
        }
    };
});

app.filter('cut', function () {
    return function (value, wordwise, max, tail) {
        if (!value) return '';

        max = parseInt(max, 10);
        if (!max) return value;
        if (value.length <= max) return value;

        value = value.substr(0, max);
        if (wordwise) {
            var lastspace = value.lastIndexOf(' ');
            if (lastspace !== -1) {
                //Also remove . and , so its gives a cleaner result.
                if (value.charAt(lastspace - 1) === '.' || value.charAt(lastspace - 1) === ',') {
                    lastspace = lastspace - 1;
                }
                value = value.substr(0, lastspace);
            }
        }

        return value + (tail || ' …');
    };
});

function isGuid(stringToTest) {
    var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
    var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
    return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
}


