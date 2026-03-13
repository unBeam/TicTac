using System;
using TicTacToeMiniGame.Runtime.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToeMiniGame.Runtime.Presentation
{
    public sealed class TicTacToeCellView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _markText;

        private Action<int> _onSelected;
        private int _cellIndex;

        public void Bind(int cellIndex, TicTacToeMark mark, bool isInteractable, Action<int> onSelected)
        {
            _cellIndex = cellIndex;
            _onSelected = onSelected;

            if (_markText != null)
            {
                _markText.text = GetMarkLabel(mark);
            }

            if (_button == null)
            {
                return;
            }

            _button.onClick.RemoveListener(OnButtonClicked);
            _button.onClick.AddListener(OnButtonClicked);
            _button.interactable = isInteractable && mark == TicTacToeMark.None;
        }

        private void OnButtonClicked()
        {
            Action<int> callback = _onSelected;

            if (callback != null)
            {
                callback.Invoke(_cellIndex);
            }
        }

        private string GetMarkLabel(TicTacToeMark mark)
        {
            if (mark == TicTacToeMark.X)
            {
                return "X";
            }

            if (mark == TicTacToeMark.O)
            {
                return "O";
            }

            return string.Empty;
        }
    }
}
