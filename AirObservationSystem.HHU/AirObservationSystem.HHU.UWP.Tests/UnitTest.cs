using Xunit;

namespace AirObservationSystem.HHU.UWP.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        [Theory]
        [InlineData(5)]
        [InlineData(3)]
        public void PassingOdd(int value)
        {
            Assert.True(IsOdd(value));
        }

        [Theory]
        [InlineData(6)]
        [InlineData(2)]
        public void FailingOdd(int value)
        {
            Assert.True(IsOdd(value));
        }

        bool IsOdd(int value)
        {
            return value % 2 == 1;
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
