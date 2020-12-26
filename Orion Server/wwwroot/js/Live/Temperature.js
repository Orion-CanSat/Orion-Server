var temperatureChart;
var temperatureDatasetIndex = -1;
const temperatureGetURL = '/api/temperature';

function LoadTemperatureChart() {
    var chartName = "temperatureChart";
    var ctx = document.getElementById(chartName).getContext('2d');

    temperatureChart = new CustomChart(ctx, ChartTypes.Line, 'Temperature', 255, 0, 0);
    temperatureDatasetIndex = 0;
}

function UpdateTemperatureChart() {
    if (temperatureDatasetIndex == -1)
        return;
    fetch(temperatureGetURL)
        .then(function (response) {
            return response.json();
        })
        .then(function (responseJSON) {
            if (!responseJSON.response)
                return;
            temperatureChart.AddValue(0, responseJSON.responseData.Date.split('T').shift().split('-').reverse().join('/') + ' ' + responseJSON.responseData.Date.split('T')[1], responseJSON.responseData.TemperatureC);
        });
}

$(function () {
    LoadTemperatureChart();
    setInterval(UpdateTemperatureChart, 1000);
});