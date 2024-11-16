using GamingPlatform.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;

public class LabyrinthGenerator
{
    private readonly IHubContext<LabyrinthHub> _hubContext;

    public LabyrinthGenerator(IHubContext<LabyrinthHub> hubContext)
    {
        _hubContext = hubContext;
    }
    public async Task<bool[,]> GenerateLabyrinth(int rows, int cols)
    {
        bool[,] adjacency = new bool[rows * cols, rows * cols];
        HashSet<int> visitedCells = new() { 1 };
        int[] cellOrder = new int[rows * cols];

        for (int i = 0; i < rows * cols; i++)
            cellOrder[i] = i;

        cellOrder = ShuffleArray(cellOrder);

        for (int cellCounter = 0; cellCounter < rows * cols; cellCounter++)
        {
            int currentCell = cellOrder[cellCounter];
            List<int> cellsInPath = new() { currentCell };

            while (!visitedCells.Contains(currentCell))
            {
                int currentRow = currentCell / cols;
                int currentCol = currentCell % cols;
                List<int> candidates = new();

                if (currentCol > 0 && !cellsInPath.Contains(currentCell - 1))
                    candidates.Add(currentCell - 1);
                if (currentCol < cols - 1 && !cellsInPath.Contains(currentCell + 1))
                    candidates.Add(currentCell + 1);
                if (currentRow > 0 && !cellsInPath.Contains(currentCell - cols))
                    candidates.Add(currentCell - cols);
                if (currentRow < rows - 1 && !cellsInPath.Contains(currentCell + cols))
                    candidates.Add(currentCell + cols);

                if (candidates.Count > 0)
                {
                    candidates = ShuffleArray(candidates);
                    int nextCell = -1;
                    foreach (var candidate in candidates)
                    {
                        if (visitedCells.Contains(candidate) && new Random().NextDouble() < 0.05)
                        {
                            nextCell = candidate;
                            break;
                        }
                        else if (!visitedCells.Contains(candidate))
                        {
                            nextCell = candidate;
                            break;
                        }
                    }

                    if (nextCell >= 0)
                    {
                        cellsInPath.Add(nextCell);
                        adjacency[nextCell, currentCell] = true;
                        adjacency[currentCell, nextCell] = true;
                        currentCell = nextCell;
                    }
                    else break;
                }
                else break;
            }
            visitedCells.UnionWith(cellsInPath);
        }
        // Envoi de la matrice d'adjacence aux clients
        await _hubContext.Clients.All.SendAsync("ReceiveLabyrinth", adjacency);

        return adjacency;
    }

    private static List<int> ShuffleArray(List<int> array)
    {
        Random random = new();
        for (int i = array.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
        return array;
    }

    private static int[] ShuffleArray(int[] array)
    {
        Random random = new();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
        return array;
    }
}
