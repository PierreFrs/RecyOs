using Xunit;
using System.Threading;
using RecyOs.Engine.Services;

namespace RecyOs.Tests
{
    public class SynchroWaitingTokenTests
    {
        [Fact]
        public void GetCancellationToken_ShouldReturnNonCancelledToken_WhenNewInstance()
        {
            // Arrange
            var tokenService = new SynchroWaitingToken();

            // Act
            var token = tokenService.GetCancellationToken();

            // Assert
            Assert.False(token.IsCancellationRequested);
        }

        [Fact]
        public void StopWaiting_ShouldCancelToken()
        {
            // Arrange
            var tokenService = new SynchroWaitingToken();

            // Act
            tokenService.StopWaiting();
            var token = tokenService.GetCancellationToken();

            // Assert
            Assert.True(token.IsCancellationRequested);
        }

        [Fact]
        public void ResetWaitingToken_ShouldResetCancellation()
        {
            // Arrange
            var tokenService = new SynchroWaitingToken();
            tokenService.StopWaiting();

            // Act
            tokenService.ResetWaitingToken();
            var token = tokenService.GetCancellationToken();

            // Assert
            Assert.False(token.IsCancellationRequested);
        }
    }
}
