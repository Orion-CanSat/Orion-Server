var homeBtn;
var authenticationKeyBtn;
var reloadAuthenticationKeysBtn;
var authenticationKeysTable;

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
      
    $.ajax(settings).done(function (response) {
        response = JSON.parse(response);
        var tableHTML = '<table class="table"><thead><th scope="col">Key</th></thead><tbody>';
        for (var i = 0; i < response.length; i++)
            tableHTML += '<tr><th scope="row">' + response[i] + '</th></tr>';

        tableHTML += '</tbody></table>';
    
        authenticationKeysTable.innerHTML = tableHTML;
    });
}

$(function()
{
    homeBtn = document.getElementById('homeBtn');
    authenticationKeyBtn = document.getElementById('authenticationKeyBtn');
    reloadAuthenticationKeysBtn = document.getElementById('reloadAuthenticationKeys');
    authenticationKeysTable = document.getElementById('authenticationKeysTable');

    $('#pages').children().css('height', '0');

    homeBtn.onclick = function() { Show('Index'); }
    authenticationKeyBtn.onclick = function() { Show('Authentication'); }
    reloadAuthenticationKeysBtn.onclick = ReloadKeys;

    HideEverything();
    Show('Index');

    ReloadKeys();

    setInterval(ReloadKeys, 5000);
})