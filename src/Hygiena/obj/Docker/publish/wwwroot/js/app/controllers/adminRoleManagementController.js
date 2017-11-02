app.controller("adminRoleManagementController",
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
            getAllPermissionsData();


            getRoleColumns();

            $scope.userRolesGridOptions = {
                bindingOptions: {
                    dataSource: "userRoleData",
                    columns: "userRoleColumns"
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
                height: "inherit",
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
                                caption: $scope.language.role.caption,
                                items: ["role", "roleDescription"]
                            }
                        ]
                    }
                },
                onRowInserting: function(e) {

                    userService.addRoleForAccount(e.data.__KEY__, e.data.role, e.data.roleDescription)
                        .then(function(response) {

                            },
                            function(response) {

                                console.log(response);
                            }
                        );

                },
                onRowUpdating: function(e) {

                    userService.updateRole(e.key.roleId, e.newData.role, e.newData.roleDescription)
                        .then(function(response) {

                            },
                            function(response) {

                                console.log(response);
                            }
                        );
                },
                onRowRemoving: function(e) {

                    userService.removeRole(e.data.roleId).then(function(response) {

                        },
                        function(response) {

                            console.log(response);
                        }
                    );
                },
                onSelectionChanged: function(selectedItems) {
                    var data = selectedItems.selectedRowsData[0];
                    getPermissionsData(data.role);

                }

            };

            $scope.userPermissionsGridOptions = {
                bindingOptions: {
                    dataSource: "userPermissionData",
                    columns: "userRolePermissionsColumns"
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
                height: "inherit",
                hoverStateEnabled: true,
                editing: {
                    mode: "cell",
                    allowUpdating: true,
                    allowDeleting: false,
                    allowAdding: false,
                    form: {
                        items: [
                            {
                                itemType: "group",
                                caption: $scope.language.permission.caption,
                                items: ["permissionName"]
                            }
                        ]
                    }
                },
                onRowUpdating: function(e) {

                    userService.getUpdatePermission($scope.selectedRole, e.key.permissionName, e.key.status)
                        .then(function(response) {
                            },
                            function(response) {
                                console.log(response);
                            }
                        );
                },
                onRowInserting: function(e) {


                },
                onRowRemoving: function(e) {


                },
                onSelectionChanged: function(selectedItems) {
                    var data = selectedItems.selectedRowsData[0];

                }

            };

        }

        function getRoleColumns() {

            $scope.userRoleColumns = [
                {
                    dataField: "roleId",
                    visible: false

                },
                {
                    dataField: "role",
                    width: "auto",
                    sortOrder: "asc",
                    caption: toTitleCase($scope.language.role.caption),
                    validationRules: [{ type: "required", message: $scope.language.roleRequired.caption }]
                },
                {
                    dataField: "roleDescription",
                    caption: toTitleCase($scope.language.roleDescription.caption),
                    validationRules: [{ type: "required", message: $scope.language.roleDescriptionRequired.caption }]
                }
            ];
        }

        function getPermissionColumns(permissions) {

            $scope.userRolePermissionsColumns = [
                {
                    dataField: "permissionName",
                    caption: toTitleCase($scope.language.permission.caption),
                    validationRules: [{ type: "required", message: $scope.language.permissionRequired.caption }],
                    width: "auto",
                    sortOrder: "asc",
                    lookup:
                    {
                        valueExpr: "permissionName",
                        displayExpr: "permissionName",
                        title: "Permission",
                        dataSource: permissions
                    },
                    allowEditing: false
                },
                {
                    dataField: "permissionDescription",
                    caption: toTitleCase($scope.language.permissionDescription.caption),
                    validationRules: [
                        { type: "required", message: $scope.language.permissionDescriptionRequired.caption }
                    ],
                    allowEditing: false
                },
                {
                    dataField: "status",
                    width: 75,
                    caption: toTitleCase($scope.language.permissionStatus.caption),
                    allowEditing: true
                }
            ];

        }

        function getAllPermissionsData() {
            userService.getAllRolePermissions().then(function(response) {

                    getPermissionColumns(response.data);

                },
                function(response) {

                    console.log(response);

                }
            );
        }

        function getRolesData() {


            userService.getRolesForAccount().then(function(response) {

                    $scope.userRoleData = response.data;


                },
                function(response) {

                    console.log(response);

                }
            );


        }

        function getPermissionsData(role) {

            userService.getRolePermissions(role).then(function(response) {

                    $scope.userPermissionData = response.data;
                    $scope.selectedRole = role;


                },
                function(response) {

                    console.log(response);

                }
            );

        }
    });