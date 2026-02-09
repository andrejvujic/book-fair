using Core.Helpers;
using Core.Serialization;
using System.ComponentModel;


namespace WPFClient.DTO
{
    public abstract class DTO<T> : INotifyPropertyChanged where T : class, ISerializable, IIdentifiable, new()
    {
        public DTO(T entity) {
            FromModel(entity);
        }

        public abstract DTO<T> Copy();

        protected abstract void FromModel(T entity);

        public abstract T ToModel();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
    }
}
