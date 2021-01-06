var homeBtn;
var authenticationKeyBtn;
var reloadAuthenticationKeysBtn;
var authenticationKeysTable;
var authenticationKeyInput;
var authenticationKeyInputBtn;

/**
 * Hides all child elements of pages div
 */
function HideEverything()
{
    $('#pages').children().hide();
}

/**
 * Show specific page in HTML
 * @param {string} pageName The name of the page to be shown 
 */
function Show(pageName)
{
    HideEverything();
    $('#' + pageName + '-Page').show();
    $('#' + pageName + '-Page').css('height', 'auto');
}

function AddKey(key) {
    var settings = {
        "url": "../api/adminapi",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": '{"authenticationID":"' + authenticationKey + '","requestID":"addKey","requestData":"' + key + '"}',
    };

    $.ajax(settings)
        .done(function (response) {
            response = JSON.parse(response);

            ReloadKeys();

            authenticationKeyInput.value = '';
        });
}

function RemoveKey(key)
{
    var settings = {
        "url": "../api/adminapi",
        "method": "POST",
        "timeout": 0,
        "headers": {
          "Content-Type": "application/json"
        },
        "data": '{"authenticationID":"' + authenticationKey + '","requestID":"removeKey","requestData":"' + key + '"}',
    };

    $.ajax(settings)
        .done(function (response)
        {
            response = JSON.parse(response);
            if (response.response)
                ReloadKeys();
        });
}

function ReloadKeys()
{
    var settings = {
        "url": "../api/adminapi",
        "method": "POST",
        "timeout": 0,
        "headers": {
          "Content-Type": "application/json"
        },
        "data": '{"authenticationID":"' + authenticationKey + '","requestID":"getAllKeys","requestData":""}',
    };
      
    $.ajax(settings)
        .done(function (response) {
            response = JSON.parse(response);
            var tableHTML = '<table class="table"><thead><th scope="col">Key</th><th scope="col">Last Active</th><th scope="col">Action</th></thead><tbody>';
            for (var i = 0; i < response.length; i++)
                tableHTML += '<tr><th scope="row">' + response[i].Item1 + '</th><td>' + (((new Date() - new Date(response[i].Item3)) < 7 * 1000) ? '🟢 Active' : '🔴 Inactive') + '</td><td><button class="btn btn-danger" type="button" onclick="RemoveKey(\'' + response[i].Item1 + '\');">Remove Key</button></td></tr>';

            tableHTML += '</tbody></table>';

            authenticationKeysTable.innerHTML = tableHTML;
        });
}

$(function()
{
    authenticationKeyBtn = document.getElementById('authenticationKeyBtn');
    reloadAuthenticationKeysBtn = document.getElementById('reloadAuthenticationKeys');
    authenticationKeysTable = document.getElementById('authenticationKeysTable');
    authenticationKeyInput = document.getElementById('authenticationKeyInput');
    authenticationKeyInputBtn = document.getElementById('authenticationKeyInputBtn');

    $('#pages').children().css('height', '0');

    authenticationKeyBtn.onclick = function() { Show('Authentication'); }
    reloadAuthenticationKeysBtn.onclick = ReloadKeys;
    authenticationKeyInputBtn.onclick = function () { AddKey(authenticationKeyInput.value); }
    HideEverything();

    authenticationKeyInput.addEventListener('keyup', function (e) {
        if (e.keyCode == 13) {
            event.preventDefault();
            authenticationKeyInputBtn.click();
        }
    });

    ReloadKeys();

    setInterval(ReloadKeys, 5000);
})