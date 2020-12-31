"use strict";

var gulp = require("gulp");
var concat = require("gulp-concat");

function npmCSS() {
    return gulp
        .src([
            "../node_modules/bootstrap/dist/css/bootstrap.min.css",
            "../node_modules/datatables.net-bs4/css/dataTables.bootstrap4.min.css",
            "../node_modules/datatables.net-responsive-bs4/css/responsive.bootstrap4.min.css",
            "../node_modules/toastr/build/toastr.min.css",
            "../node_modules/@claviska/jquery-alertable/jquery.alertable.css",
            "../node_modules/gijgo/css/gijgo.min.css",
            "../node_modules/bootstrap4-toggle/css/bootstrap4-toggle.min.css",
        ])
        .pipe(concat("vendor.css"))
        .pipe(gulp.dest("wwwroot/css"));
}

function npmJS() {
    return gulp
        .src([
            "../node_modules/jquery/dist/jquery.min.js",
            "../node_modules/bootstrap/dist/js/bootstrap.min.js",
            "../node_modules/jquery-validation/dist/jquery.validate.min.js",
            "../node_modules/jquery-validation/dist/additional-methods.js",
            "../node_modules/jquery-mask-plugin/dist/jquery.mask.min.js",
            "../node_modules/toastr/build/toastr.min.js",
            "../node_modules/datatables.net/js/jquery.dataTables.js",
            "../node_modules/datatables.net-bs4/js/dataTables.bootstrap4.min.js",
            "../node_modules/datatables.net-responsive/js/dataTables.responsive.min.js",
            "../node_modules/datatables.net-responsive-bs4/js/responsive.bootstrap4.min.js",
            "../node_modules/@claviska/jquery-alertable/jquery.alertable.min.js",
            "../node_modules/gasparesganga-jquery-loading-overlay/dist/loadingoverlay.min.js",
            "../node_modules/gijgo/js/gijgo.min.js",
            "../node_modules/bootstrap4-toggle/js/bootstrap4-toggle.min.js"
        ])
        .pipe(concat("vendor.js"))
        .pipe(gulp.dest("wwwroot/js"));
}

exports.vendor = gulp.series(gulp.parallel(npmCSS, npmJS));