using DatabaseFirst.DTOs;

namespace DatabaseFirst.Service;

public interface ITripsService
{
    Task<TripsSorted> getTripsSorted(int page = 1, int pageSize = 10);
    Task<bool> checkClientTrips(int idClient);
    Task<bool> checkTripDate(ClientToTrip client);
}