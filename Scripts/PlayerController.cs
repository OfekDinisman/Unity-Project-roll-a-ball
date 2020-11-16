using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //public vars
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public float jumpHeight=10;


    //private vars
    private int MAX_PICKUP = 12;
    private int score_cnt;
    private string pickUpTag = "PickUp";
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        score_cnt = 0;
        SetCountText();
        winTextObject.gameObject.SetActive(false);//hide win message
    }

    void SetCountText()
    {
        countText.text = "Count: "+score_cnt;
        if (score_cnt >= MAX_PICKUP)
        {
            winTextObject.gameObject.SetActive(true);//show win message
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMove(InputValue movementValue)
    {
        if (movementValue == null)
        {
            throw new System.ArgumentNullException(nameof(movementValue));
        }
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;

    }
 
    void FixedUpdate()
    {   //make the cubes animation
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement*speed);
        
        
    }

    private void OnTriggerEnter(Collider other)
    {//hide PickUP when bump into
        if (other.gameObject.CompareTag(pickUpTag))
        {
            other.gameObject.SetActive(false);//hide object
            score_cnt++;
            SetCountText();
        }
    }
}
