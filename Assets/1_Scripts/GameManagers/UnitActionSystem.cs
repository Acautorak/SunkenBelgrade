using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }


    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    //public event EventHandler OnEnemyUnitSelected;



    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    [SerializeField] private BaseAction selectedAction;

    private bool isBusy;
    private int selectedUnitIndex;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("ne radi singlton " + transform + " " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {

        if (isBusy)
        {
            return;
        }

        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (TryHandleUnitSelecetion())
        {
            return;
        } 

        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if (InputManager.Instance.IsScreenTouchedOrClicked())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MousePosition.GetMouseWorldPosition2D());
            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return;
            }

            if (!selectedUnit.TrySpendActionPoints(selectedAction))
            {
                return;
            }
            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }


    private bool TryHandleUnitSelecetion()
    {
        if (InputManager.Instance.IsScreenTouchedOrClicked())
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, float.MaxValue, unitLayerMask);
            //if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            if (hit.collider != null)
            {
                if (hit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit)
                    {
                        // unit is already selected
                        return false;
                    }

                    if (unit.IsEnemy())
                    {
                        // Clicked on enemy
                        return false;
                    }

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        //selectedUnit.SetIdleAnimator(); ss
        SetSelectedAction(unit.GetAction<MoveAction>());
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }

    public void SelectNextUnit()
    {
        selectedUnitIndex++;
        selectedUnit = UnitManager.Instance.GetUnitList()[selectedUnitIndex];

        if (selectedUnit.IsEnemy())
        {
            //OnEnemyUnitSelected?.Invoke(this, EventArgs.Empty);
            SelectNextUnit();
        }

        if (selectedUnitIndex > UnitManager.Instance.GetUnitList().Count)
        {
            selectedUnitIndex = 0;
        }
    }

    public void SetupSelectedUnit()
    {
        selectedUnitIndex = 0;
        selectedUnit = UnitManager.Instance.GetUnitList()[selectedUnitIndex];
        SetSelectedUnit(selectedUnit);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (selectedUnit.GetHealthNormalized() <= 0)
        {
            List<Unit> friendlyUnitList = UnitManager.Instance.GetFriendlyUnitList();

            if (friendlyUnitList.Count > 0)
            {
                SetSelectedUnit(friendlyUnitList[0]);
            }
            else Debug.LogWarning("Game Over");
        }

        //SelectNextUnit();
    }
}
