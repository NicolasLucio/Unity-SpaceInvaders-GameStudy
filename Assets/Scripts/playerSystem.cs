using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerSystem : MonoBehaviour
{
    public int whatShip;
    public GameObject shipObject;
    public GameObject enemyBulletObject;
    private Rigidbody2D shipBody;
    private bool canShoot;
    private int shootSituation;

    private Vector2 playerPosition;
    public GameObject bulletObject;
    private GameObject[] sceneEnemys;

    private GameObject victoryText;
    private Text victoryTextText;

    private bool changedPosition;
    void Start()
    {
        if (whatShip == 1)
        {
            shipBody = shipObject.GetComponent<Rigidbody2D>();
        }
        else
        {
            //Change position   
            StartCoroutine(ChangeEnemyPosition());
        }
        shootSituation = 0;
        victoryText = GameObject.Find("VictoryText");
        victoryTextText = victoryText.GetComponent<Text>();
        victoryTextText.text = "";
        changedPosition = false;
    }

    void Update()
    {
        if (canShoot == false && shootSituation == 0)
        {
            shootSituation++;
            StartCoroutine(ResetShoot());
        }
    }
    
    void FixedUpdate()
    {
        if (whatShip == 1)
        {
            playerPosition = this.transform.position;
            if (Input.GetKey(KeyCode.D) && playerPosition.x <= 8.5f)
            {
                shipBody.velocity = new Vector2(5.0f, 0.0f);
            }
            else if (Input.GetKey(KeyCode.A) && playerPosition.x >= -8.5f)
            {
                shipBody.velocity = new Vector2(-5.0f, 0.0f);
            }
            else
            {
                shipBody.velocity = Vector2.zero;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShipFire();
            }
        }
        else if (whatShip == 2 || whatShip == 3)
        {
            if (canShoot == true)
            {
                int shootPosibility = Random.Range(0, 3);
                if (shootPosibility == 2)
                {
                    ShipFire();
                }
                else
                {
                    canShoot = false;
                    shootSituation = 0;
                }
            }            
        }
    }

    void LateUpdate()
    {
        if (whatShip == 1)
        {
            CheckEnemys();
        }        
    }

    void ShipFire()
    {
        if (canShoot == true)
        {
            Vector2 tempShipPosition = this.transform.position;            
            if (whatShip == 1)
            {
                GameObject tempBullet = Instantiate(bulletObject, new Vector2(tempShipPosition.x, tempShipPosition.y + 1), Quaternion.identity);
                tempBullet.tag = "Bullet";
            }
            else if (whatShip == 2)
            {
                GameObject tempBullet = Instantiate(enemyBulletObject, new Vector2(tempShipPosition.x, tempShipPosition.y - 1), Quaternion.identity);
                tempBullet.tag = "EnemyBullet";
            }            
            canShoot = false;
            shootSituation = 0;
        }
    }

    void CheckEnemys()
    {
        sceneEnemys = GameObject.FindGameObjectsWithTag("Enemy");        
        if (sceneEnemys.Length == 0)
        {
            if (whatShip == 1)
            {                
                StartCoroutine(VictoryTimer());
            }            
        }        
    }

    IEnumerator ResetShoot()
    {
        yield return new WaitForSeconds(1.0f);
        canShoot = true;
    }

    IEnumerator VictoryTimer()
    {
        StopCoroutine(ChangeEnemyPosition());
        victoryTextText.text = "Victory!";
        Debug.Log("Vitória");
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("Game");
    }

    IEnumerator ChangeEnemyPosition()
    {
        Debug.Log("Change enemy corrotine");
        while (true)
        {
            if (whatShip == 2)
            {
                Vector2 tempPosition = this.transform.position;
                GameObject thisGameObject = this.gameObject;
                if (changedPosition == false)
                {
                    thisGameObject.transform.DOMoveX(tempPosition.x + 1.5f, 2.0f, false);                    
                }
                else if (changedPosition == true)
                {                    
                    thisGameObject.transform.DOMoveX(tempPosition.x - 1.5f, 2.0f, false);                    
                }
            }
            else if (whatShip == 3)
            {
                Vector2 tempPosition = this.transform.position;
                GameObject thisGameObject = this.gameObject;
                if (changedPosition == false)
                {
                    thisGameObject.transform.DOMoveX(tempPosition.x - 1.5f, 2.0f, false);                    
                }
                else if (changedPosition == true)
                {
                    thisGameObject.transform.DOMoveX(tempPosition.x + 1.5f, 2.0f, false);                    
                }
            }

            yield return new WaitForSeconds(2.0f);

            if (changedPosition == true)
            {
                changedPosition = false;
            }
            else if (changedPosition == false)
            {
                changedPosition = true;
            }            
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (whatShip == 1)
        {
            if (collision.gameObject.tag == "EnemyBullet")
            {
                //Perde o jogo
                Debug.Log("Perdeu");
                SceneManager.LoadScene("Game");
            }
        }
        else if (whatShip == 2 || whatShip == 3)
        {
            if (collision.gameObject.tag == "Bullet")
            {
                this.gameObject.SetActive(false);
                Destroy(this.gameObject);                
            }
        }        
    }
}
