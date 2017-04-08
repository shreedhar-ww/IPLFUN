
var oTableSchedule;
$(document).ready(function () {
    getGrid();
});

function getGrid() {
    oTableSchedule = $('#MatchList').dataTable({
        "sAjaxSource": "/IPL/GetSchedule",
        "aoColumns": [
            { "mData": "Day", "sTitle": "Day" },
            { "mData": "TeamA", "sTitle": "Team A" },
            { "mData": "TeamB", "sTitle": "Team B" },
            { "mData": "SetQuestions", "bSortable": false, "sTitle": "Set Questions" },
            { "mData": "SetAnswers", "bSortable": false, "sTitle": "Set Answers" },
        ],
        "aLengthMenu": [[10, 25, 50, 100, 1000], [10, 25, 50, 100, "All"]],
        "iDisplayLength": 10,
        "bDestroy": true,
        "bFilter": false,
        "bInfo": true,
        "bSortCellsTop": true
    });
}

function fn_setMatchDetails(matchId, teamA, teamB) {
    $.ajax({
        type: "POST",
        url: "/IPL/SetTempDataForMatchDetails",
        data: { matchId: matchId, teamA: teamA, teamB: teamB },
        success: function () {
            window.location.href = '/IPL/SetBidQuestions';
        }
    });
}

function fn_setMatchDetailsForAnswers(matchId, teamA, teamB) {
    $.ajax({
        type: "POST",
        url: "/IPL/SetTempDataForMatchDetails",
        data: { matchId: matchId, teamA: teamA, teamB: teamB },
        success: function () {
            window.location.href = '/IPL/SetBidAnswers';
        }
    });
}

function fn_SetQuestions() {
    debugger;
    var matchId = $("#matchId").val();
    var questions = [];
    var rowCount = 0;
    $(".TeamA tr").each(function () {
        if (rowCount != 0) {
            var bidId = $(this).attr("id");
            var isActive = $(this).find("input[type='checkbox']").prop("checked");
            var bidPoint = $(this).find(".bidPoint").val();
            if (isActive == true)
                questions.push({ BidId: bidId, BidPoint: bidPoint });
        }
        rowCount++;
    });
    rowCount = 0;
    $(".TeamB tr").each(function () {
        if (rowCount != 0) {
            var bidId = $(this).attr("id");
            var isActive = $(this).find("input[type='checkbox']").prop("checked");
            var bidPoint = $(this).find(".bidPoint").val();
            if (isActive == true)
                questions.push({ BidId: bidId, BidPoint: bidPoint });
        }
        rowCount++;
    });
    questions = JSON.stringify({ 'questions': questions, 'matchId': matchId });
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: '/IPL/SaveBidQuestions',
        data: questions,
        success: function (response) {
            ShowMessage(response.data.Status, response.data.Message);
        },
        failure: function (response) {
        }
    });
}

function fn_ActivateQuestion(val, bidId) {
    if (val == "Yes") val = true; else val = false;
    var matchId = $("#matchId").val();
    $.ajax({
        type: "POST",
        url: "/IPL/ActivateBidQuestion",
        data: { bidId: bidId, val: val, matchId: matchId },
        success: function (response) {
            ShowMessage(response.data.Status, response.data.Message);
        }
    });
}

function fn_SubmitResult(val, bidId) {
    if (val == "Yes") val = true; else val = false;
    var matchId = $("#matchId").val();
    $.ajax({
        type: "POST",
        url: "/IPL/SubmitBidResult",
        data: { bidId: bidId, val: val, matchId: matchId },
        success: function (response) {
            ShowMessage(response.data.Status, response.data.Message);
            $("tr[id=" + bidId + "]").addClass("strikeout");
            $("tr[id=" + bidId + "]").find("button").attr("disabled", "disabled");
        }
    });
}

function fn_SubmitBidResultByBidder(val, masterId) {
    if (val == "Yes") val = true; else val = false;
    $.ajax({
        type: "POST",
        url: "/IPL/SubmitBidResultByBidder",
        data: { masterId: masterId, val: val },
        success: function (response) {
            if (response.data.Status == 0) {
                $(".hide-for-success").hide();
                $(".for-success").show();
            } else
                ShowMessage(response.data.Status, response.data.Message);
        }
    });
}