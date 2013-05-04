using UnityEngine;
using System.Collections.Generic;

public class EdgeDetector : MonoBehaviour {
    List<Collider> colliders = new List<Collider>();

	// Use this for initialization
	void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        colliders.Add(other);
    }

    void OnTriggerExit( Collider other )
    {
        bool wasEmpty = (colliders.Count == 0);
        colliders.Remove(other);
        if(!wasEmpty && colliders.Count == 0)
            gameObject.SendMessageUpwards("onWalkedOffEdge", gameObject.name);
            //transform.parent.gameObject.GetComponent <Actor>().onWalkedOffEdge(id);
    }
}
