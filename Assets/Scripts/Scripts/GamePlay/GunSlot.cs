using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
using Player.Inventory;

public enum GunState
{
    down,
    hide,
    aim,
    one_hand,
    two_hand,

}
public class GunSlot : MonoBehaviour
{

    [SerializeField]
    float m_BulletSpeed;
    //Gun gun;


    [SerializeField]
    bool fire_pressed;
    //public float Timed;
    //Gun[] guns;

    //public Transform MyGuns.guns_transform;



    float zoom_time;



    public bool showFps = true;
    [SerializeField]
    private bool active;



    public void SetGunActive() { active = true; }
    void OnGUI()
    {
        if (showFps)
        {
            SingleUtils.FPSCounter();
        }
    }


    Vector3 one_hand_pos;
    Vector3 two_hand_pos;
    Vector3 zoom_pos = new Vector3(0, -0.15f, 1);
    Vector3 down_pos;
    Vector3 hide_pos;

    Vector3 zoom_point1;
    Vector3 zoom_point2 = new Vector3(0, -0.15f, 1);


    float fow1;
    float fow2;


    public float standartFow = 60f;
    void Start()
    {

        MyGuns.ammos[0] = new Ammo(5, 0, 0);
        MyGuns.ammos[1] = new Ammo(5, 0, 1);
        //first_ammo = new Ammo(0, 0);
        //second_ammo = new Ammo(0, 0);
        /*
            MyGuns.AddGun(new Gun(12, MyGuns.guns_transform.GetChild(0).gameObject,
               MyGuns.bullet_rigid, MyGuns.guns_transform.GetChild(0).GetComponent<Animator>(), false, 0.2f, 2, new Vector3(5, 5, 5) / 100));

            MyGuns.SetLastBullets(10,  MyGuns.ammos[0]);
            MyGuns.AddGun(new Gun(30, MyGuns.guns_transform.GetChild(1).gameObject,
                MyGuns.bullet_rigid, MyGuns.guns_transform.GetChild(1).GetComponent<Animator>(), true, 0.1f, 4, new Vector3(10, 10, 10) / 100));
            MyGuns.SetLastBullets(10, MyGuns.ammos[1]);
            MyGuns.gun = MyGuns.GetGun(0);
            MyGuns.choosed = 0;
            */
        // bullet_tail = MyGuns.bullet_transform.GetComponent<LineRenderer>();
        //MyGuns.gun.SetBulletSpeed(m_BulletSpeed);


        //two_hand_pos = MyGuns.gun.gun_object.transform.localPosition;
        two_hand_pos = new Vector3(0.57f, -0.44f, 1.31f);
        one_hand_pos = two_hand_pos + new Vector3(0.5f, -0.1f);
        down_pos = two_hand_pos + new Vector3(-0.5f, -2, 0.5f);
        hide_pos = zoom_pos - new Vector3(0, 1, 0);

        rotationLastX = SingleUtils.camera.rotation.x;
        rotationLastY = transform.rotation.y;
        active = true;

        //ChangeGunState(GunState.two_hand);
    }
    int close_damage = 0;
    public int GetCloseDamage() { return close_damage; }
    void CloseDamage()
    {

        close_damage = 1;
        if (MyGuns.gun.reloading || MyGuns.gun.shoot)
        {
            MyGuns.gun.Stop();

        }
        int rand0m_ = Random.Range(0, 3);
        //MyGuns.gun.gun_anim.SetTrigger("Hand");
        MyGuns.gun.gun_anim.Play("Hand" + rand0m_);



    }
    void ChooseGun()
    {
        int count = MyGuns.GetGunsCount();
        if (count > 1)
        {
            int mew = MyGuns.choosed;
            if (Input.mouseScrollDelta.y >= 1)
            {
                mew += 1;
                if (mew > count - 1) mew = 0;
                MyGuns.ReGun(mew);
            }
            else if (Input.mouseScrollDelta.y <= -1)
            {
                mew -= 1;
                if (mew < 0) mew = count - 1;
                MyGuns.ReGun(mew);
            }

        }
        if (Input.GetKeyUp(KeyCode.Alpha1) && count > 0)
        {

            MyGuns.ReGun(0);
            MyGuns.gun.SetBulletSpeed(m_BulletSpeed);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2) && count > 1)
        {

            MyGuns.ReGun(1);
            MyGuns.gun.SetBulletSpeed(m_BulletSpeed * 2);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2) && count > 2)
        {
            MyGuns.ReGun(2);
        }


    }

    void SetStats(string value)
    {

        DebugGUI.debug_text.text = value;/*
        if (MyGuns.gun.shoot) DebugGUI.shoot_togle.isOn = true;
        else DebugGUI.shoot_togle.isOn = false;
        if (MyGuns.gun.reloading) DebugGUI.reload_togle.isOn = true;
        else DebugGUI.reload_togle.isOn = false;*/
    }

    /*void BulletTail()
    {
        bullet_tail.SetPosition(0, MyGuns.gun.bullet_transform.position);
        bullet_tail.SetPosition(1, MyGuns.gun.bullet_transform.position + (MyGuns.gun.gun_object.transform.Find("Spawn").position - MyGuns.bullet_transform.position).normalized * MyGuns.gun.GetBulRigidVelocity().magnitude / m_BulletSpeed * 50);
    }*/


    public bool Stats;


    void StartReload()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            MyGuns.gun.GoReloading();
            if (MyGuns.gun.reloading)
            {
                MyGuns.gun.GoAnimeReload();
                active = false;
                ChangeGunState(GunState.two_hand);
            }
        }
    }

    Vector3 pos1 = Vector3.zero;
    Vector3 pos2 = Vector3.zero;
    int k;
    public void ChangeGunState(GunState state)
    {
        if (MyGuns.gun != null)
        {
            zoom_time = 0;
            pos1 = MyGuns.gun.gun_object.transform.localPosition;
            fow1 = SingleUtils.person.playerCamera.fieldOfView;
            switch (state)
            {
                case GunState.aim:
                    MyGuns.gun.NormalizeOffset();
                    pos2 = zoom_pos;

                    fow2 = 45;
                    k = 8;
                    break;
                case GunState.down:

                    pos2 = down_pos;
                    fow1 = SingleUtils.person.playerCamera.fieldOfView;
                    fow2 = standartFow;
                    k = 8;
                    break;
                case GunState.hide:
                    pos2 = hide_pos;
                    k = 8;
                    fow2 = standartFow;
                    active = false;
                    MyGuns.gun.Stop();

                    break;
                case GunState.one_hand:
                    MyGuns.gun.NormalizeOffset();
                    MyGuns.gun.offset *= 2.5f;
                    pos2 = one_hand_pos;
                    k = 8;
                    fow2 = standartFow;
                    break;
                case GunState.two_hand:
                    MyGuns.gun.NormalizeOffset();
                    MyGuns.gun.offset *= 1.5f;
                    pos2 = two_hand_pos;
                    k = 8;
                    fow2 = standartFow;
                    break;
            }
            SingleUtils.gun_state = state;
        }
    }
    void FixedUpdate()
    {
        RotationGun();

    }
    void Update()
    {

        if (MyGuns.GetGunsCount() > 0)
        {
            if (!active)
            {
                if (MyGuns.gun.reloading)
                {

                    MyGuns.gun.delta_time += Time.deltaTime;
                    if (MyGuns.gun.delta_time >= 3)
                    {
                        MyGuns.gun.Reload();
                        active = true;
                        MyGuns.gun.reloading = false;
                        MyGuns.gun.delta_time = 0;

                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        ChangeGunState(GunState.two_hand);
                        active = true;


                    }
                }
            }
            else
            {
                if (close_damage == 1)
                {
                    MyGuns.gun.delta_time = MyGuns.gun.gun_anim.GetCurrentAnimatorStateInfo(0).length;
                    close_damage = 2;
                    Debug.Log(MyGuns.gun.delta_time);
                }
                else
                if (close_damage == 2)
                {
                    MyGuns.gun.delta_time -= Time.deltaTime;
                    if (MyGuns.gun.delta_time <= 0)
                    {
                        MyGuns.gun.Stop();
                        close_damage = 0;
                    }


                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        CloseDamage();
                    }
                    fire_pressed = Input.GetButton("Fire1");
                    if (MyGuns.gun.shoot)
                    {
                        MyGuns.gun.delta_time += Time.smoothDeltaTime;
                        if (MyGuns.gun.non_stop)
                        {
                            if (MyGuns.gun.delta_time >= MyGuns.gun.interval_time)
                            {
                                MyGuns.gun.gun_anim.SetBool("Shoot", false);
                                if (!fire_pressed)
                                {
                                    MyGuns.gun.shoot = false;
                                    //person.cameraSmoothing = 5;
                                }
                                else
                                {
                                    MyGuns.gun.Shoot();
                                }
                                MyGuns.gun.delta_time = 0;


                            }
                        }
                        else
                        {
                            if (MyGuns.gun.delta_time >= MyGuns.gun.interval_time)
                            {
                                //person.cameraSmoothing = 5;
                                MyGuns.gun.delta_time = 0;
                                MyGuns.gun.shoot = false;
                            }
                        }
                    }
                    else
                    {
                        ChooseGun();
                        if (Input.GetButtonDown("Fire1"))
                        {
                            MyGuns.gun.Shoot();
                            MyGuns.gun.delta_time = 0;

                        }

                    }
                    switch (SingleUtils.gun_state)
                    {
                        case GunState.aim:
                            StartReload();
                            if (Input.GetMouseButtonUp(1))
                            {
                                ChangeGunState(GunState.two_hand);
                            }
                            break;
                        case GunState.one_hand:
                            if (Input.GetKeyUp(KeyCode.L)) ChangeGunState(GunState.two_hand);
                            break;
                        case GunState.two_hand:
                            StartReload();
                            if (Input.GetMouseButton(1)) ChangeGunState(GunState.aim);
                            if (Input.GetKeyUp(KeyCode.L)) ChangeGunState(GunState.one_hand);
                            if (Input.GetKeyDown(KeyCode.X)) ChangeGunState(GunState.hide);
                            break;


                        case GunState.down:


                            break;


                    }
                }
            }
            zoom_time += Time.deltaTime * k;

            MyGuns.gun.gun_object.transform.localPosition = Vector3.Slerp(pos1, pos2, zoom_time);


           //SingleUtils.person.playerCamera.fieldOfView = Mathf.Lerp(fow1, fow2, zoom_time);
            //SingleUtils.person.playerCamera.fieldOfView = Mathf.Clamp(SingleUtils.person.playerCamera.fieldOfView, 45, standartFow);

            zoom_time = Mathf.Clamp01(zoom_time);



            SetStats(MyGuns.gun.GetStats());
            // BulletTail();

            if (Input.GetKeyDown(KeyCode.G))
            {
                if (MyGuns.GetGunsCount() > 0)
                {

                    GameObject droping_gun = MyGuns.guns_transform.GetChild(MyGuns.choosed).gameObject;
                    Destroy(droping_gun.GetComponent<ClosDamageTrigger>());
                    ChangeLayer(droping_gun.transform, 11);
                    droping_gun.GetComponent<BoxCollider>().enabled = true;
                    Rigidbody drop_rigid = droping_gun.AddComponent<Rigidbody>();

                    droping_gun.transform.parent = null;
                    drop_rigid.AddForce(transform.forward * 100);

                    GunStruct stor = droping_gun.AddComponent<GunStruct>();

                    Gun drp_gun = MyGuns.GetGun(MyGuns.choosed);
                    drp_gun.Stop();
                    active = true;
                    stor.delta = drp_gun.interval_time;
                    stor.kick = drp_gun.GetKick();
                    stor.offset = drp_gun.offset;
                    stor.non_stop = drp_gun.non_stop;
                    stor.bullet_max = drp_gun.GetBulletMax();
                    stor.ammo_number = drp_gun.GetAmmoId();
                    stor.ammo_count = drp_gun.GetBulletsNow();

                    MyGuns.RemoveGun(MyGuns.choosed);
                    MyGuns.gun = null;
                }
                if (MyGuns.GetGunsCount() > 0)
                {
                    MyGuns.ReNewGun(MyGuns.GetGunsCount() - 1);
                }

            }
        }


        /*
                RaycastHit hit;//, ~(1 << LayerMask.NameToLayer("Ignore Raycast")
                if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out hit, 10))
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        GameObject hitted = hit.collider.gameObject;
                        if (hitted.tag == "Ammo1")
                        {
                            MyGuns.ammos[0].bullets_stacks += 1;
                            Destroy(hitted.gameObject);
                        }
                        else if (hitted.tag == "Ammo2")
                        {
                            MyGuns.ammos[1].bullets_stacks += 1;
                            Destroy(hitted.gameObject);
                        }
                        else if (hitted.tag == "Gun")
                        {
                            if (MyGuns.GetGunsCount() < 3)
                            {
                                if (MyGuns.GetGunsCount() > 0)
                                {
                                    if (!MyGuns.gun.shoot&&!MyGuns.gun.reloading && SingleUtils.gun_state == GunState.two_hand)
                                    {
                                        ChangeLayer(hitted.transform, 9);
                                        // Destroy(hitted.GetComponent<BoxCollider>());
                                        hitted.GetComponent<BoxCollider>().isTrigger = true;
                                        Destroy(hitted.GetComponent<Rigidbody>());
                                        MyGuns.CheckGun(hitted, MyGuns.guns_transform, hide_pos);
                                        ChangeGunState(GunState.two_hand);
                                        Destroy(hitted.GetComponent<GunStruct>());
                                    }
                                }
                                else
                                {

                                    ChangeLayer(hitted.transform, 9);
                                    hitted.GetComponent<BoxCollider>().isTrigger = true;
                                    Destroy(hitted.GetComponent<Rigidbody>());
                                    MyGuns.CheckGun(hitted, MyGuns.guns_transform, hide_pos);
                                    ChangeGunState(GunState.two_hand);
                                    Destroy(hitted.GetComponent<GunStruct>());

                                }
                            }



                        }
                    }

                }
        */
        // Timed = gun.delta_time;
    }
    public Vector3 GetHidePos()
    {
        return hide_pos;
    }
    public void ChangeLayer(Transform obj, int layer)
    {
        for (int i = 0; i < obj.childCount; i++)
        {
            Transform obj_chile = obj.GetChild(i);
            obj_chile.gameObject.layer = layer;
            if (obj_chile.transform.childCount > 0)
            {
                ChangeLayer(obj_chile, layer);
            }
        }
    }
    [Header("Rotation")]
    private Vector2 velocityGunRotate;
    private float gunWeightX, gunWeightY;
    [Tooltip("The time waepon will lag behind the camera view best set to '0'.")]
    public float rotationLagTime = 0f;
    private float rotationLastY;
    private float rotationDeltaY;
    private float angularVelocityY;
    private float rotationLastX;
    private float rotationDeltaX;
    private float angularVelocityX;
    [Tooltip("Value of forward rotation multiplier.")]
    public float delta_time_k = 5;
    public Vector2 forwardRotationAmount = Vector2.one;
    /*
    * Rotatin the weapon according to mouse look rotation.
    * Calculating the forawrd rotation like in Call Of Duty weapon weight
    */





    void RotationGun()
    {
        Vector2 rot = new Vector2(SingleUtils.person.followAngles.x, SingleUtils.person.followAngles.y);
        rotationDeltaY = rot.y - rotationLastY;
        rotationDeltaX = rot.x - rotationLastX;

        rotationLastY = rot.y;
        rotationLastX = rot.x;

        angularVelocityY = Mathf.Lerp(angularVelocityY, rotationDeltaY, Time.deltaTime * delta_time_k);
        angularVelocityX = Mathf.Lerp(angularVelocityX, rotationDeltaX, Time.deltaTime * delta_time_k);

        gunWeightX = Mathf.SmoothDamp(gunWeightX, rot.x, ref velocityGunRotate.x, rotationLagTime);
        gunWeightY = Mathf.SmoothDamp(gunWeightY, rot.y, ref velocityGunRotate.y, rotationLagTime);

        //transform.rotation = Quaternion.Euler (gunWeightX + (angularVelocityX*forwardRotationAmount.x), gunWeightY + (angularVelocityY*forwardRotationAmount.y), 0);
        MyGuns.guns_transform.localRotation = Quaternion.Euler((angularVelocityX * forwardRotationAmount.x), -(angularVelocityY * forwardRotationAmount.y), 0);
    }


}




