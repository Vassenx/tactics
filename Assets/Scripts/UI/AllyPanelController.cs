using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class AllyPanelController : MonoBehaviour
    {
        [Header("Selected Ally - Visuals")] [SerializeField]
        private Image selectedAllyIcon;

        [SerializeField] private TextMeshProUGUI selectedAllyName;

        [Header("Selected Ally - Stats")]
        [SerializeField] private TextMeshProUGUI healthText;

        [SerializeField] private TextMeshProUGUI attackText;
        [SerializeField] private TextMeshProUGUI defenseText;
        [SerializeField] private TextMeshProUGUI magicText;

        [Header("Allies List")]
        [SerializeField] private HorizontalLayoutGroup alliesList;
        [SerializeField] private AllyButtonController allyButtonControllerPrefab;

        // controllers
        private AllyButtonController selectedButtonController;
        private List<AllyButtonController> allyButtonControllers;

        private void Awake()
        {
            this.enabled = false; // no update function
            
            BoardManager.OnBoardInitialized += CreateCharacterButtons;
            InputHandler.OnSelectCharacter += CharacterSelected;
        }

        private void OnDestroy()
        {
            allyButtonControllers.Clear();

            InputHandler.OnSelectCharacter -= CharacterSelected;
            BoardManager.OnBoardInitialized -= CreateCharacterButtons;
        }

        private void CharacterSelected(Character character)
        {
            Ally ally = (Ally) character;
            if (ally != null)
            {
                UpdateSelectedAlly(FindButtonByAlly(ally));
            }
        }

        private AllyButtonController FindButtonByAlly(Ally allyToFind)
        {
            return allyButtonControllers.Find(x => x.ally == allyToFind);
        }

        private void CreateCharacterButtons()
        {
            allyButtonControllers = new List<AllyButtonController>();

            BoardManager.Instance.alliesOnBoard?.ForEach(ally =>
            {
                AllyButtonController newAllyButtonController = Instantiate(allyButtonControllerPrefab, alliesList.transform);
                newAllyButtonController.Initialize(ally, this);
                allyButtonControllers.Add(newAllyButtonController);

                if (selectedButtonController == null)
                {
                    InputHandler.OnSelectCharacter?.Invoke(newAllyButtonController.ally);
                }
            });
        }

        private void UpdateSelectedAlly(AllyButtonController newSelectedButtonController)
        {
            // highlighting
            if (selectedButtonController != null)
            {
                selectedButtonController.ToggleHighlight(false);
            }
            newSelectedButtonController.ToggleHighlight(true);

            // visuals
            var ally = newSelectedButtonController.ally;
            selectedAllyIcon.sprite = ally.UIInfo.portrait;
            selectedAllyName.text = ally.UIInfo.charName;
            healthText.text = "HP " + ally.stats.maxHealth;
            // TODO attack/defense/magic

            selectedButtonController = newSelectedButtonController;
        }

        public void OnClickButton(AllyButtonController newSelectedButtonController)
        {
            InputHandler.OnSelectCharacter?.Invoke(newSelectedButtonController.ally);
        }
    }
}