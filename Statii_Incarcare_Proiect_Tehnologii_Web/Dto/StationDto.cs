namespace Statii_Incarcare_Proiect_Tehnologii_Web.Dto;

public class StationDto
{
    public String name { get; set; }

    public String city { get; set; }

    public String address { get; set; }

    public decimal coordX { get; set; }

    public decimal coordY { get; set; }
    
    public Guid idUser { get; set; }

}