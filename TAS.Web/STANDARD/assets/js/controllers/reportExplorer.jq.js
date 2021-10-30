var jwt;
var paramArray = [];
$(document).ready(function () {
    jwt = $.cookie("jwt");
    LoadAllReports();
    $('#drp-rptSelector').change(selectedReportChanged);
    $("#viewReport").click(viewReport);
    $("#excelReport").click(excelReport);
    $('#startDate').datepicker().datepicker('setDate', new Date());
});

var LoadAllReports = function () {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/TAS.Web/api/Report/GetAllReportInformationByUserId",
        contentType: 'application/json; charset=UTF-8',
        data: JSON.stringify({
            loggedInUserId: $.cookie("LoggedInUserId"),
            Authorization: jwt
        }),
        success: function (data) {
            $('#drp-rptSelector')
                .find('option')
                .remove();
            $('#drp-rptSelector').append('<option value="00000000-0000-0000-0000-000000000000"> Please Select </option>');
            for (var i = 0; i < data.length; i++) {
                $('#drp-rptSelector').append('<option value="' + data[i].Id + '">' + data[i].ReportName + '</option>');
            }
        }
    });
}

var selectedReportChanged = function () {
    $('#paramSection').empty();
    var selectedReportId = $('#drp-rptSelector option:selected').val();
    paramArray = [];
    if (isGuid(selectedReportId)) {

        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: 'application/json; charset=UTF-8',
            url: "/TAS.Web/api/Report/GetAllReportParamInformationByReportId",
            data: JSON.stringify({
                reportId: selectedReportId,
                Authorization: jwt
            }),
            success: function (data) {
                paramArray = data;
                buidParametesEntrySection(data)
                //  swal.close();
            }
        });
    }
}

var buidParametesEntrySection = function (paramData) {
    if (paramData.length == 0)
        $('#paramSection').empty();

    for (var i = 0; i < paramData.length; i++) {
        if (paramData[i].ParamType === 'DATERANGE' && paramData[i].IsFromField === true) {
            buildDateRange(i);
        } else if (paramData[i].ParamType === 'TEXT') {
            buildTextBox(i);
        } else if (paramData[i].ParamType === 'INT') {
            buildIntTextBox(i);
        } else if (paramData[i].ParamType === 'DROPDOWN') {
            buildDropdown(i);
            loadDropdownData(i);
        }//yet to grow types
    }
}

