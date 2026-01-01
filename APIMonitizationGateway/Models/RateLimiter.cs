namespace APIMonitizationGateway.Models
{
    public class RateLimiter
    {
        private int _count;
        private DateTime _window = DateTime.UtcNow;
        public bool Allow(int limit)
        {
            if ((DateTime.UtcNow - _window).TotalSeconds >= 1)
            {
                _count = 0;
                _window = DateTime.UtcNow;
            }
            _count++;
            return _count <= limit;
        }
    }
}
