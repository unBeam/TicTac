using System.Threading;
using Cysharp.Threading.Tasks;

namespace TicTacToeMiniGame.Runtime.Contracts
{
    public interface ITicTacToeMiniGameRunner
    {
        UniTask<TicTacToeMiniGameResult> RunAsync(TicTacToeMiniGameRequest request, CancellationToken cancellationToken);
    }
}
