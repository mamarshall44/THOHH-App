﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

	public GameObject firstTurned;
	public GameObject secondTurned;
	public Sprite back;
	public int matches;
	public Text matchCount;
	public float wait = 3f;
	public Text timer;
	public GameObject popUp;
	public GameObject popUpTimed;
	public Text pScore;
	public Text hScore;
	public GameObject hNotice;

	public AudioClip matchClip;
	public AudioClip badClip;

	float t;
	float waitTime;
	float highScore;

	void Start()
	{
		highScore = PlayerPrefs.GetFloat ("High Score");
		popUp.SetActive (false);
		if (PlayerPrefs.GetInt ("Timed") == 1)
		{
			timer.gameObject.SetActive (true);
		}
	}
	// Update is called once per frame
	void Update () 
	{
		if (matches < 9)
		{
			if (PlayerPrefs.GetInt ("Timed") == 1)
			{
			
				t += Time.deltaTime;
				int minutes = Mathf.FloorToInt (t / 60F);
				int seconds = Mathf.FloorToInt (t - minutes * 60);
				string niceTime = string.Format ("{0:0}:{1:00}", minutes, seconds);

				timer.text = "Time " + niceTime;
			}
			else
			{
				timer.gameObject.SetActive (false);
			}

			if (secondTurned != null)
			{
				if (firstTurned.GetComponent<FlipCards> ().tileFront == secondTurned.GetComponent<FlipCards> ().tileFront)
				{
					GetComponent<AudioSource> ().clip = matchClip;

					if (waitTime >= wait)
					{
						waitTime = 0;
						matches += 1;
						firstTurned.SetActive (false);
						secondTurned.SetActive (false);
						firstTurned = null;
						secondTurned = null;
					}
					else
					{
						waitTime += Time.deltaTime;
					}
				}
				else
				{
					GetComponent<AudioSource> ().clip = badClip;

					if (waitTime >= wait)
					{
						waitTime = 0;
						firstTurned.GetComponent<Image> ().sprite = back;
						secondTurned.GetComponent<Image> ().sprite = back;
						firstTurned.GetComponent<Button> ().interactable = true;
						secondTurned.GetComponent<Button> ().interactable = true;
						firstTurned = null;
						secondTurned = null;
					}
					else
					{
						waitTime += Time.deltaTime;
					}
				}

				GetComponent<AudioSource> ().Play ();
			}

			matchCount.text = "Match Count " + matches;
		}
		else
		{
			if (PlayerPrefs.GetInt ("Timed") == 1)
			{
				popUpTimed.SetActive (true);
				if (t < highScore || highScore == 0f)
				{
					highScore = t;
					PlayerPrefs.SetFloat ("High Score", highScore);
					hNotice.SetActive (true);
				}

				int pMin = Mathf.FloorToInt (t / 60F);
				int pSec = Mathf.FloorToInt (t - pMin * 60);
				string pTime = string.Format ("{0:0}:{1:00}", pMin, pSec);
				pScore.text = pTime;

				int hMin = Mathf.FloorToInt (highScore / 60F);
				int hSec = Mathf.FloorToInt (highScore - hMin * 60);
				string hTime = string.Format ("{0:0}:{1:00}", hMin, hSec);
				hScore.text = hTime;
			}
			else
			{
				popUp.SetActive (true);
			}
		}
	}
}