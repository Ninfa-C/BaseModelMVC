using System.Security.Claims;
using HotelManagment.Data;
using HotelManagment.Models;
using HotelManagment.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagment.Services
{
    public class PrenotazioniServices
    {
        private readonly HotelManagmentDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public PrenotazioniServices(HotelManagmentDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        private async Task<bool> SaveAChanges()
        {
            try
            {
                var rowsAffected = await _db.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Camera>> GetAllRooms()
        {
            try
            {
                var room = new List<Camera>();

                room = await _db.Camera
                    .OrderBy(c => c.Numero)
                    .ToListAsync();
                return room;
            }
            catch
            {
                return new List<Camera>();
            }
        }

        public async Task<PrenotazioniListModel> GetAllPrenotazioni()
        {
            try
            {
                var model = new PrenotazioniListModel();
                model.Prenotazioni = await _db.Prenotazioni
                    .Include(p => p.User)
                    .Include(p =>p.Cliente)
                    .Include(p =>p.Camera)
                    .ToListAsync();
                return model;
            }
            catch
            {
                return new PrenotazioniListModel() { Prenotazioni = new List<Prenotazione>() };
            }
        }

        public async Task<bool> DeletePrenotazione(Guid id)
        {
            var result = await _db.Prenotazioni.FindAsync(id);
            if (result == null)
            {
                return false;
            }
            _db.Prenotazioni.Remove(result);
            return await SaveAChanges();
        }

        private async Task<Cliente> FindOrCreate(PrenotazioneAddModel model)
        {
            var cliente = await _db.Cliente.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (cliente == null)
            {
                cliente = new Cliente
                {
                    ClienteId = Guid.NewGuid(),
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    Phone = model.Phone
                };
                _db.Cliente.Add(cliente);
                await SaveAChanges();
            }
            return cliente;
        }


        public async Task<bool> AddPrenotazione(PrenotazioneAddModel model, ClaimsPrincipal claim)
        {
            var worker = await _userManager.FindByEmailAsync(claim.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            var person = await FindOrCreate(model);

            var result = new Prenotazione()
            {
                PrenotazioneId = Guid.NewGuid(),
                UserId = worker.Id,
                ClienteId = person.ClienteId,
                CameraId = model.CameraId,
                DataFine = model.DataFine,
                DataInizio = model.DataInizio,
                Stato = model.Stato
            };
            _db.Prenotazioni.Add(result);
            return await SaveAChanges();
        }
    }
}
