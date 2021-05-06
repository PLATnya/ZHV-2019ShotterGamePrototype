using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
   public Transform f;
   public Transform s;

    Rigidbody rigid_S;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rigid_S = s.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        f.position += f.forward * speed*Time.deltaTime  ;
        rigid_S.velocity = s.forward * speed;


    }
}
