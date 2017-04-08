var myVar;
function ShowMessage(status, text) {
    clearTimeout(myVar);
    var Cssclass = getClassFromStatus(status);
    $("#result .alert").removeClass("alert-info alert-warning alert-success alert-danger");
    $("#result .alert").addClass(Cssclass);
    $(".resultMessage").html(text);
    $("#result").show();
    myVar=setTimeout(function () {
        $("#result").hide('slow');
        $("#result .alert").removeClass(Cssclass);
        $(".resultMessage").html("");
    }, 5000);
}

function getClassFromStatus(status) {
    switch (status) {
        case 0: return "alert-success";
        case 1: return "alert-danger";
        case 2: return "alert-warning";
        case 3: return "alert-info";
        default: return "";
    }
}

