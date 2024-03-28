document.addEventListener("DOMContentLoaded", function () {

    const patientList = document.getElementById("patientList");
    const patientForm = document.getElementById("patientForm");
    const patientIllnesses = document.getElementById("illnessesList");
    const patientVaccins = document.getElementById("vaccinsList");
    const illnessForm = document.getElementById("addillness");
    const vaccinForm = document.getElementById("addvaccin");
    const statisticsBtn = document.getElementById("statisticsBtn");

    //**************** Init page data *************************

    statisticsBtn.addEventListener('click', function () {
        window.open("statistics.html");
    });

    function fetchPatients() {

        const url = "http://localhost:5281/patient";

        fetch(url)
            .then(response => response.json())
            .then(data => {
                displayPatients(data);
            })
            .catch(error => console.error("Error fetching patients:", error));
    }

    function fetchManufacturers() {

        const url = "http://localhost:5281/vaccinsManufacturer";

        fetch(url)
            .then(response => response.json())
            .then(data => {
                document.manufacturers = data;
                populateManufacturers(data);
            })
            .catch(error => console.error("Error fetching patients:", error));
    }

    //display list of all patients in HMO
    function displayPatients(patients) {

        patientList.innerHTML = "";
        patients.forEach(patient => {


            const listItem = document.createElement("div");
            listItem.innerHTML = `
                <img src="data:image/png;base64,${patient.image}" style="max-width:${patient.image ? 180 : 0}px;max-height:{${patient.image} ? 140 : 0}px;float:right;"></img>

                <strong style="float:clear">Id:</strong> ${patient.id}<br>
                <strong>First Name:</strong> ${patient.firstName}<br>
                <strong>Last Name:</strong> ${patient.lastName}<br>
                <strong>Address:</strong> ${patient.address}<br>
                <strong>BirthDate:</strong> ${new Date(patient.birthDate).toLocaleDateString("en-GB")}<br>
                <strong>Phone:</strong> ${patient.phone}<br>
                <strong>CellPhone:</strong> ${patient.cellPhone}<br>

               
                <button class="editBtn" data-id="${patient.id}">Details</button>
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

    //fill the selection list with manufacturers
    function populateManufacturers(manufactureres) {

        const selectManufacturer = vaccinForm.getElementsByClassName("manufacturer")[0];

        manufactureres.forEach(m => {
            var option = document.createElement("option");
            option.text = m.name;
            option.value = m.id;
            selectManufacturer.add(option);
        })
    }

    //**********************************************************



    //****************** Patient CRUD **************************
    
    //function to show the patient details
    function editPatient(patient) {
        var idInput = patientForm.getElementsByClassName("id")[0];
        idInput.disabled = true;
        fillPatientIllnessesAndVaccinations(patient.id);

        const birthdateAsDate = new Date(patient.birthDate);
        var year = birthdateAsDate.toLocaleString("default", { year: "numeric" });
        var month = birthdateAsDate.toLocaleString("default", { month: "2-digit" });
        var day = birthdateAsDate.toLocaleString("default", { day: "2-digit" });
        // Generate yyyy-mm-dd date string
        var formattedDate = year + "-" + month + "-" + day;

        idInput.value = patient.id;
        patientForm.getElementsByClassName("fname")[0].value = patient.firstName;
        patientForm.getElementsByClassName("lname")[0].value = patient.lastName;
        patientForm.getElementsByClassName("address")[0].value = patient.address;
        patientForm.getElementsByClassName("birth")[0].value = formattedDate;
        patientForm.getElementsByClassName("phone")[0].value = patient.phone;
        patientForm.getElementsByClassName("cellPhone")[0].value = patient.cellPhone;
    }

    //function to delete a patient
    function deletePatient(patientId) {

        const url = `http://localhost:5281/patient/${patientId}`;

        fetch(url, { method: 'DELETE' })
            .then(data => {
                fetchPatients();
            })
            .catch(error => console.error("Error fetching patients:", error));
    }

    //save the patient details (create/update)
    patientForm.addEventListener("submit", function (event) {
        event.preventDefault();

        const patientData = {
            id: patientForm.querySelector(".id").value,
            firstName: patientForm.querySelector(".fname").value,
            lastName: patientForm.querySelector(".lname").value,
            address: patientForm.querySelector(".address").value,
            birthDate: patientForm.querySelector(".birth").value,
            phone: patientForm.querySelector(".phone").value,
            cellPhone: patientForm.querySelector(".cellPhone").value,
            image: document.getElementById("image").files[0]
        };

        const patientId = patientForm.querySelector(".id").value;
        const idInput = patientForm.getElementsByClassName("id")[0];

        const formData = new FormData();
        formData.append("id", patientData.id);
        formData.append("firstName", patientData.firstName);
        formData.append("lastName", patientData.lastName);
        formData.append("address", patientData.address);
        formData.append("birthDate", patientData.birthDate);
        formData.append("phone", patientData.phone);
        formData.append("cellPhone", patientData.cellPhone);
        formData.append("image", patientData.image);

        if (idInput.disabled) {
            const url = `http://localhost:5281/patient/${patientId}`;

            fetch(url, {
                method: "PUT",
                body: formData
            })
                .then(data => {
                    if (data.status == 400) {
                        console.error("Error saving patient");
                        return;
                    }
                    fetchPatients(); // Refresh patient list after submitting the form
                    patientForm.reset(); // Clear form fields after submission
                    patientVaccins.reset();
                    patientIllnesses.reset();

                    idInput.disabled = false;
                })
                .catch(error => {
                    console.error("Error saving patient:", JSON.stringify(error))
                });
        }
        else {
            const url = "http://localhost:5281/patient";

            fetch(url, {
                method: "POST",
                body: formData
            })
                .then(data => {
                    if (data.status == 400) {
                        console.error("Error saving patient");
                        return;
                    }
                    fetchPatients(); // Refresh patient list after submitting the form
                    patientForm.reset(); // Clear form fields after submission
                    patientVaccins.reset();
                    patientIllnesses.reset();
                })
                .catch(error => {
                    console.error("Error saving patient:", JSON.stringify(error))
                });
        }
    });

    //**********************************************************

    //******************** Ilness and Vaccin CRUD **************

    //save the new illness to the current customer
    illnessForm.addEventListener("submit", function (event) {
        event.preventDefault();
        const patientId = patientForm.querySelector(".id").value;
        const illnessData = {
            positiveDate: addillness.querySelector(".sDate").value,
            negativeDate: addillness.querySelector(".fDate").value,
            patientId: patientId
        };

        const url = "http://localhost:5281/illness";

        fetch(url, {
            method: "POST", // Adjust method as needed for creating or updating patients
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(illnessData)
        })
            .then(data => {
                illnessForm.reset(); // Clear form fields after submission
                fillPatientIllnessesAndVaccinations(patientId);
            })
            .catch(error => console.error("Error saving illness:", error));
    })

    //save the new vaccin to the current customer
    vaccinForm.addEventListener("submit", function (event) {
        event.preventDefault();
        const patientId = patientForm.querySelector(".id").value;
        const vaccinData = {
            date: vaccinForm.querySelector(".vaccinDate").value,
            vaccinsManufacturerID: vaccinForm.querySelector(".manufacturer").selectedOptions[0].value,
            patientId: patientId
        };

        const url = "http://localhost:5281/vaccin";

        fetch(url, {
            method: "POST", // Adjust method as needed for creating or updating patients
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(vaccinData)
        })
            .then(data => {
                vaccinForm.reset(); // Clear form fields after submission
                fillPatientIllnessesAndVaccinations(patientId);
            })
            .catch(error => console.error("Error saving vaccin:", error));
    })

    //***********************************************************

    //======================= bind edited patient vaccins and illnesses ==================================

    //show the vaccins of the current customer
    function displayVaccins(vaccins) {

        patientVaccins.innerHTML = "";

        vaccins.forEach(vaccin => {
            const listItem = document.createElement("div");
            listItem.innerHTML = `
            <div>
                <strong>Vaccin Date:</strong> ${new Date(vaccin.date).toLocaleDateString("en-GB")} <br>
                <strong>Vaccin Manufacturer:</strong> ${document.manufacturers.find(m => m.id == vaccin.vaccinsManufacturerID).name}<br>

                <hr>
            </div> `;
            patientVaccins.appendChild(listItem);
        });
    }

     //show the illnesses of the current customer
    function displayIllnesses(illnesses) {
        patientIllnesses.innerHTML = "";

        illnesses.forEach(illness => {
            const listItem = document.createElement("div");
            listItem.innerHTML = `
            <div>
                <strong>Positive Date:</strong> ${new Date(illness.positiveDate).toLocaleDateString("en-GB")}<br>
                <strong>Negative Date:</strong> ${new Date(illness.negativeDate).toLocaleDateString("en-GB")}<br>

                <hr>
            </div>
            `;
            patientIllnesses.appendChild(listItem);
        });
    }

     //show the vaccins&illnesses of the current customer
    function fillPatientIllnessesAndVaccinations(patientId) {

        const vaccinsUrl = `http://localhost:5281/vaccin/patient/${patientId}`;
        fetch(vaccinsUrl)
            .then(response => response.json())
            .then(data => {
                displayVaccins(data);
            })
            .catch(error => console.error("Error fetching vaccins:", error));

        const illnessesUrl = `http://localhost:5281/illness/patient/${patientId}`;
        fetch(illnessesUrl)
            .then(response => response.json())
            .then(data => {
                displayIllnesses(data);
            })
            .catch(error => console.error("Error fetching illnesses:", error));
    }

    //===========================================================================================

    fetchManufacturers()
    fetchPatients();
});

