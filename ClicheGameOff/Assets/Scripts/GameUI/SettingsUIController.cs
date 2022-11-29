using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUI
{
    public class SettingsUIController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField] 
        private List<GameObject> reflectionObjects;
        [SerializeField] 
        private GameObject debugCanvas;
        
        public void ResetData()
        {
            //Delete the save
            GameManager.DeleteSave();
            GameManager.Instance.DestroyInstance();
            
            //Reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ToggleAudio()
        {
            audioSource.enabled = !audioSource.enabled;
        }

        public void ToggleDebug()
        {
            debugCanvas.SetActive(!debugCanvas.activeSelf);
        }

        public void ToggleReflections()
        {
            reflectionObjects.ForEach(o => o.SetActive(!o.activeSelf));
        }
    }
}
