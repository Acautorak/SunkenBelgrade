using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : PersistentMonoSingleton<PartyManager>
{
    public int gold = 1500;
    public int gems = 50;
    [SerializeField] private List<PartyUnitData> partyUnitList = new List<PartyUnitData>();

    public event EventHandler OnGoldChanged;


    private void Start()
    {
        LoadSavedUnits();
    }

    public void BuyUnitGold(PartyUnitData partyUnit)
    {
        if (partyUnitList.Count >= 4)
        {
            Debug.Log("nisam kupio jer je party FULL");
            return;
        }

        if (gold > partyUnit.goldCost)
        {
            Debug.Log("Kupio sam " + partyUnit.unitName);
            gold -= partyUnit.goldCost;
            partyUnitList.Add(partyUnit);
            OnGoldChanged?.Invoke(this, EventArgs.Empty);

        }
        else
        {
            Debug.LogError("Nisam kupio " + partyUnit.unitName);
        }
    }

    public void SaveUnitList()
    {
        SaveManager.SaveListJson("PartyUnitList", partyUnitList);
    }

    public void LoadSavedUnits()
    {
        List<PartyUnitData> loadedPartyUnits = SaveManager.LoadListJson<PartyUnitData>("PartyUnitList");
        partyUnitList.Clear();
        foreach (PartyUnitData partyUnit in loadedPartyUnits)
        {
            partyUnitList.Add(partyUnit);
        }

        Debug.Log("imamo " + partyUnitList.Count + "unita");

        SaveManager.LoadGoldGems();
    }

    private void OnApplicationQuit()
    {
        SaveUnitList();
    }

    public List<PartyUnitData> GetPartyUnitList()
    {
        return partyUnitList;
    }
}

