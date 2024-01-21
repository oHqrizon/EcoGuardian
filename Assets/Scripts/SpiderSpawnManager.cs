using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSpawnManager : MonoBehaviour
{
    public GameObject spiderPrefab;
    public float spawnRate = 1f;
    public float minScale = 0.5f;
    public float maxScale = 1.5f;
    public float count = 0f;
    public float speed = 4f;
    private bool started = false;
    
    private SpiderAttack spider;
    private Transform spiderP;
    private Transform player;
    private SpiderHP spiderHP;
    private PlayerHealth playerHealth;

    void Start()
    {
        spider = GameObject.FindGameObjectWithTag("Spider").GetComponent<SpiderAttack>();
        spiderP = GameObject.FindGameObjectWithTag("Spider").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spiderHP = GetComponent<SpiderHP>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    public void SpawnSpider()
    {
        GameObject newSpider = Instantiate(spiderPrefab, spiderP.position, Quaternion.identity);
        newSpider.tag = "MiniSpider";
        float randomScale = Random.Range(minScale, maxScale);
        newSpider.transform.localScale = new Vector3(randomScale, randomScale, 1f);
        count ++;
    }

    void StopSpawning() {
        CancelInvoke("SpawnSpider");
    }

    void Update() {
        if (spider.jumped == true && !started) {
            InvokeRepeating("SpawnSpider", 2f, spawnRate);
            started = true;
        }

        if (count >= 3) {
            StopSpawning();
        }

        GameObject[] miniSpiders = GameObject.FindGameObjectsWithTag("MiniSpider");
        foreach (GameObject miniSpider in miniSpiders)
        {
            if (miniSpider != null)
            {
                //miniSpider.transform.LookAt(player);
                miniSpider.transform.position = Vector3.MoveTowards(miniSpider.transform.position, 
                player.position, Time.deltaTime * speed);
            }
        }

        if (spiderHP.dead == true || playerHealth.getHealth() == 0f) {
            foreach (GameObject miniSpider in miniSpiders)
            {
                if (miniSpider != null)
                {
                    Destroy(miniSpider);
                    count = 0;
                    started = false;
                }
            }
        }
    }

}
