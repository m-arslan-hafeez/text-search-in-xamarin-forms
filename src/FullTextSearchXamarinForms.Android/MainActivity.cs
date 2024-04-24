using Android.App;
using Android.Content.PM;
using Android.OS;
using FullTextSearchXamarinForms.Droid.Services;
using FullTextSearchXamarinForms.Services;
using pdftron;
using Splat;
using System.IO;

namespace FullTextSearchXamarinForms.Droid
{
    [Activity(Label = "FullTextSearchXamarinForms", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            CopyResToFiles();

            PDFNet.Initialize(this, Resource.Raw.pdfnet, "LICENSE_KEY");

            Locator.CurrentMutable.RegisterConstant<ITextExtractor>(new PdfTronTextExtractor());

            LoadApplication(new App());
        }

        private void CopyResToFiles()
        {
            var documentsPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "files");

            if (!Directory.Exists(documentsPath))
            {
                Directory.CreateDirectory(documentsPath);
            }

            using (var resource = this.Resources.OpenRawResource(Resource.Raw.sample))
            {
                using (var file = File.Open(Path.Combine(documentsPath, "sample.pdf"), FileMode.Create, FileAccess.ReadWrite))
                {
                    resource.CopyTo(file);
                    file.Close();
                }
                resource.Close();
            }
        }
    }
}