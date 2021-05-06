using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
public class BorderTrigger : MonoBehaviour
{
    bool can_jump;
    [SerializeField]
    Transform border;

    public bool can_over;
    public bool can_on = true;

	public float ledger;

    private void OnTriggerStay(Collider other)
    {

	   if (other.CompareTag("Player") && !SingleUtils.grab_trigger.grabing_on)
        {
            SingleUtils.on_trigger_border = true;
            SingleUtils.border = border;
            SingleUtils.can_over = can_over;
            SingleUtils.can_on = can_on;
			SingleUtils.ledge = ledger;
               
            //Debug.Log(Vector3.Angle(SingleUtils.player.forward, (SingleUtils.border.GetComponent<Collider>().ClosestPoint(SingleUtils.player.position) - SingleUtils.player.position)));
            //Debug.DrawRay(SingleUtils.player.position, SingleUtils.player.forward,Color.blue,5);
            //Debug.DrawLine(SingleUtils.border.GetComponent<Collider>().ClosestPoint(SingleUtils.player.position),SingleUtils.player.position,Color.red,5);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            SingleUtils.on_trigger_border = false;
            SingleUtils.border = null;
            SingleUtils.can_over = false;
        }
    }
}
