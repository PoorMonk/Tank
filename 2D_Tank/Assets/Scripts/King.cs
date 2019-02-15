using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour {
    private SpriteRenderer m_sr;
    public Sprite m_brokenKingSprite;
	// Use this for initialization
	void Start () {
        m_sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void KingDie()
    {
        m_sr.sprite = m_brokenKingSprite;
        Manager.Instance.IsGameOver = true;
    }
}
