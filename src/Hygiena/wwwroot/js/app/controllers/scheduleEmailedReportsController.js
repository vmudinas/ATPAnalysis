app.controller("scheduleEmailedReportsController",
    function ($rootScope, $scope, scheduleEmailedReportsService, userService, userSettings, periodsArray, hygEnum, language) {

        $scope.language = language[0];
        $scope.rptSched = [];
        var sitesPerAccount = [];
        var haveAllCollectionsLoaded = false;
        var cellDurationMinutes = 30; // this is the schedule duration

        init();

        function init() {

            if ($scope.language !== undefined) {

                if (haveAllCollectionsLoaded) { return; }

                userService.getSites().then(
                    function (response) {
                        if (response.data !== null && typeof response.data !== "undefined") {
                            sitesPerAccount = assignSites(response.data);

                            getSchedules(true);

                            userService.getUserSettings().then(
                                function (response) {
                                    Object.assign(userSettings, response.data);
                                    if (userSettings.reportScheduleCurrentView === null ||
                                        typeof userSettings.reportScheduleCurrentView === "undefined" ||
                                        userSettings.reportScheduleCurrentView === "") {
                                        $scope.rptSchedCurrView = "week"; // 1st time will be blank - default to week view (TBD)
                                    } else {
                                        $scope.rptSchedCurrView = userSettings.reportScheduleCurrentView;
                                    }
                                },
                                function (response) { console.log(response); }
                            );
                        }
                    },
                    function (response) {
                        console.log(response);
                    }
                );
            }
        }

        function prepareSchedules() {

            // set start and end dates from strings to dates. Also, set value and display expressions for sites.
            for (var i = 0; i < $scope.rptSched.length; i++) {
                if ($scope.rptSched[i].siteIds !== null && $scope.rptSched[i].siteIds.length > 0) {
                    for (var j = 0; j < $scope.rptSched[i].siteIds.length; j++) {
                        $scope.rptSched[i].siteIds[j].valueExpr = "siteId";
                        $scope.rptSched[i].siteIds[j].displayExpr = "name";
                    }
                }

                if ($scope.rptSched[i].startDate !== null && typeof $scope.rptSched[i].startDate !== "undefined") {
                    $scope.rptSched[i].startDate = moment.utc($scope.rptSched[i].startDate).local();
                }
                if ($scope.rptSched[i].endDate !== null && typeof $scope.rptSched[i].endDate !== "undefined") {
                    $scope.rptSched[i].endDate = moment.utc($scope.rptSched[i].endDate).local();
                }
            }
        }

        function setScheduleSource() {

            $scope.reportsSchedule = {
                showAllDayPanel: false,
                bindingOptions: { dataSource: "rptSched", currentView: "rptSchedCurrView" },
                dataSource: $scope.rptSched,
                resources:
                [
                    {
                        fieldExpr: "reportId",
                        label: $scope.language.report.caption,
                        allowMultiple: false,
                        dataSource:
                        [
                            {
                                id: "0610aaec-6a8c-4c32-860a-da85f6565ea7", // TODO: get report resources from DB
                                text: "PCF Summary"
                            },
                            {
                                id: "8200fdf4-0206-4926-9087-924c2df01f7e",
                                text: "PCF Summary By Day of Week"
                            },
                            {
                                id: "33310f30-f71c-4753-9f7a-c4cb7f49e00c",
                                text: "PCF Summary By Location"
                            },
                            {
                                id: "efe2b156-02ff-4e70-b5e6-abd2560d1b56",
                                text: "PCF Summary By ..."
                            },
                            {
                                id: "634a1ec3-8396-485e-9adc-3d1ddb772e99",
                                text: "Fails Summary By Location"
                            },
                            {
                                id: "b8a248ad-4006-4676-9799-9a6076feeb17",
                                text: "Average RLUs By Day"
                            },
                            {
                                id: "0c66e534-0545-42b3-abb3-02153ceb2cd4",
                                text: "Average RLUs By ..."
                            },
                            {
                                id: "148e5840-d412-4ad6-8da4-1c5dc3ec40eb",
                                text: "Restest Analysis"
                            },
                            {
                                id: "7361fa03-18a3-4c64-86d1-d7cd95c02f4d",
                                text: "Restest Analysis ..."
                            },
                            {
                                id: "de2f82e3-dd39-45a1-a1f1-034c62be9d3e",
                                text: "Tests Performed By User"
                            },
                            {
                                id: "eba79080-8588-43c4-92cf-0710efc3b12f",
                                text: "Tests Performed By ..."
                            },
                            {
                                id: "2aa597ba-ecb4-461a-820d-4211dbf6124c",
                                text: "Customized Reports Here ..."
                            }
                        ]
                    },
                    {
                        fieldExpr: "reportPeriod",
                        label: $scope.language.period.caption,
                        allowMultiple: false,
                        dataSource:
                        [
                            {
                                id: hygEnum.periodsEnum.Today,
                                text: periodsArray[hygEnum.periodsEnum.Today].period
                            },
                            {
                                id: hygEnum.periodsEnum.ThisWeek,
                                text: periodsArray[hygEnum.periodsEnum.ThisWeek].period
                            },
                            {
                                id: hygEnum.periodsEnum.LastWeek,
                                text: periodsArray[hygEnum.periodsEnum.LastWeek].period
                            },
                            {
                                id: hygEnum.periodsEnum.ThisMonth,
                                text: periodsArray[hygEnum.periodsEnum.ThisMonth].period
                            },
                            {
                                id: hygEnum.periodsEnum.LastMonth,
                                text: periodsArray[hygEnum.periodsEnum.LastMonth].period
                            },
                            {
                                id: hygEnum.periodsEnum.ThisQuarter,
                                text: periodsArray[hygEnum.periodsEnum.ThisQuarter].period
                            },
                            {
                                id: hygEnum.periodsEnum.LastQuarter,
                                text: periodsArray[hygEnum.periodsEnum.LastQuarter].period
                            },
                            {
                                id: hygEnum.periodsEnum.ThisYear,
                                text: periodsArray[hygEnum.periodsEnum.ThisYear].period
                            },
                            {
                                id: hygEnum.periodsEnum.LastYear,
                                text: periodsArray[hygEnum.periodsEnum.LastYear].period
                            },
                            {
                                id: hygEnum.periodsEnum.Yesterday,
                                text: periodsArray[hygEnum.periodsEnum.Yesterday].period
                            }
                        ]
                    },
                    {
                        fieldExpr: "siteIds",
                        label: $scope.language.sites.caption,
                        allowMultiple: true
                    }
                ],
                views: ["day", "week", "month"],
                currentDate: new Date(),
                currentView: "day",
                cellDuration: cellDurationMinutes,
                startDayHour: 7,
                endDayHour: 31,
                height: function () {
                    return window.innerHeight / 1.3;
                },
                onOptionChanged: function (e) {
                    if (e !== null && typeof e !== "undefined" && e.name === "currentView") {
                        userSettings.reportScheduleCurrentView = e.value;
                        // dates are bypassed for "reportSchedule", but needed in method signature, so just passing dashboard dates
                        userService.updateUserSettings("reportSchedule",
                            userSettings,
                            moment.utc(userSettings.dashDateFrom).format(),
                            moment.utc(userSettings.dashDateTo).format()).then(
                            function (response) { },
                            function (response) { console.log(response); }
                            );
                    }
                    //alert("onOptionChanged");
                },
                onAppointmentFormCreated: function (e) {
                    //alert("onAppointmentFormCreated");
                    var form = e.form;
                    var formItems = form.option("items");

                    var preSelectedSites = [];
                    if (e.appointmentData !== null && typeof e.appointmentData !== "undefined" &&
                        e.appointmentData.siteIds !== null && typeof e.appointmentData.siteIds !== "undefined" &&
                        e.appointmentData.siteIds.length > 0) {

                        for (var i = 0; i < e.appointmentData.siteIds.length; i++) { preSelectedSites.push(e.appointmentData.siteIds[i].siteId); }
                    }

                    form.itemOption("allDay", { visible: false }); // hide the all day slider and description field
                    form.itemOption("description", { visible: false });
                    form.itemOption("text",
                        {
                            label: { text: $scope.language.title.caption },
                            validationRules: [{ type: "required", message: $scope.language.missingTitle.caption }],
                            editorOptions: { maxLength: 256 }
                        });
                    form.itemOption("reportId", {
                        label: { text: toTitleCase($scope.language.report.caption) },
                        validationRules: [{ type: "required", message: $scope.language.missingReport.caption }]
                    });
                    form.itemOption("reportPeriod", {
                        label: { text: toTitleCase($scope.language.period.caption) },
                        validationRules: [{ type: "required", message: $scope.language.missingPeriod.caption }]
                    });
                    form.itemOption("siteIds", {
                        validationRules: [{ type: "required", message: $scope.language.missingSite.caption }],
                        editorOptions: {
                            showSelectionControls: true,
                            displayExpr: "name",
                            valueExpr: "siteId",
                            noDataText: "",
                            hideSelectedItems: true,
                            value: preSelectedSites,
                            dataSource: sitesPerAccount
                        }
                    });

                    form.itemOption("startDate", { editorOptions: { type: "datetime", readOnly: false, pickerType: "rollers" } });
                    form.itemOption("endDate", { visible: false });

                    // email list gets duplicated when cancelling and re-adding. check isDataFieldInArray
                    if (!isDataFieldInArray(formItems, "emailList")) {
                        formItems.push(
                            {
                                dataField: "emailList",
                                editorType: "dxTextArea",
                                label: { text: $scope.language.sendTo.caption },
                                helpText: $scope.language.emailList.caption,
                                editorOptions: { maxLength: 1024 },
                                validationRules: [
                                    { type: "required", message: $scope.language.missingEmails.caption },
                                    {
                                        type: "pattern",
                                        pattern:
                                        "^[A-Za-z0-9_.]+@[A-Za-z0-9-]{2,}\.[A-Za-z]{2,}([ ]*,[ ]*[A-Za-z0-9_.]+@[A-Za-z0-9-]{2,}\.[A-Za-z]{2,})*,?$",
                                        message: $scope.language.invalidEmail.caption
                                    }
                                ]
                            }
                        );
                    }

                    form.option({ items: formItems });
                },
                //onAppointmentClick: function (e) { },
                //onAppointmentDblClick: function (e) { },
                onAppointmentAdding: function (e) {
                    //alert("Appointment Adding");
                    var reportSchedule = getReportScheduleFormJSON(e.appointmentData, true);

                    scheduleEmailedReportsService.addReportSchedule(reportSchedule).then(
                        function (response) { getSchedules(false); },
                        function (response) { console.log(response); }
                    );
                },
                //onAppointmentAdded: function (e) { },
                onAppointmentUpdating: function (e) {
                    //alert("Appointment Updating");
                    var reportSchedule = getReportScheduleFormJSON(e.newData, false);
                    scheduleEmailedReportsService.updateReportSchedule(reportSchedule).then(
                        function (response) {
                            getSchedules(false);
                        },
                        function (response) { console.log(response); }
                    );
                },
                //onAppointmentUpdated: function (e) { },
                onAppointmentDeleting: function (e) {
                    //alert("Appointment Deleting");
                    e.cancel = $.Deferred();
                    DevExpress.ui.dialog.confirm($scope.language.deleteSchedule.caption,
                        $scope.language.confirm.caption).done(function (dialogResponse) {
                            e.cancel.resolve(!dialogResponse);
                            // Note: gets here whether or not delete confirmed
                        });
                },
                onAppointmentDeleted: function (e) {
                    //alert("Appointment Deleted");
                    // Note: we get here only if we respond "Yes" to delete prompt from onAppointmentDeleting
                    scheduleEmailedReportsService
                        .deleteReportSchedule(e.appointmentData.accountId, e.appointmentData.scheduleId).then(
                        function (response) { },
                        function (response) { console.log(response); }
                        );
                }
            };
        }

        function getSchedules(isInit) {
            scheduleEmailedReportsService.getReportSchedules().then(
                function (responseSchedules) {
                    if (responseSchedules.data !== null) {
                        $scope.rptSched = responseSchedules.data;
                        prepareSchedules();

                        if (isInit) { setScheduleSource(); }

                        haveAllCollectionsLoaded = true;
                    }
                },
                function (responseSchedules) {
                    console.log(responseSchedules);
                    $scope.rptSched = [];
                }
            );
        }

        function isDataFieldInArray(arrIn, dataField) {
            var isInArray = false;

            if (arrIn !== null && typeof arrIn !== "undefined" && arrIn.length > 0) {
                for (var i = 0; i < arrIn.length; i++) {
                    if (arrIn[i].dataField === dataField) {
                        isInArray = true;
                        break;
                    }
                }
            }

            return isInArray;
        }

        function getReportScheduleFormJSON(rptSched, isAdding) {
            var rptSchedJSON = {};
            var siteIds = [];
            var objSiteId = {};
            var scheduleId = "";

            for (var i = 0; i < rptSched.siteIds.length; i++) {
                objSiteId = {};

                if (isAdding) {
                    objSiteId.id = objSiteId.scheduleId = null;
                    objSiteId.siteId = typeof rptSched.siteIds[i] === "object"
                        ? rptSched.siteIds[i].siteId
                        : rptSched.siteIds[i];
                } else {
                    if (typeof rptSched.siteIds[i] === "object") {
                        objSiteId.id = rptSched.siteIds[i].id;
                        objSiteId.scheduleId = rptSched.siteIds[i].scheduleId;
                        scheduleId = objSiteId.scheduleId; // for non "object" loop iteration (see "else" below)
                        objSiteId.siteId = rptSched.siteIds[i].siteId;
                    } else {
                        objSiteId.siteId = rptSched.siteIds[i];

                        if (scheduleId === "") { scheduleId = rptSched.scheduleId; }

                        objSiteId.scheduleId = scheduleId;
                    }
                }

                siteIds.push(objSiteId);
            }

            rptSchedJSON.scheduleId = isAdding ? null : rptSched.scheduleId;
            rptSchedJSON.emailList = rptSched.emailList;
            rptSchedJSON.startDate = moment.utc(rptSched.startDate).format("lll");
            rptSchedJSON.endDate = moment(rptSchedJSON.startDate).add(cellDurationMinutes, "minutes").format("lll");
            rptSchedJSON.lastSent = isAdding ? null : moment.utc(rptSched.lastSent).format("lll");
            rptSchedJSON.nextSend = isAdding ? null : moment.utc(rptSched.nextSend).format("lll");
            rptSchedJSON.reportId = rptSched.reportId;
            rptSchedJSON.reportPeriod = rptSched.reportPeriod;
            rptSchedJSON.text = rptSched.text; // this is really the schedule title
            rptSchedJSON.siteIds = siteIds; // array of objects of GUIDs
            rptSchedJSON.recurrenceRule = rptSched.recurrenceRule === null || typeof rptSched.recurrenceRule === "undefined"
                ? null
                : rptSched.recurrenceRule;

            return rptSchedJSON;
        }

        function assignSites(sitesIn) {
            // returns a site array with siteId instead of Sites.id
            var sitesOut = [];

            if (sitesIn === null || typeof sitesIn === "undefined") { return sitesOut; }

            for (var i = 0; i < sitesIn.length; i++) {
                siteObj = {};
                siteObj.siteId = sitesIn[i].id;
                siteObj.name = sitesIn[i].name;
                sitesOut.push(siteObj);
            }

            return sitesOut;
        }

        // Note: this is another way (other than onOptionChanged) to track user settings for day, week, month view change.
        //$scope.$watch("rptSchedCurrView", function (e) {
        //    alert(e);
        //});

    }
);