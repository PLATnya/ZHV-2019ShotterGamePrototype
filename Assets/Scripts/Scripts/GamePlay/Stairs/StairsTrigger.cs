using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerStay(Collider col){
        if(col.CompareTag("Player")){
            
        }
    }
}
