using System;
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
    public class SolrSearchContext : Sitecore.ContentSearch.SolrProvider.SolrSearchContext, IProviderSearchContext, IDisposable
    {
        public SolrSearchContext(Sitecore.ContentSearch.SolrProvider.SolrSearchIndex index, SearchSecurityOptions options = SearchSecurityOptions.Default) : base(index, options)
        {
        }

        IQueryable<TItem> IProviderSearchContext.GetQueryable<TItem>()
        {
            return this.GetQueryable<TItem>(new IExecutionContext[0]);
        }

        IQueryable<TItem> IProviderSearchContext.GetQueryable<TItem>(IExecutionContext executionContext)
        {
            return this.GetQueryable<TItem>(new[] { executionContext });
        }

        public override IQueryable<TItem> GetQueryable<TItem>(params IExecutionContext[] executionContexts)
        {
            var linqIndex = new Sitecore.Support.ContentSearch.SolrProvider.LinqToSolrIndex<TItem>(this, executionContexts);

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

