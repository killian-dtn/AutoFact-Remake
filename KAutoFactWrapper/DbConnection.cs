using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace KAutoFactWrapper
{
    public class DbConnection
    {
        private Wrapper Wp;
		private string ConnectionString;

		private static DbConnection instance = null;
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

		private DbConnection()
		{
			this.Wp = Wrapper.Instance;
			this.Connection = new MySqlConnection();
		}
	}
}