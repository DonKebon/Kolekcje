using System.ComponentModel;

namespace Kolekcje.Models
{
    internal class Item : INotifyPropertyChanged
    {

        private string itemName;
        public string ItemName
        {
            get => itemName;
            set
            {
                if (itemName != value)
                {
                    itemName = value;
                    OnPropertyChanged(nameof(ItemName));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
