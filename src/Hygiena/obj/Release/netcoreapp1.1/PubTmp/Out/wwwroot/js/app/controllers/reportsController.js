app.controller("reportsController",
    function(
        $sce,
        $rootScope,
        $scope,
        reportsService,
        periodsArray,
        hygEnum,
        languageService,
        language
    ) {

       

        init();

        function init() {


            $scope.printDiv = function (divName) {

                reportsService.generateReport($scope.reportReportType.report,
                    $scope.datePickerFromDate,
                    $scope.datePickerToDate).then(function (response) {
                        console.log(response.data);

                    },
                    function (response) {
                        console.log(response);
                    }
                    );


                let printContents = document.getElementById(divName).innerHTML;
                let popupWin = window.open("", "_blank");
                popupWin.document.open();
                popupWin.document
                    .write(`<html><head><link rel="stylesheet" href="./css/site.css"/></head><body onload="window.print()"><hr/><div class="reportTypeTitle">${$scope.pcfTypeGroupSelected} report</div><hr/><div class="reportChartClass">${printContents
                        }</div></body></html>`);
                popupWin.document.close();
            };
            
            $scope.filterRow = {
                visible: true,
                applyFilter: "auto"
            };

            $scope.headerFilter = {
                visible: true
            };

            var tabs = [
                {
                    id: 0,
                    text: "Charts",
                    icon: "chart",
                    content: "User tab content"
                },
                {
                    id: 1,
                    text: "DataGrid",
                    icon: "menu",
                    content: "Comment tab content"
                }
            ];

            $scope.reportGridOptions = {
                bindingOptions: {
                    dataSource: "reportData",
                    columns: "reportcolumns",
                    filterRow: "filterRow",
                    headerFilter: "headerFilter"
                },
                "export": {
                    enabled: true,
                    fileName: "ReportData"
                },       
                showBorders: true,
                showColumnLines: true,
                showRowLines: true,
                rowAlternationEnabled: true,
                showScrollbar: "always",
                paging: {
                    pageSize: 100000000000000000
                },
                loadPanel: {
                    enabled: false
                },
                height: function () {
                    return window.innerHeight / 1.5;
                },
                hoverStateEnabled: true
            };

            $scope.tabs = tabs;
            $scope.selectedTab = 0;
          

            $scope.$watch("selectedTab", function (newValue) {

     
                $scope.tabContent = tabs[newValue].content;
                if (newValue === 0) {

                    if ($("#reportGrid").dxDataGrid("instance").getDataSource() !== null ) {
                        
                        updateChartData($("#reportGrid").dxDataGrid("instance").getDataSource()._items);

                    }
                
                }

            });

            $scope.idList = [];

            $scope.dateBox = {
                dateTimeFrom: {
                    type: "datetime",
                    pickerType: "rollers",
                    min: new Date(1985, 0, 1),
                    bindingOptions: { value: "datePickerFromDate._d" },
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
                        getReportData();
                    }
                },
                dateTimeTo: {
                    type: "datetime",
                    pickerType: "rollers",
                    bindingOptions: { value: "datePickerToDate._d" },

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
                        getReportData();


                    },
                    applyValueMode: "useButtons"
                }
            };

            $scope.changePeriod = function (e) {
                updatePerdioFunc(e.id);
                getReportData();
            };

            $scope.updateReport = function (e) {

                filterReports();


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

            $scope.idList.length = 0;
            $scope.loadingVisible = true;
            $scope.language = language[0];
            $scope.hygEnum = hygEnum;
            if (typeof $scope.language !== "undefined") {


                $scope.reportcolumns =  [
                    {
                        caption: toTitleCase($scope.language.plan.caption)
                    },
                    {
                        caption: toTitleCase($scope.language.numberOfFails.caption),
                        dataField: "numberOfFails"
                    },
                    {
                        dataField: "deviceName"
                    },
                    {
                        dataField: "unitName"
                    },
                    {
                        dataField: "unitNo"
                    }
                    ,
                    {
                        dataField: "planName"
                    },
                    {
                        dataField: "locationName"
                    },
                    {
                        dataField: "userName"
                    }
                ];

                $scope.pcfType = [$scope.language.summary.caption, $scope.language.dayofWeek.caption, $scope.language.month.caption, $scope.language.year.caption, $scope.language.user.caption, $scope.language.plan.caption, $scope.language.location.caption];
                $scope.pcfTypeGroupSelected = $scope.language.summary.caption;


                $scope.chatType = [$scope.language.pieChart.caption, $scope.language.barChart.caption, $scope.language.donutChart.caption];
                $scope.chatTypeSelected = $scope.language.pieChart.caption;


                $scope.pcfTypeGroup = {
                    reportType: {
                        value: $scope.pcfTypeGroupSelected,
                        bindingOptions: { items: "pcfType" },
                        layout: "horizontal",
                        onValueChanged: function (e) {
                          
                            $scope.loadingVisible = true;
                            $scope.pcfTypeGroupSelected = e.value;
                            chatType();
                            $scope.loadingVisible = false;
                        },
                        onOptionChanged: function (e) {
                           
                          

                        }
                    },
                    changeLayout: {
                        items: $scope.chatType,
                        value: $scope.chatType[0],
                        layout: "horizontal",
                        onValueChanged: function (e) {
                      
                            $scope.chatTypeSelected = e.value;
                            chatType();
                          
                           
                        },
                        onOptionChanged: function (e) {


                        }
                    }
                };

                setDefaults();
                updatePerdioFunc(hygEnum.periodsEnum.LastYear);
                getReportData();
                $scope.loadingVisible = false;
            }

        }

        function filterReports() {

            for (var i = 0, len = $scope.idList.length; i < len; i++) {

                var pass = $scope.idList[i].data("Pass")[0].values[0].value;
                var fail = $scope.idList[i].data("Fail")[0].values[0].value;
                var caution = $scope.idList[i].data("Caution")[0].values[0].value;
                var retest = $scope.idList[i].data("Retest")[0].values[0].value;

                $scope.idList[i].hide(["Pass", "Fail", "Caution", "Other", "Retest"]);
                $scope.idList[i].legend.hide(["Pass", "Fail", "Caution", "Other", "Retest"]);
          

                if ($scope.reportReportType.id === $scope.hygEnum.reportEnum.PCF) {

                    $scope.idList[i].legend.show(["Pass", "Fail", "Caution"]);
                    $scope.idList[i].show(["Pass", "Fail", "Caution"]);
                }
                else if ($scope.reportReportType.id === $scope.hygEnum.reportEnum.Fail) {

                    $scope.idList[i].legend.show(["Fail", "Other"]);
                    $scope.idList[i].show(["Fail", "Other"]);

                    $scope.idList[i].load({
                        columns: [
                            ["Other", (pass + caution + retest)]
                        ]
                    });

                }
                else if ($scope.reportReportType.id === $scope.hygEnum.reportEnum.Pass) {
                    $scope.idList[i].show(["Pass", "Other"]);
                    $scope.idList[i].legend.show(["Pass", "Other"]);

                    $scope.idList[i].load({
                        columns: [
                            ["Other", (caution + retest + fail)]
                        ]
                    });

                }
                else if ($scope.reportReportType.id === $scope.hygEnum.reportEnum.Retest) {
                    $scope.idList[i].show(["Retest", "Other"]);
                    $scope.idList[i].legend.show(["Retest", "Other"]);

                    $scope.idList[i].load({
                        columns: [
                            ["Other", (caution + pass + fail)]
                        ]
                    });

                }
                else if ($scope.reportReportType.id === $scope.hygEnum.reportEnum.Caution) {
                    $scope.idList[i].show(["Caution", "Other"]);
                    $scope.idList[i].legend.show(["Caution", "Other"]);
               
                    $scope.idList[i].load({
                        columns: [
                            ["Other", (retest + pass + fail)]
                        ]
                    });

                }
            }
        }


        function getReportData() {

            reportsService.getAllData(
                $scope.datePickerFromDate,
                $scope.datePickerToDate).then(function (response) {
                    $scope.reportData = response.data;
                    updateChartData(response.data);
                   
                 
                },
                function (response) {
                    console.log(response);
                });
        }

        function updateChartData(data) {
          

            $scope.fails = data.filter(function (value) {
                return value.rlu > value.upper;
            });

            $scope.caution = data.filter(function (value) {
                return value.rlu <= value.upper && value.rlu > value.lower;
            });

            $scope.pass = data.filter(function (value) {
                return value.rlu <= value.lower;
            });

            $scope.retest = data.filter(function (value) {
                return value.repeatedTest !== null;
            });

            chatType();

        }

        function filterByWeekDay(day) {
            
            const div = document.createElement("div");
            div.id = day.replace(" ", "") + "_id_";
            div.className = "reportChartClass";

            document.getElementById("chartDiv").appendChild(div);

            let fails = $scope.fails.filter(function (value) {
                return  moment(value.resultDate).locale("en").format("dddd") === day;
            }).length;
            // x => x.RLU <= x.Upper && x.RLU > x.Lower 
            let caution = $scope.caution.filter(function (value) {
                return  value.rlu > value.lower && moment(value.resultDate).locale("en").format("dddd") === day;
            }).length;
            //x.RLU <= x.Lower
            let pass = $scope.pass.filter(function (value) {
                return  moment(value.resultDate).locale("en").format("dddd") === day;
            }).length;
            let retest = $scope.retest.filter(function (value) {
                return  moment(value.resultDate).locale("en").format("dddd") === day;
                 }).length;
           

            if (fails > 0 || pass > 0 || caution > 0 || retest > 0) {

                $scope.idList.push(c3.generate(returnChartDataColums(fails, pass, caution, retest, div.id, chartTypeName())));
            }

        }
        function filterByMonth(monthName, monthNo) {

            const div = document.createElement("div");
            div.id = monthName.replace(" ", "") + "_id_";
            div.className = "reportChartClass";
            
            document.getElementById("chartDiv").appendChild(div);

            let fails = $scope.fails.filter(function (value) {
                return moment(value.resultDate).format("M") === monthNo;
            }).length;
            // x => x.RLU <= x.Upper && x.RLU > x.Lower 
            let caution = $scope.caution.filter(function (value) {
                return  moment(value.resultDate).format("M") === monthNo;
            }).length;
            //x.RLU <= x.Lower
            let pass = $scope.pass.filter(function (value) {
                return  moment(value.resultDate).format("M") === monthNo;
            }).length;

            let retest = $scope.retest.filter(function (value) {
                return moment(value.resultDate).format("M") === monthNo;
            }).length;
      

            if (fails > 0 || pass > 0 || caution > 0 || retest > 0) {
                $scope.idList.push(c3.generate(returnChartDataColums(fails, pass, caution, retest, div.id, chartTypeName())));
            }
        }
        function filterByYear(year) {

            const div = document.createElement("div");
            div.id = `_${year.toString().replace(" ", "")}` + "_id_";
            div.className = "reportChartClass";
          
            document.getElementById("chartDiv").appendChild(div);

            let fails = $scope.fails.filter(function (value) {
                return value.rlu > value.upper && moment(value.resultDate).format("YYYY").toString() === year.toString();
            }).length;
            // x => x.RLU <= x.Upper && x.RLU > x.Lower 
            let caution = $scope.caution.filter(function (value) {
                return  value.rlu > value.lower && moment(value.resultDate).format("YYYY").toString() === year.toString();
            }).length;
            //x.RLU <= x.Lower
            let pass = $scope.pass.filter(function (value) {
                return  moment(value.resultDate).format("YYYY").toString() === year.toString();
            }).length;
            let retest = $scope.retest.filter(function(value) {
                return  moment(value.resultDate).format("YYYY").toString() === year.toString();
            }).length;
            

            if (fails > 0 || pass > 0 || caution > 0 || retest > 0) {
                $scope.idList.push(c3.generate(returnChartDataColums(fails, pass, caution, retest, div.id, chartTypeName())));
            }

        }
        function filterByUser(userName) {

               const div = document.createElement("div");
                div.id = userName.replace(" ", "") + "_id_";
                div.className = "reportChartClass";
             
                document.getElementById("chartDiv").appendChild(div);

                let fails = $scope.fails.filter(function (value) {
                    return value.userName === userName;
                }).length;
                // x => x.RLU <= x.Upper && x.RLU > x.Lower 
                let caution = $scope.caution.filter(function (value) {
                    return value.userName === userName;
                }).length;
                //x.RLU <= x.Lower
                let pass = $scope.pass.filter(function (value) {
                    return  value.userName === userName;
                }).length;

                let retest = $scope.retest.filter(function (value) {
                    return value.userName === userName;
                }).length;

                if (fails > 0 || pass > 0 || caution > 0 || retest > 0) {
                    $scope.idList.push(c3.generate(returnChartDataColums(fails, pass, caution, retest, div.id, chartTypeName())));
            }

        }
        function filterByPlan(plan) {
            
            const div = document.createElement("div");
            div.id = plan.replace(" ", "") + "_id_";
            div.className = "reportChartClass";
         
            document.getElementById("chartDiv").appendChild(div);

            let fails = $scope.fails.filter(function (value) {
                return  value.planName === plan;
            }).length;
            // x => x.RLU <= x.Upper && x.RLU > x.Lower 
            let caution = $scope.caution.filter(function (value) {
                return  value.planName === plan;
            }).length;
            //x.RLU <= x.Lower
            let pass = $scope.pass.filter(function (value) {
                return value.planName === plan;
            }).length;

            let retest = $scope.retest.filter(function (value) {
                return value.planName === plan;
            }).length;

              
            if (fails > 0 || pass > 0 || caution > 0 || retest > 0) {
                $scope.idList.push(c3.generate(returnChartDataColums(fails, pass, caution, retest, div.id, chartTypeName())));
            }
        }
        function filterByLocation(location) {
           
            const div = document.createElement("div");
            div.id = location.replace(" ", "") + "_id_";
            div.className = "reportChartClass";
          
            document.getElementById("chartDiv").appendChild(div);
          
            let fails = $scope.fails.filter(function (value) {
                return value.locationName === location;
            }).length;
            // x => x.RLU <= x.Upper && x.RLU > x.Lower 
            let caution = $scope.caution.filter(function (value) {
                return  value.locationName === location;
            }).length;
            //x.RLU <= x.Lower
            let pass = $scope.pass.filter(function (value) {
                return  value.locationName === location;
            }).length;


            let retest = $scope.retest.filter(function (value) {
                return value.locationName === location;
            }).length;

            if (fails > 0 || pass > 0 || caution > 0 || retest > 0) {
                $scope.idList.push(c3.generate(returnChartDataColums(fails, pass, caution, retest, div.id, chartTypeName())));
            }
        }

        function chatType() {
            $scope.idList.length = 0;
            if (typeof $scope.language !== "undefined") {
                $("#chartDiv").empty();
                if (!$scope.reportData) return null;
                // Day of Week 
                if ($scope.pcfTypeGroupSelected === $scope.language.dayofWeek.caption) {

                    filterByWeekDay("Sunday");
                    filterByWeekDay("Monday");
                    filterByWeekDay("Tuesday");
                    filterByWeekDay("Wednesday");
                    filterByWeekDay("Thursday");
                    filterByWeekDay("Friday");
                    filterByWeekDay("Saturday");

                } else if ($scope.pcfTypeGroupSelected === $scope.language.month.caption) {
                    filterByMonth("January", "1");
                    filterByMonth("February", "2");
                    filterByMonth("March", "3");
                    filterByMonth("April", "4");
                    filterByMonth("May", "5");
                    filterByMonth("June", "6");
                    filterByMonth("July", "7");
                    filterByMonth("August", "8");
                    filterByMonth("September", "9");
                    filterByMonth("October", "10");
                    filterByMonth("November", "11");
                    filterByMonth("December", "12");
                } else if ($scope.pcfTypeGroupSelected === $scope.language.year.caption) {

                    var years = moment($scope.datePickerToDate).diff($scope.datePickerFromDate, "years");
                    for (var year = 0; year < years; year++) {
                        filterByYear(($scope.datePickerFromDate._d.getFullYear() + year));
                    }

                    filterByYear($scope.datePickerToDate._d.getFullYear());


                } else {
                    var y;
                    var uniqueNames;
                    var i;
                    if ($scope.pcfTypeGroupSelected === $scope.language.user.caption) {
                        uniqueNames = [];
                        for (i = 0; i < $scope.reportData.length; i++) {
                            if (uniqueNames.indexOf($scope.reportData[i].userName) === -1) {
                                uniqueNames.push($scope.reportData[i].userName);
                            }
                        }

                        for (y = 0; y < uniqueNames.length; y++) {
                            filterByUser(uniqueNames[y]);
                        }

                    }
                    else if ($scope.pcfTypeGroupSelected === $scope.language.location.caption) {


                        var uniqueLocationNames = [];
                        for (i = 0; i < $scope.reportData.length; i++) {
                            if (uniqueLocationNames.indexOf($scope.reportData[i].locationName) === -1) {
                                uniqueLocationNames.push($scope.reportData[i].locationName);
                            }
                        }

                        for (y = 0; y < uniqueLocationNames.length; y++) {
                            filterByLocation(uniqueLocationNames[y]);
                        }

                    }
                    else if ($scope.pcfTypeGroupSelected === $scope.language.plan.caption) {
                        uniqueNames = [];
                        for (i = 0; i < $scope.reportData.length; i++) {
                            if (uniqueNames.indexOf($scope.reportData[i].planName) === -1) {
                                uniqueNames.push($scope.reportData[i].planName);
                            }
                        }

                        for (y = 0; y < uniqueNames.length; y++) {
                            filterByPlan(uniqueNames[y]);
                        }

                    }
                    else {

                        var chart = document.createElement("div");
                        chart.id = $scope.language.summary.caption;
                        
                        document.getElementById("chartDiv").appendChild(chart);
  
                        let total = $scope.reportData.length;


                        if (total  > 0) {

                            $scope.idList.push(c3.generate(returnChartDataColums($scope.fails.length, $scope.pass.length, $scope.caution.length, $scope.retest.length, chart.id, chartTypeName())));
                        }
                    }
                }
                filterReports();
                return null;
            }


            return null;
        }

        function returnChartDataColums(fails, pass, caution, retest, id, type) {

          
            let result = {
                bindto: `#${id}`,
                padding: {
                    top: 20,
                    right: 50,
                    bottom: 10,
                    left:30
                },
                size: {
                    width:540
                },
                data: {
                    type: type,
                    empty: {
                        label: {
                            text: $scope.language.noDataAvailable.caption 
                        }
                    }
                },
                title: {
                    text: id.replace("_id_", "").replace("_", "")
                },
                color:
                {
                    pattern: ["red", "green", "orange", "pink", "gray"]
                }
            }
  
            result.data.columns = [
                ["Fail", fails],
                ["Pass", pass],
                ["Caution", caution],
                ["Retest", retest]
            ];
            result.color.pattern = ["red", "green", "orange", "pink", "gray"];
            //
        
            return result;
        }

        function setDefaults() {
            try {
                $scope.title = "reportController";

                if (typeof $scope.language !== "undefined" && $scope.language !== null) {

                    $scope.reportTypes = [
                        { id: 0, report: $scope.language.passCautionFail.caption },
                        { id: 1, report: $scope.language.fail.caption },
                        { id: 2, report: $scope.language.pass.caption },
                        { id: 3, report: $scope.language.retest.caption },
                        { id: 4, report: $scope.language.caution.caption }
                        
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

                } else if (period === hygEnum.periodsEnum.LastMonth) {
                    $scope.datePickerFromDate = moment().subtract(1, "month").startOf("month")
                        .startOf("day");
                    $scope.datePickerToDate = moment().subtract(1, "month").endOf("month").endOf("day");
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.LastMonth];

                } else if (period === hygEnum.periodsEnum.LastYear) {
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

                } else if (period === hygEnum.periodsEnum.ThisWeek) {
                    $scope.datePickerFromDate = moment().startOf("week").startOf("day");
                    $scope.datePickerToDate = moment().endOf("day");
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.ThisWeek];

                } else if (period === hygEnum.periodsEnum.ThisMonth) {
                    $scope.datePickerFromDate = moment().startOf("month").startOf("day");
                    $scope.datePickerToDate = moment().endOf("day");
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.ThisMonth];

                } else if (period === hygEnum.periodsEnum.ThisYear) {
                    $scope.datePickerFromDate = moment().startOf("year").startOf("month").startOf("day");
                    $scope.datePickerToDate = moment().endOf("day");
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.ThisYear];

                } else if (period === hygEnum.periodsEnum.Today) {
                    $scope.datePickerFromDate = moment().startOf("day");
                    $scope.datePickerToDate = moment().endOf("day");
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.Today];

                } else if (period === hygEnum.periodsEnum.Yesterday) {
                    $scope.datePickerFromDate = moment().subtract(1, "day").startOf("day");
                    $scope.datePickerToDate = moment().subtract(1, "day").endOf("day");
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.Yesterday];

                } else if (period === hygEnum.periodsEnum.Custom) {
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.Custom];

                }

            }
        }

        function chartTypeName() {

            if ($scope.chatTypeSelected === $scope.language.pieChart.caption) {
                return "pie";
            } else if ($scope.chatTypeSelected === $scope.language.barChart.caption) {
                return"bar";

            } else if ($scope.chatTypeSelected === $scope.language.donutChart.caption ) {
                return"donut";
            }
            //else {
            //    return"line";
            //}
            return "pie";
        };

    });