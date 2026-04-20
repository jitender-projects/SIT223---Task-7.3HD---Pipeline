using Microsoft.AspNetCore.Mvc;
using robot_controller_api;
using robot_controller_api.Models;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/robot-commands")]
public class RobotCommandsController : ControllerBase
{
    private readonly IRobotCommandDataAccess _robotCommandsRepo;

    public RobotCommandsController(IRobotCommandDataAccess robotCommandsRepo)
    {
        _robotCommandsRepo = robotCommandsRepo;
    }

    [HttpGet()]
    public IEnumerable<RobotCommand> GetAllRobotCommands()
    {
        return _robotCommandsRepo.GetRobotCommands();
    }

    [HttpGet("move")]
    public IEnumerable<RobotCommand> GetMoveCommands()
    {
        return _robotCommandsRepo.GetRobotCommands().Where(c => c.IsMoveCommand);
    }

    [HttpGet("{id}", Name = "GetRobotCommand")]
    public IActionResult GetRobotCommandById(int id)
    {
        var command = _robotCommandsRepo.GetRobotCommands().FirstOrDefault(c => c.Id == id);
        if(command == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(command);
        }
    }

    [HttpPost()]
    public IActionResult AddRobotCommand(RobotCommand newCommand)
    {
        if (newCommand == null)
        {
            return BadRequest();
        }
        if(_robotCommandsRepo.GetRobotCommands().Any(c => c.Name.Equals(newCommand.Name, StringComparison.OrdinalIgnoreCase)))
        {
            return Conflict();
        }

        newCommand.Id = _robotCommandsRepo.GetRobotCommands().Max(c => c.Id) + 1;
        newCommand.CreatedDate = DateTime.Now;
        newCommand.ModifiedDate = DateTime.Now;

        _robotCommandsRepo.InsertRobotCommand(newCommand);
        return CreatedAtRoute("GetRobotCommand", new {id = newCommand.Id}, newCommand);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateRobotCommand(int id, RobotCommand updatedCommand)
    {
        var command = _robotCommandsRepo.GetRobotCommands().FirstOrDefault(c => c.Id == id);
        if (command == null)
        {
            return NotFound();
        }
        command.Name = updatedCommand.Name;
        command.Description = updatedCommand.Description;
        command.IsMoveCommand = updatedCommand.IsMoveCommand;
        command.ModifiedDate = DateTime.Now;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRobotCommand(int id)
    {
        var command = _robotCommandsRepo.GetRobotCommands().FirstOrDefault(c => c.Id == id);
        if(command == null)
        {
            return NotFound();
        }
        _robotCommandsRepo.DeleteRobotCommand(id);
        return NoContent();
    }
}
