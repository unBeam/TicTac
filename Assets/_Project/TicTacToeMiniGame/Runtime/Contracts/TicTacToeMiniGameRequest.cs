namespace TicTacToeMiniGame.Runtime.Contracts
{
    public sealed class TicTacToeMiniGameRequest
    {
        public TicTacToeMiniGameRequest(int rewardAmount, int seed)
        {
            RewardAmount = rewardAmount;
            Seed = seed;
        }

        public int RewardAmount { get; }

        public int Seed { get; }
    }
}
