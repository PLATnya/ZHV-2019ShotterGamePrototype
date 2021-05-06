using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
using UnityEngine.Events;
public class NoControl : MonoBehaviour
{


    RigidbodyConstraints start_freeze;
    Quaternion start_rotation_d;
    Quaternion help_rotation_d;
    Vector3 start_position_d;
    Vector3 help_position_d;
    bool goUp;
    bool goDown;
    public bool fucking_falling;


    public UnityEvent onFaceToFloor;
    void FuckTheControl(Vector3 force, Vector3 point)
    {
        start_position_d = transform.position;
        start_rotation_d = transform.rotation;


        SingleUtils.gun_slot.ChangeGunState(GunState.hide);



        SingleUtils.person.ControllerPause(true);

        SingleUtils.person.CameraPause();
        SingleUtils.person.enableCameraShake = false;

        SingleUtils.person.fps_Rigidbody.constraints = RigidbodyConstraints.None;
        //SingleUtils.person.enabled = false;
        //SingleUtils.person.fps_Rigidbody.velocity = new Vector3(0, 10, 1);
        SingleUtils.person.fps_Rigidbody.AddForceAtPosition(force, point);
    }
    void GetTheControl()
    {
        SingleUtils.person.fps_Rigidbody.constraints = start_freeze;
        SingleUtils.gun_slot.ChangeGunState(GunState.two_hand);
        SingleUtils.gun_slot.SetGunActive();
        SingleUtils.person.ControllerPause();
        SingleUtils.person.CameraUnpause();
        SingleUtils.person.enableCameraShake = false;
        // SingleUtils.person.enabled = true;
        fucking_falling = false;
    }


    void StartDying(Vector3 force, Vector3 point)
    {
        FuckTheControl(force, point);
        SingleUtils.post.StartCoroutine(SingleUtils.post.FallSmooth());
        StartCoroutine(GoDown(limit_time, chenge_time));
        fucking_falling = true;


    }
    void StartWakeUp()
    {
        SingleUtils.post.StartCoroutine(SingleUtils.post.StandSmooth());
        //goUp = true;
        StartCoroutine(StandUp(limit_time));
        StartCoroutine(GoUp(limit_time, chenge_time, 2));

    }

    #region slow_motion
    [SerializeField]
    [Range(0, 1)]
    float time;
    [SerializeField]
    float chenge_time;
    [SerializeField]
    float limit_time;
    float speed;

    public float limit;
    #endregion

    public void FuckingFall(Vector3 dir, Vector3 point)
    {
        if (!goDown && !goUp)
        {
            SingleUtils.grab_trigger.Fall();

            StartDying(dir * 100, point);
        }

    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (SingleUtils.grab_trigger.grabing)
        {
            bool condition = false;//уловие при котором падаем
            if (condition)
            {
                FuckingFall(collision.relativeVelocity.normalized, collision.GetContact(0).point);
            }
        }

        Vector3 force = collision.impulse;
        
        if (force.magnitude >= limit)
        {
            Vector3 inverse_force = -collision.relativeVelocity / collision.contactCount;
            for (int i = 0; i < collision.contactCount; i++)
            {
                StartDying(inverse_force, collision.GetContact(i).point);
            }
        }
    }*/

    void Start()
    {
        start_freeze = SingleUtils.person.fps_Rigidbody.constraints;
        time = 1;
        speed = 0.02f / chenge_time;
    }

    public void debug()
    {
        if (!goDown && !goUp)
        {
            time = Time.timeScale;
            if (Input.GetKeyDown(KeyCode.U))
            {
                StartDying(new Vector3(5, 5, 0), SingleUtils.player.position + new Vector3(0, 1, 0));

            }

            if (Time.timeScale < 1)
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    StartWakeUp();
                }
            }


        }

    }
    public IEnumerator GoDown(float limit, float change)
    {
        goDown = true;

        bool g = false;
        while (true)
        {
            yield return null;


            if (!g)
            {
                RaycastHit hit;
                if (Physics.Raycast(SingleUtils.player.position, Vector3.down, out hit, 3))
                {

                }
                if (hit.collider != null)
                {
                    if (Vector3.Angle(hit.normal, Vector3.up) < SingleUtils.person.advanced.maxSlopeAngle){
                        g = true;
                        Debug.Log("iNVOKED");
                        onFaceToFloor.Invoke();
                        onFaceToFloor.RemoveAllListeners();
                    }
                }
            }
            else if (g)
            {
                if (Time.timeScale > limit)
                {
                    //time -= speed / Time.timeScale;
                    time -= 0.02f * (1 - limit) / change;

                    Time.timeScale = time;
                    Time.fixedDeltaTime = Time.timeScale * 0.02f;
                    Time.timeScale = Mathf.Clamp01(Time.timeScale);
                }else{
                    break;
                }
            }
        }
        time = Mathf.Clamp(time, limit, 1);
        goDown = false;

    }
    public IEnumerator GoUp(float limit, float change, float k)
    {
        goUp = true;
        bool f = false;
        while (true)
        {

            if (!f)
            {
                RaycastHit hit;
                if (Physics.Raycast(SingleUtils.player.position, Vector3.down, out hit, 3))
                {

                }
                if (hit.collider != null)
                {
                    f = true;
                }
            }
            else if (f)
            {
                if (Time.timeScale < 1)
                {
                    //if(Time.timeScale>1) Time.timeScale = 1;
                    //time += speed / 4 / Time.timeScale;
                    time += 0.02f * (1 - limit) / change / k;

                    Time.timeScale = time;
                    Time.fixedDeltaTime = Time.timeScale * 0.02f;
                    // float t = ((time - l  imit) / (1 - limit));

                }
                else
                {
                    break;
                }
            }
            yield return null;
        }
        /*
        while (Time.timeScale < 1)
        {
            yield return null;

            //if(Time.timeScale>1) Time.timeScale = 1;
            //time += speed / 4 / Time.timeScale;
            time += 0.02f * (1 - limit) / change / k;

            Time.timeScale = time;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            // float t = ((time - l  imit) / (1 - limit));

        }*/
        time = Mathf.Clamp(time, limit_time, 1);
        goUp = false;
    }
    IEnumerator StandUp(float limit)
    {
        RaycastHit hit;
        if (Physics.Raycast(SingleUtils.player.position, Vector3.down, out hit))
        {
            start_position_d = hit.point + new Vector3(0, 1, 0);
        }
        help_position_d = transform.position;
        help_rotation_d = transform.rotation;
        float t = 0;
        while (time < 1)
        {
            yield return null;
            t = ((time - limit) / (1 - limit));
            transform.position = Vector3.LerpUnclamped(help_position_d, start_position_d, t);
            transform.rotation = Quaternion.LerpUnclamped(help_rotation_d, start_rotation_d, t);

        }
        GetTheControl();
    }
    void Update()
    {
        /*
        time = Mathf.Clamp(time, limit_time, 1);
        if (goDown)
        {
            if (Time.timeScale > limit_time)
            {
                time -= speed / Time.timeScale;
                Time.timeScale = time;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                Time.timeScale = Mathf.Clamp01(Time.timeScale);
            }
            else
            {
                goDown = false;
            }
        }
        else if (goUp)
        {

            if (Time.timeScale < 1)
            {

                //if(Time.timeScale>1) Time.timeScale = 1;
                time += speed / 4 / Time.timeScale;
                Time.timeScale = time;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                float t = ((time - 0.1f) / (1 - limit_time));

            }
            else
            {

                GetTheControl();
            }
        }*/


    }

}
