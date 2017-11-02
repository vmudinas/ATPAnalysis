
app.service("gridDefinitionService",
    function($http, language) {
        return {
            getEditText: function() {

                return {
                    addRow: language.addRow,
                    cancelAllChanges: language.cancelAllChanges,
                    cancelRowChanges: language.cancelRowChanges,
                    confirmDeleteMessage: language.confirmDeleteMessage,
                    confirmDeleteTitle: language.confirmDeleteTitle,
                    deleteRow: language.deleteRow,
                    editRow: language.editRow,
                    saveAllChanges: language.saveAllChanges,
                    saveRowChanges: language.saveRowChanges,
                    undeleteRow: language.undeleteRow,
                    validationCancelChanges: language.validationCancelChanges

                };
            },
            getcolumnChooser: function(language, enabled, height, width, mode) {
                return {
                    emptyPanelText: language.emptyPanelText,
                    enabled: enabled,
                    height: height,
                    mode: mode,
                    title: language.columnChooserTitle,
                    width: width

                };
            }
        };
    });