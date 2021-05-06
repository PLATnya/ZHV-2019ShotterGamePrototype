using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
using Player.Inventory;
using UnityEngine.Events;
public class GetObjects : MonoBehaviour
{
    //   float time;

    public bool grabed;
    Rigidbody obj_rigid;
    [SerializeField]
    public Transform obj_trans;
    Vector3 point;
    float start_drag;
    [SerializeField]
    float force;

    bool forcing;
    public float max_force;
    [SerializeField]
    bool fly;

    bool fly_rot;
    public float delta_fly;
    float delta_x;
    float delta_y;


    float start_mouse_sens;
    float start_angular_drag;

    int grabed_type;


    Vector3 start_forward;

    // Start is called before the first frame update
    void Start()
    {
        //start_mouse_sens = SingleUtils.person.mouseSensitivity;
        start_mouse_sens = SingleUtils.person.mouseSensitivity;
    }

    float Sigmoid(float x)
    {
        return 1 / (1 + Mathf.Exp(-x));
    }

    public void Ungrab()
    {
        SingleUtils.gun_slot.ChangeGunState(GunState.two_hand);
        grabed = false;
        obj_rigid.useGravity = true;
        obj_rigid.drag = start_drag;
        obj_rigid.angularDrag = start_angular_drag;
        forcing = false;
    }
    float grabed_distance;
    /*
    Vector2 _mouseAbsolute;
    Vector2 _smoothMouse;
    Vector2 smoothing = new Vector2(3, 3);
    Vector2 sensitivity = new Vector2(2, 2);
    Vector2 TestStats()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        // Interpolate mouse movement over time to apply smoothing delta.
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

        // Find the absolute mouse movement value from point zero.
        _mouseAbsolute += _smoothMouse;
        Debug.Log(_smoothMouse);
        return _smoothMouse ;

    }*/
    // Update is called once per frame

