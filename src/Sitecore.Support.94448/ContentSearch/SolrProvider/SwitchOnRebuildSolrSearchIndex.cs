﻿using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Maintenance;
using Sitecore.ContentSearch.Security;

namespace Sitecore.Support.ContentSearch.SolrProvider
{
    public class SwitchOnRebuildSolrSearchIndex : Sitecore.ContentSearch.SolrProvider.SwitchOnRebuildSolrSearchIndex
    {
        public SwitchOnRebuildSolrSearchIndex(string name, string core, string rebuildcore, IIndexPropertyStore propertyStore) : base(name, core, rebuildcore, propertyStore)
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

