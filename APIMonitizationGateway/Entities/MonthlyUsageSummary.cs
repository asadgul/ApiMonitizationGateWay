namespace APIMonitizationGateway.Entities
{
    public class MonthlyUsageSummary
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalRequests { get; set; }
        public decimal Price { get; set; }
    }
}
