app.controller("adminController",
    function($scope, userService, language) {

        $scope.title = "Admin";
        $scope.language = language[0];

        $scope.$watch("password.oldPassword",
            function(e) {
                resetErrors();
            });
        $scope.$watch("password.newPassword",
            function(e) {
                resetErrors();
            });

        $scope.$watch("password.confirmPassword",
            function(e) {
                resetErrors();
            });

        function resetErrors() {
            $scope.success = false;
            $scope.incorrectpassword = false;
            $scope.passwordRequiresDigit = false;
            $scope.passwordRequiresNonAlphanumeric = false;
            $scope.passwordTooShort = false;
            $scope.passwordRequiresLower = false;
            $scope.passwordRequiresUpper = false;
            $scope.genericError = false;

        }

        $scope.changePassword = function(language) {

            resetErrors();

            userService.updatePassword($scope.password).then(function(response) {


                    if (response.data === "Success") {

                        $scope.success = true;
                    } else {
                        switch (response.data[0].code) {
                        case "PasswordMismatch":
                            $scope.incorrectpassword = true;
                            break;

                        case "PasswordRequiresDigit":

                            $scope.passwordRequiresDigit = true;
                            break;
                        case "PasswordRequiresNonAlphanumeric":
                            $scope.passwordRequiresNonAlphanumeric = true;
                            break;

                        case "PasswordTooShort":
                            $scope.passwordTooShort = true;

                            break;
                        case "PasswordRequiresLower":
                            $scope.passwordRequiresLower = true;

                            break;
                        case "PasswordRequiresUpper":
                            $scope.passwordRequiresUpper = true;
                            break;
                        default:
                            $scope.genericError = true;

                        }
                    }
                },
                function(response) {
                    console.log(response);
                }
            );

        };
    });