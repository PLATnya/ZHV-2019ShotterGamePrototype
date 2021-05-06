using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigget : MonoBehaviour
{
    public TextMesh text;

    private void Start()
    {
        text.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.gameObject.SetActive(false);
        }
    }
}
