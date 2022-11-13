using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using NUnit.Framework;

using RobotTools.Core.Kop;
using RobotTools.Core.Utilities;

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
