app.controller('DashboardCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, toaster, $filter, $translate) {
        $scope.warrentyChartData = [];
        $scope.productChartData = [];
        $scope.claimStatusChartData = [];
        $scope.claimInvoiceChartData = [];
        $scope.makeChartData = {
            labels: [''],
            datasets: [
                {
                    data: [0]
                }]
        };
        $scope.monthChatsData = {
            labels: [''],
            datasets: [{
                data: [0]
            }]
        };
        //supportive functions
        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        };
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        };
        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };
        var customInfoMessage = function (msg) {
            toaster.pop('info', 'Information', msg, 12000);
        };

        var geoChart = {};
        geoChart.type = "GeoChart";
        geoChart.data = [
            ['Locale', 'Policy Count']
        ];

        geoChart.options = {

            chartArea: { left: 10, top: 10, bottom: 0, height: "100%" },
            colorAxis: { colors: ['#aec7e8', '#1f77b4'] },
            displayMode: 'regions'
        };
        //  $scope.localchart = geoChart;

        function getGreetingsBySystemTime() {
            var d = new Date(); // for now
            var hour = d.getHours();
            if (hour < 12) {
                return $filter('translate')('dashboard.greetings.morning');
            } else if (hour < 18) {
                return $filter('translate')('dashboard.greetings.afternoon');;
            } else {
                return $filter('translate')('dashboard.greetings.evening');;
            }
        }
        $scope.dashbordwelcomeName = "IMPETUS";
        $scope.greetings = getGreetingsBySystemTime();
        $scope.colorChart = ["#F7464A", "#46BFBD", "#FDB45C", "#FB0D65", "#413BFE", "#4BD7CD", "#FE7236", "#83FE64", "#F464FE", "#E5FE64", "#989B88", "#FE960D", "#F60DFE"];
        $scope.userValidityCheck = function () {
            var userId = $localStorage.LoggedInUserId;
            if (isGuid(userId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/dashboard/RetrivePolicySectionDataForDashboard',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: {
                        "loggedInUserId": $localStorage.LoggedInUserId
                    }
                }).success(function (data, status, headers, config) {
                    if (data.status === 'ok') {
                        for (var i = 0; i < data.data.length; i++) {
                            if (data.data[i].chartCode === 'ByWarrenty') {
                                if (data.data[i].status === 'ok') {
                                    for (var k = 0; k < data.data[i].data.length; k++) {
                                        data.data[i].data[k].color = $scope.colorChart[k];
                                    }
                                    $scope.warrentyChartData = data.data[i].data;
                                    $scope.warrentyChartAccess = false;

                                } else {
                                    $scope.warrentyChartAccess = true;

                                }
                            } else if (data.data[i].chartCode === "ByProduct") {
                                if (data.data[i].status === 'ok') {
                                    for (var l = 0; l < data.data[i].data.length; l++) {
                                        data.data[i].data[l].color = $scope.colorChart[l];
                                    }
                                    $scope.productChartData = data.data[i].data;
                                    $scope.productChartAccess = false;
                                } else {
                                    $scope.productChartAccess = true;
                                }
                            } else if (data.data[i].chartCode === "ByMake") {
                                if (data.data[i].status === 'ok') {
                                    $scope.makeChartData = data.data[i].data;
                                    $scope.makeChartOptions = {

                                        // Sets the chart to be responsive
                                        responsive: true,

                                        //Boolean - Whether to show lines for each scale point
                                        scaleShowLine: true,

                                        //Boolean - Whether we show the angle lines out of the radar
                                        angleShowLineOut: true,

                                        //Boolean - Whether to show labels on the scale
                                        scaleShowLabels: false,

                                        // Boolean - Whether the scale should begin at zero
                                        scaleBeginAtZero: true,

                                        //String - Colour of the angle line
                                        angleLineColor: 'rgba(0,0,0,.1)',

                                        //Number - Pixel width of the angle line
                                        angleLineWidth: 1,

                                        //String - Point label font declaration
                                        pointLabelFontFamily: '"Arial"',

                                        //String - Point label font weight
                                        pointLabelFontStyle: 'normal',

                                        //Number - Point label font size in pixels
                                        pointLabelFontSize: 10,

                                        //String - Point label font colour
                                        pointLabelFontColor: '#666',

                                        //Boolean - Whether to show a dot for each point
                                        pointDot: true,

                                        //Number - Radius of each point dot in pixels
                                        pointDotRadius: 3,

                                        //Number - Pixel width of point dot stroke
                                        pointDotStrokeWidth: 1,

                                        //Number - amount extra to add to the radius to cater for hit detection outside the drawn point
                                        pointHitDetectionRadius: 20,

                                        //Boolean - Whether to show a stroke for datasets
                                        datasetStroke: true,

                                        //Number - Pixel width of dataset stroke
                                        datasetStrokeWidth: 2,

                                        //Boolean - Whether to fill the dataset with a colour
                                        datasetFill: true,

                                        //String - A legend template
                                        legendTemplate: '<ul class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].strokeColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>'
                                    };
                                    $scope.makeChartAccess = false;

                                } else {
                                    $scope.makeChartAccess = true;
                                }
                            } else if (data.data[i].chartCode === "ByCountry") {
                                if (data.data[i].status === 'ok') {
                                    for (var j = 0; j < data.data[i].data.length; j++) {
                                        var x = [data.data[i].data[j].data[0].Key, data.data[i].data[j].data[0].Value];
                                        geoChart.data.push(x);
                                    }
                                    $scope.localchart = geoChart;
                                    $scope.countryChartAccess = false;

                                } else {
                                    $scope.countryChartAccess = true;
                                }
                            } else if (data.data[i].chartCode === "ByMonth") {
                                if (data.data[i].status === 'ok') {
                                    $scope.monthChatsData = data.data[i].data;
                                    $scope.monthChartOptions = {

                                        // Sets the chart to be responsive
                                        responsive: true,

                                        //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
                                        scaleBeginAtZero: true,

                                        //Boolean - Whether grid lines are shown across the chart
                                        scaleShowGridLines: true,

                                        //String - Colour of the grid lines
                                        scaleGridLineColor: "rgba(0,0,0,.05)",

                                        //Number - Width of the grid lines
                                        scaleGridLineWidth: 1,

                                        //Boolean - If there is a stroke on each bar
                                        barShowStroke: true,

                                        //Number - Pixel width of the bar stroke
                                        barStrokeWidth: 2,

                                        //Number - Spacing between each of the X value sets
                                        barValueSpacing: 5,

                                        //Number - Spacing between data sets within X values
                                        barDatasetSpacing: 1,

                                        //String - A legend template
                                        legendTemplate: '<ul class="tc-chart-js-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>'
                                    };
                                    $scope.monthlyChartAccess = false;


                                } else {
                                    $scope.monthlyChartAccess = true;
                                }
                            }
                        }
                    } else {
                        $scope.warrentyChartAccess = true;
                        $scope.productChartAccess = true;
                        $scope.makeChartAccess = true;
                        $scope.countryChartAccess = true;
                        $scope.monthlyChartAccess = true;
                        customErrorMessage(data.status);
                    }
                }).error(function (data, status, headers, config) {
                });


                //$http({
                //    method: 'POST',
                //    url: '/TAS.Web/api/ClaimInvoice/RetriveInvoiceEntryDataForDashboard',
                //    data: { "ClaimSubmittedDealerId": $scope.ClaimInvoiceEntry.DealerId },
                //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                //}).success(function (data, status, headers, config) {
                //    if (data == null) {
                //        claimInvoiceChartAccess = false;
                //        customErrorMessage("No claims found.");
                //    }
                //    var l = 0;
                //    angular.forEach(data.data, function (value) {

                //        value.highlight = "";
                //        value.color = $scope.colorChart[l];
                //        $scope.claimInvoiceChartData[l] = value;
                //        l++;
                //    });
                //    claimInvoiceChartAccess = true;

                //});

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/GetAllClaimDashboardStatus',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data == null) {
                        claimStatusChartAccess = false;
                        customErrorMessage("No claims found.");
                    }
                    var l = 0;
                    angular.forEach(data.data, function (value) {

                        value.highlight = "";
                        value.color = $scope.colorChart[l];
                        $scope.claimStatusChartData[l] = value;
                        l++;
                    });
                    claimStatusChartAccess = true;
                });




            } else {
                swal("TAS Information", "You're not authorized to access this page.", "error");
                setTimeout(function () { swal.close(); }, 5000);
                $state.go('login.signin', { 'tpaId': $localStorage.tpaName });
            }

        }

        $scope.loadClaimData = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetAllClaimDashboardStatus',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data == null) {
                    claimStatusChartAccess = false;
                    customErrorMessage("No claims found.");
                }
                var l = 0;
                angular.forEach(data.data, function (value) {

                    value.highlight = "";
                    value.color = $scope.colorChart[l];
                    $scope.claimStatusChartData[l] = value;
                    l++;
                });
                claimStatusChartAccess = true;
            });
            $scope.claimStatusChartOptions = {

                // Sets the chart to be responsive
                responsive: true,

                //Boolean - Whether we should show a stroke on each segment
                segmentShowStroke: true,

                //String - The colour of each segment stroke
                segmentStrokeColor: '#fff',

                //Number - The width of each segment stroke
                segmentStrokeWidth: 2,

                //Number - The percentage of the chart that we cut out of the middle
                percentageInnerCutout: 0, // This is 0 for Pie charts

                //Number - Amount of animation steps
                animationSteps: 100,

                //String - Animation easing effect
                animationEasing: 'easeOutBounce',

                //Boolean - Whether we animate the rotation of the Doughnut
                animateRotate: true,

                //Boolean - Whether we animate scaling the Doughnut from the centre
                animateScale: false,
                //tc-chart-js-legend
                //String - A legend template
                legendTemplate: '<ul class="list-inline"><% for (var i=0; i<segments.length; i++){%><li><span style="background-color:<%=segments[i].fillColor%>">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;<%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>'

            };
        }

        $scope.warrentyChartOptions = {

            // Sets the chart to be responsive
            responsive: true,

            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,

            //String - The colour of each segment stroke
            segmentStrokeColor: '#fff',

            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,

            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 0, // This is 0 for Pie charts

            //Number - Amount of animation steps
            animationSteps: 100,

            //String - Animation easing effect
            animationEasing: 'easeOutBounce',

            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,

            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,
            //tc-chart-js-legend
            //String - A legend template
            legendTemplate: '<ul class="list-inline"><% for (var i=0; i<segments.length; i++){%><li><span style="background-color:<%=segments[i].fillColor%>">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;<%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>'

        };
        $scope.claimStatusChartOptions = {

            // Sets the chart to be responsive
            responsive: true,

            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,

            //String - The colour of each segment stroke
            segmentStrokeColor: '#fff',

            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,

            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 0, // This is 0 for Pie charts

            //Number - Amount of animation steps
            animationSteps: 100,

            //String - Animation easing effect
            animationEasing: 'easeOutBounce',

            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,

            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,
            //tc-chart-js-legend
            //String - A legend template
            legendTemplate: '<ul class="list-inline"><% for (var i=0; i<segments.length; i++){%><li><span style="background-color:<%=segments[i].fillColor%>">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;<%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>'

        };
        $scope.claimInvoiceChartOptions = {

            // Sets the chart to be responsive
            responsive: true,

            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,

            //String - The colour of each segment stroke
            segmentStrokeColor: '#fff',

            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,

            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 0, // This is 0 for Pie charts

            //Number - Amount of animation steps
            animationSteps: 100,

            //String - Animation easing effect
            animationEasing: 'easeOutBounce',

            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,

            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,
            //tc-chart-js-legend
            //String - A legend template
            legendTemplate: '<ul class="list-inline"><% for (var i=0; i<segments.length; i++){%><li><span style="background-color:<%=segments[i].fillColor%>">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;<%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>'

        };
        $scope.productChartoptions = {

            // Sets the chart to be responsive
            responsive: true,

            //Boolean - Whether we should show a stroke on each segment
            segmentShowStroke: true,

            //String - The colour of each segment stroke
            segmentStrokeColor: '#fff',

            //Number - The width of each segment stroke
            segmentStrokeWidth: 2,

            //Number - The percentage of the chart that we cut out of the middle
            percentageInnerCutout: 50, // This is 0 for Pie charts

            //Number - Amount of animation steps
            animationSteps: 100,

            //String - Animation easing effect
            animationEasing: 'easeOutBounce',

            //Boolean - Whether we animate the rotation of the Doughnut
            animateRotate: true,

            //Boolean - Whether we animate scaling the Doughnut from the centre
            animateScale: false,

            //String - A legend template
            legendTemplate: '<ul class="list-inline"><% for (var i=0; i<segments.length; i++){%><li><span style="background-color:<%=segments[i].fillColor%>">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>&nbsp;<%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>'

        };

    });