var loadDropdownData = function (drpParamSeq) {
    if (!paramArray[drpParamSeq].IsDependOnPreviousParam) {
        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: 'application/json; charset=UTF-8',
            url: "/TAS.Web/api/Report/GetAllDataForReportDropdown",
            async: false,
            data: JSON.stringify({
                paramId: paramArray[drpParamSeq].Id,
                Authorization: jwt
            }),
            success: function (data) {
                $('#param-' + paramArray[drpParamSeq].Sequance)
                    .find('option')
                    .remove();
                $('#param-' + paramArray[drpParamSeq].Sequance).append('<option value=""> please select </option>');
                if (paramArray[drpParamSeq].IsShowAll) { $('#param-' + paramArray[drpParamSeq].Sequance).append('<option value="All"> All </option>'); }
                for (var i = 0; i < data.length; i++) {
                    $('#param-' + paramArray[drpParamSeq].Sequance).append('<option value="' + data[i].value + '">' + data[i].text + '</option>');
                }
            }
        });
    }
}
var loadDependantDropdownData = function (sequance) {
    for (var i = 0; i < paramArray.length; i++) {
        if (paramArray[sequance].Sequance === paramArray[i].PreviousParamSequance) {
            var parentParamSeq = paramArray[i].PreviousParamSequance;
            var paranetParamValue = $("#param-" + parentParamSeq + " option:selected").val();
            $.ajax({
                type: "POST",
                dataType: "json",
                contentType: 'application/json; charset=UTF-8',
                url: "/TAS.Web/api/Report/GetAllDataForReportFromParentDropdown",
                async: false,
                data: JSON.stringify({
                    paramId: paramArray[sequance].Id,
                    parentParamValue: paranetParamValue,
                    parentParamId: paramArray[parentParamSeq].Id,
                    Authorization: jwt
                }),
                success: function (data) {
                    $('#param-' + paramArray[i].Sequance)
                        .find('option')
                        .remove();
                    $('#param-' + paramArray[i].Sequance).append('<option value="00000000-0000-0000-0000-000000000000"> Please Select </option>');
                    for (var j = 0; j < data.length; j++) {
                        $('#param-' + paramArray[i].Sequance).append('<option value="' + data[j].value + '">' + data[j].text + '</option>');
                    }
                }
            });
        }
    }
}
var buildDropdown = function (drpParamSeq) {

    var dropdownHtml = '<div class="col-md-6">' +
        '            <div class="form-group">' +
        '                <label class="control-label" for="form-field-1">' +
        '                    ' + paramArray[drpParamSeq].ParamName + '<span class="symbol required"></span>' +
        '                </label>' +
        '                <select class="form-control input-sm dynamicDropdown" id="param-' + paramArray[drpParamSeq].Sequance + '"' +
        '                    onchange=loadDependantDropdownData("' + drpParamSeq + '")' +
        '> ' +
        '                   ' +
        '                    <option value ="">please select </option>' +
        '                </select>' +
        '' +
        '            </div>' +
        '        </div>';
    $('#paramSection').append(dropdownHtml);

}
var buildIntTextBox = function (textParamSeq) {

    var intTextBoxHtml = '<div class="col-md-6">' +
        '            <div class="form-group">' +
        '                <label class="control-label">' +
        '                  ' + paramArray[textParamSeq].ParamName + '<span class="symbol required"></span>' +
        '                </label>' +
        '                <input type="number" class="form-control  input-sm text-right" id="param-' + paramArray[textParamSeq].Sequance + '" >' +
        '            </div>' +
        '        </div>';

    $('#paramSection').append(intTextBoxHtml);
}
var buildTextBox = function (textParamSeq) {

    var textBoxHtml = '<div class="col-md-6">' +
        '            <div class="form-group">' +
        '                <label class="control-label">' +
        '                  ' + paramArray[textParamSeq].ParamName + '<span class="symbol required"></span>' +
        '                </label>' +
        '                <input type="text" class="form-control  input-sm text-left" id="param-' + paramArray[textParamSeq].Sequance + '">' +
        '            </div>' +
        '        </div>';

    $('#paramSection').append(textBoxHtml);
}
var buildDateRange = function (dateRangeFromParamSeq) {

    var dateRangeHtml = '<div class="col-md-6">' +
        '            <label class="control-label">' +
        '                ' + paramArray[dateRangeFromParamSeq].ParamName + '<span class="symbol required"></span>' +
        '            </label>' +
        '            <div class="input-group date" data-provide="datepicker" data-date-format="dd-M-yyyy"' +
        '                 data-date-autoclose="true" id="startDate">' +
        '                <input type="text" id="param-' + paramArray[dateRangeFromParamSeq].Sequance + '" class="form-control">' +
        '                <div class="input-group-addon">' +
        '                    <span class="glyphicon glyphicon-th"></span>' +
        '                </div>' +
        '            </div>' +
        '' +
        '        <br/></div>' +
        '        <div class="col-md-6">' +
        '            <label class="control-label">' +
        '                ' + paramArray[dateRangeFromParamSeq + 1].ParamName + '<span class="symbol required"></span>' +
        '            </label>' +
        '            <div class="input-group date" data-provide="datepicker" data-date-format="dd-M-yyyy"' +
        '                 data-date-autoclose="true" id="endDate">' +
        '                <input type="text" id="param-' + paramArray[dateRangeFromParamSeq + 1].Sequance + '" class="form-control">' +
        '                <div class="input-group-addon">' +
        '                    <span class="glyphicon glyphicon-th"></span>' +
        '                </div>' +
        '            </div>' +
        '        </div><div class="clearfix"></div>';
    // $('#param-' + paramArray[dateRangeFromParamSeq].Sequance + ).datepicker("setDate", new Date());

    $('#paramSection').append(dateRangeHtml);
}

