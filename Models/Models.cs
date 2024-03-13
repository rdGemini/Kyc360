using System;
using System.Collections.Generic;

public interface IEntity
{
    public List<Address>? Addresses { get; set; }
    public List<Date>? Dates { get; set; } 
    public bool Deceased { get; set; }
    public string Id { get; set; }
    public List<Name>? Names { get; set; }
    public string Gender { get; set; }
}

public class Entity : IEntity
{
    public List<Address>? Addresses { get; set; }
    public List<Date> Dates { get; set; }
    public bool Deceased { get; set; }
    public string Id { get; set; }
    public List<Name> Names { get; set; }
    public string Gender { get; set; }
}

public class Address
{
    public string? AddressLine { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}

public class Date
{
    public string? DateType { get; set; }
    public DateTime? DateValue { get; set; }
}

public class Name
{
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? Surname { get; set; }
}
