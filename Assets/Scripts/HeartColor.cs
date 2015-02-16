using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class HeartColor : MonoBehaviour
{
		//variables for calibration
		private IConnect2Devices devices;
		private float timer = 0.0f;
		private const float CALIBRATION_TIME = 3.5f;
		private bool _isPortOpen;

		// Use this for initialization
		void Start ()
		{
				devices = GameObject.FindGameObjectWithTag ("Arduino").GetComponent<Connect2Arduino> ();
				_isPortOpen = devices.SetPort ("COM3", 115200, 1);
				/*("/dev/tty.usbmodem14121", 115200);
				("/dev/tty.usbmodem1451", 115200);
				("COM3", 115200);*/
				timer = CALIBRATION_TIME;
		}

		//variables
		private float max_shot = 0.0f;
		private float min_shot = 10000.0f;
		private float threshold_shot;
		private float percent_shot = 0.7f;
		private float threshold_audio;
		private float percent_audio = 0.7f;
		private Vector3 min_size = new Vector3 (10.0f, 10.0f, 10.0f);
		//通常状態Scale
		private Vector3 max_size = new Vector3 (20.0f, 20.0f, 20.0f);
		//拍動状態Scale
		private float[] data;
		private float signal;
		public Text msg;
	
		// Update is called once per frame
		void Update ()
		{
				if (_isPortOpen) {
						msg.color = new Color (1, 0, 0);
						msg.text = "Port can not open.\n";
				} else {
						data = devices.GetVoltages ();
						signal = data [0];
	
						//疑似心拍 for test
						/*data = Mathf.Sin (5.0f * Time.time) * 10.0f + 10.0f;
			threshold_shot = max_shot * percent_shot;
			threshold_audio = max_shot * percent_audio;*/

						if (timer >= 0.0) {
								max_shot = Mathf.Max (max_shot, signal);
								min_shot = Mathf.Min (min_shot, signal);
								threshold_shot = max_shot * percent_shot;
								threshold_audio = max_shot * percent_audio;

								//for test
								/*max_shot = Mathf.Max (max_shot, signal);
			min_shot = Mathf.Min (min_shot, signal);*/

								//for text
								msg.text = "Now Calibrating\n" + "Remaing Time : " + Mathf.CeilToInt (timer).ToString ();
								timer -= Time.deltaTime;
						} else {
								//for text
								msg.text = null;

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
}