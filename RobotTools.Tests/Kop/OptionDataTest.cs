#if LOCAL_MACHINE
namespace RobotTools.Tests.Kop
{
    [TestFixture]
    public class OptionDataTest
    {
        private string _text;
        [SetUp]
        public void SetUp()
        {
            _text = File.ReadAllText(@"C:\temp\VisionTech\MetaData.xml");
        }
        [Category("OptionData")]
        [Test]
        public void CreateTest()
        {
            var optionData = _text.FromXml<OptionData>();

            Assert.IsTrue(true);
        }
    }
}
#endif
