using UnityEngine;
using System.Collections;
using System;

public class Color : MonoBehaviour
{
	/*
	//variables for calibration
	private Connect2Biopac m_C2B;
	private float timer = 0.0f;
	private const float CALIBRATION_TIME = 3.0f;
	*/
	private float sinData = 0.0f;	//for test

	// Use this for initialization
	void Start () 
	{
		/*
		m_C2B = GameObject.FindGameObjectWithTag("BIOPAC").GetComponent<Connect2Biopac>();
		timer = Time.time;
		*/
	}

	//for threshold
	private float max_shot = 10.0f;
	private float percent_shot = 0.8f;
	//private bool shot = false;
	//private float before_data = 0.0f;

	//additional variables
	private Vector3 min_size = new Vector3(10.0f, 10.0f, 10.0f);	//通常状態Scale
	private Vector3 max_size = new Vector3(12.0f, 12.0f, 12.0f);	//拍動状態Scale
	private float threshold_shot;
	private float threshold_audio;
	private float percent_audio = 0.7f;
	
	// Update is called once per frame
	void Update ()
	{
	
		//疑似心拍 for test
		sinData = Mathf.Sin (5.0f * Time.time) * 10.0f;
		//Debug.Log (sinData);
		threshold_shot = max_shot * percent_shot;
		threshold_audio = max_shot * percent_audio;

		/*
		if ((Time.time - timer) > CALIBRATION_TIME) {
			max_shot = Mathf.Max (max_shot, m_C2B.inputData);
		}else{
		*/
			//for scale
			if (sinData > threshold_shot) {
				this.transform.localScale = max_size;
			} else {
				this.transform.localScale = min_size;
			}

			//for color
			this.renderer.material.color = new UnityEngine.Color(1, 1 - sinData / max_shot, 1- sinData / max_shot);

			//for audio
			if (sinData > threshold_audio && !this.audio.isPlaying) {
				this.audio.Play ();
			}
		//}
	}
}