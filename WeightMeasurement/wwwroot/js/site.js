// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var modalOptions = {
    keyboard: false,
    backdrop: "static"
};

var language = {
    emptyTable: '<span class="text-danger">No data available</span>'
};

//Alert for delete 
function Alert(title, icon, color, body, callback) {
    $.alertable.alert("<strong>" + title + "</strong><br /><br /><i class='fas " + icon + " " + color + "'></i> " + body, {
        html: true
    }).always(callback);
}

function Confirm(title, icon, color, body, callback) {
    $.alertable.confirm("<strong>" + title + "</strong><br /><br /><i class='fas " + icon + " " + color + "'></i> " + body, {
        html: true
    }).then(callback);
}

$('[data-toggle="tooltip"]').tooltip()

