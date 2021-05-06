using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyOpen : MonoBehaviour
{
    public Rigidbody door_rigid;
    public void Open(){
        door_rigid.isKinematic = false;
        this.gameObject.tag = "Untagged";
        Destroy(this);  
    }
}
