app.controller("plansController",
    function($scope, plansService) {


        $scope.title = "Plans";


        $scope.plansColumns = [
            {
                caption: "Order", // $scope.language.resultDate.caption.toCamelCase,
                dataField: "displayOrder",
                allowSorting: true,
                sortOrder: "asc",
                width: "auto"
            },
            {
                caption: "Plan Name", // $scope.language.rLU.caption.toCamelCase,
                dataField: "planName",
                allowSorting: true
            },
            {
                caption: "Created By", //$scope.language.upper.caption.toCamelCase,
                dataField: "createdBy",
                allowSorting: true
            }
        ];

        $scope.plansGridOptions = {
            bindingOptions: {
                dataSource: "plansSource",
                columns: "plansColumns"
            },
            "export": {
                enabled: true,
                fileName: "plans"
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
            }
        };


        init();

        function init() {
            plansService.getCurrentPlans()
                .then(function(response) {

                        $scope.plansSource = response.data;

                    },
                    function(response) {

                        console.log(response);
                    }
                );


        }
    });