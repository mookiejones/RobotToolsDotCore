
using System.IO;
using System.Linq;

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
            var visionText = new KopFile(VisionTechPath);

            var copFile = new KopFile(KopFilePath);
        }

        private const string DIRECTORY = @"C:\kuka";

        [Category("Kop")]
        [Test]
        public void TestDirectory()
        {

            var files = Directory.EnumerateFiles(DIRECTORY, "*.kop", SearchOption.AllDirectories);

            var kopFiles = files.Select(CreateKopFile)
                .ToList();
        }

        private KopFile CreateKopFile(string path)
        {
            var file = new KopFile(path);

            return file;
        }
    }
}
