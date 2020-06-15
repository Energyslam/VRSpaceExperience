using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalScoreObject : MonoBehaviour
{
    void Update()
    {
        this.transform.LookAt(GameManager.Instance.player.transform.position + new Vector3(0, 4, 0));
    }
}
