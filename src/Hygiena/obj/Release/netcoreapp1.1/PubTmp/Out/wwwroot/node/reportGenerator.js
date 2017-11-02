"use strict";
module.exports.vitas = function(callback, reportUrl, reportTitle) {

   
    var greet = function (file) {

        return file;
    };

    //const fs = require("fs");
    //const screenshot = require("screenshot-stream");
    //${reportTitle}.jpg
     var pathToFile = `.//wwwroot//node//reports`;

const Pageres = require('pageres');

greet()

const pageres = new Pageres({ delay: 2, filename: reportTitle})  
    .src(reportUrl, ['1024x768'])
    .dest(pathToFile)
    .run()
    .then(() => callback(null, pathToFile + "//" + reportTitle + ".png"));

    



//const printscreen = require("mt-printscreen");
 
//    printscreen(reportUrl, {
 
//  viewport: {
//    width: 1650,
//    height: 1060
//  },
//  timeout: 10000,
//  format: "jpeg",
//  dir: pathToFile,
//  fileName: reportTitle,
//  quality: 25
 
//}, (err, data) => {

//   
//  /*
//   * Optional: Callback definition
//   * data is the result returned from the capture method
//   */
//  //require('fs').stat(data.file, (err, stats) =>
//  //  console.log(`
//  //    - There are ${data.output.divs} divs in this page.
//  //    - Your screenshot is available at ${data.file} and is ${stats.size} bytes.
//  //  `));
//});

};