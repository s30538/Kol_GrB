namespace WebApplication1.Models.DTOs;

public class GetVisits
{
    public int Id_Client { get; set; }
    public string FirstName_Client { get; set; }
    public string LastName_Client { get; set; }
    public DateTime DateOfBirth_Client { get; set; }
    
    public int Id_Mechanic { get; set; }
    public string Licence_Mechanic { get; set; }
    
    
    public List<Visit> Visit { get; set; }
}

public class Visit
{
    public string Name { get; set; }
    public float serviceFee { get; set; }
}