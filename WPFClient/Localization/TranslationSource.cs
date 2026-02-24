using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Windows.Data;

namespace WPFClient.Localization
{
    class TranslationSource : INotifyPropertyChanged
    {
        private static readonly TranslationSource instance = new TranslationSource();
        public static TranslationSource Instance
        {
            get { return instance; }
        }

        private readonly ResourceManager resourceManager = Properties.Resources.ResourceManager;
        private CultureInfo? currentCulture = null;

        public string this[string key]
        {
            get => resourceManager.GetString(key, CurrentCulture) ?? string.Empty;
        }

        public CultureInfo? CurrentCulture
        {
            get { return currentCulture; }
            set
            {
                if (currentCulture != value)
                {
                    currentCulture = value;
                    var @event = PropertyChanged;
                    if (@event != null)
                    {
                        @event.Invoke(this, new PropertyChangedEventArgs(string.Empty));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class LocalizationExtension : Binding
    {
        public LocalizationExtension(string name)
            : base("[" + name + "]")
        {
            Mode = BindingMode.OneWay;
            Source = TranslationSource.Instance;
        }
    }
}
