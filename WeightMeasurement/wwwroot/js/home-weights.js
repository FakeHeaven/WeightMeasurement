$(function () {
    $(document).on("click", "#weight-add", function () {
        LoadWeightManageModal(0, $(this).data("subuser-id"));

    });

    $(document).on("click", ".weight-edit", function () {
        LoadWeightManageModal($(this).data("id"), $(this).data("subuser-id"));
    });

    $(document).on("click", ".weight-delete", function () {
        DeleteWeight($(this).data("id"), $(this).data("name"));
    });

    $(document).on("click", ".weight-view", function () {
        RetrieveWeightList($(this).data("subuser-id"));

    });

    AttachTableRules();
});

function LoadWeightManageModal(id, subuserid) {
    $.ajax({
        url: "/Home/WeightManage?id=" + id + "&subuserid=" + subuserid,
        method: "GET",
        success: function (html) {
            $("#modal").html(html);
            $('#modal').modal(modalOptions, 'show');
            AttachWeightFormValidation();

            $('#date').datepicker({              
                format: 'd.m.yyyy'
            });
        }
    });
}

function AttachTableRules() {
    $("#weights-table").DataTable({
        saveState: true,
        pagingType: "full",
        pageLength: 25,
        lengthChange: false,
        searching: true,
        ordering: false,
        length: false,
        info: false,
        dom: 'fBrtip',
        language: language
    });
}

function RetrieveWeightList(subUserId) {
    $.ajax({
        url: "/Home/RetrieveWeightList?subUserId=" + subUserId,
        method: "GET",
        success: function (html) {
            $("#weights").html(html);
            AttachTableRules();
        }
    });
}

function DeleteWeight(id, name) {
    Confirm("Remove weight", "fa-exclamation-triangle", "text-danger", `Are you sure you want to remove <strong>${name}'s</strong> weight record?`,
        function () {
            $.ajax({
                url: "/Home/WeightDelete?id=" + id,
                method: "GET",
                success: function (result) {
                    if (result.success) {
                        toastr.success("Weight successfully removed.");
                        RetrieveWeightList(subUserId);
                    } else {
                        toastr.error("Weight could not be removed.");
                    }
                },
                error: function () {
                    toastr.error("Weight could not be removed.");
                }
            });
        });
}

function AttachWeightFormValidation() {
    $("#form").validate({
        errorElement: 'div',
        errorLabelContainer: '#validate-error',
        rules: {
            name: {
                required: true,
                pattern: /^[A-z]+$/
            },

            weight: {
                required: true, 
            },

            date: {
                required: true,
            }

        },
        messages: {
            name: {
                required: "Please supply a name for this Sub User.",
                pattern: "This value can only contain alphabetic characters."
            },

            weight: {
                required: "Please supply a name for this Sub User.",
                
            },

            date: {
                required: "Please select a date for this weight entry.",
            }


        },
        submitHandler: function (form) {
            $.ajax({
                url: "/Home/WeightUpdate",
                cache: false,
                type: "POST",
                data: {
                    id: form.id.value,
                    subuserid: form.subuserid.value,
                    weight: form.weight.value,
                    date: form.date.value
                },
                success: function (result) {
                    if (result.success) {
                        toastr.success("New weight successfully added.");
                        RetrieveWeightList(subUserId);
                    } else {
                        toastr.error("New weight could not be added.");
                    }
                },
                error: function () {
                    toastr.error("New weight could not be added.");
                }
            });

            $('#modal').modal('hide');
        }
    });
}