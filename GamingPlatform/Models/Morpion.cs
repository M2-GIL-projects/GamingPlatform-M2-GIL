using System;
using System.Text;

namespace GamingPlatform.Models
{
    public class Morpion : IGameBoard
    {
        private string[,] _board;
        private const int Size = 3; // Taille du plateau Morpion

        public void InitializeBoard()
        {
            _board = new string[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _board[i, j] = string.Empty; // Initialisation des cases vides
                }
            }
        }

        public string RenderBoard()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    builder.Append(string.IsNullOrEmpty(_board[i, j]) ? "[ ]" : $"[{_board[i, j]}]");
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }

        public bool IsGameOver()
        {
            // Vérifie les lignes, colonnes et diagonales pour une victoire
            for (int i = 0; i < Size; i++)
            {
                if (!string.IsNullOrEmpty(_board[i, 0]) &&
                    _board[i, 0] == _board[i, 1] && _board[i, 1] == _board[i, 2]) return true;

                if (!string.IsNullOrEmpty(_board[0, i]) &&
                    _board[0, i] == _board[1, i] && _board[1, i] == _board[2, i]) return true;
            }

            if (!string.IsNullOrEmpty(_board[0, 0]) &&
                _board[0, 0] == _board[1, 1] && _board[1, 1] == _board[2, 2]) return true;

            if (!string.IsNullOrEmpty(_board[0, 2]) &&
                _board[0, 2] == _board[1, 1] && _board[1, 1] == _board[2, 0]) return true;

            // Vérifie s'il reste des cases vides
            foreach (var cell in _board)
            {
                if (string.IsNullOrEmpty(cell)) return false;
            }

            // Match nul si le plateau est plein
            return true;
        }

        public void MakeMove(int x, int y, string playerSymbol)
        {
            if (x < 0 || x >= Size || y < 0 || y >= Size || !string.IsNullOrEmpty(_board[x, y]))
                throw new InvalidOperationException("Mouvement invalide");
            _board[x, y] = playerSymbol;
        }
    }
}
