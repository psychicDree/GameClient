using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
	private TMP_Text timer;
	private int time = -1;
	private Button gameWonButton;
	private Button gameLostButton;
	void Start()
	{
		timer = transform.Find("Timer").GetComponent<TMP_Text>();
		timer.gameObject.SetActive(false);
		gameWonButton = transform.Find("GameOver/GameWonButton").GetComponent<Button>();
		gameLostButton = transform.Find("GameOver/GameLostButton").GetComponent<Button>();
		gameWonButton.gameObject.SetActive(false); 
		gameLostButton.gameObject.SetActive(false);
		gameWonButton.onClick.AddListener(OnResultClick);
		gameLostButton.onClick.AddListener(OnResultClick);
	}

	public override void OnEnter()
	{
		EnterAnimation();
	}

	private void EnterAnimation()
	{
		gameObject.SetActive(true);
		transform.localScale = Vector3.zero;
		transform.DOScale(1, 0.5f);
		transform.localPosition = new Vector3(650, 0, 0);
		transform.DOLocalMove(Vector3.zero, 0.5f);
	}

	private void HideAnimation()
	{
		transform.DOScale(0, 0.5f);
		transform.DOLocalMoveX(650, 0.5f).OnComplete(() => gameObject.SetActive(false));
	}

	public override void OnResume()
	{
		EnterAnimation();
	}

	public override void OnPause()
	{
		HideAnimation();
	}

	public override void OnExit()
	{
		HideAnimation();
	}

	private void ShowTimer(int time)
	{
		timer.gameObject.SetActive(true);
		timer.text = time.ToString();
		Color tempColor = timer.color;
		tempColor.a = 1;
		timer.color = tempColor;
		timer.transform.localScale = Vector3.one;
		timer.transform.DOScale(2, 0.3f).SetDelay(0.3f);
		timer.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(()=>timer.gameObject.SetActive(false));
	}

	public void ShowTimerSync(int time)
	{
		this.time = time;
	}

	private void Update()
	{
		if (this.time > -1)
		{
			ShowTimer(time);
			time = -1;
		}
	}

	private void OnResultClick()
	{
		uiManager.PopPanel();
		uiManager.PopPanel();
		facade.OnGameOver();
	}
	public void OnGameOverResponse(ReturnCode returnCode)
	{
		switch (returnCode)
		{
			case ReturnCode.GameWon:
				gameWonButton.gameObject.SetActive(true);
				break;
			case ReturnCode.GameLost:
				gameLostButton.gameObject.SetActive(true);
				break;
		}
	}
}
