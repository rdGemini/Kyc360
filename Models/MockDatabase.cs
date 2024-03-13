using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

public class MockDatabase
{
    private readonly List<IEntity> _entities;

    public MockDatabase()
    {
        _entities = new List<IEntity>();
    }

    public List<IEntity> GetEntities()
    {
        return _entities;
    }

    public IEntity GetEntityById(string id)
    {
        return _entities.FirstOrDefault(e => e.Id == id);
    }

    public void AddEntity(Entity entity)
    {
        _entities.Add(entity);
    }

    public void UpdateEntity(Entity entity)
    {
        var existingEntity = _entities.FirstOrDefault(e => e.Id == entity.Id);
        if (existingEntity != null)
        {
            _entities.Remove(existingEntity);
            _entities.Add(entity);
        }
    }

    public void DeleteEntity(string id)
    {
        var entity = _entities.FirstOrDefault(e => e.Id == id);
        if (entity != null)
        {
            _entities.Remove(entity);
        }
    }

    public List<IEntity> SearchEntities(string searchText)
    {
        return _entities
            .Where(e =>
                e.Names.Any(n =>
                    (n.FirstName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (n.MiddleName?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (n.Surname?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false)
                ) ||
                e.Addresses.Any(a =>
                    (a.AddressLine?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (a.City?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (a.Country?.Contains(searchText, StringComparison.OrdinalIgnoreCase) ?? false)
                )
            )
            .ToList();
    }

    public List<IEntity> FilterEntities(string gender, DateTime? startDate, DateTime? endDate, string[] countries)
    {
        return _entities
            .Where(e =>
                (string.IsNullOrEmpty(gender) || e.Gender == gender) &&
                (!startDate.HasValue || e.Dates.Any(d => d.DateType == "Birth" && d.DateValue >= startDate.Value)) &&
                (!endDate.HasValue || e.Dates.Any(d => d.DateType == "Birth" && d.DateValue <= endDate.Value)) &&
                (countries == null || countries.Length == 0 || e.Addresses.Any(a => countries.Contains(a.Country)))
            )
            .ToList();
    }
}