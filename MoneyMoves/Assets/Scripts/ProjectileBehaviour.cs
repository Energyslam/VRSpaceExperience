using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colliding with " + other.gameObject.name);
        if (other.CompareTag("Collideable"))
        {
            other.GetComponent<ICollisionBehaviour>().SolveCollision();
            Destroy(this.gameObject);
        }
        if (other.name == "arrowA")
        {
            GameManager.Instance.platform.ChangeStateToA();
        }
        if (other.name == "arrowB")
        {
            GameManager.Instance.platform.ChangeStateToB();
        }
    }
}
