using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
public class ThroughTheWindowTrigger : MonoBehaviour
{
    


    public int part;

    //Vector3[] olds;
    //Quaternion[] olds_rot;
   
    Vector3 help_player_pos;
    Quaternion help_player_rot;

    public static int exit_type;


    bool firstly;
    FloorController floor;
    void Start()
    {
        //firstly = true;
        
       // olds = new Vector3[2];
       
       /* olds[0] = floor.parts[0].localPosition;
        olds[1] = floor.parts[1].localPosition;
        olds_rot[0] = floor.parts[0].localRotation;  
        olds_rot[1] = floor.parts[1].localRotation;
        */
        floor = WorkingRoomDirector.floorControl.firstController;
    }

    void OnTriggerEnter(Collider col){
        if(col.CompareTag("Player")){
            if(WorkingRoomDirector.floorControl.second_floor.activeSelf){
                WorkingRoomDirector.floorControl.second_floor.SetActive(false);
            }
        }
    }
    

   
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float local_z = transform.InverseTransformPoint(SingleUtils.player.position).z;
            if (part == 0)
            {
                if (local_z > 0)
                {
                    exit_type = 0;
                    floor.parts[1].localPosition = floor.start_locals[1];
                    floor.parts[1].forward = floor.parts[0].forward;

                    //SingleUtils.player.parent = floor.parts[0];
                    help_player_pos = floor.parts[0].InverseTransformPoint(SingleUtils.player.position);

                    
                    Transform spawn = floor.parts[1].Find("Spawn");
                    floor.parts[0].position = spawn.position;
                    floor.parts[0].forward = spawn.forward;

                    
                    
                    SingleUtils.player.position = floor.parts[0].TransformPoint(help_player_pos);
                    
                    //SingleUtils.player.parent = null;
                }
                else
                {
                    exit_type = 1;
                    //SingleUtils.player.parent = floor.parts[0];
                    help_player_pos = floor.parts[0].InverseTransformPoint(SingleUtils.player.position);

                    floor.parts[0].localPosition = floor.start_locals[0];
                    
                    
                    SingleUtils.player.position = floor.parts[0].TransformPoint(help_player_pos);
                    //SingleUtils.player.parent = null;
                    Transform spawn = floor.parts[0].Find("Spawn");
                    floor.parts[1].position = spawn.position;
                    floor.parts[1].forward = spawn.forward;
                }

            }
            else
            {
                if (local_z > 0)
                {
                    exit_type = 3;
                    //SingleUtils.player.parent = floor.parts[1];
                    help_player_pos = floor.parts[1].InverseTransformPoint(SingleUtils.player.position);

                    floor.parts[1].localPosition = floor.start_locals[1];

                    SingleUtils.player.position = floor.parts[1].TransformPoint(help_player_pos);
                    //SingleUtils.player.parent = null;
                    Transform spawn = floor.parts[1].Find("Spawn");
                    floor.parts[0].position = spawn.position;
                    floor.parts[0].forward = spawn.forward;
                }
                else
                {
                    exit_type = 2;
                    Transform spawn = floor.parts[0].Find("Spawn");
                    floor.parts[0].localPosition = floor.start_locals[0];
                    floor.parts[0].forward = floor.parts[1].forward;
                    floor.parts[0].transform.rotation = Quaternion.Euler(floor.parts[1].rotation.eulerAngles - spawn.localRotation.eulerAngles);
                    //floor.parts[0].rotation = Quaternion.Euler(floor.parts[0].rotation.eulerAngles-(floor.parts[1].rotation.eulerAngles - floor.parts[0].rotation.eulerAngles));
                    //SingleUtils.player.parent = floor.parts[1];
                    help_player_pos = floor.parts[1].InverseTransformPoint(SingleUtils.player.position);

                    
                    floor.parts[1].position = spawn.position;
                    floor.parts[1].forward = spawn.forward;
                    
                    SingleUtils.player.position = floor.parts[1].TransformPoint(help_player_pos);
                    //SingleUtils.player.parent = null;
                }
            }
        }
    }

}
