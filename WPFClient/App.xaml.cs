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
            TranslationSource.Instance.CurrentCulture = culture;
        }
    }
}
