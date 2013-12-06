using UnityEngine;
using System.Collections.Generic;

public class TileVisibility : MonoBehaviour 
{
	public delegate void VisibilityChangedEventHandler (object sender);
	public event VisibilityChangedEventHandler VisibilityChangedEvent;
	
	public Material HiddenMat;
	public Material VisibleMat;
	
	private bool isVisible;
	public bool IsVisible { get { return isVisible; } }
	
	private HashSet<GameObject> visibilitySources;
	private int visibleLayer; 
	private int hiddenLayer; 
	
	void Start () 
	{
		Debug.Log ("START");
		visibilitySources = new HashSet<GameObject> ();
		visibleLayer = LayerMask.NameToLayer ("Visible");
		hiddenLayer = LayerMask.NameToLayer ("Hidden");
		renderer.material = HiddenMat;
	}
	
	public void OnVisibilityChanged ()
	{
		if (VisibilityChangedEvent != null)
			VisibilityChangedEvent (this);
	}
	
	public void Reveal (GameObject revealer)
	{
		gameObject.layer = visibleLayer;
		renderer.material = VisibleMat;
		visibilitySources.Add (revealer);
		isVisible = true;
		OnVisibilityChanged ();
	}
	
	public void Hide (GameObject hider)
	{
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
