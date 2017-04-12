using System;
using System.Collections.Generic;
using System.Reflection;
using Sitecore.ContentSearch.Linq.Common;
using Sitecore.ContentSearch.Linq.Solr;
using SolrNet;

namespace Sitecore.Support.ContentSearch.SolrProvider
{
    public class LinqToSolrIndex<TItem> : Sitecore.ContentSearch.SolrProvider.LinqToSolrIndex<TItem>
    {
        private static readonly MethodInfo applyScalarMethods;
        private static FieldInfo queryMapperFieldInfo;

        static LinqToSolrIndex()
        {
            Sitecore.Support.ContentSearch.SolrProvider.LinqToSolrIndex<TItem>.queryMapperFieldInfo = typeof(SolrIndex<TItem>).GetField("queryMapper", BindingFlags.NonPublic | BindingFlags.Instance);
            Sitecore.Support.ContentSearch.SolrProvider.LinqToSolrIndex<TItem>.applyScalarMethods = typeof(Sitecore.ContentSearch.SolrProvider.LinqToSolrIndex<TItem>).GetMethod("ApplyScalarMethods", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public LinqToSolrIndex(Sitecore.ContentSearch.SolrProvider.SolrSearchContext context, IExecutionContext executionContext) : this(context, new[] { executionContext })
        {
            
        }

        public LinqToSolrIndex(Sitecore.ContentSearch.SolrProvider.SolrSearchContext context, IExecutionContext[] executionContexts) : base(context, executionContexts)
        {
            Sitecore.Support.ContentSearch.SolrProvider.LinqToSolrIndex<TItem>.queryMapperFieldInfo.SetValue(this, new Sitecore.Support.ContentSearch.Linq.Solr.SolrQueryMapper(base.Parameters));
        }

        protected object ApplyScalarMethods<TResult, TDocument>(SolrCompositeQuery compositeQuery, object processedResults, SolrQueryResults<Dictionary<string, object>> results)
        {
            return Sitecore.Support.ContentSearch.SolrProvider.LinqToSolrIndex<TItem>.applyScalarMethods.MakeGenericMethod(new Type[] { typeof(TResult), typeof(TDocument) }).Invoke(this, new object[] { compositeQuery, processedResults, results });
        }
    }
}

