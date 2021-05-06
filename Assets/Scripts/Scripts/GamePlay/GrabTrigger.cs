using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using Scripts.Utils;

public class GrabTrigger : MonoBehaviour
{

    public Transform hands;
    #region Tools

    public Rigidbody rigid;
    [SerializeField]
    private float time;
    public bool grabing_on;
    public bool grabing;
    public Transform grabed;
    private Vector3 new_position;
    private Vector3 help_position;
    private Quaternion help_rotation, new_rotation;
    private Vector3 size;
    private float player_height;
    Collider grabed_collider;
    Vector3 closest_collider_position;
    Vector3 help_velocity;
    public Transform player;

    GameObject debug;

    public float Player_Height
    {
        get { return player_height; }
    }
    [SerializeField]
    private float Time_Managment => time;
    #endregion


    private bool triggered_border;



    #region Grabing

    Vector3 vector_forward;
    public float ledge;
    public bool dynamic_grabed_body;
    #endregion

    #region Dash
    [SerializeField]
    bool dash;


    #endregion

    #region Stairs

    [SerializeField]
    bool stairs;
    #endregion

    public bool GetStairs()
    {
        return stairs;
    }

    private void Start()
    {
        start_sensivity = SingleUtils.person.mouseSensitivity;
        //player = transform.parent.parent.parent;

        SingleUtils.person = player.GetComponent<FirstPersonAIO>();
        player_height = player.GetComponent<CapsuleCollider>().height;

        debug = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(debug.GetComponent<BoxCollider>());
        debug.name = "DEBUG";
        debug.transform.localScale /= 5;




    }

