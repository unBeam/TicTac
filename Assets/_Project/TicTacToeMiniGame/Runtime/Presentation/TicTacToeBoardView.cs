using System;
using TicTacToeMiniGame.Runtime.Domain;
using UnityEngine;

namespace TicTacToeMiniGame.Runtime.Presentation
{
    public sealed class TicTacToeBoardView : MonoBehaviour
    {
        [SerializeField] private TicTacToeCellView[] _cells;

        public event Action<int> CellSelected;

        public void Render(TicTacToeBoard board, bool isInteractable)
        {
            if (board == null || _cells == null)
            {
                return;
            }

            int count = Math.Min(_cells.Length, 9);

            for (int index = 0; index < count; index++)
            {
                TicTacToeCellView cell = _cells[index];

                if (cell == null)
                {
                    continue;
                }

                cell.Bind(index, board.GetCell(index), isInteractable, RaiseCellSelected);
            }
        }

        private void RaiseCellSelected(int cellIndex)
        {
            Action<int> handler = CellSelected;

            if (handler != null)
            {
                handler.Invoke(cellIndex);
            }
        }
    }
}
