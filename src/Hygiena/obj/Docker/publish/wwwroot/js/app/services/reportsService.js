app.service("reportsService",
    function($http) {
        return {
            generateReport: function (type, fromDateUtc, toDateUtc) {
                return $http.get("../api/generateReport",
                    {
                        params: { reportType: type, fromUtc: fromDateUtc, toUtc: toDateUtc  }
                    });
            },
            getAllData: function() {
                return [];
            },
            getTestReports: function () {

                var result = [];
                for (var i = 0; i < 20; i++) {
                    result.push({
                        accountId: i,
                        actualIncubationTime: null,
                        controlExperationDate: null,
                        controlLotNumber: null,
                        controlModifiedBy: null,
                        controlModifiedDate: null,
                        controlName: null,
                        correctedTest: null,
                        correctiveAction: null,
                        createdBy: "MPC\vmudinas",
                        createdDate: "2016-11-07T13:45:16.237",
                        customField1: "",
                        customField2: "",
                        customField3: "",
                        customField4: "",
                        customField5: "",
                        customField6: "",
                        customField7: "",
                        customField8: "",
                        customField9: "",
                        customField10: "",
                        deviceCategory: null,
                        deviceName: null,
                        deviceTemprature: null,
                        deviceUOM: null,
                        dilution: null,
                        groupName: "",
                        incubationTime: null,
                        isDeleted: false,
                        locationName: "Location-1" + i,
                        lower: 10,
                        modifiedBy: "MPC\vmudinas",
                        modifiedDate: null,
                        notes: "",
                        personnel: null,
                        rank: 0,
                        rawADCOutput: 0,
                        repeatedTest: null,
                        result: false,
                        resultDate: "2017-02-07T22:27:00",
                        resultId: "68b7eae8-ab4e-4e51-93d8-34ffd90a482b",
                        rlu: 4,
                        roomNumber: null,
                        sampleType: null,
                        siteId: "3bfc272a-9a7f-427f-831a-d44e57b4f1eb",
                        surfaceName: "",
                        planName: "Plan 19",
                        testState: 3,
                        unitAngle: null,
                        unitName: "Unit - 69293",
                        unitNo: 69293,
                        unitSoftware: null,
                        unitType: "EnSURE v2",
                        upper: 44,
                        userName: "User 2",
                        warningId: 0,
                        zone: ""
                    });
                }
                return result;
         
            }

        };
    });