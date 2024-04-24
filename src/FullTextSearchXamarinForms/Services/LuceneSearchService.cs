using System;
using System.Collections.Generic;
using System.IO;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Search;
using Lucene.Net.Documents;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;
using Field = Lucene.Net.Documents.Field;
using Splat;
using Lucene.Net.QueryParsers.Classic;

namespace FullTextSearchXamarinForms.Services
{
    public class LuceneSearchService : ISearchService
    {
        ITextExtractor _pdfTextExtractor;
        private readonly string _indexPath;
        private readonly string _documentsPath;

        public LuceneSearchService(ITextExtractor pdfTextExtractor)
        {
            _pdfTextExtractor = pdfTextExtractor ?? Locator.Current.GetService<ITextExtractor>();

            _indexPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "index");
            _documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "files");
        }

        public void BuildIndex()
        {
            var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
            using (var indexDirectory = FSDirectory.Open(_indexPath))
            {
                var iwc = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer)
                {
                    OpenMode = System.IO.Directory.Exists(_indexPath) ? OpenMode.CREATE_OR_APPEND : OpenMode.CREATE
                };

                using (var writer = new IndexWriter(indexDirectory, iwc))
                {
                    var pdfFiles = System.IO.Directory.GetFiles(_documentsPath, "*.pdf");
                    foreach (var pdfFile in pdfFiles)
                    {
                        var resultText = _pdfTextExtractor.Extract(pdfFile);

                        var document = new Document
                        {
                            new StringField("Path", pdfFile, Field.Store.YES),
                            new TextField("Content", resultText, Field.Store.YES)
                        };

                        if (writer.Config.OpenMode == OpenMode.CREATE)
                        {
                            writer.AddDocument(document);
                        }
                        else
                        {
                            writer.UpdateDocument(new Term("Path", pdfFile), document);
                        }
                    }

                    writer.Commit();
                }
            }
        }

        public List<string> Search(string text)
        {
            text = text ?? string.Empty;

            var results = new List<string>();

            var analyzer = new StandardAnalyzer(LuceneVersion.LUCENE_48);
            var parser = new QueryParser(LuceneVersion.LUCENE_48, "Content", analyzer);
            var query = parser.Parse(text);

            using (var indexDirectory = FSDirectory.Open(_indexPath))
            {
                using (var indexReader = DirectoryReader.Open(indexDirectory))
                {
                    var searcher = new IndexSearcher(indexReader);
                    var hits = searcher.Search(query, 100);

                    foreach (var scoreDoc in hits.ScoreDocs)
                    {
                        var document = searcher.Doc(scoreDoc.Doc);
                        results.Add(document.Get("Path"));
                    }
                }
            }
            return results;
        }
    }
}