using Foundation;
using FullTextSearchXamarinForms.iOS.Services;
using FullTextSearchXamarinForms.Services;
using pdftron;
using Splat;
using System.IO;
using UIKit;

namespace FullTextSearchXamarinForms.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            CopyResToFiles();

            PDFNet.Initialize("LICENSE_KEY");

            Locator.CurrentMutable.RegisterConstant<ITextExtractor>(new PdfTronTextExtractor());

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        private void CopyResToFiles()
        {
            var documentsPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "files");

            if (!Directory.Exists(documentsPath))
            {
                Directory.CreateDirectory(documentsPath);
            }

            var pathToResoure = NSBundle.MainBundle.PathForResource("sample", "pdf");

            using (var resource = File.Open(pathToResoure, FileMode.Open))
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
