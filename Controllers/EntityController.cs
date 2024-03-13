using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

[Route("api/entities")]
[ApiController]
public class EntityController : ControllerBase
{
    private readonly MockDatabase _mockDatabase;

    public EntityController(MockDatabase mockDatabase)
    {
        _mockDatabase = mockDatabase;
    }

   [HttpGet]
    public IActionResult GetEntities(int page = 1, int pageSize = 10, string sortBy = "Id", bool ascending = true)
    {
        var entities = _mockDatabase.GetEntities().AsQueryable().OrderByProperty(sortBy, ascending);

        var totalItems = entities.Count();
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        entities = entities.Skip((page - 1) * pageSize).Take(pageSize);

        var result = new
        {
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
            TotalItems = totalItems,
            Items = entities.ToList()
        };

        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetEntityById(string id)
    {
        var entity = _mockDatabase.GetEntityById(id);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(entity);
    }

    [HttpPost]
    public IActionResult CreateEntity(Entity entity)
    {
        _mockDatabase.AddEntity(entity);
        return CreatedAtAction(nameof(GetEntityById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateEntity(string id, Entity entity)
    {
        if (id != entity.Id)
        {
            return BadRequest();
        }

        var existingEntity = _mockDatabase.GetEntityById(id);
        if (existingEntity == null)
        {
            return NotFound();
        }

        _mockDatabase.UpdateEntity(entity);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEntity(string id)
    {
        var entity = _mockDatabase.GetEntityById(id);
        if (entity == null)
        {
            return NotFound();
        }

        _mockDatabase.DeleteEntity(id);
        return NoContent();
    }

    [HttpGet("search")]
    public IActionResult SearchEntities([FromQuery] string search)
    {
        var entities = _mockDatabase.SearchEntities(search);
        return Ok(entities);
    }

    [HttpGet("filter")]
    public IActionResult FilterEntities([FromQuery] string gender, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string[] countries)
    {
        var entities = _mockDatabase.FilterEntities(gender, startDate, endDate, countries);
        return Ok(entities);
    }
}