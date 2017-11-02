app.controller("reportsController",
    function ($sce,$rootScope, $scope, resultService, periodsArray, hygEnum, language, languageService, userSettings, reportsService, userService) {
             
        init();                  
     
        $scope.visiblePopup = false;

        $scope.popupOptions = {
            closeOnBackButton: true, 
            fullScreen: true,
            height:"auto",
            width: "auto",
            showTitle: true,
            title: "Generated Report",
            dragEnabled: false,
            closeOnOutsideClick: true,
            bindingOptions: {
                visible: "visiblePopup",
                contentTemplate: "reportHtml"
              
            }
        };
     
        var reportBy = ["Summary","Day", "Week", "Month", "Year", "User", "Plan", "Location"];

        var chatType = ["Pie Chart", "Bar Chart"];
        $scope.radioGroup = {
            
            reportType: {
                items: reportBy,
                value: reportBy[0],
                layout: "horizontal"
            },
            changeLayout: {
                items: chatType,
                value: chatType[0],
                layout: "horizontal"
            }
        };

        $scope.gridOptions = {
            columnAutoWidth: true,
            columnChooser: {
                enabled: true,
                height: 250,
                width: 400
            },          
            height: 440,
            showBorders: true,          
            "export": {
                enabled: true,
                fileName: "reportData"
            },
            bindingOptions: {
                dataSource: "gridSource"
            }
        };

        $scope.changePeriod =
            function (e) {
                updatePerdioFunc(e.id);
            };

        $scope.reportHtml = "";
        $scope.loadingVisible = false;
        $scope.closeOnOutsideClick = false;
        $scope.showIndicator = true;
        $scope.showPane = true;
        $scope.shading = true;

        $scope.loadOptions = {
            shadingColor: "rgba(0,0,0,0.4)",
            bindingOptions: {
                visible: "loadingVisible",
                showIndicator: "showIndicator",
                showPane: "showPane",
                shading: "shading",
                closeOnOutsideClick: "closeOnOutsideClick"
            },
            onShown: function () {

            },
            onHidden: function () {

            }
        };
        $scope.printReport= function() { 
          
            window.print();
        }
        $scope.showLoadPanel = function () {
           
            $scope.loadingVisible = true;

            reportsService.generateReport($scope.reportReportType.report, $scope.datePickerFromDate, $scope.datePickerToDate).
                then(function (response)
                {
                    var originalContents = document.body.innerHTML;
                    document.body.innerHTML = $sce.trustAsHtml(response.data);

                    $scope.visiblePopup = true;
                    $scope.reportHtml = $sce.trustAsHtml(response.data);
                    $scope.loadingVisible = false;
                  
                    
                },
                function (response)
                {
                    console.log(response);
                }
            );
        };
        function init() {

            $scope.dateBox = {
                dateTimeFrom: {
                    type: "datetime",
                    pickerType: "rollers",
                    min: new Date(1985, 0, 1),
                    bindingOptions: { value: "datePickerFromDate._d"},
                    onChange: function (data) {
                        console.log("change");

                    },
                    onChanged: function (data) {
                        console.log("onChanged");

                    },
                    onClosed: function (data) {
                        console.log("onClosed");

                        $scope.selectedP = periodsArray[hygEnum.periodsEnum.Custom];
                        data.model.selectedP = periodsArray[hygEnum.periodsEnum.Custom];
                    }
                },
                dateTimeTo: {
                    type: "datetime",
                    pickerType: "rollers",
                    bindingOptions: { value: "datePickerToDate._d"},

                    onChange: function (data) {
                        console.log("change");

                    },
                    onChanged: function (data) {
                        console.log("onChanged");

                    },
                    onClosed: function (data) {
                    
                        console.log("onClosed");
                        data.model.selectedP = periodsArray[hygEnum.periodsEnum.Custom];
                        $scope.selectedP = periodsArray[hygEnum.periodsEnum.Custom];
               

                    },
                    applyValueMode: "useButtons"
                }
            };
            setDefaults();
         
            updatePerdioFunc(hygEnum.periodsEnum.LastYear);


            resultService.getResults($scope.datePickerFromDate, $scope.datePickerToDate).
                then(function (response) {
                  
                    $scope.gridSource =  { store: response.data };
                },
                function (response) {
                    console.log(response);
                }
                );
         
        }
      
        function setDefaults() {
            try {
                $scope.title = "reportController";

                if (typeof $scope.language !== "undefined" && $scope.language !== null) {

                 
                    $scope.reportTypes = [
                        { id: 1, report: "Pass Caution Fail" },
                        { id: 2, report: "Failed Test" },
                        { id: 2, report: "Trend" },
                        { id: 2, report: "Retest" },
                        { id: 2, report: "Auditor" }
                    ];

                   $scope.reportReportType = $scope.reportTypes[0]; // TODO: use stored cust preference instead of zero

                    $scope.periods = periodsArray;

                }
            } catch (e) {
                console.log("Set Defaults Error");
                console.log(e);
            }
        }

        function updatePerdioFunc(period) {

            if (typeof period !== "undefined") {

                if (period === hygEnum.periodsEnum.LastWeek) {
                   
                    $scope.datePickerFromDate = moment().subtract(1, "week").startOf("week").startOf("day");
                    $scope.datePickerToDate = moment().subtract(1, "week").endOf("week").endOf("day");
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.LastWeek];

                }
                else if (period === hygEnum.periodsEnum.LastMonth) {
                    $scope.datePickerFromDate = moment().subtract(1, "month").startOf("month")
                        .startOf("day");
                    $scope.datePickerToDate = moment().subtract(1, "month").endOf("month").endOf("day");
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.LastMonth];

                }
                else if (period === hygEnum.periodsEnum.LastYear) {
                    $scope.datePickerFromDate = moment()
                        .subtract(1, "year")
                        .startOf("year")
                        .startOf("month")
                        .startOf("day");
                    $scope
                        .datePickerToDate = moment()
                            .subtract(1, "year")
                            .endOf("year")
                            .endOf("month")
                            .endOf("day");
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.LastYear];

                }
                else if (period === hygEnum.periodsEnum.ThisWeek) {
                    $scope.datePickerFromDate = moment().startOf("week").startOf("day");
                    $scope.datePickerToDate = moment().endOf("day");
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.ThisWeek];

                }
                else if (period === hygEnum.periodsEnum.ThisMonth) {
                    $scope.datePickerFromDate = moment().startOf("month").startOf("day");
                    $scope.datePickerToDate = moment().endOf("day");
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.ThisMonth];

                }
                else if (period === hygEnum.periodsEnum.ThisYear) {
                    $scope.datePickerFromDate = moment().startOf("year").startOf("month").startOf("day");
                    $scope.datePickerToDate = moment().endOf("day");
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.ThisYear];

                }
                else if (period === hygEnum.periodsEnum.Today) {
                    $scope.datePickerFromDate = moment().startOf("day");
                    $scope.datePickerToDate = moment().endOf("day");
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.Today];

                }
                else if (period === hygEnum.periodsEnum.Yesterday) {
                    $scope.datePickerFromDate = moment().subtract(1, "day").startOf("day");
                    $scope.datePickerToDate = moment().subtract(1, "day").endOf("day");
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.Yesterday];

                }
                else if (period === hygEnum.periodsEnum.Custom) {
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.Custom];

                }

            }
        }
    });