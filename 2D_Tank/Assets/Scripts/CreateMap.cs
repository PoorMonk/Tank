using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreateMap : MonoBehaviour {

    public GameObject[] m_mapItems;
    private List<Vector3> m_itemPosList = new List<Vector3>();
    private int m_enemyCount = 20;
    private GameObject m_enemyTank;

    private void Awake()
    {
        InitMap();
    }

    private void InitMap()
    {
        CreateItem(m_mapItems[0], new Vector3(0, -8, 0));   //CreateKing
        //CreateItem(m_mapItems[7], new Vector3(-0.75f, -7.75f, 0));  //CreateWall
        //CreateItem(m_mapItems[7], new Vector3(-0.75f, -8.25f, 0));
        CreateItem(m_mapItems[9], new Vector3(-1f, -8f, 0));
        CreateItem(m_mapItems[9], new Vector3(1f, -8f, 0));
        //for (int i = 0; i < 4; i++)
        //{
        //    CreateItem(m_mapItems[7], new Vector3(-0.75f + 0.5f * i, -7.25f, 0));
        //}

        for (int i = -1; i < 2; i++)
        {
            CreateItem(m_mapItems[9], new Vector3(i, -7, 0));
            CreateEnemyWithPos(new Vector3(i * 10, 8, 0));
        }
        for (int i = 0; i < 20; i++)
        {
            CreateItem(m_mapItems[2], GetRandomPosition());
            CreateItem(m_mapItems[3], GetRandomPosition());
            CreateItem(m_mapItems[4], GetRandomPosition());
        }
        for (int i = 0; i < 60; i++)
        {
            CreateItem(m_mapItems[9], GetRandomPosition());
        }

        Manager.Instance.Revive(1);
        if (!Manager.Instance.m_singleMode)
        {
            Manager.Instance.Revive(2);
        }      
        InvokeRepeating("CreateEnemy", 5.0f, 5f);
    }

    private GameObject CreateItem(GameObject obj, Vector3 pos)
    {
        GameObject go = Instantiate(obj, pos, Quaternion.identity);
        go.transform.SetParent(transform);
        m_itemPosList.Add(pos);
        return go;
    }

    private Vector3 GetRandomPosition()
    {
        while (true)
        {
            Vector3 pos = new Vector3(Random.Range(-10, 11), Random.Range(-7, 8), 0);
            if (!m_itemPosList.Contains(pos))
            {
                return pos;
            }
        }
    }

    private void CreateEnemyWithPos(Vector3 pos)
    {
        CreateItem(m_mapItems[5], pos);
        m_enemyCount--;
        m_enemyTank = GameObject.Find("EnemyImg");
        if (m_enemyTank != null)
        {
            m_enemyTank.SetActive(false);
        }
        
    }

    private void CreateEnemy()
    {
        if (Manager.Instance.IsGameOver)
        {
            CancelInvoke();
            return;
        }
        int xPos = Random.Range(-1, 2);
        CreateEnemyWithPos(new Vector3(xPos * 10, 8, 0));
        if (m_enemyCount <= 0)
        {
            CancelInvoke();
        }
    }
}
