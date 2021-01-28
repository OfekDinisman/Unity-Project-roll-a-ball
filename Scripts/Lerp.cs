using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Lerp : MonoBehaviour
{
    [SerializeField]
    private Transform start;
    [SerializeField]
    private Transform end ;

    [SerializeField]
    [Range(0f,1f)]
    private float LerpPct = 0.5f;

    public float finalValue;

    public float DurationInSeconds = 4;
    float percentage = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

          percentage += Time.deltaTime; // add deltatime to percentage every update
        //if (transform.position.x >= end.position.x)
        //{
        //    Transform temp = end;
        //    end = start;
        //    start = end;
        //}
         transform.position = Vector3.Lerp(start.position, end.position, percentage);
        //transform.position = Vector3.Lerp(start.position, end.position, percentage);


        // transform.rotation = Quaternion.Lerp(start.rotation, end.rotation, percentage);
    }
}
