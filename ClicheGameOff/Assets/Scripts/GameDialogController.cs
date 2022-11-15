using Dialog;
using UnityEngine;
using Yarn.Unity;

public class GameDialogController : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;

    [SerializeField] private GameDialog tutorialDialog;
    private void Start()
    {
        GameManager.Instance.SetDialogueController(this);
    }

    public void StartTutorial()
    {
        StartDialog(tutorialDialog);
    }

    public void StartDialog(GameDialog dialog)
    {
        dialogueRunner.yarnProject = dialog.yarnProject;
        dialogueRunner.startNode = dialog.startNode;
        dialogueRunner.startAutomatically = true;
        dialogueRunner.gameObject.SetActive(true);
    }
}