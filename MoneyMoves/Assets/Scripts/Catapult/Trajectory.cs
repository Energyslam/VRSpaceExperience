using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField]
    GameObject pullPoint;
    [SerializeField]
    Material lineMaterial;

    private Transform arcObjectTransform;

    private LineRenderer rend;

    private SlingShooter slingShooter;

    private Vector3 forward, forwardVelocity;

    private float projectileSpeed, projectileMass;
    public float startWidth, endWidth;

    private int arcLength = 500;

    void Start()
    {
        rend = this.gameObject.AddComponent<LineRenderer>();
        rend.positionCount = arcLength;      
        rend.material = lineMaterial;
        rend.textureMode = LineTextureMode.Tile;
        rend.startWidth = startWidth;
        rend.endWidth = endWidth;

        slingShooter = pullPoint.GetComponent<SlingShooter>();
        projectileSpeed = slingShooter.projectileSpeed;
        slingShooter.onPullReset.AddListener(OnPullReturn);
    }

    void OnPullReturn()
    {
        for(int i =0; i < arcLength; i++)
        {
            rend.SetPosition(i, Vector3.zero);
        }
    }

    void FixedUpdate()
    {
        if (!slingShooter.isPulling)
        {
            return;
        }
        forward = this.transform.position - pullPoint.transform.position;
        forwardVelocity = forward * projectileSpeed;

        Vector3[] rendPos = new Vector3[arcLength];
        for(int i = 0; i < arcLength; i++)
        {
            Vector3 grav = Physics.gravity * i * (Time.fixedDeltaTime / 2); //represents Time.fixedDeltaTime divided by 2
            Vector3 calcedPosition = pullPoint.transform.position + (forwardVelocity + grav) * i * Time.fixedDeltaTime;

            rendPos[i] = calcedPosition;
        }
        rend.SetPositions(rendPos);
    }
}