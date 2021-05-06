using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTimeTeleporterUtilTrigger : MonoBehaviour
{
    public TreeTimeTeleporter teleport_trigger;
    
    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")&&teleport_trigger.teleported){
            teleport_trigger.second_door.SetActive(true);
            teleport_trigger.SetRoomBack();
            teleport_trigger.teleported = false;
            WorkingRoomDirector.floorControl.firstController.Refresh();
            if(teleport_trigger.CheckActiveOn()){
                Destroy(teleport_trigger.gameObject);
                Destroy(this.gameObject);
                WorkingRoomDirector.zero_ended = true;
            }
        }

    }
}
