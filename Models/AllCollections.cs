using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kolekcje.Models
{
    internal class AllCollections
    {
        public ObservableCollection<SingleCollection> All_Collections { get; set; } = new ObservableCollection<SingleCollection>();
        public string CollectionName { get; set; }

        public AllCollections() => LoadCollections();
        public void LoadCollections()
        {
            All_Collections.Clear();
            string appDataPath = FileSystem.AppDataDirectory;

            IEnumerable<SingleCollection> allCollections = Directory
                .EnumerateFiles(appDataPath, $"*_collection.txt")
                .Select(collectionName => new SingleCollection()
                {
                    CollectionName = File.ReadLines(collectionName).First()
                });

            foreach (SingleCollection pingus in allCollections)
            {
                All_Collections.Add(pingus);
            }
        }
    }
}
