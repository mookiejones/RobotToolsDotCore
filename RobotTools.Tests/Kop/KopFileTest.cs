
using NUnit.Framework;
using RobotTools.Core.Kop;

namespace RobotTools.Tests.Kop
{
    [TestFixture]
    public class KopFileTest
    {
        private const string KopFilePath = @"C:\kuka\AceShopRobot\KUKA.PROFINET MS\V_4_1_4_18\KUKA.PROFINET MS.kop";
        private const string VisionTechPath = @"C:\temp\VisionTech.kop";
        [Category("Kop")]
        [Test]
        public void TestFile()
        {
            Assert.IsTrue(true);
        }

       
    }
} 
