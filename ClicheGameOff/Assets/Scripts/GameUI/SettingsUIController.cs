using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUI
{
    public class SettingsUIController : MonoBehaviour
    {
        public void ResetData()
        {
            //Delete the save
            GameManager.DeleteSave();
            GameManager.Instance.DestroyInstance();
            
            //Reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
