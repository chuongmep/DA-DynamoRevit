$(document).ready(function () {
    createAppBundleActivity();
    prepareLists();

    $('#clearAccount').click(clearAccount);
    $('#startWorkitem').click(startWorkitem);

    startConnection();
});

function prepareLists() {
    list('activity', '/api/aps/designautomation/activities');
    list('engines', '/api/aps/designautomation/engines');
    list('localBundles', '/api/appbundles');
}

function list(control, endpoint) {
    $('#' + control).find('option').remove().end();
    jQuery.ajax({
        url: endpoint,
        success: function (list) {
            if (list.length === 0)
                $('#' + control).append($('<option>', {
                    disabled: true,
                    text: 'Nothing found'
                }));
            else
                list.forEach(function (item) {
                    $('#' + control).append($('<option>', {
                        value: item,
                        text: item
                    }));
                });
        }
    });
}

function clearAccount() {
    if (!confirm('Clear existing activities & appbundles before start. ' +
        'This is useful if you believe there are wrong settings on your account.' +
        '\n\nYou cannot undo this operation. Proceed?'))
        return;

    jQuery.ajax({
        url: 'api/aps/designautomation/account',
        method: 'DELETE',
        success: function () {
            writeLog('Account cleared, all appbundles & activities deleted');
            prepareLists();

        }
    });
}

function createAppBundleActivity() {
    startConnection(function () {
        writeLog("Checking for Dynamo appbundle and activity on Revit 2024...");
        createAppBundle(function () {
            createActivity(function () {
                prepareLists();
            });
        });
    });
}

function createAppBundle(cb) {
    jQuery.ajax({
        url: 'api/aps/designautomation/appbundles',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            zipFileName: $('#localBundles').val(),
            engine: $('#engines').val()
        }),
        success: function (res) {
            if (res.diagnostic) {
                writeLog('AppBundle: ' + res.diagnostic);
            } else {
                writeLog('AppBundle: ' + res.appBundle + ', v' + res.version);
            }
            if (cb)
                cb();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            writeLog(' -> ' + (xhr.responseJSON && xhr.responseJSON.diagnostic ? xhr.responseJSON.diagnostic : thrownError));
        }
    });
}

function createActivity(cb) {
    jQuery.ajax({
        url: 'api/aps/designautomation/activities',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            zipFileName: "D4DA",
            engine: "Autodesk.Revit+2024"
        }),
        success: function (res) {
            writeLog('Activity: ' + res.activity);
            if (cb)
                cb();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            writeLog(' -> ' + (xhr.responseJSON && xhr.responseJSON.diagnostic ? xhr.responseJSON.diagnostic : thrownError));
        }
    });
}

function startWorkitem() {
    var inputRevitFile = document.getElementById('inputRevitFile');
    var inputDynamoFile = document.getElementById('inputDynamoFile');
    if (inputRevitFile.files.length === 0 || inputDynamoFile.files.length === 0) {
        alert('Please set the input files');
        return;
    }
    if ($('#activity').val() === null)
        return (alert('Please select an activity'));
    var file1 = inputRevitFile.files[0];
    var file2 = inputDynamoFile.files[0]; //[string(inputRevitFile.files[0]), inputDynamoFile.files[0]]

    startConnection(function () {
        let formData = new FormData();
        formData.append('filesToUpload', file1);
        formData.append('filesToUpload', file2);
        formData.append('data', JSON.stringify({
            activityName: $('#activity').val(),
            browerConnectionId: connectionId
        }));
        writeLog('Uploading input file...');
        $.ajax({
            url: 'api/aps/designautomation/workitems',
            data: formData,
            processData: false,
            contentType: false,
            //contentType: 'multipart/form-data',
            //dataType: 'json',
            type: 'POST',
            success: function (res) {
                writeLog('Workitem started: ' + res.workItemId);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                writeLog(' -> ' + (xhr.responseJSON && xhr.responseJSON.diagnostic ? xhr.responseJSON.diagnostic : thrownError));
            }
        });
    });
}

function writeLog(text) {
    $('#outputlog').append('<div style="border-top: 1px dashed #C0C0C0">' + text + '</div>');
    var elem = document.getElementById('outputlog');
    elem.scrollTop = elem.scrollHeight;
}

var connection;
var connectionId;

function startConnection(onReady) {
    if (connection && connection.connected) {
        if (onReady)
            onReady();
        return;
    }
    connection = io();
    connection.on('connect', function () {
        connectionId = connection.id;
        if (onReady)
            onReady();
    });

    connection.on('downloadResult', function (url) {
        writeLog('<a href="' + url + '">Download result file here</a>');
    });

    connection.on('downloadReport', function (url) {
        writeLog('<a href="' + url + '">Download report file here</a>');
    });

    connection.on('onComplete', function (message) {
        if (typeof message === 'object')
            message = JSON.stringify(message, null, 2);
        writeLog(message);
    });
}
