using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [Space]
    [SerializeField] private Animator animator;
    [SerializeField] private Image image;

    public void LoadNewScene()
    {
        StartCoroutine(FadeCo());
    }

    IEnumerator FadeCo()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        animator.SetBool("FadeIn", true);

        yield return new WaitUntil(() => image.color.a == 1);

        SceneManager.LoadScene(nextSceneName);
    }

}
