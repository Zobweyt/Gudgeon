using Discord.Interactions;
using Gudgeon.Common.Customization.TwentyFortyEight;

namespace Gudgeon.Modules.Games.TwentyFortyEight
{
    internal class Board
    {
        private const int MaxBoardSize = 4;
        private int Points = 0;

        private string[,] DefaultBoard
        {
            get
            {
                return new string[4, 4]
                {
                    {
                        TwentyFortyEightEmojis.T0, TwentyFortyEightEmojis.T0, TwentyFortyEightEmojis.T0, TwentyFortyEightEmojis.T0
                    },
                    {
                        TwentyFortyEightEmojis.T0, TwentyFortyEightEmojis.T0, TwentyFortyEightEmojis.T0, TwentyFortyEightEmojis.T0
                    },
                    {
                        TwentyFortyEightEmojis.T0, TwentyFortyEightEmojis.T0, TwentyFortyEightEmojis.T0, TwentyFortyEightEmojis.T0
                    },
                    {
                        TwentyFortyEightEmojis.T0, TwentyFortyEightEmojis.T0, TwentyFortyEightEmojis.T0, TwentyFortyEightEmojis.T0
                    }
                };
            }
        }
        public string[,] BoardArray;

        public Board()
        {
            BoardArray = DefaultBoard;
            GenerateNewTile();
        }

