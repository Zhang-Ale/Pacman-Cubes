using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class EnemyMovement : Observable
{
    GameObject player;
    NavMeshAgent enemy;
    public GameObject part, PUparticle;
    EnemyHealth EH;
    EnemyShoot ES;
    public GameObject pistol; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        poweredUp = false;
        shotgunPoweredUp = false; 
        enemy = gameObject.GetComponent<NavMeshAgent>();
        menu = GameObject.FindGameObjectWithTag("Menu").GetComponent<Menu>();
        pus = GameObject.FindGameObjectWithTag("PUS").GetComponent<PUSpawner>();
        EH = GetComponent<EnemyHealth>();
        ES = GetComponent<EnemyShoot>(); 
    }

    void Update()
    {
        if (menu.gameStarted && !EH.notified)
        {
            enemy.speed = 30; 
            enemy.SetDestination(player.transform.position);
        }
        else
        {
            enemy.speed = 0;
            enemy.SetDestination(this.gameObject.transform.position);
        }

        
        if(GetComponent<Rigidbody>().velocity.magnitude != 0)
        {
            InstantiateParticle(part);
        }

        if (poweredUp)
        {
            StartCoroutine(PowerUpTime());
        }
        else
        {
            StopCoroutine(PowerUpTime());
        }

        if (shotgunPoweredUp)
        {
            StartCoroutine(ShotgunPowerUpTime());
        }
        else
        {
            StopCoroutine(ShotgunPowerUpTime()); 
        }
    }

    IEnumerator PowerUpTime()
    {
        InstantiateParticle(PUparticle);
        yield return new WaitForSeconds(powerUpDuration);
        poweredUp = false;
    }

    IEnumerator ShotgunPowerUpTime()
    {
        pistol.SetActive(true);
        ES.enabled = true;
        yield return new WaitForSeconds(powerUpDuration);
        shotgunPoweredUp = false;
        pistol.SetActive(false);
        ES.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PowerUp")
        {
            poweredUp = true;
            Destroy(other.gameObject);
            pus.Invoke("PUSpawn", 5); 
        }

        if (other.tag == "ShotgunPowerUp")
        {
            shotgunPoweredUp = true;
            Destroy(other.gameObject);
            pus.Invoke("SPUSpawn", 5);
        }
    }
}
