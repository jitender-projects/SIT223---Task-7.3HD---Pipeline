using robot_controller_api;
using System.Collections.Generic;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

public interface IMapDataAccess
{
    List<Models.Map> GetMaps();
    void InsertMap(Models.Map map);
    void UpdateMap(int id, Models.Map map);
    void DeleteMap(int id);
}