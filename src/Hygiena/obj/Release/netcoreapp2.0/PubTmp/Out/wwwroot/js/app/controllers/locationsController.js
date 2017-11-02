app.controller("locationsController",
    function($scope, locationsService) {


        $scope.title = "Locations";
        $scope.locationsColumns = [
            {
                caption: "Order", // $scope.language.resultDate.caption.toCamelCase,
                dataField: "displayOrder",
                allowSorting: true,
                sortOrder: "asc",
                width: "auto"
            },
            {
                caption: "Location Name", // $scope.language.rLU.caption.toCamelCase,
                dataField: "locationName",
                allowSorting: true
            },
            {
                caption: "Rank", //$scope.language.upper.caption.toCamelCase,
                dataField: "rank",
                allowSorting: true
            },
            {
                caption: "Upper", //$scope.language.upper.caption.toCamelCase,
                dataField: "upper",
                allowSorting: true
            },
            {
                caption: "Lower", //$scope.language.upper.caption.toCamelCase,
                dataField: "lower",
                allowSorting: true
            },
            {
                caption: "Zone", //$scope.language.upper.caption.toCamelCase,
                dataField: "zone",
                allowSorting: true
            },
            {
                caption: "Room Number", //$scope.language.upper.caption.toCamelCase,
                dataField: "roomNumber",
                allowSorting: true
            },
            {
                caption: "Status", //$scope.language.upper.caption.toCamelCase,
                dataField: "status",
                allowSorting: true
            },
            {
                caption: "Notes", //$scope.language.upper.caption.toCamelCase,
                dataField: "notes",
                allowSorting: true
            }
        ];

        $scope.locationsGridOptions = {
            bindingOptions: {
                dataSource: "locationsSource",
                columns: "locationsColumns"
            },
            "export": {
                enabled: true,
                fileName: "locations"
            },
            showBorders: true,
            showColumnLines: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            loadPanel: {
                enabled: false
            },
            scrolling: {
                mode: "virtual"
            },
            selection: {
                mode: "single"
            },
            height: function() {
                return window.innerHeight / 1.5;
            },
            hoverStateEnabled: true,
            editing: {
                mode: "form",
                allowUpdating: true,
                allowDeleting: true,
                allowAdding: true
                //form: {
                //    items: [
                //        {
                //            itemType: "group",
                //            caption: $scope.language.role.caption,
                //            items: ["roleName", "roleDescription"]
                //        }
                //    ]
                //}
            }
        };


        init();

        function init() {
            locationsService.getCurrentLocations()
                .then(function(response) {

                        $scope.locationsSource = response.data;

                    },
                    function(response) {

                        console.log(response);
                    }
                );


        }
    });