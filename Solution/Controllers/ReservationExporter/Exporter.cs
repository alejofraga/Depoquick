using BusinessLogic.Domain;
using DataLayer.repositories;

namespace Controllers.ReservationExporter;

public abstract class Exporter
{
    public abstract void Export(List<Reservation> reservations);
    
    public static Exporter CreateExporter(string format, DepositRepository depositRepository)
    {
        return format switch
        {
            "csv" => new CsvExporter(depositRepository),
            "txt" => new TxtExporter(depositRepository),
            _ => throw new ArgumentException("Formato no soportado")
        };
    }
}