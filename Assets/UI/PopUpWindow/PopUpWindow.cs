using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

// Tutorial from MadCat Tutorials: https://www.youtube.com/watch?v=c-K5KmQW-Zk&list=PLu_jCQwLZZq8lJXEKroIb7SKK3F8klGGo
namespace PopUp
{
    public class PopUpWindow : VisualElement
    {
        [UnityEngine.Scripting.Preserve]
        public new class UxmlFactory : UxmlFactory<PopUpWindow> { }

        private const string styleResource = "PopUpWindowStyleSheet";
        private const string ussPopUp = "popup_window";
        private const string ussPopUpContainer = "popup_container";
        private const string ussHorzContainer = "horizontal_container";
        private const string ussPopUpMsg = "popup_msg";

        private const string ussPopUpButton = "popup_button";
        private const string ussPopUpButtonCancel = "button_cancel";
        private const string ussPopUpButtonConfirm = "button_confirm";
        
        public PopUpWindow()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(styleResource));
            AddToClassList(ussPopUpContainer);
            
            VisualElement window = new VisualElement();
            window.AddToClassList(ussPopUp);
            hierarchy.Add(window);
            
            // Text Section
            VisualElement horizontalContainerText = new VisualElement();
            horizontalContainerText.AddToClassList(ussHorzContainer);
            window.Add(horizontalContainerText);

            Label msgLabel = new Label();
            msgLabel.text = "Do you really want to?";
            msgLabel.AddToClassList(ussPopUpMsg);
            horizontalContainerText.Add(msgLabel);
            
            // Button Section
            VisualElement horizontalContainerButton = new VisualElement();
            horizontalContainerButton.AddToClassList(ussHorzContainer);
            window.Add(horizontalContainerButton);

            Button confirmButton = new Button() { text = "CONFIRM" };
            confirmButton.AddToClassList(ussPopUpButton);
            confirmButton.AddToClassList(ussPopUpButtonConfirm);
            horizontalContainerButton.Add(confirmButton);
            
            Button cancelButton = new Button() { text = "CANCEL" };
            cancelButton.AddToClassList(ussPopUpButton);
            cancelButton.AddToClassList(ussPopUpButtonCancel);
            horizontalContainerButton.Add(cancelButton);

            confirmButton.clicked += OnConfirm;
            cancelButton.clicked += OnCancel;
        }

        public event Action confirmed;
        public event Action cancelled;

        protected void OnConfirm()
        {
            confirmed?.Invoke();
        }

        protected void OnCancel()
        {
            cancelled?.Invoke();
        }
    }
}