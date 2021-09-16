namespace Apex.Demo.RTS.AI
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Component for facilitating the Mouse Over text, showing the name of entities when the mouse hovers above them.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.SingletonMonoBehaviour{Apex.Demo.RTS.AI.MouseOverManager}" />
    public class MouseOverManager : SingletonMonoBehaviour<MouseOverManager>
    {
        [SerializeField, Tooltip("The text component to write the entity name in.")]
        private Text _mouseOverText;

        [SerializeField, Tooltip("The panel that the text component resides in (is a child of).")]
        private Transform _mouseOverPanel;

        private void OnEnable()
        {
            if (_mouseOverPanel == null)
            {
                throw new ArgumentNullException("_mouseOverPanel");
            }

            if (_mouseOverText == null)
            {
                throw new ArgumentNullException("_mouseOverText");
            }

            // The mouse over text starts by being disabled
            _mouseOverPanel.gameObject.SetActive(false);
        }

        /// <summary>
        /// Sets the mouse over text and moves it to the specified world coordinate position.
        /// </summary>
        /// <param name="text">The text to show.</param>
        /// <param name="worldPosition">The world position to show at.</param>
        public void ShowMouseOver(string text, Vector3 worldPosition)
        {
            _mouseOverPanel.gameObject.SetActive(true);
            _mouseOverText.text = text;

            var pos = Camera.main.WorldToScreenPoint(worldPosition);
            _mouseOverPanel.transform.position = new Vector3(pos.x + 25f, pos.y + 50f, 0f);
        }

        /// <summary>
        /// Hides the mouse over text.
        /// </summary>
        public void HideMouseOver()
        {
            _mouseOverPanel.gameObject.SetActive(false);
        }
    }
}