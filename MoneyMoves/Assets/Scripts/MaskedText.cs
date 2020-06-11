using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskedText : MonoBehaviour
{
    [SerializeField] GameObject textObject;
    [SerializeField] float scrollSpeed;
    [SerializeField] float resetPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        textObject.transform.localPosition += new Vector3(scrollSpeed, 0, 0);
        if (textObject.transform.localPosition.x < resetPoint)
        {
            textObject.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}
