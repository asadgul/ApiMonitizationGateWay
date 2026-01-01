namespace APIMonitizationGateway.Entities
{
    public class ApiUsageLog
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Endpoint { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }

    }
}
