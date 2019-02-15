using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float m_bulletSpeed = 10.0f;
    public bool m_isMySelf = true;
    public AudioClip m_blockHitClip;
    public AudioClip m_dieClip;
    //public AudioClip m_getBonusClip;
    public AudioClip m_heartDamageClip;
    private GameObject m_comboWallParent;
    private GameObject m_comboBlockParent;

    private void Awake()
    {
        m_comboWallParent = GameObject.Find("ComboWall");
        m_comboBlockParent = GameObject.Find("ComboBlock");
        if (m_comboWallParent != null && m_comboWallParent.transform.childCount < 1)
        {
            Destroy(m_comboWallParent.gameObject);
        }
        if (m_comboBlockParent != null && m_comboBlockParent.transform.childCount < 1)
        {
            Destroy(m_comboBlockParent.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(transform.up * m_bulletSpeed * Time.deltaTime, Space.World);
        if (transform.position.x < -11 || 11 < transform.position.x ||
            transform.position.y < -9 || 9 < transform.position.y)
        {
            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Tank":
                if (!m_isMySelf)
                {
                    int num = collision.gameObject.GetComponent<Player>().PlayerNumber;
                    collision.SendMessage("TankDie", num);
                    AudioManager.Instance.PlayAudio(m_dieClip);
                    Destroy(gameObject);
                }
                break;
            case "Enemy":
                if (m_isMySelf)
                {
                    collision.SendMessage("EnemyDie");
                    Destroy(gameObject);
                    //Manager.Instance.Score += 1;
                    AudioManager.Instance.PlayAudio(m_dieClip);
                    //Debug.Log("Score: " + Manager.Instance.Score);
                }
                break;
            case "Block":
                Destroy(gameObject);
                AudioManager.Instance.PlayAudio(m_blockHitClip);
                break;
            case "King":
                collision.SendMessage("KingDie");
                AudioManager.Instance.PlayAudio(m_heartDamageClip);
                Destroy(gameObject);
                Manager.Instance.GameOver();
                break;
            case "Wall":
                Destroy(collision.gameObject);
                Destroy(gameObject);
                break;
            case "EnemyBullet":
                if (m_isMySelf)
                {
                    Destroy(collision.gameObject);
                    Destroy(gameObject);
                }
                break;
            default:
                break;
        }
    }
}
