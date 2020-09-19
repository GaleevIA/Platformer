using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public List<Collider2D> colls;
    Rigidbody2D RB;
    public float timer;
    public float expPower;

    // Start is called before the first frame update
    void Start()
    {
        timer = 1;
        expPower = 2;
        RB = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if (!colls.Contains(collision))
                colls.Add(collision);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            foreach(Collider2D collider in colls)
            {
                if (collision == collider)
                {
                    colls.Remove(collision);
                    break;
                }               
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Invoke("Explode", timer);
        }
    }

    void Explode()
    {
        foreach(Collider2D coll in colls)
        {
            coll.GetComponent<Rigidbody2D>().AddForce(Power(coll), ForceMode2D.Impulse);
            coll.GetComponent<Enemy>().health -= 30;
        }

        RB.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
        RB.simulated = false;
        Destroy(gameObject, 0.5f);
    }

    Vector2 Power(Collider2D collider)
    {
        float dir;
        Vector3 collPos = collider.transform.position;
        dir = (collPos.x > transform.position.x) ? 1 : -1;

        return new Vector2(dir, dir) * expPower;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
