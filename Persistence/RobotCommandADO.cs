using Npgsql;
using System.Collections.Generic;
using robot_controller_api;
using System;
using robot_controller_api.Models;
// Make sure to add the 'using' statement for wherever your RobotCommand model is stored!
// e.g., using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class RobotCommandDataAccess
    {
        // Connection string pointing to your local database
        private const string CONNECTION_STRING = "Host=localhost; Username=postgres; Password=Pass1234; Database=sit331";

        public List<RobotCommand> GetRobotCommands()
        {
            var robotCommands = new List<RobotCommand>();

            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM public.robotcommand", conn);
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                // Reading values off the data reader by column name and casting them
                var command = new RobotCommand(
                    (int)dr["id"],
                    (string)dr["Name"],
                    (bool)dr["ismovecommand"],
                    (DateTime)dr["createddate"],
                    (DateTime)dr["modifieddate"],
                    dr["description"] as string
                );

                robotCommands.Add(command);
            }

            return robotCommands;
        }

        public void InsertRobotCommand(RobotCommand command)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            // Using parameterized queries prevents SQL injection and handles formatting
            var sql = @"INSERT INTO public.robotcommand 
                (""Name"", description, ismovecommand, createddate, modifieddate) 
                VALUES (@name, @desc, @ismove, @created, @modified)";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("name", command.Name);
            // Handle the nullable description carefully
            cmd.Parameters.AddWithValue("desc", command.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("ismove", command.IsMoveCommand);
            cmd.Parameters.AddWithValue("created", command.CreatedDate);
            cmd.Parameters.AddWithValue("modified", command.ModifiedDate);

            cmd.ExecuteNonQuery();
        }

        public void UpdateRobotCommand(int id, RobotCommand command)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            var sql = @"UPDATE public.robotcommand 
                SET ""Name"" = @name, 
                    description = @desc, 
                    ismovecommand = @ismove, 
                    modifieddate = @modified 
                WHERE id = @id";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", command.Name);
            cmd.Parameters.AddWithValue("desc", command.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("ismove", command.IsMoveCommand);
            cmd.Parameters.AddWithValue("modified", DateTime.Now); // Update the modified time

            cmd.ExecuteNonQuery();
        }

        public void DeleteRobotCommand(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            var sql = "DELETE FROM public.robotcommand WHERE id = @id";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);

            cmd.ExecuteNonQuery();
        }
    }
}