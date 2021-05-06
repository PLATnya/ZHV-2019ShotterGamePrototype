using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
public class VentilationTrigger : MonoBehaviour, PlotTrigger
{
    public bool Can = true;



    public GameObject[] activate;
    public GameObject[] disactivate;
    public Transform forest;
    public Transform forest_spawn;
    public bool backAgain = false;



    bool playerInTrigger;

    public bool PlayerInTrigger() { return playerInTrigger; }

    Vector3 forest_old_position;
    void OnTriggerEnter(Collider col)
    {

        if (col.CompareTag("Player") && Can)
        {
            SetActivate(true);
            SetDisactivate(false);
            if (forest != null)
            {
                forest.gameObject.SetActive(true);
                forest_old_position = forest.position;
                forest.forward = forest_spawn.forward;
                forest.position = forest_spawn.position;
            }
            if (!backAgain)
                Destroy(this.gameObject);
        }
    }

    public void SetActivate(bool active)
    {
        for (int i = 0; i < activate.Length; i++)
        {
            activate[i].SetActive(active);
        }
    }
    public void SetDisactivate(bool active)
    {
        for (int i = 0; i < disactivate.Length; i++)
        {
            disactivate[i].SetActive(active);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player") && Can)
        {
            if (backAgain)
            {

                SetDisactivate(true);
                SetActivate(false);
                if (forest != null)
                {
                    forest.position = forest_old_position;
                    forest.gameObject.SetActive(false);
                }
            }
        }
    }
}
