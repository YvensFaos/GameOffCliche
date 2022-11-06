using System;
using Data;
using Gameplay;
using Gameplay.Skills;
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

        instance = this;
        DontDestroyOnLoad(this);
    }
    #endregion
    
    [Header("References")]
    [SerializeField]
    private GameConstants constants;
    [SerializeField]
    private GameProgression gameProgress;
    [SerializeField] 
    private GameUpgradeUnlocker gameUpgradeUnlocker;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private PlayerData currentPlayerData;

    private DataMinerRunController currentRun;
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
        gameProgress.Reset();
        if (playerPrefsData is null or "")
        {
            currentPlayerData = new PlayerData();
            if (currentPlayerData.HardDriveSize == 0)
            {
                currentPlayerData.HardDriveSize = constants.initialHardDriveSize;
            }
        }
        else
        {
            currentPlayerData = JsonUtility.FromJson<PlayerData>(playerPrefsData);
            var playerUpgrades = currentPlayerData.Upgrades;
            playerUpgrades.ForEach(pair =>
            {
                gameProgress.SetGameUpgradeLevel(pair.One, pair.Two);
                pair.One.UpgradeUnlock(pair.Two);
            });
            var playerSkills = currentPlayerData.Skills;
            playerSkills.ForEach(skill =>
            {
                skill.Reset();
                player.TryToAddSkill(skill);
            });
        }
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
    
    public void UpgradeUnlocked(GameUpgrade upgrade, int level)
    {   
        currentPlayerData.SetUpgradeLevel(upgrade, level);
        gameUpgradeUnlocker.UnlockEvent(upgrade, currentPlayerData);
        updatePlayerInfoDelegate?.Invoke(currentPlayerData);
        Save();
    }

    public float GetCurrentUpgradeValue(GameUpgrade upgrade)
    {
        return upgrade.ValueCurve.EvaluateAtLevel(currentPlayerData.GetUpgradeLevel(upgrade), out _);
    }

    public bool CheckPlayerData(DataQualifier dataQualifier, int value)
    {
        switch (dataQualifier)
        {
            case DataQualifier.Good:
                return CheckPlayerGoodData(value);
            case DataQualifier.Bad:
                return CheckPlayerBadData(value);
            default:
                throw new ArgumentOutOfRangeException(nameof(dataQualifier), dataQualifier, null);
        }
    }
    private bool CheckPlayerGoodData(int goodData) => currentPlayerData.GoodData >= goodData;
    private bool CheckPlayerBadData(int badData) => currentPlayerData.BadData >= badData;

    //Getters & Setters
    public DataMinerRunController CurrentRun => currentRun;
    public PlayerData CurrentPlayerData => currentPlayerData;
    public GameConstants Constants => constants;
    public GameProgression GameProgress => gameProgress;
    public PlayerController Player => player;

    public void SetDataMinerRunController(DataMinerRunController dataMinerRunController) =>
        currentRun = dataMinerRunController;

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
