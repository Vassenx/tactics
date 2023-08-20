using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class AllyButtonController : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI allyName;
        
        //private const string k_highlightStyle = "highlight-button-style";

        public Ally ally { get; private set; }
        private AllyPanelController allyPanelController;
        
        public void Initialize(Ally connectedAlly, AllyPanelController panel)
        {
            enabled = false; // no update function

            ally = connectedAlly;
            allyPanelController = panel;
            SetData();
            
            RegisterCallbacks();
        }
        
        public void OnDestroy()
        {
            UnregisterCallbacks();
        }

        private void SetData()
        {
            icon.sprite = ally.UIInfo.portrait;
        }

        private void OnClick()
        {
            if (allyPanelController)
            {
                allyPanelController.OnClickButton(this);
            }
        }

        public void ToggleHighlight(bool enable)
        {
            if (enable)
            {
                Color oldColor = button.image.color;
                button.image.color = new Color(oldColor.r, oldColor.b, oldColor.g, 0.8f);
            }
            else
            {
                Color oldColor = button.image.color;
                button.image.color = new Color(oldColor.r, oldColor.b, oldColor.g, 0.2f);
            }
        }

        private void RegisterCallbacks()
        {
            button.onClick.AddListener(OnClick);
        }

        private void UnregisterCallbacks()
        {
            button.onClick.RemoveListener(OnClick);
        }
    }
}

