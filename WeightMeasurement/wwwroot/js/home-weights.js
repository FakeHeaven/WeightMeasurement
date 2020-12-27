$(function () {
    $("#weight-add").on("click", function () {
        LoadWeightManageModal(0);
    });

    $(document).on("click", ".weight-edit", function () {
        LoadWeightManageModal($(this).data("id"));
    });

    $(document).on("click", ".weight-delete", function () {
        DeleteWeight($(this).data("id"), $(this).data("name"));

    });
});

function LoadWeightManageModal(id) {
    $.ajax({
        url: "/Home/WeightManage?id=" + id,
        method: "GET",
        success: function (html) {
            $("#modal").html(html);
            $('#modal').modal(modalOptions, 'show');
            AttachWeightFormValidation();

        }
    });
}


function RetrieveWeightList() {
    $.ajax({
        url: "/Home/RetrieveWeightList",
        method: "GET",
        success: function (html) {
            $("#weights").html(html);

        }
    });
}

function DeleteWeight(id, name) {
    Confirm("Remove weight", "fa-exclamation-triangle", "text-danger", `Are you sure you want to remove <strong>${name}</strong> from the list?`,
        function () {
            $.ajax({
                url: "/Home/WeightDelete?id=" + id,
                method: "GET",
                success: function (result) {
                    if (result.success) {
                        toastr.success("Weight successfully removed.");  
                        RetrieveWeightList();
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
                
            }
        },
        messages: {
            name: {
                required: "Please supply a name for this Sub User.",
                pattern: "This value can only contain alphabetic characters."
            },

            weight: {
                required: "Please supply a name for this Sub User.",
                
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
                    weight: form.weight.value
                },
                success: function (result) {
                    if (result.success) {
                        toastr.success("New weight successfully added.");
                        RetrieveWeightList();
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