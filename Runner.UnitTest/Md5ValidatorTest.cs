using NUnit.Framework;
using Modules;

namespace Runner.UnitTest
{
    public class Md5ValidatorTest
    {
        Md5Validator _sut;
        [SetUp]
        public void Setup()
        {
            _sut = new Md5Validator(null);
        }

        [TestCase("corona", "8215E48BD370871E71A61118277B6876")]
        [TestCase("baba",   "21661093E56E24CD60B10092005C4AC7")]
        [TestCase("aaaaa",  "594F803B380A41396ED63DCA39503542")]
        public void CurrentValue_WhenSet_ThenValid(string input, string expectedValue)
        {

            var ret = _sut.Md5Hash(input);

            Assert.AreEqual(expectedValue, ret);
        }
    }

}