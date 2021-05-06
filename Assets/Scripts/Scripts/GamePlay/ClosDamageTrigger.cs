using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosDamageTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider col)
    {
        if (Scripts.Utils.SingleUtils.gun_slot.GetCloseDamage() == 2)
        {
            if(col.CompareTag("Soldier")){
                col.GetComponent<SoldierAi>().MakePain(50);
            }
            //make hurt to enemy
        }
    }
}
