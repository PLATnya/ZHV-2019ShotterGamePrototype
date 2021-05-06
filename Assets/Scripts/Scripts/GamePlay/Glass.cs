using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    BoxCollider mainCollldier;
    MeshRenderer mainRender;
    public bool fucked;
    public WIndowNearBorder wIndow;
    void Start()
    {
        mainCollldier = GetComponent<BoxCollider>();
        mainRender = GetComponent<MeshRenderer>();
         
    }
    void OnCollisionEnter(Collision col)
    {
        if (!fucked)
        {
            if (col.impulse.magnitude > 10)
            {
                mainRender.enabled = false;
                mainCollldier.enabled = false;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                fucked = true;
                wIndow.OnOpen();
            }
        }

    }

}
