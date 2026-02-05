using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
