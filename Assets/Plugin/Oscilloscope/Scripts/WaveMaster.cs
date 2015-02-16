﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaveMaster : MonoBehaviour
{
		private UtilDraw2D m_draw2D;
		private IConnect2Devices devices;

		private int max_width;
		private int max_height;
		private int interval_width = 3;
		private LinkedList<float> wave_point_x;

		private float before_data;

		private Wave[] m_wave;
		private const int WAVE_NUM = 2;

		private int separateNum;
		private Vector3[] separateLine;

		void Start ()
		{
				devices = GameObject.FindGameObjectWithTag ("Arduino").GetComponent<Connect2Arduino> ();
				//devices.SetPort ("COM3", 115200, 1);
				devices.SetPort ("/dev/tty.usbmodem1411", 115200, 1);
				//("/dev/tty.usbmodem1451", 115200);
				//("COM3", 115200);

				m_draw2D = new UtilDraw2D ();
				wave_point_x = new LinkedList<float> ();
				m_wave = new Wave[WAVE_NUM];

				max_width = (int)Screen.width / interval_width;
				for (int i = 0; i < max_width; i++) {
						wave_point_x.AddLast (i * interval_width);
				}

				max_height = (int)Screen.height;
				float interval_height = max_height / (float)WAVE_NUM;
				for (int i = 0; i < WAVE_NUM; i++) {
						m_wave [i] = new Wave (interval_height, i, max_width);
				}

				// 線の数 * 先端終端
				separateNum = WAVE_NUM - 1;
				separateLine = new Vector3[separateNum * 2];
				for (int i = 0; i < separateNum; i++) {
						float height = max_height / (float)WAVE_NUM * (i + 1);
						separateLine [i] = new Vector3 (max_width * interval_width, height, 0.0f);
						separateLine [i + separateNum] = new Vector3 (0, height, 0.0f);
				}

		}

		void Update ()
		{
				float[] data = devices.GetVoltages ();
				m_wave [0].addData (data [0]);
				float tmp = (data [0] * 0.1f + before_data * 0.9f);
				m_wave [1].addData (tmp);
				before_data = tmp;
		}

		private Vector3[] posA;

		void OnPostRender ()
		{
				// 描画開始
				m_draw2D.Begin ();
				{
						// 区切り用の線を描画
						Vector3[] Line = new Vector3[2];
						for (int i = 0; i < separateNum; i++) {
								Line [0] = separateLine [i];
								Line [1] = separateLine [i + separateNum];
								m_draw2D.DrawLines (Line, new Color (0.0f, 0.0f, 0.0f));
						}

		
						for (int i = 0; i < WAVE_NUM; i++) {
								posA = new Vector3[m_wave [i].wave_point.Count];

								for (int j = 0; j < m_wave [i].wave_point.Count; j += 1) {
										posA [j] = new Vector3 (wave_point_x.ElementAt (j), m_wave [i].wave_point.ElementAt (j), 0.0f);
								}
								if (posA.Count () >= 2)
										m_draw2D.DrawLines (posA, new Color (0.0f, 0.0f, 0.0f));
						}
				}
				// 描画終了
				m_draw2D.End ();
		}

		public void fitButton ()
		{
				for (int i = 0; i < m_wave.Length; i++) {
						m_wave [i].fitData ();
				}
		}
				
}
