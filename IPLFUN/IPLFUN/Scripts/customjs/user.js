
var oTableChannel;
$(document).ready(function () {
    getGrid();
});

function getGrid() {
    oTableChannel = $('#UserList').dataTable({
        "sAjaxSource": "/User/GetUsers",
        "aoColumns": [
            { "mData": "Name", "sTitle": "Name" },
            { "mData": "Mobile", "sTitle": "Mobile Number" },
            { "mData": "EmailId", "sTitle": "Email" },
            { "mData": "Points", "sTitle": "points" },
            { "mData": "Action", "bSortable": false, "sTitle": "Action" },
        ],
        "aLengthMenu": [[10, 25, 50, 100, 1000], [10, 25, 50, 100, "All"]],
        "iDisplayLength": 10,
        "bDestroy": true,
        "bFilter": false,
        "bInfo": true,
        "bSortCellsTop": true
    });
}

function fn_registerNewUser() {    
    $("#add-user").load("/User/CreateUser");
}

function fn_RegistrationSuccess(data) {
    debugger;
    $("#userModal").modal("toggle");
    ShowMessage(data.Status, data.Message);
    getGrid();
}

function fn_RegistrationFailure()
{
    alert("failed");
}

