using FullTextSearchXamarinForms.Services;
using Splat;
using System;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace FullTextSearchXamarinForms
{
    public partial class MainPage : ContentPage
    {
        ISearchService _searchService;

        public MainPage()
        {
            InitializeComponent();
            _searchService = Locator.Current.GetService<ISearchService>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SearchButton.Clicked += SearchButton_Clicked;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            SearchButton.Clicked -= SearchButton_Clicked;
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            var results = _searchService.Search(SearchInput.Text);
            foreach (var result in results)
            {
                sb.AppendLine(result);
            }

            Results.Text = sb.ToString();
        }
    }
}
