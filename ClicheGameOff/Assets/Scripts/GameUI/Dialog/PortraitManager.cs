using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace GameUI.Dialog
{
    public class PortraitManager : MonoBehaviour
    {
        [SerializeField] private List<Portrait> portraits;
        [SerializeField] private Image leftPortrait;
        [SerializeField] private Image rightPortrait;
        
        private static readonly Color DisabledColor = new Color(0, 0, 0, 0);
        
        [YarnCommand("displayLeftPortrait")]
        public void DisplayLeftPortrait(string character) {
            DisplayPortrait(leftPortrait, GetPortraitByName(character));
        }

        [YarnCommand("displayRightPortrait")]
        public void DisplayRightPortrait(string character) {
            DisplayPortrait(rightPortrait, GetPortraitByName(character));
        }

        [YarnCommand("removeLeftPortrait")]
        public void RemoveLeftPortrait()
        {
            DisplayPortrait(leftPortrait, null);
        }
        
        [YarnCommand("removeRightPortrait")]
        public void RemoveRightPortrait()
        {
            DisplayPortrait(rightPortrait, null);
        }
        
        private static void DisplayPortrait(Image display, Sprite sprite)
        {
            if (sprite == null)
            {
                display.color = DisabledColor;
            }
            else
            {
                display.sprite = sprite;    
                display.color = Color.white;
            }
        }

        private Sprite GetPortraitByName(string spriteName) =>
            portraits.Find(portrait => portrait.IsPortrait(spriteName))?.sprite;
    }
}
