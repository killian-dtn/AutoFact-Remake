using System;
using System.Reflection;
using KAutoFactWrapper;
using KAutoFactWrapper.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestsKAutoFactWrapper.TestClasses;

[assembly: Wrapper]
namespace TestsKAutoFactWrapper
{
    [TestClass]
    public class TestWrapper
    {
        public Wrapper Wrapper_ { get; private set; }
        
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        public TestWrapper()
        {
            this.Wrapper_ = Wrapper.Instance;
        }

        [TestMethod]
        public void TestWrapperLoading()
        {
            try
            {
                Type[] Types = { typeof(Foo), typeof(Bar), typeof(Baz) };
                foreach(Type t in Types)
                {
                    Assert.AreEqual<Type>(t, this.Wrapper_.ClassByTable[t.GetCustomAttribute<DbClassAttribute>().DbName]);
                    Assert.AreEqual<string>(this.Wrapper_.TableByClass[t], t.GetCustomAttribute<DbClassAttribute>().DbName);
                }
            }
            catch (IndexOutOfRangeException e) { Assert.Fail(e.Message); }
            catch (NullReferenceException e) { Assert.Fail(e.Message); }
            catch (Exception e) { Assert.Fail(e.Message); }
        }
    }
}
