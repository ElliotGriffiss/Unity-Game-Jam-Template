using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasAnimator : MonoBehaviour
{
    public Image TargetImage;
    public Sprite[] Images;

    public float FrameRate = 0.1f;

    private float currentFrameTime = 0;
    private byte currentIndex = 0;

    private void Update()
    {
        if (currentFrameTime < FrameRate)
        {
            currentFrameTime += Time.fixedDeltaTime;
        }
        else
        {
            currentIndex++;
            currentFrameTime = 0;

            if (currentIndex >= Images.Length)
            {
                currentIndex = 0;
            }

            TargetImage.sprite = Images[currentIndex];
        }
    }
}
