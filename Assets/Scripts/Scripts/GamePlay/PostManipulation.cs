using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
public class PostManipulation : MonoBehaviour
{
    public VolumeProfile profile;
    DepthOfField focus;
    ShadowsMidtonesHighlights smh;



    [SerializeField]
    float t;
    [SerializeField]
    Collider col;
    float help;
    bool fuck_clip;
    bool fall;



    public Camera testing_camera;
    Matrix4x4 originalProjection;

    void Start()
    {
        profile.TryGet(out focus);
        profile.TryGet(out smh);
        


        originalProjection = testing_camera.projectionMatrix;



    }
    public IEnumerator DepthFade(float depth, float t_up, float t_down)
    {
        fuck_clip = true;
        float time = 0;
        float start_focus = focus.focusDistance.value;
        float finish_focus = start_focus - (start_focus * depth);
        while (time < t_up + t_down)
        {
            time += Time.deltaTime;
            if (time < t_up)
            {
                focus.focusDistance.value = Mathf.Lerp(start_focus, finish_focus, time / t_up);
            }
            else
            {
                RaycastHit hit = HitDistance();
                focus.focusDistance.value = Mathf.Lerp(finish_focus, hit.distance, (time - t_up) / t_down);
            }
            yield return null;
        }
        fuck_clip = false;
        t = 0;
    }

    RaycastHit HitDistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {

        }
        return hit;
    }
    public Shader interact_shader;


    void Update()
    {





        Matrix4x4 p = originalProjection;
        p.m01 += Mathf.Sin(Time.time * 1.2F) * 0.1F;
        p.m10 += Mathf.Sin(Time.time * 1.5F) * 0.1F;
        testing_camera.projectionMatrix = p;


        // Debug.Log(Camera.main.projectionMatrix);
        if (!fuck_clip && !fall)
        {

            RaycastHit hit = HitDistance();
            if (hit.collider == col)
            {
                float finish = hit.distance;
                if (col == null) finish = 30;
                t += Time.deltaTime;
                float g = Mathf.Clamp(t, 0, 2.5f);

                //focus.focusDistance.value = Mathf.MoveTowards(focus.focusDistance.value, hit.distance,2.5f);
                focus.focusDistance.value = Mathf.Lerp(help, finish, g / 2.5f);
                focus.focusDistance.value = Mathf.Clamp(focus.focusDistance.value, 1, 30);
                if (t > 4)
                {
                    StartCoroutine(DepthFade(1, 1f, 0.8f));

                }
            }
            else
            {
                help = focus.focusDistance.value;
                t = 0;
                col = hit.collider;
            }
        }
    }



    public IEnumerator FallSmooth()
    {
        fuck_clip = false;
        fall = true;
        while (focus.focusDistance.value > 0)
        {
            focus.focusDistance.value = Mathf.MoveTowards(focus.focusDistance.value, 0, 1);
            yield return null;
        }

    }

    public IEnumerator StandSmooth()
    {
        fall = false;
        while (focus.focusDistance.value < 5)
        {
            focus.focusDistance.value = Mathf.MoveTowards(focus.focusDistance.value, 5, 1);
            yield return null;
        }
    }

    public IEnumerator DarkInEyes(float donw_t, float up_t, float amplitude)
    {
        float t = 0;
        float all = donw_t + up_t;
        float non_aplitude = (1 - amplitude);
        while (t < all)
        {
            t += Time.deltaTime;
            if (t < donw_t)
            {
                float a = (donw_t - t) / donw_t * amplitude + non_aplitude;
                smh.shadows.value = new Vector4(a, a, a, 0);
                smh.midtones.value = new Vector4(a, a, a, 0);
            }
            else
            {
                float a = (t - donw_t) / up_t * amplitude + non_aplitude;
                smh.shadows.value = new Vector4(a, a, a, 0);
                smh.midtones.value = new Vector4(a, a, a, 0);
            }

            yield return null;
        }
    }

}
