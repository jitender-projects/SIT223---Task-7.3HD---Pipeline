using Microsoft.AspNetCore.Mvc; //Library for building web APIs
using robot_controller_api.Persistence; //Import our Persistence namespace to access our data
using robot_controller_api.Models;
using System.Runtime.CompilerServices; //Import our Models namespace to access our Map model

namespace robot_controller_api.Controllers; //Define the namespace for our controller

[ApiController] //This attribute enables API-specific behaviors like automatic model validation.
[Route("api/maps")] //This attribute defines the base route for all endpoints in this controller. All routes will start with "api/maps".

//Declaring a Controller class named MapsController that inherits from ControllerBase, which provides basic functionalities for handling HTTP requests and responses.
public class MapsController : ControllerBase
{
    // Declare a private, readonly field to hold our database repository interface. This will allow us to interact with our data store.
    private readonly IMapDataAccess _mapRepo;

    //When the API starts, ASP.NET automatically passes the correct Repository here.
    public MapsController (IMapDataAccess mapRepo)
    {
        _mapRepo = mapRepo;
    }

    //Maps to a GET request at the base route ("/api/maps).
    [HttpGet()]
    public IEnumerable<Map> GetAllMaps()
    {
        //Calls the repository to fetch all maps from the Postgre database and return them to the user.
        return _mapRepo.GetMaps();
    }

    //Maps to a GET request at "/api/maps/square.
    [HttpGet("square")]
    public IEnumerable<Map> GetSquareMapsOnly()
    {
        //Fetches all maps and uses a LINQ query to filter and return only those where Column equal Rows.
        return _mapRepo.GetMaps().Where(m => m.Columns == m.Rows);
    }

    //Maps to a GET request at "/api/maps/{id}". The 'Name' property allows us to reference this route later.
    [HttpGet("{id}", Name = "GetMap")]
    public IActionResult GetMapById(int id)
    {
        //It searches the database for the first map that matches the provided ID.
        var map = _mapRepo.GetMaps().FirstOrDefault(m => m.Id == id);
        //No map is found with that ID...
        if(map == null)
        {
            // return a 404 Not Found HTTP Status.
            return NotFound();
        }
        //If a map is found, return it with a 200 OK status.
        else
        {
            return Ok(map);
        }
    }

    //Maps to a POST request at the base route ("/api/maps") to create a new map.
    [HttpPost()]
    public IActionResult AddMap(Map newMap)
    {
        //Check if the user sent an empty payload
        if(newMap == null)
        {
            return BadRequest();
        }
        //Check if a map with this exact name already exists in the database. If exists then it returns a 409 Conflict status.
        if(_mapRepo.GetMaps().Any(m => m.Name.Equals(newMap.Name, StringComparison.OrdinalIgnoreCase))){
            return Conflict();
        }

        //generate a new ID by finding the current highest ID and adding 1. If the database is empty, start at 1.
        newMap.Id = _mapRepo.GetMaps().Any() ? _mapRepo.GetMaps().Max(m => m.Id) + 1 : 1;
        //Set the creation timestamp to the exact current time.
        newMap.CreatedDate = DateTime.Now;
        newMap.ModifiedDate = DateTime.Now;

        //Pass the fully constructed map object to the repository to insert it into the database.
        _mapRepo.InsertMap(newMap);
        return CreatedAtRoute("GetMap", new {id = newMap.Id}, newMap);
    }

    //Maps to a PUT request at "/api/maps/{id}" to completely update an existing map.
    [HttpPut("{id}")]
    public IActionResult UpdateMap(int id, Map updatedMap)
    {
        //Search the database to ensure the map the user wants to update actually exists.
        var map = _mapRepo.GetMaps().FirstOrDefault(m => m.Id == id);
        //If it doesn't exist, return a 404 Not Found status.
        if(map == null)
        {
            return NotFound();
        }

        //Update the properties of the retrieved map with the new data from the request body.
        map.Name = updatedMap.Name;
        map.Columns = updatedMap.Columns;
        map.Rows = updatedMap.Rows;
        map.Description = updatedMap.Description;
        map.ModifiedDate = DateTime.Now;

        _mapRepo.UpdateMap(id, map);
        return NoContent();
    }

    //Maps a DELETE request at "/api/maps/{id}" to remove a map.
    [HttpDelete("{id}")]
    public IActionResult DeleteMap(int id)
    {
        //First, we check if the map with the specified ID exists in the database.
        var map = _mapRepo.GetMaps().FirstOrDefault(m => m.Id == id);
        //If it doesn't exist, we return a 404 Not Found status to indicate that the resource cannot be deleted because it doesn't exist.
        if(map == null)
        {
            return NotFound();
        }
        //If the map exist then instruct the repository to delete it from the database.
        else
        {
            _mapRepo.DeleteMap(id);
        }
        return NoContent();
    }

    //Maps to a GET request at "/api/maps/{id}/{x}-{y}" to check if the provided coordinates are within the bounds of the specified map.
    [HttpGet("{id}/{x}-{y}")]
    public IActionResult CheckCoordinate (int id, int x, int y)
    {
        if(x < 0 || y < 0)
        {
            return BadRequest();
        }
        var map = _mapRepo.GetMaps().FirstOrDefault(m => m.Id == id);
        if(map == null)
        {
            return NotFound();
        }
        bool isOnMap = x < map.Columns && y < map.Rows;
        return Ok(isOnMap);
    }
}
