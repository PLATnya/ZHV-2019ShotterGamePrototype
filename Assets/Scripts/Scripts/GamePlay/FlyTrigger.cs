using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
public class FlyTrigger : MonoBehaviour
{
    public static bool go_no_grav_up;
    private void OnTriggerStay(Collider col)
    {
        Debug.Log(go_no_grav_up);
        if (!SingleUtils.person.fly)
        {
            if (col.CompareTag("Player"))
            {
                if (!SingleUtils.person.fly)
                {
                    if (Input.GetButtonDown("Jump") && !go_no_grav_up)
                    {
                        go_no_grav_up = true;
                        //SingleUtils.person.ControllerPause();
                        SingleUtils.person.Fly();
                    }
                }
            }
        }
        else
        {

            if (!go_no_grav_up)
                SingleUtils.person.fucking_velocity = (SingleUtils.person.playerCamera.transform.forward * SingleUtils.person.GetInput().y + SingleUtils.player.right * SingleUtils.person.GetInput().x) * SingleUtils.person.speed;
            else
            {
                SingleUtils.person.fucking_velocity = Vector3.up * 5;
                if(Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.D))
               // if (Mathf.Abs(Input.GetAxis("Vertical")) >= 1 || Mathf.Abs(Input.GetAxis("Horizontal")) >= 1)
                {

                    go_no_grav_up = false;
                    //.person.ControllerPause();
                    //SingleUtils.person.Fly();
                }
            }

        }
    }
    void OnTriggerExit(Collider col){
        if(col.CompareTag("Player")){
            //SingleUtils.grab_trigger.Fall();
        }
    }
}
