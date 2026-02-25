using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using WPFClient.Localization;

namespace WPFClient
{
    
    public partial class App : Application
    {
        public static Context Context { get; } = Context.Instance;

        public App()
        {
            InitializeComponent();
            Context.RestoreRelations();
        }

        public void ChangeLanguage(string language)
        {
            var culture = new CultureInfo(language);

            /*
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            var xamlLanguage = XmlLanguage.GetLanguage(
                culture.IetfLanguageTag
            );

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    xamlLanguage
                )
            );

            foreach (Window w in Current.Windows)
                w.Language = xamlLanguage;
            */

            TranslationSource.Instance.CurrentCulture = culture;
        }
    }
}
