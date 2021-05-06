using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    public Transform upper_obj;
    public BoxCollider own_collider;
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 point = transform.InverseTransformPoint(collision.GetContact(0).point);
        Vector3 point2 = transform.InverseTransformPoint(transform.position + transform.up * own_collider.size.y * 0.7f / 2);
        if (point.y>=point2.y)
        {
            upper_obj = collision.transform;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform == upper_obj)
        {
            upper_obj = null;
        }
    }
}

