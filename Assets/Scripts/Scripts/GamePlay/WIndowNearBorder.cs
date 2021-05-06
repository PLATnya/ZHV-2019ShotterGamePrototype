using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
public class WIndowNearBorder :MonoBehaviour
{
    public GameObject border;

    public Glass glass;
    public bool closed;
    public void OnOpen(){
        border.SetActive(true);
    }
    public void OnClose(){
        if(!glass.fucked)
            border.SetActive(false);
    }
}
