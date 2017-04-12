using System.Globalization;
using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Diagnostics;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Pipelines;
using Sitecore.ContentSearch.Pipelines.QueryGlobalFilters;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.Security;
using Sitecore.ContentSearch.Utilities;

namespace Sitecore.Support.ContentSearch.SolrProvider
{
    public class SolrAnalyticsSearchContext : Sitecore.ContentSearch.SolrProvider.SolrAnalyticsSearchContext
    {
        public SolrAnalyticsSearchContext(Sitecore.ContentSearch.SolrProvider.SolrSearchIndex index, SearchSecurityOptions options = SearchSecurityOptions.Default) : base(index, options)
        {
        }

        public override IQueryable<TItem> GetQueryable<TItem>(params IExecutionContext[] executionContexts)
        {
            var defaultCulture = CultureInfo.GetCultureInfo(ContentSearchManager.SearchConfiguration.AnalyticsDefaultLanguage);
            var updatedContexts = executionContexts.Where(x => !(x is CultureExecutionContext)).ToList();
            updatedContexts.Add(new CultureExecutionContext(defaultCulture));

            var linqIndex = new Sitecore.Support.ContentSearch.SolrProvider.LinqToSolrIndex<TItem>(this, updatedContexts.ToArray());

            if (ContentSearchConfigurationSettings.EnableSearchDebug)
            {
                (linqIndex as IHasTraceWriter).TraceWriter = new LoggingTraceWriter(SearchLog.Log);
            }

            var queryable = linqIndex.GetQueryable();

            if (typeof(TItem).IsAssignableFrom(typeof(SearchResultItem)))
            {
                var args = new QueryGlobalFiltersArgs(linqIndex.GetQueryable(), typeof(TItem), executionContexts.ToList());
                this.Index.Locator.GetInstance<Sitecore.Abstractions.ICorePipeline>().Run(PipelineNames.QueryGlobalFilters, args);
                queryable = (IQueryable<TItem>)args.Query;
            }

            return queryable;
        }
    }
}