    void OnTriggerEnter(Collider col)
    {
        if (!grabing && !grabing_on)
        {
            if (col.CompareTag("OnOBJ"))
            {


                triggered_border = true;
                grabed = col.transform;
                if (grabed.GetComponent<Rigidbody>()) dynamic_grabed_body = true;
                else dynamic_grabed_body = false;

                grabed_collider = col;
                MakeSizeOnEnter(col);
                //size = grabed.GetComponent<BoxCollider>().size;
            }
            else if (col.CompareTag("ClearCover") || col.CompareTag("ClearBorder"))
            {
                MakeSizeOnEnter(col);
            }

        }

        //MakeGrabedPosition();
    }
    void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Stairs"))
        {
            if (!stairs)
            {


                grabed = col.transform;


                grabed_collider = grabed.GetComponent<Collider>();
                stairs = true;
                MakeSizeOnEnter(col);

            }
        }
        else
        {
            if (col.CompareTag("OnOBJ"))
            {
                /*
                Vector3 look  =grabed.position - new Vector3(SingleUtils.player.position.x,grabed.position.y,SingleUtils.player.position.z);
                hands.rotation = Quaternion.LookRotation(Vector3.Cross(look,grabed.right),grabed.up);
                Vector3 hands_pos = grabed.InverseTransformPoint(SingleUtils.player.position);
                hands_pos = new Vector3(Mathf.Clamp(hands_pos.x,-size.x/2,size.x/2),size.y/2,Mathf.Clamp(hands_pos.z,-size.z/2,size.z/2));
                hands_pos+=grabed.position;
                hands.position = hands_pos;*/
                //Vector3 to_player_norm = (SingleUtils.player.position - grabed.position).normalized;
                //Vector3 hands_pos = grabed.position + new Vector3(to_player_norm.x*(size.x/2),0,to_player_norm.z*(size.z/2));
                //hands.position = hands_pos;
            }
            else if (col.CompareTag("ClearCover") || col.CompareTag("ClearBorder"))
            {
                SingleUtils.on_trigger_border = true;
                SingleUtils.border = col.transform;

                SingleUtils.can_over = true;
                SingleUtils.ledge = 0.4f;
                //SingleUtils.ledge = Mathf.Clamp(SingleUtils.ledge,0.4f,1);
            }
        }
    }
    void MakeSizeOnEnter(Collider col)
    {
        if (col.GetType() == typeof(MeshCollider))
        {
            size = (col as MeshCollider).bounds.size;
        }
        else if (col.GetType() == typeof(BoxCollider))
        {
            size = (col as BoxCollider).size;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == grabed && !grabing)
        {
            triggered_border = false;
            size = Vector3.zero;
        }

        if (other.CompareTag("Stairs"))
        {
            if (stairs && !grabing)
            {
                grabed = null;
                stairs = false;
            }
        }
    }

    public void Fall()
    {

        SingleUtils.person.mouseSensitivity = start_sensivity;
        SingleUtils.person.useHeadbob = true;
        stairs = false;
        SingleUtils.person._crouchModifiers.useCrouch = true;
        SingleUtils.person.canJump = true;
        if (!grabing_on) time = 0;
        SingleUtils.gun_slot.ChangeGunState(GunState.two_hand);
        SingleUtils.gun_slot.SetGunActive();

        //SingleUtils.person.playerCanMove = true;

        SingleUtils.person.ControllerPause();
        if (!SingleUtils.person.enableCameraMovement) SingleUtils.person.CameraUnpause();
        //player.GetComponent<CapsuleCollider>().enabled = true;
        rigid.isKinematic = false;
        rigid.useGravity = true;

        grabing = false;
        grabing_on = false;
        triggered_border = false;
        //Physics.IgnoreCollision(SingleUtils.person.capsule, grabed_collider, false);
        SingleUtils.person.fps_Rigidbody.detectCollisions = true;
        //SingleUtils.person.capsule.height = size.y;
        //SingleUtils.person.capsule.radius = si;
    }

    void StopPlayer()
    {
        time = 0;
        rigid.useGravity = false;
        rigid.isKinematic = true;
        rigid.velocity = Vector3.zero;

        //  Debug.Break();
        //Physics.IgnoreCollision(SingleUtils.person.capsule, grabed_collider, true);
        SingleUtils.person.fps_Rigidbody.detectCollisions = false;
        //SingleUtils.person.capsule.height /= 2;

        //SingleUtils.person.playerCanMove = false;
        //SingleUtils.person.ControllerPause();

    }

    bool for_border_on_one_point;
    public void SetBorderForOnePoint() { for_border_on_one_point = true; }
    public void GrabingForBorder(Transform border)
    {
        if (SingleUtils.obj_getter.grabed)
            SingleUtils.obj_getter.Ungrab();
        ledge = SingleUtils.ledge;
        if (Player.Inventory.MyGuns.gun != null)
        {
            Player.Inventory.MyGuns.gun.Stop();
        }
        SingleUtils.person.camera_anim.SetTrigger("Border");
        SingleUtils.gun_slot.ChangeGunState(GunState.one_hand);
        grabed_collider = border.GetComponent<Collider>();
        MakeSizeOnEnter(grabed_collider);
        grabed = border;

        Vector3 player_in_up = MakePlayerInUp();
        closest_collider_position = grabed_collider.ClosestPoint(player_in_up);
        // float hord = Vector3.Distance(grabed.transform.position, new Vector3(closest_collider_position.x, grabed.transform.position.y, closest_collider_position.z));

        dash = true;
        help_velocity = rigid.velocity;
        StopPlayer();

        grabing = true;
        Vector3 offset_pos = NewWave(closest_collider_position, player_in_up);
        if (for_border_on_one_point)
        {
            GetInUp(offset_pos, 0);
            for_border_on_one_point = false;
        }
        else
        {
            GetInUp(offset_pos, ledge * 2);
        }

    }


    void GetInUp(Vector3 h_position, float hord)
    {

        start_camera_rotation = SingleUtils.person.playerCamera.transform.rotation;
        //SingleUtils.person.enableCameraMovement = false;
        SingleUtils.person.useHeadbob = false;
        float horder = 0;
        bool checker = true;
        if (hord == -1)
        {
            //horder = Vector3.Distance(grabed.transform.position, new Vector3(closest_collider_position.x, grabed.transform.position.y, closest_collider_position.z));
            checker = true;
            //checker = hord >= ledge;
        }
        else
        {
            horder = hord;
        }
        if (checker)
        {
            // Debug.Log("Checker 2");
            grabing_on = true;
            time = 0;
            help_position = h_position;


            Vector3 clos = grabed_collider.ClosestPoint(transform.position);
            closest_collider_position = grabed.InverseTransformPoint(clos);


            new_position = MakeGrabedOnPosition(clos, horder);
            help_position = player.position;
            grabing = false;
        }

        //player.GetComponent<CapsuleCollider>().enabled = false;
    }
    public bool stairs_stop;

    float start_sensivity;
    Quaternion start_camera_rotation;
    Quaternion finish_camera_rotation;

    void FixedUpdate()
    {
        //if(Input.GetKeyDown(KeyCode.J)) StartCoroutine(Health.Healing(3,10));
        //if(Input.GetKeyDown(KeyCode.V)) Health.BulletHit();
        // if (Input.GetKeyDown(KeyCode.J)) SingleUtils.person.ControllerPause();



        if (grabing)
        {

            SingleUtils.person.capsule.height = Mathf.MoveTowards(SingleUtils.person.capsule.height, SingleUtils.person._crouchModifiers.colliderHeight, 5 * Time.deltaTime);
            if (SingleUtils.person.staminaInternal <= 0)
            {
                Fall();
            }
            bool pressed_down_jump = Input.GetButton("Jump");
            float first_angle = Vector3.Angle(SingleUtils.player.forward, (closest_collider_position - SingleUtils.player.position));
            if (first_angle < 130)
            {
                float angle = first_angle * Mathf.PI / 180;
                SingleUtils.person.mouseSensitivity = start_sensivity * Mathf.Cos(angle / 1.5f);
            }
            if (stairs)
            {

                //rigid.velocity = Vector3.zero;
                float up = Input.GetAxis("Vertical");
                //SingleUtils.person.fps_Rigidbody.MovePosition(help_position);
                //SingleUtils.person.fps_Rigidbody.MovePosition(help_position);



                //SingleUtils.person.fucking_velocity = (help_position - SingleUtils.player.position) * 10;
                Vector3 go = (help_position - SingleUtils.player.position);
                SingleUtils.person.fps_Rigidbody.MovePosition(Vector3.Lerp(SingleUtils.player.position, SingleUtils.player.position + go *
                SingleUtils.person.walkSpeedInternal * 2, Time.deltaTime));
                //SingleUtils.person.fps_Rigidbody.velocity = (help_position - SingleUtils.player.position) * 10;



                Vector3 buf = help_position;
                if (!pressed_down_jump)
                    help_position += grabed.up * Input.GetAxis("Vertical") * Time.deltaTime * 4;
                if (Vector3.Distance(help_position, SingleUtils.player.position) >= 0.5f) help_position = buf;
                Vector3 local = grabed.InverseTransformPoint(help_position);

                //if(SingleUtils.person.fps_Rigidbody.velocity.y!=0) stairs_stop = false;
                if (local.y < (-size.y / 2) - player_height * 0.8f || local.y > (size.y / 2) + player_height * 0.8f)
                {
                    Debug.Log("Stop");
                    Fall();

                    SingleUtils.person.fps_Rigidbody.velocity = SingleUtils.player.forward * Input.GetAxis("Vertical") * 5;
                    help_position = buf;
                }
                /*
                else if (local.y > (size.y / 2))
                {
                    Transform going_up_point = grabed.GetChild(grabed.childCount - 1);
                    time = 0;
                    grabing_on = true;
                    help_position = player.position;
                    new_position = going_up_point.localPosition;
                    grabing = false;
                    stairs = false;
                    SingleUtils.person.fucking_velocity = Vector3.zero;

                }*/
                //Debug.Log(local);

                //player.position = Vector3.Lerp(player.position, help_position, Time.deltaTime * 10);

                //rigid.drag = 20;
                //rigid.velocity = (help_position - player.position)*2;


                //  if (up != 0) help_position += grabed.up * Time.deltaTime * Input.GetAxis("Vertical") * 5;

                //if (Input.GetButton("Jump"))
                //{


                // Fall();

                //}
                if (Input.GetButtonUp("Jump") && SingleUtils.person.staminaInternal > 3)
                {
                    SingleUtils.person.staminaInternal -= 3;
                    Fall();
                    SingleUtils.person.fps_Rigidbody.velocity = SingleUtils.person.transform.right * Input.GetAxis("Horizontal") +
                    SingleUtils.person.transform.forward * Input.GetAxis("Vertical") + new Vector3(0, SingleUtils.person.jumpPower, 0);
                }
            }
            else
            {

                if (time <= 1)
                {
                    if (dynamic_grabed_body)
                    {
                        Vector3 player_in_up = MakePlayerInUp();


                        closest_collider_position = grabed_collider.ClosestPoint(player_in_up);


                        new_position = NewWave(closest_collider_position, player_in_up);
                    }

                    time += Time.deltaTime + Time.deltaTime * 2 * (SingleUtils.person.staminaInternal * 2 / 100);
                    SingleUtils.person.fps_Rigidbody.MovePosition(Vector3.Slerp(help_position, new_position, time));
                    //player.position = Vector3.Slerp(help_position, new_position, time);
                    SingleUtils.person.fps_Rigidbody.MoveRotation(Quaternion.Slerp(help_rotation, new_rotation, time));

                    //player.rotation = Quaternion.Slerp(help_rotation, new_rotation, time);
                }
                else if (time > 1)
                {

                    // closest_collider_position = grabed_collider.ClosestPoint(SingleUtils.player.position);
                    if (!pressed_down_jump)
                    {

                        //Debug.Log(SingleUtils.person.mouseSensitivity);
                        //Quaternion look = Quaternion.LookRotation((closest_collider_position - SingleUtils.player.position), Vector3.up);
                        //SingleUtils.person.start_clamp_horizotal_angles = look.y;

                        if (Input.GetAxis("Vertical") > 0)
                        {

                            if (Vector3.Angle(SingleUtils.player.forward, closest_collider_position - SingleUtils.player.position) <= 45)
                            {
                                SingleUtils.person.camera_anim.SetTrigger("GrabOn");
                                GetInUp(player.position, -1);
                            }
                        }
                        else if (Input.GetAxis("Horizontal") != 0)
                        {


                            float y = player.position.y;
                            closest_collider_position = grabed_collider.ClosestPoint(player.position);
                            vector_forward = closest_collider_position - player.position;
                            Vector3 vector_right = Vector3.Cross(vector_forward, -player.up * Input.GetAxis("Horizontal"));
                            if (!Physics.Raycast(SingleUtils.player.position, vector_right, 0.5f))
                            {
                                player.position += (vector_right) * 2 * Time.deltaTime;
                            }

                        }
                    }
                    if (Input.GetButtonUp("Jump") && SingleUtils.person.staminaInternal > 3)
                    {
                        SingleUtils.person.staminaInternal -= 3;
                        Fall();
                        SingleUtils.person.fps_Rigidbody.velocity = SingleUtils.person.transform.right * Input.GetAxis("Horizontal") +
                        SingleUtils.person.transform.forward * Input.GetAxis("Vertical") + new Vector3(0, SingleUtils.person.jumpPower, 0);
                    }
                }
                if (dynamic_grabed_body)
                    player.position += grabed.GetComponent<Rigidbody>().velocity * Time.deltaTime;
                SingleUtils.person.staminaInternal -= 3 * Time.deltaTime;
            }
            if ((Input.GetKeyUp(KeyCode.F) || SingleUtils.person.staminaInternal <= 0) && !pressed_down_jump)
            {
                //if(grabing)SingleUtils.person.mouseSensitivity*=2;
                Fall();
                if (stairs) stairs = false;
            }
            if (Input.GetKeyUp(KeyCode.B))
            {
//                SingleUtils.no_control.FuckingFall(-SingleUtils.player.forward, SingleUtils.player.position);
            }
        }
        else
        {

            if (grabing_on)
            {
                SingleUtils.person.capsule.height = Mathf.MoveTowards(SingleUtils.person.capsule.height, SingleUtils.person._crouchModifiers.colliderHeight, 5 * Time.deltaTime);
                Vector3 finish_pos = grabed.TransformPoint(new_position);
                //Time.timeScale = 0.2f;
                //Time.fixedDeltaTime = 0.02f * Time.timeScale;
                //  Debug.Log(time);
                if (time <= 1)
                {

                    /*
                    if (time <= 0.5f)
                    {
                        SingleUtils.person.playerCamera.transform.rotation = Quaternion.Slerp(SingleUtils.person.playerCamera.transform.rotation,
                    Quaternion.LookRotation(closest_collider_position - SingleUtils.player.position), Time.deltaTime * 5);
                        finish_camera_rotation= SingleUtils.person.playerCamera.transform.rotation;
                    }
                    else if (time > 0.5f)
                    {
                        SingleUtils.person.playerCamera.transform.rotation = Quaternion.Slerp(finish_camera_rotation,start_camera_rotation,(time-0.5f)*2);
                    }*/


                    time += Time.deltaTime + Time.deltaTime * 2 * (SingleUtils.person.staminaInternal * 2 / 100);

                }
                else if (time >= 1)
                {
                    SingleUtils.person.useHeadbob = true;
                    // SingleUtils.person.enableCameraMovement = true;
                    Fall();
                    if (dash)
                    {
                        dash = false;
                        rigid.velocity = help_velocity / 2;
                        SingleUtils.on_trigger_border = false;
                        SingleUtils.gun_slot.ChangeGunState(GunState.two_hand);
                    }
                    grabing = false;
                    grabing_on = false;


                }

                debug.transform.position = new_position;
                Vector3 new_rigid_pos = Vector3.Slerp(help_position, finish_pos, time);

                SingleUtils.person.fps_Rigidbody.MovePosition(new_rigid_pos);
                //player.position = Vector3.Slerp(help_position, finish_pos, time);
                if (SingleUtils.person.staminaInternal < 0) Fall();
            }
            else
            {
                /* if(Input.GetButtonDown("Jump")&&Mathf.Abs(Vector3.Angle(person.transform.forward,SingleUtils.border.GetComponent<Collider>().ClosestPoint(person.transform.position) - person.transform.position))<30&&
                 Input.GetAxis("Vertical")>0&&person.IsGrounded&&!dash){

                     Debug.Log(person.IsGrounded);
                     GrabingForBorder(SingleUtils.border);
                 }*/
            }

            if (triggered_border)
            {
                Vector3 player_in_up = MakePlayerInUp();
                closest_collider_position = grabed_collider.ClosestPoint(player_in_up);

                if (Input.GetKeyDown(KeyCode.F) && SingleUtils.person.staminaInternal > 3)
                {
                    bool can = true;
                    if(SingleUtils.obj_getter.grabed&&SingleUtils.obj_getter.obj_trans == grabed){
                        can = false;
                    }
                    if (can)
                    {
                        SingleUtils.person.staminaInternal -= 3;
                        //SingleUtils.person.mouseSensitivity/=2;
                        // SingleUtils.person.clam_horizontal_rotation = true;
                        //Vector3 cresos = Vector3.Cross(SingleUtils.player.forward, closest_collider_position - SingleUtils.player.position);
                        //float k = (cresos.y / Mathf.Abs(cresos.y));
                        //SingleUtils.person.start_clamp_horizotal_angles = Vector3.Angle(SingleUtils.player.forward, closest_collider_position - SingleUtils.player.position) * k + SingleUtils.player.rotation.eulerAngles.y;

                        SingleUtils.person.camera_anim.SetTrigger("Grab");
                        SingleUtils.person.useHeadbob = false;
                        SingleUtils.person._crouchModifiers.useCrouch = false;

                        SingleUtils.gun_slot.ChangeGunState(GunState.hide);
                        grabing = true;

                        StopPlayer();
                        help_position = player.position;

                        new_position = NewWave(closest_collider_position, player_in_up);

                        time = 0;
                        help_rotation = player.rotation;
                        new_rotation = NewWaveRotation(closest_collider_position, player_in_up);

                    }


                }
            }
            else if (stairs)
            {
                if (Input.GetKeyDown(KeyCode.F) && Vector3.Distance(SingleUtils.player.position, grabed_collider.ClosestPoint(SingleUtils.player.position)) <= 1)
                {
                    SingleUtils.gun_slot.ChangeGunState(GunState.hide);
                    SingleUtils.person.fps_Rigidbody.useGravity = false;
                    //SingleUtils.person.enabled = false;Physics.IgnoreCollision
                    //SingleUtils.person.playerCanMove = false;
                    SingleUtils.person.ControllerPause(false);
                    //SingleUtils.person.canJump = false;
                    grabing = true;
                    Vector3 closest_local_point = grabed.InverseTransformPoint(grabed_collider.ClosestPoint(player.position));

                    Vector3 loc = grabed.TransformPoint(new Vector3(1, closest_local_point.y, 0));
                    //grabed.up
                    help_position = loc;
                    // Physics.IgnoreCollision(SingleUtils.person.capsule, grabed_collider, true);




                }
            }
        }
    }
    float stair_length;



    Vector3 MakePlayerInUp()
    {
        float y = grabed.transform.position.y + (size.y - player_height) / 2;
        return new Vector3(player.position.x, y, player.position.z);
    }

    Quaternion NewWaveRotation(Vector3 closest_point, Vector3 player_in_up)
    {

        return Quaternion.LookRotation((closest_point - player_in_up).normalized);
    }
    Vector3 NewWave(Vector3 closest_point, Vector3 player_in_up)
    {


        Vector3 to_player_vector = (player_in_up - closest_point).normalized;
        vector_forward = -to_player_vector;
        float offset = 1;
        Vector3 grabed_position = closest_point + to_player_vector * offset;

        return grabed_position;
    }


    Vector3 MakeGrabedOnPosition(Vector3 closest_point, float hord)

    {
        float y = grabed.transform.position.y + size.y / 2 + player_height / 2 + 0.1f;
        float pos_k = ledge;
        if (SingleUtils.on_trigger_border) pos_k = hord + ledge;
        Vector3 grabed_on_position = closest_point + (closest_point - player.position).normalized * pos_k;
        grabed_on_position = new Vector3(grabed_on_position.x, y, grabed_on_position.z);

        return grabed.InverseTransformPoint(grabed_on_position);
    }






}

