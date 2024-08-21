using System.Text;
using BusinessLogic.Domain;
using DataLayer.repositories;

namespace Controllers.ReservationExporter;

public class CsvExporter : Exporter
{
    public DepositRepository DepositRepository { get; set; }
    public CsvExporter(DepositRepository depositRepository)
    {
        DepositRepository = depositRepository;
    }
    public override void Export(List<Reservation> reservations)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Deposito , Reserva , Pago");
        var downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        string baseFileName = "reservas.csv";
        string filePath = Path.Combine(downloadsPath, baseFileName);
        
        if (File.Exists(filePath))
        {
            int counter = 1;
            while (File.Exists(filePath))
            {
                string newFileName = $"{Path.GetFileNameWithoutExtension(baseFileName)}_{counter}{Path.GetExtension(baseFileName)}";
                filePath = Path.Combine(downloadsPath, newFileName);
                counter++;
            }
        }
    
        foreach (var reservation in reservations)
        {
            var deposit = DepositRepository.GetDeposit(reservation.DepositId);
            string conditioning = deposit.Conditioning ? "Si" : "No";
            string paymentStatus; 
            if (reservation.PaymentStatus == PaymentStatus.Paid)
            {
                paymentStatus = "Capturado";
            }
            else if (reservation.PaymentStatus == PaymentStatus.Pending)
            {
                paymentStatus = "Reservado";
            }
            else
            {
                paymentStatus = "Devuelto";
            }
            sb.AppendLine($"Nombre:({deposit.Name})  Area: ({deposit.Area})  Tamaño: ({deposit.Size})  Calefaccion ({conditioning}) , " +
                          $"Id: ({reservation.Id})  Fecha de inicio: ({reservation.StartDate})  Fecha de fin: ({reservation.EndDate}) , " +
                          $"Estado del Pago: {paymentStatus}");
        }
        File.WriteAllText(filePath, sb.ToString());
    }
}