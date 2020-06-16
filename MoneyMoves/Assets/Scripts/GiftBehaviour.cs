using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBehaviour : MonoBehaviour
{
    public bool isGrabbable;
    [SerializeField] float speed = 1f;
    [SerializeField] float wobbleMagnitude = 1f;
    [SerializeField] GameObject fireworkGO, grabber, deliverPoint, floatingPoints;

    public StaticCapsule attachedStatic;
    bool canMove;
    Vector3 startPos;

    [SerializeField]
    private AudioSource explodeSFX;

    void Start()
    {
        deliverPoint = GameManager.Instance.DeliverPoint;
        startPos = this.transform.position;
        //this.GetComponent<Animator>().SetFloat("BobSpeed", Random.Range(0.5f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, deliverPoint.transform.position, speed * Time.deltaTime);
        }
        if (this.transform.position == deliverPoint.transform.position)
        {
            canMove = false;
            grabber.SetActive(false);
            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<Rigidbody>().isKinematic = false;
        }
        Wobble();
    }

    public void Wobble()
    {
        this.transform.position = startPos + transform.up * Mathf.Sin(Time.time * speed) * wobbleMagnitude;
    }

    public void StartMoving()
    {
        canMove = true;
    }
    public void SolveCollision()
    {
        if (!isGrabbable)
        {
            fireworkGO.SetActive(true);
            fireworkGO.transform.parent = null;
            Destroy(fireworkGO, 6f);
            attachedStatic.UpdateGifts(this.gameObject);
            explodeSFX.clip = GameManager.Instance.GetOrchestraClip();
            explodeSFX.Play();
            GameManager.Instance.UpdateOrchestra();
            FloatingPoints pts = Instantiate(floatingPoints, this.transform.position, Quaternion.identity).GetComponent<FloatingPoints>();
            pts.points = 1500;
            GetComponent<MeshRenderer>().enabled = false;
            Destroy(this.gameObject, explodeSFX.clip.length);
            GameManager.Instance.AddScore(1500);
        }
        else if (isGrabbable)
        {
            this.GetComponent<Animator>().enabled = false;
            grabber.SetActive(true);
            if (attachedStatic != null)
            {
                attachedStatic.UpdateGifts(this.gameObject);
            }
            BoxCollider col = this.GetComponent<BoxCollider>();
            if (col != null)
            {
                col.enabled = false;
            }
            GameManager.Instance.AddScore(1500);
            Destroy(this.gameObject, 10f);
        }
    }
}
