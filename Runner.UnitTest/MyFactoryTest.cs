using NUnit.Framework;
using Modules.Interfaces;

namespace Runner.UnitTest
{
    public class MyFactoryTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateInstance_WhenValidClass_ThenInstanceIsCreated()
        {
            string binaryFile = "Modules.dll";
            string className = "Modules.AlphabeticalLowerProducer";

            var ret = MyFactory.CreateInstance<IDataProducer>(binaryFile, className, null);

            Assert.IsNotNull(ret);
            Assert.AreEqual("a", ret.CurrentValue);
        }
    }

}
