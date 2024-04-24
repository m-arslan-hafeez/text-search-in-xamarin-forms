using System.Text;
using FullTextSearchXamarinForms.Services;
using pdftron.PDF;

namespace FullTextSearchXamarinForms.Droid.Services
{
    public class PdfTronTextExtractor : ITextExtractor
    {
        public string Extract(string path)
        {
            var builder = new StringBuilder();

            using (var doc = new PDFDoc(path))
            {
                doc.InitSecurityHandler();

                using (TextExtractor txt = new TextExtractor())
                {
                    PageIterator itr = doc.GetPageIterator();
                    for (; itr.HasNext(); itr.Next())
                    {
                        txt.Begin(itr.Current());
                        builder.AppendLine(txt.GetAsText());
                    }
                }
            }
            return builder.ToString();
        }
    }
}