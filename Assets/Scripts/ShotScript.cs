using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotScript : MonoBehaviour
{
    public bool playerShot;
    public float shotSpeed;
    public GameObject pShot;
    public GameObject eShot;

    public float lifeTime;
    private float currLifeTime;

    
    // Start is called before the first frame update
    void Start()
    {
        currLifeTime = lifeTime;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 move = transform.up * Time.deltaTime * shotSpeed;
        transform.position += move;
        currLifeTime -= Time.deltaTime;
        if (currLifeTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void setUpShot(bool isPlayer)
    {
        playerShot = isPlayer;
        pShot.SetActive(isPlayer);
        eShot.SetActive(!isPlayer);
        currLifeTime = lifeTime;
        gameObject.tag = (isPlayer) ? "Shot" : "EnemyShot";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && playerShot)
        {
            return;
        }
        gameObject.SetActive(false);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.collider.tag != "Player" || !playerShot)
    //    {
    //        this.gameObject.SetActive(false);
    //    }
    //}
}
