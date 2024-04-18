namespace Kolekcje
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.CollectionPage), typeof(Views.CollectionPage));
        }
    }
}
