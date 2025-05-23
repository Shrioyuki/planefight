using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public GameObject bulletPrefab;
    public float spawnRate = 1;

    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1 / spawnRate)
        {
            timer -= 1 / spawnRate;
            SpawnBullet();
        }
    }

    void SpawnBullet() 
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}
