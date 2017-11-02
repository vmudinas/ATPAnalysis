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
            height: 600,
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