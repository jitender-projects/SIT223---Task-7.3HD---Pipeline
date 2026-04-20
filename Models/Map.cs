using System;
using System.Collections.Generic;

namespace robot_controller_api.Models;

public partial class Map
{
    public Map(){}

    public Map(int id, int columns, int rows, string name, DateTime createdDate, DateTime modifiedDate, string? description = null)
    {
        Id = id;
        Columns = columns;
        Rows = rows;
        Name = name;
        Description = description;
        CreatedDate = createdDate;
        ModifiedDate = modifiedDate;
    }
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Columns { get; set; }

    public int Rows { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }
}
