using UnityEngine;
using System.Collections;
using System;

public class HeartColor : MonoBehaviour
{
	//variables for calibration
	private ConnectInterface devices;
	private float timer = 0.0f;
	private const float CALIBRATION_TIME = 3.5f;
	private float[] data;
	private float signal;

	// Use this for initialization
	void Start ()
	{
		devices = GameObject.FindGameObjectWithTag ("Arduino").GetComponent<Connect2Arduino> ();
		devices.setPort ("COM3", 115200, 1);
		//("/dev/tty.usbmodem14121", 115200);
		//("/dev/tty.usbmodem1451", 115200);
		//("COM3", 115200);
		timer = CALIBRATION_TIME;
	}

	//variables
	private float max_shot = 0.0f;
	private float min_shot = 0.0f;
	private float threshold_shot;
	private float percent_shot = 0.8f;
	private float threshold_audio;
	private float percent_audio = 0.7f;
	private Vector3 min_size = new Vector3 (10.0f, 10.0f, 10.0f); //通常状態Scale
	private Vector3 max_size = new Vector3 (20.0f, 20.0f, 20.0f); //拍動状態Scale
	
	// Update is called once per frame
	void Update ()
	{
	
		//疑似心拍 for test
		/*
		data = Mathf.Sin (5.0f * Time.time) * 10.0f + 10.0f;
		Debug.Log (data);
		threshold_shot = max_shot * percent_shot;
		threshold_audio = max_shot * percent_audio;
		*/

		data = devices.getVoltages ();
		signal = data [0];

		if ((timer - Time.time) >= 0.0) {
			max_shot = Mathf.Max (max_shot, signal);
			min_shot = Mathf.Min (min_shot, signal);
			threshold_shot = max_shot * percent_shot;
			threshold_audio = max_shot * percent_audio;

			//for test
			/*
			max_shot = Mathf.Max (max_shot, signal);
			min_shot = Mathf.Min (min_shot, signal);
			*/
		} else {
			//for scale
			if (signal > threshold_shot) {
				this.transform.localScale = max_size;
			} else {
				this.transform.localScale = min_size;
			}

			//for color
			this.renderer.material.color = new Color (1, 1 - signal / max_shot, 1 - signal / max_shot);

			//for light
			this.light.intensity = (signal - min_shot) / (max_shot - min_shot) + 1.0f;

			//for audio
			if (signal > threshold_audio && !this.audio.isPlaying) {
				this.audio.Play ();
			}
		}
	}
}