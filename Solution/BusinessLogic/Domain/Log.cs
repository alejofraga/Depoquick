namespace BusinessLogic.Domain
{
    public class Log
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ClientEmail { get; set; }
        public Client Client { get; set; }
        
        public void AddAction(string newAction,DateTime time)
        {
            string[] validActions = { "Inicio de sesión", "Cierre de sesión", "Agregó una nueva reseña" };
            if (validActions.Contains(newAction))
            {
                Description = $"{time}: {newAction}";
            }
            else
            {
                throw new ArgumentException("Ocurrio un error al guardar en el historial de acciones");
            }
        }
    }
}