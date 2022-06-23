using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Animator animator;
    [SerializeField] private Image image;
    [SerializeField] private Button NextButton;

    [SerializeField] private AudioSource talking;

    private string sceneName;

    private Queue<string> sentences;
    private string Sentance;
    private bool SkipPressed = true;

    void Start()
    {
        sentences = new Queue<string>();
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        talking = GetComponent<AudioSource>();
        NextButton.interactable = false;
    }

    public void StartDialogue (DialogueData dialogue)
    {
        Debug.Log("starting dialogue");

        sentences = new Queue<string>();
        NextButton.interactable = true;

        foreach (string sentence in dialogue.sentances)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (SkipPressed == false)
        {
            SkipPressed = true;
            StopAllCoroutines();
            dialogueText.text = Sentance;
            talking.Stop();
        }
        else
        {
            SkipPressed = false;

            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            Sentance = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentenceCo(Sentance));
        }
    }

    IEnumerator TypeSentenceCo(string sentence)
    {
        talking.Play();
        dialogueText.text = "";        
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;          
            yield return new WaitForSeconds(0.03f);

        }
        talking.Stop();
        SkipPressed = true;
    }

    void EndDialogue()
    {
        NextButton.interactable = false;
        StartCoroutine(FadeCo(sceneName));
        Debug.Log("end");
    }
    IEnumerator FadeCo(string sceneName)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        animator.SetBool("FadeIn", true);
        yield return new WaitUntil(() => image.color.a == 1);
        if(sceneName == "Cutscene 1")
        {
            SceneManager.LoadScene("Cutscene 2");
        }
        else if (sceneName == "Cutscene 2" || sceneName == "Cutscene 3")
        {
            SceneManager.LoadScene("Test Scene");
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("Test Scene");
        }
    }
}
