const ChartTypes = {
	Line: 'line',
	Pie: 'pie',
	Bar: 'bar'
};

class CustomChart {
	#_element = null;
	#_type = null;
	#_options = {
		maintainAspectRatio: true
	};
	#_labels = [];
	#_datasets = [];
	#_chart = null;

	/**
	 * Creates Chart and adds first dataset to it. First Dataset has the Index of 0.
	 * @param {Element} element
	 * @param {ChartTypes} type
	 * @param {string} label First Dataset Label
	 * @param {number} r Red value of first dataset
	 * @param {number} g Green value of first dataset
	 * @param {number} b Blue value of first dataset
	 */
	constructor(element, type, label, r, g, b) {
		this.#_element = element;
		this.#_type = type;

		this.#_datasets.push({
			label: label,
			backgroundColor: 'rgba(' + r + ', ' + g + ' , ' + b + ', 0.1)',
			borderColor: 'rgb(' + r + ', ' + g + ' , ' + b + ')',
			data: []
		});

		this.#_chart = new Chart(element, {
			type: type,
			data: {
				labels: this.#_labels,
				datasets: this.#_datasets
			},
			options: this.#_options
		});

		this.Update();
	}

	/**
	 * Add value to dataset
	 * @param {any} datasetIndex Index of dataset. Usually returned from AddDataset
	 * @param {any} label Label/X-Axis Label
	 * @param {any} value Y-Axis Value
	 */
	AddValue(datasetIndex, label, value) {
		if (this.#_labels.indexOf(label) == -1)
			this.#_labels.push(label);
		const labelIndex = this.#_labels.indexOf(label);
		const datasetSize = this.#_datasets[datasetIndex].data.length;
		for (var i = datasetSize; i < labelIndex; i++)
			this.#_datasets[datasetIndex].data.push(null);

		this.#_datasets[datasetIndex].data[labelIndex] = value;
		this.Update();
	}

	/**
	 * Adds a Dataset to a chart
	 * @param {string} label Dataset Label
	 * @param {number} r Red value of first dataset
	 * @param {number} g Green value of first dataset
	 * @param {number} b Blue value of first dataset
	 * @returns {number} Returns Index of Dataset
	 */
	AddDataset(label, r, g, b) {
		this.#_datasets.push({
			label: label,
			backgroundColor: 'rgba(' + r +', ' + g + ' , ' + b + ', 0.1)',
			borderColor: 'rgb(' + r + ', ' + g + ' , ' + b + ')',
			data: []
		});

		return this.#_datasets.length - 1;
	}

	Update() {
		this.#_chart.update();
    }
}