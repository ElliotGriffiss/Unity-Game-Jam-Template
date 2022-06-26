using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShrimpCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gui;
    private int Count = 0;

    private void Start()
    {
        gui.text = "x" + Count;
    }

    public void UpdateCounter()
    {
        Count++;
        gui.text = "x" + Count;
    }
}
