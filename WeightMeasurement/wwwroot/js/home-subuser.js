$(function () {
    $("#subuser-add").on("click", function () {
        LoadSubUserManageModal(0);
    });

    $(document).on("click", ".subuser-edit", function () {
        LoadSubUserManageModal($(this).data("id"));
    });

    $(document).on("click", ".subuser-delete", function () {
        DeleteSubUser($(this).data("id"), $(this).data("name"));

    });
});

function LoadSubUserManageModal(id) {
    $.ajax({
        url: "/Home/SubUserManage?id=" + id,
        method: "GET",
        success: function (html) {
            $("#modal").html(html);
            $('#modal').modal(modalOptions, 'show');
            AttachSubUserFormValidation();

            $('#dob').datepicker({
                format: 'd.m.yyyy'
            });
        }
    });
}

function RetrieveSubUserList(userId) {
    $.ajax({
        url: "/Home/RetrieveSubUserList?userId=" + userId,
        method: "GET",
        success: function (html) {
            $("#subusers").html(html);
            
        }
    });
}

function DeleteSubUser(id, name) {
    Confirm("Remove SubUser", "fa-exclamation-triangle", "text-danger", `Are you sure you want to remove <strong>${name}</strong> from the list?`,
        function () {
            $.ajax({
                url: "/Home/SubUserDelete?id=" + id,
                method: "GET",
                success: function (result) {
                    if (result.success) {
                        toastr.success("Sub User successfully removed.");
                        RetrieveSubUserList(userId);
                    } else {
                        toastr.error("Sub User could not be removed.");
                    }
                },
                error: function () {
                    toastr.error("Sub User could not be removed.");
                }
            });
        });

    
}

function AttachSubUserFormValidation() {
    $("#form").validate({
        errorElement: 'div',
        errorLabelContainer: '#validate-error',
        rules: {
            name: {
                required: true,
                pattern: /^[A-z]+$/
            },

            dob: {
                required: true,
                
            }
        },
        messages: {
            name: {
                required: "Please supply a name for this Sub User.",
                pattern: "This value can only contain alphabetic characters."
            },

            dob: {
                required: "Please supply a name for this Sub User.",
                
            }
        },
        submitHandler: function (form) {
            $.ajax({
                url: "/Home/SubUserUpdate",
                cache: false,
                type: "POST",
                data: {
                    id: form.id.value,
                    name: form.name.value,
                    dob: form.dob.value
                },
                success: function (result) {
                    if (result.success) {
                        toastr.success("New Sub User successfully added.");
                        RetrieveSubUserList(userId);
                    } else {
                        toastr.error("New Sub User could not be added.");
                    }
                },
                error: function () {
                    toastr.error("New Sub User could not be added.");
                }
            });

            $('#modal').modal('hide');
        }
    });
}