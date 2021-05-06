using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteAlways]
public class EditingPart2 : MonoBehaviour
{
    Vector3 pos;
    [SerializeField]
    int starts;
    // Start is called before the first frame update
    void Start()
    {
        starts++;
        if (starts > 2)
            transform.position = pos;
        //if (PlayerPrefs.GetInt("played")==1) transform.position = pos;
        //if (PlayerPrefs.GetInt("played")==0&& EditorApplication.isPlaying) PlayerPrefs.SetInt("played",1);

        
        
    }

    // Update is called once per frame
    private void OnDisable()
    {
        pos = transform.position;
    }
}
