$(function () {

    AttachTableRules();
    $(document).on("change", ".user-toggle", function () {
        var userId = $(this).data("user-id");
        var isChecked = $(this).prop("checked")
        $.ajax({
            url: "/Admin/UserStatusToggle",
            method: "POST",
            data: {
                userId: userId,
                isChecked: isChecked
            },
            success: function (result) {
                if (result.success) {
                    toastr.success("Sub User successfully removed.");
                } else {
                    toastr.error("Sub User could not be removed.");
                }
            },
            error: function () {
                toastr.error("User could not be activated. hehe idk maybe");
            }
        });
    });

});


function AttachTableRules() {
    $("#users-table").DataTable({
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