        public bool IsFull
        {
            get
            {
                foreach (string tile in BoardArray)
                {
                    if (tile == TwentyFortyEightEmojis.T0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
        public bool CanMove
        {
            get
            {
                for (int column = 1; column < MaxBoardSize; column++)
                {
                    for (int tile = 0; tile < MaxBoardSize; tile++)
                    {
                        string backTile = BoardArray[column, tile];
                        string frontTile = BoardArray[column - 1, tile];

                        if (frontTile == backTile && backTile != TwentyFortyEightEmojis.T0)
                        {
                            return true;
                        }
                    }
                }
                for (int column = 2; column > -1; column--)
                {
                    for (int tile = 0; tile < MaxBoardSize; tile++)
                    {
                        string backTile = BoardArray[column, tile];
                        string frontTile = BoardArray[column + 1, tile];

                        if (frontTile == backTile && backTile != TwentyFortyEightEmojis.T0)
                        {
                            return true;
                        }
                    }
                }
                for (int column = 0; column < MaxBoardSize; column++)
                {
                    for (int tile = 1; tile < MaxBoardSize; tile++)
                    {
                        string backTile = BoardArray[column, tile];
                        string frontTile = BoardArray[column, tile - 1];

                        if (frontTile == backTile && backTile != TwentyFortyEightEmojis.T0)
                        {
                            return true;
                        }
                    }
                }
                for (int column = 0; column < MaxBoardSize; column++)
                {
                    for (int tile = 2; tile > -1; tile--)
                    {
                        string backTile = BoardArray[column, tile];
                        string frontTile = BoardArray[column, tile + 1];

                        if (frontTile == backTile && backTile != TwentyFortyEightEmojis.T0)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }
        public bool HasTwentyFortyEightTile
        {
            get
            {
                foreach (string tile in BoardArray)
                {
                    if (tile == TwentyFortyEightEmojis.T2048)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void ShiftBoard(string customId)
        {
            switch (customId)
            {
                case "2048-shift-board-up":
                    ShiftUp();
                    break;
                case "2048-shift-board-down":
                    ShiftDown();
                    break;
                case "2048-shift-board-left":
                    ShiftLeft();
                    break;
                case "2048-shift-board-right":
                    ShiftRight();
                    break;
            }
        }
        private void ShiftUp()
        {
            bool isBoardShifted = false;

            for (int move = 1; move < MaxBoardSize; move++)
            {
                for (int column = 1; column < MaxBoardSize; column++)
                {
                    for (int tile = 0; tile < MaxBoardSize; tile++)
                    {
                        string backTile = BoardArray[column, tile];
                        string frontTile = BoardArray[column - 1, tile];

                        if (frontTile == TwentyFortyEightEmojis.T0 && backTile != TwentyFortyEightEmojis.T0)
                        {
                            BoardArray[column - 1, tile] = backTile;
                            BoardArray[column, tile] = TwentyFortyEightEmojis.T0;

                            isBoardShifted = true;
                        }
                    }
                }
            }

            for (int column = 1; column < MaxBoardSize; column++)
            {
                for (int tile = 0; tile < MaxBoardSize; tile++)
                {
                    string backTile = BoardArray[column, tile];
                    string frontTile = BoardArray[column - 1, tile];

                    if (frontTile == backTile && backTile != TwentyFortyEightEmojis.T0)
                    {
                        BoardArray[column - 1, tile] = MergeEqualTiles(frontTile);
                        CountPointsForMerging(frontTile);
                        BoardArray[column, tile] = TwentyFortyEightEmojis.T0;

                        isBoardShifted = true;
                    }
                }
            }

            if (!isBoardShifted)
            {
                return;
            }

            for (int move = 0; move < MaxBoardSize - 1; move++)
            {
                for (int column = 1; column < 4; column++)
                {
                    for (int tile = 0; tile < MaxBoardSize; tile++)
                    {
                        string backTile = BoardArray[column, tile];
                        string frontTile = BoardArray[column - 1, tile];

                        if (frontTile == TwentyFortyEightEmojis.T0 && backTile != TwentyFortyEightEmojis.T0)
                        {
                            BoardArray[column - 1, tile] = backTile;
                            BoardArray[column, tile] = TwentyFortyEightEmojis.T0;
                        }
                    }
                }
            }

            GenerateNewTile();
        }
        private void ShiftDown()
        {
            bool isBoardShifted = false;

            for (int move = 1; move < MaxBoardSize; move++)
            {
                for (int column = 2; column > -1; column--)
                {
                    for (int tile = 0; tile < MaxBoardSize; tile++)
                    {
                        string backTile = BoardArray[column, tile];
                        string frontTile = BoardArray[column + 1, tile];

                        if (frontTile == TwentyFortyEightEmojis.T0 && backTile != TwentyFortyEightEmojis.T0)
                        {
                            BoardArray[column + 1, tile] = backTile;
                            BoardArray[column, tile] = TwentyFortyEightEmojis.T0;

                            isBoardShifted = true;
                        }
                    }
                }
            }

            for (int column = 2; column > -1; column--)
            {
                for (int tile = 0; tile < MaxBoardSize; tile++)
                {
                    string backTile = BoardArray[column, tile];
                    string frontTile = BoardArray[column + 1, tile];

                    if (frontTile == backTile && backTile != TwentyFortyEightEmojis.T0)
                    {
                        BoardArray[column + 1, tile] = MergeEqualTiles(frontTile);
                        CountPointsForMerging(frontTile);
                        BoardArray[column, tile] = TwentyFortyEightEmojis.T0;

                        isBoardShifted = true;
                    }
                }
            }

            if (!isBoardShifted)
            {
                return;
            }

            for (int move = 1; move < MaxBoardSize - 1; move++)
            {
                for (int column = 2; column > -1; column--)
                {
                    for (int tile = 0; tile < MaxBoardSize; tile++)
                    {
                        string backTile = BoardArray[column, tile];
                        string frontTile = BoardArray[column + 1, tile];

                        if (frontTile == TwentyFortyEightEmojis.T0 && backTile != TwentyFortyEightEmojis.T0)
                        {
                            BoardArray[column + 1, tile] = backTile;
                            BoardArray[column, tile] = TwentyFortyEightEmojis.T0;

                            isBoardShifted = true;
                        }
                    }
                }
            }

            GenerateNewTile();
        }
        private void ShiftLeft()
        {
            bool isBoardShifted = false;

            for (int move = 1; move < MaxBoardSize; move++)
            {
                for (int column = 0; column < MaxBoardSize; column++)
                {
                    for (int tile = 1; tile < MaxBoardSize; tile++)
                    {
                        string backTile = BoardArray[column, tile];
                        string frontTile = BoardArray[column, tile - 1];

                        if (frontTile == TwentyFortyEightEmojis.T0 && backTile != TwentyFortyEightEmojis.T0)
                        {
                            BoardArray[column, tile - 1] = backTile;
                            BoardArray[column, tile] = TwentyFortyEightEmojis.T0;

                            isBoardShifted = true;
                        }
                    }
                }
            }

            for (int column = 0; column < MaxBoardSize; column++)
            {
                for (int tile = 1; tile < MaxBoardSize; tile++)
                {
                    string backTile = BoardArray[column, tile];
                    string frontTile = BoardArray[column, tile - 1];

                    if (frontTile == backTile && backTile != TwentyFortyEightEmojis.T0)
                    {
                        BoardArray[column, tile - 1] = MergeEqualTiles(frontTile);
                        CountPointsForMerging(frontTile);
                        BoardArray[column, tile] = TwentyFortyEightEmojis.T0;

                        isBoardShifted = true;
                    }
                }
            }

            if (!isBoardShifted)
            {
                return;
            }

            for (int move = 1; move < MaxBoardSize - 1; move++)
            {
                for (int column = 0; column < MaxBoardSize; column++)
                {
                    for (int tile = 1; tile < MaxBoardSize; tile++)
                    {
                        string backTile = BoardArray[column, tile];
                        string frontTile = BoardArray[column, tile - 1];

                        if (frontTile == TwentyFortyEightEmojis.T0 && backTile != TwentyFortyEightEmojis.T0)
                        {
                            BoardArray[column, tile - 1] = backTile;
                            BoardArray[column, tile] = TwentyFortyEightEmojis.T0;
                        }
                    }
                }
            }

            GenerateNewTile();
        }
        private void ShiftRight()
        {
            bool isBoardShifted = false;

            for (int move = 1; move < MaxBoardSize; move++)
            {
                for (int column = 0; column < MaxBoardSize; column++)
                {
                    for (int tile = 2; tile > -1; tile--)
                    {
                        string backTile = BoardArray[column, tile];
                        string frontTile = BoardArray[column, tile + 1];

                        if (frontTile == TwentyFortyEightEmojis.T0 && backTile != TwentyFortyEightEmojis.T0)
                        {
                            BoardArray[column, tile + 1] = backTile;
                            BoardArray[column, tile] = TwentyFortyEightEmojis.T0;

                            isBoardShifted = true;
                        }
                    }
                }
            }

            for (int column = 0; column < MaxBoardSize; column++)
            {
                for (int tile = 2; tile > -1; tile--)
                {
                    string backTile = BoardArray[column, tile];
                    string frontTile = BoardArray[column, tile + 1];

                    if (frontTile == backTile && backTile != TwentyFortyEightEmojis.T0)
                    {
                        BoardArray[column, tile + 1] = MergeEqualTiles(frontTile);
                        CountPointsForMerging(frontTile);
                        BoardArray[column, tile] = TwentyFortyEightEmojis.T0;

                        isBoardShifted = true;
                    }
                }
            }

            if (!isBoardShifted)
            {
                return;
            }

            for (int move = 1; move < MaxBoardSize - 1; move++)
            {
                for (int column = 0; column < MaxBoardSize; column++)
                {
                    for (int tile = 2; tile > -1; tile--)
                    {
                        string backTile = BoardArray[column, tile];
                        string frontTile = BoardArray[column, tile + 1];

                        if (frontTile == TwentyFortyEightEmojis.T0 && backTile != TwentyFortyEightEmojis.T0)
                        {
                            BoardArray[column, tile + 1] = backTile;
                            BoardArray[column, tile] = TwentyFortyEightEmojis.T0;
                        }
                    }
                }
            }

            GenerateNewTile();
        }

        public void GenerateNewTile()
        {
            Random random = new();
            int value = random.Next(0, 6);

            string newTile;

            if (value == 0)
            {
                newTile = TwentyFortyEightEmojis.T4;
            }
            else
            {
                newTile = TwentyFortyEightEmojis.T2;
            }

            while (true)
            {
                int column = random.Next(0, 4);
                int tile = random.Next(0, 4);

                if (BoardArray[column, tile] != TwentyFortyEightEmojis.T0)
                {
                    continue;
                }

                BoardArray[column, tile] = newTile;

                break;
            }
        }
        public string MergeEqualTiles(string tile) => tile switch
        {
            TwentyFortyEightEmojis.T2 => TwentyFortyEightEmojis.T4,
            TwentyFortyEightEmojis.T4 => TwentyFortyEightEmojis.T8,
            TwentyFortyEightEmojis.T8 => TwentyFortyEightEmojis.T16,
            TwentyFortyEightEmojis.T16 => TwentyFortyEightEmojis.T32,
            TwentyFortyEightEmojis.T32 => TwentyFortyEightEmojis.T64,
            TwentyFortyEightEmojis.T64 => TwentyFortyEightEmojis.T128,
            TwentyFortyEightEmojis.T128 => TwentyFortyEightEmojis.T256,
            TwentyFortyEightEmojis.T256 => TwentyFortyEightEmojis.T512,
            TwentyFortyEightEmojis.T512 => TwentyFortyEightEmojis.T1024,
            TwentyFortyEightEmojis.T1024 => TwentyFortyEightEmojis.T2048
        };
        private void CountPointsForMerging(string tile) => Points += tile switch
        {
            TwentyFortyEightEmojis.T2 => 2,
            TwentyFortyEightEmojis.T4 => 4,
            TwentyFortyEightEmojis.T8 => 8,
            TwentyFortyEightEmojis.T16 => 16,
            TwentyFortyEightEmojis.T32 => 32,
            TwentyFortyEightEmojis.T64 => 64,
            TwentyFortyEightEmojis.T128 => 128,
            TwentyFortyEightEmojis.T256 => 256,
            TwentyFortyEightEmojis.T512 => 512,
            TwentyFortyEightEmojis.T1024 => 1024
        };

        public string BuildBoardMessage(SocketInteractionContext context)
        {
            string text = ":black_large_square::regional_indicator_a: :regional_indicator_b: :regional_indicator_c: :regional_indicator_d:\n";

            for (int column = 0; column < Board.MaxBoardSize; column++)
            {
                text += column switch
                {
                    0 => ":one:",
                    1 => ":two:",
                    2 => ":three:",
                    3 => ":four:"
                };

                for (int tile = 0; tile < Board.MaxBoardSize; tile++)
                {
                    text += BoardArray[column, tile] + " ";
                }

                text += "\n";
            }

            return text += $"\n{context.User.Mention}: {Points} points";
        }
    }
}