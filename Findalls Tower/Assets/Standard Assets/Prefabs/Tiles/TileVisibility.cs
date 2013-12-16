using UnityEngine;
using System.Collections.Generic;

public class TileVisibility : MonoBehaviour 
{
	public delegate void VisibilityChangedEventHandler (object sender, Transform tileTransform);
	public event VisibilityChangedEventHandler VisibilityChangedEvent;
	
	public Material HiddenMat;
	public Material VisibleMat;
	
	private bool isVisible;
	public bool IsVisible { get { return isVisible; } }
	public bool IsDiscovered { get; private set; }
	
	private HashSet<GameObject> visibilitySources;
	private int visibleLayer; 
	private int hiddenLayer; 
	
	void Awake () 
	{
		IsDiscovered = false;
		visibilitySources = new HashSet<GameObject> ();
		visibleLayer = LayerMask.NameToLayer ("Visible");
		hiddenLayer = LayerMask.NameToLayer ("Hidden");
		renderer.material = HiddenMat;
	}
	
	public void OnVisibilityChanged ()
	{
		if (VisibilityChangedEvent != null)
			VisibilityChangedEvent (this, transform);
	}
	
	public void Reveal (GameObject revealer)
	{
		IsDiscovered = true;
		gameObject.layer = visibleLayer;
		renderer.material = VisibleMat;
		
		visibilitySources.Add (revealer);
		isVisible = true;
		OnVisibilityChanged ();
	}
	
	public void Hide (GameObject hider)
	{
		Debug.Log ("HIDE");
		visibilitySources.Remove (hider);
		
		if (visibilitySources.Count == 0)
		{
			isVisible = false;
			OnVisibilityChanged ();
			gameObject.layer = hiddenLayer;
//			renderer.material = HiddenMat;
		}
	}
	
	
}
