using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private float m_playerSpeed = 3.0f;
    public Sprite[] m_playerSprites;
    private SpriteRenderer m_playerSprite;
    public GameObject m_bullet;
    public GameObject m_explosionPrefab;
    private Direction m_tankDirection;
    private float m_bulletTimer;
    Transform m_bulletsTransform;
    private float m_invincibleTime = 3f;
    private bool m_isVincible = true;
    public GameObject m_invincibleObj;

    private int m_starCount = 0;

    private int m_playerNumber = 1;

    private int m_id;

    #region 属性
    public float PlayerSpeed
    {
        get
        {
            return m_playerSpeed;
        }

        set
        {
            m_playerSpeed = value;
        }
    }

    public int PlayerNumber
    {
        get
        {
            return m_playerNumber;
        }

        set
        {
            m_playerNumber = value;
        }
    }

    public int StarCount
    {
        get
        {
            return m_starCount;
        }

        set
        {
            m_starCount = value;
        }
    }

    public int Id
    {
        get
        {
            return m_id;
        }

        set
        {
            m_id = value;
        }
    }
    #endregion

    public AudioClip m_engineDrivingClip;
    public AudioClip m_engineIdleClip;

    

    // Use this for initialization
    void Start () {
        m_playerSprite = GetComponent<SpriteRenderer>();
        m_bulletsTransform = GameObject.Find("Bullets").transform;
        //m_invincibleObj = GameObject.Find("Shield");
        m_playerSprite.sprite = m_playerSprites[m_starCount * 4];
	}
	
	// Update is called once per frame
	void Update () {
        if (m_isVincible)
        {
            m_invincibleTime -= Time.deltaTime;
            if (m_invincibleTime < 0)
            {
                m_isVincible = false;
                m_invincibleObj.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (Manager.Instance.IsGameOver)
        {
            return;
        }
        TankMove();
        if (0.4f < m_bulletTimer)
        {
            TankAttack();
        }
        else
        {
            m_bulletTimer += Time.fixedDeltaTime;
        }
    }

    private void TankAttack()
    {
        if ((m_playerNumber == 1 && Input.GetKeyDown(KeyCode.Space)) || (m_playerNumber == 2 && Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            GameObject go = Instantiate(m_bullet, transform.position, Quaternion.identity);
            go.transform.Rotate(Vector3.back, 90 * (int)m_tankDirection);
            go.transform.parent = m_bulletsTransform;
            m_bulletTimer = 0.0f;
        }
    }

    public bool IsTouchedOtherTank(Vector2 pos, int dir)
    {
        //Debug.Log(m_playerNumber.ToString() + ":" + pos);
        foreach (int id in Manager.Instance.m_tankPosList.Keys)
        {
            if (id == m_id)
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
        float h = Input.GetAxisRaw("Horizontal" + m_playerNumber.ToString());
        float v = 0;
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
            v = Input.GetAxisRaw("Vertical" + m_playerNumber.ToString());
            movePos = Vector3.up * v * PlayerSpeed * Time.fixedDeltaTime;
            if (!IsTouchedOtherTank((Vector2)transform.position + movePos, 1))
            {
                transform.Translate(Vector3.up * v * PlayerSpeed * Time.fixedDeltaTime);
            }
            if (v == 1.0f)
            {
                m_playerSprite.sprite = m_playerSprites[m_starCount * 4 + 0];
                m_tankDirection = Direction.UP;
            }
            else if (v == -1.0f)
            {
                m_playerSprite.sprite = m_playerSprites[m_starCount * 4 + 2];
                m_tankDirection = Direction.BOTTOM;
            }
        }
        else if (h == 1.0f)
        {
            m_playerSprite.sprite = m_playerSprites[m_starCount * 4 + 1];
            m_tankDirection = Direction.RIGHT;
        }
        else
        {
            m_playerSprite.sprite = m_playerSprites[m_starCount * 4 + 3];
            m_tankDirection = Direction.LEFT;
        }
        //Debug.Log(m_playerNumber.ToString() + ":" + transform.position);
        //Debug.Log("count:" + Manager.Instance.m_tankPosList.Count);

        /*  //坦克的声音
        if (0.05f < Mathf.Abs(h) || 0.05f < Mathf.Abs(v))
        {
            AudioManager.Instance.PlayAudio(m_engineDrivingClip);
        }
        else
        {
            AudioManager.Instance.PlayAudio(m_engineIdleClip);
        }
        */
    }

    public void TankDie(int tankNo)
    {
        if (!m_isVincible)
        {
            Instantiate(m_explosionPrefab, transform.position, Quaternion.identity);
            Manager.Instance.m_tankPosList.Remove(tankNo);
            Destroy(gameObject);
            Manager.Instance.Revive(tankNo);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "LifeBonus":
                GetLifeBonus();
                Destroy(collision.gameObject);
                break;
            case "WatchBonus":
                Manager.Instance.EnemyAllStop();
                Destroy(collision.gameObject);
                break;
            case "ShovelBonus":
                Destroy(collision.gameObject);
                break;
            case "BoomBonus":
                Manager.Instance.EnemyAllBoom();
                Destroy(collision.gameObject);
                break;
            case "StarBonus":
                GetStarBonus();
                Destroy(collision.gameObject);
                break;
            case "HatBonus":
                GetHatBonus();
                Destroy(collision.gameObject);
                break;
            default:
                break;
        }
    }

    private void GetHatBonus()
    {
        m_isVincible = true;
        m_invincibleTime = 8;
        m_invincibleObj.SetActive(true);
    }

    private void GetLifeBonus()
    {
        if (m_playerNumber == 1)
        {
            Manager.Instance.LifeCount1++;
        }
        else if (m_playerNumber == 2)
        {
            Manager.Instance.LifeCount2++;
        }
    }

    public void GetStarBonus()
    {
        if (3 <= m_starCount)
        {
            return;
        }

        // 换皮肤
        m_starCount++;
        m_playerSprite.sprite = m_playerSprites[m_starCount * 4 + (int)m_tankDirection];

        // 加快移动速度
        m_playerSpeed += 0.5f;
        Debug.Log(m_playerSpeed);

        // 加快子弹速度和威力
    }
}

public enum Direction
{
    UP,
    RIGHT,
    BOTTOM,
    LEFT
}
