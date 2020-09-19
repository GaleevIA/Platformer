using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 0.4f;
    public int playerDamage = 20;
    public int enemyDamage = 1;
    public int direction;
    public AudioClip destroyClip;

    void Start()
    {

    }

    private void EnvironmentCollision(Collision2D collision)
    {
        Rigidbody2D RB = gameObject.GetComponent<Rigidbody2D>();
        RB.AddForce(transform.position * -3, ForceMode2D.Impulse);
    }

    private void EnemyCollision(Collision2D collision)
    {
        GameObject temp = collision.gameObject;
        temp.GetComponent<Enemy>().health -= playerDamage;
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(destroyClip, transform.position);
    }

    private void PlayerCollision(Collision2D collision)
    {
        GameObject temp = collision.gameObject;
        temp.GetComponent<Character>().health -= enemyDamage;
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(destroyClip, transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Environment")
        {
            EnvironmentCollision(collision);
        }
        else if (collision.gameObject.tag == "Enemy"
            && collision.gameObject.tag != name)
        {
            EnemyCollision(collision);
        }
        else if(collision.gameObject.tag == "Player"
            && collision.gameObject.tag != name)
        {
            PlayerCollision(collision);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        transform.position += Vector3.right * direction * speed;
    }
}
