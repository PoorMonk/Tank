using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class Option : MonoBehaviour {

    public Transform m_pos1;
    public Transform m_pos2;
    private int m_option;
    private float m_showTime = 0f;
    private bool m_isShow = true;
    private float m_disappearTime = 0f;
    private GameObject m_cover;

	// Use this for initialization
	void Start () {
        m_option = 1;
        m_cover = GameObject.Find("Cover");
        Tweener tw = GetComponent<Image>().DOFade(0, 2);
        tw.SetLoops(-1);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.W))
        {
            m_option = 1;
            
            transform.position = m_pos1.position;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            m_option = 2;
            transform.position = m_pos2.position;
        }
        if (Input.GetKeyDown(KeyCode.Space) && m_option != 0)
        {
            PlayerPrefs.SetInt("PlayerMode", m_option);
            SceneManager.LoadScene(1);
        }

        //OptionAnimator();
	}

    void OptionAnimator()
    {
        if (m_isShow)
        {
            m_cover.SetActive(false);
            m_showTime += Time.deltaTime;
            if (1.0f <= m_showTime)
            {
                m_isShow = false;
                m_disappearTime = 0;
            }
        }
        else
        {
            m_cover.SetActive(true);
            m_disappearTime += Time.deltaTime;
            if (0.5f <= m_disappearTime)
            {
                m_isShow = true;
                m_showTime = 0;
            }
        }
    }
}
