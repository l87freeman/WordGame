namespace WordGame.Game.Domain.Models
{
    public class Game
    {
        private bool isBotIncluded = false;

        public void AddOrRemoveBot()
        {
            this.isBotIncluded = !this.isBotIncluded;
        }
    }
}