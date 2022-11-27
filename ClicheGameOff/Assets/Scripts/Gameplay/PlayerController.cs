using Gameplay.Skills;
using Events;
using UnityEngine;

namespace Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        [SerializeField] private GameplayEventsSO gameplayEventsSO;

        [Header("Settings")]
        [SerializeField] private LayerMask floorMask;
        
        [Header("References")]
        [SerializeField] private GameObject displayObject;
        [SerializeField] private GameObject turnedOnObject;
        [SerializeField] private Camera mainCamera;

        //Private
        private Vector3 lastValidHit;
        private bool inContactWithTheGround;
        
        private readonly int MOUSE_LEFT = 0;

        #region MonoBehaviour Methods

        private void OnEnable()
        {
            if(gameplayEventsSO)
            {
                gameplayEventsSO.OnRunStarted += HandleOnRunStarted;
                gameplayEventsSO.OnRunEnded += HandleOnRunEnded;
            }
            else
            {
                Debug.LogError($"The gameplayEventsSO variable is null. ({this.GetType().ToString()}).");
            }
        }

        private void OnDisable()
        {
            if(gameplayEventsSO)
            {
                gameplayEventsSO.OnRunStarted -= HandleOnRunStarted;
                gameplayEventsSO.OnRunEnded -= HandleOnRunEnded;
            }
            else
            {
                Debug.LogError($"The gameplayEventsSO variable is null. ({this.GetType().ToString()}).");
            }
        }

        private void Update()
        {
            if(!GameManager.Instance.MainRunner.isRunning) return;

            var ray = mainCamera.ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast (ray, out var hit, 100, floorMask))
            {
                lastValidHit = hit.point;
                inContactWithTheGround = true;
            }
            else
            {
                inContactWithTheGround = false;
            }
            
            displayObject.SetActive(false);
            turnedOnObject.SetActive(false);
            turnedOnObject.transform.position = LastValidHit;
            displayObject.transform.position = LastValidHit;
            if (!inContactWithTheGround) return;
            
            if (Input.GetMouseButton(MOUSE_LEFT))
            {
                turnedOnObject.SetActive(true);
            }
            else
            {
                displayObject.SetActive(true);
            }

            UseSkills();
        }

        #endregion

        #region Skills Related

        private void UseSkills()
        {
            GameManager.Instance.CurrentPlayerData.Skills.ForEach(TryToUseSkill);
        }

        public void TryToUseSkill(GameSkill skill)
        {
            if (!skill.CheckKeyAndCoolDown()) return;
            StartCoroutine(skill.CoolDownCoroutine());
            GameManager.Instance.UseSkill(skill);
        }

        #endregion

        #region Event Handlers

        private void HandleOnRunStarted()
        {
            float increaseFactor = GameManager.Instance.CurrentPlayerData.PlayerRadius;
            displayObject.transform.localScale = Vector3.one * increaseFactor;
            turnedOnObject.transform.localScale = Vector3.one * increaseFactor;
        }

        private void HandleOnRunEnded()
        {
            displayObject.SetActive(false);
            turnedOnObject.SetActive(false);
        }

        #endregion

        public Vector3 LastValidHit => lastValidHit;
    }
}
