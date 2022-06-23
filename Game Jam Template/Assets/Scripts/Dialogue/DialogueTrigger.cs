using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData dialogue;

    private void Start()
    {
        StartCoroutine(WaitForEntry());
    }

    private IEnumerator WaitForEntry()
    {
        yield return new WaitForSeconds(1f);
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
