using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace EntityAI
{
    public class DAC
    {
        private string SQLFileName = "mytestfile.sqlite";
        private SQLiteConnection Con;

        private string tblTest = "testTable";

        public DAC ()
        {
            Con = new SQLiteConnection($"Data Source={SQLFileName};");
        }


        public void CreateFile()
        {
            SQLiteConnection.CreateFile(SQLFileName);
        }

        public void CreateTable()
        {
            string sql = $"CREATE TABLE {tblTest} (name VARCHAR(20), score INT)";
            RunCommand(sql);
        }

        public void InsertData()
        {
            string sql = $"insert into {tblTest} (name, score) values ('Me', 3000)";
            RunCommand(sql);

            sql = $"insert into {tblTest} (name, score) values ('Myself', 6000)";
            RunCommand(sql);

            sql = $"insert into {tblTest} (name, score) values ('And I', 9001)";
            RunCommand(sql);
        }

        public DataSet SelectData()
        {
            string sql = $"select * from {tblTest} order by score desc";
            return RunSelect(sql);
        }

        private DataSet RunSelect(string sql)
        {
            DataSet ds = new DataSet();
            var da = new SQLiteDataAdapter(sql, Con);

            try
            {
                Con.Open();
                da.Fill(ds);

            }
            finally
            {
                Con.Close();
            }

            return ds;
        }
        private void RunCommand(string sql)
        {
            SQLiteCommand comm = new SQLiteCommand(sql, Con);
            try
            {
                Con.Open();
                comm.ExecuteNonQuery();
            }
            finally
            {
                Con.Close();
            }
        }
    }
}
