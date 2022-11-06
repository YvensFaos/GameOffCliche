using System;
using System.Collections.Generic;
using Gameplay.Skills;
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
    [SerializeField] 
    private List<GameSkill> skills;

    public PlayerData() : this(0, 0, 0)
    {
        if (GameManager.Instance != null)
        {
            hardDriveSize = GameManager.Instance.Constants.initialHardDriveSize;    
        }
        upgrades = new List<GameUpgradeLevelPair>();
        skills = new List<GameSkill>();
    }

    public PlayerData(int goodData, int badData, int hardDriveSize)
    {
        this.goodData = goodData;
        this.badData = badData;
        this.hardDriveSize = hardDriveSize;
        upgrades = new List<GameUpgradeLevelPair>();
        skills = new List<GameSkill>();
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

    public List<GameUpgradeLevelPair> Upgrades => upgrades;

    public List<GameSkill> Skills
    {
        get => skills;
        set => skills = value;
    }

    public void AddSkill(GameSkill skill)
    {
        if (!skills.Contains(skill))
        {
            skills.Add(skill);
        }
    }

    public int GetUpgradeLevel(GameUpgrade upgrade)
    {
        var pair = Upgrades.Find(pair => pair.One.Equals(upgrade));
        return pair?.Two ?? -1;
    } 

    public void SetUpgradeLevel(GameUpgrade upgrade, int value)
    {
        var pair = Upgrades.Find(pair => pair.One.Equals(upgrade));
        if (pair == null)
        {
            pair = new GameUpgradeLevelPair(upgrade, 0);
            Upgrades.Add(pair);
        }

        pair.Two = value;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}