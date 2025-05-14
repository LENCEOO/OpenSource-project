using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private int scorePoint = 100;

    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private GameObject[] itemPrefabs; // 적을 죽였을 때 획득 가능한 아이템
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHP>().TakeDamage(damage);

            OnDie();
        }
    }
    public void OnDie()
    {
        playerController.Score += scorePoint;

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // 일정 확률로 아이템 생성
        SpawnItem();

        Destroy(gameObject);
    }

    private void SpawnItem()
    {
        // 파워업 (10%), 폭탄+1(5%), 체력회복(15%)
        int spawnItem = Random.Range(0, 100);
        if ( spawnItem < 10 )
        {
            Instantiate(itemPrefabs[0], transform.position, Quaternion.identity);
        }
        else if (spawnItem < 15)
        {
            Instantiate(itemPrefabs[1], transform.position, Quaternion.identity);
        }
        else if (spawnItem < 30)
        {
            Instantiate(itemPrefabs[2], transform.position, Quaternion.identity);
        }

    }
}
