using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterPanelUIController : MonoBehaviour
{
    [Header("Button Template")]
    [SerializeField] private VisualTreeAsset characterButtonTemplate;

    private VisualElement root;
    
    // List of all Characters
    private const string k_CharacterButtonsContainer = "chars-container";
    private const string k_HighlightButtonStyle = "highlight-button-style";
    private const string k_button = "char-button";

    // Selected Character
    private const string k_SelectedIcon = "icon";
    private const string k_SelectedNameLabel = "name-text";
    private const string k_HPLabel = "hp-stats-text";
    private const string k_AttackLabel = "attack-stats-text";
    private const string k_DefenseLabel = "defense-stats-text";
    private const string k_MagicLabel = "magic-stats-text";

    private VisualElement selectedIcon;
    private Label selectedNameLabel;
    private Label HPLabel;
    private Label AttackLabel;
    private Label DefenseLabel;
    private Label MagicLabel;

    // controllers
    private CharacterButtonController selectedButtonController;
    private List<CharacterButtonController> characterButtonControllers;
    
    // TODO: change size of panel depending on num of characters
    
    private void OnEnable()
    {
        SetVisualElements();

        BoardManager.OnBoardInitialized += CreateCharacterButtons;
        
        InputHandler.OnSelectCharacter += (character) => { UpdateSelectedCharacter(FindButtonByChar(character));};
    }

    private void SetVisualElements()
    {
        var uiDoc = GetComponent<UIDocument>();
        var uiRoot = uiDoc.rootVisualElement;
        root = uiRoot;
        
        // Selected Character
        selectedIcon = root.Q<VisualElement>(k_SelectedIcon);
        selectedNameLabel = root.Q<Label>(k_SelectedNameLabel);
        HPLabel = root.Q<Label>(k_HPLabel);
        AttackLabel = root.Q<Label>(k_AttackLabel);
        DefenseLabel = root.Q<Label>(k_DefenseLabel);
        MagicLabel = root.Q<Label>(k_MagicLabel);
    }

    private CharacterButtonController FindButtonByChar(Character charToFind)
    {
        return characterButtonControllers.Find(x => x.character == charToFind);
    }

    private void CreateCharacterButtons()
    {
        var buttonsContainer = root.Q<VisualElement>(k_CharacterButtonsContainer);
        characterButtonControllers = new List<CharacterButtonController>();
        
        BoardManager.Instance.alliesOnBoard?.ForEach(character =>
        {
            TemplateContainer buttonContainer = characterButtonTemplate.Instantiate();
            CharacterButtonController charButtonController = new CharacterButtonController(character, buttonContainer, this);
            buttonsContainer.Add(buttonContainer);
            characterButtonControllers.Add(charButtonController);

            if (selectedButtonController == null)
            {
                InputHandler.OnSelectCharacter?.Invoke(charButtonController.character);
            }
        });
    }
    
    private void UpdateSelectedCharacter(CharacterButtonController newSelectedButtonController)
    {
        // highlighting
        selectedButtonController?.ToggleHighlight(false);
        newSelectedButtonController.ToggleHighlight(true);
        
        // visuals
        var character = newSelectedButtonController.character;
        selectedIcon.style.backgroundImage = new StyleBackground(character.UIInfo.portrait);
        selectedNameLabel.text = character.UIInfo.charName;
        HPLabel.text = "HP " + character.stats.maxHealth;
        // TODO attack/defense/magic
        
        selectedButtonController = newSelectedButtonController;
    }
    
    public void OnClickButton(CharacterButtonController newSelectedButtonController)
    {
        InputHandler.OnSelectCharacter?.Invoke(newSelectedButtonController.character);
    }

    private void OnDisable()
    {
        characterButtonControllers.Clear();
        
        BoardManager.OnBoardInitialized -= CreateCharacterButtons;
    }
}
