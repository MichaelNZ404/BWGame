﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraCollision : MonoBehaviour
{
    // Start is called before the first frame update
    // public float minDistance = 1f;
    // public float maxDistance = 4f;
    // public float smooth = 10f;
    // Vector3 dollyDir;
    // public Vector3 dollyDirAdjusted;
    // public float distance;

    // void Start()
    // {
    //     dollyDir = transform.localPosition.normalized;
    //     distance = transform.localPosition.magnitude;
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
    //     RaycastHit hit;
    //     if (Physics.Linecast (transform.parent.position, desiredCameraPos, out hit)) {
    //         distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
    //     }
    //     else {
    //         distance = maxDistance;
    //     }
    //     transform.localPosition = Vector3.lerp (transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    // }
}
