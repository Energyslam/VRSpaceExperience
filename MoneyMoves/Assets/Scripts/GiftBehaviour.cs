using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBehaviour : ICollisionBehaviour
{
    [SerializeField] GameObject fireworkGO;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SolveCollision()
    {
        if (!fireworkGO.activeInHierarchy)
        {
            fireworkGO.SetActive(true);
            fireworkGO.transform.parent = null;
            Destroy(this.gameObject);
            GameManager.Instance.AddScore(10);
        }
    }
}
