var homeBtn;

var authenticationKeyBtn;
var reloadAuthenticationKeysBtn;
var authenticationKeysTable;
var authenticationKeyInput;
var authenticationKeyInputBtn;

var moduleBtn;
var reloadModulesBtn;
var moduleTable;

var mailBtn;
var mailServerInput;
var mailSenderInput;
var mailPasswordInput;
var mailRecipientInput;

var pagesBtn;
var activePageEditing = '';
var newPageNameInput;

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


async function AddKey(key) {
    var settings = {
        "url": "../api/adminapi",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": {
            "AuthenticationID": authenticationKey,
            "RequestID": "addKey",
            "RequestData": key
        }
    };

    const response = await GetData(settings, true);
    ReloadKeys();
    authenticationKeyInput.value = '';
}

async function RemoveKey(key)
{
    var settings = {
        "url": "../api/adminapi",
        "method": "POST",
        "timeout": 0,
        "headers": {
          "Content-Type": "application/json"
        },
        "data": {
            "AuthenticationID": authenticationKey,
            "RequestID": "removeKey",
            "RequestData": key
        }
    };

    const response = await GetData(settings, true);
    if (response)
        ReloadKeys();
}

async function ReloadKeys()
{
    var settings = {
        "url": "../api/adminapi",
        "method": "POST",
        "timeout": 0,
        "headers": {
          "Content-Type": "application/json"
        },
        "data": {
            "AuthenticationID": authenticationKey,
            "RequestID": "getAllKeys",
            "RequestData": ""
        }
    };
    
    const response = await GetData(settings, true);
    var tableHTML = '<table class="table"><thead><th scope="col">Key</th><th scope="col">Last Active</th><th scope="col">Action</th></thead><tbody>';
    for (var i = 0; i < response.length; i++)
        tableHTML += '<tr><th scope="row">' + response[i].Item1 + '</th><td>' + (((new Date() - new Date(response[i].Item3)) < 7 * 1000) ? '🟢 Active' : '🔴 Inactive') + '</td><td><button class="btn btn-danger" type="button" onclick="RemoveKey(\'' + response[i].Item1 + '\');">Remove Key</button></td></tr>';

    tableHTML += '</tbody></table>';

    authenticationKeysTable.innerHTML = tableHTML;
}


async function ReloadModules()
{
    var settings = {
        "url": "../api/adminapi",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": {
            "authenticationID": authenticationKey,
            "requestID": "getAllModules",
            "requestData": ""
        }
    }

    const response = await GetData(settings, true);
    var tableHTML = '<table class="table"><thead><th scope="col">Module ID</th><th scope="col">Module Name</th><th scope="col">Actions</th></thead><tbody>';
    for (var i = 0; i < response.length; i++)
                tableHTML += '<tr><th scope="row">' + response[i].Item1 + '</th><td>' + response[i].Item2 + '</td><td>' + ((response[i].Item3) ? '<button class="btn btn-primary" type="button" onclick="UnloadModule(\'' + response[i].Item2 + '\');">Unload Module</button>' : '<button class="btn btn-success" type="button" onclick="LoadModule(\'' + response[i].Item2 + '\');">Load Module</button>') + '<button class="btn btn-danger" type="button" onclick="RemoveModule(\'' + response[i].Item2 + '\');">Remove Module</button></td></tr>';

    tableHTML += '</tbody></table>';

    moduleTable.innerHTML = tableHTML;
}

function UnloadModule(moduleName)
{
    var settings = {
        "url": "../api/adminapi",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": '{"authenticationID":"' + authenticationKey + '","requestID":"unloadModule","requestData":"' + moduleName + '"}',
    }

    $.ajax(settings)
        .done(function (response) {
            response = JSON.parse(response);
            console.log(response);
            if (response.response)
                window.location.reload();
        });
}

function LoadModule(moduleName)
{
    var settings = {
        "url": "../api/adminapi",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": '{"authenticationID":"' + authenticationKey + '","requestID":"loadModule","requestData":"' + moduleName + '"}',
    }

    $.ajax(settings)
        .done(function (response) {
            response = JSON.parse(response);
            console.log(response);
            if (response.response)
                window.location.reload();
        });
}

async function RemoveModule(moduleName)
{
    var settings = {
        "url": "../api/adminapi",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": {
            "authenticationID": authenticationKey,
            "requestID": "deleteModule",
            "requestData": moduleName
        }
    }

    const response = await GetData(settings, true);
    if (response)
        window.location.reload();
}

async function GetMailSettings() {
    var settings = {
        "url": "../api/adminapi",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": {
            "authenticationID": authenticationKey,
            "requestID": "getErrorMailSettings",
            "requestData": ""
        }
    }

    const response = await GetData(settings, true);
    if (response == '{}')
        return;

    mailServerInput.value = response['Server'];
    mailSenderInput.value = response['Sender'];
    mailPasswordInput.value = response['Password'];
    mailRecipientInput.value = response['Recipient'];
}

