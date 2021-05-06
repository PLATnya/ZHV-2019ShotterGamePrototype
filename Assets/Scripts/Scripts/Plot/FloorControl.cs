using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//[CreateAssetMenu(fileName = "Fllooor", menuName = "Scripts/Scripts/Plot", order = 1)]
public class FloorControl : MonoBehaviour
{
    public GameObject  first_floor;
    public GameObject second_floor;
    public GameObject third_floor;

    public FloorController firstController;
    void Awake(){
        firstController= first_floor.GetComponent<FloorController>();
        
    }
}