public struct Ammo
{
    public int id;
    public int ost;
    public int bullets_stacks;
    public Ammo(int stacks, int o, int i)
    {
        ost = o;
        id = i;
        bullets_stacks = stacks;
    }
}
public class Gun
{
    public Animator gun_anim;
    float bul_speed = 1000;
    public GameObject gun_object;
    //Rigidbody bullet_object;
    int bullets_now;
    int bullets_max;

    public bool reloading;
    public bool shoot;
    public float delta_time;
    private Vector3 start_offset;
    public Vector3 offset;

    public bool non_stop;
    public float interval_time;

    float kick;
    public string name;
    Ammo ammo;

    Vector3 bul_velocity;
    public Gun(int bullets_max, GameObject gun_object, Animator gun_anim, bool non_Stop, float delta, float kick, Vector3 offset)
    {

        //this.bullet_object = bullet_object;
        this.gun_object = gun_object;
        this.bullets_max = bullets_max;
        delta_time = 0;
        this.gun_anim = gun_anim;
        non_stop = non_Stop;
        interval_time = delta;

        this.kick = kick;
        this.offset = offset;
        start_offset = offset;
    }
    public int GetAmmoId() { return ammo.id; }
    public int GetBulletsNow() { return bullets_now; }
    public int GetBulletMax() { return bullets_max; }
    public float GetKick() { return kick; }
    public void NormalizeOffset()
    {
        offset = start_offset;
    }
    public Vector3 GetBulRigidVelocity()
    {
        return bul_velocity;
    }
    public void SetBulletSpeed(float s)
    {

        bul_speed = s;
    }


