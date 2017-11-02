app.controller("adminUnitController",
    function($scope, userService, language) {
        init();

        $scope.title = "AdminUnit";

        function init() {

            try {
                $scope.language = language[0];
                $scope.maingroup = true;


                $scope.unitTokenGridOptions = {
                    bindingOptions: {
                        dataSource: "unitTokenData",
                        columns: "unitTokenColumns"
                    },
                    paging: {
                        enabled: false
                    },
                    showBorders: true,
                    showColumnLines: true,
                    showRowLines: true,
                    filterRow: { visible: true },
                    searchPanel: { visible: true },
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
                        return window.innerHeight / 2;
                    },
                    hoverStateEnabled: true,
                    editing: {
                        mode: "form",
                        allowUpdating: false,
                        allowDeleting: true,
                        allowAdding: true,
                        form: {
                            items: [
                                {
                                    itemType: "group",
                                    caption: $scope.language.generateToken.caption,
                                    items: ["token", "unitName", "siteName"]
                                }
                            ]
                        }
                    },
                    onRowInserting: function(e) {
                        // update
                        registerToken(e);
                    },
                    onRowRemoving: function(e) {
                        removeToken(e);
                    }

                };


                getColumns();

                getTokens();
            } catch (e) {

            }


        }


        function removeToken(e) {

            userService.getRemoveToken(e.data.token, e.data.creator).then(function(response) {

                    getTokens();
                },
                function(response) {

                    console.log(response);
                }
            );
        }

        function registerToken(e) {

            userService.getRegisterToken(e.data.siteName, e.data.token, e.data.unitName).then(function(response) {
                    $scope.token = response.data;
                    getTokens();
                },
                function(response) {

                    console.log(response);
                }
            );
        }

        function getColumns() {

            userService.getSites().then(function(response) {

                    $scope.unitTokenColumns = getTokenColumns(response.data);
                },
                function(response) {

                    console.log(response);
                }
            );


        }

        function getTokens() {

            userService.getUnitTokens().then(function(response) {

                    $scope.unitTokenData = response.data;

                },
                function(response) {
                    $scope.unitTokenData = [];
                    console.log(response);
                }
            );
        }

        function getTokenColumns(sites) {


            return [
                {
                    dataField: "unitName",
                    caption: toTitleCase($scope.language.unitName.caption),
                    width: "auto",
                    validationRules: [{ type: "required", message: "Unit name is required" }]
                },
                {
                    dataField: "siteName",
                    caption: toTitleCase($scope.language.siteName.caption),
                    validationRules: [{ type: "required", message: "Site Name is required" }],
                    width: 300,
                    lookup: {
                        valueExpr: "id",
                        displayExpr: "name",
                        title: "Site Name",
                        dataSource: sites
                    },
                    cellTemplate: function(container, options) {

                        $("<div> " + options.value + " </div>").appendTo(container);

                    }
                },
                {
                    dataField: "unitNumber",
                    caption: toTitleCase($scope.language.status.caption),
                    width: "auto",
                    cellTemplate: function(container, options) {

                        if (options.value) {
                            $("<div class='registeredClass'> " +
                                toTitleCase($scope.language.registered.caption) +
                                " </div>").appendTo(container);
                        } else {
                            $("<div class='pendingClass'> " + toTitleCase($scope.language.pending.caption) + " </div>")
                                .appendTo(container);
                        }
                    }
                },
                {
                    dataField: "unitNumber",
                    caption: toTitleCase($scope.language.unitNumber.caption),
                    width: "auto"
                },
                {
                    dataField: "creator",
                    caption: toTitleCase($scope.language.creator.caption),
                    width: 200,
                },
                {
                    dataField: "token",
                    width: "auto",
                    caption: toTitleCase($scope.language.tokenName.caption),
                    validationRules: [
                        { type: "required", message: "Token is required" },
                        {
                            type: "custom",
                            validationCallback: function(options) {

                                var compare = $.grep($scope.unitTokenData,
                                    function(obj) { return obj.token == options.value; });

                                if (compare.length > 0) {
                                    return false;
                                }


                                return true;
                            },
                            message: "Token already exist"
                        }
                    ]
                }
            ];
        }
    });