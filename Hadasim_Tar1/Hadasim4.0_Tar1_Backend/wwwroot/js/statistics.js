document.addEventListener("DOMContentLoaded", function () {

    function fetchPatients() {

        const url = "http://localhost:5281/illness/statistics";

        fetch(url)
            .then(response => response.json())
            .then(data => {
                displayIllnessStats(data);
            })
            .catch(error => console.error("Error fetching patients:", error));    
    }    

    function displayIllnessStats(data) {
        const xValues = Object.keys(data);
        const yValues = Object.values(data);

        new Chart("illness", {
            type: "line",
            data: {
                labels: xValues,
                datasets: [{
                    backgroundColor: "rgba(0,0,255,1.0)",
                    borderColor: "rgba(0,0,255,0.1)",
                    data: yValues
                }]
            },
            //options: { ...}
        });
    }

    fetchPatients();
});

