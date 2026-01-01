namespace APIMonitizationGateway.Entities
{
    public class Tier
    {
        public int Id { get; set; }
        public string Name { get; set; } =string.Empty;
        public int MonthlyQuota { get; set; }
        public int RateLimitPerSecond { get; set; }
        public decimal MonthlyPrice { get; set; }
    }
}
