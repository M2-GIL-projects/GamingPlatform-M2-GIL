using System;
using System.Text;

namespace GamingPlatform.Models
{
    public class Morpion
    {
        private const int BoardSize = 3;

        // État du jeu
        public string[,] Board { get; private set; }
        public string CurrentPlayer { get; set; } = "X";
        public bool IsGameOver { get; set; } = false;
        public string Winner { get; set; }

        // Gestion des joueurs
        public Guid LobbyId { get; set; }
        public string PlayerX { get; set; }
        public string PlayerO { get; set; }
        public string CurrentPlayerName { get; set; }
        public string CurrentPlayerConnectionId { get; set; }
        public string PlayerXConnectionId { get; set; }
        public string PlayerOConnectionId { get; set; }

        public Morpion()
        {
            InitializeBoard();
        }

        public void InitializeBoard()
        {
            Board = new string[BoardSize, BoardSize];
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    Board[i, j] = null;
                }
            }
        }

        public bool CheckForWin(string player)
        {
            for (int i = 0; i < BoardSize; i++)
            {
                if (Board[i, 0] == player && Board[i, 1] == player && Board[i, 2] == player) return true;
                if (Board[0, i] == player && Board[1, i] == player && Board[2, i] == player) return true;
            }

            if (Board[0, 0] == player && Board[1, 1] == player && Board[2, 2] == player) return true;
            if (Board[0, 2] == player && Board[1, 1] == player && Board[2, 0] == player) return true;

            return false;
        }

        public bool IsBoardFull()
        {
            foreach (var cell in Board)
            {
                if (cell == null) return false;
            }
            return true;
        }

        public void MakeMove(int x, int y, string playerSymbol)
        {
            if (x < 0 || x >= BoardSize || y < 0 || y >= BoardSize || !string.IsNullOrEmpty(Board[x, y]))
                throw new InvalidOperationException("Mouvement invalide");

            Board[x, y] = playerSymbol;
        }

        public string RenderBoard()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    builder.Append(string.IsNullOrEmpty(Board[i, j]) ? "[ ]" : $"[{Board[i, j]}]");
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}
