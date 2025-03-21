using HotelManagment.Services;
using HotelManagment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagment.Controllers
{
    [Authorize]
    public class PrenotazioniController : Controller
    {
        private readonly PrenotazioniServices _prenotazioniServices;
        public PrenotazioniController(PrenotazioniServices p)
        {
            _prenotazioniServices = p;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("Prenotazioni/Archivio")]
        public async Task<IActionResult> GetPrenotazioni()
        {
            var result = await _prenotazioniServices.GetAllPrenotazioni();
            return PartialView("_PrenotazioniList", result);
        }

        [HttpGet("Prenotazioni/AggiungiPrenotazione")]
        public async Task<IActionResult> Add()
        {
            ViewBag.Rooms = await _prenotazioniServices.GetAllRooms();
            return PartialView("_AddPrenotazioni");
        }

        [HttpPost]
        public async Task<IActionResult> AddSave(PrenotazioneAddModel model)

        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Error while saving entity to database"
                });
            }

            var result = await _prenotazioniServices.AddPrenotazione(model, User);

            if (!result)
            {
                return Json(new
                {
                    success = false,
                    message = "Error while saving entity to database"
                });
            }

            string logmessage = "Entity saved successfully to database";

            return Json(new
            {
                success = true,
                message = logmessage
            });
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _prenotazioniServices.DeletePrenotazione(id);
            if (!result)
            {
                TempData["Error"] = "Errore nell'eliminazione del libro dal database";
            }
            return RedirectToAction("Index");
        }


    }
}
