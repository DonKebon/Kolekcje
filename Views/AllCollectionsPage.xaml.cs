using Kolekcje.Models;
using System.Diagnostics;

namespace Kolekcje.Views;

public partial class AllCollectionsPage : ContentPage
{
	public AllCollectionsPage()
	{
        InitializeComponent();

        BindingContext = new AllCollections();
    }
    protected override void OnAppearing()
    {
        Debug.WriteLine(FileSystem.AppDataDirectory);
        ((AllCollections)BindingContext).LoadCollections();
    }

    private async void AddCollection_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is AllCollections _collection)
        {
            if (!String.IsNullOrEmpty(_collection.CollectionName))
            {
                string appDataPath = FileSystem.AppDataDirectory;

                string fileName = $"{_collection.CollectionName}_collection.txt";
                //Shell.Current.DisplayAlert("Alert", _collection.CollectionName, "ok");

                File.WriteAllText(Path.Combine(appDataPath, fileName), _collection.CollectionName);
                await Shell.Current.GoToAsync($"{nameof(CollectionPage)}?{nameof(CollectionPage.ItemName)}={_collection.CollectionName}");
            }
        }
    }
    private async void CollectionChoice(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            var _collection = (SingleCollection)e.CurrentSelection[0];
            await Shell.Current.GoToAsync($"{nameof(CollectionPage)}?{nameof(CollectionPage.ItemName)}={_collection.CollectionName}");
            collectionViewCollections.SelectedItem = null;
        }
    }
}