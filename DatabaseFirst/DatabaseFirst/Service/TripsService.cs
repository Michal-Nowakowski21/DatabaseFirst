using DatabaseFirst.DTOs;
using DatabaseFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseFirst.Service;

public class TripsService:ITripsService
{
    private readonly Database12Context _dbContext;

    public TripsService(Database12Context dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<TripsSorted> getTripsSorted(int page = 1, int pageSize = 10)
    {
        var totalTrips = await _dbContext.Trips.CountAsync();
        var totalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

        var trips = await _dbContext.Trips
            .Include(t => t.Client_Trips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries)
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new tripsDTO
            {
                name = t.Name,
                description = t.Description,
                dateFrom = t.DateFrom,
                dateTo = t.DateTo,
                maxPeople = t.MaxPeople,
                countries = t.IdCountries.Select(c => new CountriesDto
                {
                    name = c.Name
                }).ToArray(),
                clients = t.Client_Trips.Select(ct => new ClientsDto
                {
                    firstname = ct.IdClientNavigation.FirstName,
                    lastname = ct.IdClientNavigation.LastName
                }).ToArray()
            })
            .ToArrayAsync();

        return new TripsSorted
        {
            pageNum = page,
            pageSize = pageSize,
            allPages = totalPages,
            trips = trips
        };
    }

    public async Task<bool> checkClientTrips(int idClient)
    {
        var totalTrips = await _dbContext.Client_Trips.CountAsync(ct => ct.IdClient == idClient);
        if (totalTrips>0)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> checkTripDate(ClientToTrip client)
    {
        var trip = await _dbContext.Trips.FindAsync(client.idTrip); 
        if ((trip!=null) & (trip.DateFrom> DateTime.Today))
        {
            return true;
        }
        return false;
    }
    

}