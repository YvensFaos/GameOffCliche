using System.Linq;
using TMPro;
using UnityEngine;

namespace Utils
{
    /**
    This script changes a UI Text to display the current and average FPS.
    */
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;
    
        [SerializeField]
        private int averageSample;
    
        private float[] averageValues;
        private int averageIndex;
    
        public void Awake()
        {
            if (text == null)
            {
                text = GetComponent<TextMeshProUGUI>();    
            }
            
            averageValues = new float[averageSample];
            averageIndex = 0;
            averageValues[averageIndex] = 0;
        }

        public void Update()
        {
            averageValues[averageIndex] = Time.deltaTime;
            averageIndex = (++averageIndex) % averageSample;
            var fps = 1.0f / Time.deltaTime;
            var averageFps = 1.0f / (averageValues.Sum(value => value) / averageSample);
            text.text = "FPS: " + fps.ToString("0.0") + " AVG: " + averageFps.ToString("0.0");
        }
    }
}
