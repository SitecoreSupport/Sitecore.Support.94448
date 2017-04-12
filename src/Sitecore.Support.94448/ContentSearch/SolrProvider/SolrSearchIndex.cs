using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Maintenance;
using Sitecore.ContentSearch.Security;

namespace Sitecore.Support.ContentSearch.SolrProvider
{
    public class SolrSearchIndex : Sitecore.ContentSearch.SolrProvider.SolrSearchIndex
    {
        public SolrSearchIndex(string name, string core, IIndexPropertyStore propertyStore) : base(name, core, propertyStore)
        {
        }

        public override IProviderSearchContext CreateSearchContext(SearchSecurityOptions options = SearchSecurityOptions.Default)
        {
            if (this.Group == IndexGroup.Experience)
            {
                return new SolrAnalyticsSearchContext(this, options);
            }
            return new Sitecore.Support.ContentSearch.SolrProvider.SolrSearchContext(this, options);
        }
    }
}

