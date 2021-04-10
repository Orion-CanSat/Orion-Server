Object.defineProperty(
    String.prototype,
    'hashCode',
    {
        value: function ()
            {
                var hash = 0, i, chr;
                for (i = 0; i < this.length; i++) {
                    chr = this.charCodeAt(i);
                    hash = ((hash << 5) - hash) + chr;
                    hash |= 0;
                }
                return hash;
            }
    }
);

const chartContainer = document.getElementById('chart-container');
var loadedCharts = [];

async function UpdateChart(chart)
{
    try
    {
        fetch('/api/' + chart.name)
            .then(async function(response)
            {
                return response.json();
            })
            .then(function (responseJSON)
            {
                if (!responseJSON.response)
                    return;

                chart.chart.AddValue(
                    chart.chartIndex,
                    responseJSON.responseData.Date.split('T').shift().split('-').reverse().join('/') + ' ' + responseJSON.responseData.Date.split('T')[1],
                    responseJSON.responseData[chart.name.charAt(0).toUpperCase() + chart.name.slice(1) + chart.chartUnit]
                );
            }
            );
    }
    catch
    {

    }
}

$(function ()
{
    for (var k = 0; k < charts.length; k++)
    {
        var row = document.createElement('div');
        row.classList = 'row';

        for (var i = 0; i < charts[k].length; i++) {
            var col = document.createElement('div');
            col.classList = 'col';

            var canva = document.createElement('canvas');
            canva.style = 'width: 50%; padding-bottom: 5vh; padding-top: 5vh';

            var hashValue = charts[k][i][0].Item1.hashCode();

            var cc = new CustomChart(
                canva,
                ChartTypes.Line,
                charts[k][i][0].Item1.charAt(0).toUpperCase() + charts[k][i][0].Item1.slice(1),
                hashValue % 255,
                (hashValue >> 8) % 255,
                (hashValue >> 16) % 255
            );

            loadedCharts.push(
                {
                    name: charts[k][i][0].Item1,
                    chart: cc,
                    chartIndex: 0,
                    chartUnit: charts[k][i][0].Item2
                }
            );

            for (var j = 1; j < charts[k][i].length; j++) {
                hashValue = charts[k][i][j].Item1.hashCode();

                cc.AddDataset(
                    charts[k][i][j].Item1.charAt(0).toUpperCase() + charts[k][i][j].Item1.slice(1),
                    hashValue % 255,
                    (hashValue >> 8) % 255,
                    (hashValue >> 16) % 255
                );

                loadedCharts.push(
                    {
                        name: charts[k][i][j].Item1,
                        chart: cc,
                        chartIndex: j,
                        chartUnit: charts[k][i][j].Item2
                    }
                );
            }

            cc.Update();

            col.appendChild(canva);

            row.appendChild(col);
        }

        chartContainer.appendChild(row);
    }


    setInterval(function ()
        {
            for (var i = 0; i < loadedCharts.length; i++) {
                try {
                    UpdateChart(loadedCharts[i]);
                }
                catch { }
            }
    },
        1000
    );
});