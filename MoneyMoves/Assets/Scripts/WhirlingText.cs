using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WhirlingText : MonoBehaviour
{
    public float spinSpeed;
    public float decreaseSizeSpeed;
    float currentLerp = 1;
    float currentSize = 1;
    bool removeText;

    private void Start()
    {
        spinSpeed = 25f;
        decreaseSizeSpeed = 0.001f;
        removeText = true;
    }
    void Update()
    {
        if (removeText)
        {
            this.transform.localEulerAngles += new Vector3(0, 0, spinSpeed);
            if (this.transform.localScale.x > 0f)
            {
                currentLerp -= decreaseSizeSpeed;
                currentSize = Mathf.Lerp(0, currentSize, currentLerp);
                this.transform.localScale *= currentSize;
            }
            else if (this.transform.localScale.x <= 0f)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
