using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UIElements;

public class ScrollingText : MonoBehaviour
{
    TextMeshProUGUI textfield;

    char[] text = new char[25];
    char[] charArray;

    int value = 20;
    int currentIndex = 0;
    int everySoManyFixedUpdates = 4;
    int currentEverySoMany = 0;

    void Start()
    {
        textfield = this.GetComponent<TextMeshProUGUI>();
        for (int i = 0; i < text.Length; i++)
        {
            text[i] = ' ';
        }
    }

    void Create()
    {
        for (int i = 0; i < text.Length; i++)
        {
            text[i] = ' ';
        }

        string newText = "Time: ";
        if (value < 10)
        {
            newText += "0";
        }
        newText += value;
        char[] newArray = newText.ToCharArray();
        int newArraySize = newArray.Length;

        int overflowSize = 0;
        if (currentIndex + newArraySize > text.Length)
        {
            overflowSize = (currentIndex + newArraySize) - text.Length;
        }

        for (int i = 0; i < newArraySize - overflowSize; i++)
        {
            text[currentIndex + i] = newArray[i];
        }

        for (int i = 0; i < overflowSize; i++)
        {
            text[i] = newArray[(newArray.Length - 1) - overflowSize + i];
        }

        textfield.text = text.ArrayToString();



        currentIndex++;
        if (currentIndex > text.Length -1) currentIndex = 0;
    }

    void FixedUpdate()
    {
        currentEverySoMany++;
        if (currentEverySoMany < everySoManyFixedUpdates)
        {
            return;
        }
        else if (currentEverySoMany >= everySoManyFixedUpdates)
        {
            currentEverySoMany = 0;

            value--;
            if (value - 1 < 0) value = 20;
            Create();
        }


    }
}
