using System.Threading.Tasks;

namespace Digital.BrewPub.Features.Brewery
{
    public interface HandleQuery<TQuery, TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}