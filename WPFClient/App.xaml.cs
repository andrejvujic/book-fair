using System.Windows;

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
    }
}
