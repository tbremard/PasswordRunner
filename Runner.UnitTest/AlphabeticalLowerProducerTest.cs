using NUnit.Framework;

namespace Runner.UnitTest
{
    public class AlphabeticalLowerProducerTest
    {
        AlphabeticalLowerProducer _sut;
        [SetUp]
        public void Setup()
        {
            _sut = new AlphabeticalLowerProducer();
        }

        [Test]
        public void CurrentValue_WhenSet_ThenValid()
        {
            const string initial_value = "aaaaa";

            _sut.CurrentValue = initial_value;

            Assert.AreEqual(initial_value, _sut.CurrentValue);
        }

        [TestCase("aaaaa", "aaaab")]
        [TestCase("aaaaz", "aaaba")]
        [TestCase("azzzz", "baaaa")]
        public void GetNextPassword_WhenCurrentValueIsSet_ThenValid(string initialValue, string expectedValue)
        {
            _sut.CurrentValue = initialValue;

            var ret = _sut.GetNextPassword();

            Assert.AreEqual(expectedValue, ret);
        }

    }
}