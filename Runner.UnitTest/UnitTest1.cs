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

        [Test]
        public void GetNextPassword_WhenCurrentValueIsSet_ThenValid()
        {
            const string initial_value  = "aaaaa";
            const string expected_value = "baaaa";

            _sut.CurrentValue = initial_value;
            var ret = _sut.GetNextPassword();

            Assert.AreEqual(expected_value, ret);
        }

    }
}