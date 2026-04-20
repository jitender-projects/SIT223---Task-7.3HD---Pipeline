using System;
using System.Collections.Generic;
using System.Linq;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

// This class implements the interface, allowing it to be swapped in via Dependency Injection
public class MapEF : IMapDataAccess
{
    private readonly RobotContext _context;

    // Inject the EF DbContext into the repository
    public MapEF(RobotContext context)
    {
        _context = context;
    }

    public List<Map> GetMaps()
    {
        // EF automatically translates this into 'SELECT * FROM map'
        return _context.Maps.ToList(); 
    }

    public void InsertMap(Map map)
    {
        map.Id = 0;
        _context.Maps.Add(map);
        // SaveChanges is required in EF to actually execute the SQL transaction
        _context.SaveChanges(); 
    }

    public void UpdateMap(int id, Map map)
    {
        // .Find() efficiently searches the local tracked entities first, then the database
        var existingMap = _context.Maps.Find(id);
        
        if (existingMap != null)
        {
            // Update the properties
            existingMap.Name = map.Name;
            existingMap.Columns = map.Columns;
            existingMap.Rows = map.Rows;
            existingMap.Description = map.Description;
            existingMap.ModifiedDate = DateTime.Now;
            
            // Execute the update
            _context.SaveChanges();
        }
    }

    public void DeleteMap(int id)
    {
        var existingMap = _context.Maps.Find(id);
        if (existingMap != null)
        {
            _context.Maps.Remove(existingMap);
            _context.SaveChanges();
        }
    }
}