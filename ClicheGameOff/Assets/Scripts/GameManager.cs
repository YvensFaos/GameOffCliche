using System;
using System.Collections.Generic;
using Data;
using Gameplay;
using Gameplay.Skills;
using GameUI;
using Progression;
using UnityEngine;

//Tick event delegate: called every tick.
public delegate void UpdatePlayerInfoDelegate(in PlayerData playerData);
public delegate void UseSkillDelegate(GameSkill skill, in PlayerController playerController);

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance => instance;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Debug.Log("Game Manager Started!");
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void DestroyInstance()
    {
        Destroy(gameObject);
        instance = null;
    }
    #endregion
    
    [Header("References")]
    [SerializeField]
    private GameConstants constants;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private PlayerData currentPlayerData;
    [SerializeField]
    private GameDialogController dialogController;
    [SerializeField]
    private DataSpawner mainSpawner;
    [SerializeField] 
    private List<UpgradeUIController> upgradeUIControllers;

    [Header("All Upgrades")] 
    [SerializeField]
    private List<GameUpgrade> gameUpgrades;
    [SerializeField]
    private List<GameSkill> gameSkills;
    [SerializeField]
    private List<GamePublication> gamePublications;
    
    private DataMinerRunController mainRunner;
    
    //Delegates
    private UpdatePlayerInfoDelegate updatePlayerInfoDelegate;
    private UseSkillDelegate useSkillDelegate;

    private const string PlayerPrefString = "playerData";

    private void OnEnable()
    {
        Load();
    }
    
    #region Persistence
    private void Load()
    {
        //Update to use names for different save files
        var playerPrefsData = PlayerPrefs.GetString(PlayerPrefString);
        if (playerPrefsData is null or "")
        {
            CreateNewSave();
            //First run
            dialogController.StartTutorial();
        }
        else
        {
            currentPlayerData = PlayerData.InitializeFromJson(playerPrefsData);
            currentPlayerData.UnlockCurrentProgress();
        }
    }

    private void CreateNewSave()
    {
        currentPlayerData = new PlayerData();
        if (currentPlayerData.HardDriveSize == 0)
        {
            currentPlayerData.HardDriveSize = constants.initialHardDriveSize;
        }
        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetString(PlayerPrefString, currentPlayerData.ToJson());
    }

    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey(PlayerPrefString);
    }
    #endregion
    
    public void ManagePlayerCollected(DataQualifier dataQualifier, int value)
    {
        switch (dataQualifier)
        {
            case DataQualifier.Good:
                ManagePlayerGoodData(value);
                updatePlayerInfoDelegate?.Invoke(currentPlayerData);
                break;
            case DataQualifier.Bad:
                ManagePlayerBadData(value);
                updatePlayerInfoDelegate?.Invoke(currentPlayerData);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dataQualifier), dataQualifier, null);
        }
        Save();
    }
    
    public void ManagePlayerCollectedData(int goodData, int badData)
    {
        ManagePlayerGoodData(goodData);
        ManagePlayerBadData(badData);
        updatePlayerInfoDelegate?.Invoke(currentPlayerData);
        Save();
    }

    private void ManagePlayerGoodData(int goodData)
    {
        currentPlayerData.GoodData += goodData;
    }

    private void ManagePlayerBadData(int badData)
    {
        currentPlayerData.BadData += badData;
    }

    public void UseSkill(GameSkill skill)
    {
        useSkillDelegate?.Invoke(skill, player);
    }
    
    public void UpgradeUnlock(GameUpgrade upgrade, int level)
    {   
        currentPlayerData.SetUpgradeLevel(upgrade, level);
        updatePlayerInfoDelegate?.Invoke(currentPlayerData);
        Save();
    }

    public void OpenUpgrade(GameUpgrade upgrade)
    {
        upgradeUIControllers.ForEach(upgradeUIController =>
        {
            upgradeUIController.AddUpgrade(upgrade);
        });
    }

    public PublishedPaper PublishNewPaper(GamePublication publication, int goodData, int badData)
    {
        var paper = new PublishedPaper(goodData, badData, publication.name);
        currentPlayerData.Papers.Add(paper);
        return paper;
    }

    public float GetCurrentUpgradeValue(GameUpgrade upgrade)
    {
        return upgrade.ValueCurve.EvaluateAtLevel(currentPlayerData.GetUpgradeLevel(upgrade), out _);
    }

    public bool CheckPlayerData(DataQualifier dataQualifier, int value)
    {
        return dataQualifier switch
        {
            DataQualifier.Good => CheckPlayerGoodData(value),
            DataQualifier.Bad => CheckPlayerBadData(value),
            _ => throw new ArgumentOutOfRangeException(nameof(dataQualifier), dataQualifier, null)
        };
    }
    private bool CheckPlayerGoodData(int goodData) => currentPlayerData.GoodData >= goodData;
    private bool CheckPlayerBadData(int badData) => currentPlayerData.BadData >= badData;

    //Getters & Setters
    public DataMinerRunController MainRunner => mainRunner;
    public PlayerData CurrentPlayerData => currentPlayerData;
    public GameConstants Constants => constants;
    public PlayerController Player => player;
    public List<GameUpgrade> GameUpgrades => gameUpgrades;
    public List<GameSkill> GameSkills => gameSkills;
    public GameDialogController DialogController => dialogController;
    public List<GamePublication> GamePublications => gamePublications;
    public DataSpawner MainSpawner => mainSpawner;

    public GameUpgrade GetUpgradeByName(string upgradeName)
    {
        return GameUpgrades.Find(upgrade => upgrade.GetName().Equals(upgradeName));
    }

    public GameSkill GetSkillByName(string skillName)
    {
        return GameSkills.Find(skill => skill.GetName().Equals(skillName));
    }

    public GamePublication GetPublicationByName(string publicationName)
    {
        return GamePublications.Find(publication => publication.name.Equals(publicationName));
    }

    public void SetDataMinerRunController(DataMinerRunController dataMinerRunController) =>
        mainRunner = dataMinerRunController;

    public void SetDialogueController(GameDialogController gameDialogController) =>
        dialogController = gameDialogController;
    
    public void SetPlayerData(PlayerData playerData)
    {
        currentPlayerData = playerData;
        updatePlayerInfoDelegate?.Invoke(currentPlayerData);
    } 

    public void SubscribeUpdatePlayerInfo(UpdatePlayerInfoDelegate newUpdatePlayerInfoDelegate) =>
        updatePlayerInfoDelegate += newUpdatePlayerInfoDelegate;

    public void UnsubscribeUpdatePlayerInfo(UpdatePlayerInfoDelegate removeUpdatePlayerInfoDelegate) =>
        updatePlayerInfoDelegate -= removeUpdatePlayerInfoDelegate;

    public void SubscribeUseSkillDelegate(UseSkillDelegate newUseSkillDelegate) =>
        useSkillDelegate += newUseSkillDelegate;

    public void UnsubscribeUseSkillDelegate(UseSkillDelegate removeUseSkillDelegate) =>
        useSkillDelegate -= removeUseSkillDelegate;
}
