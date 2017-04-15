
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
        "bSortCellsTop": true,
        "bLengthChange": false,
    });
}

function fn_registerNewUser() {    
    $("#add-user").load("/User/CreateUser");
}

//function fn_submitForm()
//{
//    $.ajax({
//        type: "POST",
//        url: "/IPL/SubmitBidResultByBidder",
//        data: { masterId: masterId, val: val },
//        success: function (response) {
//            $("#userModal").modal("toggle");
//            ShowMessage(response.data.Status, response.data.Message);
//            getGrid();
//        }
//    });
//}

function fn_RegistrationSuccess(response) {
    debugger;
    $("#userModal").modal("toggle");
    ShowMessage(response.data.Status, response.data.Message);
    getGrid();
}



