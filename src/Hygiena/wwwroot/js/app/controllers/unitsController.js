app.controller("unitsController",
    function($scope, unitsService, language) {

        init();

        function init() {
            $scope.language = language[0];
            $scope.language.save.caption = toTitleCase($scope.language.save.caption);
            $scope.basicTabClass = "active";
            $scope.planTabClass = $scope.basicTabClass === "active" ? "" : "active";
            $scope.saveBasicMuted = "text-muted";
            $scope.savePlansMuted = "text-muted";
            $scope.selectedUnit = {}; // this one to be populated based on selected unit
            $scope.allPlans = []; // All available plans - [0]{id: ..., name: ...}, [1]{id: ..., name: ...}, etc.
            $scope
                .allPlansPerUnit = [];
// All available plans that have NOT been added to a unit (i.e. difference of allPlans and assignedPlansPerUnit)
            $scope.assignedPlansPerUnit = []; // All plans that have been added to a unit

            // TODO: service that gets units (per acct)
            $scope.units = [];
            var serNo = "";

            for (let i = 0; i < 5; i++) {
                serNo = (i + 1).toString() + (i + 1).toString() + (i + 1).toString();
                $scope.units.push({
                    serialNo: serNo,
                    name: "Unit - " + serNo,
                    site: "Hospital " + (i + 1).toString(),
                    version: "Ensure5",
                    assignedPlans: [
                        { id: i + 1, name: "Plan " + (i + 1).toString() },
                        { id: i + 2, name: "Plan " + (i + 2).toString() }
                    ]
                });
            }

            // TODO: service that gets all plans (per acct, per site?)
            for (let i = 0; i < 10; i++) {
                $scope.allPlans.push({ id: i + 1, name: "Plan " + (i + 1).toString() });
            }
        }

        $scope.title = toTitleCase($scope.language.units.caption);

        $scope.dataGridHdrOptions = {
            bindingOptions: {
                dataSource: "units",
                columns: "unitsHdrColumns"
            },
            showBorders: true,
            showColumnLines: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            allowSortingBySummary: true,
            allowSorting: true,
            allowFiltering: true,
            headerFilter: { visible: true },
            allowExpandAll: true,
            loadPanel: { enabled: false },
            scrolling: {
                mode: "virtual",
                showScrollbar: "always"
            },
            selection: { mode: "single" },
            editing: {
                mode: "batch"
            },
            onSelectionChanged: function(selectedItems) {
                if (selectedItems.selectedRowsData.length > 0) {
                    if (selectedItems.selectedRowsData[0].serialNo !== undefined) {
                        populateSelectedUnitDetail(selectedItems.selectedRowsData[0].serialNo);
                    }
                }
            },
            onContentReady: function(e) {
                e.component.selectRowsByIndexes([0], true);
            },
            height: function() {
                return window.innerHeight / 2;
            }
        };

        $scope.dataGridAllPlansOptions = {
            bindingOptions: {
                dataSource: "allPlansPerUnit",
                columns: "planNameAvailableColumns"
            },
            allowFiltering: false,
            allowSortingBySummary: false,
            allowSorting: false,
            selection: { mode: "multiple", showCheckBoxesMode: "always" },
            allowExpandAll: true,
            scrolling: { mode: "virtual", showScrollbar: "always" },
            showBorders: true,
            height: function() {
                return window.innerHeight / 2;
            }
        };

        $scope.dataGridAssignedPlansOptions = {
            bindingOptions: {
                dataSource: "assignedPlansPerUnit",
                columns: "planNameAssignedColumns"
            },
            allowFiltering: false,
            allowSortingBySummary: false,
            allowSorting: false,
            selection: { mode: "multiple", showCheckBoxesMode: "always" },
            allowExpandAll: true,
            scrolling: { mode: "virtual", showScrollbar: "always" },
            showBorders: true,
            height: function() {
                return window.innerHeight / 2;
            }
        };

        $scope.unitsHdrColumns = [
            {
                caption: toTitleCase($scope.language.serialNo.caption),
                dataField: "serialNo",
                allowSorting: true,
                width: "auto"
            },
            {
                caption: toTitleCase($scope.language.name.caption),
                dataField: "name",
                allowSorting: true
            },
            {
                caption: toTitleCase($scope.language.site.caption),
                dataField: "site",
                allowSorting: true
            }
        ];

        $scope.planNameAvailableColumns = [
            {
                caption: $scope.language.availablePlans.caption,
                dataField: "name"
            }
        ];

        $scope.planNameAssignedColumns = [
            {
                caption: $scope.language.plansOnUnit.caption,
                dataField: "name"
            }
        ];

        $scope.unitBasicFieldChange = function() {
            $scope.saveBasicMuted = $scope.selectedUnit.serialNo === undefined ||
                $scope.selectedUnit.name === undefined ||
                $scope.selectedUnit.version === undefined ||
                $scope.selectedUnit.site === undefined
                ? "text-muted"
                : "";
        };

        $scope.tabClick = function(whichTab) {
            $scope.basicTabClass = whichTab === "Basic" ? "active" : "";
            $scope.planTabClass = $scope.basicTabClass === "active" ? "" : "active";
        };

        $scope.savePlansClick = function() {
            $scope.savePlansMuted = "text-muted";
            alert("TODO: Save plans for " + $scope.selectedUnit.name + " to the database.");
        };

        $scope.saveUnitBasicInfoClick = function() {
            $scope.saveBasicMuted = "text-muted";
            alert(`TODO: Save basic information for ${$scope.selectedUnit.name} to the database.`);
        };

        $scope.movePlansFromAvailableToAssigned = function() {
            let allPlansDataGrid = $("#allPlansPerUnitGrid").dxDataGrid("instance");
            let arrSelectedAvailPlans = allPlansDataGrid.getSelectedRowsData();
            if (arrSelectedAvailPlans !== undefined && arrSelectedAvailPlans.length > 0) {
                for (let i = 0; i < arrSelectedAvailPlans.length; i++) {
                    $scope.assignedPlansPerUnit.push(arrSelectedAvailPlans[i]);
                }
                $scope.savePlansMuted = "";
                sortPlanArrays();
            } else {
                // TODO: surface a "Please selected something..." message ???
            }

            $scope.allPlansPerUnit = getArrayDiff($scope.allPlans, $scope.assignedPlansPerUnit);
        };

        $scope.movePlansFromAssignedToAvailable = function() {
            let assignedPlansDataGrid = $("#assignedPlansGrid").dxDataGrid("instance");
            let arrSelectedAssignedPlans = assignedPlansDataGrid.getSelectedRowsData();
            if (arrSelectedAssignedPlans !== undefined && arrSelectedAssignedPlans.length > 0) {
                for (let i = 0; i < arrSelectedAssignedPlans.length; i++) {
                    $scope.allPlansPerUnit.push(arrSelectedAssignedPlans[i]);
                }
                $scope.savePlansMuted = "";
                sortPlanArrays();
            } else {
                // TODO: surface a "Please selected something..." message ???
            }

            $scope.assignedPlansPerUnit = getArrayDiff($scope.allPlans, $scope.allPlansPerUnit);
        };

        function sortPlanArrays() {
            $scope.allPlansPerUnit.sort(compare);
            $scope.assignedPlansPerUnit.sort(compare);
        }

        function compare(a, b) {
            if (a.name < b.name) {
                return -1;
            } else if (a.name > b.name) {
                return 1;
            } else {
                return 0;
            }
        }

        function populateSelectedUnitDetail(serialNo) {
            $("#assignedPlansGrid").dxDataGrid("instance").clearSelection();
            $("#allPlansPerUnitGrid").dxDataGrid("instance").clearSelection();
            $scope.saveBasicMuted = "text-muted";
            $scope.savePlansMuted = "text-muted";

            for (let i = 0; i < $scope.units.length; i++) {
                if ($scope.units[i].serialNo === serialNo) {
                    $scope.selectedUnit = angular.copy($scope.units[i]);
                    $scope.selectedUnitName = angular.copy($scope.selectedUnit.name);
                    $scope.allPlansPerUnit = [];
                    $scope.assignedPlansPerUnit = [];
                    for (let j = 0; j < $scope.selectedUnit.assignedPlans.length; j++) {
                        $scope.assignedPlansPerUnit.push($scope.selectedUnit.assignedPlans[j]);
                    }
                    $scope.allPlansPerUnit = getArrayDiff($scope.allPlans, $scope.assignedPlansPerUnit);
                    break;
                }
            }
        }

        function clearUnitDetail() {
            $("#assignedPlansGrid").dxDataGrid("instance").clearSelection();
            $("#allPlansPerUnitGrid").dxDataGrid("instance").clearSelection();
            $scope.saveBasicMuted = "text-muted";
            $scope.savePlansMuted = "text-muted";

            $scope.selectedUnit = {};
            $scope.selectedUnitName = "";
            $scope.allPlansPerUnit = [];
            $scope.assignedPlansPerUnit = [];
            $scope.allPlansPerUnit = [];
        }

        function getArrayDiff(arrSuperSet, arrSubSet) {
            // returns subtraction of arrSubSet minus arrSuperSet - assumes all arrays of objects with "id" and "name" fields
            arrDiff = [];
            var wasSubFoundInSuper = false;

            for (let i = 0; i < arrSuperSet.length; i++) {
                let result = $.grep(arrSubSet, function(e) { return e.id === arrSuperSet[i].id; });
                wasSubFoundInSuper = result.length !== 0;

                if (!wasSubFoundInSuper) {
                    arrDiff.push(arrSuperSet[i]);
                }
            }

            return arrDiff;
        }

    });