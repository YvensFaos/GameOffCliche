using System;
using Data;
using Gameplay;
using Progression;
using UnityEngine;

//Tick event delegate: called every tick.
public delegate void UpdatePlayerInfo(in PlayerData playerData);

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

    private void OnEnable()
    {
        //Later replace this with reading from the PlayerPrefs
        currentPlayerData = new PlayerData();
        if (currentPlayerData.HardDriveSize == 0)
        {
            currentPlayerData.HardDriveSize = constants.initialHardDriveSize;
        }
    }

    private PlayerData currentPlayerData;
    private DataMinerRunController currentRun;
    private UpdatePlayerInfo updatePlayerInfo;

    [SerializeField]
    private GameConstants constants;
    [SerializeField]
    private GameProgression gameProgress;

    public void ManagePlayerCollected(DataQualifier dataQualifier, int value)
    {
        switch (dataQualifier)
        {
            case DataQualifier.Good:
                ManagePlayerGoodData(value);
                updatePlayerInfo?.Invoke(currentPlayerData);
                break;
            case DataQualifier.Bad:
                ManagePlayerBadData(value);
                updatePlayerInfo?.Invoke(currentPlayerData);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dataQualifier), dataQualifier, null);
        }
    }
    
    public void ManagePlayerCollectedData(int goodData, int badData)
    {
        ManagePlayerGoodData(goodData);
        ManagePlayerBadData(badData);
        updatePlayerInfo?.Invoke(currentPlayerData);
    }

    private void ManagePlayerGoodData(int goodData)
    {
        currentPlayerData.GoodData += goodData;
    }

    private void ManagePlayerBadData(int badData)
    {
        currentPlayerData.BadData += badData;
    }

    public void UpgradeUnlocked(GameUpgrade upgrade)
    {   
        //Something! :)
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

    public void SetDataMinerRunController(DataMinerRunController dataMinerRunController) =>
        currentRun = dataMinerRunController;

    public void SubscribeUpdatePlayerInfo(UpdatePlayerInfo newUpdatePlayerInfo) =>
        updatePlayerInfo += newUpdatePlayerInfo;

    public void UnsubscribeUpdatePlayerInfo(UpdatePlayerInfo removeUpdatePlayerInfo) =>
        updatePlayerInfo -= removeUpdatePlayerInfo;
}
