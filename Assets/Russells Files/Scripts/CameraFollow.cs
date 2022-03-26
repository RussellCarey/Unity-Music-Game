using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera cameraMain;
    public Vector3 offest;
    public List<Transform> targets;

    float widthToBeSeen;
    public Bounds targetBounds;
    public Bounds bounds;

    float maxY;
    float leastY;

    void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offest;
        cameraMain.transform.position = new Vector3(newPosition.x, newPosition.y, -10);


        for(int i = 0; i < targets.Count; i++)
        {
            
            if(targets[i].position.y > maxY)
            {
                maxY = targets[i].position.y;
            }

            if(targets[i].position.y < leastY)
            {
                leastY = targets[i].position.y;
            }
        }

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = bounds.size.x / bounds.size.y;

        if (screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = (bounds.size.y / 2) + 3;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = (bounds.size.y / 2 * differenceInSize) + 3;
        }


    

}

    Vector3 GetCenterPoint()
    {
        if(targets.Count == 1)
        {
            return targets[0].position;
        }

        bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

}








/*
 *
 *
 *  public Camera cameraMain;
    public Transform cameraPointA;
    public Transform cameraPointB;
    public Transform[] cameraPoints;
    private Vector3 newPos;


    var totalX = 0f;
        var totalY = 0f;

        foreach (var point in cameraPoints)
        {
            totalX += point.transform.position.x;
            totalY += point.transform.position.y;
        }
        var centerX = totalX / cameraPoints.Length;
        var centerY = totalY / cameraPoints.Length;

        newPos = Vector3.Lerp(gameObject.transform.position, cameraPoints[0].transform.position, Time.deltaTime);
        cameraMain.transform.position = new Vector3(centerX, centerY, newPos.z);



        //cameraMain.transform.position = cameraPointA.position + (cameraPointA.position - cameraPointB.position) / 2;
        //cameraMain.transform.position = cameraPoint.position;*/
