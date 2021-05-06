using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Test : MonoBehaviour
{
    Rigidbody rigid;
    bool go;


    public float speed;


    public bool with_rigid;


    public float mass;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
       

        if (go)
        {
            going();
            if (Input.GetKeyUp(KeyCode.G))
            {
                go = false;
                if (with_rigid)
                    Destroy(GetComponent<Rigidbody>());
            }

        }
        else
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                go = true;
                if (with_rigid)
                {
                    if (!GetComponent<Rigidbody>())
                        rigid = this.gameObject.AddComponent<Rigidbody>();
                    else rigid = GetComponent<Rigidbody>();
                    rigid.useGravity = false;
                    rigid.mass = mass   ;
                    rigid.velocity = new Vector3(0, 0, speed);
                }
            }
        }
    }

    void going()
    {
        if(with_rigid) { }
        else 
        transform.position +=new Vector3(0,0, Time.deltaTime * speed);
    }

}
