// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

async function GetData(ajaxSettings = {}, returnOnlyData = false)
{
    if (ajaxSettings.hasOwnProperty('data') && typeof ajaxSettings.data !== 'string' && !(ajaxSettings.data instanceof String))
        ajaxSettings.data = JSON.stringify(ajaxSettings.data);
    
    var response = await $.ajax(ajaxSettings);
    
    try
    {
        if (typeof response === 'string' || response instanceof String)
            response = JSON.parse(response);
    }
    catch (e)
    {
        console.log(response);
        throw e;
    }
    
    if (response.hasOwnProperty('error') && response.error)
    {
        console.log('An error occured');
        if (response.hasOwnProperty('errorMessage'))
            console.log(response.errorMessage);
        throw DOMException();
    }
    
    try
    {
        if (response.hasOwnProperty('responseData') && (typeof response.responseData === 'string' || response.responseData instanceof String))
            response.responseData = JSON.parse(response.responseData);
    }
    catch { }
    
    if (returnOnlyData)
        response = response.responseData;

    return response;
}