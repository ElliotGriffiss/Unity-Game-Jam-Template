using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [SerializeField] private string nextSceneName;

    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Animator animator;
    [SerializeField] private Image image;
    [SerializeField] private Button nextButton;

    [SerializeField] private AudioSource talking;

    private Queue<string> sentences;
    private string Sentance;
    private bool SkipPressed = true;

    void Start()
    {
        sentences = new Queue<string>();
        Scene currentScene = SceneManager.GetActiveScene();
        talking = GetComponent<AudioSource>();
        nextButton.interactable = false;
    }

    public void StartDialogue (DialogueData dialogue)
    {
        sentences = new Queue<string>();
        nextButton.interactable = true;

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
        nextButton.interactable = false;
        StartCoroutine(FadeCo());
    }
    IEnumerator FadeCo()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        animator.SetBool("FadeIn", true);

        yield return new WaitUntil(() => image.color.a == 1);
        
        SceneManager.LoadScene(nextSceneName);
    }

    /// <summary>
    /// Handles Skip Functionality.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
