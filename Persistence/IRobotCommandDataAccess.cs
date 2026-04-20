using robot_controller_api;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

public interface IRobotCommandDataAccess
{
    List<Models.RobotCommand> GetRobotCommands();
    void InsertRobotCommand(Models.RobotCommand command);
    void UpdateRobotCommand(int id, Models.RobotCommand command);
    void DeleteRobotCommand(int id);
}