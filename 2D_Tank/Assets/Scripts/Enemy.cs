using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private float playerSpeed = 3.0f;
    public Sprite[] m_playerSprites;
    private SpriteRenderer m_playerSprite;
    public GameObject m_bullet;
    public GameObject m_explosionPrefab;
    private Direction m_tankDirection;
    private float m_bulletTimer;
    Transform m_bulletsTransform;
    private float m_enemyMoveTime;
    private float h, v;
    private Rigidbody2D m_rigidibody2D;
    private bool m_canMove = true;
    private float m_cannotMoveTime;

    private int m_enemyID;

    public int EnemyID
    {
        get
        {
            return m_enemyID;
        }

        set
        {
            m_enemyID = value;
        }
    }

    public float PlayerSpeed
    {
        get
        {
            return playerSpeed;
        }

        set
        {
            playerSpeed = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        m_playerSprite = GetComponent<SpriteRenderer>();
        m_rigidibody2D = GetComponent<Rigidbody2D>();
        m_bulletsTransform = GameObject.Find("Bullets").transform;
        ChangeDirection(ref h, ref v);
    }

    private void FixedUpdate()
    {
        if (!Manager.Instance.EnemyCanMove || Manager.Instance.IsGameOver)
        {
            return;
        }
        if (3f < m_bulletTimer)
        {
            TankAttack();
        }
        else
        {
            m_bulletTimer += Time.fixedDeltaTime;
        }
        TankMove();
    }

    private void TankAttack()
    {        
        GameObject go = Instantiate(m_bullet, transform.position, Quaternion.identity);
        go.transform.Rotate(Vector3.back, 90 * (int)m_tankDirection);
        go.transform.parent = m_bulletsTransform;
        m_bulletTimer = 0.0f;
    }

    private void ChangeDirection(ref float h, ref float v)
    {
        int num = Random.Range(0, 8);
        if (0 == num) //up
        {
            h = 0;
            v = 1;
        }
        else if (0 < num && num <= 2)
        {
            h = 1;
            v = 0;
        }
        else if (2 < num && num <= 4)
        {
            h = -1;
            v = 0;
        }
        else
        {
            h = 0;
            v = -1;
        }
    }

    public bool IsTouchedOtherTank(Vector2 pos, int dir)
    {        
        foreach (int id in Manager.Instance.m_tankPosList.Keys)
        {
            if (id == m_enemyID)
            {
                continue;
            }
            Vector2 vec = (Vector2)Manager.Instance.m_tankPosList[id].transform.position;
            if (dir == 0)
            {
                if (Mathf.Abs(vec.y - pos.y) < 0.9f && Mathf.Abs(vec.x - pos.x) < 1.0f)
                {
                    return true;
                }
            }
            else 
            {
                if (Mathf.Abs(vec.x - pos.x) < 0.9f && Mathf.Abs(vec.y - pos.y) < 1.0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void TankMove()
    {
        if (3f <= m_enemyMoveTime)
        {
            ChangeDirection(ref h, ref v);
            m_enemyMoveTime = 0f;
        }
        else
        {
            m_enemyMoveTime += Time.fixedDeltaTime;
        }

        Vector2 movePos = new Vector2();
        if (h != 0)
        {
            movePos = Vector3.right * h * PlayerSpeed * Time.fixedDeltaTime;
            if (!IsTouchedOtherTank((Vector2)transform.position + movePos, 0))
            {
                transform.Translate(Vector3.right * h * PlayerSpeed * Time.fixedDeltaTime);
            }           
        }
        
        if (h == 0)
        {
            movePos = Vector3.up * v * PlayerSpeed * Time.fixedDeltaTime;
            if (!IsTouchedOtherTank((Vector2)transform.position + movePos, 1))
            {
                transform.Translate(Vector3.up * v * PlayerSpeed * Time.fixedDeltaTime);
            }
           
            if (v == 1.0f)
            {
                m_playerSprite.sprite = m_playerSprites[0];
                m_tankDirection = Direction.UP;
            }
            else if (v == -1.0f)
            {
                m_playerSprite.sprite = m_playerSprites[2];
                m_tankDirection = Direction.BOTTOM;
            }
        }
        else if (h == 1.0f)
        {
            m_playerSprite.sprite = m_playerSprites[1];
            m_tankDirection = Direction.RIGHT;
        }
        else
        {
            m_playerSprite.sprite = m_playerSprites[3];
            m_tankDirection = Direction.LEFT;
        }
    }

    public void EnemyDie()
    {
        Instantiate(m_explosionPrefab, transform.position, Quaternion.identity);
        Manager.Instance.m_tankPosList.Remove(m_enemyID);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            h = 0;
            v = 0;
            m_enemyMoveTime = 3f;
            //m_rigidibody2D.mass = 100;
        }
    }
}
