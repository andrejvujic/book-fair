using System;
using System.Globalization;
using System.Windows.Data;
using Core.Models;
using WPFClient.Localization;

namespace WPFClient.Localization
{
    public class LocalizedBookGenreConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length == 0 || values[0] == null)
                return string.Empty;

            if (values[0] is not BookGenre genre)
                return string.Empty;

            string key = genre switch
            {
                BookGenre.FANTASY => "Genre_Fantasy",
                BookGenre.SCIENCE_FICTION => "Genre_ScienceFiction",
                BookGenre.MYSTERY => "Genre_Mystery",
                BookGenre.THRILLER => "Genre_Thriller",
                BookGenre.ROMANCE => "Genre_Romance",
                BookGenre.HORROR => "Genre_Horror",
                BookGenre.COMEDY => "Genre_Comedy",
                BookGenre.NOVEL => "Genre_Novel",
                BookGenre.POETRY => "Genre_Poetry",
                BookGenre.DRAMA => "Genre_Drama",
                _ => "Genre_Unknown"
            };

            return TranslationSource.Instance[key];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}