using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using WPFClient.DTO;

namespace WPFClient.Localization
{
    public class LocalizedBookSelectionTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string defaultTextKey = parameter as string ?? string.Empty;
            if (string.IsNullOrWhiteSpace(defaultTextKey))
                return string.Empty;

            var defaultText = TranslationSource.Instance[defaultTextKey];

            if (values == null || values.Length == 0)
                return defaultText;

            if (values[0] is BookDTO book && !string.IsNullOrWhiteSpace(book.Title))
                return book.Title;

            return defaultText;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}