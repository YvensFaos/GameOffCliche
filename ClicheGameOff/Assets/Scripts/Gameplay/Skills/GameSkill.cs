using System.Collections;
using UnityEngine;

namespace Gameplay.Skills
{
    [CreateAssetMenu(fileName = "Game Skill", menuName = "Cliche/Game Skill", order = 0)]
    public class GameSkill : ScriptableObject
    {
        public string GetName() => name;
        public float cooldown;
        public KeyCode shortCut = KeyCode.Q;

        private bool isOnCooldown;
        
        public bool CheckKeyAndCoolDown()
        {
            if (isOnCooldown) return false;
            var input = Input.GetKeyUp(shortCut);
            if (input)
            {
                isOnCooldown = true;    
            }
            return input;
        }

        public IEnumerator CoolDownCoroutine()
        {
            yield return new WaitForSeconds(cooldown);
            isOnCooldown = false;
        }

        public void Reset()
        {
            isOnCooldown = false;
        }
    }
}