using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceRubble : MonoBehaviour
{
    [SerializeField]
    private GameObject floatingPoints;

    private int value = 1;

    [SerializeField]
    private float pointMultiplier = 1.0f;

    private void Start()
    {
        float scaleMultiplier = GetComponent<RandomScaleOnStartup>().maxScale.x / transform.localScale.x;

        value = Mathf.RoundToInt(pointMultiplier * scaleMultiplier * GetComponent<MeteorMovement>().moveSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ProjectileBehaviour>() != null)
        {
            FloatingPoints pts = Instantiate(floatingPoints, this.transform.position, Quaternion.identity).GetComponent<FloatingPoints>();
            pts.points = value;
            GameManager.Instance.AddScore(value);
        }
    }
}
