using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Operations
{
    public abstract class DomainOperation<TEntity, TState, TResult>
      where TEntity : notnull
      where TState : class
    {
        public abstract TResult Transform(TEntity entity, TState? state);
    }
} 
