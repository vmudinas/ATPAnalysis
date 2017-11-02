app.service("scheduleEmailedReportsService",
    function($http) {
        return {
            getReportSchedules: function() {
                return $http.get("../api/GetReportSchedules");
            },
            updateReportSchedule: function(clientReportSchedule) {
                var data = $.param({ crs: clientReportSchedule });
                var config = { headers: { "Content-Type": "application/x-www-form-urlencoded;charset=utf-8;" } };
                return $http.post("../api/UpdateReportSchedule", data, config);
            },
            addReportSchedule: function(clientReportSchedule) {
                var data = $.param({ crs: clientReportSchedule });
                var config = { headers: { "Content-Type": "application/x-www-form-urlencoded;charset=utf-8;" } };
                return $http.post("../api/AddReportSchedule", data, config);
            },
            deleteReportSchedule: function(accountId, scheduleId) {
                var data = $.param({ accountId: accountId, scheduleId: scheduleId });
                var config = { headers: { "Content-Type": "application/x-www-form-urlencoded;charset=utf-8;" } };
                return $http.post("../api/DeleteReportSchedule", data, config);
            }
        };
    }
);