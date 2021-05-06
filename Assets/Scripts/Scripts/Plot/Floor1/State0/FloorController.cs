using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public Transform[] floor;
    public Transform[] parts;
    public Transform[] ceiling;

    public Vector3[] start_locals;
    public Quaternion[] start_rots;
    void Start()
    {
        start_rots = new Quaternion[parts.Length];
        start_locals = new Vector3[parts.Length];
        for (int i = 0; i < parts.Length; i++)
        {
            start_locals[i] = parts[i].localPosition;
            start_rots[i] = parts[i].localRotation;
        }
    }
    public void Refresh()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].localPosition = start_locals[i];
            parts[i].localRotation = start_rots[i];
        }
    }

}
