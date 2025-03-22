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
        [Authorize(Roles = "Admin,Manager")]

        [HttpGet("Prenotazioni/AggiungiPrenotazione")]
        public async Task<IActionResult> Add()
        {
            ViewBag.Rooms = await _prenotazioniServices.GetAvaibleRooms();
            return PartialView("_AddPrenotazioni");
        }
        [Authorize(Roles = "Admin,Manager")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _prenotazioniServices.DeletePrenotazione(id);
            if (!result)
            {
                TempData["Error"] = "Errore nell'eliminazione del libro dal database";
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> EditPrenotazione(Guid id)
        {
            var result = await _prenotazioniServices.GetSinglePrenotazione(id);
            if (result == null)
            {
                return RedirectToAction("Index");
            };

            var model = new PrenotazioniEditModel
            {
                IdPrenotazione = result.PrenotazioneId,
                CameraId = result.CameraId,
                DataInizio = result.DataInizio,
                DataFine = result.DataFine,
                ClienteId = result.ClienteId,
                Stato = result.Stato,
                Name = result.Cliente.Name,
                Surname = result.Cliente.Surname,
                Email = result.Cliente.Email,
                Phone = result.Cliente.Phone,
            };

            ViewBag.Rooms = await _prenotazioniServices.GetAvaibleRooms();
            return PartialView("_EditPrenotazioni", model);
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> EditSave(PrenotazioniEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Error while saving entity to database"
                });
            }
            var result = await _prenotazioniServices.EditPrenotazione(model);

            if (!result)
            {
                return Json(new
                {
                    success = false,
                    message = "Error while editing entity to database"
                });
            }

            string logmessage = "Entity edited successfully to database";

            return Json(new
            {
                success = true,
                message = logmessage
            });
        }


    }
}
