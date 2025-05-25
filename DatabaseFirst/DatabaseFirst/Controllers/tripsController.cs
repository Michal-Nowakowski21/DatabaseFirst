using DatabaseFirst.DTOs;
using DatabaseFirst.Models;
using DatabaseFirst.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatabaseFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tripsController : ControllerBase
    {
        private ITripsService _tripsService;
        private readonly Database12Context _dbContext;

        public tripsController(ITripsService tripsService,Database12Context dbContext)
        {
            _tripsService = tripsService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10)
        {
            var trips = await _tripsService.getTripsSorted(page, pageSize);

            return Ok(trips);
        }

        [HttpDelete("{idClient}")]
        public async Task<IActionResult> Delete(int idClient)
        {
            var trips = await _tripsService.checkClientTrips(idClient);
            if (trips)
            {
                return BadRequest("You are not allowed to delete clients that have trips");
            }
            else
            {
                var client = await _dbContext.Clients.FindAsync(idClient);
                _dbContext.Clients.Remove(client);
                await _dbContext.SaveChangesAsync();
                return Ok("Client deleted");
            }
            
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> Post(ClientToTrip client)
        {
            var clientt = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Pesel == client.pesel);
            if (clientt != null)
            {
                return BadRequest("Client already exists");
                
            }
            if (clientt == null)
            {
                clientt = new Client
                {
                    FirstName = client.firstName,
                    LastName = client.lastName,
                    Email = client.email,
                    Telephone = client.phoneNumber,
                    Pesel = client.pesel
                };
                _dbContext.Clients.Add(clientt);
                await _dbContext.SaveChangesAsync();
            }
            var clienttrip = await _dbContext.Client_Trips.FindAsync(clientt.IdClient, client.idTrip);
            if (clienttrip != null)
            {
                if (clienttrip.PaymentDate != null && client.paymentDate == null)
                {
                    return BadRequest("Client already paid for this trip — cannot set payment date to null.");
                }
                return BadRequest("Client already added to this trip");
            }

            var tripCheck = await _tripsService.checkTripDate(client);
            if (!tripCheck)
            {
                return BadRequest("Trip doesnt exist or has already started");
            }
            var newClientTrip = new Client_Trip
            {
                IdClient = clientt.IdClient,
                IdTrip = client.idTrip,
                RegisteredAt = DateTime.UtcNow,
                PaymentDate = client.paymentDate
            };

            _dbContext.Client_Trips.Add(newClientTrip);
            await _dbContext.SaveChangesAsync();
            return Ok("Trip added");
        }
    }
}
