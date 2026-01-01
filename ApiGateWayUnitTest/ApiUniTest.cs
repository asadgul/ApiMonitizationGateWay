using APIMonitizationGateway.Models;

namespace ApiGateWayUnitTest
{
    public class ApiUniTest
    {
        [Fact]
        public void Allow_ShouldReturnTrue_WhenWithinLimit()
        {
            // Arrange
            var limiter = new RateLimiter();
            int limit = 2;

            // Act
            bool first = limiter.Allow(limit);
            bool second = limiter.Allow(limit);

            // Assert
            Assert.True(first);
            Assert.True(second);
        }
        [Fact]
        public void Allow_ShouldReturnFalse_WhenLimitExceeded()
        {
            // Arrange
            var limiter = new RateLimiter();
            int limit = 2;

            // Act
            limiter.Allow(limit);
            limiter.Allow(limit);
            bool third = limiter.Allow(limit);

            // Assert
            Assert.False(third);
        }
        
    }
}