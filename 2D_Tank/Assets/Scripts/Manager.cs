using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    private int m_lifeCount1 = 3;
    private int m_score1;
    private int m_lifeCount2 = 3;
    private int m_score2;
    private bool m_isGameOver = false;
    private static Manager m_instance;
    public GameObject m_bornObj;
    public Text m_lifeCountText1;
    public Text m_lifeCountText2;
    public Text m_levelText;
    private GameObject m_player2;
    private GameObject m_gameOverImg;
    private GameObject m_map;
    public List<GameObject> m_enemyLists = new List<GameObject>();
    private bool m_enemyCanMove = true;
    private float m_cannotMoveTime;

    public bool m_singleMode = true;

    private int m_tankID = 3;
    public Dictionary<int, GameObject> m_tankPosList = new Dictionary<int, GameObject>();

    #region 属性
    public static Manager Instance
    {
        get
        {
            return m_instance;
        }
    }

    public bool IsGameOver
    {
        get
        {
            return m_isGameOver;
        }

        set
        {
            m_isGameOver = value;
        }
    }

    public int LifeCount1
    {
        get
        {
            return m_lifeCount1;
        }

        set
        {
            m_lifeCount1 = value;
            m_lifeCountText1.text = m_lifeCount1.ToString();
        }
    }

    public int Score1
    {
        get
        {
            return m_score1;
        }

        set
        {
            m_score1 = value;
        }
    }

    public int LifeCount2
    {
        get
        {
            return m_lifeCount2;
        }

        set
        {
            m_lifeCount2 = value;
            m_lifeCountText2.text = m_lifeCount2.ToString();
        }
    }

    public int Score2
    {
        get
        {
            return m_score2;
        }

        set
        {
            m_score2 = value;
        }
    }

    public bool EnemyCanMove
    {
        get
        {
            return m_enemyCanMove;
        }

        set
        {
            m_enemyCanMove = value;
        }
    }

    public int TankID
    {
        get
        {
            return m_tankID;
        }

        set
        {
            m_tankID = value;
        }
    }
    #endregion

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        m_map = GameObject.Find("Map");
        m_gameOverImg = GameObject.Find("UIGameOver");
        m_gameOverImg.SetActive(false);
        m_player2 = GameObject.Find("PlayerTank2");
        if (2 == PlayerPrefs.GetInt("PlayerMode"))
        {
            m_singleMode = false;
        }
        else
        {
            m_player2.SetActive(false);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start() 
    {
        m_tankID = 3;
        m_tankPosList.Clear();    
    }

    private void Update()
    {
        if (!m_enemyCanMove)
        {
            m_cannotMoveTime += Time.deltaTime;
            if (8 <= m_cannotMoveTime)
            {
                m_enemyCanMove = true;
            }
        }
    }

    public void Revive(int playNo = 1)
    {
        if (1 == playNo)
        {
            if (0 < m_lifeCount1)
            {
                GameObject go = Instantiate(m_bornObj, new Vector3(-2, -8, 0), Quaternion.identity);
                go.GetComponent<Born>().m_isPlayer = true;
                go.GetComponent<Born>().m_playerNo = playNo;
                m_lifeCount1--;
                m_lifeCountText1.text = m_lifeCount1.ToString();
            }
            else
            {
                if (m_singleMode)
                {
                    GameOver();
                }
            }
        }
        else if (2 == playNo)
        {
            if (0 < m_lifeCount2)
            {
                GameObject go = Instantiate(m_bornObj, new Vector3(2, -8, 0), Quaternion.identity);
                go.GetComponent<Born>().m_isPlayer = true;
                go.GetComponent<Born>().m_playerNo = playNo;
                m_lifeCount2--;
                m_lifeCountText2.text = m_lifeCount2.ToString();
            }
            else
            {
                if (m_lifeCount1 <= 0)
                {
                    GameOver();
                }
            }
        }
    }

    public void GameOver()
    {
        StartCoroutine("ShowGameOver");
    }

    IEnumerator ShowGameOver()
    {        
        m_isGameOver = true;
        m_gameOverImg.SetActive(true);
        m_map.SetActive(false);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

    public void EnemyAllStop()
    {
        m_enemyCanMove = false;
    }

    public void EnemyAllBoom()
    {
        for (int i = 0; i < m_enemyLists.Count; i++)
        {
            m_enemyLists[i].GetComponent<Enemy>().EnemyDie();
        }
        m_enemyLists.Clear();
    }
}
