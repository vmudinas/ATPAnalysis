app.controller("scheduleEmailedReportsController",
    function ($scope, userService, language) {

        $scope.language = language[0];

        init();

        function init() {

            if ($scope.language !== undefined) {
                //TODO: complete
            }
        }

        $scope.emailScheduler = {
            dataSource: [{
                text: 'Meeting customers',
                startDate: new Date(2017, 4, 10, 11, 0),
                endDate: new Date(2017, 4, 10, 13, 0)
            }, {
                text: 'Summing up the results',
                startDate: new Date(2017, 4, 11, 12, 0),
                endDate: new Date(2017, 4, 11, 13, 0)
            },
                // ...
            ],
            currentDate: new Date(2017, 4, 10),
            startDayHour: 8,
            endDayHour: 19
        }
    }
);
