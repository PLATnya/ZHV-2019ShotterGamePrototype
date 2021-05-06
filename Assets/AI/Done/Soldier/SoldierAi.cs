using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Scripts.Utils;
enum Emotions
{
    waiting = 0,
    shoot_and_sit = 1,
    shoot_and_run = 2,
    no_shoot_and_run = 3,
    shoot_no_run = 4,
    death = 5
}
public class SoldierAi : MonoBehaviour
{
    public float CloseDistance;
    public Transform soldier;
    public NavMeshAgent agent;
    public float courage;
    public float MinDistance;
    //public Rigidbody bullet_rigid;
    //public Transform bullet_transform;
    public Transform area_transform;
    public Animator animator;
    public Transform spawn;



    [SerializeField]
    Emotions emotion;
    [SerializeField]
    Vector3 target;
    [SerializeField]
    bool shoot;
    [SerializeField]
    bool player_close;
    float lucky;
    int courage_level;
    bool in_area;
    [SerializeField]
    int cicles;
    [SerializeField]
    int health
    {
        get; set;
    }

    public static List<Transform> covers;
    public static List<bool> free_covers;
    int cur_cover;
    public static Transform parent;
    AreaTrigger area;
    [SerializeField]
    bool sit;

    [SerializeField]
    float time;
    float shoot_time;
    [SerializeField]
    bool til_no_see;

    [SerializeField]
    bool scared;

    float cover_diagonal;
    Vector3 area_size;

    Renderer soldier_render;
    public Material[] materials;

    public Transform[] ragdoll_part;
    GameObject debug_cube;

