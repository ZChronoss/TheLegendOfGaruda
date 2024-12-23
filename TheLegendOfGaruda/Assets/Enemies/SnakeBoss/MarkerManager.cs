using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MarkerManager : MonoBehaviour
{
    public class Marker{
        public Vector3 pos;
        public Quaternion rot;

        public Marker(Vector3 position, Quaternion rotation){
            pos = position;
            rot = rotation;
        }
    }
    public bool recording = true;

    public List<Marker> markerList = new List<Marker>();

    void FixedUpdate(){
        if(recording){
            updateMarkerList();
        }
    }

    public void updateMarkerList(){
        if (!transform.GetComponent<Rigidbody2D>()){
            transform.AddComponent<Rigidbody2D>();
            transform.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        markerList.Add(new Marker(transform.position, transform.rotation));
    }
    // Update is called once per frame
    public void clearMarkerList(){
        markerList.Clear();
        markerList.Add(new Marker(transform.position, transform.rotation));
    }
}
