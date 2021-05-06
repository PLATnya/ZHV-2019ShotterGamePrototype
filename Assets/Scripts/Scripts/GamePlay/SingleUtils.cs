using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Utils
{

    public class SingleUtils
    {


        public static PostManipulation post = (PostManipulation)Object.FindObjectOfType(typeof(PostManipulation));
        public static GetObjects obj_getter = (GetObjects)Object.FindObjectOfType(typeof(GetObjects));
        public static CameraShake shaker = (CameraShake)Object.FindObjectOfType(typeof(CameraShake));
        public static Lean.Pool.LeanGameObjectPool bullet_debug_pool = GameObject.FindGameObjectWithTag("DebugBulletsPool").GetComponent<Lean.Pool.LeanGameObjectPool>();
        public static GrabTrigger grab_trigger = (GrabTrigger)Object.FindObjectOfType(typeof(GrabTrigger));
        public static Lean.Pool.LeanGameObjectPool bullets_pool = GameObject.FindGameObjectWithTag("BulletsPool").GetComponent<Lean.Pool.LeanGameObjectPool>();
        public static GunSlot gun_slot = (GunSlot)Object.FindObjectOfType(typeof(GunSlot));
        public static GunState gun_state;
        public static FirstPersonAIO person = (FirstPersonAIO)Object.FindObjectOfType(typeof(FirstPersonAIO));
        public static Transform player = person ? person.transform : null;
        public static Transform canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        //public static Transform canvas = GameObject.Find("GamePlay").transform.Find("Canvas").transform;
        public static Transform border;
        public static bool on_trigger_border;
        public static bool can_over;
        public static bool can_on;
        public static float ledge;
        public static Transform camera = Camera.main ? Camera.main.transform : null;
        public static NoControl no_control = player ? player.GetComponent<NoControl>() : null;
        static float deltaTime;




        public static IEnumerator FovMaster(float fow, float delta)
        {
            float t = 0;
            float start_fow = person.playerCamera.fieldOfView;
            float half_time = delta / 2;
            while (t < delta)
            {

                yield return null;
                t += Time.deltaTime;
                if (t <= half_time)
                {
                    person.playerCamera.fieldOfView += Time.deltaTime * ((fow - start_fow) / half_time);
                }
                else
                {
                    person.playerCamera.fieldOfView -= Time.deltaTime * ((fow - start_fow) / half_time);
                }
                if (fow >= start_fow)
                    person.playerCamera.fieldOfView = Mathf.Clamp(person.playerCamera.fieldOfView, start_fow, fow);
                else person.playerCamera.fieldOfView = Mathf.Clamp(person.playerCamera.fieldOfView, fow, start_fow);
            }
        }
        public static void FPSCounter()
        {

            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = Color.white;
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
        }
        public static bool CanPreLoad(Transform obj)
        {
            bool res = false;
            Transform camera_trans = Camera.main.transform;
            if (Vector3.Distance(SingleUtils.player.position, obj.position) >= 5 && Mathf.Abs(Vector3.Angle(camera_trans.forward, obj.position - camera_trans.position)) >= 90)
                res = true;
            return res;
        }
        public static bool PreLoad(PlotTrigger trigger, Transform check_obj, Transform new_obj, Vector3 new_position)
        {
            bool res = false;
            if (trigger.PlayerInTrigger())
            {
                if (CanPreLoad(check_obj))
                {
                    new_obj.position = new_position;
                    res = true;
                }
            }
            return res;
        }
    }

    public interface PlotTrigger
    {
        bool PlayerInTrigger();
    }


    public class Health
    {

        public static float health = 100;
        public static void BulletHit()
        {

            MakePain(20);
            SingleUtils.shaker.StartCoroutine(SingleUtils.shaker.FuckSHakeIt(0.2f, 0.25f, 0.02f));
            SingleUtils.shaker.StartCoroutine(SingleUtils.FovMaster(SingleUtils.person.playerCamera.fieldOfView - 5, 0.15f));
            //SingleUtils.post.StartCoroutine(SingleUtils.post.DarkInEyes(0.05f,0.1f,0.5f));
        }

        public static void EarthQuake()
        {
            SingleUtils.shaker.StartCoroutine(SingleUtils.shaker.FuckSHakeIt(1f, 0.5f, 0.02f));
            SingleUtils.shaker.StartCoroutine(SingleUtils.FovMaster(SingleUtils.person.playerCamera.fieldOfView - 10, 0.5f));
            //SingleUtils.post.StartCoroutine(SingleUtils.post.DarkInEyes(0.02f,0.8f,0.2f));
        }
        public static void MakePain(int h_p)
        {
            // health -= h_p;
            health = Mathf.Clamp(health, 0, 100);
            GetHealthStats();
            CheckSpeed();
        }
        public static void GetHealthStats()
        {
            DebugGUI.health_text.text = "health: " + health;
            //DebugGUI.debug_text.text+="\nhealth: "+health;
        }
        public static void CheckSpeed()
        {
            SingleUtils.person.walkSpeed = 1 + 2 * (health / 100.0f);
            SingleUtils.person.sprintSpeed = 3 + 6 * (health / 100.0f);
        }
        public static IEnumerator Healing(float delta, float mult)
        {
            float t = 0;
            while (t < delta)
            {
                yield return null;
                Debug.Log("Hralth: " + health);
                CheckSpeed();
                health += Time.deltaTime * mult;
                health = Mathf.Clamp(health, 0, 100);
                t += Time.deltaTime;
                GetHealthStats();
            }
            //Debug.Log("health" + health);
        }




    }
    public class DebugGUI : MonoBehaviour
    {
        public static Text health_text = SingleUtils.canvas.Find("Health").GetComponent<Text>();
        public static Text debug_text = SingleUtils.canvas.Find("TextDebug").GetComponent<Text>();
        


    }
}