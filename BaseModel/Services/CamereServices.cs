using System.Security.Claims;
using HotelManagment.Data;
using HotelManagment.Models;
using HotelManagment.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagment.Services
{
    public class CamereServices
    {
        private readonly HotelManagmentDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PrenotazioniServices _prenotazioniServices;
        public CamereServices(HotelManagmentDbContext db, UserManager<ApplicationUser> userManager, PrenotazioniServices p)
        {
            _db = db;
            _userManager = userManager;
            _prenotazioniServices = p;
        }

        public async Task<bool> DeleteRoom(Guid id)
        {
            var result = await _db.Camera.FindAsync(id);
            if (result == null)
            {
                return false;
            }
            _db.Camera.Remove(result);
            return await _prenotazioniServices.SaveAChanges();
        }


        public async Task<bool> AddCamera(CameraAddModel model)
        {
            var fileName = model.img.FileName;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload", "images", fileName);

            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.img.CopyToAsync(stream);
            }
            var webPath = Path.Combine("upload", "images", fileName);
            var result = new Camera()
            {
                CameraId = Guid.NewGuid(),
                Numero = model.Numero,
                Tipo = model.Tipo,
                Prezzo = model.Prezzo,
                Img = webPath,
                IsLoan = false,

            };
            _db.Camera.Add(result);
            return await _prenotazioniServices.SaveAChanges();
        }


        public async Task<Camera?> GetSingleRoom(Guid id)
        {
            try
            {
                var result = await _db.Camera.FindAsync(id);

                if (result == null)
                {
                    return null;
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> EditRoom(CameraEditModel model)
        {
            string webPath;
            if (model.imgFile != null)
            {
                var fileName = model.imgFile.FileName;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload", "images", fileName);
                await using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.imgFile.CopyToAsync(stream);
                }
                webPath = Path.Combine("upload", "images", fileName);
            }
            else
            {
                webPath = model.Img;
            }

            var result = await _db.Camera.FindAsync(model.IdCamera);
            if (result == null)
            {
                return false;
            }
            result.Numero = model.Numero;
            result.Tipo = model.Tipo;
            result.Prezzo = model.Prezzo;
            result.Img = webPath;

            return await _prenotazioniServices.SaveAChanges();
        }


    }
}
