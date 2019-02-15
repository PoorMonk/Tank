using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StartGameUIController : MonoBehaviour {

    public Transform m_startUI;

	// Use this for initialization
	void Start () {
        m_startUI.DOLocalMoveY(5, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
