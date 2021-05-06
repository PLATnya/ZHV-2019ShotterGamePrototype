using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
public class AreaTrigger : MonoBehaviour
{
    public bool in_area;


    private void Start()
    {
        SingleUtils.bullets_pool.transform.position = transform.position;
        //SingleUtils.player = GameObject.FindGameObjectWithTag("Player").transform;
        //SingleUtils.player_collider = SingleUtils.player.GetComponent<Collider>();
        SoldierAi.covers = new List<Transform>();
        SoldierAi.free_covers = new List<bool>();
        if (SoldierAi.parent&&SoldierAi.parent.childCount != 0)
        {
            for (int i = 0; i < SoldierAi.parent.childCount; i++)
            {
                Transform current_child = SoldierAi.parent.GetChild(i);
                if (SoldierAi.parent.GetChild(i).tag == "Cover")
                {

                    SoldierAi.covers.Add(current_child);
                    SoldierAi.free_covers.Add(false);

                }
            }
        }else{
            GameObject[] covers = GameObject.FindGameObjectsWithTag("Cover");
            for(int i = 0;i<covers.Length;i++){
                SoldierAi.covers.Add(covers[i].transform);
                SoldierAi.free_covers.Add(false);
            }
        }

    }
    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            in_area = true;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            in_area = false;
        }
    }
}
