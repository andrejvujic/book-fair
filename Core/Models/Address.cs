using Core.Helpers;
using Core.Serialization;
namespace Core.Models;


public class Address : ISerializable, IIdentifiable
{
    public int Id { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    public Address() { }

    public Address(string street, string streetNumber, string city, string country)
    {
        Street = street;
        StreetNumber = streetNumber;
        City = city;
        Country = country;
    }

    public override string ToString()
    {
        return
            "[Address]:" +
            $"\n\tId: {Id}" +
            $"\n\tStreet: {Street}" +
            $"\n\tStreetNumber: {StreetNumber}" +
            $"\n\tCity: {City}" +
            $"\n\tCountry: {Country}";
    }

    public string[] ToCSV()
    {
        string[] csv =
        {
            Id.ToString(),
            Street,
            StreetNumber,
            City,
            Country
        };
        return csv;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        Street = values[1];
        StreetNumber = values[2];
        City = values[3];
        Country = values[4];
    }
}