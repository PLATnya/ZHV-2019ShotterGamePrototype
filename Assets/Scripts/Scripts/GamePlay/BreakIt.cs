using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BreakIt : MonoBehaviour
{

    public float limit_force;
    bool breaked;
    Coroutine breaking;
    public UnityEvent g;
    // Start is called before the first frame update
    

    private void OnCollisionEnter(Collision collision)
    {
        if (!breaked)
        {
            Vector3 force = collision.impulse;
            Debug.Log(force.magnitude);
            if (force.magnitude>= limit_force)
            {
                BreakEvents.BreakGlass(transform, collision.GetContact(0).point,force,limit_force);
                breaked = true;
                // g.Invoke();
            }
        }
    }
    private void Update()
    {
        
    }

}
