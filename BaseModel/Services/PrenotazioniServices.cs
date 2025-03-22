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

        public async Task<bool> SaveAChanges()
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

        public async Task<List<Camera>> GetAvaibleRooms()
        {
            try
            {
                var room = new List<Camera>();

                room = await _db.Camera
                    .Where(b => b.IsLoan == false)
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
                    .Include(p => p.Cliente)
                    .Include(p => p.Camera)
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
            var room = await _db.Camera.FirstOrDefaultAsync(c => c.CameraId == result.CameraId);
            room.IsLoan = false;
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

        private async Task<Cliente> FindOrCreate(PrenotazioniEditModel model)
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
            else
            {
                cliente.Name = model.Name;
                cliente.Surname = model.Surname;
                cliente.Email = model.Email;
                cliente.Phone = model.Phone;
                 
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
                Stato = "Confermata"
            };
            _db.Prenotazioni.Add(result);
            var room = await _db.Camera.FirstOrDefaultAsync(c => c.CameraId == model.CameraId);
            room.IsLoan = true;

            return await SaveAChanges();
        }

        public async Task<Prenotazione?> GetSinglePrenotazione(Guid id)
        {
            try
            {
                var result = await _db.Prenotazioni.FindAsync(id);

                if (result == null)
                {
                    return null;
                }
                var cliente = await _db.Cliente.FindAsync(result.ClienteId);
                if (cliente == null)
                {
                    return null;
                }
                result.Cliente = cliente;
                var camera = await _db.Camera.FindAsync(result.CameraId);
                if (camera == null)
                {
                    return null;
                }
                camera.IsLoan = false;
                await SaveAChanges();
                result.Camera = camera;
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> EditPrenotazione(PrenotazioniEditModel model) 
        {
            var person = await FindOrCreate(model);
            var result = await _db.Prenotazioni.FindAsync(model.IdPrenotazione);
            if (result == null)
            {
                return false;
            }
            result.DataInizio = model.DataInizio;
            result.DataFine = model.DataFine;
            result.CameraId = model.CameraId;
            result.Stato = model.Stato;

            var room = await _db.Camera.FirstOrDefaultAsync(c => c.CameraId == model.CameraId);
            room.IsLoan = true;

            return await SaveAChanges();
        }
    }
}
