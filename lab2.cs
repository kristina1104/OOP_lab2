using System;
using System.Collections.Generic;

namespace lab2
{
    public abstract class GameAccount 
    {
        public string UserName { get; set; }
        public int CurrentRating { get; set; }
        public int GamesCount { get; set; }
        private List<GameHistory> gamesHistory;

        public GameAccount(string userName, int initialRating)
        {
            UserName = userName;
            CurrentRating = initialRating;
            GamesCount = 0;
            gamesHistory = new List<GameHistory>();
        }

        public abstract void UpdateRating(Game game);

        public void WinGame(string opponentName, Game game)
        {
            int winningRating = game.GetWinningRating();
            CurrentRating = Math.Min(CurrentRating + winningRating, 10);
            GamesCount++;
            gamesHistory.Add(new GameHistory(opponentName, true, winningRating, GamesCount));
        }

        public void LoseGame(string opponentName, Game game)
        {
            int losingRating = game.GetLosingRating();
            CurrentRating = Math.Max(CurrentRating - losingRating, 1);
            GamesCount++;
            gamesHistory.Add(new GameHistory(opponentName, false, losingRating, GamesCount));
        }

        public string GetStats()
        {
            string result = $"Player: {UserName}, Rating: {CurrentRating}\n";
            result += $"{string.Format("{0, -10}", "GameIndex")}{string.Format("{0, -15}", "OpponentName")}{string.Format("{0, -10}", "Outcome")}{string.Format("{0, -10}", "Rating")}\n";

            if (gamesHistory != null)
            {
                foreach (var game in gamesHistory)
                {
                    string outcome = game.Won ? "Victory" : "Defeat";

                    result += $"{string.Format("{0, -10}", game.GameIndex)}{string.Format("{0, -15}", game.OpponentName)}{string.Format("{0, -10}", outcome)}{string.Format("{0, -10}", game.Rating)}\n";
                }
            }

            return result;
        }

        public class GameHistory
        {
            public string OpponentName { get; }
            public int Rating { get; }
            public int GameIndex { get; }
            public bool Won { get; }

            public GameHistory(string opponentName, bool won, int rating, int gameIndex)
            {
                OpponentName = opponentName;
                Won = won;
                Rating = rating;
                GameIndex = gameIndex;
            }
        }
    }

    public abstract class Game
    {
        public abstract int GetWinningRating();
        public abstract int GetLosingRating();
    }

    public class StandardGameAccount : GameAccount
    {
        public StandardGameAccount(string userName, int initialRating) : base(userName, initialRating)
        {
        }

        public override void UpdateRating(Game game)
        {
            
            CurrentRating += game.GetWinningRating(); // При виграші додаємо рейтинг гри
        }
    }

    public class ReducedPenaltyGameAccount : GameAccount
    {
        public ReducedPenaltyGameAccount(string userName, int initialRating) : base(userName, initialRating)
        {
        }

        public override void UpdateRating(Game game)
        {
            
            CurrentRating += game.GetWinningRating() / 2; // Зменшуємо штраф при виграші
        }
    }

    public class BonusPointsGameAccount : GameAccount
    {
        public BonusPointsGameAccount(string userName, int initialRating) : base(userName, initialRating)
        {
        }

        public override void UpdateRating(Game game)
        {
           
            CurrentRating += game.GetWinningRating() + (GamesCount % 5 == 0 ? 20 : 0); // Додаємо бонус за кожну 5-ту перемогу
        }
    }

    public class StandardGame : Game
    {
        public override int GetWinningRating()
        {
            
            return 100; // При виграші додаємо 100 балів
        }

        public override int GetLosingRating()
        {
           
            return 50; // При програші знімаємо 50 балів
        }
    }

    public class TrainingGame : Game
    {
        public override int GetWinningRating()
        {
           
            return 50; // При виграші додаємо 50 балів
        }

        public override int GetLosingRating()
        {
            
            return 25; // При програші знімаємо 25 балів
        }
    }

    public class SinglePlayerGame : Game
    {
        public override int GetWinningRating()
        {
           
            return 75; // При виграші додаємо 75 балів
        }

        public override int GetLosingRating()
        {
            
            return 30; // При програші знімаємо 30 балів
        }
    }

    public static class GameFactory
    {
        public static Game CreateStandardGame()
        {
            return new StandardGame();
        }

        public static Game CreateTrainingGame()
        {
            return new TrainingGame();
        }

        public static Game CreateSinglePlayerGame()
        {
            return new SinglePlayerGame();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            // Демонстрація роботи з об’єктами класів
            GameAccount player1 = new StandardGameAccount("Player1", 1000);
            GameAccount player2 = new ReducedPenaltyGameAccount("Player2", 1200);

            Game standardGame = GameFactory.CreateStandardGame();
            Game trainingGame = GameFactory.CreateTrainingGame();
            Game singlePlayerGame = GameFactory.CreateSinglePlayerGame();

            // Імітація ігор та оновлення статистики гравців
            player1.WinGame("Player2", standardGame);
            player2.LoseGame("Player1", standardGame);

            player1.WinGame("Player2", trainingGame);
            player2.LoseGame("Player1", trainingGame);

            player1.WinGame("Player2", singlePlayerGame);
            player2.LoseGame("Player1", singlePlayerGame);

            // Виведення статистики кожного гравця
            Console.WriteLine(player1.GetStats());
            Console.WriteLine(player2.GetStats());
        }
    }
}    