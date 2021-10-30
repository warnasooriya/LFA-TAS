app.controller('BulkPolicyUploadCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, ngDialog, FileUploader) {
        $scope.ModalName = "Bulk Policy Upload";
        $scope.ModalDescription = "";
        document.getElementById("loading").style.display = "none";
        $scope.errorTab1 = "";
        $scope.UploadData = {
            CommodityType: "test ", 
            Status: "test ", 
            ProductName: "test ", 
            ProductCode: "test ", 
            DealerName: "test ", 
            DealerCountry: "test ", 
            DealerCurrency: "test ", 
            DealerBranch: "test ", 
            DealerBranchCity: "test ", 
            DealerBranchServiceContractPerson: "test ", 
            DealerBranchServiceTelephoneNo: "test ", 
            DealerBranchServiceFax: "test ", 
            DealerBranchServiceEmail: "test ", 
            DealerBranchSalesContractPerson: "test ", 
            DealerBranchSalesTelephoneNo: "test ", 
            DealerBranchSalesFax: "test ", 
            DealerBranchSalesEmail: "test ", 
            InsurerName: "test ", 
            InsurerCode: "test ", 
            InsurerCountry: "test ", 
            VehicleVinNo: "test ", 
            Manufacturer: "test ", 
            ManufacturerCode: "test ", 
            ManufacturerWaranty: "test ", 
            ManufacturerWarantyMeasureType: "test ", 
            ManufacturerWarantyCountry: "test ", 
            ManufacturerWarantyMonth: "test ", 
            ManufacturerWarantyStartDate: "test ", 
            ManufacturerWarantyKm: "test ", 
            MakeName: "test ", 
            MakeCode: "test ", 
            ModelName: "test ", 
            ModelCode: "test ", 
            ModelRiskStartDate: "test ", 
            CategoryName: "test ", 
            CategoryLength: "test ", 
            VehiclelCountryOfOrigine: "test ", 
            VehicleCylinderCount: "test ", 
            VehicleBodyType: "test ", 
            VehiclePlateNo: "test ", 
            VehicleModelYear: "test ", 
            VehicleFuelType: "test ", 
            VehicleAspiration: "test ", 
            VehicleVariant: "test ", 
            VehicleTransmission: "test ", 
            VehicleTransmissionTechnology: "test ", 
            PurchaseDate: "test ", 
            VehicleRegistrationDate: "test ", 
            VehicleEngineCapacity: "test ", 
            VehicleDriveType: "test ", 
            VehicleUsageType: "test ", 
            CommodityUsageType: "test ", 
            ElectronicCountryOfOrigine: "test ", 
            ElectronicSerialNo: "test ", 
            ElectronicModelYear: "test ", 
            ElectronicInvoiceNo: "test ", 
            ItemPrice: "test ", 
            DealerPrice: "test ", 
            ElectronicUsageType: "test ", 
            ContractDealName: "test ", 
            ContractDealType: "test ", 
            ReinsurerName: "test ", 
            ReinsurerCode: "test ", 
            PromotionalDeal: "test ", 
            AutoRenewal: "test ", 
            DiscountAvailable: "test ", 
            DealStartDate: "test ", 
            DealEndDate: "test ", 
            PremiumTotal: "test ", 
            ClaimLimitation: "test ", 
            LiabilityLimitation: "test ", 
            ExtensionName: "test ", 
            ExtensionKm: "test ", 
            ExtensionMonth: "test ", 
            ExtensionHours: "test ", 
            PremiumBasedOnDescription: "test ", 
            PremiumBasedonCode: "test ", 
            BasicPremium: "test ", 
            OtherPremium: "test ", 
            AdditionalPremium: "test ", 
            SportsPremium: "test ", 
            FourByFourPremium: "test ", 
            MinimumPremium: "test ", 
            MaximumPremium: "test ", 
            RSAProviderName: "test ", 
            Region: "test ", 
            RateperAnum: "test ", 
            GrossPremium: "test ", 
            PremiumCurrencyType: "test ", 
            CoverType: "test ", 
            SalesPerson: "test ", 
            DealerPaymentCurrencyType: "test ", 
            CustomerPaymentCurrencyType: "test ", 
            PaymentMode: "test ", 
            CustomerFirstName: "test ", 
            CustomerLastName: "test ", 
            Nationality: "test ", 
            DateOfBirth: "test ", 
            CustomerCountry: "test ", 
            CustomerOccupation: "test ", 
            CustomerTitle: "test ", 
            CustomerGender: "test ", 
            CustomerMaritalStatus: "test ", 
            CustomerMobileNo: "test ", 
            CustomerPostalCode: "test ", 
            CustomerIdType: "test ", 
            CustomerIdNo: "test ", 
            CustomerIdIssueDate: "test ", 
            CustomerEmail: "test ", 
            CustomerType: "test ", 
            UsageType: "test ", 
            BusinessName: "test ", 
            BusinessAddress: "test ", 
            BusinessPhoneNo: "test ", 
            CustomerAddress: "test ", 
            CustomerCity: "test ", 
            HrsUsedAtPolicySale: "test ", 
            PolicyNo: "test ", 
            RefNo: "test ", 
            Comment: "test ", 
            Premium: "test ", 
            DealerPayment: "test ", 
            CustomerPayment: "test ", 
            SpecialDeal: "test ", 
            PartialPayment: "test ", 
            PolicySoldDate: "test ", 
            PolicyStartDate: "test ", 
            PolicyEndDate: "test ", 
            Discount: "test ", 
            DealerPolicy: "test ", 
            AutoApproval: false
        };
        $scope.gridOptions = {
            data: 'List',
            paginationPageSizes: [5, 10, 20],
            paginationPageSize: 5,
            enablePaginationControls: true,
            enableRowSelection: true,
            enableCellSelection: false,
            columnDefs: [
                { field: "CommodityType", displayName: "Commodity Type", width: 150},
                { field: "Status", displayName: "Status", width: 150},
                { field: "ProductName", displayName: "Product Name", width: 150},
                { field: "ProductCode", displayName: "Product Code", width: 150},
                { field: "DealerName", displayName: "Dealer Name", width: 150},
                { field: "DealerCountry", displayName: "Dealer Country", width: 150},
                { field: "DealerCurrency", displayName: "Dealer Currency", width: 150},
                { field: "DealerBranch", displayName: "Dealer Branch", width: 150},
                { field: "DealerBranchCity", displayName: "Dealer Branch City", width: 150},
                { field: "DealerBranchServiceContractPerson", displayName: "Dealer Branch Service Contract Person", width: 150},
                { field: "DealerBranchServiceTelephoneNo", displayName: "Dealer Branch Service Telephone No", width: 150},
                { field: "DealerBranchServiceFax", displayName: "Dealer Branch Service Fax", width: 150},
                { field: "DealerBranchServiceEmail", displayName: "Dealer Branch Service Email", width: 150},
                { field: "DealerBranchSalesContractPerson", displayName: "Dealer Branch Sales Contract Person", width: 150},
                { field: "DealerBranchSalesTelephoneNo", displayName: "Dealer Branch Sales Telephone No", width: 150},
                { field: "DealerBranchSalesFax", displayName: "Dealer Branch Sales Fax", width: 150},
                { field: "DealerBranchSalesEmail", displayName: "Dealer Branch Sales Email", width: 150},
                { field: "InsurerName", displayName: "Insurer Name", width: 150},
                { field: "InsurerCode", displayName: "Insurer Code", width: 150},
                { field: "InsurerCountry", displayName: "Insurer Country", width: 150},
                { field: "VehicleVinNo", displayName: "Vehicle Vin No", width: 150},
                { field: "Manufacturer", displayName: "Manufacturer", width: 150},
                { field: "ManufacturerCode", displayName: "Manufacturer Code", width: 150},
                { field: "ManufacturerWaranty", displayName: "Manufacturer Waranty", width: 150},
                { field: "ManufacturerWarantyMeasureType", displayName: "Manufacturer Waranty Measure Type", width: 150},
                { field: "ManufacturerWarantyCountry", displayName: "Manufacturer Waranty Country", width: 150},
                { field: "ManufacturerWarantyMonth", displayName: "Manufacturer Waranty Month", width: 150},
                { field: "ManufacturerWarantyStartDate", displayName: "Manufacturer Waranty Start Date", width: 150},
                { field: "ManufacturerWarantyKm", displayName: "Manufacturer Waranty Km", width: 150},
                { field: "MakeName", displayName: "Make Name", width: 150},
                { field: "MakeCode", displayName: "Make Code", width: 150},
                { field: "ModelName", displayName: "Model Name", width: 150},
                { field: "ModelCode", displayName: "Model Code", width: 150},
                { field: "ModelRiskStartDate", displayName: "Model Risk Start Date", width: 150},
                { field: "CategoryName", displayName: "Category Name", width: 150},
                { field: "CategoryLength", displayName: "Category Length", width: 150},
                { field: "VehiclelCountryOfOrigine", displayName: "Vehiclel Country Of Origine", width: 150},
                { field: "VehicleCylinderCount", displayName: "Vehicle Cylinder Count", width: 150},
                { field: "VehicleBodyType", displayName: "Vehicle Body Type", width: 150},
                { field: "VehiclePlateNo", displayName: "Vehicle Plate No", width: 150},
                { field: "VehicleModelYear", displayName: "Vehicle Model Year", width: 150},
                { field: "VehicleFuelType", displayName: "Vehicle Fuel Type", width: 150},
                { field: "VehicleAspiration", displayName: "Vehicle Aspiration", width: 150},
                { field: "VehicleVariant", displayName: "Vehicle Variant", width: 150},
                { field: "VehicleTransmission", displayName: "Vehicle Transmission", width: 150},
                { field: "VehicleTransmissionTechnology", displayName: "Vehicle Transmission Technology", width: 150},
                { field: "PurchaseDate", displayName: "Purchase Date", width: 150},
                { field: "VehicleRegistrationDate", displayName: "Vehicle Registration Date", width: 150},
                { field: "VehicleEngineCapacity", displayName: "Vehicle Engine Capacity", width: 150},
                { field: "VehicleDriveType", displayName: "Vehicle Drive Type", width: 150},
                { field: "VehicleUsageType", displayName: "Vehicle Usage Type", width: 150},
                { field: "CommodityUsageType", displayName: "Commodity Usage Type", width: 150},
                { field: "ElectronicCountryOfOrigine", displayName: "Electronic Country Of Origine", width: 150},
                { field: "ElectronicSerialNo", displayName: "Electronic Serial No", width: 150},
                { field: "ElectronicModelYear", displayName: "Electronic Model Year", width: 150},
                { field: "ElectronicInvoiceNo", displayName: "Electronic Invoice No", width: 150},
                { field: "ItemPrice", displayName: "Item Price", width: 150},
                { field: "DealerPrice", displayName: "Dealer Price", width: 150},
                { field: "ElectronicUsageType", displayName: "Electronic Usage Type", width: 150},
                { field: "ContractDealName", displayName: "Contract Deal Name", width: 150},
                { field: "ContractDealType", displayName: "Contract Deal Type", width: 150},
                { field: "ReinsurerName", displayName: "Reinsurer Name", width: 150},
                { field: "ReinsurerCode", displayName: "Reinsurer Code", width: 150},
                { field: "PromotionalDeal", displayName: "Promotional Deal", width: 150},
                { field: "AutoRenewal", displayName: "Auto Renewal", width: 150},
                { field: "DiscountAvailable", displayName: "Discount Available", width: 150},
                { field: "DealStartDate", displayName: "Deal Start Date", width: 150},
                { field: "DealEndDate", displayName: "Deal End Date", width: 150},
                { field: "PremiumTotal", displayName: "Premium Total", width: 150},
                { field: "ClaimLimitation", displayName: "Claim Limitation", width: 150},
                { field: "LiabilityLimitation", displayName: "Liability Limitation", width: 150},
                { field: "ExtensionName", displayName: "Extension Name", width: 150},
                { field: "ExtensionKm", displayName: "Extension Km", width: 150},
                { field: "ExtensionMonth", displayName: "Extension Month", width: 150},
                { field: "ExtensionHours", displayName: "Extension Hours", width: 150},
                { field: "PremiumBasedOnDescription", displayName: "Premium Based On Description", width: 150},
                { field: "PremiumBasedonCode", displayName: "Premium Based on Code", width: 150},
                { field: "BasicPremium", displayName: "Basic Premium", width: 150},
                { field: "OtherPremium", displayName: "Other Premium", width: 150},
                { field: "AdditionalPremium", displayName: "Additional Premium", width: 150},
                { field: "SportsPremium", displayName: "Sports Premium", width: 150},
                { field: "FourByFourPremium", displayName: "Four By Four Premium", width: 150},
                { field: "MinimumPremium", displayName: "Minimum Premium", width: 150},
                { field: "MaximumPremium", displayName: "Maximum Premium", width: 150},
                { field: "RSAProviderName", displayName: "RSAProviderName", width: 150},
                { field: "Region", displayName: "Region", width: 150},
                { field: "RateperAnum", displayName: "Rateper Anum", width: 150},
                { field: "GrossPremium", displayName: "Gross Premium", width: 150},
                { field: "PremiumCurrencyType", displayName: "Premium Currency Type", width: 150},
                { field: "CoverType", displayName: "Cover Type", width: 150},
                { field: "SalesPerson", displayName: "Sales Person", width: 150},
                { field: "DealerPaymentCurrencyType", displayName: "Dealer Payment Currency Type", width: 150},
                { field: "CustomerPaymentCurrencyType", displayName: "Customer Payment Currency Type", width: 150},
                { field: "PaymentMode", displayName: "Payment Mode", width: 150},
                { field: "CustomerFirstName", displayName: "Customer First Name", width: 150},
                { field: "CustomerLastName", displayName: "Customer Last Name", width: 150},
                { field: "Nationality", displayName: "Nationality", width: 150},
                { field: "DateOfBirth", displayName: "Date Of Birth", width: 150},
                { field: "CustomerCountry", displayName: "Customer Country", width: 150},
                { field: "CustomerOccupation", displayName: "Customer Occupation", width: 150},
                { field: "CustomerTitle", displayName: "Customer Title", width: 150},
                { field: "CustomerGender", displayName: "Customer Gender", width: 150},
                { field: "CustomerMaritalStatus", displayName: "Customer Marital Status", width: 150},
                { field: "CustomerMobileNo", displayName: "Customer Mobile No", width: 150},
                { field: "CustomerPostalCode", displayName: "Customer Postal Code", width: 150},
                { field: "CustomerIdType", displayName: "Customer Id Type", width: 150},
                { field: "CustomerIdNo", displayName: "Customer Id No", width: 150},
                { field: "CustomerIdIssueDate", displayName: "Customer Id Issue Date", width: 150},
                { field: "CustomerEmail", displayName: "Customer Email", width: 150},
                { field: "CustomerType", displayName: "Customer Type", width: 150},
                { field: "UsageType", displayName: "Usage Type", width: 150},
                { field: "BusinessName", displayName: "Business Name", width: 150},
                { field: "BusinessAddress", displayName: "Business Address", width: 150},
                { field: "BusinessPhoneNo", displayName: "Business Phone No", width: 150},
                { field: "CustomerAddress", displayName: "Customer Address", width: 150},
                { field: "CustomerCity", displayName: "Customer City", width: 150},
                { field: "HrsUsedAtPolicySale", displayName: "Hrs Used At Policy Sale", width: 150},
                { field: "PolicyNo", displayName: "Policy No", width: 150},
                { field: "RefNo", displayName: "Ref No", width: 150},
                { field: "Comment", displayName: "Comment", width: 150},
                { field: "Premium", displayName: "Premium", width: 150},
                { field: "DealerPayment", displayName: "Dealer Payment", width: 150},
                { field: "CustomerPayment", displayName: "Customer Payment", width: 150},
                { field: "SpecialDeal", displayName: "Special Deal", width: 150},
                { field: "PartialPayment", displayName: "Partial Payment", width: 150},
                { field: "PolicySoldDate", displayName: "Policy Sold Date", width: 150},
                { field: "PolicyStartDate", displayName: "Policy Start Date", width: 150},
                { field: "PolicyEndDate", displayName: "Policy End Date", width: 150},
                { field: "Discount", displayName: "Discount", width: 150},
                { field: "DealerPolicy", displayName: "Dealer Policy", width: 150},
                {
                    field: "AutoApproval",
                    displayName: "Auto Approval",
                    width: 150,
                    visible:false
                }
            ]
        };
        $scope.Path = "";
        $scope.gridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
           // gridApi.selection.on.rowSelectionChanged($scope, SetValues);
        }
        function SetValues()
        {
            $scope.mySelectedRows = $scope.gridApi.selection.getSelectedRows();
            angular.forEach($scope.List, function (value) {
                value.AutoApproval = "No";
            });
            angular.forEach($scope.mySelectedRows, function (value) {
                value.AutoApproval = "Yes";
            });
        }
        function clearBordxControls() {
            $scope.Bordx.Id = "00000000-0000-0000-0000-000000000000";
            $scope.List = [];
        }
        $scope.Upload = function ()
        {
            if (uploader1.queue.length > 0) {
                uploader1.queue[0].upload();
            }
            else {
                $scope.Path = "";
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/PolicyReg/ConvertData',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.List = data;
                }).error(function (data, status, headers, config) {
                });
            }
        }

        $scope.saveBulkUpload = function ()
        {
            alert('here');
            SetValues();
            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/SavePolicyBulkUpload',
                data: {"Policies":$scope.List},
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if(data=="Ok")
                {
                    SweetAlert.swal({
                        title: "Policy Bulk Upload",
                        text: "Successfully Saved!",
                        confirmButtonColor: "#007AFF"
                    });
                }
                else {
                    SweetAlert.swal({
                        title: "Policy Bulk Upload",
                        text: "Save Failed:" + data+"!",
                        type: "warning",
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                }
            }).error(function (data, status, headers, config) {
                SweetAlert.swal({
                    title: "Policy Bulk Upload",
                    text: "Save Failed !",
                    type: "warning",
                    confirmButtonColor: "rgb(221, 107, 85)"
                });
            });
        }
        var uploader1 = $scope.uploader1 = new FileUploader({
            url: window.location.protocol + '//' + window.location.host + '/TAS.Web/api/PolicyReg/Upload',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwtt : $localStorage.jwt }
        });

        uploader1.filters.push({
            name: 'extensionFilter',
            fn: function (item, options) {
                var filename = item.name;
                var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
                if (extension == "xlx" || extension == "xlsx" )
                    return true;
                else {
                    alert('Invalid file format. Please select a Excel file with xlx or xlsx format and try again.');
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
                if (this.queue.length < 2)
                    return true;
                else {
                    alert('You have exceeded the limit of uploading files.');
                    return false;
                }
            }
        });
        uploader1.onWhenAddingFileFailed = function (item, filter, options) {
            console.info('onWhenAddingFileFailed', item, filter, options);
        };
        uploader1.onAfterAddingFile = function (fileItem) {
        };
        uploader1.onSuccessItem = function (fileItem, response, status, headers) {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/ConvertData',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.List = data;
            }).error(function (data, status, headers, config) {
            });
        };
        uploader1.onErrorItem = function (fileItem, response, status, headers) {
            alert('We were unable to upload your file. Please try again.');
        };
        uploader1.onCancelItem = function (fileItem, response, status, headers) {
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
            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/ConvertData',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.List = data;
            }).error(function (data, status, headers, config) {
            });
        };
    });