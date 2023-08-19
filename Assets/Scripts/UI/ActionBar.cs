using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour
{
    [Header("Main Buttons")]
    [SerializeField] private Button moveButton;
    [SerializeField] private Button attackButton;

    [Header("Attack Sub-Buttons")]
    [SerializeField] private Button meleeButton;
    [SerializeField] private Button rangedButton;

    [SerializeField] private Ally ally;

    public static Action<Ally> OnPickMove;
    public static Action OnPickAttack;
    public static Action<Ally, int> OnPickAttackType;

    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main; // TODO
    }

    private void OnEnable()
    {
        moveButton.onClick.AddListener(OnPickMoveButton);
        attackButton.onClick.AddListener(OnPickAttackButton);
        meleeButton.onClick.AddListener(OnPickMeleeAttackButton);
        rangedButton.onClick.AddListener(OnPickRangedAttackButton);

        // enabled but not visible until character is selected
        ToggleMainButtons(false);
        ToggleSubButtons(false);
    }

    private void OnDisable()
    {
        moveButton.onClick.RemoveListener(OnPickMoveButton);
        attackButton.onClick.RemoveListener(OnPickAttackButton);
        meleeButton.onClick.RemoveListener(OnPickMeleeAttackButton);
        rangedButton.onClick.RemoveListener(OnPickRangedAttackButton);
    }

    public void ShowActionBar()
    {
        ToggleMainButtons(true);
        ToggleSubButtons(false);
    }
    
    public void HideActionBar()
    {
        ToggleMainButtons(false);
        ToggleSubButtons(false);
    }
    
    private void OnPickMoveButton()
    {
        ToggleMainButtons(false);
        OnPickMove?.Invoke(ally);
    }

    private void OnPickAttackButton()
    {
        ToggleMainButtons(false);
        ToggleSubButtons(true);
        OnPickAttack?.Invoke();
    }
    
    private void OnPickMeleeAttackButton()
    {
        ToggleSubButtons(false);
        OnPickAttackType?.Invoke(ally, 0);
    }
    
    private void OnPickRangedAttackButton()
    {
        ToggleSubButtons(false);
        OnPickAttackType?.Invoke(ally, 1);
    }
    
    private void ToggleMainButtons(bool enable)
    {
        moveButton.gameObject.SetActive(enable);
        attackButton.gameObject.SetActive(enable);
    }
    
    private void ToggleSubButtons(bool enable)
    {
        meleeButton.gameObject.SetActive(enable);
        rangedButton.gameObject.SetActive(enable);
    }
}
