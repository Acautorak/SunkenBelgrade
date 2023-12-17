using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;

    private BaseAction baseAction;

    public void SetActionButton(BaseAction baseAction)
    {
        this.baseAction = baseAction;

        textMeshPro.text = baseAction.GetActionName().ToUpper();
        button.image.sprite = baseAction.GetActionImage();

        button.onClick.AddListener(() =>
        {
            UnifiedActionManager.Instance.SetSelectedAction(baseAction);
        });
//        button.onClick.AddListener(() => ButtonClickSound.Instance.PlayClickSound());
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnifiedActionManager.Instance.GetSelectedAction(); //UnitActionSystem
        selectedGameObject.SetActive(selectedBaseAction == baseAction);
    }

    public void DestroyThisButton()
    {
        Destroy(gameObject);
    }
}
