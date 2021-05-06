using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleStateThreeActive : MonoBehaviour
{
    public GameObject angle_of_room;
    public GameObject second_angle_of_room;
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (ThroughTheWindowTrigger.exit_type == 3||ThroughTheWindowTrigger.exit_type == 0)
            {
                angle_of_room.SetActive(true);
                second_angle_of_room.SetActive(false);
            }
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (ThroughTheWindowTrigger.exit_type == 0 || ThroughTheWindowTrigger.exit_type == 3)
            {
                /*
                float local_z = transform.InverseTransformPoint(col.gameObject.transform.position).z;
                if (local_z > 0)
                {
                    
                }*/
                angle_of_room.SetActive(false);
                second_angle_of_room.SetActive(true);
            }
        }
    }
}
