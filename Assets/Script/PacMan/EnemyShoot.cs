using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : Observable
{
    public float _fireRate = 0; 
    public float forceMultiplicator = 10;
    public Transform shootPosition;
    public GameObject _bulletPrefab;
    private GameObject player;
    public GameObject pistol;
    public Animator pistolAnim;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Vector3 direction = player.transform.position;
        pistol.transform.rotation = Quaternion.LookRotation(direction);
        if (_fireRate < Time.time)
        {
            ShootBullet(direction);
            Notify(this.gameObject, Action.OnPlayerShoot);
            _fireRate = Time.time + 0.5f;
        }
    }

    void ShootBullet(Vector3 direction)
    {
        pistolAnim.SetTrigger("Shoot");
        for (int i = 0; i < 1; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab, shootPosition.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = direction * forceMultiplicator;
        }
    }

    private void Awake()
    {
        IObserver gm = GameObject.FindObjectOfType<PlayerActions>();
        AddObserver(gm);
    }

    private void OnDisable()
    {
        IObserver gm = GameObject.FindObjectOfType<PlayerActions>();
        RemoveObserver(gm);
    }
}
