using System;
using System.Collections.Generic;
using Progression;
using UnityEngine;

/**
 * This class is responsible for keeping the player's progress
 */
[Serializable]
public class PlayerData
{
    [SerializeField]
    private int goodData;
    [SerializeField]
    private int badData;
    [SerializeField] 
    private int hardDriveSize;
    [SerializeField]
    private List<GameUpgradeLevelPair> upgrades;

    public PlayerData() : this(0, 0, 0)
    {
        if (GameManager.Instance != null)
        {
            hardDriveSize = GameManager.Instance.Constants.initialHardDriveSize;    
        }

        upgrades = new List<GameUpgradeLevelPair>();
    }

    public PlayerData(int goodData, int badData, int hardDriveSize)
    {
        this.goodData = goodData;
        this.badData = badData;
        this.hardDriveSize = hardDriveSize;
        upgrades = new List<GameUpgradeLevelPair>();
    }

    public int GoodData
    {
        get => goodData;
        set => goodData = value;
    }

    public int BadData
    {
        get => badData;
        set => badData = value;
    }

    public int HardDriveSize
    {
        get => hardDriveSize;
        set => hardDriveSize = value;
    }

    public int GetUpgradeLevel(GameUpgrade upgrade) => upgrades.Find(pair => pair.One.Equals(upgrade)).Two;

    public void SetUpgradeLevel(GameUpgrade upgrade, int value)
    {
        var pair = upgrades.Find(pair => pair.One.Equals(upgrade));
        if (pair == null)
        {
            pair = new GameUpgradeLevelPair(upgrade, 0);
            upgrades.Add(pair);
        }

        pair.Two = value;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}