    void DropWindow()
    {
        WIndowNearBorder win = obj_trans.GetComponent<WIndowNearBorder>();
        if (fly)
        {
            win.OnOpen();
            win.closed = true;
        }
        else
        {
            win.OnClose();
        }
        obj_rigid.useGravity = !fly;
        obj_rigid.velocity = Vector3.zero;


        grabed = false;
    }
    void Update()
    {
        //();


        if (grabed)
        {

            switch (grabed_type)
            {


                case 0:

                    obj_rigid.angularDrag = 10;
                    if(Input.GetKey(KeyCode.X)){
                        obj_rigid.angularVelocity = obj_trans.up*5;
                    }else if(Input.GetKey(KeyCode.V)){
                        obj_rigid.angularVelocity = obj_trans.right*5;
                    }
                    //if (!fly)
                    //{

                    if (Input.GetKey(KeyCode.C))
                    {
                        point = SingleUtils.person.playerCamera.transform.position + SingleUtils.person.playerCamera.transform.forward * 1;
                    }
                    else
                    {
                        point = SingleUtils.person.playerCamera.transform.position + SingleUtils.person.playerCamera.transform.forward * 1 - SingleUtils.person.playerCamera.transform.right * 0.5f;

                    }
                    if (Input.GetMouseButtonDown(1))
                    {
                        forcing = true;

                    }
                    if (forcing && force < max_force)
                    {
                        force += Time.deltaTime * max_force;

                    }/*
                        else if (!forcing)
                        {
                            if (Input.GetKeyDown(KeyCode.F))
                            {
                                fly = true;
                                SingleUtils.person.mouseSensitivity/=3;
                                SingleUtils.person.mouseSensitivity/=3;
                                //control.mouseLook.XSensitivity /= 3;
                                //control.mouseLook.YSensitivity /= 3;
                            }
                        }*/

                    if (Input.GetMouseButtonUp(1))
                    {
                        Ungrab();
                        obj_rigid.AddForce(SingleUtils.person.playerCamera.transform.forward * force);
                        force = 0;
                    }
                    

                    obj_rigid.velocity = (point - obj_trans.position) * 10;
                    if (Input.GetMouseButtonDown(0))
                    {
                        Ungrab();
                    }
                    /*}

                    else
                    {
                        if (fly_rot)
                        {
                            obj_rigid.velocity = (point - obj_trans.position) * 10;
                            SingleUtils.person.mouseSensitivity = 0;
                            //control.mouseLook.XSensitivity = 0;
                            //control.mouseLook.YSensitivity = 0;
                            obj_rigid.angularVelocity = (obj_trans.up * Input.GetAxis("Mouse X") + obj_trans.right * Input.GetAxis("Mouse Y")) * 10;
                            //new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * 10;
                            if (Input.GetMouseButtonUp(1))
                            {
                                fly_rot = false;
                                SingleUtils.person.mouseSensitivity =start_mouse_sens/3;
                                //control.mouseLook.XSensitivity = start_mouse_sens / 3;
                                //control.mouseLook.YSensitivity = start_mouse_sens / 3;
                            }
                        }
                        else
                        {
                            if (Input.GetMouseButtonDown(1))
                            {
                                fly_rot = true;
                            }
                            delta_x = (Sigmoid(Input.GetAxis("Mouse X")) - 0.5f) * delta_fly;
                            delta_y = (Sigmoid(Input.GetAxis("Mouse Y")) - 0.5f) * delta_fly;
                            obj_rigid.velocity = ((point + camera.transform.right * delta_x + camera.transform.up * delta_y) - obj_trans.position) * 5 * (new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude + 1);

                        }


                        if (Input.GetKeyUp(KeyCode.F))
                        {
                            SingleUtils.person.mouseSensitivity = start_mouse_sens;
                            //control.mouseLook.XSensitivity = start_mouse_sens;
                            //control.mouseLook.YSensitivity = start_mouse_sens;
                            fly = false;
                        }
                    }*/
                    break;

                case 1:
                    if (Input.GetKeyUp(KeyCode.F) || Vector3.Distance(obj_trans.position, SingleUtils.person.playerCamera.transform.position) > 5)
                    {

                        DropWindow();

                    }
                    else
                    {

                        obj_rigid.velocity = Vector3.up * Input.GetAxis("Mouse Y") * 3;

                        if (obj_trans.position.y >= point.y + obj_trans.GetComponent<BoxCollider>().size.y) fly = true;
                        else fly = false;
                    }
                    break;

                case 2:

                    obj_rigid.velocity = ((SingleUtils.player.position + SingleUtils.player.forward * 1.5f) - obj_trans.position) * 5;
                    obj_rigid.MoveRotation(Quaternion.LookRotation(new Vector3(SingleUtils.player.position.x, obj_trans.position.y, SingleUtils.player.position.z) - obj_trans.position, Vector3.up));
                    //obj_rigid.AddForce(Vector3.down);
                    if (Input.GetKeyUp(KeyCode.Z))
                    {
                        obj_rigid.isKinematic = true;
                        grabed = false;
                    }


                    break;
            }

        }

        Ray ray = SingleUtils.person.playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        //SingleUtils.person.playerCamera.GetComponent<>
        if (Physics.Raycast(ray, out hit, 5))
        {


            GameObject col = hit.collider.gameObject;
            //Debug.Log(col.name);
            if (col.layer == LayerMask.NameToLayer("Interact"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {


                    if (col.tag == "Ammo1")
                    {
                        MyGuns.ammos[0].bullets_stacks += 1;
                        Destroy(col.gameObject);
                    }
                    else if (col.tag == "Ammo2")
                    {
                        MyGuns.ammos[1].bullets_stacks += 1;
                        Destroy(col.gameObject);
                    }


                    if (!grabed)
                    {

                        if (col.tag == "Interact")
                        {
                            GetObject(hit);

                        }
                        else if (col.tag == "Window")
                        {
                            if (!col.GetComponent<WIndowNearBorder>().closed)
                            {
                                grabed_type = 1;
                                grabed = true;
                                obj_rigid = hit.rigidbody;

                                obj_rigid.useGravity = false;
                                //obj_rigid.isKinematic = true;
                                obj_trans = col.transform;
                                point = obj_trans.position;
                            }

                        }
                        else if (col.tag == "Key")
                        {
                            col.GetComponent<KeyOpen>().Open();

                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (!grabed)
                    {
                        if (col.tag == "OnOBJ" || col.tag == "ClearCover" || col.tag == "ClearBorder" || col.tag == "Cover")
                        {
                            if (!SingleUtils.grab_trigger.grabing && !SingleUtils.grab_trigger.grabing_on)
                            {
                                grabed_type = 2;
                                grabed = true;
                                obj_rigid = hit.rigidbody;
                                obj_rigid.isKinematic = false;
                                obj_trans = col.transform;
                                SingleUtils.gun_slot.ChangeGunState(GunState.hide);
                                start_forward = SingleUtils.player.position - obj_trans.position;
                            }
                        }

                    }
                }
                if (Input.GetKey(KeyCode.F))
                {
                    if (col.tag == "Gun")
                    {
                        if (gettingTime < 1)
                        {
                            gettingTime += Time.deltaTime;
                        }
                        else
                        {
                            GetGun(col);
                            gettingTime = 0;
                        }
                    }
                    else
                    {
                        if (gettingTime > 0) gettingTime = 0;

                    }
                    /*
                    if (col.tag == "Heal")
                    {
                        if (Health.healing_script == null){
                            //StartCoroutine(Health.StartHealing(1));
                            Health.healing_script = col.GetComponent<HealingScript>();
                        }
                        float delta = 0.2f;
                        Health.healing_script.all -= delta;
                        if (delta >= 0){
                            float scale = Health.healing_script.all/Health.healing_script.max;
                            Health.healing_script.heal_obj.localScale = new Vector3(scale,scale,scale);
                            Health.Healing(delta);
                        }
                    }else{
                        if (Health.healing_script != null){
                            Debug.Log("Fuvk");  
                             Health.healing_script = null;
                            // StartCoroutine(Health.StopHealing(1));
                        }
                    }*/
                }
                if (Input.GetKeyUp(KeyCode.F))
                {
                    if (col.tag == "Gun")
                    {
                        if (gettingTime < 1)
                        {
                            GetObject(hit);
                            gettingTime = 0;
                        }
                    }
                    /*else if (col.tag == "Heal")
                    {
                        //StartCoroutine(Health.StopHealing(1));
                        Health.healing_script = null;
                    }*/
                }
            }
            else
            {
                if (gettingTime > 0) gettingTime = 0;
            }
        }

    }
    [SerializeField]
    float gettingTime;
    void GetObject(RaycastHit hit)
    {
        grabed_type = 0;

        SingleUtils.gun_slot.ChangeGunState(GunState.one_hand);
        grabed = true;
        obj_rigid = hit.rigidbody;
        obj_rigid.useGravity = false;
        start_drag = obj_rigid.drag;
        obj_rigid.drag = 10;
        obj_trans = hit.transform;
        obj_rigid.angularVelocity = Vector3.zero;
        max_force /= obj_rigid.mass;
    }
    void GetGun(GameObject col)
    {
        if (MyGuns.GetGunsCount() < 3)
        {
            if (MyGuns.GetGunsCount() > 0)
            {
                if (!MyGuns.gun.shoot && !MyGuns.gun.reloading && SingleUtils.gun_state == GunState.two_hand)
                {
                    SingleUtils.gun_slot.ChangeLayer(col.transform, 9);
                    // Destroy(col.GetComponent<BoxCollider>());
                    col.GetComponent<BoxCollider>().enabled = false;
                    Destroy(col.GetComponent<Rigidbody>());
                    MyGuns.CheckGun(col, MyGuns.guns_transform, SingleUtils.gun_slot.GetHidePos());
                    SingleUtils.gun_slot.ChangeGunState(GunState.two_hand);
                    Destroy(col.GetComponent<GunStruct>());
                }
            }
            else
            {

                SingleUtils.gun_slot.ChangeLayer(col.transform, 9);
                col.GetComponent<BoxCollider>().isTrigger = true;
                Destroy(col.GetComponent<Rigidbody>());
                MyGuns.CheckGun(col, MyGuns.guns_transform, SingleUtils.gun_slot.GetHidePos());
                SingleUtils.gun_slot.ChangeGunState(GunState.two_hand);
                Destroy(col.GetComponent<GunStruct>());

            }
        }
    }
}
