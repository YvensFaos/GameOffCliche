using System.Collections;
using Dialog;
using UnityEngine;
using Yarn;
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
        StartCoroutine(DialogueStart(dialog));
    }

    private IEnumerator DialogueStart(GameDialog dialog)
    {
        dialogueRunner.gameObject.SetActive(true);
        yield return null;
        dialogueRunner.SetProject(dialog.yarnProject);
        yield return null;
        dialogueRunner.Dialogue.SetNode(dialog.startNode);
        dialogueRunner.Stop();
        yield return null;
        try
        {
            dialogueRunner.StartDialogue(dialog.startNode);
        }
        catch (DialogueException exception)
        {
            Debug.LogWarning(exception.Message);
            Debug.LogWarning(exception.HelpLink);
        }
    }
}