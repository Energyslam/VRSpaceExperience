using UnityEngine;

public class CapsuleMovement : MonoBehaviour, IObservable
{
    private int? dockedAt;

    [SerializeField]
    private uint speed;

    [SerializeField]
    Shader shader;

    [SerializeField]
    private CapsuleAnimations topCap;

    [SerializeField]
    private Renderer botCap;

    private Vector3 originalPosition, direction;

    public enum State { IDLE, MOVINGTODOCK, ATDOCK, LEAVINGDOCK };

    private State state = State.IDLE;

    private bool isOpen = false;

    public void Subscribe()
    {
        CapsuleManager._instance.AddObservable(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Subscribe();
        MaterialPropertyBlock material = new MaterialPropertyBlock();
        material.SetColor("_BaseColor", Random.ColorHSV());
        botCap.SetPropertyBlock(material);
        topCap.gameObject.GetComponent<Renderer>().SetPropertyBlock(material);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.IDLE:
                break;
            case State.MOVINGTODOCK:
                MoveTowardsDock();
                break;
            case State.LEAVINGDOCK:
                MoveAwayFromDock();
                break;
            case State.ATDOCK:
                Docked();
                break;
        }

        // Rotate towards target location
        transform.forward = direction;
    }

    private void MoveTowardsDock()
    {
        if (transform.position == CapsuleManager._instance.dockingPlaces[(int)dockedAt].transform.position && state == State.MOVINGTODOCK)
        {
            state = State.ATDOCK;
            Invoke("LeaveDock", Random.Range(5, 15));
            topCap.Animate(true); // Animation plays that opens the capsule
        }

        transform.position = Vector3.MoveTowards(transform.position, CapsuleManager._instance.dockingPlaces[(int)dockedAt].transform.position, speed * Time.deltaTime);
        direction = CapsuleManager._instance.dockingPlaces[(int)dockedAt].transform.position - transform.position;
    }

    private void MoveAwayFromDock()
    {
        if (transform.position == originalPosition && state == State.LEAVINGDOCK)
        {
            state = State.IDLE;
            Subscribe();
        }

        transform.position = Vector3.MoveTowards(transform.position, originalPosition, speed * Time.deltaTime);
        direction = originalPosition - transform.position;
    }

    private void Docked()
    {
        // Look to middle
        direction = CapsuleManager._instance.dockingPlaces[(int)dockedAt].transform.forward;
        direction = new Vector3(direction.x, 0.0f, direction.z);
    }

    public void GoToPlayer(int? dock)
    {
        dockedAt = dock;
        originalPosition = transform.position;
        state = State.MOVINGTODOCK;
    }

    public void LeaveDock()
    {
        topCap.Animate(false); // Animation plays that closes the capsule
        state = State.LEAVINGDOCK;
        CapsuleManager._instance.RemoveObservable(gameObject);
        CapsuleManager._instance.LeftPlayer(gameObject, dockedAt);
        dockedAt = null;
    }
}