    public static bool by_hands;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            health -= 10;
        }
    }




    Transform bul_trans;
    Rigidbody bul_rigid;
    Bullet bullet;
    bool pause;
    void CheckBull()
    {
        //if (bul_trans == null)

        SingleUtils.bullets_pool.Spawn();
        GameObject[] buls = GameObject.FindGameObjectsWithTag("Bullet");
        bul_trans = buls[buls.Length - 1].transform;
        bul_trans.gameObject.SetActive(true);
        //bul_trans = SingleUtils.bullets_pool.Spawn(spawn.position, Quaternion.identity).transform;


        bul_rigid = bul_trans.GetComponent<Rigidbody>();
        bullet = bul_trans.GetComponent<Bullet>();
        bullet.spawn = spawn.position;

    }
    void Shoot()
    {
        if (shoot_time < 0.5f)
        {
            GoTime(ref shoot_time);

        }
        else
        {
            
            animator.SetTrigger("Shoot");
            /*
            CheckBull();
            bul_trans.position = spawn.position;
            bul_rigid.AddForce((SingleUtils.player.position - spawn.position).normalized * 100);
            */
            shoot_time = 0;

        }
    }
    //проверка укрытия от игрока
    void SetTarget(bool in_cover, bool go_back, bool set_our_target)
    {
        if (in_cover)
        {
            sit = true;
            for (int i = 0; i < covers.Count; i++)
            {

                if (!free_covers[i])
                {

                    if (go_back)
                    {
                        Vector3 local_cover = SingleUtils.player.InverseTransformPoint(covers[i].position);
                        if (local_cover.z < 0)
                        {
                            cur_cover = i;
                            Vector3 size = covers[cur_cover].GetComponent<Collider>().bounds.size;
                            cover_diagonal = Mathf.Sqrt(Mathf.Pow(size.x, 2) + Mathf.Pow(size.z, 2));
                            free_covers[i] = true;
                            break;
                        }
                    }
                    else
                    {
                        cur_cover = i;
                        Vector3 size = covers[cur_cover].GetComponent<Collider>().bounds.size;
                        cover_diagonal = Mathf.Sqrt(Mathf.Pow(size.x, 2) + Mathf.Pow(size.z, 2));

                        free_covers[i] = true;
                        break;
                    }
                    ;
                }

            }
            if (cur_cover == -1)
            {
                sit = false;
                in_cover = false;
            }

            if (sit)
                target = covers[cur_cover].position;
        }

        if (!in_cover)
        {
            //пока херня
            /*
            if (go_back)
            {
                Vector3 player_local_area_pos = area_transform.InverseTransformPoint(Utils.player.position);

            }
            else
            {

            }
            */
            Vector3 randomDirection = Vector3.zero;
            bool get_rand = true;
            if (!set_our_target)
            {
                if (get_rand)
                {
                    float x = 30;
                    float z = 30;
                    if (area_size.x < x)
                        x = Random.Range(-area_size.x / 2, area_size.x / 2);
                    else x = Random.Range(-x / 2, x / 2);
                    if (area_size.z < z)
                        z = Random.Range(-area_size.z / 2, area_size.z / 2);
                    else z = Random.Range(-z / 2, z / 2);
                    randomDirection = area_transform.position + new Vector3(x, 0, z);
                }
                else
                {
                    Transform ppp = soldier.parent;
                    float x_min = ppp.GetChild(0).position.x;
                    float x_max = ppp.GetChild(0).position.x;
                    float z_min = ppp.GetChild(0).position.z;
                    float z_max = ppp.GetChild(0).position.z;
                    for (int i = 0; i < ppp.childCount; i++)
                    {
                        Vector3 pos = ppp.GetChild(i).position;
                        if (pos.x > x_max) x_max = pos.x;
                        if (pos.x < x_min) x_min = pos.x;
                        if (pos.z > z_max) z_max = pos.z;
                        if (pos.z < z_min) z_min = pos.z;
                    }
                    float[,] edges = new float[2, 2];
                    edges[0, 0] = Random.Range(area_transform.position.x - area_size.x / 2, x_min);
                    edges[0, 1] = Random.Range(area_transform.position.x + area_size.x / 2, x_max);
                    edges[1, 0] = Random.Range(area_transform.position.z - area_size.z / 2, z_min);
                    edges[1, 1] = Random.Range(area_transform.position.z + area_size.z / 2, z_max);
                    int where_rand = Random.Range(0, 2);
                    if (where_rand == 0)
                    {
                        randomDirection = new Vector3(edges[0, Random.Range(0, 2)], 0, edges[1, Random.Range(0, 2)]);
                    }
                    else
                    {
                        int Rand_Rand_Rand = Random.Range(0, 2);
                        if (Rand_Rand_Rand == 0)
                        {
                            randomDirection = new Vector3(edges[0, Random.Range(0, 2)], 0, Random.Range(area_transform.position.z - area_size.z / 2, area_transform.position.z + area_size.z / 2));
                        }
                        else
                        {
                            randomDirection = new Vector3(Random.Range(area_transform.position.x - area_size.x / 2, area_transform.position.x + area_size.x / 2), 0, edges[1, Random.Range(0, 2)]);
                        }
                    }

                }
            }
            else
            {
                Vector3 loc_dec_pos = area_transform.position - SingleUtils.player.position;



                float k = Mathf.Min(area_size.x / 2 - Mathf.Abs(loc_dec_pos.x), area_size.z / 2 - Mathf.Abs(loc_dec_pos.z));
                if (k > 10) k /= 2;
                randomDirection = SingleUtils.player.position + SingleUtils.player.forward * k;



            }


            randomDirection = new Vector3(randomDirection.x, soldier.position.y, randomDirection.z);


            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, Mathf.Min(area_size.z, area_size.x), 1);
            target = hit.position;
            //debug_cube.transform.position = target;

        }

    }

    int CheckCourageAndLucky()
    {

        if (courage > 80)
        {
            return 1;
        }
        else if (courage <= 80 && courage > 40)
        {
            return 0;
        }
        else
        {
            return -1;
        }

    }


    void CheckFuck()
    {
        player_close = Vector3.Distance(SingleUtils.player.position, soldier.position) < CloseDistance;
    }

    void Hide()
    {
        Vector3 pos = covers[cur_cover].position - (SingleUtils.player.position - covers[cur_cover].position).normalized * (cover_diagonal / 2);

        NavMeshHit hit;

        NavMesh.SamplePosition(pos, out hit, cover_diagonal * 2, 1);
        agent.SetDestination(hit.position);
    }


    private void Awake()
    {
        //GameObject bullet = Instantiate<GameObject>(bullet_transform.gameObject);
        //bullet.transform.localScale /= 10;
        //bullet_transform = bullet.transform;
        //bullet_rigid = bullet.GetComponent<Rigidbody>();
        //bullet_rigid.mass = 0.01f;
        soldier_render = soldier.GetComponent<Renderer>();
        area = area_transform.GetComponent<AreaTrigger>();
        courage = Random.Range(0, 100);
        courage_level = CheckCourageAndLucky();

        cur_cover = -1;
        parent = area_transform;
        area_size = area_transform.GetComponent<BoxCollider>().size;

    }



    private void Start()
    {
        debug_cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(debug_cube.GetComponent<BoxCollider>());
        // CheckBull();

        health = 100;
    }

    void CheckScarePlayer()
    {
        if (!scared)
        {
            if (Vector3.Distance(soldier.position, SingleUtils.player.position) < 4)
            {
                Debug.Log("Scared: " + soldier.gameObject.name);
                soldier_render.material = materials[2];
                ChangeEmotion(Emotions.shoot_and_run, true);
                sit = false;
                scared = true;
                //SetTarget(false, false, true);
                SetTarget(false, true, false);
            }
        }
        
        /*
        if(!by_hands){
        if (Vector3.Distance(soldier.position, SingleUtils.player.position) < 1 && !pause)
        {
            StartCoroutine(CloseDamage());
            Debug.Break();
        }
        }*/
    }
    void ObNUll(bool shooting)
    {
        

        lucky = Random.Range(0, 100);
        if (emotion == Emotions.shoot_and_sit)
        {
            lucky = 100;
            sit = false;
        }
        time = 0;
        if (shooting)
        {
            shoot = false;
            shoot_time = 1;
        }
        else
        {
            shoot = false;
        }



        cicles = 0;
    }



    void ChangeEmotion(Emotions new_em, bool shooting)
    {
        ObNUll(shooting);
        AnimSet(new_em);
        emotion = new_em;
        
    }


    void ChangeEmotionAndSetTarget(Emotions new_em, bool go_back)
    {


        AnimSet(new_em);
        ObNUll(false);
        emotion = new_em;
        if (new_em == Emotions.shoot_and_run)
        {
            //animator.SetTrigger("Run");
            shoot = true;
            shoot_time = 1;
            SetTarget(lucky < 50, go_back, false);
            soldier_render.material = materials[2];

        }
        else if (new_em == Emotions.no_shoot_and_run)
        {

            //animator.SetTrigger("Run");
            SetTarget(lucky < 50, go_back, false);
            soldier_render.material = materials[3];

        }
        else if (new_em == Emotions.shoot_and_sit)
        {
            sit = true;
            //   animator.SetTrigger("Shoot");
            shoot_time = 1;
            soldier_render.material = materials[1];
        }
        else if (new_em == Emotions.shoot_no_run)
        {
            //    animator.SetTrigger("Shoot");
            shoot = true;
            shoot_time = 1;
            sit = false;
            soldier_render.material = materials[4];
            
        }
        else if (new_em == Emotions.waiting)
        {
            //  animator.SetTrigger("Stay");
            sit = true;
            soldier_render.material = materials[0];
        }

    }


    void AnimSet(Emotions new_em)
    {
        if (new_em == Emotions.shoot_and_run)
        {
            animator.SetTrigger("BackRun");
            animator.SetFloat("CrouchBlend", 0);

        }
        else if (new_em == Emotions.no_shoot_and_run)
        {
            animator.SetTrigger("Run");
            animator.SetFloat("CrouchBlend", 0);

        }
        else if (new_em == Emotions.shoot_and_sit)
        {
            animator.SetTrigger("Run");

        }
        else if (new_em == Emotions.shoot_no_run)
        {
            animator.SetTrigger("Stay");

        }
        else if (new_em == Emotions.waiting)
        {
            animator.SetTrigger("Stay");

        }
    }

    bool CanSeePlayer()
    {
        bool res = false;



        // Debug.DrawRay(soldier.position, Utils.player_collider.ClosestPoint(soldier.position) - soldier.position, Color.blue, 20);
        RaycastHit hit;
        Vector3 point = soldier.position + new Vector3(0, 2, 0);
        if (Physics.Raycast(point, SingleUtils.person.capsule.ClosestPoint(point) - soldier.position, out hit))

        {
            if (hit.collider == SingleUtils.person.capsule)
            {
                res = true;
            }
            else
            {
                Debug.Log(soldier.gameObject.name + "   " + hit.collider.gameObject.name);
            }
        }
        return res;
    }

    bool CanCheckPLayer()
    {
        bool res = false;

        Vector3 origin = soldier.position + new Vector3(0, 2 - agent.radius / 2, 0);

        /*
        RaycastHit[] hits = Physics.RaycastAll(origin, (SingleUtils.player.position - origin));
        Debug.DrawRay(origin, (SingleUtils.player.position - origin), Color.red, 20);
        for (int i = 0; i < hits.Length; i++)
        {
            

        }*/

        RaycastHit hit;
        if (Physics.Raycast(origin, (SingleUtils.player.position - origin), out hit))
        {
            if (hit.collider.tag == "Player")
            {
                res = true;
            }
            else
            {
                Debug.Log("down hit:" + hit.collider.gameObject.name);
            }
        }
        if (Physics.Raycast(origin, ((SingleUtils.person.playerCamera.transform.position - new Vector3(0, 0.1f, 0)) - origin), out hit))
        {
            if (hit.collider.tag == "Player")
            {
                res = true;
            }
            else
            {
                Debug.Log("Up hit:" + hit.collider.gameObject.name);
            }
        }
        if (!res)
        {
            Debug.DrawRay(origin, (SingleUtils.player.position - origin), Color.red, 20);
            Debug.DrawRay(origin, ((SingleUtils.person.playerCamera.transform.position - new Vector3(0, 0.1f, 0)) - origin), Color.blue, 20);
        }
        return res;
    }
    void RotateOnPlayer()
    {
        soldier.rotation = Quaternion.Slerp(soldier.rotation, Quaternion.LookRotation(new Vector3(SingleUtils.player.position.x, soldier.position.y, SingleUtils.player.position.z) - soldier.position), Time.deltaTime * 10);

    }
    void Run(bool back)
    {
        agent.SetDestination(target);
        if (til_no_see)
        {
            bool can = CanCheckPLayer();
            if (can)
            {
                ChangeEmotionAndSetTarget(Emotions.shoot_no_run, false);
                til_no_see = false;

            }
            else
            {
                target = soldier.position + (SingleUtils.player.position - soldier.position);
            }
        }
        else
        {
            if (back)
            {
                RotateOnPlayer();

            }
            else
            {

            }
        }
    }


    void CheckWay()
    {
        if (agent.isPathStale)
        {
            ChangeEmotionAndSetTarget(Emotions.shoot_and_run, false);
            Debug.Log("STOLEN");
        }
    }


    bool CheckDistance()
    {
        bool res = false;


        if (sit)
        {
            if (Vector3.Distance(soldier.position, target) < cover_diagonal)
            {

                // Debug.Log("shoot_and_sit");
                ChangeEmotionAndSetTarget(Emotions.shoot_and_sit, false);
                res = true;
            }
        }
        else
        {
            if (Vector3.Distance(soldier.position, target) < MinDistance)
            {
               // if (scared) scared = false;
                ChangeEmotionAndSetTarget(Emotions.shoot_no_run, false);
                res = true;
            }
        }


        return res;
    }
    void GoTime(ref float t)
    {
        if (!pause)
            t += Time.deltaTime;

    }
    void EmotionChange()
    {

        bool near_ = false;
        CheckScarePlayer();
        switch (emotion)
        {
            case Emotions.waiting:
                if (area.in_area)
                {
                    if (player_close)
                    {

                        switch (courage_level)
                        {
                            case 1:

                                ChangeEmotionAndSetTarget(Emotions.shoot_and_run, true);
                                break;

                            case 0:

                                ChangeEmotionAndSetTarget(Emotions.shoot_and_run, false);
                                break;
                            case -1:

                                ChangeEmotionAndSetTarget(Emotions.no_shoot_and_run, false);
                                break;
                        }

                    }
                    else
                    {

                        bool can_see = CanSeePlayer();
                        if (can_see)
                        {
                            ChangeEmotionAndSetTarget(Emotions.shoot_no_run, false);
                        }
                    }
                }


                break;




            case Emotions.shoot_and_sit:
                sit = true;

                if (area.in_area && player_close)
                {
                    if (shoot)
                    {

                        bool no_fuck = CanCheckPLayer();
                        if (!no_fuck)
                        {
                             Debug.Log("OH SHIT<HERE WE GO AGAIN");
                            //Debug.Break();
                            til_no_see = true;
                            
                            ChangeEmotion(Emotions.no_shoot_and_run, false);
                        }

                        RotateOnPlayer();
                        Shoot();
                        //bool timed = Timer(2);

                        GoTime(ref time);
                        if (time >= 2)
                        {
                            animator.SetTrigger("Run");
                            animator.SetFloat("CrouchBlend",1);
                            shoot = false;

                            time = 0;
                            Debug.Log("Cicled shoot_and_sit");
                            cicles++;
                            if (cicles >= 6 + courage_level * 2)
                            {
                                sit = false;
                                cicles = 0;
                                if (courage_level == -1)
                                {
                                    free_covers[cur_cover] = false;
                                    ChangeEmotionAndSetTarget(Emotions.no_shoot_and_run, false);
                                    cur_cover = -1;
                                }
                                else
                                {
                                    free_covers[cur_cover] = false;
                                    ChangeEmotionAndSetTarget(Emotions.shoot_and_run, true);
                                    cur_cover = -1;

                                }
                                time = 0;
                                shoot = false;
                            }
                            else
                            {

                                //animation_sit

                            }
                        }

                    }
                    else
                    {
                        Hide();
                        // bool timed = Timer(4);
                        GoTime(ref time);
                        if (time >= 4)
                        {
                            animator.SetTrigger("Stay");
                            animator.SetFloat("CrouchBlend",0);
                            shoot = true;
                            shoot_time = 1;
                            time = 0;
                            cicles++;
                            Debug.Log("Cicled shoot_and_sit");
                            if (cicles >= 6 + courage_level * 2)
                            {
                                sit = false;
                                cicles = 0;
                                if (courage_level == 0 || courage_level == -1)
                                {
                                    free_covers[cur_cover] = false;
                                    ChangeEmotionAndSetTarget(Emotions.no_shoot_and_run, true);
                                    cur_cover = -1;
                                }
                                else
                                {
                                    free_covers[cur_cover] = false;
                                    ChangeEmotionAndSetTarget(Emotions.shoot_and_run, false);
                                    cur_cover = -1;

                                }
                                time = 0;
                                shoot = false;
                            }
                        }
                    }

                }
                else
                {
                    ChangeEmotionAndSetTarget(Emotions.waiting, false);
                }

                break;






            case Emotions.shoot_and_run:
                CheckWay();
                Run(true);
                //CheckScarePlayer();
                if (shoot)
                {
                    Shoot();
                    //bool timed = Timer(2);
                    GoTime(ref time);

                    if (time >= 2)
                    {
                        time = 0;
                        near_ = CheckDistance();
                        if (!near_)
                        {
                            shoot = false;
                        }
                    }
                }
                else
                {
                    //                    bool timed = Timer(2);

                    GoTime(ref time);

                    if (time >= 2)
                    {
                        time = 0;

                        near_ = CheckDistance();
                        if (!near_)
                        {
                            shoot = true;
                            shoot_time = 1;
                        }
                    }
                }

                break;




            case Emotions.no_shoot_and_run:
                CheckWay();
                Run(false);
                near_ = CheckDistance();
                //CheckScarePlayer();
                break;





            case Emotions.shoot_no_run:
                RotateOnPlayer();
                if (shoot)
                {
                    Shoot();
                    GoTime(ref time);

                    if (time >= 1)
                    {
                        cicles++;

                        time = 0;
                        shoot = false;
                        if (cicles >= 6 + courage_level * 2)
                        {
                            if(scared) scared = false; 
                            cicles = 0;
                            if (courage_level == -1)
                            {
                                ChangeEmotionAndSetTarget(Emotions.no_shoot_and_run, false);
                            }
                            else
                            {
                                ChangeEmotionAndSetTarget(Emotions.shoot_and_run, true);

                            }
                            time = 0;
                            shoot = false;
                        }
                    }
                }
                else
                {
                    //bool timed = Timer(1);
                    GoTime(ref time);
                    if (time >= 1)
                    {
                        cicles++;
                        //Debug.Log("Cicled shoot_no_run");
                        shoot = true;
                        shoot_time = 1;
                        time = 0;
                        if (cicles >= 6 + courage_level * 2)
                        {
                            if(scared) scared = false;
                            cicles = 0;
                            if (courage_level == 0 || courage_level == -1)
                            {
                                ChangeEmotionAndSetTarget(Emotions.no_shoot_and_run, true);
                            }
                            else
                            {
                                ChangeEmotionAndSetTarget(Emotions.shoot_and_run, false);

                            }
                            time = 0;
                            shoot = false;
                        }

                    }
                }


                break;



            case Emotions.death:
                if (cur_cover != -1)
                {

                    free_covers[cur_cover] = false;
                    cur_cover = -1;
                }
                Destroy(soldier.GetChild(0).GetComponent<Animator>());
                Destroy(soldier.GetComponent<NavMeshAgent>());
                Destroy(soldier.GetComponent<BoxCollider>());
                for (int i = 0; i < ragdoll_part.Length; i++)
                {
                    ragdoll_part[i].gameObject.AddComponent<Rigidbody>();
                    ragdoll_part[i].gameObject.AddComponent<BoxCollider>();
                }

                Destroy(soldier.GetComponent<SoldierAi>());
                break;



        }
    }
    IEnumerator CloseDamage()
    {
        pause = true;
        animator.SetTrigger("Hand");
        by_hands = true;
        float max_t = 1;
        float t = 0;
        while (t < max_t)
        {
            yield return null;
            t += Time.deltaTime;

        }
        pause = false;
        by_hands = false;

    }
    public void MakePain(int h_p)
    {

        health -= h_p;
    }
    public bool debuging;
    public bool debug_in_play;
    private void Update()
    {




        if (health <= 0)
        {
            emotion = Emotions.death;
        }
        if (debuging)
        {
            emotion = Emotions.shoot_no_run;
            bool check = CanCheckPLayer();
            Debug.Log("can check payer: " + check);
        }
        else
        {
            if (debug_in_play)
            {
                if (Input.GetKeyDown(KeyCode.Q)) Debug.Break();
            }
            EmotionChange();
        }
        CheckFuck();

    }

}
