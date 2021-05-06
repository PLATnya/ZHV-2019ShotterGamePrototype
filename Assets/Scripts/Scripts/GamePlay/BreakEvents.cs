using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEvents : MonoBehaviour
{
    public static void BreakGlass(Transform glass, Vector3 contact_point, Vector3 force, float limit_force)
    {
        float width = glass.lossyScale.x/2;
        Destroy(glass.GetComponent<BoxCollider>());

        Destroy(glass.GetComponent<MeshFilter>());
        Debug.Log("width"+width);
        List<Transform> get_out_list = new List<Transform>();
        for(int i = 0;i<glass.childCount; i++)
        {
            Transform part = glass.GetChild(i);
            
            part.gameObject.SetActive(true);
            float forced = force.magnitude * ((width -   Vector3.Distance(contact_point, part.position) / width)/2);
            
            if (forced >= limit_force)
            {
                get_out_list.Add(part);
                
                
            }

        }
        for(int i=0; i < get_out_list.Count; i++)
        {
            get_out_list[i].parent = null;
            Rigidbody part_rigid = get_out_list[i].GetComponent<Rigidbody>();
            part_rigid.isKinematic = false;
            //part_rigid.AddForce(new Vector3(Mathf.Sqrt(force.x), Mathf.Sqrt(force.y), Mathf.Sqrt(force.z))/4);
        }
    }
    
}
