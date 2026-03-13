using TicTacToeMiniGame.Runtime.Domain;

namespace TicTacToeMiniGame.Runtime.Contracts
{
    public sealed class TicTacToeMiniGameResult
    {
        public TicTacToeMiniGameResult(TicTacToeOutcome outcome, RewardData reward)
        {
            Outcome = outcome;
            Reward = reward;
        }

        public TicTacToeOutcome Outcome { get; }

        public RewardData Reward { get; }
    }
}
