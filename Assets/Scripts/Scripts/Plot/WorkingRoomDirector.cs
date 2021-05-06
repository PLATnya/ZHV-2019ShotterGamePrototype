using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
public class f{

}
public class WorkingRoomDirector : MonoBehaviour
{
    int state;


    public bool DebugMode = true;
    public GameObject[] triggers_zero;
    public static bool zero_ended;
    public static bool one_ended;
    public GameObject[] triggers_one;


    public GameObject[] help_objects;




    void Awake()
    {
        floorControl = GetComponent<FloorControl>();
        ChangeState(1);
        
    }

    VentilationTrigger angleTrigger;
    VentilationTrigger turningTrigger;


    public static FloorControl floorControl;

    public void ChangeState(int st)
    {
        state = st;

        switch (st)
        {
            case 0:
                if (DebugMode)
                {
                    floorControl.third_floor.SetActive(false);
                    floorControl.second_floor.SetActive(true);
                    floorControl.first_floor.SetActive(true);
                    floorControl.firstController.parts[0].gameObject.SetActive(true);
                    floorControl.firstController.parts[1].gameObject.SetActive(true);
                    for (int i = 0; i < triggers_one.Length; i++)
                    {
                        triggers_one[i].SetActive(false);
                        triggers_one[i].GetComponent<Collider>().enabled = false;
                    }
                    for(int i = 0;i<help_objects.Length;i++){
                        help_objects[i].SetActive(false);
                    }
                    triggers_zero[0].GetComponent<VentilationTrigger>().SetActivate(false);
                    triggers_zero[1].GetComponent<VentilationTrigger>().SetActivate(false);
                }
                for (int i = 0; i < triggers_zero.Length; i++)
                    if (DebugMode)
                    {
                        triggers_zero[i].GetComponent<Collider>().enabled = true;
                    }
                    else
                        triggers_zero[i].SetActive(true);
                angleTrigger = triggers_zero[0].GetComponent<VentilationTrigger>();
                turningTrigger = triggers_zero[1].GetComponent<VentilationTrigger>();
                break;
            case 1:
                //ДВЕРЬ
                if (DebugMode)
                {
                    triggers_zero[0].GetComponent<VentilationTrigger>().SetActivate(true);
                    triggers_zero[1].GetComponent<VentilationTrigger>().SetActivate(true);
                    floorControl.first_floor.SetActive(true);
                    floorControl.firstController.parts[0].gameObject.SetActive(true);
                    floorControl.third_floor.SetActive(false);
                    for(int i = 0;i<help_objects.Length;i++){
                        help_objects[i].SetActive(true);
                    }
                }
                floorControl.firstController.parts[1].gameObject.SetActive(false);
                floorControl.second_floor.SetActive(false);
                for (int i = 0; i < triggers_zero.Length; i++)
                {
                    if (DebugMode)
                        triggers_zero[i].GetComponent<Collider>().enabled = false;
                    else
                        Destroy(triggers_zero[i]);
                }
                for (int i = 0; i < triggers_one.Length; i++)
                {
                    triggers_one[i].SetActive(true);
                    triggers_one[i].GetComponent<Collider>().enabled = true;
                }
                break;
        }
    }
    void OnFaceToFloorAfterSlopeDown()
    {
        floorControl.first_floor.transform.position += Vector3.up * 20;
        SingleUtils.player.position += Vector3.up * 20;
        triggers_one[0].transform.parent.parent.gameObject.SetActive(false);
        help_objects[0].SetActive(true);
    }
    public void FuckingSlopeDown()
    {
        Transform floor = triggers_one[0].transform.parent;
        SingleUtils.no_control.onFaceToFloor.AddListener(OnFaceToFloorAfterSlopeDown);
        StartCoroutine(Resetransform(floor, floor.position, floor.rotation.eulerAngles + new Vector3(-30, 0, 0), 2));
        StartCoroutine(Resetransform(floorControl.first_floor.transform, floorControl.first_floor.transform.position + Vector3.down * 20, floorControl.first_floor.transform.rotation.eulerAngles, 2));
    }
    IEnumerator Resetransform(Transform trans, Vector3 pos, Vector3 rot, float time)
    {
        float wl = 0;
        Vector3 start_pos = trans.position;
        Quaternion start_rot = trans.rotation;
        while (wl < time)
        {
            wl += Time.deltaTime;
            trans.position = Vector3.Slerp(start_pos, pos, wl / time);
            trans.rotation = Quaternion.Slerp(start_rot, Quaternion.Euler(rot), wl / time);
            yield return null;
        }
    }
    public void FallThroughTheLift()
    {
        //triggers[3]
        StartCoroutine(SingleUtils.post.DarkInEyes(0.5f, 1, 1));
        floorControl.second_floor.SetActive(false);
        floorControl.third_floor.SetActive(true);


        help_objects[0].SetActive(false);
        SingleUtils.player.position = help_objects[1].transform.position;
        //кароч падаем в темную комнату а третьем этаже, дверь закрыта, сначало надо включить свет, а при включении света внезапно появляются пару 
        //зомбаков и тип внезапно и тип страшно, находим ключ и идем дальше
    }

    public void GoUpLift()
    {
        //triggers[2]
        triggers_one[1].SetActive(false);
        FloorAngleTrigger repeat = triggers_one[1].GetComponent<FloorAngleTrigger>();
        repeat.fukBak = false;
        repeat.SetFucked(true);
        triggers_one[3].SetActive(true);
        triggers_one[3].GetComponent<FloorAngleTrigger>().SetFucked(false);
    }
    public void FallInTheLiftDark()
    {
        //triggers[1]
        StartCoroutine(SingleUtils.post.DarkInEyes(0.2f, 0.5f, 1));
        float y = triggers_one[2].transform.localScale.y / 2 + 1.5f;
        SingleUtils.player.position = triggers_one[2].transform.position - new Vector3(0, y, 0);
        SingleUtils.person.fps_Rigidbody.velocity/=2;
    }

    void Update()
    {
        switch (state)
        {
            case 0:
                bool onTurningGround = floorControl.firstController.floor[0] ==
                SingleUtils.person.ground;
                angleTrigger.Can = onTurningGround;
                turningTrigger.Can = onTurningGround;

                if (onTurningGround && zero_ended)
                {
                    ChangeState(1);
                }
                break;
            case 1:
                break;
        }
    }
}
