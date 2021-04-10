var statusText = document.getElementById('status');

async function UpdateStatusText() {
    var settings = {
        "url": "../../api/status",
        "method": "GET",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        }
    }

    var response = await GetData(settings, false);
    var status = -1;
    if ((response.response) && ((-1 <= response.responseData.Stat) && (response.responseData.Stat <= 1)))
        status = response.responseData.Stat;

    if (status == -1)
        statusText.innerHTML = 'Unknown <span style="height: 15px; width: 15px; background-color: #BBB; border-radius: 50%; display: inline-block"></span>';
    else if (status == 0)
        statusText.innerHTML = 'Offline <span style="height: 15px; width: 15px; background-color: #F00; border-radius: 50%; display: inline-block"></span>';
    else if (status == 1)
        statusText.innerHTML = 'Online <span style="height: 15px; width: 15px; background-color: #0F0; border-radius: 50%; display: inline-block"></span>';
}

$(function () {
    setInterval(UpdateStatusText, 2000);
});