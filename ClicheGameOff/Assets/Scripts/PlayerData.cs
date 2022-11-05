using System;
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
    private int betterResearchSkillLevel;
    [SerializeField]
    private int betterFactCheckingLevel;

    public PlayerData() : this(0, 0, 0, 0, 0)
    {
        if (GameManager.Instance != null)
        {
            hardDriveSize = GameManager.Instance.Constants.initialHardDriveSize;    
        }
    }

    public PlayerData(int goodData, int badData, int hardDriveSize, int betterResearchSkillLevel, int betterFactCheckingLevel)
    {
        this.goodData = goodData;
        this.badData = badData;
        this.hardDriveSize = hardDriveSize;
        this.betterResearchSkillLevel = betterResearchSkillLevel;
        this.betterFactCheckingLevel = betterFactCheckingLevel;
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

    public int BetterResearchSkillLevel
    {
        get => betterResearchSkillLevel;
        set => betterResearchSkillLevel = value;
    }

    public int BetterFactCheckingLevel
    {
        get => betterFactCheckingLevel;
        set => betterFactCheckingLevel = value;
    }
}