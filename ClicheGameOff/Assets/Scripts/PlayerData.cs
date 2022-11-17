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
    [Header("Player Data")]
    [SerializeField]
    private int goodData;
    [SerializeField]
    private int badData;
    [SerializeField] 
    private int hardDriveSize;
    [SerializeField] 
    private float stunResistance;
    [SerializeField]
    private List<GameUpgradeLevelPair> upgrades;
    [SerializeField] 
    private List<GameSkill> skills;
    [SerializeField] 
    private List<PublishedPaper> papers;
    
    [Header("Publication Data")]
    [SerializeField]
    private int publicationProgress;
    [SerializeField]
    private int goodDataUsedSoFar;
    [SerializeField]
    private int badDataUsedSoFar;
    
    [SerializeField, HideInInspector]
    private List<GameUpgradeNameLevelPair> upgradeNames;
    [SerializeField, HideInInspector]
    private List<string> skillNames;

    public PlayerData() : this(0, 0, 0, 0.0f, 0, 0, 0)
    { }

    public PlayerData(int goodData, int badData, int hardDriveSize, float stunResistance, int publicationProgress, int goodDataUsedSoFar, int badDataUsedSoFar)
    {
        this.goodData = goodData;
        this.badData = badData;
        if (hardDriveSize == 0 && GameManager.Instance != null)
        {
            this.hardDriveSize = GameManager.Instance.Constants.initialHardDriveSize;    
        }
        else
        {
            this.hardDriveSize = hardDriveSize;    
        }

        this.stunResistance = stunResistance;
        this.publicationProgress = publicationProgress;
        this.goodDataUsedSoFar = goodDataUsedSoFar;
        this.badDataUsedSoFar = badDataUsedSoFar;
        InitializeUpgrades();
        upgradeNames = new List<GameUpgradeNameLevelPair>();
        InitializeSkills();
        skillNames = new List<string>();
        
        Papers = new List<PublishedPaper>();
    }

    public static PlayerData InitializeFromJson(string json)
    {
        var playerData = JsonUtility.FromJson<PlayerData>(json);
        playerData.InitializeUpgrades();
        playerData.upgradeNames.ForEach(pair =>
        {
            var gameUpgradeLevelPair = pair.ToGameUpgradeLevelPair();
            playerData.upgrades.Add(gameUpgradeLevelPair);
        });
        playerData.InitializeSkills();
        playerData.skillNames.ForEach(skillName =>
        {
            var skill = GameManager.Instance.GetSkillByName(skillName);
            playerData.skills.Add(skill);
        });
        if (playerData.HardDriveSize == 0 && GameManager.Instance != null)
        {
            playerData.hardDriveSize = GameManager.Instance.Constants.initialHardDriveSize;    
        }
        
        return playerData;
    }

    public void InitializeUpgradesAndSkills()
    {
        upgrades.ForEach(gameUpgradeLevelPair =>
        {
            gameUpgradeLevelPair.UpgradeUnlock();    
        });
        skills.ForEach(skill => skill.Reset());
        papers.ForEach(paper =>
        {
            var publication = GameManager.Instance.GetPublicationByName(paper.publicationName);
            // publication.Up
        });
    }
    
    public void InitializeUpgrades()
    {
        upgrades = new List<GameUpgradeLevelPair>();
    }

    public void InitializeSkills()
    {
        skills = new List<GameSkill>();
    }
    
    public void AddSkill(GameSkill skill)
    {
        if (skills.Contains(skill)) return;
        skills.Add(skill);
    }

    public bool HasSkill(GameSkill skill) => skills.Contains(skill);

    public int GetUpgradeLevel(GameUpgrade upgrade)
    {
        var pair = Upgrades.Find(pair => pair.One.Equals(upgrade));
        return pair?.Two ?? -1;
    }

    public bool HasPublished(GamePublication publication)
    {
        var found = Papers.FindIndex(paper => paper.publicationName.Equals(publication.name));
        return found >= 0;
    }

    public bool IsUpgradeMaxedOut(GameUpgrade upgrade)
    {
        var pair = Upgrades.Find(pair => pair.One.Equals(upgrade));
        return pair != null && pair.IsMaxedOut();
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
    
    public bool IncreaseUpgradeLevel(GameUpgrade upgrade)
    {
        var pair = Upgrades.Find(pair => pair.One.Equals(upgrade));
        if (pair != null)
        {
            return pair.IncreaseLevel();
        }

        Upgrades.Add(new GameUpgradeLevelPair(upgrade, 1));
        return false;
    }

    public string ToJson()
    {
        //Update serialize lists
        upgradeNames = new List<GameUpgradeNameLevelPair>();
        upgrades.ForEach(upgradePair =>
        {
            upgradeNames.Add(new GameUpgradeNameLevelPair(upgradePair.One.GetName(), upgradePair.Two));
        });
        skillNames = new List<string>();
        skills.ForEach(skill =>
        {
            skillNames.Add(skill.GetName());
        });
        return JsonUtility.ToJson(this);
    }
    
    //Getters & Setter
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

    public List<GameSkill> Skills
    {
        get => skills;
        set => skills = value;
    }
    
    public List<GameUpgradeLevelPair> Upgrades => upgrades;

    public int PublicationProgress
    {
        get => publicationProgress;
        set => publicationProgress = value;
    }

    public int GoodDataUsedSoFar
    {
        get => goodDataUsedSoFar;
        set => goodDataUsedSoFar = value;
    }

    public int BadDataUsedSoFar
    {
        get => badDataUsedSoFar;
        set => badDataUsedSoFar = value;
    }

    public List<PublishedPaper> Papers
    {
        get => papers;
        set => papers = value;
    }

    public float StunResistance
    {
        get => stunResistance;
        set => stunResistance = value;
    }
}