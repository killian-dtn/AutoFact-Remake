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
            Foo foo = new Foo(0);
            Bar bar = new Bar(0);
            Baz baz = new Baz(0);
            Assert.AreEqual<string>("FOO", foo.GetDbTableName());
            Assert.AreEqual<string>("BAR", bar.GetDbTableName());
            Assert.AreEqual<string>("BAZ", baz.GetDbTableName());
        }

        [TestMethod]
        public void TestBaseEntityGetDbPropValue()
        {
            Bar bar = new Bar(0);
            bar.BarItem = 45;
            Assert.AreEqual<int>(bar.BarItem, (int)bar.GetDbPropValue("BAR_ITEM"));
        }
    }
}