var validateReportParameters = function () {
    var isValid = true;
    var selectedReportId = $('#drp-rptSelector option:selected').val();
    if (!isGuid(selectedReportId)) {
        isValid = false;
        $('#drp-rptSelector').addClass("has-error");
    } else
        $('#drp-rptSelector').removeClass("has-error");
    for (var i = 0; i < paramArray.length; i++) {
        if (paramArray[i].ParamType === 'TEXT') {
            var textValue = $('#param-' + paramArray[i].Sequance).val();
            if (typeof textValue === 'undefined' || textValue.trim().length === 0) {
                isValid = false;
                $('#param-' + paramArray[i].Sequance).addClass("has-error");
            } else {
                $('#param-' + paramArray[i].Sequance).removeClass("has-error");
            }
        } else if (paramArray[i].ParamType === 'INT') {
            var textValue = $('#param-' + paramArray[i].Sequance).val();
            if (typeof textValue === 'undefined' || textValue.trim().length === 0 || !parseInt(textValue)) {
                isValid = false;
                $('#param-' + paramArray[i].Sequance).addClass("has-error");
            } else {
                $('#param-' + paramArray[i].Sequance).removeClass("has-error");
            }
        } else if (paramArray[i].ParamType === 'DATERANGE') {
            //value exist validation
            var valueok = false;
            var textValue = $('#param-' + paramArray[i].Sequance).val();
            if (typeof textValue === 'undefined' || textValue.trim().length === 0 || !Date.parse(textValue)) {
                isValid = false;
                $('#param-' + paramArray[i].Sequance).addClass("has-error");
            } else {
                $('#param-' + paramArray[i].Sequance).removeClass("has-error");
                valueok = true;
            }
            //date range validation
            if (valueok) {
                var rangeok = true;
                if (paramArray[i].IsFromField === true) {
                    if (Date.parse(textValue) <= Date.parse($('#param-' + paramArray[i + 1].Sequance).val())) {
                        $('#param-' + paramArray[i].Sequance).removeClass("has-error");
                        $('#param-' + paramArray[i + 1].Sequance).removeClass("has-error");

                    } else {
                        isValid = rangeok = false;
                        $('#param-' + paramArray[i].Sequance).addClass("has-error");
                        $('#param-' + paramArray[i + 1].Sequance).addClass("has-error");
                    }
                } else {
                    if (Date.parse(textValue) >= Date.parse($('#param-' + paramArray[i - 1].Sequance).val())) {
                        $('#param-' + paramArray[i].Sequance).removeClass("has-error");
                        $('#param-' + paramArray[i - 1].Sequance).removeClass("has-error");

                    } else {
                        isValid = rangeok = false;
                        $('#param-' + paramArray[i].Sequance).addClass("has-error");
                        $('#param-' + paramArray[i - 1].Sequance).addClass("has-error");

                    }
                }
                if (!rangeok) {
                    //show date range issue
                    //  alert('Date Range is Invalid.');
                }
            }
        } else if (paramArray[i].ParamType === 'DROPDOWN') {
            var drpValue = $('#param-' + paramArray[i].Sequance + ' option:selected').val();
            var rangeok = true;
            if (drpValue == 'All') {

                $('#param-' + paramArray[i].Sequance).removeClass("has-error");
            }
            else if (typeof drpValue === 'undefined' || !isGuid(drpValue)) {
                isValid = false;
                $('#param-' + paramArray[i].Sequance).addClass("has-error");
            } else {
                $('#param-' + paramArray[i].Sequance).removeClass("has-error");
            }

            if ( paramArray[i].IsIncluedeInDateRange && drpValue != 'All' && typeof drpValue !== 'undefined') {
                if (paramArray[i].IsFromField === true) {
                    if (typeof $('#param-' + paramArray[i + 1].Sequance + ' option:selected').val() !== 'undefined') {
                        rangeok = false;
                        var fromValArr = $('#param-' + paramArray[i].Sequance + ' option:selected').text().split('-');
                        var toValArr = $('#param-' + paramArray[i + 1].Sequance + ' option:selected').text().split('-');
                        var fromVal = 0; var toVal = 0;
                        fromValArr.forEach(function (num, i) { fromVal += (i == 1) ? parseFloat(num) / 12 : parseFloat(num) || 0; });
                        toValArr.forEach(function (num, i) { toVal += (i == 1) ? parseFloat(num) / 12 : parseFloat(num) || 0; });
                        if (fromVal <= toVal) {
                            rangeok = true;
                            $('#param-' + paramArray[i].Sequance).removeClass("has-error");
                            $('#param-' + paramArray[i + 1].Sequance).removeClass("has-error");
                        }
                        else {
                            isValid = false;
                            $('#param-' + paramArray[i].Sequance).addClass("has-error");
                            $('#param-' + paramArray[i + 1].Sequance).addClass("has-error");
                        }
                    }
                }
                else {
                    if (typeof $('#param-' + paramArray[i - 1].Sequance + ' option:selected').val() !== 'undefined') {
                        rangeok = false;
                        var fromValArr = $('#param-' + paramArray[i - 1].Sequance + ' option:selected').text().split('-');
                        var toValArr = $('#param-' + paramArray[i].Sequance + ' option:selected').text().split('-');
                        var fromVal = 0; var toVal = 0;
                        fromValArr.forEach(function (num, i) { fromVal += (i == 1) ? parseFloat(num) / 12 : parseFloat(num) || 0; });
                        toValArr.forEach(function (num, i) { toVal += (i == 1) ? parseFloat(num) / 12 : parseFloat(num) || 0; });
                        if (fromVal <= toVal) {
                            rangeok = true;
                            $('#param-' + paramArray[i].Sequance).removeClass("has-error");
                            $('#param-' + paramArray[i - 1].Sequance).removeClass("has-error");
                        }
                        else {
                            isValid = false;
                            $('#param-' + paramArray[i].Sequance).addClass("has-error");
                            $('#param-' + paramArray[i - 1].Sequance).addClass("has-error");
                        }
                    }
                }
            }
        }//yet to grow
    }

    return isValid;
}
var viewReport = function () {
    if (validateReportParameters()) {
        var paramData = [];
        for (var i = 0; i < paramArray.length; i++) {
            if (paramArray[i].ParamType === 'TEXT') {
                var textValue = $('#param-' + paramArray[i].Sequance).val();
                var data = {
                    id: paramArray[i].Id,
                    value: textValue
                };
                paramData.push(data);
            } else if (paramArray[i].ParamType === 'INT') {
                var textValue = $('#param-' + paramArray[i].Sequance).val();
                var data = {
                    id: paramArray[i].Id,
                    value: textValue
                };
                paramData.push(data);
            } else if (paramArray[i].ParamType === 'DATERANGE') {

                var textValue = $('#param-' + paramArray[i].Sequance).val();
                var data = {
                    id: paramArray[i].Id,
                    value: textValue
                };
                paramData.push(data);

            } else if (paramArray[i].ParamType === 'DROPDOWN') {
                var drpValue = $('#param-' + paramArray[i].Sequance + ' option:selected').val();
                var data = {
                    id: paramArray[i].Id,
                    value: drpValue
                };
                paramData.push(data);
            }//yet to grow
        }

        // swal({ title: 'Processing...!', text: "Please wait...", showConfirmButton: false });


        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: 'application/json; charset=UTF-8',
            url: "/TAS.Web/api/Report/ViewReport",
            async: false,
            data: JSON.stringify({
                reportId: $('#drp-rptSelector option:selected').val(),
                params: paramData,
                Authorization: jwt
            }),
            success: function (datar) {
                if (typeof datar !== "undefined" && datar !== null && datar.length > 0 && isGuid(datar)) {
                    var url = window.location.protocol + '//' + window.location.host +
                        '/TAS.Web/ReportExplorer.aspx?key=' + datar + "&jwt=" + jwt.replace(/['"]+/g, '');
                    // window.open(url, '_blank');
                    try {

                        var a = document.createElement('A');
                        a.href = url;
                        // a.download = 'DealerInvoiceCode.pdf';
                        document.body.appendChild(a);
                        a.click();
                        document.body.removeChild(a);

                    } catch (e) {
                        // swal.close();
                        Swal.fire({
                            type: 'error',
                            title: '',
                            text: "Someting went wrong. Pelase contract administrator.",
                            footer: ''
                        })

                    }
                } else {
                    // swal.close();
                    Swal.fire({
                        type: 'error',
                        title: '',
                        text: "Someting went wrong. Pelase contract administrator.",
                        footer: ''
                    })
                }
            }
        });
    } else {
        // swal.close();
        Swal.fire({
            type: 'error',
            title: '',
            text: 'Please fill in or correct highlighted fields.',
            footer: ''
        })
    }
}

