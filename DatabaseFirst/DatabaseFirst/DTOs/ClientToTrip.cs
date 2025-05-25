namespace DatabaseFirst.DTOs;

public class ClientToTrip
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string email { get; set; }
    public string phoneNumber { get; set; }
    public string pesel { get; set; }
    public int idTrip { get; set; }
    public string tripName { get; set; }
    public DateTime? paymentDate { get; set; }
}