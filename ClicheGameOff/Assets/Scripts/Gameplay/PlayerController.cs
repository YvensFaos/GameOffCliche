using System.Collections.Generic;
using System.Linq;
using Gameplay.Skills;
using UnityEngine;

namespace Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private LayerMask floorMask;
        
        [Header("References")]
        [SerializeField] private GameObject displayObject;
        [SerializeField] private GameObject turnedOnObject;
        [SerializeField] private Camera mainCamera;

        [Header("Skills")] 
        [SerializeField] private List<GameSkill> skills;

        //Private
        private Vector3 lastValidHit;
        private bool inContactWithTheGround;
        
        private readonly int MOUSE_LEFT = 0;

        private void Update()
        {
            var ray = mainCamera.ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast (ray, out RaycastHit hit, 100, floorMask))
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

        private void UseSkills()
        {
            skills.ForEach(TryToUseSkill);
        }

        public void TryToUseSkill(GameSkill skill)
        {
            if (!skill.CheckKeyAndCoolDown()) return;
            StartCoroutine(skill.CoolDownCoroutine());
            GameManager.Instance.UseSkill(skill);
        }

        public void TryToAddSkill(GameSkill skill)
        {
            if (!skills.Contains(skill))
            {
                skills.Add(skill);
            }
        }
        
        public Vector3 LastValidHit => lastValidHit;
    }
}
