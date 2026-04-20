using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using robot_controller_api;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

public class RobotCommandRepository : IRobotCommandDataAccess, IRepository
{
    private IRepository _repo => this;

    public List<Models.RobotCommand> GetRobotCommands()
    {
        return _repo.ExecuteReader<Models.RobotCommand>("SELECT * FROM public.robotcommand");
    }

    public void InsertRobotCommand(Models.RobotCommand command)
    {
        var sqlParams = new NpgsqlParameter[] {
            new("name", command.Name),
            new("desc", command.Description ?? (object)DBNull.Value),
            new("ismove", command.IsMoveCommand),
            new("created", command.CreatedDate),
            new("modified", command.ModifiedDate)
        };
        _repo.ExecuteReader<Models.RobotCommand>(
            @"INSERT INTO public.robotcommand (""Name"", description, ismovecommand, createddate, modifieddate) 
              VALUES (@name, @desc, @ismove, @created, @modified) RETURNING *;", sqlParams);
    }

    public void UpdateRobotCommand(int id, Models.RobotCommand command)
    {
        var sqlParams = new NpgsqlParameter[] {
            new("id", id),
            new("name", command.Name),
            new("desc", command.Description ?? (object)DBNull.Value),
            new("ismove", command.IsMoveCommand)
        };
        _repo.ExecuteReader<Models.RobotCommand>(
            @"UPDATE public.robotcommand SET ""Name""=@name, description=@desc, ismovecommand=@ismove, modifieddate=current_timestamp 
            WHERE id=@id RETURNING *;", sqlParams);
    }

    public void DeleteRobotCommand(int id)
    {
        var sqlParams = new NpgsqlParameter[] { new("id", id) };
        _repo.ExecuteReader<Models.RobotCommand>("DELETE FROM public.robotcommand WHERE id = @id RETURNING *;", sqlParams);
    }
}