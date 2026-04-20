using System;
using System.Collections.Generic;
using System.Linq;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

public class RobotCommandEF : IRobotCommandDataAccess
{
    private readonly RobotContext _context;

    public RobotCommandEF(RobotContext context)
    {
        _context = context;
    }

    public List<RobotCommand> GetRobotCommands()
    {
        return _context.RobotCommands.ToList();
    }

    public void InsertRobotCommand(RobotCommand command)
    {
        command.Id = 0;
        _context.RobotCommands.Add(command);
        _context.SaveChanges();
    }

    public void UpdateRobotCommand(int id, RobotCommand command)
    {
        var existingCommand = _context.RobotCommands.Find(id);
        
        if (existingCommand != null)
        {
            existingCommand.Name = command.Name;
            existingCommand.Description = command.Description;
            existingCommand.IsMoveCommand = command.IsMoveCommand;
            existingCommand.ModifiedDate = DateTime.Now;
            
            _context.SaveChanges();
        }
    }

    public void DeleteRobotCommand(int id)
    {
        var existingCommand = _context.RobotCommands.Find(id);
        if (existingCommand != null)
        {
            _context.RobotCommands.Remove(existingCommand);
            _context.SaveChanges();
        }
    }
}