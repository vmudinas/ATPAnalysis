
app.controller("adminUserManagementController",
    function($scope, userService, language) {

        $scope.title = "User Management";
        $scope.language = language[0];


        init();

        function init() {

            if ($scope.language !== undefined) {
                setContext();
            }
        }

        function setContext() {
            getRolesData();
            getUserData();
            $scope.userManagementGridOptions = {
                bindingOptions: {
                    dataSource: "userData",
                    columns: "userManagerColumns"
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
                height: 300,
                hoverStateEnabled: true,
                editing: {
                    mode: "form",
                    allowUpdating: true,
                    allowDeleting: true,
                    allowAdding: true,
                    form: {
                        items: [
                            {
                                itemType: "group",
                                caption: $scope.language.userDetails.caption,
                                items: ["userName", "email", "roleName"]
                            }
                        ]
                    }
                },
                onRowInserting: function(e) {

                    userService.getAddUser(e.data).then(function(response) {

                        },
                        function(response) {

                            console.log(response);
                        }
                    );
                },
                onRowUpdating: function(e) {
                  
                    userService.getUpdateUser(e.oldData.userName, e.newData.userName, e.newData.email, e.newData.roleName)
                        .then(function(response) {


                            },
                            function(response) {

                                console.log(response);
                            }
                        );
                },
                onRowRemoving: function(e) {
                    // removeToken(e);

                    userService.getRemoveUser(e.data).then(function(response) {


                        },
                        function(response) {

                            console.log(response);
                        }
                    );
                },
                onSelectionChanged: function(selectedItems) {

                    getUserSite(selectedItems.selectedRowsData[0].userName);
                    // Call user service to get All the sites for the user


                }

            };
            $scope.userSiteGridOptions = {
                bindingOptions: {
                    dataSource: "userSiteData",
                    columns: "userSiteColumns"
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
                height: 350,
                hoverStateEnabled: true,
                editing: {
                    mode: "cell",
                    allowUpdating: true,
                    allowDeleting: false,
                    allowAdding: false
                },
                onRowUpdating: function(e) {
                    console.log(e);
                    console.log(e.oldData);


                    userService.updateUserSite(e.oldData).then(function(response) {

                            console.log(response.data);

                        },
                        function(response) {

                            console.log(response);
                        }
                    );


                }

            };
        }

        function getUserSite(userName) {
            // Get All the site for Account
            // Get Sites for the User
            //If user is in the site set checkbox to trues

            $scope.userSiteColumns = [
                {
                    dataField: "site",
                    allowEditing: false
                },
                {
                    dataField: "selected"
                }
            ];


            userService.getUserSites(userName).then(function(response) {

                    $scope.userSiteData = response.data;

                },
                function(response) {

                    console.log(response);
                }
            );


            //$scope.userSiteData = [{

            //    "siteName": "Super Mart of the West",
            //    "selected": false
            //},
            //{               
            //    "siteName": "222t",
            //    "selected": true
            //}];
        }

        function getUserManagementColumns(roles) {

            $scope.userManagerColumns = [
                {
                    dataField: "userName",
                    width: "150",
                    sortOrder: "asc",
                    caption: toTitleCase($scope.language.userName.caption),
                    validationRules: [{ type: "required", message: $scope.language.userNameRequired.caption }]
                },
                {
                    dataField: "email",
                    caption: toTitleCase($scope.language.email.caption),
                    validationRules: [
                        { type: "required", message: $scope.language.emailRequired.caption }, { type: "email" }
                    ]
                },
                {
                    dataField: "roleName",
                    caption: toTitleCase($scope.language.role.caption),
                    width: "150",
                    validationRules: [{ type: "required", message: $scope.language.roleRequired.caption }],
                    lookup:
                    {
                        valueExpr: "role",
                        displayExpr: "role",
                        dataSource: roles
                    }
                }
            ];


        }

        function getUserData() {

            userService.getUsersForAccount().then(function(response) {

                    $scope.userData = response.data;

                },
                function(response) {

                    console.log(response);
                }
            );


        }

        function getRolesData() {


            userService.getRolesForAccount().then(function(response) {

                    $scope.userRoleData = response.data;
                    getUserManagementColumns($scope.userRoleData);

                },
                function(response) {

                    console.log(response);

                }
            );


        }

    });