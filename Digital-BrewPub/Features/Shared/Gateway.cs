using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital.BrewPub.Features.Shared
{
    public interface Gateway<in TRequest, TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request);
    }
}
