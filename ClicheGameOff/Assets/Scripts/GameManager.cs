using Gameplay;
using UnityEngine;

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

    private DataMinerRunController currentRun;
    public DataMinerRunController CurrentRun => currentRun;
    public void SetDataMinerRunController(DataMinerRunController dataMinerRunController) =>
        currentRun = dataMinerRunController;
    
}
