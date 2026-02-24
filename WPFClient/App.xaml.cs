using System.Windows;
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
            TranslationSource.Instance.CurrentCulture = new System.Globalization.CultureInfo(language);
        }
    }
}
