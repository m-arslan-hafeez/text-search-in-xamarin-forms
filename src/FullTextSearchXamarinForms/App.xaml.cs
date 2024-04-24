using FullTextSearchXamarinForms.Services;
using Splat;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FullTextSearchXamarinForms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var textExtractor = Locator.Current.GetService<ITextExtractor>();
            var luceneSearch = new LuceneSearchService(textExtractor);
            
            Locator.CurrentMutable.RegisterConstant<ISearchService>(luceneSearch);

            luceneSearch.BuildIndex();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
