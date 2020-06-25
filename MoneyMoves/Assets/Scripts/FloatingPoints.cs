using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingPoints : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float fadeDuration = 2;
    public int points = 10;
    [SerializeField] TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.LookAt(GameManager.Instance.platform.transform);
        text.CrossFadeAlpha(0f, fadeDuration, false);
        Destroy(this.gameObject, fadeDuration);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += Vector3.up * Time.deltaTime * speed;
        text.text = "+" + points + " points";
    }
}
