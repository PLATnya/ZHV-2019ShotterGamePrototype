using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
public class TreeTimeTeleporter : MonoBehaviour
{


    public Transform spawn;//место на 2 этаже
    bool in_room;//Ты зашел?
    public Transform[] doors;// дверьки
    Transform room;//ты зашел куда?
    int door_count;//скока дверек прошел


    public bool teleported;
    Vector3 old_local_room_pos;
    Quaternion old_local_room_rotation;

    public GameObject second_door;



    Vector3 local_player_pos;
    Vector3 player_room_rot_offset;
    public void SetRoomBack()
    {
        room.localPosition = old_local_room_pos;
        room.localRotation = old_local_room_rotation;
    }

    void Start()
    {
        transform.position = doors[door_count].position;
        transform.forward = doors[door_count].forward;
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float local_z = transform.InverseTransformPoint(other.transform.position).z;
            if (local_z > 0)
            {
                in_room = true;
                room = doors[door_count].parent;
                SingleUtils.player.parent = room;



            }
            else
            {
                in_room = false;
                if (SingleUtils.player.parent == room)
                {
                    SingleUtils.player.parent = null;
                }
            }


        }
    }
    bool Prediction(int n)
    {
        bool res = false;
        switch (n)
        {
            case 0:
                res = SingleUtils.CanPreLoad(doors[door_count]);
                break;
            case 1:
                res = SingleUtils.CanPreLoad(doors[door_count]);

                break;
            case 2:

                res = SingleUtils.CanPreLoad(doors[door_count]);
                break;
        }
        return res;
    }
    public bool CheckActiveOn(){
        bool res = false;
        if (door_count >= doors.Length&&!in_room) {
            res = true;
        }
        return res;
    }
    void Update()
    {
        if (in_room)
        {
            bool can = Prediction(door_count);
            if (can)
            {

                if (door_count < doors.Length)
                {
                    
                    if(!WorkingRoomDirector.floorControl.second_floor.activeSelf) WorkingRoomDirector.floorControl.second_floor.SetActive(true);
                    //local_player_pos = room.InverseTransformPoint(SingleUtils.player.position);
                    player_room_rot_offset = room.eulerAngles - SingleUtils.player.eulerAngles;


                    second_door.SetActive(false);
                    old_local_room_pos = room.localPosition;
                    old_local_room_rotation = room.localRotation;
                    room.transform.position = spawn.position;
                    room.forward = spawn.forward;
                    if (door_count < doors.Length - 1)
                    {
                        transform.position = doors[door_count + 1].position;
                        transform.forward = doors[door_count + 1].forward;
                    }
                    teleported = true;


                    SingleUtils.person.addFuckingAngles = true;
                    SingleUtils.person.fucingAngles = room.eulerAngles - player_room_rot_offset;
                    SingleUtils.person.ReloadCamera();


                    SingleUtils.player.parent = null;
                    //SingleUtils.player.position = room.TransformPoint(local_player_pos);


                }
                door_count++;
                in_room = false;
            }
        }
        else
        {
            
        }
    }

}
