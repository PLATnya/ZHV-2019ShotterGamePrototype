using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class FloorAngleTrigger : MonoBehaviour
{
    public UnityEvent triggerEnterEvent;
    public bool fukBak = false;
    [SerializeField]
    bool fucked;
    public void SetFucked(bool fuc){
        fucked = fuc;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&!fucked)
        {
            triggerEnterEvent.Invoke();
            if(!fukBak)
                fucked = true;
            
        }
    }
}