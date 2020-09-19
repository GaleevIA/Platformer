using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;  
    public float speed;
    public Transform player;
    public LayerMask mask;
    public GameObject bullPref;
    public AudioClip shootClip;

    bool condPatrol;
    bool condActive;
    bool condShoot;
    float X;
    float Y;
    int rayDistance;
    bool right;
    Vector3 direction;
    Rigidbody2D rigidBody;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        speed = 3;
        condPatrol = true;
        rayDistance = 1;
        X = -1;
        Y = -1;
        direction = Vector3.left;
    }
   
    private void CheckEnemy()
    {
        if (health <= 0
            || transform.position.y < -10)
        {
            Destroy(gameObject, 0f);
            player.gameObject.GetComponent<Character>().curCount++;
        }
    }

    void Update()
    {
        CheckEnemy();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Transform PLtransform = collision.transform;
            Rigidbody2D RB = PLtransform.GetComponent<Rigidbody2D>();

            if (transform.position.x <= PLtransform.position.x)
                RB.AddForce(new Vector3(5, 5), ForceMode2D.Impulse);         
            else
                RB.AddForce(new Vector3(-5, -5), ForceMode2D.Impulse);
                
            PLtransform.GetComponent<Character>().health -= 5;              
        }
    }

    Vector3 GetDirection()
    {
        return transform.position.x < player.position.x ? Vector3.right : Vector3.left;
    }

    IEnumerator Shoot()
    {
        GameObject temp = Instantiate(bullPref, transform.position, Quaternion.identity);
        temp.name = gameObject.tag;
        temp.GetComponent<Bullet>().direction = (!right) ? -1 : 1;
        AudioSource.PlayClipAtPoint(shootClip, transform.position);
        condShoot = false;
        yield return new WaitForSeconds(100);             
    }

    private void CheckCondition()
    {
        if (condActive)
        {
            RaycastHit2D rc = Physics2D.Raycast(transform.position, new Vector2(X, Y), rayDistance, mask);

            if (condPatrol)
            {
                Y = -1;
                rayDistance = 1;

                if (rc.collider)
                    Move();
                else if (rigidBody.velocity.y >= 0)
                    Flip();
            }
            else
            {
                Y = 0;
                rayDistance = 6;

                if (Mathf.Abs(transform.position.x - player.transform.position.x) <= rayDistance)
                {
                    if (rc.collider)
                    {
                        if (rc.collider.tag == "Player")
                        {
                            StartCoroutine(Shoot());
                            condShoot = true;
                        }
                    }
                }
                else
                {
                    direction = GetDirection();
                    Move();
                }

            }
        }
    }

    private void FixedUpdate()
    {
        CheckCondition();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            condPatrol = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            condPatrol = true;
    }

    private void OnBecameInvisible()
    {
        condActive = false;
    }

    private void OnBecameVisible()
    {
        condActive = true;
    }

    private void Flip()
    {
        right = !right;
        Vector2 sc = transform.localScale;
        sc.x *= -1;
        X *= -1;
        direction *= -1;
        transform.localScale = sc;
    }
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
}
