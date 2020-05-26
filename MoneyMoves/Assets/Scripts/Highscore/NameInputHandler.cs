using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class NameInputHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameField;
    // Start is called before the first frame update
    void Start()
    {
        nameField.text = "";
    }

    public void AddLetter(string letter)
    {
        if (nameField.text.Length < 15)
        {
            nameField.text += letter;
        }
    }
}
