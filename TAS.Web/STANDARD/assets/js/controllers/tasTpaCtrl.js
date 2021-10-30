app.controller('TASTpaCtrl', 
    function ($scope, $rootScope, $http, FileUploader, $rootScope, $location, SweetAlert, $modal, $localStorage, $cookieStore) {

         $scope.tpa = {
            Id: "",
            Name: "",
            TelNumber: "",
            Address: "",
            Banner: "",
            Logo: "",
            DiscountDescription: "",
            
        }
      
        $scope.savedtpaId = "";
        $scope.publicLink = "";
        $scope.signInLink = "";
      
        $scope.resetTpa = function () {
            $scope.tpa = {
                Id: "",
                Name: "",
                TelNumber: "",
                Address: "",
                Banner: "",
                Logo: "",
                DiscountDescription: "",
            }
           
            $scope.publicLink = "";
            $scope.signInLink = "";

            $('input[type=file]').val('');//no angular implementation
        }
        $scope.loadExistingTPAs = function () {
            var jwt = $cookieStore.get('jwt');
            $http({
                method: 'POST',
                url: '/TAS.Web/api/TASTPA/GetAllTPAs',
                headers: { 'Authorization':  $localStorage.tasjwt == undefined ? jwt : $localStorage.tasjwt}
            }).success(function (data, status, headers, config) {
                $scope.tpas = data;
            }).error(function (data, status, headers, config) {
            });

        };
        
        $scope.SetTpa = function () {
            var jwt = $cookieStore.get('jwt');
            if ($scope.tpa.Id != "" && typeof ($scope.tpa.Id) !== "undefined" && $scope.tpa.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/TASTPA/GetTPADetailsById',
                    data: { "tpaId": $scope.tpa.Id },
                    headers: { 'Authorization': $localStorage.tasjwt == undefined ? jwt : $localStorage.tasjwt },
                    async:false,
                }).success(function (data, status, headers, config) {
                    $scope.tpa = data[0];
                    $scope.tpa.bannerByte = "";
                    $scope.tpa.logoByte = "";
                    //$scope.showBanner();
                    //$scope.showLogo();
                    $scope.publicLink = "https://www.leftfield.xyz/TAS.Web/STANDARD/index.html#/home/products/" + $scope.tpa.Name;
                    $scope.signInLink = "https://www.leftfield.xyz/TAS.Web/STANDARD/index.html#/login/signin/" + $scope.tpa.Name;
                }).error(function (data, status, headers, config) {
                });
            } else {
                $scope.resetTpa();
                $scope.publicLink = "";
            }
        }
        $scope.saveOrUpdateTpa = function () {
            var jwt = $cookieStore.get('jwt');
            if ($scope.tpa.Name != "") {
                if (uploader1.queue.length > 0) {
                    uploader1.queue[0].upload();

                } else if (uploader2.queue.length > 0) {
                    uploader2.queue[0].upload();
                } else {
                    if ($scope.tpa.Id != "") {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/TASTPA/UpdateTPA',
                            headers: { 'Authorization': $localStorage.tasjwt == undefined ? jwt : $localStorage.tasjwt },
                            data: $scope.tpa
                        }).success(function (data, status, headers, config) {
                            $scope.resetTpa();
                            $scope.loadExistingTPAs();
                            SweetAlert.swal({
                                title: "TAS Information",
                                text: "Successfully Updated",
                                confirmButtonColor: "#007AFF"
                            });
                        }).error(function (data, status, headers, config) {

                        });
                    } else {
                        $scope.savedtpaId = $scope.tpa.Id;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/TASTPA/SaveTPA',
                            headers: { 'Authorization': $localStorage.tasjwt == undefined ? jwt : $localStorage.tasjwt },
                            data: $scope.tpa
                        }).success(function (data, status, headers, config) {
                            //$scope.resetTpa();
                            $scope.loadExistingTPAs();
                            $scope.tpa.Id = $scope.savedtpaId;
                            $scope.SetTpa();
                            if (data == true) {
                                SweetAlert.swal({
                                    title: "TAS Information",
                                    text: "Successfully Saved! Get your URLs below",
                                    confirmButtonColor: "#007AFF"
                                });
                            } else {
                                SweetAlert.swal({
                                    title: "TAS Information",
                                    text: "Save Failed!",
                                    type: "warning",
                                    confirmButtonColor: "rgb(221, 107, 85)"
                                });
                            }
                        }).error(function (data, status, headers, config) {

                        });
                    }
                }

            } else {
                SweetAlert.swal({
                    title: "TAS Information",
                    text: "Please Enter Required Details.",
                    type: "warning",
                    confirmButtonColor: "rgb(221, 107, 85)"
                });
                
            }
        }
        $scope.showBanner = function () {
            var jwt = $cookieStore.get('jwt');
            if ($scope.tpa.Banner != "00000000-0000-0000-0000-000000000000") {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Image/GetImageById',
                    headers: { 'Authorization': $localStorage.tasjwt == undefined ? jwt : $localStorage.tasjwt },
                    data: { 'ImageId': $scope.tpa.Banner },
                }).success(function (data, status, headers, config) {
                    $scope.tpa.bannerByte = data;
                }).error(function (data, status, headers, config) {
                });
            }
            
        }
        $scope.showLogo = function () {
            var jwt = $cookieStore.get('jwt');
            if ($scope.tpa.Logo != "00000000-0000-0000-0000-000000000000") {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Image/GetImageById',
                    headers: { 'Authorization': $localStorage.tasjwt == undefined ? jwt : $localStorage.tasjwt },
                    data: { 'ImageId': $scope.tpa.Logo },
                }).success(function (data, status, headers, config) {
                    $scope.tpa.logoByte = data;
                }).error(function (data, status, headers, config) {
                });
            }
        }
        $scope.isShowing = function (value) {

            if (value== '00000000-0000-0000-0000-000000000000' || value=='' || value==null)
                return false;
            else
                return true;

        };

        var uploader1 = $scope.uploader1 = new FileUploader({
            url: '/TAS.Web/api/Image/UploadImage'
        });
        var uploader2 = $scope.uploader2 = new FileUploader({
            url: '/TAS.Web/api/Image/UploadImage'
        });

        uploader1.filters.push({
            name: 'extensionFilter',
            fn: function (item, options) {
                var filename = item.name;
                var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
                if (extension == "png" || extension == "jpg" || extension == "bmp" || extension == "gif")
                    return true;
                else {
                    alert('Invalid file format. Please select a file with jpg/png/bmp or gif format  and try again.');
                    return false;
                }
            }
        });

        uploader1.filters.push({
            name: 'sizeFilter',
            fn: function (item, options) {
                var fileSize = item.size;
                fileSize = parseInt(fileSize) / (1024 * 1024);
                if (fileSize <= 5)
                    return true;
                else {
                    alert('Selected file exceeds the 5MB file size limit. Please choose a new file and try again.');
                    return false;
                }
            }
        });

        uploader1.filters.push({
            name: 'itemResetFilter',
            fn: function (item, options) {
                if (this.queue.length < 5)
                    return true;
                else {
                    alert('You have exceeded the limit of uploading files.');
                    return false;
                }
            }
        });

        // CALLBACKS

        uploader1.onWhenAddingFileFailed = function (item, filter, options) {
            console.info('onWhenAddingFileFailed', item, filter, options);
        };
        uploader1.onAfterAddingFile = function (fileItem) {
            // alert('Files ready for upload.');
        };

        uploader1.onSuccessItem = function (fileItem, response, status, headers) {
            var jwt = $cookieStore.get('jwt');
            $scope.uploader1.queue = [];
            $scope.uploader1.progress = 0;
            $scope.tpa.Banner = response.replace(/\"/g, "");
            if (uploader2.queue.length > 0) {
                uploader2.queue[0].upload();
                //alert('Selected file has been uploaded successfully.');
            } else {
                if ($scope.tpa.Id != "") {
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/TASTPA/UpdateTPA',
                        headers: { 'Authorization': $localStorage.tasjwt == undefined ? jwt : $localStorage.tasjwt },
                        data: $scope.tpa
                    }).success(function (data, status, headers, config) {
                        $scope.resetTpa();
                        $scope.loadExistingTPAs();
                    }).error(function (data, status, headers, config) {

                    });
                } else {
                    $scope.savedtpaId = $scope.tpa.Id;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/TASTPA/SaveTPA',
                        headers: { 'Authorization': $localStorage.tasjwt == undefined ? jwt : $localStorage.tasjwt },
                        data: $scope.tpa
                    }).success(function (data, status, headers, config) {
                        //$scope.resetTpa();
                        $scope.loadExistingTPAs();
                        $scope.tpa.Id = $scope.savedtpaId;
                        $scope.SetTpa();
                    }).error(function (data, status, headers, config) {

                    });
                }
            }

        };
        uploader1.onErrorItem = function (fileItem, response, status, headers) {
            alert('We were unable to upload your file. Please try again.');
        };
        uploader1.onCancelItem = function (fileItem, response, status, headers) {
            // alert('File uploading has been cancelled.');
        };

        uploader1.onAfterAddingAll = function (addedFileItems) {
            console.info('onAfterAddingAll', addedFileItems);
        };
        uploader1.onBeforeUploadItem = function (item) {
            console.info('onBeforeUploadItem', item);
        };
        uploader1.onProgressItem = function (fileItem, progress) {
            console.info('onProgressItem', fileItem, progress);
        };
        uploader1.onProgressAll = function (progress) {
            console.info('onProgressAll', progress);
        };

        uploader1.onCompleteItem = function (fileItem, response, status, headers) {
            console.info('onCompleteItem', fileItem, response, status, headers);
        };
        uploader1.onCompleteAll = function () {
            console.info('onCompleteAll');
        };

        // console.info('uploader', uploader);
        uploader2.filters.push({
            name: 'extensionFilter',
            fn: function (item, options) {
                var filename = item.name;
                var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
                if (extension == "png" || extension == "jpg" || extension == "bmp" || extension == "gif")
                    return true;
                else {
                    alert('Invalid file format. Please select a file with jpg/png/bmp or gif format  and try again.');
                    return false;
                }
            }
        });

        uploader2.filters.push({
            name: 'sizeFilter',
            fn: function (item, options) {
                var fileSize = item.size;
                fileSize = parseInt(fileSize) / (1024 * 1024);
                if (fileSize <= 5)
                    return true;
                else {
                    alert('Selected file exceeds the 5MB file size limit. Please choose a new file and try again.');
                    return false;
                }
            }
        });

        uploader2.filters.push({
            name: 'itemResetFilter',
            fn: function (item, options) {
                if (this.queue.length < 5)
                    return true;
                else {
                    alert('You have exceeded the limit of uploading files.');
                    return false;
                }
            }
        });

        // CALLBACKS

        uploader2.onWhenAddingFileFailed = function (item, filter, options) {
            console.info('onWhenAddingFileFailed', item, filter, options);
        };
        uploader2.onAfterAddingFile = function (fileItem) {
            // alert('Files ready for upload.');
        };

        uploader2.onSuccessItem = function (fileItem, response, status, headers) {
            var jwt = $cookieStore.get('jwt');
            $scope.uploader2.queue = [];
            $scope.uploader2.progress = 0;
            //alert('Selected file has been uploaded successfully.');
            $scope.tpa.Logo = response.replace(/\"/g, "");
            if ($scope.tpa.Id != "") {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/TASTPA/UpdateTPA',
                    headers: { 'Authorization': $localStorage.tasjwt == undefined ? jwt : $localStorage.tasjwt },
                    data: $scope.tpa
                }).success(function (data, status, headers, config) {
                    $scope.resetTpa();
                    $scope.loadExistingTPAs();
                }).error(function (data, status, headers, config) {

                });
            } else {
                $scope.savedtpaId = $scope.tpa.Id;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/TASTPA/SaveTPA',
                    headers: { 'Authorization': $localStorage.tasjwt == undefined ? jwt : $localStorage.tasjwt },
                    data: $scope.tpa
                }).success(function (data, status, headers, config) {
                    //$scope.resetTpa();
                    $scope.loadExistingTPAs();
                    $scope.tpa.Id = $scope.savedtpaId;
                    $scope.SetTpa();
                }).error(function (data, status, headers, config) {

                });
            }

        };
        uploader2.onErrorItem = function (fileItem, response, status, headers) {
            alert('We were unable to upload your file. Please try again.');
        };
        uploader2.onCancelItem = function (fileItem, response, status, headers) {
            // alert('File uploading has been cancelled.');
        };

        uploader2.onAfterAddingAll = function (addedFileItems) {
            console.info('onAfterAddingAll', addedFileItems);
        };
        uploader2.onBeforeUploadItem = function (item) {
            console.info('onBeforeUploadItem', item);
        };
        uploader2.onProgressItem = function (fileItem, progress) {
            console.info('onProgressItem', fileItem, progress);
        };
        uploader2.onProgressAll = function (progress) {
            console.info('onProgressAll', progress);
        };

        uploader2.onCompleteItem = function (fileItem, response, status, headers) {
            console.info('onCompleteItem', fileItem, response, status, headers);
        };
        uploader2.onCompleteAll = function () {
            console.info('onCompleteAll');
        };

    });