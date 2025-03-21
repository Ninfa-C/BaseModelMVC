document.addEventListener("DOMContentLoaded", () => {
    getAllPrenotazioni();
});

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
