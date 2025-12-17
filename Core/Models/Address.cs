namespace Core.Models;

public class Address
{
    public int Id { get; set; }
    public string Street { get; set; }
    public string StreetNumber { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

    public Address() { }

    public Address(int id, string street, string streetNumber, string city, string country)
    {
        Id = id;
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
}