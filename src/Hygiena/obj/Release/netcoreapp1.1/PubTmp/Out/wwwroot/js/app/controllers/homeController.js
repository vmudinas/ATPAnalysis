app.controller("homeController",
    function($rootScope,
        $scope,
        homeService,
        periodsArray,
        hygEnum,
        language,
        languageService,
        userService,
        userSettings) {

        init();

        $scope.updateDashBoard = function(selectedFailedBy) {
            $scope.selectedFailedBy = selectedFailedBy;
            try {
                if (typeof $scope.language !== "undefined") {

                    loadFailGrid();
                    getAllByLocation();
                }
            } catch (e) {
                console.log("Failed on currentDashBoard");
            }
        };
        $scope.updateDashBoardName = function(dashName) {
            try {
                if (typeof $scope.language !== "undefined") {
                    $scope.currentDashBoard = dashName;
                    loadFailGrid();
                    getAllByLocation();
                }
            } catch (e) {
                console.log("Failed on currentDashBoard");
            }
        };

        function updatePeriod(period) {
            if (typeof period !== "undefined") {
                if (period === hygEnum.periodsEnum.LastWeek) {
                    $scope.datePickerFromDate = moment().subtract(1, "week").startOf("week").startOf("day").format();
                    $scope.datePickerToDate = moment().subtract(1, "week").endOf("week").endOf("day").format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.LastWeek];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.LastMonth) {
                    $scope.datePickerFromDate = moment().subtract(1, "month").startOf("month").startOf("day").format();
                    $scope.datePickerToDate = moment().subtract(1, "month").endOf("month").endOf("day").format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.LastMonth];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.LastYear) {
                    $scope.datePickerFromDate = moment().subtract(1, "year").startOf("year").startOf("month")
                        .startOf("day").format();
                    $scope.datePickerToDate = moment().subtract(1, "year").endOf("year").endOf("month").endOf("day")
                        .format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.LastYear];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.ThisWeek) {
                    $scope.datePickerFromDate = moment().startOf("week").startOf("day").format();
                    $scope.datePickerToDate = moment().endOf("day").format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.ThisWeek];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.ThisMonth) {
                    $scope.datePickerFromDate = moment().startOf("month").startOf("day").format();
                    $scope.datePickerToDate = moment().endOf("day").format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.ThisMonth];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.ThisYear) {
                    $scope.datePickerFromDate = moment().startOf("year").startOf("month").startOf("day").format();
                    $scope.datePickerToDate = moment().endOf("day").format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.ThisYear];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.Today) {
                    $scope.datePickerFromDate = moment().startOf("day").format();
                    $scope.datePickerToDate = moment().endOf("day").format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.Today];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.Yesterday) {
                    $scope.datePickerFromDate = moment().subtract(1, "day").startOf("day").format();
                    $scope.datePickerToDate = moment().subtract(1, "day").endOf("day").format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.Yesterday];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.ThisQuarter) {
                    var currQtr = moment().quarter();
                    var monthOffset;

                    if (currQtr === 1) {
                        monthOffset = 0;
                    } else if (currQtr === 2) {
                        monthOffset = 3;
                    } else if (currQtr === 3) {
                        monthOffset = 6;
                    } else if (currQtr === 4) {
                        monthOffset = 9;
                    }

                    var qtrFromDate = moment([moment().year(), monthOffset]);
                    $scope.datePickerFromDate = moment(qtrFromDate).startOf("month").startOf("day").format();

                    $scope.datePickerToDate = moment().endOf("day").format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.ThisQuarter];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.LastQuarter) {
                    var currQtr = moment().quarter();
                    var fromMonthOffset, toMonthOffset;
                    var yearOffset = moment().year();

                    if (currQtr === 1) {
                        yearOffset--;
                        fromMonthOffset = 9;
                    } else if (currQtr === 2) {
                        fromMonthOffset = 0;
                    } else if (currQtr === 3) {
                        fromMonthOffset = 3;
                    } else if (currQtr === 4) {
                        fromMonthOffset = 6;
                    }

                    toMonthOffset = fromMonthOffset + 2;

                    var qtrFromDate = moment([yearOffset, fromMonthOffset]);
                    var qtrToDate = moment([yearOffset, toMonthOffset]);

                    $scope.datePickerFromDate = moment(qtrFromDate).startOf("month").startOf("day").format();
                    $scope.datePickerToDate = moment(qtrToDate).endOf("month").endOf("day").format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.LastQuarter];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.Custom) {
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.Custom];

                    if (userSettings.dashDateFrom !== null &&
                        userSettings.dashDateTo !== null &&
                        typeof userSettings.dashDateFrom !== "undefined" &&
                        typeof userSettings.dashDateTo !== "undefined") {
                        $scope.datePickerFromDate = moment.utc(userSettings.dashDateFrom).format();
                        $scope.datePickerToDate = moment.utc(userSettings.dashDateTo).format();
                    }
                }

                userSettings.dashPeriod = period;
                userSettings.dashDateFrom = $scope.datePickerFromDate;
                userSettings.dashDateTo = $scope.datePickerToDate;
                updateUserSettings();

            }
        }

        $scope.changePeriod =
            function(e) {
                updatePeriod(e.id);
            };

        $scope.$watch("selectedFailedBy",
            function(e) {

                updateFailedBy(e);
            });


        function updateFailedBy(e) {

            try {
                if (typeof $scope.language !== "undefined") {
                    if (typeof e !== "undefined" && e !== null) {
                        $scope.failsGridColumns = [];
                        $scope.failGridColumns = [];

                        userSettings.dashType = e.id;


                        if (e.id === $scope.dashEnum.dashType.FailsByLoc) {
                            $scope.failsGridColumns = homeService.getFailsByLocationColumns($scope.language);
                            $scope.failGridColumns = [
                                {
                                    caption: toTitleCase($scope.language.resultDate.caption),
                                    dataField: "resultDate",
                                    dataType: "date",
                                    format: userSettings.dateFormatShort,
                                    allowSorting: true,
                                    width: "auto"
                                },
                                {
                                    caption: toTitleCase($scope.language.rLU.caption),
                                    dataField: "rlu",
                                    allowSorting: true
                                },
                                {
                                    caption: toTitleCase($scope.language.upper.caption),
                                    dataField: "upper",
                                    allowSorting: true
                                }
                            ];
                        } else {
                            $scope.failsGridColumns = homeService.getFailsByPlanColumns($scope.language);
                            $scope.failGridColumns = [
                                {
                                    caption: toTitleCase($scope.language.location.caption),
                                    dataField: "locationName",
                                    allowSorting: true,
                                    width: "auto"
                                },
                                {
                                    caption: toTitleCase($scope.language.pass.caption),
                                    dataField: "pass",
                                    allowSorting: true,
                                    width: "auto"
                                },
                                {
                                    caption: toTitleCase($scope.language.caution.caption),
                                    dataField: "caution",
                                    allowSorting: true
                                },
                                {
                                    caption: toTitleCase($scope.language.fail.caption),
                                    dataField: "fail",
                                    allowSorting: true
                                }
                            ];
                        }

                        updateUserSettings();
                    }
                }
            } catch (e) {
                console.log("Failed on selectedFailedBy");
            }
        }

        function init() {
            $scope.loaded = false;
            $scope.language = language[0];

            if (typeof $scope.language !== "undefined") {
                try {
                    $scope.dashEnum = {};
                    $scope.dashEnum.dashType = { "FailsByLoc": 1, "FailsByPlan": 2, "TBD": 3 };
                    $scope.chartData = {};
                    $scope.chartPeriodData = {};
                    $scope.failDataSource = {};
                    $scope.failGridColumns = {};
                    $scope.failsGridColumns = {};
                    $scope.failsdataSource = {};
                    $scope.trendChartData = {};
                    $scope.dateBox = {};
                    $scope.periods = periodsArray;
                    $scope.commonSeriesSettings = {};
                    $scope.argumentAxis = {};
                    $scope.trendSeries = {};
                    $scope.trendChartOptions2 = {};
                    $scope.trendChartOptions = {};
                    $scope.chartPeriodOptions = {};
                    $scope.failGridOptions = {};
                    $scope.failsGridOptions = {};
                    $scope.datePickerToDate = new Date();
                    $scope.datePickerFromDate = new Date();
                    $scope.trendChartData2 = {};

                    $scope.dateBox = {
                        dateTimeFrom: {
                            type: "datetime",
                            pickerType: "rollers",
                            min: new Date(1985, 0, 1),
                            value: $scope.datePickerFromDate,
                            bindingOptions: { value: "datePickerFromDate" },
                            onClosed: function(data) {

                                $scope.selectedP = periodsArray[hygEnum.periodsEnum.Custom];
                                data.model.selectedP = periodsArray[hygEnum.periodsEnum.Custom];
                                userSettings.dashPeriod = hygEnum.periodsEnum.Custom;
                                userSettings.dashDateFrom = data.model.datePickerFromDate;
                                updateFailedBy($scope.selectedFailedBy);

                            }
                        },
                        dateTimeTo: {
                            type: "datetime",
                            pickerType: "rollers",
                            value: $scope.datePickerToDate,
                            bindingOptions: { value: "datePickerToDate" },
                            onClosed: function(data) {

                                data.model.selectedP = periodsArray[hygEnum.periodsEnum.Custom];
                                $scope.selectedP = periodsArray[hygEnum.periodsEnum.Custom];
                                userSettings.dashPeriod = hygEnum.periodsEnum.Custom;
                                userSettings.dashDateTo = data.model.datePickerToDate;
                                updateFailedBy($scope.selectedFailedBy);
                            },
                            applyValueMode: "useButtons"
                        }
                    };
                    $scope.commonSeriesSettings = {
                        argumentField: "resultDate",
                        valueField: "number",
                        ignoreEmptyPoints: true
                    };
                    $scope.argumentAxis = {
                            valueMarginsEnabled: false,
                            grid: { visible: false },
                            label: {
                                overlappingBehavior: "hidessss",
                                customizeText: function(e) {
                                    return moment(e.value).format("lll");
                                }
                            }
                        },
                        $scope.trendSeries = [
                            {
                                valueField: "rlu",
                                color: "purple",
                                type: "scatter",
                                name: "RLU",
                                point: { visible: false }
                            },
                            {
                                valueField: "lower",
                                color: "green",
                                name: "Lower",
                                type: "line",
                                point: { visible: false }
                            },
                            {
                                valueField: "upper",
                                name: "Upper",
                                color: "red",
                                type: "line",
                                point: { visible: false }
                            }
                        ];

                    $scope.trendChartOptions2 = {
                        bindingOptions: {
                            dataSource: "trendChartData2",
                            series: "trendChartSeries2"
                        },
                        size: { height: 250 },
                        commonSeriesSettings: {
                            argumentField: "locationName",
                            type: "stackedBar"
                        }
                    };

                    $scope.trendChartOptions = {
                        bindingOptions: {
                            dataSource: "trendChartData",
                            series: "trendSeries",
                            commonSeriesSettings: "commonSeriesSettings",
                            argumentAxis: "argumentAxis"
                        },
                        size: { height: 250 },
                        tooltip: {
                            enabled: true,
                            location: "edge",
                            customizeTooltip: function(arg) {
                                var seriesNameTranslated = "";

                                if (typeof arg.seriesName !== "undefined" && arg.seriesName !== null) {
                                    if (arg.seriesName === "Lower") {
                                        seriesNameTranslated = toTitleCase($scope.language.lower.caption);
                                    } else if (arg.seriesName === "Upper") {
                                        seriesNameTranslated = toTitleCase($scope.language.upper.caption);
                                    } else {
                                        seriesNameTranslated = arg.seriesName;
                                    }
                                }

                                return {
                                    text: seriesNameTranslated +
                                        " - " +
                                        arg.value +
                                        " - " +
                                        moment(arg.argument).format("LLL")
                                };
                            }
                        },
                        customizePoint: function(e) {
                            var x = $.grep($scope.trendChartData,
                                function(n, i) {
                                    return n.resultDate === e.argument;
                                });

                            if (e.seriesName === "RLU") {
                                if (e.value === 0) {
                                    return { visible: false };
                                } else if (e.value > x[0].upper) {
                                    return { color: "red", visible: true };
                                } else if (e.value <= x[0].lower) {
                                    return { color: "green", visible: true };
                                } else {
                                    return { color: "orange", visible: true };
                                }
                            }
                            return { visible: false };
                        },
                        customizeLabel: function(e) {
                            if (this.seriesName === "Lower" && this.index === 0) {
                                return {
                                    visible: true,
                                    backgroundColor: "green",
                                    customizeText: function() {
                                        return toTitleCase($scope.language.lower.caption);
                                    }
                                };
                            } else if (this.seriesName === "Upper" && this.index === 0) {
                                return {
                                    visible: true,
                                    backgroundColor: "red",
                                    customizeText: function() {
                                        return toTitleCase($scope.language.upper.caption);
                                    }
                                };
                            }

                            return { visible: false };
                        },
                        legend: { visible: false },
                        label: { visible: true },
                        valueAxis: {
                            title: { text: toTitleCase($scope.language.rLuValues.caption) },
                            position: "left"
                        }
                    };

                    $scope.chartOptions = {
                        bindingOptions: { dataSource: "chartData" },
                        height: 250,
                        size: { width: "auto" },
                        palette: "bright",
                        startAngle: 90,
                        series: [
                            {
                                argumentField: "result",
                                valueField: "area",
                                label: {
                                    visible: true,
                                    font: { size: 12 },
                                    connector: {
                                        visible: true,
                                        width: 0.5
                                    },
                                    position: "columns",
                                    customizeText: function(arg) {
                                        return window
                                            .toTitleCase($scope.language[arg.argumentText.toLowerCase()].caption) +
                                            "\n" +
                                            arg.percentText;
                                    }
                                }
                            }
                        ],
                        legend: { visible: false },
                        customizePoint: function(point) {
                            return {
                                color: $scope.chartData[point.index].color
                            };
                        }
                    };

                    $scope.chartPeriodOptions = {
                        bindingOptions: { dataSource: "chartPeriodData" },
                        height: 250,
                        size: { width: "auto" },
                        startAngle: 90,
                        palette: "bright",
                        series: [
                            {
                                argumentField: "result",
                                valueField: "area",
                                label: {
                                    visible: true,
                                    font: { size: 12 },
                                    connector: {
                                        visible: true,
                                        width: 0.5
                                    },
                                    position: "columns",
                                    customizeText: function(arg) {
                                        return toTitleCase($scope.language[arg.argumentText.toLowerCase()].caption) +
                                            "\n" +
                                            arg.percentText;
                                    }
                                }
                            }
                        ],
                        legend: { visible: false },
                        customizePoint: function(point) {
                            return {
                                color: $scope.chartPeriodData[point.index].color
                            };
                        }
                    };

                    $scope.failGridOptions = {
                        bindingOptions: {
                            dataSource: "failDataSource",
                            columns: "failGridColumns"
                        },
                        showBorders: true,
                        showColumnLines: true,
                        showRowLines: true,
                        rowAlternationEnabled: true,
                        loadPanel: { enabled: false },
                        scrolling: { mode: "virtual" },
                        selection: { mode: "single" },
                        height: 350,
                        hoverStateEnabled: true
                    };

                    $scope.failsGridOptions = {
                        bindingOptions: {
                            dataSource: "failsdataSource",
                            columns: "failsGridColumns"
                        },
                        showBorders: true,
                        showColumnLines: true,
                        showRowLines: true,
                        rowAlternationEnabled: true,
                        loadPanel: { enabled: false },
                        scrolling: { mode: "virtual" },
                        selection: { mode: "single" },
                        height: 350,
                        hoverStateEnabled: true,
                        onSelectionChanged: function(selectedItems) {
                            if (selectedItems.selectedRowsData.length > 0) {
                                if (selectedItems.selectedRowsData[0].location !== undefined) {
                                    $scope.updateDashBoardName(selectedItems.selectedRowsData[0].location);
                                    getAllByLocation();
                                } else if (selectedItems.selectedRowsData[0].plan !== undefined) {
                                    $scope.updateDashBoardName(selectedItems.selectedRowsData[0].plan);
                                    getAllByLocation();
                                } else {
                                    $scope.updateDashBoardName("No Data");
                                }
                            } else {
                                $scope.updateDashBoardName("No Data");
                            }
                        },
                        onContentReady: function(e) {
                            e.component.selectRowsByIndexes([0], true);
                        }
                    };

                    $scope.trendChartSeries2 = [
                        { valueField: "caution", name: toTitleCase($scope.language.caution.caption), color: "orange" },
                        { valueField: "pass", name: toTitleCase($scope.language.pass.caption), color: "green" },
                        { valueField: "fail", name: toTitleCase($scope.language.fail.caption), color: "red" }
                    ];

                    userService.getUserSettings().then(function(response) {
                            Object.assign(userSettings, response.data);

                            setDefaults();

                            $scope.loaded = true;
                        },
                        function(response) {

                            console.log(response);
                        }
                    );


                } catch (e) {
                    console.log(e);
                    console.log("Error on init");
                }
            }
        } // end Init()

        function setDefaults() {
            try {
                $scope.title = "homeController";

                if (userSettings.dashPeriod === null || typeof userSettings.dashPeriod === "undefined") {
                    userSettings.dashPeriod = hygEnum.periodsEnum.LastYear;
                }

                // if customer, dates should also exist, but just in case they don't
                if (userSettings.dashPeriod === hygEnum.periodsEnum.Custom &&
                    (userSettings.dashDateFrom === null || userSettings.dashDateTo === null)) {
                    userSettings.dashDateFrom = moment().startOf("day");
                    userSettings.dashDateTo = moment().endOf("day");
                }

                if (userSettings.dashType === null || typeof userSettings.dashType === "undefined") {
                    userSettings.dashType = $scope.dashEnum.dashType.FailsByLoc;
                }

                if (typeof $scope.language !== "undefined" && $scope.language !== null) {
                    $scope.failsBy = [
                        { id: 1, dashboard: toTitleCase($scope.language.failsByLocation.caption) },
                        { id: 2, dashboard: toTitleCase($scope.language.failsByPlan.caption) }
                    ];

                    $scope.selectedFailedBy = $scope.failsBy[(userSettings.dashType - 1)];
                    languageService.translatePeriods(periodsArray, $scope.language, hygEnum);
                    $scope.periods = periodsArray;
                }

                getGridData();

                updatePeriod(userSettings.dashPeriod);

            } catch (e) {
                console.log("Set Defaults Error");
                console.log(e);
            }
        }

        function getGridData() {
            try {

                if (typeof $scope.selectedFailedBy !== "undefined" && $scope.selectedFailedBy !== null) {
                    // the variable is defined
                    homeService.getFails($scope.selectedFailedBy.id,
                            moment.utc($scope.datePickerFromDate).format(),
                            moment.utc($scope.datePickerToDate).format())
                        .then(function(response) {
                                if (response.data === "") {
                                    $scope.failsdataSource = [];
                                } else {
                                    $scope.failsdataSource = response.data;
                                }
                            },
                            function(response) {
                                $scope.failsdataSource = [];
                                console.log(response);
                            }
                        );
                }
            } catch (e) {
                console.log("Get Grid Data Error");
                console.log(e);
            }
        }

        function getAllByLocation() {
            try {


                if (typeof $scope.selectedFailedBy !== "undefined" && $scope.selectedFailedBy !== null) {
                    if ($scope.selectedFailedBy.id === $scope.dashEnum.dashType.FailsByLoc) {
                        homeService.getAllByLocation($scope.currentDashBoard,
                                moment.utc($scope.datePickerFromDate).format(),
                                moment.utc($scope.datePickerToDate).format())
                            .then(function(response) {
                                    if (response.data === "") {
                                        $scope.trendChartData2 = [];
                                        $scope.trendChartData = [];
                                    } else {
                                        $scope.trendChartData = response.data;
                                    }
                                },
                                function(response) {
                                    $scope.trendChartData = [];
                                    $scope.trendChartData2 = [];
                                    console.log(response);
                                }
                            );
                    } else {
                        // Implement Trend by plan
                        homeService.getFailByPlan($scope.currentDashBoard,
                                moment.utc($scope.datePickerFromDate).format(),
                                moment.utc($scope.datePickerToDate).format())
                            .then(function(response) {
                                    $scope.trendChartData = [];
                                    if (response.data === "") {
                                        $scope.trendChartData2 = [];
                                    } else {
                                        $scope.trendChartData2 = response.data;
                                    }
                                },
                                function(response) {
                                    console.log(response);
                                    $scope.trendChartData = [];
                                    $scope.trendChartData2 = [];
                                }
                            );
                    }
                }
            } catch (e) {
                console.log("Get all by location Error");
            }
        }

        function loadFailGrid() {
            try {

                if (typeof $scope.selectedFailedBy !== "undefined" && $scope.selectedFailedBy !== null) {
                    try {
                        if ($scope.selectedFailedBy.id === $scope.dashEnum.dashType.FailsByPlan) {
                            homeService.getFailByPlan($scope.currentDashBoard,
                                    moment.utc($scope.datePickerFromDate).format(),
                                    moment.utc($scope.datePickerToDate).format())
                                .then(function(response) {
                                        if (response.data === "") {
                                            $scope.failDataSource = [];
                                        } else {
                                            $scope.failDataSource = response.data;
                                            loadPassCautionFailChart();
                                        }
                                    },
                                    function(response) {
                                        $scope.failDataSource = [];
                                        console.log(response);
                                    }
                                );
                        } else {
                            homeService.getFailByLocation($scope.currentDashBoard,
                                    moment.utc($scope.datePickerFromDate).format(),
                                    moment.utc($scope.datePickerToDate).format())
                                .then(function(response) {
                                        if (response.data === "") {
                                            $scope.failDataSource = [];
                                        } else {
                                            $scope.failDataSource = response.data;
                                            loadPassCautionFailChart();
                                        }
                                    },
                                    function(response) {
                                        $scope.failDataSource = [];
                                        console.log(response);
                                    }
                                );
                        }

                        loadPassCautionFailChart();
                        loadPeriodChart();
                    } catch (e) {
                        console.log(e);
                    }
                }
            } catch (e) {
                console.log("loadFailGrid");
            }
        }

        function setDates() {
            $scope.datePickerFromDate = moment().startOf("day");
            $scope.datePickerToDate = moment().endOf("day");
        }

        function loadPeriodChart() {

            homeService.getFailsByPeriodChart(moment.utc($scope.datePickerFromDate).format(),
                    moment.utc($scope.datePickerToDate).format())
                .then(function(response) {
                        if (response.data === "") {
                            $scope.chartPeriodData = [];
                        } else {
                            $scope.chartPeriodData = response.data;
                        }
                    },
                    function(response) {
                        console.log("fail on loadPeriodChart");
                        $scope.chartPeriodData = [];
                        console.log(response);
                    }
                );
        }

        function loadPassCautionFailChart() {

            if ($scope.selectedFailedBy.id === $scope.dashEnum.dashType.FailsByPlan) {
                homeService.getFailsByPlanChart($scope.currentDashBoard,
                        moment.utc($scope.datePickerFromDate).format(),
                        moment.utc($scope.datePickerToDate).format())
                    .then(function(response) {
                            if (response.data === "") {
                                $scope.chartData = [];
                            } else {
                                $scope.chartData = response.data;
                            }
                        },
                        function(response) {
                            $scope.chartData = [];
                            console.log("fail on getFailsByPlanChart");
                            console.log(response);
                        }
                    );
            } else {
                homeService.getFailsByLocationChart($scope.currentDashBoard,
                        moment.utc($scope.datePickerFromDate).format(),
                        moment.utc($scope.datePickerToDate).format())
                    .then(function(response) {
                            if (response.data === "") {
                                $scope.chartData = [];
                            } else {
                                $scope.chartData = response.data;
                            }
                        },
                        function(response) {
                            $scope.chartData = [];
                            console.log("fail on getFailsByLocationChart");
                            console.log(response);
                        }
                    );
            }
        }

        function updateUserSettings() {


            userService.updateUserSettings("dash",
                userSettings,
                moment.utc(userSettings.dashDateFrom).format(),
                moment.utc(userSettings.dashDateTo).format()
            ).then(function(response) {
                    getGridData();
                },
                function(response) {

                    console.log(response);
                }
            );

        }
    });