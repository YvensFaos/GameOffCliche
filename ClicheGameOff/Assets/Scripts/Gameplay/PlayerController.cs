using System;
using UnityEngine;

namespace Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private LayerMask floorMask;
        
        [Header("References")]
        [SerializeField]
        private GameObject displayObject;
        [SerializeField]
        private GameObject turnedOnObject;
        [SerializeField] 
        private Camera mainCamera;

        //Private
        private Vector3 currentHit;
        private bool inContactWithTheGround;
        
        private readonly int MOUSE_LEFT = 0;
        
        private void Update()
        {
            var ray = mainCamera.ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast (ray, out RaycastHit hit, 100, floorMask))
            {
                currentHit = hit.point;
                inContactWithTheGround = true;
            }
            else
            {
                inContactWithTheGround = false;
            }
            
            displayObject.SetActive(false);
            turnedOnObject.SetActive(false);
            if (!inContactWithTheGround) return;
            
            if (Input.GetMouseButton(MOUSE_LEFT))
            {
                turnedOnObject.SetActive(true);
                turnedOnObject.transform.position = currentHit;
            }
            else
            {
                displayObject.SetActive(true);
                displayObject.transform.position = currentHit;
            }
        }
    }
}
