using System.Collections.Generic;

namespace FullTextSearchXamarinForms.Services
{
    public interface ISearchService
    {
        void BuildIndex();

        List<string> Search(string text);
    }
}
