using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace KAutoFactWrapper
{
    public class DbConnection
    {
        private Wrapper Wrapper_;
		private string ConnectionString;
		private MySqlCompiler KataCompiler;
		private QueryFactory KataFactory;

		private static DbConnection instance = new DbConnection();
		public static DbConnection Instance
		{
			get
			{
				if (DbConnection.instance == null)
					DbConnection.instance = new DbConnection();
				return DbConnection.instance;
			}
		}

		public MySqlConnection Connection { get; private set; }

		static DbConnection() { }
		private DbConnection()
		{
			this.Wrapper_ = Wrapper.Instance;
			this.Connection = new MySqlConnection();
			this.KataCompiler = new MySqlCompiler();
			this.KataFactory = new QueryFactory(this.Connection, this.KataCompiler);
		}

		public object SelectAll<T>() where T : BaseEntity<T>
		{
			return this.Wrapper_.CreateSelectAllRequest<T>(this.KataFactory).Get();
		}

		public T SelectByPrimaryKey<T>(Dictionary<string, object> PrimaryKeys) where T : BaseEntity<T>
		{
			object res = this.Wrapper_.CreateSelectByPrimaryKeyRequest<T>(this.KataFactory, PrimaryKeys).First();
			throw new NotImplementedException();
		}

		public void Insert<T>(T Entity) where T : BaseEntity<T>
		{
			Query q = this.Wrapper_.CreateInsertRequest<T>(this.KataFactory, Entity);
			throw new NotImplementedException();
		}

		public void Update<T>(T Entity) where T : BaseEntity<T>
		{
			Query q = this.Wrapper_.CreateUpdateRequest<T>(this.KataFactory, Entity);
			throw new NotImplementedException();
		}

		public void Delete<T>(T Entity) where T : BaseEntity<T>
		{
			Query q = this.Wrapper_.CreateDeleteRequest<T>(this.KataFactory, Entity);
			throw new NotImplementedException();
		}
	}
}