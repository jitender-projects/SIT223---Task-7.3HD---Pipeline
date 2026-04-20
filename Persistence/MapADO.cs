using Npgsql;
using System.Collections.Generic;
using robot_controller_api;
using System;
using robot_controller_api.Models;
// Make sure to add your Models using statement here!

namespace robot_controller_api.Persistence
{
    public class MapDataAccess
    {
        private const string CONNECTION_STRING = "Host=localhost; Username=postgres; Password=Pass1234; Database=sit331";

        public List<Map> GetMaps()
        {
            var maps = new List<Map>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();
            
            using var cmd = new NpgsqlCommand("SELECT * FROM public.map", conn);
            using var dr = cmd.ExecuteReader();
            
            while (dr.Read())
            {
                var map = new Map(
                    (int)dr["id"],
                    (int)dr["columns"],
                    (int)dr["rows"],
                    (string)dr["Name"],
                    (DateTime)dr["createddate"],
                    (DateTime)dr["modifieddate"],
                    dr["description"] as string // or null if not present
                );
                maps.Add(map);
            }
            return maps;
        }

        public void InsertMap(Map map)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            var sql = @"INSERT INTO public.map (""Name"", columns, rows, createddate, modifieddate) 
                        VALUES (@name, @cols, @rows, @created, @modified)";
                        
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("name", map.Name);
            cmd.Parameters.AddWithValue("cols", map.Columns);
            cmd.Parameters.AddWithValue("rows", map.Rows);
            cmd.Parameters.AddWithValue("created", map.CreatedDate);
            cmd.Parameters.AddWithValue("modified", map.ModifiedDate);

            cmd.ExecuteNonQuery();
        }

        public void UpdateMap(int id, Map map)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            var sql = @"UPDATE public.map 
                        SET ""Name"" = @name, columns = @cols, rows = @rows, modifieddate = @modified 
                        WHERE id = @id";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", map.Name);
            cmd.Parameters.AddWithValue("cols", map.Columns);
            cmd.Parameters.AddWithValue("rows", map.Rows);
            cmd.Parameters.AddWithValue("modified", DateTime.Now);

            cmd.ExecuteNonQuery();
        }

        public void DeleteMap(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            var sql = "DELETE FROM public.map WHERE id = @id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);

            cmd.ExecuteNonQuery();
        }
    }
}