    public void SetBullets(int bullets_now, Ammo ammo)
    {


        this.ammo = ammo;
        this.bullets_now = bullets_now;

    }

    public void Shoot()
    {

        bool answ = false;
        if (non_stop) answ = true;
        else answ = !shoot;

        if (!reloading && answ)
        {

            if (bullets_now > 0)
            {
                delta_time = 0;
                if (!non_stop)
                    gun_anim.SetTrigger("Shoot");
                else gun_anim.SetBool("Shoot", true);

                SingleUtils.person.targetAngles += new Vector3(kick, 0, 0);
                // person.cameraSmoothing = 1;

                shoot = true;
                bullets_now -= 1;


                GameObject bul = SingleUtils.bullets_pool.Spawn(gun_object.transform.Find("Spawn").position, Quaternion.identity);
                Bullet bul_script = bul.GetComponent<Bullet>();
                bul_script.bullet_speed = bul_speed;
                bul_script.spawn = gun_object.transform.parent.position;
                //if (!bullet_object.gameObject.activeSelf) bullet_object.gameObject.SetActive(true);

                // bullet_object.velocity = Vector3.zero;
                //bullet_object.gameObject.transform.position = gun_object.transform.position + gun_object.transform.forward * 2;
                //bullet_object.velocity = Vector3.zero;

                //bullet_object.gameObject.transform.position = gun_object.transform.Find("Spawn").position;

                bul_script.rigid.AddForce((Camera.main.transform.forward + new Vector3(Random.Range(-offset.x, offset.x), Random.Range(-offset.y, offset.y), Random.Range(-offset.z, offset.z))) * bul_speed);
                bul_velocity = bul_script.rigid.velocity;
                //Debug.Log(bullet_object.velocity);



            }
            // GetStats();
        }
    }
    public void Reload()
    {

        int need_ = bullets_max - bullets_now;
        if (ammo.ost < need_)
        {
            if (ammo.bullets_stacks > 0)
            {
                ammo.bullets_stacks -= 1;
                ammo.ost += bullets_max - need_;
                bullets_now = bullets_max;
            }
            else
            {
                bullets_now += ammo.ost;
                ammo.ost = 0;
            }
        }
        else
        {
            bullets_now += need_;
            ammo.ost -= need_;
        }

    }
    public void GoReloading()
    {
        if (!reloading && !shoot)
        {
            if (bullets_now >= 0 && bullets_now < bullets_max)
            {

                reloading = true;

            }

        }
    }
    public void GoAnimeReload()
    {
        delta_time = 0;
        gun_anim.SetTrigger("Reload");
    }

    public void Stop()
    {
        shoot = false;
        reloading = false;
        delta_time = 0;

        gun_anim.Rebind();
        //gun_anim.StopPlayback();

    }
    public string GetStats()
    {
        return "bullets_now: " + bullets_now + "\nstacks:" + ammo.bullets_stacks + "\nost: " + ammo.ost;

    }
    public void AddBulletsStack()
    {
        ammo.bullets_stacks += 1;
    }


}