using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class CharacterButtonController
    {
        private const string k_button = "char-button";
        private const string k_iconContainer = "icon-container";
        private const string k_icon = "icon";
        private const string k_highlightStyle = "highlight-button-style";

        private Button button;
        private VisualElement iconContainer;
        private VisualElement icon;

        public CharacterUIInfo characterData { get; private set; }
        private CharacterPanelUIController characterPanel;
        
        public CharacterButtonController(CharacterUIInfo characterUIInfo, TemplateContainer characterButtonElement, CharacterPanelUIController panel)
        {
            characterData = characterUIInfo;
            characterPanel = panel;
            SetVisualElements(characterButtonElement);
            SetData();
            RegisterCallbacks();
        }
        
        ~CharacterButtonController()
        {
            UnregisterCallbacks();
        }

        private void SetVisualElements(TemplateContainer characterButtonElement)
        {
            button = characterButtonElement.Q<Button>(k_button);
            iconContainer = characterButtonElement.Q(k_iconContainer);
            icon = characterButtonElement.Q(k_icon);
        }

        private void SetData()
        {
            icon.style.backgroundImage = new StyleBackground(characterData.portrait);
        }

        private void OnClick()
        {
            if (characterPanel)
            {
                characterPanel.OnClickButton(this);
            }
        }

        public void ToggleHighlight(bool enable)
        {
            if (enable)
            {
                button.AddToClassList(k_highlightStyle);
            }
            else
            {
                button.RemoveFromClassList(k_highlightStyle);
            }
        }

        private void RegisterCallbacks()
        {
            button.clicked += OnClick;
        }

        private void UnregisterCallbacks()
        {
            button.clicked -= OnClick;
        }
    }
}
