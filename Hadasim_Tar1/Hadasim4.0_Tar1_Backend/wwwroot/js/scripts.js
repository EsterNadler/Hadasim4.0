document.addEventListener("DOMContentLoaded", function () {
    const patientList = document.getElementById("patientList");
    const patientForm = document.getElementById("patientForm");
    const patientIllnesses = document.getElementById("illnessesList");
    const patientVaccins = document.getElementById("vaccinsList");
    const illnessForm = document.getElementById("addillness");
    const vaccinForm = document.getElementById("addvaccin");

    //**************** Init page data *************************

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

    function displayPatients(patients) {

        patientList.innerHTML = "";
        patients.forEach(patient => {
            const listItem = document.createElement("div");
            //<strong>ID:</strong> ${patient.id}<br>
            listItem.innerHTML = `
                <strong>Id:</strong> ${patient.id}<br>
                <strong>First Name:</strong> ${patient.firstName}<br>
                <strong>Last Name:</strong> ${patient.lastName}<br>
                <strong>Address:</strong> ${patient.address}<br>
                <strong>BirthDate:</strong> ${new Date(patient.birthDate).toLocaleDateString("en-GB") }<br>
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

        const patientData = {
            id: patientForm.querySelector(".id").value,
            firstName: patientForm.querySelector(".fname").value,
            lastName: patientForm.querySelector(".lname").value,
            address: patientForm.querySelector(".address").value,
            birthDate: patientForm.querySelector(".birth").value,
            phone: patientForm.querySelector(".phone").value,
            cellPhone: patientForm.querySelector(".cellPhone").value
        };

        const idInput = patientForm.getElementsByClassName("id")[0];

        if (idInput.disabled) {
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
                idInput.disabled = false;
            })
            .catch(error => console.error("Error saving patient:", error));
        }
        else {
            const url = "http://localhost:5281/patient";

            fetch(url, {
                method: "POST", 
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(patientData)
            })
            .then(data => {
                fetchPatients(); // Refresh patient list after submitting the form
                patientForm.reset(); // Clear form fields after submission
            })
            .catch(error => console.error("Error saving patient:", error));
        }
    });

    //**********************************************************

    //******************** Ilness and Vaccin CRUD **************

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

    function displayVaccins(vaccins) {

        patientVaccins.innerHTML = "";

        vaccins.forEach(vaccin => {
            const listItem = document.createElement("div");
            //<strong>ID:</strong> ${patient.id}<br>
            listItem.innerHTML = `
            <div>
                <strong>Vaccin Date:</strong> ${new Date(vaccin.date).toLocaleDateString("en-GB") } <br>
                <strong>Vaccin Manufacturer:</strong> ${document.manufacturers.find(m=>m.id==vaccin.vaccinsManufacturerID).name}<br>

                <hr>
            </div> `;
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
                <strong>Positive Date:</strong> ${new Date(illness.positiveDate).toLocaleDateString("en-GB") }<br>
                <strong>Negative Date:</strong> ${new Date(illness.negativeDate).toLocaleDateString("en-GB") }<br>

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
    var imageContainer = document.getElementById("imageContainer");
    var changeImageButton = document.getElementById("changeImageButton");

    function changeImage() {
        var imageUrl = "data:image/jpeg;base64,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"; // URL של התמונה החדשה
        var img = document.createElement("img"); // יצירת תגית img
        img.src = imageUrl; // הגדרת ה-src של התמונה
        img.alt = "New Image"; // הגדרת ה-attribue alt לתמונה (אופציונלי)

        // ניקוי התוכן של ה-container והוספת התמונה החדשה
        imageContainer.innerHTML = '';
        imageContainer.appendChild(img);
    }

    var imageContainer = document.getElementById("imageContainer");
    var changeImageButton = document.getElementById("changeImageButton");

    //function handleImageUpload(event) {
    //    event.preventDefault(); // מניעת התנהגות ברירת המחדל של הטופס

    //    var fileInput = document.getElementById("imageUpload");
    //    var file = fileInput.files[0]; // קובץ התמונה שנבחר על ידי המשתמש

    //    // וידוא שנבחר קובץ תמונה
    //    if (file.type.startsWith('image/')) {
    //        var reader = new FileReader();
    //        reader.onload = function () {
    //            // יצירת תמונה והצגתה בעמוד
    //            var img = document.createElement("img");
    //            img.src = reader.result;
    //            img.alt = "Uploaded Image";
    //            imageContainer.innerHTML = '';
    //            imageContainer.appendChild(img);
    //        };
    //        reader.readAsDataURL(file);
    //    } else {
    //        alert("Please select an image file.");
    //    }
    //}

    // הוספת event listener לטופס
    //changeImageButton.addEventListener("submit", handleImageUpload);

    fetchManufacturers()
    fetchPatients();
 });

