
app.service("resultService",
    function ($http, $filter) {
        return {
            getResults: function (fromDateUtc, toDateUtc) {
                return $http.get("../api/result",
                    {
                        params: { fromUtc: fromDateUtc, toUtc: toDateUtc }
                    });
            },
            getPagedResultsCount: function (fromUtc, toUtc, loadOption) {

                var data = {
                    fromUtc: fromUtc,
                    toUtc: toUtc,
                    loadOption: loadOption
                };

                var config = {
                    params: data,
                    headers: { 'Accept': 'application/json' }
                };

                return $http.get("../api/PagedResultsCount", config);

            },

            getPagedResults: function (fromUtc, toUtc, loadOption) {

                var data = {
                    fromUtc: fromUtc,
                    toUtc: toUtc,
                    loadOption: loadOption
                };

                var config = {
                    params: data,
                    headers: { 'Accept': 'application/json' }
                };


                return $http.get("../api/PagedResults", config);
            },
            updateResult: function (clientResult) {
                var data = $.param({ cr: clientResult });
                var config = { headers: { "Content-Type": "application/x-www-form-urlencoded;charset=utf-8;" } };
                return $http.post("../api/UpdateResult", data, config);
            },
            updateResultGridSchema: function (resultGridSchemaJSON) {
                var data = $.param({ resultGridSchemaJSON: resultGridSchemaJSON });
                var config = { headers: { "Content-Type": "application/x-www-form-urlencoded;charset=utf-8;" } };
                return $http.post("../api/UpdateResultGridSchema", data, config);
            },
            getResultGridSchemaJSON: function () {
                return $http.get("../api/GetResultGridSchema");
            },
            getResultColumns: function (userPermissions, lang, userSettings, resultSchemaJsonString) {

                var baseSchema = [];
                baseSchema.length = 0


                getBaseColObj("", "actionImage", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.resultDate.caption), "resultDate", "date", baseSchema);
                getBaseColObj(toTitleCase(lang.locationName.caption), "locationName", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.unitNo.caption), "unitNo", "number", baseSchema);
                getBaseColObj(toTitleCase(lang.unitName.caption), "unitName", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.plans.caption), "planName", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.userName.caption), "userName", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.lower.caption), "lower", "number", baseSchema);
                getBaseColObj(toTitleCase(lang.upper.caption), "upper", "number", baseSchema);
                getBaseColObj(toTitleCase(lang.rLU.caption), "rlu", "number", baseSchema);
                getBaseColObj(toTitleCase(lang.notes.caption), "notes", "string", baseSchema);               
                getBaseColObj(toTitleCase(lang.group.caption), "groupName", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.surface.caption), "surfaceName", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.createdBy.caption), "createdBy", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.rank.caption), "rank", "number", baseSchema);
                getBaseColObj(toTitleCase(lang.zone.caption), "zone", "string", baseSchema);         
                getBaseColObj(toTitleCase(lang.unitType.caption), "unitType", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.roomNumber.caption), "roomNumber", "number", baseSchema);
                getBaseColObj(toTitleCase(lang.personnel.caption), "personnel", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.correctiveAction.caption), "correctiveAction", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.customColumn1.caption), "customColumn1", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.customColumn2.caption), "customColumn2", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.customColumn3.caption), "customColumn3", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.customColumn4.caption), "customColumn4", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.customColumn5.caption), "customColumn5", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.customColumn6.caption), "customColumn6", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.customColumn7.caption), "customColumn7", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.customColumn8.caption), "customColumn8", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.customColumn9.caption), "customColumn9", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.customColumn10.caption), "customColumn10", "string", baseSchema);
                getBaseColObj(toTitleCase(lang.isResultMarkedDeleted.caption), "isDeleted", "boolean", baseSchema);
                
                if (resultSchemaJsonString) {
                    var arrRsltSchmaCols = angular.fromJson(resultSchemaJsonString);
                    Object.assign(baseSchema, arrRsltSchmaCols);
                }

                setDefaultValues(baseSchema, userSettings, userPermissions)
             
              

                return baseSchema;
            }
        };
        function setDefaultValues(baseSchema, userSettings, userPermissions)
        {            

            $.grep(baseSchema, function (b, index) {

                if (b.dataField === "actionImage") {
                    b.caption = "";
                    b.width = 30;
                    b.cellTemplate = function (container, options) {
                        if (typeof options !== "undefined" && options !== null && options.data !== null) {
                            $("<div class='" + options.data.resultState + "'>&nbsp;</div>").appendTo(container);
                        }
                    };
                    b.allowFiltering = false;
                    b.allowReordering = false;
                    b.allowGrouping = false;
                    b.allowSorting = false;
                    b.showInColumnChooser = false;
                    b.allowResizing = false;
                    baseSchema[index] = b;
                }

                if (b.dataField === "resultDate") {
                   
                    b.allowHiding = false;
                    b.format = userSettings.dateFormatShort;
                    baseSchema[index] = b;
                }
                if (b.dataField === "locationName") {

                    b.allowEditing = userPermissions.canEditResultLocation;
                    baseSchema[index] = b;
                }
                if (b.dataField === "testType") {

                    b.calculateCellValue = function (data) {
                        if (typeof data !== "undefined" && data !== null) {
                            if (data.testType === "repeatReading") { return toTitleCase(lang.repeatReading.caption); }
                            else if (data.testType === "reTested") { return toTitleCase(lang.reTested.caption); }
                            else if (data.testType === "reTestResult") { return toTitleCase(lang.reTestResult.caption); }
                            else { return toTitleCase(lang.normal.caption); }
                        }
                    };
                    baseSchema[index] = b;
                }
                if (b.dataField === "resultState") {

                    b.calculateCellValue = function (data) {
                        if (typeof data !== "undefined" && data !== null) {
                            if (data.resultState === "pass") { return toTitleCase(lang.pass.caption); }
                            else if (data.resultState === "fail") { return toTitleCase(lang.fail.caption); }
                            else { return toTitleCase(lang.caution.caption); }
                        }
                    };
                    baseSchema[index] = b;
                }
                if (b.dataField === "groupName") {

                    b.allowEditing = userPermissions.canEditResultGroup;
                    baseSchema[index] = b;
                }
                if (b.dataField === "surfaceName") {

                    b.allowEditing = userPermissions.canEditResultSurface;
                    baseSchema[index] = b;
                }            
                if (b.dataField === "notes") {

                    b.allowEditing = userPermissions.canEditResultNotes;
                    baseSchema[index] = b;
                }   

                if (b.dataField === "rank") {

                    b.allowEditing = userPermissions.canEditResultRank;
                    baseSchema[index] = b;
                }   
                if (b.dataField === "zone") {

                    b.allowEditing = userPermissions.canEditResultZone;
                    baseSchema[index] = b;
                }   

                if (b.dataField === "isDeleted") {

                    b.allowEditing = userPermissions.canMarkResultsDeleted;
                    b.width = 100;                 
                    b.allowFiltering = true;
                    b.allowHeaderFiltering = true;
                    baseSchema[index] = b;                
               
                    
                }   

            });
        }
        // assigns most common fields. if different, assign individually after this call.
        function getBaseColObj(caption, dataField, dataType, baseObject) {

            getBaseColObjExtension(caption, dataField, dataType, baseObject, "auto", false, true, true
                , true, null, null, true, null);
        };

        function getBaseColObjExtension(caption, dataField, dataType, baseObject, width, allowEditing, showInColumnChooser, allowResizing
            , allowFiltering, calculateCellValue, cellTemplate, allowHiding, format) {
            var objColumn = {};
            objColumn.caption = caption;
            objColumn.dataField = dataField;
            objColumn.dataType = dataType;
            objColumn.width = width;
            objColumn.allowEditing = allowEditing;
            objColumn.showInColumnChooser = showInColumnChooser;
            objColumn.allowResizing = allowResizing;
            objColumn.allowFiltering = allowFiltering;
            objColumn.calculateCellValue = calculateCellValue;
            objColumn.cellTemplate = cellTemplate;
            objColumn.allowHiding = allowHiding;
            objColumn.format = format;


            baseObject.push(objColumn);
        }
    });
