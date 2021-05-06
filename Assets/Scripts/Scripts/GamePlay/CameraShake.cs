using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
public class CameraShake : MonoBehaviour
{
    
    // Start is called before the first frame update
    public IEnumerator FuckSHakeIt(float duration, float amplitude, float frec)
    {
        Transform my_trans = SingleUtils.player.GetChild(0).GetChild(0);
        float t = 0;
        float abs = amplitude;
        SingleUtils.person.AnimateCamera = false;
        SingleUtils.person.camera_anim.enabled = false;
        float now_frec = 0;
        while (t < duration)
        {
            Vector3 finish_pos = Vector3.zero;
            if (now_frec < frec)
            {
                now_frec += Time.deltaTime;
            }
            else
            {
                now_frec = 0;
                finish_pos = new Vector3(Random.Range(-abs, abs), Random.Range(-abs, abs), 0);
            }
            abs -= Time.deltaTime * (amplitude / duration);
            my_trans.localPosition = Vector3.Lerp(my_trans.localPosition, finish_pos, Time.deltaTime / frec);
            t += Time.deltaTime;
            yield return null;
        }
        my_trans.localPosition = Vector3.zero;
        SingleUtils.person.AnimateCamera = true;
        SingleUtils.person.camera_anim.enabled = true;
    }
   
}
