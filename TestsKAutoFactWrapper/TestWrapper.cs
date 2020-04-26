using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using SqlKata.Compilers;
using SqlKata.Execution;
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
        public DbConnection DbConnection_ { get; private set; }
        public QueryFactory KataFactory { get; private set; }

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        public Type[] TestTypes { get; private set; }

        public TestWrapper()
        {
            this.Wrapper_ = Wrapper.Instance;
            this.DbConnection_ = DbConnection.Instance;
            this.KataFactory = new QueryFactory(this.DbConnection_.Connection, new MySqlCompiler());
            this.TestTypes = new Type[] { typeof(Foo), typeof(Bar), typeof(Baz) };
        }

        [TestMethod]
        public void TestWrapperLoading()
        {
            try
            {
                foreach(Type t in this.TestTypes)
                {
                    Assert.AreEqual<Type>(t, this.Wrapper_.ClassByTable[t.GetCustomAttribute<DbClassAttribute>().DbName]);
                    Assert.AreEqual<string>(this.Wrapper_.TableByClass[t], t.GetCustomAttribute<DbClassAttribute>().DbName);
                }
            }
            catch (IndexOutOfRangeException e) { Assert.Fail(e.Message); }
            catch (NullReferenceException e) { Assert.Fail(e.Message); }
            catch (Exception) { throw; }
        }

        [TestMethod]
        public void TestWrapperTableStructsLoading()
        {
            Dictionary<string, Dictionary<string, PropertyInfo>> ExpectedStructs = new Dictionary<string, Dictionary<string, PropertyInfo>>
            {
                {
                    "FOO", new Dictionary<string, PropertyInfo>
                    {
                        { "ID", typeof(Foo).GetProperty("Id") },
                        { "FOO_ITEM", typeof(Foo).GetProperty("FooItem") }
                    }
                },
                {
                    "BAR", new Dictionary<string, PropertyInfo>
                    {
                        { "ID", typeof(Bar).GetProperty("Id") },
                        { "BAR_ITEM", typeof(Bar).GetProperty("BarItem") }
                    }
                },
                {
                    "BAZ", new Dictionary<string, PropertyInfo>
                    {
                        { "ID", typeof(Baz).GetProperty("Id") },
                        { "BAZ_ITEM", typeof(Baz).GetProperty("BazItem") }
                    }
                }
            };

            try
            {
                foreach (KeyValuePair<string, Dictionary<string, PropertyInfo>> Table in ExpectedStructs)
                    foreach (KeyValuePair<string, PropertyInfo> Line in Table.Value)
                        Assert.AreEqual<PropertyInfo>(Line.Value, this.Wrapper_.TableStructs[Table.Key][Line.Key]);
            }
            catch (IndexOutOfRangeException e) { Assert.Fail(e.Message); }
            catch (NullReferenceException e) { Assert.Fail(e.Message); }
            catch (Exception) { throw; }
        }

        [TestMethod]
        public void TestWrapperPrimaryKeyLoading()
        {
            PrimaryKeyStruct Foo_PKStruct = new PrimaryKeyStruct(this.TestTypes[0], this.TestTypes[0].GetProperty("Id"));
            PrimaryKeyStruct Bar_PKStruct = new PrimaryKeyStruct(this.TestTypes[1], this.TestTypes[1].GetProperty("Id"));
            PrimaryKeyStruct Baz_PKStruct = new PrimaryKeyStruct(this.TestTypes[2], this.TestTypes[2].GetProperty("Id"));
            Dictionary<string, PrimaryKeyStruct> PKStructs = new Dictionary<string, PrimaryKeyStruct>
            { 
                { "FOO", Foo_PKStruct }, 
                { "BAR", Bar_PKStruct }, 
                { "BAZ", Baz_PKStruct } 
            };

            try
            {
                foreach (Type t in this.TestTypes)
                {
                    DbClassAttribute dca = t.GetCustomAttribute<DbClassAttribute>();
                    Assert.AreEqual<PrimaryKeyStruct>(PKStructs[dca.DbName], this.Wrapper_.PrimaryKeysOfTables[dca.DbName]);
                }
            }
            catch (IndexOutOfRangeException e) { Assert.Fail(e.Message); }
            catch (NullReferenceException e) { Assert.Fail(e.Message); }
            catch (Exception) { throw; }
        }

        [TestMethod]
        public void TestWrapperForeignKeyLoading()
        {
            ForeignKeyStruct Bar_FKStruct = new ForeignKeyStruct(typeof(Bar), new Dictionary<PropertyInfo, PropertyInfo> { { this.TestTypes[1].GetProperty("Id"), this.TestTypes[0].GetProperty("Id") } });
            ForeignKeyStruct Baz_FKStruct = new ForeignKeyStruct(typeof(Baz), new Dictionary<PropertyInfo, PropertyInfo> { { this.TestTypes[2].GetProperty("Id"), this.TestTypes[1].GetProperty("Id") } });

            Dictionary<string, ForeignKeyStruct> FKStructs = new Dictionary<string, ForeignKeyStruct>
            {
                { "FOO", new ForeignKeyStruct(typeof(Foo)) },
                { "BAR", Bar_FKStruct },
                { "BAZ", Baz_FKStruct }
            };

            try
            {
                foreach (KeyValuePair<string, ForeignKeyStruct> FKStruct in FKStructs)
                    Assert.AreEqual<ForeignKeyStruct>(FKStruct.Value, this.Wrapper_.ForeignKeysOfTables[FKStruct.Key]);
            }
            catch (IndexOutOfRangeException e) { Assert.Fail(e.Message); }
            catch (NullReferenceException e) { Assert.Fail(e.Message); }
            catch (Exception) { throw; }
        }

        [TestMethod]
        public void TestWrapperGetClassExtendsTree()
        {
            Dictionary<Type, List<string>> ExtendsTrees = new Dictionary<Type, List<string>>
            {
                { typeof(Foo), new List<string>() },
                { typeof(Bar), new List<string> { "FOO" } },
                { typeof(Baz), new List<string> { "BAR", "FOO" } }
            };

            Assert.AreEqual<List<string>>(new List<string>(), this.Wrapper_.GetClassExtendsTree<Foo>());
            Assert.AreEqual<List<string>>(new List<string> { "FOO" }, this.Wrapper_.GetClassExtendsTree<Bar>());
            Assert.AreEqual<List<string>>(new List<string> { "BAR", "FOO" }, this.Wrapper_.GetClassExtendsTree<Baz>());
        }
    }
}
