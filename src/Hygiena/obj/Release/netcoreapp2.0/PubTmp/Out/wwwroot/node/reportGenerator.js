"use strict";
module.exports.vitas = function (callback, reportUrl, reportTitle) {


    var greet = function (file) {

        return file;
    };

    var pathToFile = `.//wwwroot//node//reports`;

    const Pageres = require('pageres');

    const pageres = new Pageres({ delay: 50, filename: reportTitle })
        .src(reportUrl, ['1470x1024'])
        .dest(pathToFile)
        .run()
        .then(() => callback(null, pathToFile + "//" + reportTitle + ".png"));



};