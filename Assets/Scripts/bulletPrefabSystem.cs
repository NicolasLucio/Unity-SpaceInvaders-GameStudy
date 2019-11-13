using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletPrefabSystem : MonoBehaviour
{
    public int whatBullet;
    public GameObject bulletObject;
    private Rigidbody2D bulletBody;
    
    void Start()
    {        
        bulletBody = bulletObject.GetComponent<Rigidbody2D>();
        if (whatBullet == 1)
        {
            bulletBody.velocity = new Vector2(0.0f, 5.0f);
        }
        else if (whatBullet == 2)
        {
            bulletBody.velocity = new Vector2(0.0f, -8.0f);
        }        
    }
    
    void Update()
    {
        Vector2 tempBulletPosition = bulletObject.transform.position;
        if (tempBulletPosition.y <= -5 || tempBulletPosition.y >= 7)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }    
}
