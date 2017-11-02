app.controller("resultController",
    function ($scope, resultService, hygEnum, periodsArray, language, userService, userSettings) {
        var shouldForceResultsRefresh, didSecondPageLoadHappen, isPageNewlyLoaded, savingTimeout, resultSchemaJsonString;

        init();

        function init() {
            $scope.previousState = {};
            isPageNewlyLoaded = true;
            savingTimeout = 0;
            shouldForceResultsRefresh = didSecondPageLoadHappen = false;

            $scope.language = language[0];

            if ($scope.language !== undefined) {
                $scope.language.period.caption = toTitleCase($scope.language.period.caption);
                $scope.language.from.caption = toTitleCase($scope.language.from.caption);
                $scope.language.to.caption = toTitleCase($scope.language.to.caption);

                userService.getUserRolePermissions().then(function(response) {
                    $scope.userPermissions = response.data;

                    userService.getUserSettings().then(function (response) {
                        userSettings = response.data;

                        setResultDefaults();
                        setResultDatesConfig();

                        updateResultPeriod(userSettings.resultsPeriod, false);

                        resultService.getResultGridSchemaJSON().then(function (response) {
                            resultSchemaJsonString = response.data;
                            getResultGridData();
                        });
                    });
                },
                function(response) { console.log(response); }
                );
            }
        }

        function getResultGridData() {
         
            $scope.isDeletedFilter = false;

            $scope.resultData = {
                load: function (loadOptions) {
                                     

                    return resultService.getPagedResults(moment.utc($scope.datePickerFromDate), moment.utc($scope.datePickerToDate), loadOptions)
                        .then(function (response) {
                         
                            if ($scope.girdState) {
                                saveGridState($scope.girdState);

                            }
                            return response.data;
                        }, function (response) {
                            console.log(response);

                        });

                }
                ,
                totalCount: function (loadOptions) {
              
                    return resultService.getPagedResultsCount(moment.utc($scope.datePickerFromDate), moment.utc($scope.datePickerToDate), loadOptions)
                        .then(function (response) {
                                   return response.data;
                        }, function (response) {
                           console.log(response);

                        });
                },
                update: function (key, values)
                {
                   
                    Object.assign(key, values);

                    resultService.updateResult(key)
                        .then(function (response) {
                            refreshGrid();
                        },
                        function (response) {
                            console.log(response);
                        }
                        );
                }
            };

            $scope.resultColumns = resultService.getResultColumns($scope.userPermissions, $scope.language, userSettings, resultSchemaJsonString); $scope.resultColumns = resultService.getResultColumns($scope.userPermissions, $scope.language, userSettings, resultSchemaJsonString); $scope.resultColumns = resultService.getResultColumns($scope.userPermissions, $scope.language, userSettings, resultSchemaJsonString); $scope.resultColumns = resultService.getResultColumns($scope.userPermissions, $scope.language, userSettings, resultSchemaJsonString); $scope.resultColumns = resultService.getResultColumns($scope.userPermissions, $scope.language, userSettings, resultSchemaJsonString);
        }

        function logArrayElements(element, index, array) {
            console.log('a[' + index + '] = ' + element);
        }

        $scope.resultsDataGridOptions = {
            bindingOptions: {
                dataSource: "resultData",
                columns: "resultColumns"
            },
            "export": {
                enabled: true,
                fileName: "results"
            },
            remoteOperations: {
                sorting: true,
                filtering: true,
                paging: true,
                grouping: true,
                summary: true,
                groupPaging:true
            },
            paging: {
                pageSize: 12
            },
            pager: {
                showPageSizeSelector: true,
                allowedPageSizes: [8, 12, 20]
            },
            searchPanel: { visible: true },
            filterRow: { visible: true },
            columnChooser: {
                enabled: true,
                height: 250,
                width: 400
            },
            stateStoring: {
                enabled: true,
                savingTimeout: 0, // 0 milliseconds - no delay in applying pre-saved result grid schema
                type: "custom",
                storageKey: "resultsStorage",
                customSave: function (gridState) {
                    $scope.girdState = gridState;
                    saveGridState(gridState);
                },
                customLoad: function (e) {
                }
            },
            allowSortingBySummary: true,
            allowSorting: true,
            allowFiltering: true,
            allowColumnReordering: true,
            allowColumnResizing: true,
            allowGrouping: true,
            headerFilter: { visible: true },
            allowExpandAll: true,
            selection: { mode: "single" },
            loadPanel: { enabled: true },
            groupPanel: { visible: true },
            scrolling: {
                //mode: "virtual", // bad - kills columns sortOrder
                mode: "standard",
                showScrollbar: "always"
            },
            editing: {
                mode: "cell",
                allowUpdating: true,
                allowDeleting: false,
                allowAdding: false
            },
            showBorders: true,
            showRowLines: true,
            sorting: { mode: "multiple" },           
            height: 550
        };

        function saveGridState(gridState)
        {
            if (!angular.equals(gridState, $scope.previousState))
            {
                $scope.previousState = gridState;

                userService.updateUserSettings("results", userSettings, moment.utc(userSettings.resultsDateFrom).format("lll"),
                    moment.utc(userSettings.resultsDateTo).format("lll"));

                resultSchemaJsonString = angular.toJson(gridState.columns, false);
                resultService.updateResultGridSchema(resultSchemaJsonString);
            }        
        }

        function setResultDefaults() {
            $scope.title = "resultController";
            $scope.periods = periodsArray;

            if (userSettings.resultsPeriod === null || typeof userSettings.resultsPeriod === "undefined") {
                userSettings.resultsPeriod = hygEnum.periodsEnum.ThisYear;
            }

            // if customer, dates should also exist, but just in case they don't, default dates to today - 00:00 through 23.59
            if (userSettings.resultsPeriod === hygEnum.periodsEnum.Custom
                && (userSettings.resultsDateFrom === null || userSettings.resultsDateTo === null)) {
                userSettings.resultsDateFrom = moment().startOf("day")._d;
                userSettings.resultsDateTo = moment().endOf("day")._d;
            }

            $scope.selectedPeriodResult = $scope.periods[userSettings.resultsPeriod];
        }

        $scope.resetResultGridTemplateClick = function () {
            resultSchemaJsonString = "";
            resultService.updateResultGridSchema(resultSchemaJsonString);
            shouldForceResultsRefresh = true; // this tells custom save to refresh
            getResultGridData();
        };

        $scope.changeResultPeriod = function (e) {
            updateResultPeriod(e.id, true);  
            refreshGrid();
        };

        function refreshGrid() {
            var dataGrid = $('#resultGrid').dxDataGrid('instance');
            dataGrid.refresh();    
        };

        function updateResultPeriod(period, shouldGetResultGridData) {

            if (typeof period === "undefined") { return; }

            if (period === hygEnum.periodsEnum.LastWeek) {
                $scope.datePickerFromDate = moment().subtract(1, "week").startOf("week").startOf("day");
                $scope.datePickerToDate = moment().subtract(1, "week").endOf("week").endOf("day");
            } else if (period === hygEnum.periodsEnum.LastMonth) {
                $scope.datePickerFromDate = moment().subtract(1, "month").startOf("month").startOf("day");
                $scope.datePickerToDate = moment().subtract(1, "month").endOf("month").endOf("day");
            } else if (period === hygEnum.periodsEnum.LastYear) {
                $scope.datePickerFromDate = moment().subtract(1, "year").startOf("year").startOf("month").startOf("day");
                $scope.datePickerToDate = moment().subtract(1, "year").endOf("year").endOf("month").endOf("day");
            } else if (period === hygEnum.periodsEnum.ThisWeek) {
                $scope.datePickerFromDate = moment().startOf("week").startOf("day");
                $scope.datePickerToDate = moment().endOf("day");
            } else if (period === hygEnum.periodsEnum.ThisMonth) {
                $scope.datePickerFromDate = moment().startOf("month").startOf("day");
                $scope.datePickerToDate = moment().endOf("day");
            } else if (period === hygEnum.periodsEnum.ThisYear) {
                $scope.datePickerFromDate = moment().startOf("year").startOf("month").startOf("day");
                $scope.datePickerToDate = moment().endOf("day");
            } else if (period === hygEnum.periodsEnum.Today) {
                $scope.datePickerFromDate = moment().startOf("day");
                $scope.datePickerToDate = moment().endOf("day");
            } else if (period === hygEnum.periodsEnum.Yesterday) {
                $scope.datePickerFromDate = moment().subtract(1, "day").startOf("day");
                $scope.datePickerToDate = moment().subtract(1, "day").endOf("day");
            }
            else if (period === hygEnum.periodsEnum.ThisQuarter) {
                var currQtr = moment().quarter();
                var monthOffset;

                if (currQtr === 1) { monthOffset = 0; }
                else if (currQtr === 2) { monthOffset = 3; }
                else if (currQtr === 3) { monthOffset = 6; }
                else if (currQtr === 4) { monthOffset = 9; }

                var qtrFromDate = moment([moment().year(), monthOffset]);
                $scope.datePickerFromDate = moment(qtrFromDate).startOf("month").startOf("day");
                $scope.datePickerToDate = moment().endOf("day");
            }
            else if (period === hygEnum.periodsEnum.LastQuarter) {
                var currQtr = moment().quarter();
                var fromMonthOffset, toMonthOffset;
                var yearOffset = moment().year();

                if (currQtr === 1) {
                    yearOffset--;
                    fromMonthOffset = 9;
                }
                else if (currQtr === 2) { fromMonthOffset = 0; }
                else if (currQtr === 3) { fromMonthOffset = 3; }
                else if (currQtr === 4) { fromMonthOffset = 6; }

                toMonthOffset = fromMonthOffset + 2;

                var qtrFromDate = moment([yearOffset, fromMonthOffset]);
                var qtrToDate = moment([yearOffset, toMonthOffset]);

                $scope.datePickerFromDate = moment(qtrFromDate).startOf("month").startOf("day");
                $scope.datePickerToDate = moment(qtrToDate).endOf("month").endOf("day")
            }
            else if (period === hygEnum.periodsEnum.Custom) {
                if (userSettings.resultsDateFrom !== null && userSettings.resultsDateTo !== null && typeof userSettings.resultsDateFrom !== "undefined"
                    && typeof userSettings.resultsDateTo !== "undefined")
                {
                    $scope.datePickerFromDate = moment.utc(userSettings.resultsDateFrom);
                    $scope.datePickerToDate = moment.utc(userSettings.resultsDateTo);
                }
            }

            $scope.selectedPeriodResult = $scope.periods[period];
            userSettings.resultsPeriod = period;
            userSettings.resultsDateFrom = $scope.datePickerFromDate._d;
            userSettings.resultsDateTo = $scope.datePickerToDate._d;

            userService.updateUserSettings("results", userSettings, moment.utc(userSettings.resultsDateFrom).format("lll"),
                moment.utc(userSettings.resultsDateTo).format("lll"));

            if (shouldGetResultGridData) { getResultGridData(); }
        }

        function setResultDatesConfig() {
            $scope.dateBox = {
                dateTimeFromResult: {
                    type: "datetime",
                    //format: "dd/MM/yyyy", // format is deprecated as of 16.1. Using format here breaks i18n.
                    pickerType: "rollers",
                    min: new Date(1985, 0, 1),
                    value: $scope.datePickerFromDate,
                    bindingOptions: { value: "datePickerFromDate" },
                    //bindingOptions: { value: "datePickerFromDate._d", max: "datePickerToDate._d" }, // experimental - NOPE - period change can violate
                    onValueChanged: function (data) {
                        if ($scope.datePickerFromDate > $scope.datePickerToDate) {
                            //alert("To date must be greater than from date.");
                            // set "to" date to "from" date?
                        }
                    },
                    onClosed: function (data) {
                        userSettings.resultsDateFrom = $scope.datePickerFromDate;
                        userSettings.resultsPeriod = hygEnum.periodsEnum.Custom;
                        $scope.selectedPeriodResult = $scope.periods[hygEnum.periodsEnum.Custom];
                        getResultGridData();
                    }
                },
                dateTimeToResult: {
                    type: "datetime",
                    //format: "shortDate", // format is deprecated as of 16.1...
                    pickerType: "rollers",
                    value: $scope.datePickerToDate,
                    bindingOptions: { value: "datePickerToDate" },
                    //bindingOptions: { value: "datePickerToDate._d", min: "datePickerFromDate._d" }, // experimental - NOPE - period change can violate
                    onValueChanged: function (data) {
                        if ($scope.datePickerFromDate > $scope.datePickerToDate) {
                            //alert("To date must be greater than from date.");
                            // set "to" date to "from" date?
                        }
                    },
                    onClosed: function (data) {
                        userSettings.resultsDateTo = $scope.datePickerToDate;
                        userSettings.resultsPeriod = hygEnum.periodsEnum.Custom;
                        $scope.selectedPeriodResult = $scope.periods[hygEnum.periodsEnum.Custom];
                        getResultGridData();
                    }
                }
            };
        }
    });
