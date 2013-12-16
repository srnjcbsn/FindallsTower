﻿using UnityEngine;
using System.Collections;

public class ItemMat : MonoBehaviour
{
	public Material[] mats;
	public int interval = 1000;
	private int iter = 0;
	MeshRenderer planeRenderer;
	double time;
	double timeOld;
	float alpha = 0f;

	void Start ()
	{
		planeRenderer = GetComponentInChildren<MeshRenderer> ();
		planeRenderer.material = mats [iter];
		timeOld = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
		ApplyAlphaChannel ();
	}
	// Update is called once per frame
	void Update ()
	{
		//Change material at a given interval
		time = System.DateTime.Now.TimeOfDay.TotalMilliseconds;
		if (time >= timeOld + interval)
		{
			timeOld = time;
//			planeRenderer.material = mats [iter++];

			renderer.material = mats [iter++];
			if (iter == mats.Length)
				iter = 0;

			ApplyAlphaChannel ();
		}
	}

	public void TileVisibilityChanged (object sender, Transform tileTransform)
	{
		if (tileTransform.GetComponent<TileVisibility> ().IsVisible)
			alpha = 1f;
		else
			alpha = 0f;

		ApplyAlphaChannel ();
	}

	private void ApplyAlphaChannel ()
	{
		Color thisColor = renderer.material.color;
		renderer.material.color = new Color (thisColor.r, thisColor.g, thisColor.b, alpha);
	}
}
