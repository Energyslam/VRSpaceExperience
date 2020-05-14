using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToRandomCell : MonoBehaviour
{
    private GridManager grid;

    private Vector3 targetLocation = Vector3.zero;

    public enum State { IDLE, WALKING, ROTATING };

    [SerializeField]
    private State state = State.IDLE;

    [SerializeField]
    private GameObject target = null;

    [SerializeField]
    private float startingHeight = 0;

    [SerializeField]
    private LayerMask layer;

    [SerializeField]
    private float walkingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        grid = transform.root.GetComponent<GridManager>();
        transform.localPosition = Vector3.zero;
        MoveToCell();
    }

    private void MoveToCell()
    {
        int randX = Random.Range(1, grid.columns - 2);
        int randY = Random.Range(1, grid.rows - 2);
        StartCoroutine(WalkToCell(grid.grid[randX, randY], Random.Range(5, 8)));
    }

    private IEnumerator WalkToCell(GridCell cell, float delay)
    {
        yield return new WaitForSeconds(delay);
        target = cell.gameObject;
        targetLocation = target.transform.position;
        state = State.WALKING;
    }

    private void FixedUpdate()
    {
        if (transform.position == targetLocation)
        {
            targetLocation = Vector3.zero;
            target = null;
            state = State.IDLE;
            MoveToCell();
        }

        if (state == State.WALKING && target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetLocation, walkingSpeed);
            transform.LookAt(targetLocation);
        }
    }
}
