using Kolekcje.Models;
using System.Collections;
using System.Collections.ObjectModel;

namespace Kolekcje.Views;

[QueryProperty(nameof(ItemName), nameof(ItemName))]
public partial class CollectionPage : ContentPage
{
    public CollectionPage()
    {
        InitializeComponent();

        BindingContext = new Item();
        collectionViewItems.ItemsSource = Items;

    }
    private ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
    
    private string itemName;
    public string ItemName
    {
        get
        {
            return itemName;
        }
        set
        {
            if (value != null)
            {
                itemName = value;
                LoadCollection(value);
            }
        }
    }
    private void LoadCollection(string itemName)
    {
        string appDataPath = FileSystem.AppDataDirectory;
        string itemsFromFile = File.ReadAllText(Path.Combine(appDataPath, $"{itemName}_collection.txt"));
        string[] separatedItems = itemsFromFile.Split("\r\n");
        separatedItems = separatedItems.Skip(1).ToArray();

        foreach (string ItemLine in separatedItems)
        {
            string[] product = ItemLine.Split(';');

            Item readyItem = new Item()
            {
                ItemName = product[0]
            };

            Items.Add(readyItem);
        };
    }
    private async void ItemEdit(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            var _item = (Item)e.CurrentSelection[0];
            string _itemName = _item.ItemName;
            collectionViewItems.SelectedItem = null;

            string NewItemName = await DisplayPromptAsync($"Edytujesz przedmiot: {_itemName}", "", initialValue: $"{_itemName}");

            if (!string.IsNullOrEmpty(NewItemName))
            {
                _item.ItemName = NewItemName;
                string appDataPath = FileSystem.AppDataDirectory;
                string itemsFromFile = File.ReadAllText(Path.Combine(appDataPath, $"{itemName}_collection.txt"));
                string[] separatedItems = itemsFromFile.Split("\r\n");
                separatedItems = separatedItems.Skip(1).ToArray();


                string newWholeCollection = $"{this.ItemName}";
                foreach (string ItemLine in separatedItems)
                {
                    string[] things = ItemLine.Split(';');

                    if (things[0] != _itemName)
                    {
                        newWholeCollection += $"\r\n{things[0]}";
                    }
                    else if (things[0] == _itemName)
                    {
                        newWholeCollection += $"\r\n{NewItemName}";
                    }
                }

                File.Delete(Path.Combine(FileSystem.AppDataDirectory + $"{this.ItemName}_collection.txt"));
                File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, $"{this.ItemName}_collection.txt"), newWholeCollection);
            }
        }
    }
    private async void AddItem_Clicked(object sender, EventArgs e)
    {
        string fileName = $"{this.ItemName}_collection.txt";
        var context = ((Item)BindingContext);
        if (!String.IsNullOrEmpty(context.ItemName))
        {
            string itemsFromFile = File.ReadAllText(Path.Combine(FileSystem.AppDataDirectory, $"{itemName}_collection.txt"));
            string[] separatedItems = itemsFromFile.Split("\r\n");
            separatedItems = separatedItems.Skip(1).ToArray();


            bool itemWasBefore = false;
            foreach (string ItemLine in separatedItems)
            {
                if (context.ItemName == ItemLine)
                {
                    itemWasBefore = true;
                }
            }

            if (itemWasBefore)
            {
                bool sameItem = await DisplayAlert("UWAGA!", "Dodajesz do kolekcji przedmiot który ju¿ siê w niej znajduje, chcesz kontynuowaæ?", "Tak", "Nie");

                if (sameItem)
                {
                    File.AppendAllText(Path.Combine(FileSystem.AppDataDirectory, fileName), $"\r\n{context.ItemName}");

                    Item newItem = new Item()
                    {
                        ItemName = context.ItemName
                    };
                    Items.Add(newItem);
                }
            }
            else
            {
                File.AppendAllText(Path.Combine(FileSystem.AppDataDirectory, fileName), $"\r\n{context.ItemName}");

                Item newItem = new Item()
                {
                    ItemName = context.ItemName
                };
                Items.Add(newItem);
            }
        }
    }

    private async void CollectionDelete_Clicked(object sender, EventArgs e)
    {
        File.Delete(FileSystem.AppDataDirectory + $"/{this.ItemName}_collection.txt");
        await Shell.Current.GoToAsync("..");
    }

    private void DeleteItem_Clicked(Object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = (Item)button.BindingContext;

        string appDataPath = FileSystem.AppDataDirectory;
        string itemsFromFIle = File.ReadAllText(Path.Combine(appDataPath, $"{this.ItemName}_collection.txt"));
        string[] separatedItems = itemsFromFIle.Split("\r\n");
        separatedItems = separatedItems.Skip(1).ToArray();


        string newWholeCollection = $"{this.ItemName}";
        foreach (string ItemLine in separatedItems)
        {
            string[] things = ItemLine.Split(';');

            if (things[0] != item.ItemName)
            {
                newWholeCollection += $"\r\n{things[0]}";
            }
            else continue;
        }

        File.Delete(Path.Combine(FileSystem.AppDataDirectory + $"{this.ItemName}_collection.txt"));
        File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, $"{this.ItemName}_collection.txt"), newWholeCollection);
        Items.Clear();
        LoadCollection(this.ItemName);
    }
}