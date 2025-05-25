namespace DatabaseFirst.DTOs;

public class TripsSorted
{
    public int pageNum{get;set;}
    public int pageSize{get;set;}
    public int allPages{get;set;}
    public tripsDTO[] trips{get;set;}
}

public class tripsDTO
{
    public string name{get;set;}
    public string description{get;set;}
    public DateTime dateFrom{get;set;}
    public DateTime dateTo{get;set;}
    public int maxPeople{get;set;}
    public CountriesDto[] countries{get;set;}
    public ClientsDto[] clients{get;set;}
}

public class CountriesDto
{
    public string name{get;set;}
}

public class ClientsDto
{
    public string firstname{get;set;}
    public string lastname{get;set;}
}