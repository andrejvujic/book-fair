// See https://aka.ms/new-console-template for more information
using Core.Models;
using Core.Helpers.Relations;

List<Author> GetTestAuthors1()
{
    List<Author> authors = new List<Author>();
    authors.Add(
        new Author(
            0, "Petar", "Petrović",
            new DateOnly(1985, 3, 12),
            new Address(), "+381651234567", "petar.petrovic@gmail.com",
            "123456", 20,
            new List<Book>()
        )
    );

    authors.Add(
        new Author(
            1, "Marko", "Marković",
            new DateOnly(1990, 7, 25),
            new Address(), "+381641112233", "marko.markovic@gmail.com",
            "654321", 15,
            new List<Book>()
        )
    );

    authors.Add(
        new Author(
            2, "Jovan", "Jovanović",
            new DateOnly(1978, 11, 5),
            new Address(), "+381631998877", "jovan.jovanovic@gmail.com",
            "abcdef", 30,
            new List<Book>()
        )
    );

    authors.Add(
        new Author(
            3, "Nikola", "Nikolić",
            new DateOnly(1988, 1, 19),
            new Address(), "+381621234999", "nikola.nikolic@gmail.com",
            "ghijk", 10,
            new List<Book>()
        )
    );

    authors.Add(
        new Author(
            4, "Milan", "Milić",
            new DateOnly(1995, 9, 2),
            new Address(), "+381601112244", "milan.milic@gmail.com",
            "qwerty", 5,
            new List<Book>()
        )
    );
    return authors;
}
List<Author> GetTestAuthors2()
{
    List<Author> authors = new List<Author>();

    authors.Add(
        new Author(
            5, "Stefan", "Stojanović",
            new DateOnly(1982, 6, 14),
            new Address(), "+381641234888", "stefan.stojanovic@gmail.com",
            "lmnopq", 18,
            new List<Book>()
        )
    );

    authors.Add(
        new Author(
            6, "Aleksandar", "Ilić",
            new DateOnly(1993, 4, 30),
            new Address(), "+381631223344", "aleksandar.ilic@gmail.com",
            "aleks93", 8,
            new List<Book>()
        )
    );

    authors.Add(
        new Author(
            7, "Dušan", "Pavlović",
            new DateOnly(1975, 12, 9),
            new Address(), "+381651998822", "dusan.pavlovic@gmail.com",
            "dusan75", 25,
            new List<Book>()
        )
    );

    return authors;
}

List<Author> GetAllAuthors()
{
    List<Author> authors = new List<Author>();
    authors.Add(
        new Author(
            0, "Petar", "Petrović",
            new DateOnly(1985, 3, 12),
            new Address(), "+381651234567", "petar.petrovic@gmail.com",
            "123456", 20,
            new List<Book>()
        )
    );

    authors.Add(
        new Author(
            1, "Marko", "Marković",
            new DateOnly(1990, 7, 25),
            new Address(), "+381641112233", "marko.markovic@gmail.com",
            "654321", 15,
            new List<Book>()
        )
    );

    authors.Add(
        new Author(
            2, "Jovan", "Jovanović",
            new DateOnly(1978, 11, 5),
            new Address(), "+381631998877", "jovan.jovanovic@gmail.com",
            "abcdef", 30,
            new List<Book>()
        )
    );

    authors.Add(
        new Author(
            3, "Nikola", "Nikolić",
            new DateOnly(1988, 1, 19),
            new Address(), "+381621234999", "nikola.nikolic@gmail.com",
            "ghijk", 10,
            new List<Book>()
        )
    );

    authors.Add(
        new Author(
            4, "Milan", "Milić",
            new DateOnly(1995, 9, 2),
            new Address(), "+381601112244", "milan.milic@gmail.com",
            "qwerty", 5,
            new List<Book>()
        )
    );

    authors.Add(
        new Author(
            5, "Stefan", "Stojanović",
            new DateOnly(1982, 6, 14),
            new Address(), "+381641234888", "stefan.stojanovic@gmail.com",
            "lmnopq", 18,
            new List<Book>()
        )
    );

    authors.Add(
        new Author(
            6, "Aleksandar", "Ilić",
            new DateOnly(1993, 4, 30),
            new Address(), "+381631223344", "aleksandar.ilic@gmail.com",
            "aleks93", 8,
            new List<Book>()
        )
    );

    authors.Add(
        new Author(
            7, "Dušan", "Pavlović",
            new DateOnly(1975, 12, 9),
            new Address(), "+381651998822", "dusan.pavlovic@gmail.com",
            "dusan75", 25,
            new List<Book>()
        )
    );

    return authors;
}


Publisher publisher1 = new Publisher(0, "Savo", "Oroz", new Author(), new List<Author>(), new List<Book>());
Publisher publisher2 = new Publisher(1, "Ana", "Jovic", new Author(), new List<Author>(), new List<Book>());

//Publisher publisher1 = new Publisher(0, "Savo", "Oroz", new Author(), GetTestAuthors1(), new List<Book>());
//Publisher publisher2 = new Publisher(1, "Ana", "Jovic", new Author(), GetTestAuthors2(), new List<Book>());

ManyToManyRelation<Publisher, Author> publisherAuthors = new ManyToManyRelation<Publisher, Author>("PublisherAuthors.txt");
publisherAuthors.Connect(publisher1, GetAllAuthors(), publisher1.Authors.Add);
publisherAuthors.Connect(publisher2, GetAllAuthors(), publisher2.Authors.Add);
//publisherAuthors.AddAll(publisher1, publisher1.Authors);
//publisherAuthors.AddAll(publisher2, publisher2.Authors);

Console.WriteLine($"Publisher number 1 has {publisher1.Authors.Count} authors");
Console.WriteLine($"Publisher number 2 has {publisher2.Authors.Count} authors");

// publisherAuthors.AddAll(publisher, publisher.Authors);
