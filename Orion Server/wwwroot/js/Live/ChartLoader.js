Object.defineProperty(String.prototype, 'hashCode', {
    value: function () {
        var hash = 0, i, chr;
        for (i = 0; i < this.length; i++) {
            chr = this.charCodeAt(i);
            hash = ((hash << 5) - hash) + chr;
            hash |= 0;
        }
        return hash;
    }
});

var charts = [];
var chartNameToJSONResponseValue = {
    'temperature': 'TemperatureC',
    'pressure': 'PressurePa'
};

/**
 * 
 * @param {string} chartName
 * @returns {CustomChart}
 */
function LoadChart(chartName) {
    var ctx = document.getElementById(chartName + '-Chart').getContext('2d');

    var hashValue = chartName.hashCode();
    var newChart = new CustomChart(
        ctx,
        ChartTypes.Line,
        chartName.charAt(0).toUpperCase() + chartName.slice(1),
        hashValue % 255,
        (hashValue >> 8) % 255,
        (hashValue >> 16) % 255);
    return newChart;
}

function UpdateChart(chart) {
    try {
        fetch('/api/' + chart.name)
            .then(function (respone) {
                return respone.json();
            })
            .then(function (responseJSON) {
                if (!responseJSON.response)
                    return;
                console.log(responseJSON);
                console.log(chartNameToJSONResponseValue[chart.name]);
                chart.chart.AddValue(0,
                    responseJSON.responseData.Date.split('T').shift().split('-').reverse().join('/') + ' ' + responseJSON.responseData.Date.split('T')[1],
                    responseJSON.responseData[chartNameToJSONResponseValue[chart.name]]);
            });
    }
    catch { }
}

$(function () {
    var chartElements = $('.chart-container').children();
    for (var i = 0; i < chartElements.length; i++) {
        var chartElement = chartElements[i];
        try {
            var chartElementId = chartElement.id;
            var chartElementName = chartElementId.split('-')[0];
            console.log(chartElementName);
            var chart = LoadChart(chartElementName);
            charts.push({
                name: chartElementName,
                chart: chart
            });
        }
        catch { }
    }

    setInterval(function () {
        for (var i = 0; i < charts.length; i++) {
            UpdateChart(charts[i]);
        }
    }, 1000);
})