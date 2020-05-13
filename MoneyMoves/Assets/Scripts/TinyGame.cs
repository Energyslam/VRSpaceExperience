using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyGame : MonoBehaviour
{
    [Header("Boundaries")]
    [SerializeField] Transform topLeft;
    [SerializeField] Transform topRight;
    [SerializeField] Transform bottomLeft;
    [SerializeField] Transform bottomRight;
    [SerializeField] Transform middle;
    [Header("")]

    [SerializeField] GameObject target;

    [SerializeField] Minigame minigame;
    public int spawnSpeed;
    int quickSpawner = 0;
    int enumLength;
    // Start is called before the first frame update
    public enum GameSide
    {
        Left,
        RIght
    }
    public GameSide gameSide;
    enum Sides
    {
        Left,
        Right,
        Top,
        Bottom
    }
    void Start()
    {
        enumLength = System.Enum.GetNames(typeof(Sides)).Length;
    }

    void SpawnObject()
    {
        GameObject newTarget = Instantiate(target, transform);
        newTarget.transform.localPosition = GetPositionOnSide(GetRandomSide());
        MinigameTarget miniTarget = newTarget.GetComponent<MinigameTarget>();
        miniTarget.target = middle;
        miniTarget.gameSide = gameSide;
        miniTarget.minigame = minigame;
    }
    Sides GetRandomSide()
    {
        return (Sides)Random.Range(0, enumLength);
    }

    Vector3 GetPositionOnSide(Sides side)
    {
        Vector3 pos = Vector3.zero;
        if (side == Sides.Left)
        {
            pos = new Vector3(0, Random.Range(bottomLeft.localPosition.y, topLeft.localPosition.y), bottomLeft.localPosition.z);
        }
        else if (side == Sides.Right)
        {
            pos = new Vector3(0, Random.Range(bottomRight.localPosition.y, topRight.localPosition.y), bottomRight.localPosition.z);
        }
        else if (side == Sides.Top)
        {
            pos = new Vector3(0, topRight.localPosition.y, Random.Range(topLeft.localPosition.z, topRight.localPosition.z));
        }
        else if (side == Sides.Bottom)
        {
            pos = new Vector3(0, bottomRight.localPosition.y, Random.Range(bottomLeft.localPosition.z, bottomRight.localPosition.z));
        }
        return pos;
    }
    // Update is called once per frame
    void Update()
    {
        quickSpawner++;
        if (quickSpawner >= spawnSpeed)
        {
            quickSpawner = 0;
            SpawnObject();
        }
    }
}
