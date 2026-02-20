using Core.Models;
using System.Collections.Generic;

namespace WPFClient.Views.Book.GenreMapping
{
    public static class GenreValues
    {
        public static List<GenreOption> All { get; } =
            new List<GenreOption>
            {
                new GenreOption
                {
                    Value = BookGenre.FANTASY,
                    Label = "Fantazija"
                },
                new GenreOption
                {
                    Value = BookGenre.SCIENCE_FICTION,
                    Label = "Naučna fantastika"
                },
                new GenreOption
                {
                    Value = BookGenre.MYSTERY,
                    Label = "Misterija"
                },
                new GenreOption
                {
                    Value = BookGenre.THRILLER,
                    Label = "Triler"
                },
                new GenreOption
                {
                    Value = BookGenre.ROMANCE,
                    Label = "Romansa"
                },
                new GenreOption
                {
                    Value = BookGenre.HORROR,
                    Label = "Horor"
                },
                new GenreOption
                {
                    Value = BookGenre.COMEDY,
                    Label = "Komedija"
                },
                new GenreOption
                {
                    Value = BookGenre.NOVEL,
                    Label = "Roman"
                },
                new GenreOption
                {
                    Value = BookGenre.POETRY,
                    Label = "Poezija"
                },
                new GenreOption
                {
                    Value = BookGenre.DRAMA,
                    Label = "Drama"
                }
            };
    }
}