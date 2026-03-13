using System;
using System.Collections.Generic;

namespace TicTacToeMiniGame.Runtime.Domain
{
    public sealed class TicTacToeBoard
    {
        private readonly TicTacToeMark[] _cells;

        public TicTacToeBoard()
        {
            _cells = new TicTacToeMark[9];
            CurrentTurn = TicTacToeMark.X;
        }

        public TicTacToeMark CurrentTurn { get; private set; }

        public ReadOnlyMemory<TicTacToeMark> Cells => _cells;

        public TicTacToeMark GetCell(int index)
        {
            return _cells[index];
        }

        public bool IsCellEmpty(int index)
        {
            if (index < 0 || index >= _cells.Length)
            {
                return false;
            }

            return _cells[index] == TicTacToeMark.None;
        }

        public bool HasEmptyCells()
        {
            for (int index = 0; index < _cells.Length; index++)
            {
                if (_cells[index] == TicTacToeMark.None)
                {
                    return true;
                }
            }

            return false;
        }

        public IReadOnlyList<int> GetEmptyCellIndices()
        {
            List<int> indices = new List<int>();

            for (int index = 0; index < _cells.Length; index++)
            {
                if (_cells[index] == TicTacToeMark.None)
                {
                    indices.Add(index);
                }
            }

            return indices;
        }

        public bool TryApplyMove(int index, TicTacToeMark mark)
        {
            if (index < 0 || index >= _cells.Length)
            {
                return false;
            }

            if (mark == TicTacToeMark.None)
            {
                return false;
            }

            if (_cells[index] != TicTacToeMark.None)
            {
                return false;
            }

            if (CurrentTurn != mark)
            {
                return false;
            }

            _cells[index] = mark;
            CurrentTurn = mark == TicTacToeMark.X ? TicTacToeMark.O : TicTacToeMark.X;
            return true;
        }

        public void Reset()
        {
            Array.Clear(_cells, 0, _cells.Length);
            CurrentTurn = TicTacToeMark.X;
        }
    }
}
