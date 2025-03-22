document.addEventListener("DOMContentLoaded", () => {
    getAllPrenotazioni();
});


//PRENOTAZIONI
async function getAllPrenotazioni() {
    let table = document.getElementById("prenotazioniContainer")
    if (!table) return;
    try {
        let response = await fetch("/Prenotazioni/Archivio");
        if (!response.ok) {
            throw new Error
        }
        let data = await response.text();
        table.innerHTML = data;
        new DataTable("#prenotazioniTable")
    } catch (error) {
        console.error("Errore nel recupero della lista prenotazioni:", error);
    }
}

async function getAdd() {
    let table = document.getElementById("prenotazioniContainer")
    if (!table) return;
    try {
        let response = await fetch("/Prenotazioni/AggiungiPrenotazione");
        if (!response.ok) {
            throw new Error
        }
        let data = await response.text();
        table.innerHTML = data;
    } catch (error) {
        console.error("Errore nel recupero del form di aggiunta prenotazione:", error);
    }
}


async function SaveAdd() {
    const form = document.getElementById("addPrenForm")
    console.log("eccomi")
    if (form) {
        const formData = new FormData(form);
        console.log(formData)
        const result = await fetch('AddSave', {
            method: "POST",
            body: formData
        });

        let response = await result.json();
        if (response.success) {
            console.log("yeah")
            getAllPrenotazioni();
        }
    } else {
        console.log("andata male")
    }
}

async function getEdit(id) {
    let table = document.getElementById("prenotazioniContainer")
    if (!table) return;
    try {
        let response = await fetch(`/Prenotazioni/EditPrenotazione/${id}`);
        if (!response.ok) {
            throw new Error
        }
        let data = await response.text();
        table.innerHTML = data;
    } catch (error) {
        console.error("Errore nel recupero del form di aggiunta prenotazione:", error);
    }
}

async function SaveEdit() {
    const form = document.getElementById("editPrenForm")
    if (form) {
        const formData = new FormData(form);
        console.log(formData)
        const result = await fetch('EditSave', {
            method: "POST",
            body: formData
        });

        let response = await result.json();
        if (response.success) {
            console.log("yeah")
            getAllPrenotazioni();
        }
    } else {
        console.log("andata male")
    }
}

//CAMERA

async function getRoomAdd() {
    let table = document.getElementById("camereContainer")
    if (!table) return;
    try {
        let response = await fetch("/Camere/AggiungiCamera");
        if (!response.ok) {
            throw new Error
        }
        let data = await response.text();
        table.innerHTML = data;
    } catch (error) {
        console.error("Errore nel recupero del form di aggiunta camera:", error);
    }
}


async function SaveRoomAdd() {
    const form = document.getElementById("addRoomForm")
    console.log("eccomi")
    if (form) {
        const formData = new FormData(form);
        console.log(formData)
        const result = await fetch('AddSave', {
            method: "POST",
            body: formData
        });

        let response = await result.json();
        if (response.success) {
            console.log("yeah")
        }
    } else {
        console.log("andata male")
    }
}


async function getRoomEdit(id) {
    let table = document.getElementById("camereContainer")
    if (!table) return;
    try {
        let response = await fetch(`EditCamere/${id}`);
        if (!response.ok) {
            throw new Error
        }
        let data = await response.text();
        table.innerHTML = data;
    } catch (error) {
        console.error("Errore nel recupero del form di aggiunta prenotazione:", error);
    }
}


async function SaveEditRoom() {
    const form = document.getElementById("editRoomForm")
    if (form) {
        const formData = new FormData(form);
        console.log("eccomi")
        const result = await fetch(`SaveEdit`, {
            method: "POST",
            body: formData
        });

        let response = await result.json();
        if (response.success) {
            console.log("yeah")
        }
    } else {
        console.log("andata male")
    }
}

//CLIENTE