using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Born : MonoBehaviour {

    public GameObject m_playerObj;
    public GameObject[] m_enemyObjList;
    public bool m_isPlayer = false;
    public int m_playerNo = 1;
    private Transform m_map;

    private void Awake()
    {
        m_map = GameObject.Find("Map").transform;
    }

    // Use this for initialization
    void Start () {
        Invoke("CreatePlayer", 1f);
        Destroy(gameObject, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreatePlayer()
    {
        GameObject go = null;
        if (m_isPlayer)
        {
            go = Instantiate(m_playerObj, transform.position, Quaternion.identity);
            go.GetComponent<Player>().PlayerNumber = m_playerNo;
            go.GetComponent<Player>().Id = m_playerNo;
            if (Manager.Instance.m_tankPosList.ContainsKey(m_playerNo))
            {
                Manager.Instance.m_tankPosList[m_playerNo] = go;
            }
            else
            {
                Manager.Instance.m_tankPosList.Add(m_playerNo, go);
            }
        }
        else
        {
            int num = Random.Range(0, 2);
            go = Instantiate(m_enemyObjList[num], transform.position, Quaternion.identity);
            go.GetComponent<Enemy>().EnemyID = Manager.Instance.TankID;
            Manager.Instance.m_enemyLists.Add(go);
            Manager.Instance.m_tankPosList.Add(Manager.Instance.TankID, go);
            Manager.Instance.TankID++;
        }
        go.transform.SetParent(m_map);
    }
}
