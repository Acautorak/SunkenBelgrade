using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Puko ti je damageManager");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public int CalculateMeleDamage(Unit attackingUnit, Unit defendingUnit, int abilityDamage)
    {
        int damageTaken = 0;
        UnitStats attackingUnitStats = attackingUnit.unitStats;
        UnitStats defendingUnitStats = defendingUnit.unitStats;

        if (!IsHit(attackingUnitStats, defendingUnitStats))
        {
            Debug.LogError("MISSED");
            return damageTaken;
        }

        if (HasDodged(defendingUnitStats))
        {
            Debug.LogError("dodged");
            return damageTaken;
        }

        int strDiff = attackingUnitStats.strength - defendingUnitStats.armor;
        float strDiffCoef;
        switch (strDiff)
        {
            case < -30:
                strDiffCoef = 0.1f;
                break;
            case < -10:
                strDiffCoef = 0.5f;
                break;
            case <= 0:
                strDiffCoef = 0.8f;
                break;
            case > 30:
                strDiffCoef = 1.8f;
                break;
            case > 10:
                strDiffCoef = 1.5f;
                break;
            default:
                strDiffCoef = 1f;
                break;
        }

        damageTaken = Mathf.RoundToInt(strDiff * strDiffCoef) - defendingUnitStats.miscDef + abilityDamage;

        return damageTaken;
    }

    public int CalculateRangedDamage(Unit attackingUnit, Unit defendingUnit, int abilityDamage)
    {
        int damageTaken = 0;
        UnitStats attackingUnitStats = attackingUnit.gameObject.GetComponent<UnitStats>();
        UnitStats defendingUnitStats = defendingUnit.gameObject.GetComponent<UnitStats>();

        if (!IsHit(attackingUnitStats, defendingUnitStats))
        {
            Debug.LogError("MISSED");
            return damageTaken;
        }

        if (HasDodged(defendingUnitStats))
        {
            Debug.LogError("dodged");
            return damageTaken;
        }

        int agiDiff = attackingUnitStats.agility - defendingUnitStats.armor;
        float agiDiffCoef;
        switch (agiDiff)
        {
            case < -30:
                agiDiffCoef = 0.1f;
                break;
            case < -10:
                agiDiffCoef = 0.5f;
                break;
            case <= 0:
                agiDiffCoef = 0.8f;
                break;
            case > 30:
                agiDiffCoef = 1.8f;
                break;
            case > 10:
                agiDiffCoef = 1.5f;
                break;
            default:
                agiDiffCoef = 1f;
                break;
        }

        damageTaken = Mathf.RoundToInt(agiDiff * agiDiffCoef) - defendingUnitStats.miscDef + abilityDamage;

        return damageTaken;
    }

    private bool IsHit(UnitStats attackingUnitStats, UnitStats defendingUnitStats)
    {
        int rollResult = Random.Range(1, 101);
        return (rollResult + attackingUnitStats.perception) >= 20;
    }

    private bool HasDodged(UnitStats unitStats)
    {
        int rollResult = Random.Range(1, 101);
        return (rollResult + unitStats.dodgeChance) >= 95;
    }
}
