namespace APIMonitizationGateway.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TierId { get; set; }
        public Tier Tier { get; set; } 
    }
}
