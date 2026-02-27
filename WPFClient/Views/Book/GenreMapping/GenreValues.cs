using Core.Models;
using System.Collections.Generic;
using WPFClient.Localization;

namespace WPFClient.Views.Book.GenreMapping
{
	public static class GenreValues
	{
		public static List<GenreOption> All
		{
			get => new List<GenreOption>
				{

				new GenreOption
				{
					Value = BookGenre.FANTASY,
					Label = TranslationSource.Instance["Genre_Fantasy"],
				},
				new GenreOption
				{
					Value = BookGenre.SCIENCE_FICTION,
					Label = TranslationSource.Instance["Genre_ScienceFiction"],
				},
				new GenreOption
				{
					Value = BookGenre.MYSTERY,
					Label = TranslationSource.Instance["Genre_Mystery"],
				},
				new GenreOption
				{
					Value = BookGenre.THRILLER,
					Label = TranslationSource.Instance["Genre_Thriller"],
				},
				new GenreOption
				{
					Value = BookGenre.ROMANCE,
					Label = TranslationSource.Instance["Genre_Romance"],
				},
				new GenreOption
				{
					Value = BookGenre.HORROR,
					Label = TranslationSource.Instance["Genre_Horror"],
				},
				new GenreOption
				{
					Value = BookGenre.COMEDY,
					Label = TranslationSource.Instance["Genre_Comedy"],
				},
				new GenreOption
				{
					Value = BookGenre.NOVEL,
					Label = TranslationSource.Instance["Genre_Novel"],
				},
				new GenreOption
				{
					Value = BookGenre.POETRY,
					Label = TranslationSource.Instance["Genre_Poetry"],
				},
				new GenreOption
				{
					Value = BookGenre.DRAMA,
					Label = TranslationSource.Instance["Genre_Drama"],
				}
			};
		}
	}
}