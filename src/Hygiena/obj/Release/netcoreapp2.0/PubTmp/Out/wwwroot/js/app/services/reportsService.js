app.service("reportsService",
    function($http) {
        return {
            generateReport: function(type, fromDateUtc, toDateUtc) {
                return $http.get("../api/GenerateReport",
                {
                    params: { reportType: type, fromUtc: fromDateUtc, toUtc: toDateUtc }
                });
            },
            getAllData: function (fromDateUtc, toDateUtc) {
                return $http.get("../api/GetReportData",
                    {
                        params: { fromUtc: fromDateUtc, toUtc: toDateUtc }
                    });
            },
            getReport: function ( type, id) {
                

               let result =  {
                   bindto: `#${id}`,
                   size: {},
                   data: {
                       type: type,
                        empty: {
                            label: {
                                text: "No Data Available"
                            }
                        }
                        },
                        title: {
                            text: id.replace("_id_", "").replace("_", "")
                        },
                        color:
                        {
                            pattern: ["red", "green", "orange", "blue", "#1f77b4", "#aec7e8", "#ff7f0e", "#ffbb78", "#2ca02c", "#98df8a", "#d62728", "#ff9896", "#9467bd", "#c5b0d5", "#8c564b", "#c49c94", "#e377c2", "#f7b6d2", "#7f7f7f", "#c7c7c7", "#bcbd22", "#dbdb8d", "#17becf", "#9edae5"]
                        }
               }



               return result;

         
            },
            getTestReports: function() {

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