


function toTitleCase(str) {
    if (str === undefined || str === null) {
        return "";
    }
    return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
}

function toCamelCase(str) {
    if (str === undefined || str === null) {
        return "";
    }
    return str.replace(/\w\S*/g, function (txt) { return txt.charAt(0).toLowerCase() + txt.substr(1); });
}

var contDisp = function () {
    const r = window.$("html").height();
    return r;
};



function getDates(startDate, stopDate) {
    const dateArray = new Array();
    var currentDate = startDate;
    while (currentDate <= stopDate) {
        dateArray.push(new Date(currentDate));
        currentDate = window.moment(currentDate).add(1, "days");
    }
    return dateArray;
}