using System;

namespace Progression
{
    [Serializable]
    public struct PublishedPaper
    {
        public string paperTitle;
        public int goodContent;
        public int badContent;

        public PublishedPaper(int goodContent, int badContent) : this(GameManager.Instance.Constants.GetRandomTitle(), goodContent, badContent)
        { }

        public PublishedPaper(string paperTitle, int goodContent, int badContent)
        {
            this.paperTitle = paperTitle;
            this.goodContent = goodContent;
            this.badContent = badContent;
        }
    }
}