namespace TicTacToeMiniGame.Runtime.Contracts
{
    public readonly struct RewardData
    {
        public RewardData(int amount, RewardRarity rarity)
        {
            Amount = amount;
            Rarity = rarity;
        }

        public int Amount { get; }

        public RewardRarity Rarity { get; }
    }
}
