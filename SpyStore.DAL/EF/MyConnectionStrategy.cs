using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace SpyStore.DAL.EF
{
    public class MyConnectionStrategy : ExecutionStrategy
    {
        public MyConnectionStrategy(ExecutionStrategyDependencies context) :
            base(context, ExecutionStrategy.DefaultMaxRetryCount, ExecutionStrategy.DefaultMaxDelay)
        {

        }

        public MyConnectionStrategy(ExecutionStrategyDependencies context, int maxRetryCount, TimeSpan maxRetryDelay) :
            base(context, maxRetryCount, maxRetryDelay)
        {

        }

        protected override bool ShouldRetryOn(Exception exception)
        {
            return true;
        }
    }
}