async function SetMailSettings() {
    var settings = {
        "url": "../api/adminapi",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": {
            "authenticationID": authenticationKey,
            "requestID": "setErrorMailSettings",
            "requestData": "{\"Server\":\"" + mailServerInput.value + "\",\"Sender\": \"" + mailSenderInput.value + "\", \"Password\": \"" + mailPasswordInput.value + "\", \"Recipient\": \"" + mailRecipientInput.value + "\"}"
        }
    }

    const response = await GetData(settings, true);

    if (response != true) {
        console.log("There was an error settings the error mail");
    }

    GetMailSettings();
}

async function LoadPageToEditor(pageName)
{
    activePageEditing = pageName;
    var settings = {
        "url": "../api/adminapi",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": {
            "authenticationID": authenticationKey,
            "requestID": "getPage",
            "requestData": pageName
        }
    }
    $('.my-editor').trumbowyg('html', await GetData(settings, true));
}

async function RemovePage(pageName)
{
    activePageEditing = pageName;
    var settings = {
        "url": "../api/adminapi",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": {
            "authenticationID": authenticationKey,
            "requestID": "removePage",
            "requestData": pageName
        }
    }

    var response = GetData(settings, true);
    if (response != true)
        console.log('Error saving page');
    else
        location.reload();
}

async function CreatePage(pageName)
{
    var settings = {
        "url": "../api/adminapi",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": {
            "authenticationID": authenticationKey,
            "requestID": "createPage",
            "requestData": pageName
        }
    }

    var response = await GetData(settings, true);
    if (response == true)
        location.reload();
}

async function SavePageFromEditor()
{
    if (activePageEditing === '')
        return;

    var settings = {
        "url": "../api/adminapi",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": {
            "authenticationID": authenticationKey,
            "requestID": "setPage",
            "requestData": JSON.stringify({
                "pageName": activePageEditing,
                "pageContent": $('.my-editor').trumbowyg('html')
            })
        }
    }

    var response = await GetData(settings, true);
    if (response != true)
        console.log('Error saving page');
}

$(function()
{
    authenticationKeyBtn = document.getElementById('authenticationKeyBtn');
    reloadAuthenticationKeysBtn = document.getElementById('reloadAuthenticationKeys');
    authenticationKeysTable = document.getElementById('authenticationKeysTable');
    authenticationKeyInput = document.getElementById('authenticationKeyInput');
    authenticationKeyInputBtn = document.getElementById('authenticationKeyInputBtn');

    moduleBtn = document.getElementById('moduleBtn');
    reloadModulesBtn = document.getElementById('reloadModules');
    moduleTable = document.getElementById('moduleTable');

    mailBtn = document.getElementById('mailBtn');
    mailServerInput = document.getElementById('mailServerInput');
    mailSenderInput = document.getElementById('mailSenderInput');
    mailPasswordInput = document.getElementById('mailPasswordInput');
    mailRecipientInput = document.getElementById('mailRecipientInput');

    pagesBtn = document.getElementById('pagesBtn');
    newPageNameInput = document.getElementById('newPageNameInput');
    $('.my-editor').trumbowyg({
        lang: 'en',
        fixedBtnPane: false,
        fixedFullWidth: false,
        autogrow: false,
        autogrowOnEnter: false,
        prefix: 'trumbowyg-',
        tagClasses: {},
        semantic: true,
        semanticKeepAttributes: false,
        resetCss: false,
        removeformatPasted: false,
        tabToIndent: false,
        tagsToRemove: [],
        tagsToKeep: ['hr', 'img', 'embed', 'iframe', 'input'],
        btns: [
            ['viewHTML'],
            ['undo', 'redo'],// Only supported in Blink browsers
            ['formatting'],
            ['strong', 'em', 'del'],
            ['superscript', 'subscript'],
            ['link'],
            ['insertImage'],
            ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
            ['unorderedList', 'orderedList'],
            ['horizontalRule'],
            ['removeformat'],
            ['fullscreen']
        ],
        btnsDef: {},
        changeActiveDropdownIcon: false,
        inlineElementsSelector: 'a,abbr,acronym,b,caption,cite,code,col,dfn,dir,dt,dd,em,font,hr,i,kbd,li,q,span,strikeout,strong,sub,sup,u',
        pasteHandlers: [],
        plugins: {},
        urlProtocol: false,
        minimalLinks: false,
        defaultLinkTarget: undefined
});


    $('#pages').children().css('height', '0');

    authenticationKeyBtn.onclick = function () { Show('Authentication'); }
    moduleBtn.onclick = function () { Show('Module'); }

    mailBtn.onclick = function () { Show('Mail'); }

    pagesBtn.onclick = function () { Show('Pages'); }

    reloadAuthenticationKeysBtn.onclick = ReloadKeys;
    authenticationKeyInputBtn.onclick = function () { AddKey(authenticationKeyInput.value); }

    reloadModulesBtn.onclick = ReloadModules;

    HideEverything();

    authenticationKeyInput.addEventListener('keyup', function (e) {
        if (e.keyCode === 13) {
            event.preventDefault();
            authenticationKeyInputBtn.click();
        }
    });

    ReloadKeys();
    ReloadModules();
    GetMailSettings();

    setInterval(ReloadKeys, 5000);
})