using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderCity : MonoBehaviour
{
    public bool grabing;

        

    private Rigidbody rigid;

    private Transform grabed;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
           
    }
}
