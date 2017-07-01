using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using PhonebookApi.Models;

namespace PhonebookApi.ActionFilters
{
    public class RangeFilter : ActionFilterAttribute
    {
        private int _min;
        private int _max;

        public RangeFilter(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var filter = actionContext.ActionArguments["filter"] as Filter;
            if (filter == null)
                throw new NullReferenceException();
            if (filter.Limit > _max)
                filter.Limit = _max;
            else if (filter.Limit < _min)
                filter.Limit = _min;

            base.OnActionExecuting(actionContext);
        }
    }
}