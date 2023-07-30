using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace PopUp
{
    public class PopUpTester : MonoBehaviour
    {
        private void OnEnable()
        {
            UIDocument ui = GetComponent<UIDocument>();
            VisualElement root = ui.rootVisualElement;

            PopUpWindow popUp = new PopUpWindow();
            root.Add(popUp);

            popUp.confirmed += () => Debug.Log("Confirmed");
            popUp.confirmed += () => root.Remove(popUp);

            popUp.cancelled += () => Debug.Log("Cancelled");
            popUp.cancelled += () => root.Remove(popUp);
        }
    }
}