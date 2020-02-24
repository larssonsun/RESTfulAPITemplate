
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Larsson.RESTfulAPIHelper.Pagination
{
    public static class ControllerExtentions
    {
        public static void SetPaginationHead<TPaged, TQueryParams>(this HttpResponse response, PaginatedList<TPaged> pagedEntity,
            TQueryParams queryParams,
            Dictionary<string, object> filterProps,
            Func<object, string> getUriByAction, Func<object, string> jsonSerialize)
            where TPaged : class
            where TQueryParams : PaginationBase
        {
            var previousPageLink = pagedEntity.HasPrevious ?
                getUriByAction(CreateProductsUri(queryParams, PaginationResourceUriType.PreviousPage, filterProps)) : null;

            var nextPageLink = pagedEntity.HasNext ?
                getUriByAction(CreateProductsUri(queryParams, PaginationResourceUriType.NextPage, filterProps)) : null;

            var meta = new
            {
                pagedEntity.TotalItemsCount,
                pagedEntity.PaginationBase.PageSize,
                pagedEntity.PaginationBase.PageIndex,
                pagedEntity.PageCount,
                previousPageLink,
                nextPageLink
            };

            response.Headers.Add("X-Pagination", jsonSerialize(meta));
        }
        private static dynamic CreateProductsUri<T>(T parameters, PaginationResourceUriType uriType, Dictionary<string, object> filterProps)
            where T : PaginationBase
        {
            dynamic paginationParms = new System.Dynamic.ExpandoObject();

            paginationParms.pageIndex = parameters.PageIndex + (int)uriType;
            paginationParms.pageSize = parameters.PageSize;
            paginationParms.orderBy = parameters.OrderBy;
            paginationParms.fields = parameters.Fields;

            var dict = (paginationParms as IDictionary<string, object>);

            filterProps.Any(x =>
            {
                dict[x.Key] = x.Value;
                return false;
            });

            return paginationParms;
        }
    }
}