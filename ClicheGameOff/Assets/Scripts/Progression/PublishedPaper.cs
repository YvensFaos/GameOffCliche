using System;

namespace Progression
{
    [Serializable]
    public struct PublishedPaper
    {
        public string paperTitle;
        public int goodContent;
        public int badContent;
        public string publicationName;

        public PublishedPaper(int goodContent, int badContent, string publicationName) : this(GameManager.Instance.Constants.GetRandomTitle(), goodContent, badContent, publicationName)
        { }

        public PublishedPaper(string paperTitle, int goodContent, int badContent, string publicationName)
        {
            this.paperTitle = paperTitle;
            this.goodContent = goodContent;
            this.badContent = badContent;
            this.publicationName = publicationName;
        }
    }
}