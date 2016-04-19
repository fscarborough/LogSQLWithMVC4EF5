$(function () {
    InitializeHiddenSqlLogDialog();
});

function InitializeHiddenSqlLogDialog() {
    $('#sql-log-dialog')
        .dialog({
            height: 600,
            width: 900,
            autoOpen: false,
            modal: true,
            title: "Entity Framework Log",
            buttons:
            {
                "Close": function () { CloseSqlLogDialog(); }
            }
        });
}

function ShowSqlLogDialog() {
    $('#sql-log-dialog').dialog('open');
}

function CloseSqlLogDialog() {
    $('#sql-log-dialog').dialog('close');
}

function LoadDebugLogFile() {
    var userName = "";

    $.when(GetLogInformation()).then(function (data) {
        userName = data.userName;

        $.ajax({
            type: "GET",
            url: "/Logs/" + userName + "/debuglog.txt",
            dataType: "text",
            success: function (logdata) {
                $("#logTextarea").text(logdata);
                ShowSqlLogDialog();
            }
        });
    });

};

function GetLogInformation() {
    return $.ajax({
        type: 'GET',
        url: '/Logging/GetDebugLogInformation',
        async: true,
        data: {}
    });
}
