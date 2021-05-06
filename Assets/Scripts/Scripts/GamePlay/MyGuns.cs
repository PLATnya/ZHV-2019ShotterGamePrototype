using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utils;
namespace Player.Inventory
{
    public class MyGuns
    {

        public static Transform guns_transform = GameObject.FindGameObjectWithTag("GunsCase").transform;
        public static Gun gun;

        //public static Transform bullet_transform = ((Bullet)Object.FindObjectOfType(typeof(Bullet))).transform;
        //public static Rigidbody bullet_rigid = bullet_transform.GetComponent<Rigidbody>();
        //public static List<Ammo> ammos = new List<Ammo>(2);
        public static Ammo[] ammos = new Ammo[2];


        public static int choosed;
        static List<Gun> guns = new List<Gun>(3);
        public static int GetGunsCount() { return guns.Count; }
        public static void CheckGun(GameObject gun_object, Transform gun_parent, Vector3 start_pos)
        {
            GunStruct gun_ = gun_object.GetComponent<GunStruct>();
            
            gun_object.transform.parent = gun_parent;
            gun_object.transform.localPosition = start_pos;
            gun_object.transform.localRotation = Quaternion.Euler(Vector3.zero);
            guns.Add(new Gun(gun_.bullet_max, gun_object, gun_object.GetComponent<Animator>(), gun_.non_stop, gun_.delta, gun_.kick, gun_.offset));
            SetLastBullets(gun_.ammo_count, MyGuns.ammos[gun_.ammo_number]);
            guns[guns.Count-1].name = gun_.name;
            if (guns.Count > 1)
            {
                ReGun(guns.Count - 1);
            }
            else
            {
                choosed = guns.Count - 1;
            }
            gun = guns[choosed];
            

        }
        public static void RemoveGun(int index)
        {
            guns.RemoveAt(index);
        }
        public static void ReGun(int new_choose)
        {
            if (new_choose != choosed)
            {
                guns_transform.GetChild(choosed).gameObject.SetActive(false);
                ReNewGun(new_choose);

            }

        }
        public static void ReNewGun(int new_choose)
        {
            choosed = new_choose;
            gun = GetGun(choosed);
            guns_transform.GetChild(choosed).gameObject.SetActive(true);
            gun = guns[choosed];
            gun.gun_object.transform.localPosition = SingleUtils.gun_slot.GetHidePos();
            SingleUtils.gun_slot.ChangeGunState(GunState.two_hand);
            
        }
        public static Gun GetGun(int index) { return guns[index]; }
        public static void AddGun(Gun gun)
        {
            guns.Add(gun);
        }
        public static void SetLastBullets(int now, Ammo ammo)
        {
            guns[guns.Count - 1].SetBullets(now, ammo);
        }
    }
}
