var map = L.map('map').setView([37.9314, 23.7133], 15);
var marker = L.marker([37.9314, 23.7133]);

marker.addTo(map);

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
    maxZoom: 19
}).addTo(map);

async function UpdateMarker() {
    var settings = {
        "url": "../../api/location",
        "method": "GET",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        }
    }

    var response = await GetData(settings, false);
    if (!response.response)
        return;
    marker.setLatLng([response.responseData.Latitude, response.responseData.Longitude]);
}

$(function () {
    setInterval(UpdateMarker, 2000);
});