var excelReport = function () {
    if (validateReportParameters()) {
        var paramData = [];
        for (var i = 0; i < paramArray.length; i++) {
            if (paramArray[i].ParamType === 'TEXT') {
                var textValue = $('#param-' + paramArray[i].Sequance).val();
                var data = {
                    id: paramArray[i].Id,
                    value: textValue
                };
                paramData.push(data);
            } else if (paramArray[i].ParamType === 'INT') {
                var textValue = $('#param-' + paramArray[i].Sequance).val();
                var data = {
                    id: paramArray[i].Id,
                    value: textValue
                };
                paramData.push(data);
            } else if (paramArray[i].ParamType === 'DATERANGE') {

                var textValue = $('#param-' + paramArray[i].Sequance).val();
                var data = {
                    id: paramArray[i].Id,
                    value: textValue
                };
                paramData.push(data);

            } else if (paramArray[i].ParamType === 'DROPDOWN') {
                var drpValue = $('#param-' + paramArray[i].Sequance + ' option:selected').val();
                var data = {
                    id: paramArray[i].Id,
                    value: drpValue
                };
                paramData.push(data);
            }//yet to grow
        }

        // swal({ title: 'Processing...!', text: "Please wait...", showConfirmButton: false });


        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: 'application/json; charset=UTF-8',
            url: "/TAS.Web/api/Report/excelReport",
            async: false,
            data: JSON.stringify({
                reportId: $('#drp-rptSelector option:selected').val(),
                params: paramData,
                Authorization: jwt
            }),
            success: function (datar) {
                if (typeof datar !== "undefined" && datar !== null && datar.length > 0 && isGuid(datar)) {
                    var url = window.location.protocol + '//' + window.location.host +
                        '/TAS.Web/ReportExplorer.aspx?key=' + datar + "&jwt=" + jwt.replace(/['"]+/g, '');
                    // window.open(url, '_blank');
                    try {

                        var a = document.createElement('A');
                        a.href = url;
                        // a.download = 'DealerInvoiceCode.pdf';
                        document.body.appendChild(a);
                        a.click();
                        document.body.removeChild(a);

                    } catch (e) {
                        // swal.close();
                        Swal.fire({
                            type: 'error',
                            title: '',
                            text: "Someting went wrong. Pelase contract administrator.",
                            footer: ''
                        })

                    }
                } else {
                    // swal.close();
                    Swal.fire({
                        type: 'error',
                        title: '',
                        text: "Someting went wrong. Pelase contract administrator.",
                        footer: ''
                    })
                }
            }
        });
    } else {
        // swal.close();
        Swal.fire({
            type: 'error',
            title: '',
            text: 'Please fill in or correct highlighted fields.',
            footer: ''
        })
    }
}

//supportive functions
var isGuid = function (stringToTest) {
    var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
    var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
    return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
};
var emptyGuid = function () {
    return "00000000-0000-0000-0000-000000000000";
};