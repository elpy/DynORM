using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Kernel
{
    class Program
    {
        static void Main(string[] args)
        {
            var b = new NpgsqlConnectionStringBuilder() { Host = "milkyway", Port = 5433, Username = "neocortex", Password = "masterboot", Database = "db" };

            using (DbConnection conn = new NpgsqlConnection(b.ToString()))
            {
                conn.Open();

                var tables = new List<string>();
                var all = conn.GetSchema("Tables");
                foreach (DataRow r in all.Rows)
                {
                    //table_catalog //table_schema //table_name //table_type
                    var schema = r["table_schema"];
                    var table = r["table_name"];
                    var type = r["table_type"];

                    if ("kernel".Equals(schema.ToString()))
                        tables.Add(table.ToString());
                }

                foreach (var table in tables)
                {
                    Console.WriteLine("Table: " + table);

                    var tableSchema = conn.GetSchema("Columns", new string[] { null, null, table });
                    foreach (DataRow row in tableSchema.Rows)
                    {
                        Console.WriteLine("Column = {0}. Type = {1}. Default = {2}. Nullable = {3}. Text lenght = {4}. Numeric precision = {5}.",
                            row["column_name"], 
                            row["data_type"], 
                            row["column_default"], 
                            row["is_nullable"], 
                            row["character_maximum_length"], 
                            row["numeric_precision"]);
                    }
                }





                /*var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT * FROM kernel.users;";

                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        Console.WriteLine(string.Format("id = {0}, user = {1};", reader.GetString(0), reader.GetString(1)));
                */



            }

            Console.ReadKey();
        }
    }
}








/*table_catalog 
table_schema
table_name
column_name
ordinal_position
column_default
is_nullable
data_type
character_maximum_length
character_octet_length
numeric_precision
numeric_precision_radix
numeric_scale
datetime_precision
character_set_catalog
character_set_schema
character_set_name
collation_catalog*/
