using UnityEngine;
using System.Collections;
using System;

public class ParticleBlue : MonoBehaviour
{
		//variables for calibration
		//private Connect2Biopac m_C2B;
		private float timer = 0.0f;
		private const float CALIBRATION_TIME = 3.5f;

		private float sinData = 0.0f;
		//for test

		// Use this for initialization
		void Start ()
		{
				//m_C2B = GameObject.FindGameObjectWithTag("BIOPAC").GetComponent<Connect2Biopac>();
				timer = CALIBRATION_TIME;
		}

		//for threshold
		private float max_shot = 10.0f;
		private float min_shot = 0.0f;
		//private bool shot = false;
		//private float before_data = 0.0f;

		//additional variables
		private float scale;
		private float threshold;
		private float percent = 0.7f;
		private float amp;

		// Update is called once per frame
		void Update ()
		{
				//疑似心拍 for test
				sinData = Mathf.Sin (5.0f * Time.time) * 10.0f + 10.0f;
				//Debug.Log (sinData);
				threshold = max_shot * percent;

				if ((timer - Time.time) >= 0.0) {
						//for BIOPAC
						//max_shot = Mathf.Max (max_shot, m_C2B.inputData);
						//min_shot = Mathf.Min (min_shot, m_C"B.inputData);

						//for test
						max_shot = Mathf.Max (max_shot, sinData);
						min_shot = Mathf.Min (min_shot, sinData);
				} else {

						//for scale
						scale = (sinData - min_shot) / (max_shot - min_shot) * 5.0f + 20.0f;
						this.transform.localScale = new Vector3 (scale, scale, scale);

						//for light
						this.light.intensity = (sinData - min_shot) / (max_shot - min_shot) + 1.0f;
						this.light.color = new UnityEngine.Color (50f / 255f, (50.0f + sinData / (max_shot - min_shot) * 70.0f) / 255.0f, 1.0f);

						//for particle 
						this.particleSystem.startColor = new UnityEngine.Color ((50.0f + sinData / (max_shot - min_shot) * 20.0f) / 255.0f, (50.0f + sinData / (max_shot - min_shot) * 90.0f) / 255.0f, 1.0f);
			
						//for audio
						if (sinData > threshold && !this.audio.isPlaying) {
								this.audio.Play ();
						}
				}
		}
}