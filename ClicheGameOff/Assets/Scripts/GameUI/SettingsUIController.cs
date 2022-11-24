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
            //Reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
