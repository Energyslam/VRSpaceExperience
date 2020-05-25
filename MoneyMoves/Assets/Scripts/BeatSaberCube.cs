using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSaberCube : MonoBehaviour
{
    BeatSaberLite beatsaber;
    public Vector3 target;
    public float speed;
    enum Side
    {
        Left,
        Right
    }
    [SerializeField] Side side;
    bool hasHitFirstCollider;
    // Start is called before the first frame update
    void Start()
    {
        beatsaber = GetComponentInParent<BeatSaberLite>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
    }

    public void HitFirstCollider()
    {
        hasHitFirstCollider = true;
        //TODO: Reset hitfirst
    }

    public void HitSecondCollider()
    {
        if (hasHitFirstCollider)
        {
            beatsaber.RemoveCubeFromList(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void DestroyCube()
    {
        if (side == Side.Left)
        {
            //do left stuff
            Debug.Log("Going left");
        }
        else if (side == Side.Right)
        {
            //do right stuff
            Debug.Log("Going Right");
        }
    }

}
