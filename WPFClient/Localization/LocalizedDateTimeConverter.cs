using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFClient.Localization
{
    public class LocalizedDateTimeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length == 0 || values[0] == null)
                return string.Empty;

            if (values[0] is DateTime dt)
            {
                var ci = (values.Length > 1 && values[1] is CultureInfo c && c != null)
                    ? c
                    : CultureInfo.CurrentCulture;

                return dt.ToString("d", ci);
            }

            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}