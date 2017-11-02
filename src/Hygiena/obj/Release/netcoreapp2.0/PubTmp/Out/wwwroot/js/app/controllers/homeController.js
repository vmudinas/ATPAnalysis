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

                    if (selectedFailedBy.id === 2) {

                        $scope.failsdataSource = $scope.failsByPlanArray;
                    } else {
                        $scope.failsdataSource = $scope.failsByLocationArray;
                    }
                    // Generate Chart By Plan Or Location 
                }
            } catch (e) {
                console.log("Failed on currentDashBoard");
            }
        };
        $scope.updateDashBoardName = function (dashName) {
     
            try {
               
                if (typeof $scope.language !== "undefined") {
                    $scope.currentDashBoard = dashName;
              
                }
            } catch (e) {
                console.log("Failed on currentDashBoard");
            }
        };

        function updatePeriod(period) {

          
            if (typeof period !== "undefined") {
                if (period === hygEnum.periodsEnum.LastWeek) {
                    $scope.datePickerFromDate = window.moment().subtract(1, "week").startOf("week").startOf("day").format();
                    $scope.datePickerToDate = window.moment().subtract(1, "week").endOf("week").endOf("day").format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.LastWeek];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.LastMonth) {
                    $scope.datePickerFromDate = window.moment().subtract(1, "month").startOf("month").startOf("day").format();
                    $scope.datePickerToDate = window.moment().subtract(1, "month").endOf("month").endOf("day").format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.LastMonth];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.LastYear) {
                    $scope.datePickerFromDate = window.moment().subtract(1, "year").startOf("year").startOf("month")
                        .startOf("day").format();
                    $scope.datePickerToDate = window.moment().subtract(1, "year").endOf("year").endOf("month").endOf("day")
                        .format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.LastYear];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.ThisWeek) {
                    $scope.datePickerFromDate = window.moment().startOf("week").startOf("day").format();
                    $scope.datePickerToDate = window.moment().endOf("day").format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.ThisWeek];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.ThisMonth) {
                    $scope.datePickerFromDate = window.moment().startOf("month").startOf("day").format();
                    $scope.datePickerToDate = window.moment().endOf("day").format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.ThisMonth];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.ThisYear) {
                    $scope.datePickerFromDate = window.moment().startOf("year").startOf("month").startOf("day").format();
                    $scope.datePickerToDate = window.moment().endOf("day").format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.ThisYear];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.Today) {
                    $scope.datePickerFromDate = window.moment().startOf("day").format();
                    $scope.datePickerToDate = window.moment().endOf("day").format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.Today];
                    updateFailedBy($scope.selectedFailedBy);
                } else if (period === hygEnum.periodsEnum.Yesterday) {
                    $scope.datePickerFromDate = window.moment().subtract(1, "day").startOf("day").format();
                    $scope.datePickerToDate = window.moment().subtract(1, "day").endOf("day").format();
                    $scope.selectedP = periodsArray[hygEnum.periodsEnum.Yesterday];
                    updateFailedBy($scope.selectedFailedBy);
                } else {
                    let currQtr;
                    var qtrFromDate;
                    if (period === hygEnum.periodsEnum.ThisQuarter) {
                        currQtr = window.moment().quarter();
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

                        qtrFromDate = window.moment([window.moment().year(), monthOffset]);
                        $scope.datePickerFromDate = window.moment(qtrFromDate).startOf("month").startOf("day").format();

                        $scope.datePickerToDate = window.moment().endOf("day").format();
                        $scope.selectedP = periodsArray[hygEnum.periodsEnum.ThisQuarter];
                        updateFailedBy($scope.selectedFailedBy);
                    } else if (period === hygEnum.periodsEnum.LastQuarter) {
                        currQtr = window.moment().quarter();
                        var fromMonthOffset, toMonthOffset;
                        var yearOffset = window.moment().year();

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

                        qtrFromDate = window.moment([yearOffset, fromMonthOffset]);
                        var qtrToDate = window.moment([yearOffset, toMonthOffset]);

                        $scope.datePickerFromDate = window.moment(qtrFromDate).startOf("month").startOf("day").format();
                        $scope.datePickerToDate = window.moment(qtrToDate).endOf("month").endOf("day").format();
                        $scope.selectedP = periodsArray[hygEnum.periodsEnum.LastQuarter];
                        updateFailedBy($scope.selectedFailedBy);
                    } else if (period === hygEnum.periodsEnum.Custom) {
                        $scope.selectedP = periodsArray[hygEnum.periodsEnum.Custom];

                        if (userSettings.dashDateFrom !== null &&
                            userSettings.dashDateTo !== null &&
                            typeof userSettings.dashDateFrom !== "undefined" &&
                            typeof userSettings.dashDateTo !== "undefined") {
                            $scope.datePickerFromDate = window.moment.utc(userSettings.dashDateFrom).format();
                            $scope.datePickerToDate = window.moment.utc(userSettings.dashDateTo).format();
                        }
                    }
                }

                userSettings.dashPeriod = period;
                userSettings.dashDateFrom = $scope.datePickerFromDate;
                userSettings.dashDateTo = $scope.datePickerToDate;
                updateUserSettings();

                //Get All data 

                var opts = {
                    lines: 13 // The number of lines to draw
                    , length: 28 // The length of each line
                    , width: 14 // The line thickness
                    , radius: 42 // The radius of the inner circle
                    , scale: 1 // Scales overall size of the spinner
                    , corners: 1 // Corner roundness (0..1)
                    , color: '#000' // #rgb or #rrggbb or array of colors
                    , opacity: 0.25 // Opacity of the lines
                    , rotate: 0 // The rotation offset
                    , direction: 1 // 1: clockwise, -1: counterclockwise
                    , speed: 1 // Rounds per second
                    , trail: 60 // Afterglow percentage
                    , fps: 20 // Frames per second when using setTimeout() as a fallback for CSS
                    , zIndex: 2e9 // The z-index (defaults to 2000000000)
                    , className: 'spinner' // The CSS class to assign to the spinner
                    , top: '50%' // Top position relative to parent
                    , left: '50%' // Left position relative to parent
                    , shadow: false // Whether to render a shadow
                    , hwaccel: false // Whether to use hardware acceleration
                    , position: 'absolute' // Element positioning
                };
                var target = document.getElementById('idSpinnerGrid');
                var spinner = new Spinner(opts).spin(target);


                $scope.allData = "";
                 homeService.getAllHomeData(window.moment.utc($scope.datePickerFromDate).format(),
                 window.moment.utc($scope.datePickerToDate).format()).then(function (response) {

                     $scope.allData = response.data;
                    // $scope.loaded = true;
                
                     spinner.stop();

                         if ($scope.allData !== undefined) {

                             if ($scope.allData !== "" || $scope.allData.lenght > 0) {

                                 // Location id 1 
                                 // Plan id 2 
                                 let groupBy = "locationName";
                                 if ($scope.selectedFailedBy.id === 2) {
                                     groupBy = "planName";
                                 }

                                 const fliteredByLocation = $scope.allData.filter(filter => filter.rlu > filter.upper).reduce(function (groups, item) {
                                     const val = item["locationName"];
                                     groups[val] = groups[val] || [];
                                     groups[val].push(item);
                                     return groups;
                                 }, {});
                                 const fliteredByPlan = $scope.allData.filter(filter => filter.rlu > filter.upper).reduce(function (groups, item) {
                                     const val = item["planName"];
                                     groups[val] = groups[val] || [];
                                     groups[val].push(item);
                                     return groups;
                                 }, {});
                                 
                                 $scope.failsByLocationArray = [];
                                 $scope.failsByPlanArray = [];


                                 Object.keys(fliteredByPlan).forEach(function(key) {

                                     $scope.failsByPlanArray.push({
                                         plan: key,
                                         numberOfFails: fliteredByPlan[key].length
                                     });
                                 });
                                 Object.keys(fliteredByLocation).forEach(function (key) {

                                     $scope.failsByLocationArray.push({
                                         location: key,
                                         numberOfFails: fliteredByLocation[key].length
                                     });
                              
                                 });

                                 $scope.failsByLocationArray = $scope.failsByLocationArray.sort(function (a, b) {
                                     return b.numberOfFails - a.numberOfFails;
                                 });
                                 $scope.failsByPlanArray = $scope.failsByPlanArray.sort(function (a, b) {
                                     return b.numberOfFails - a.numberOfFails;
                                 });

                                 if (groupBy === "locationName") {
                                     $scope.failsdataSource = $scope.failsByLocationArray;
                                 } else {
                                     $scope.failsdataSource = $scope.failsByPlanArray;
                                 }


                                 // Generate Period Chart
                                 const failed = $scope.allData.filter(filter => filter.rlu > filter.upper).length;
                                 const pass = $scope.allData.filter(filter => filter.rlu <= filter.lower).length;
                                 const caution = $scope.allData.filter(filter => filter.rlu <= filter.upper && filter.rlu > filter.lower).length;

                                 //

                                 c3.generate(returnChartDataColums(failed, pass, caution, "idpieByPeriod", "pie")); 


                             }
                         }
                 },
                 function (response) {
                     console.log(response);
                 }
             );
            }
        }

        $scope.changePeriod =
            function (e) {
                updatePeriod(e.id);
            };

        $scope.$watch("selectedFailedBy",
            function (e) {
            
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
                        
                        } else {
                            $scope.failsGridColumns = homeService.getFailsByPlanColumns($scope.language);
                         
                        }
                      
                        updateUserSettings();
                    }
                }
            } catch (e) {
                console.log("Failed on selectedFailedBy");
            }
        }

        function init() {
            $scope.selectedMenu = "home";
            $scope.loaded = false;
            $scope.language = language[0];

            if (typeof $scope.language !== "undefined") {
                try {
                    $scope.dashEnum = {};
                    $scope.dashEnum.dashType = { "FailsByLoc": 1, "FailsByPlan": 2, "TBD": 3 };
                
                    $scope.failDataSource = {};
                    $scope.failGridColumns = {};
                    $scope.failsGridColumns = {};
                    $scope.failsdataSource = {};                 
                    $scope.dateBox = {};
                    $scope.periods = periodsArray;                
                    $scope.failGridOptions = {};
                    $scope.failsGridOptions = {};
                    $scope.datePickerToDate = new Date();
                    $scope.datePickerFromDate = new Date();
                
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
                                updatePeriod(hygEnum.periodsEnum.Custom);

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
                                updatePeriod(hygEnum.periodsEnum.Custom);                           
                            },
                            applyValueMode: "useButtons"
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
                          

                                    $scope.failGridColumns = [
                                        {
                                            caption: window.toTitleCase($scope.language.resultDate.caption),
                                            dataField: "resultDate",
                                            dataType: "date",
                                            format: userSettings.dateFormatShort,
                                            allowSorting: true,
                                            width: "auto",
                                            alignment: "left"
                                        },
                                        {
                                            caption: window.toTitleCase($scope.language.rLU.caption),
                                            dataField: "rlu",
                                            allowSorting: true,
                                            alignment: "left"
                                        },
                                        {
                                            caption: window.toTitleCase($scope.language.upper.caption),
                                            dataField: "upper",
                                            allowSorting: true,
                                            alignment: "left"
                                        }
                                    ];
                                
                                    loadFailGrid();
                                
                                } else if (selectedItems.selectedRowsData[0].plan !== undefined) {
                                    $scope.updateDashBoardName(selectedItems.selectedRowsData[0].plan);
                            
                                
                                    $scope.failGridColumns = [
                                        {
                                            caption: window.toTitleCase($scope.language.location.caption),
                                            dataField: "locationName",
                                            allowSorting: true,
                                            width: "auto",
                                            alignment: "left"
                                        },
                                        {
                                            caption: window.toTitleCase($scope.language.resultDate.caption),
                                            dataField: "resultDate",
                                            dataType: "date",
                                            format: userSettings.dateFormatShort,
                                            allowSorting: true,
                                            width: "auto",
                                            alignment: "left"
                                        },
                                        {
                                            caption: window.toTitleCase($scope.language.rLU.caption),
                                            dataField: "rlu",
                                            allowSorting: true,
                                            alignment: "left"
                                        },
                                        {
                                            caption: window.toTitleCase($scope.language.upper.caption),
                                            dataField: "upper",
                                            allowSorting: true,
                                            alignment: "left"
                                        }
                                     
                                    ];
                                    loadFailGrid();
                                
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

                 
                    userService.getUserSettings().then(function(response) {
                            Object.assign(userSettings, response.data);

                            setDefaults();

                           
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
        } 

        function setDefaults() {
            try {
                $scope.title = "homeController";

                if (userSettings.dashPeriod === null || typeof userSettings.dashPeriod === "undefined") {
                    userSettings.dashPeriod = hygEnum.periodsEnum.LastYear;
                }
         
                if (userSettings.dashPeriod === hygEnum.periodsEnum.Custom &&
                    (userSettings.dashDateFrom === null || userSettings.dashDateTo === null)) {
                    userSettings.dashDateFrom = window.moment().startOf("day");
                    userSettings.dashDateTo = window.moment().endOf("day");
                }

                if (userSettings.dashType === null || typeof userSettings.dashType === "undefined") {
                    userSettings.dashType = $scope.dashEnum.dashType.FailsByLoc;
                }

                if (typeof $scope.language !== "undefined" && $scope.language !== null) {
                    $scope.failsBy = [
                        { id: 1, dashboard: window.toTitleCase($scope.language.failsByLocation.caption) },
                        { id: 2, dashboard: window.toTitleCase($scope.language.failsByPlan.caption) }
                    ];

                    $scope.selectedFailedBy = $scope.failsBy[(userSettings.dashType - 1)];
                    languageService.translatePeriods(periodsArray, $scope.language, hygEnum);
                    $scope.periods = periodsArray;
                }

             //   getGridData();
                updatePeriod(userSettings.dashPeriod);

            } catch (e) {
                console.log("Set Defaults Error");
                console.log(e);
            }
        }

        function returnChartDataColums(fails, pass, caution, id, type) {

            const result = {
                bindto: `#${id}`,           
                data: {
                    type: type,
                    empty: {
                        label: {
                            text: $scope.language.noDataAvailable.caption
                        }
                    }
                },                     
                tooltip: {
                    show: true
                },
                color:
                {
                    pattern: ["red", "green", "orange"]
                }
            }
          
                result.data.columns = [
                    ["Fail", fails],
                    ["Pass", pass],
                    ["Caution", caution]
                ];
           
            return result;
        }

        function loadTrendChart(planName, locationName, id, type)
        {

            const tempArray = ['x'];
            const failArray = ['Fail'];
            const passArray = ['Pass'];
            const cautionArray = ['Caution'];
         


            const result = {
                bindto: `#${id}`,
                color:
                {
                    pattern: ["red", "green", "orange"]
                },
                data: {
                    empty: {
                        label: {
                            text: $scope.language.noDataAvailable.caption
                        }
                    }
                },
                point: {
                    show: false
                },              
                axis: {
                    x: {
                        type: 'category',
                        show: false
                    }                
                  
                },
                line: {
                    connectNull: true,
                },                  
                tooltip: {
                    show: true
                }
            };


            if (planName.length > 0) {


                const currentdata = $scope.allData.filter(filter => filter.planName === planName); 
              

                Object.keys(currentdata).forEach(function (key) {
                    tempArray.push(currentdata[key].locationName);

                    const failed = currentdata.filter(filter => filter.locationName === currentdata[key].locationName && filter.rlu > filter.upper).length;
                    const pass = currentdata.filter(filter => filter.locationName === currentdata[key].locationName && filter.rlu <= filter.lower).length;
                    const caution = currentdata.filter(filter => filter.locationName === currentdata[key].locationName && filter.rlu <= filter.upper && filter.rlu > filter.lower).length;

                    if (failed !== 0) {
                        failArray.push(failed);
                    }
                    else
                    {
                        failArray.push(null);
                    }
                    if (pass !== 0) {
                        passArray.push(pass);
                    }
                    else
                    {
                        passArray.push(null);
                    }

                    if (caution !== 0) {
                        cautionArray.push(caution);
                    }
                    else
                    {
                        cautionArray.push(null);
                    }                 
                  
                });           
                result.axis.y = {
                    label: "Pass, Caution, Fail "
                };
                result.data = {
                    x: 'x',
                    columns: [
                        tempArray,
                        failArray,
                        passArray,
                        cautionArray
                    ],
                    groups: [
                        ['Fail', 'Pass', 'Caution']
                    ],
                    type: type,
                };                

           
            }
            else
            {
   
                result.axis.y = {
                    label: "RLU"
                };

                const currentdata = $scope.allData.filter(filter => filter.locationName === locationName); 

                const upperArray = ['Upper'];
                const lowerArray = ['Lower'];
                var upper = 0;
                var lower = 0;
               

                tempArray.push(window.moment($scope.datePickerFromDate).format("LLL"));             

                upperArray.push(currentdata[0].upper);
                lowerArray.push(currentdata[0].lower);


                Object.keys(currentdata).forEach(function (key) {                 

                    tempArray.push(window.moment(currentdata[key].resultDate).format("LLL"));

                    var rlu = currentdata[key].rlu;
                    upper = currentdata[key].upper;
                    lower = currentdata[key].lower;


                 
                    upperArray.push(upper);
                    lowerArray.push(lower);

                    if (rlu > upper) {
                        failArray.push(rlu);
                        cautionArray.push(null);
                        passArray.push(null);
                    }
                    else if (rlu <= lower)
                    {
                        failArray.push(null);
                        cautionArray.push(null);
                        passArray.push(rlu);
                    }
                    else
                    {
                        failArray.push(null);
                        cautionArray.push(rlu);
                        passArray.push(null);
                    }                             


                });

 
                tempArray.push(window.moment($scope.datePickerToDate).format("LLL"));
                upperArray.push(upper);
                lowerArray.push(lower);
                failArray.push(null);
                cautionArray.push(null);
                passArray.push(null);


                result.data = { 
                    x: 'x',
                    columns: [
                        tempArray,
                        failArray,
                        passArray,
                        cautionArray,
                        upperArray,
                        lowerArray

                    ],           
                    type: 'scatter',
                    types: {

                        Upper: 'spline',
                        Lower: 'spline'
                    }
                };           

            }    

            return result;
        }

        function loadFailGrid() {
            try {
            

                if (typeof $scope.selectedFailedBy !== "undefined" && $scope.selectedFailedBy !== null) {
                    try {
                        if ($scope.selectedFailedBy.id === $scope.dashEnum.dashType.FailsByPlan) {

                        
                            const currentdata = $scope.allData.filter(filter => filter.planName === $scope.currentDashBoard); // && filter.rlu > filter.upper Show All not only fails
                  
                       
                            const tempArray = [];

                            Object.keys(currentdata).forEach(function (key) {
                                tempArray.push({
                                        locationName: currentdata[key].locationName,
                                        resultDate: currentdata[key].resultDate,
                                        rlu: currentdata[key].rlu,
                                        upper: currentdata[key].upper
                                    });
                            });

                            $scope.failDataSource = tempArray;

                            //Generate Pie Chart by Plan 

                            const failed = $scope.allData.filter(filter => filter.planName === $scope.currentDashBoard && filter.rlu > filter.upper).length;
                            const pass = $scope.allData.filter(filter => filter.planName === $scope.currentDashBoard && filter.rlu <= filter.lower).length;
                            const caution = $scope.allData.filter(filter => filter.planName === $scope.currentDashBoard && filter.rlu <= filter.upper && filter.rlu > filter.lower).length;

                            c3.generate(returnChartDataColums(failed, pass, caution, "idpieByPlanLoc", "pie")); 

                            c3.generate(loadTrendChart($scope.currentDashBoard, "", "idchartLow", "bar")); 

                       
                        } else {

                            const currentdata = $scope.allData.filter(filter => filter.locationName === $scope.currentDashBoard); // && filter.rlu > filter.upper Show All not only failss
                            
                            const tempArray = [];

                            Object.keys(currentdata).forEach(function (key) {
                                tempArray.push({
                                    resultDate: currentdata[key].resultDate,
                                    rlu: currentdata[key].rlu,
                                    upper: currentdata[key].upper
                                });
                            });

                            $scope.failDataSource = tempArray;


                            //Generate Pie Chart by Location 

                            const failed = $scope.allData.filter(filter => filter.locationName === $scope.currentDashBoard && filter.rlu > filter.upper).length;
                            const pass = $scope.allData.filter(filter => filter.locationName === $scope.currentDashBoard && filter.rlu <= filter.lower).length;
                            const caution = $scope.allData.filter(filter => filter.locationName === $scope.currentDashBoard && filter.rlu <= filter.upper && filter.rlu > filter.lower).length;

                            c3.generate(returnChartDataColums(failed, pass, caution, "idpieByPlanLoc", "pie")); 


                            c3.generate(loadTrendChart("", $scope.currentDashBoard, "idchartLow", "scatter")); 
                        }

                    } catch (e) {
                        console.log(e);
                    }
                }
                           
            } catch (e) {
                console.log("loadFailGrid");
            }
        }
      
        function setDates() {
            $scope.datePickerFromDate = window.moment().startOf("day");
            $scope.datePickerToDate = window.moment().endOf("day");
        }   


        function updateUserSettings() {

            userService.updateUserSettings("dash",
                userSettings,
                window.moment.utc(userSettings.dashDateFrom).format(),
                window.moment.utc(userSettings.dashDateTo).format()
            ).then(function(response) {
                   // getGridData();
                },
                function(response) {

                    console.log(response);
                }
            );

        }      

    });