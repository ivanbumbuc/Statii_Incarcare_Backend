namespace Statii_Incarcare_Proiect_Tehnologii_Web.Dto;

public class BookingAdd
{
    public Guid userId { get; set; }
    public Guid plugId { get; set; }
    public String startingHour { get; set; }
    public String endingHour { get; set; }
}