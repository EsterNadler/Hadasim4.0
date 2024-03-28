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
                    backgroundColor: "rgba(255,255,255,1.0)",
                    borderColor: "rgba(0,0,255,0.1)",
                    data: yValues
                }]
            },
        });
    }

    function vaccinatedORnot() {
        fetch('/Vaccin/vaccination-status') // בקשת GET לנתיב המתאים לאנדפוינט שלך
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json(); // קריאה ל־JSON על התוצאה אם הבקשה הצליחה
            })
            .then(data => {
                pieChart(data); // פרטי התוצאה מוצגים בקונסול
                // כאן תוכל להשתמש בנתונים באופן נוסף, לדוגמה, לעדכן איזה הודעה ב-HTML עם המידע
            })
            .catch(error => {
                console.error('There was a problem with the fetch operation:', error); // אם הייתה בעיה בבקשה
            });
    }

    function pieChart(data) {
        var ctx = document.getElementById('vaccins').getContext('2d');
        var vaccins = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: ['Vaccinated', 'Not Vaccinated'],
                datasets: [{
                    label: 'Vaccinated/Not',
                    data: [data[0], data[1]],
                    backgroundColor: [
                        'rgb(255, 99, 132)',
                        'rgb(54, 162, 235)',
                    ],
                    hoverOffset: 4
                }]
            }
        });
    }


    vaccinatedORnot();
    fetchPatients();
});

