using System;
using Data;
using Gameplay;
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

    //Getters & Setters
    public DataMinerRunController CurrentRun => currentRun;
    public PlayerData CurrentPlayerData => currentPlayerData;

    public GameConstants Constants => constants;

    public void SetDataMinerRunController(DataMinerRunController dataMinerRunController) =>
        currentRun = dataMinerRunController;

    public void SubscribeUpdatePlayerInfo(UpdatePlayerInfo newUpdatePlayerInfo) =>
        updatePlayerInfo += newUpdatePlayerInfo;

    public void UnsubscribeUpdatePlayerInfo(UpdatePlayerInfo removeUpdatePlayerInfo) =>
        updatePlayerInfo -= removeUpdatePlayerInfo;
}
