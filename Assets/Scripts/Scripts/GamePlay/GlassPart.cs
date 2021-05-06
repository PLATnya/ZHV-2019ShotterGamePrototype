using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassPart : MonoBehaviour
{
    public static float glass_limit = 6;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude >= glass_limit)
        {
            Rigidbody part_rigid = GetComponent<Rigidbody>();
            part_rigid.useGravity = true;
            part_rigid.isKinematic = false;
            transform.parent = null;
        }
    }
}
