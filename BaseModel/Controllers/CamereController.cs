using AspNetCoreGeneratedDocument;
using HotelManagment.Services;
using HotelManagment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagment.Controllers
{
    [Authorize]
    public class CamereController : Controller
    {
        private readonly PrenotazioniServices _prenotazioniServices;
        private readonly CamereServices _camereServices;
        public CamereController(PrenotazioniServices p, CamereServices c)
        {
            _prenotazioniServices = p;
            _camereServices = c;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Camere = await _prenotazioniServices.GetAllRooms();
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _camereServices.DeleteRoom(id);
            if (!result)
            {
                TempData["Error"] = "Errore nell'eliminazione del libro dal database";
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("Camere/AggiungiCamera")]
        public IActionResult Add()
        {
            return PartialView("_AddCamera");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddSave(CameraAddModel model)

        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Error while saving entity to database"
                });
            }

            var result = await _camereServices.AddCamera(model);

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
        public async Task<IActionResult> EditCamere(Guid id)
        {
            var result = await _camereServices.GetSingleRoom(id);
            if (result == null)
            {
                return RedirectToAction("Index");
            };

            var model = new CameraEditModel
            {
                IdCamera = result.CameraId,
                Numero = result.Numero,
                Tipo = result.Tipo,
                Prezzo = result.Prezzo,
                Img = result.Img,
            };

            return PartialView("_EditCamera", model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveEdit(CameraEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Error while saving entity to database"
                });
            }
            var result = await _camereServices.EditRoom(model);

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
