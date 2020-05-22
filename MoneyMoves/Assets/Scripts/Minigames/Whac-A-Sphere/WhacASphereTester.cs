using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhacASphereTester : MonoBehaviour
{
    public WhacASphereManager manager;
    WhacASphere leftGame;
    WhacASphere rightGame;
    public GameObject target;
    public float speed;
    float reactionTime;
    // Start is called before the first frame update
    void Start()
    {
        leftGame = manager.leftGame;
        rightGame = manager.rightGame;
        this.speed = manager.variables.testerSpeed;
        reactionTime = manager.variables.reactionTime;
    }

    public void ImitateAHuman()
    {
        StartCoroutine(ImitateHumanReactionTime());
    }
    IEnumerator ImitateHumanReactionTime()
    {
        yield return new WaitForSeconds(reactionTime);
        FindClosestSphere();
    }
    // Update is called once per frame
    void Update()
    {
        //FindClosestSphere();
        if (target == null)
        {
            return;
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
        if (this.transform.position == target.transform.position)
        {
            target.GetComponent<Sphere>().Hit();
            target = null;
            StopAllCoroutines();
            FindClosestSphere();
        }
    }

    void FindClosestSphere()
    {
        List<GameObject> leftSpheres = leftGame.activatedSpheres;
        List<GameObject> rightSpheres = rightGame.activatedSpheres;
        if (leftSpheres.Count <= 0 && rightSpheres.Count <= 0)
        {
            return;
        }
        Vector3 closestPosition = Vector3.zero;
        float closestDistance = 0;

        foreach(GameObject go in leftSpheres)
        {
            float distance = (go.transform.position - this.transform.position).magnitude;
            if (distance < closestDistance || closestDistance == 0)
            {
                closestDistance = distance;
                target = go;
            }
            else if (distance == closestDistance)
            {
                return;
            }
        }
        foreach (GameObject go in rightSpheres)
        {
            float distance = (go.transform.position - this.transform.position).magnitude;
            if (distance < closestDistance || closestDistance == 0)
            {
                closestDistance = distance;
                target = go;
            }
            else if (distance == closestDistance)
            {
                return;
            }
        }
    }
}
