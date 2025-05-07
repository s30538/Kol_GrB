namespace WebApplication1.Models.DTOs;

public class AddVisits
{
    public int Visits_Id { get; set; }
    public int Client_Id { get; set; }
    public string MechanicaNumber { get; set; }
    public List<Service> services { get; set; }
}

public class Service
{
    public string ServiceName { get; set; }
    public int ServiceFee { get; set; }
    
}