$(function () {
    $(document).on("click", ".graph-open", function () {
        $.ajax({
            url: "/Home/GraphDateRangeSelecter?subuserid=" + $(this).data("subuser-id"), 
            method: "GET",
            success: function (html) {
                $("#modal").html(html);
                $('#modal').modal(modalOptions, 'show');
                AttachSubUserFormValidation();

                $('#date-start').datepicker({
                    uiLibrary: 'bootstrap4',
                    format: 'dd.mm.yyyy'
                });

                $('#date-end').datepicker({
                    uiLibrary: 'bootstrap4',
                    format: 'dd.mm.yyyy'
                });
            }
        });
    });

});


function AttachSubUserFormValidation() {
    $("#form").validate({
        errorElement: 'div',
        errorLabelContainer: '#validate-error',
        rules: {

            date_start: {
                required: true,
            },
            date_end: {
                required: true,
            }
        },
        messages: {
            date_start: {
                required: "Please select a start date."
            },

            date_end: {
                required: "Please select an end date."
            }
        },
        submitHandler: function (form) {

            location.href = `/graph?subuserid=${form.subuserid.value}&startdate=${form.date_start.value}&enddate=${form.date_end.value}`
        }
    });
}