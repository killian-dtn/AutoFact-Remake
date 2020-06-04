using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestsKAutoFactWrapper.TestClasses;

namespace TestsKAutoFactWrapper
{
    [TestClass]
    public class TestBaseEntity
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        public TestBaseEntity() { }
        
        [TestMethod]
        public void TestBaseEntityGetDbTableName()
        {
            Foo foo = new Foo();
            Bar bar = new Bar();
            Baz baz = new Baz();
            Assert.AreEqual<string>("FOO", foo.GetDbTableName());
            Assert.AreEqual<string>("BAR", bar.GetDbTableName());
            Assert.AreEqual<string>("BAZ", baz.GetDbTableName());
        }

        [TestMethod]
        public void TestBaseEntityGetDbPropValue()
        {
            Bar bar = new Bar();
            bar.BarItem = 45;
            Assert.AreEqual<int>(bar.BarItem, (int)bar.GetDbPropValue("BAR_ITEM"));
        }
    }
}
