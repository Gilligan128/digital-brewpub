using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Digital.BrewPub.Infrastructure.Transactions
{
    public class TransactionalActionFilter<TContext> : IActionFilter where TContext : DbContext
    {
        TContext dbContext;

        public TransactionalActionFilter(TContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

            try
            {
                if (context.Exception != null && !context.ExceptionHandled )
                    return;

                if (!context.ModelState.IsValid)
                    return;

                dbContext.Database.CommitTransaction();
            }
            finally
            {
                if(dbContext.Database.CurrentTransaction != null)
                    dbContext.Database.CurrentTransaction.Dispose();
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            dbContext.Database.BeginTransaction();
        }
    }
}
