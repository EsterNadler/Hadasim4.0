document.addEventListener("DOMContentLoaded", function () {
    const patientList = document.getElementById("patientList");
    const patientForm = document.getElementById("patientForm");
    const patientIllnesses = document.getElementById("illnessesList");
    const patientVaccins = document.getElementById("vaccinsList");

    // Function to fetch patients from the server
    function fetchPatients() {
        // Replace this URL with your actual server endpoint
        const url = "http://localhost:5281/patient";

        fetch(url)
            .then(response => response.json())
            .then(data => {
                displayPatients(data);
            })
            .catch(error => console.error("Error fetching patients:", error));
    }

    // Function to display patients in the list
    function displayPatients(patients) {

        patientList.innerHTML = "";
        patients.forEach(patient => {
            const listItem = document.createElement("div");
            //<strong>ID:</strong> ${patient.id}<br>
            listItem.innerHTML = `
                <strong>First Name:</strong> ${patient.firstName}<br>
                <strong>Last Name:</strong> ${patient.lastName}<br>
                <strong>Address:</strong> ${patient.address}<br>
                <strong>BirthDate:</strong> ${patient.birthDate}<br>
                <strong>Phone:</strong> ${patient.phone}<br>
                <strong>CellPhone:</strong> ${patient.cellPhone}<br>

                <button class="editBtn" data-id="${patient.id}">Edit</button>
                <button class="deleteBtn" data-id="${patient.id}">Delete</button>
                <hr>
            `;
            const editBtn = listItem.getElementsByClassName("editBtn")[0];
            editBtn.addEventListener("click", function () {
                editPatient(patient);
            });

            const deleteBtn = listItem.getElementsByClassName("deleteBtn")[0];
            deleteBtn.addEventListener("click", function () {
                deletePatient(patient.id);
            });

            patientList.appendChild(listItem);
        });
    }

    function editPatient(patient) {

        fillPatientIllnessesAndVaccinations(patient);

        patientForm.getElementsByClassName("id")[0].value = patient.id;
        patientForm.getElementsByClassName("fname")[0].value = patient.firstName;
        patientForm.getElementsByClassName("lname")[0].value = patient.lastName;
        patientForm.getElementsByClassName("address")[0].value = patient.address;
        patientForm.getElementsByClassName("birth")[0].value = patient.birthDate;
        patientForm.getElementsByClassName("phone")[0].value = patient.phone;
        patientForm.getElementsByClassName("cellPhone")[0].value = patient.cellPhone;
    }

    function deletePatient(patientId) {

        const url = `http://localhost:5281/patient/${patientId}`;

        fetch(url, { method: 'DELETE' })
            .then(data => {
                fetchPatients();
            })
            .catch(error => console.error("Error fetching patients:", error));
    }

    patientForm.addEventListener("submit", function (event) {
        event.preventDefault();
        const patientId = patientForm.querySelector(".id").value;
        //const formData = new FormData(patientForm);
        const patientData = {
            id: patientForm.querySelector(".id").value,
            firstName: patientForm.querySelector(".fname").value,
            lastName: patientForm.querySelector(".lname").value,
            address: patientForm.querySelector(".address").value,
            birthDate: patientForm.querySelector(".birth").value,
            phone: patientForm.querySelector(".phone").value,
            cellPhone: patientForm.querySelector(".cellPhone").value
        };
        if (patientId) {
            // Replace this URL with your actual server endpoint for creating/editing patients
            const url = `http://localhost:5281/patient/${patientId}`;

            fetch(url, {
                method: "PUT", // Adjust method as needed for creating or updating patients
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(patientData)
            })
                .then(data => {
                    fetchPatients(); // Refresh patient list after submitting the form
                    patientForm.reset(); // Clear form fields after submission
                    //patientForm.getElementsByClassName("id")[0].value = "";
                })
                .catch(error => console.error("Error saving patient:", error));
        }
        else {
            const url = "http://localhost:5281/patient";

            fetch(url, {
                method: "POST", // Adjust method as needed for creating or updating patients
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(patientData)
            })
            .then(data => {
                fetchPatients(); // Refresh patient list after submitting the form
                patientForm.reset(); // Clear form fields after submission
                //patientForm.getElementsByClassName("id")[0].value = "";
            })
            .catch(error => console.error("Error saving patient:", error));
        }
    });

    function fillPatientIllnessesAndVaccinations(patient) {

        const vaccinsUrl = "http://localhost:5281/vaccin";
        fetch(vaccinsUrl)
            .then(response => response.json())
            .then(data => {
                displayVaccins(data);
            })
            .catch(error => console.error("Error fetching vaccins:", error));


        const illnessesUrl = "http://localhost:5281/illness";
        fetch(illnessesUrl)
            .then(response => response.json())
            .then(data => {
                displayIllnesses(data);
            })
            .catch(error => console.error("Error fetching illnesses:", error));
    }

    function displayVaccins(vaccins) {

        patientVaccins.innerHTML = "";

        vaccins.forEach(vaccin => {
            const listItem = document.createElement("div");
            //<strong>ID:</strong> ${patient.id}<br>
            listItem.innerHTML = `
            <div>
                <strong>Positive Date:</strong> ${vaccin.date}<br>
                <strong>Negative Date:</strong> ${vaccin.vaccinManufaturerId}<br>

                <button class="editBtn" data-id="${vaccin.id}">Edit</button>
                <hr>
            </div>
            `;
            //const editBtn = listItem.getElementsByClassName("editBtn")[0];
            //editBtn.addEventListener("click", function () {
            //    //editPatient(patient);
            //});

            patientVaccins.appendChild(listItem);
        });
    }

    function displayIllnesses(illnesses) {
        patientIllnesses.innerHTML = "";

        illnesses.forEach(illness => {
            const listItem = document.createElement("div");
            //<strong>ID:</strong> ${patient.id}<br>
            listItem.innerHTML = `
            <div>
                <strong>Positive Date:</strong> ${illness.positiveDate}<br>
                <strong>Negative Date:</strong> ${illness.negativeDate}<br>

                <button class="editBtn" data-id="${illness.id}">Edit</button>
                <hr>
            </div>
            `;
            const editBtn = listItem.getElementsByClassName("editBtn")[0];
            editBtn.addEventListener("click", function () {
                //editPatient(patient);
            });

            patientIllnesses.appendChild(listItem);
        });
    }



    fetchPatients();
});
