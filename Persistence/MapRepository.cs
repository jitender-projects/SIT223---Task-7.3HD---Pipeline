using Npgsql;
using System;
using System.Collections.Generic;
using robot_controller_api;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

public class MapRepository : IMapDataAccess, IRepository
{
    // This allows us to access the default ExecuteReader implementation from the IRepository interface
    private IRepository _repo => this;

    public List<Map> GetMaps()
    {
        return _repo.ExecuteReader<Map>("SELECT * FROM public.map");
    }

    public void InsertMap(Map map)
    {
        var sqlParams = new NpgsqlParameter[] {
            new("name", map.Name),
            new("cols", map.Columns),
            new("rows", map.Rows),
            new("created", map.CreatedDate),
            new("modified", map.ModifiedDate)
        };
        
        // Using RETURNING * to leverage PostgreSQL's capability to return the inserted entity
        _repo.ExecuteReader<Map>(
            @"INSERT INTO public.map (""Name"", columns, rows, createddate, modifieddate) 
              VALUES (@name, @cols, @rows, @created, @modified) RETURNING *;", sqlParams);
    }

    public void UpdateMap(int id, Map map)
    {
        var sqlParams = new NpgsqlParameter[] {
            new("id", id),
            new("name", map.Name),
            new("cols", map.Columns),
            new("rows", map.Rows)
        };
        
        // Using current_timestamp for the modified date directly in the SQL query
        _repo.ExecuteReader<Map>(
            @"UPDATE public.map 
              SET ""Name""=@name, columns=@cols, rows=@rows, modifieddate=current_timestamp 
              WHERE id=@id RETURNING *;", sqlParams);
    }

    public void DeleteMap(int id)
    {
        var sqlParams = new NpgsqlParameter[] { new("id", id) };
        
        _repo.ExecuteReader<Map>("DELETE FROM public.map WHERE id = @id RETURNING *;", sqlParams);
    }
}