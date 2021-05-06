using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool opened;
    
    public float close_limit_impact;
    public Door second_door;
    

    [SerializeField]
    float impacted;
    public void AddImpacted(float delta) { impacted += delta; }
    
    private void OnCollisionEnter(Collision collision)
    {
       
        Vector3 force = collision.impulse;
        
        if (!opened)
        {
            Debug.Log(force.magnitude + "DOOR");
            impacted += force.magnitude;
            if (second_door != null) second_door.AddImpacted(force.magnitude);
            if (impacted >= close_limit_impact)
            {
                opened = true;

                GetComponent<Rigidbody>().isKinematic = false;
                Destroy(GetComponent<Door>());
                if (second_door != null)
                {
                    second_door.transform.GetComponent<Rigidbody>().isKinematic = false;
                    Destroy(second_door);
                }
                
                
            }
        }
        
    }
    
}
