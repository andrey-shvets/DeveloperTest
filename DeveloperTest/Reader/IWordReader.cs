using System.Threading;
using System.Threading.Tasks;

namespace DeveloperTest.Reader
{
    public interface IWordReader
    {
        string GetNextWord();

        Task<string> GetNextWordAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
