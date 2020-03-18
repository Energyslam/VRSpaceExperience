using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldGrabber : MonoBehaviour
{
    [SerializeField] GameObject deliveryPoint, coinUpperLocation, coinLowerLocation, coinObjectLocation, coinPrefab;
    public Vector3 targetPosition;
    public float speed;
    bool isMoving;
    bool hasGold;
    GameObject attachedCoin;

    void Start()
    {
        targetPosition = coinUpperLocation.transform.position;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        CheckPosition();
    }

    void CheckPosition()
    {
        if (transform.position == deliveryPoint.transform.position)
        {
            if (attachedCoin != null)
            {
                attachedCoin.GetComponent<Rigidbody>().useGravity = true;
                attachedCoin.GetComponent<Rigidbody>().AddTorque(new Vector3(9000f, 9000f, 9000f), ForceMode.VelocityChange);
                attachedCoin.transform.parent = null;
            }
            Destroy(this.gameObject);
        }
        if (transform.position == coinUpperLocation.transform.position)
        {
            if (hasGold)
            {
                targetPosition = deliveryPoint.transform.position;
            }
            if (!hasGold)
            {
                targetPosition = coinLowerLocation.transform.position;
            }
        }
        if (transform.position == coinLowerLocation.transform.position)
        {
            Debug.Log("Get coin");
            hasGold = true;
            targetPosition = coinUpperLocation.transform.position;
            GameObject coin = Instantiate(coinPrefab, coinObjectLocation.transform);
            attachedCoin = coin;
        }
    }
}

