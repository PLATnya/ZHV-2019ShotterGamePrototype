using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using Scripts.Utils;
public class Bullet : MonoBehaviour,IPoolable
{
    public Rigidbody rigid;
    public LineRenderer tail;
    public Transform trans;
    public float bullet_speed;
    public Vector3 spawn;
    public void OnSpawn(){

    }
    void Update(){
        tail.SetPosition(0, trans.position);
        tail.SetPosition(1, trans.position + (spawn - trans.position).normalized * rigid.velocity.magnitude / bullet_speed * 25);

    }
    public void OnDespawn(){
        rigid.velocity = Vector3.zero;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player"){
            Health.BulletHit();
        }else if(collision.gameObject.tag=="Soldier"){
            SoldierAi sold = null;
            collision.gameObject.TryGetComponent<SoldierAi>(out sold);
            if(sold!=null){
                sold.MakePain(20);
            }
        }
        SingleUtils.bullet_debug_pool.Spawn(collision.GetContact(0).point,Quaternion.identity);
        SingleUtils.bullets_pool.Despawn(this.gameObject);   
        //if (collision.gameObject.tag == "Finish") Debug.Log("BUllet on Cube");
        /*
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.localScale /= 10;
        obj.transform.position = collision.GetContact(0).point;
        obj.GetComponent<Renderer>().material = def_material;
        Destroy(obj.GetComponent<BoxCollider>());
        */
        //pool.Spawn(collision.GetContact(0).point,Quaternion.identity);
    }


    //тип сама пуля литит от середины а ее хвост направляется на пушку и тип обманочка
}
