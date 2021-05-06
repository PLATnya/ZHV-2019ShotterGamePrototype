using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
public class ParalaxByObjects : MonoBehaviour
{
    public Transform[] paralaxed_objects;
    float offset_k;


    void Update()
    {
        if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0)
        {
            for (int i = 0; i < paralaxed_objects.Length; i++)
            {
                offset_k = Mathf.Sqrt(Vector3.Distance(SingleUtils.player.position, paralaxed_objects[i].position));
                float angle = Vector3.Angle(SingleUtils.player.forward, paralaxed_objects[i].position - SingleUtils.player.position);
                paralaxed_objects[i].position+=paralaxed_objects[i].right*Mathf.Cos(angle*(3/2))*offset_k;
            }
        }
    }
}
