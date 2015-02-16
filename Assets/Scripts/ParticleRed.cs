using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class ParticleRed : MonoBehaviour
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
		private float threshold;
		private float percent = 0.7f;
		private float scale;
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
			threshold = max_shot * percent;*/

						if (timer - Time.time >= 0.0) {
								max_shot = Mathf.Max (max_shot, signal);
								min_shot = Mathf.Min (min_shot, signal);
								threshold = max_shot * percent;

								//for test
								/*max_shot = Mathf.Max (max_shot, signal);
				min_shot = Mathf.Min (min_shot, signal);*/

								//for text
								msg.text = "Now Calibrating\n" + "Remaining Time : " + Mathf.CeilToInt (timer).ToString ();
								timer -= Time.deltaTime;
						} else {
								//for text
								msg.text = null;

								//for scale
								scale = (signal - min_shot) / (max_shot - min_shot) * 5.0f + 20.0f;
								this.transform.localScale = new Vector3 (scale, scale, scale);

								//for light
								this.light.intensity = (signal - min_shot) / (max_shot - min_shot) + 1.0f;
								this.light.color = new Color (1.0f, (120.0f - signal / (max_shot - min_shot) * 50.0f) / 255.0f, (120.0f - signal / (max_shot - min_shot) * 50.0f) / 255.0f);

								//for particle 
								this.particleSystem.startColor = new Color (1.0f, (100.0f - signal / (max_shot - min_shot) * 70.0f) / 255.0f, (100.0f - signal / (max_shot - min_shot) * 70.0f) / 255.0f);
				
								//for audio
								if (signal > threshold && !this.audio.isPlaying) {
										this.audio.Play ();
								}
						}
				}
		}
}