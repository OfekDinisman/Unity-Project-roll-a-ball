using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    //public vars
    public static bool gameIsPaused;
    public static bool gameIsGameOver;
    public static bool gameIsRestart=false;
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject PauseTextObject;
    public GameObject GameOverTextObject;
    public GameObject gv;//global volume effects


    //private vars
    private bool isJumpPressed;
    private float jumpHeight = 300.0f;
    //objects
    private ParticleSystem m_particleSystem;
    //private int MAX_PICKUP = 12;
    private int score_cnt;
    private string pickUpTag = "PickUp";
    private string pickUpSpecialTag = "PickUpSpecial";
    private string InnerWallTag = "InnerWall";
    private Rigidbody rb;
    private float movementX; 
    private float movementY;
    System.Random randomDirection;
    //borders
    private float MIN_X=-8.0f;
    private float MIN_Y=-9.0f;
    private float MAX_X=9.0f;
    private float MAX_Y=9.0f;
    //color effect
    ColorAdjustments colorAdjustments;


    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        score_cnt = 0;
        SetCountText();
        PauseTextObject.gameObject.SetActive(false);//hide pause message
        GameOverTextObject.gameObject.SetActive(false);//hide game over message
        randomDirection = new System.Random();
        //retart button
        GameObject button = GameObject.Find("RestartBtn");
        Button b = button.GetComponent<Button>();
        b.onClick.AddListener(RestartGame);
        //
        //exit button
        GameObject exit_obj = GameObject.Find("ExitBtn");
        Button exit_btn = exit_obj.GetComponent<Button>();
        exit_btn.onClick.AddListener(QuitGame);
        //
        //color effect (off by default)
        gv = GameObject.Find("GlobalVolume");
        gv.gameObject.SetActive(false);
        //particleSystem
        m_particleSystem = GetComponent<ParticleSystem>();
        //show partical effect when picking up the obj
        ParticleSystem.ShapeModule _editableShape = m_particleSystem.shape;
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = new Vector3(7.18f, 0.5f, 0.0f);
        emitParams.velocity = new Vector3(0.0f, 0.0f, -2.0f);
        m_particleSystem.Emit(emitParams, 1);
    }
    private void QuitGame()
    {
        Debug.Log("Quit");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    public void RestartGame()
    {
        Debug.Log("restart");
        gameIsRestart = true;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
       
    }
    private void RestartUIUpdate()
    {
        if (gameIsRestart)
        {
            PauseTextObject.gameObject.SetActive(false);//hide pause message
            GameOverTextObject.gameObject.SetActive(false);//hide game over message
            gameIsRestart = false;
        }

    }

    private bool IsGameOver()
    {
        Vector3 movement = new Vector3();
        movement = this.gameObject.transform.position;
       // Debug.Log(movement.x);
      //  Debug.Log(movement.y);
        if (movement.x < MIN_X -2 || movement.x > MAX_X +2 )
            return true;
        if (movement.z < MIN_Y -2 || movement.z > MAX_Y +2)
            return true;
        if (movement.y < -0.5)
            return true;
        return false;
    }
    private void GameOver()
    {
        if (gameIsGameOver)
        {//end game logic
            GameOverTextObject.gameObject.SetActive(true);//show game over message
        }
    }
    void SetCountText()
    {
        countText.text = "Count: "+score_cnt;
        //if (score_cnt >= MAX_PICKUP)
        //{
        //    winTextObject.gameObject.SetActive(true);//show win message
        //}
    }


    void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;// pause the game
            PauseTextObject.gameObject.SetActive(true);//show pause message

        }
        else
        {
            Time.timeScale = 1;//continue
            PauseTextObject.gameObject.SetActive(false);//hide pause message
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isJumpPressed = true;
        }
        GameOver();
        

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
        //clicking space make the ball jump
        if (isJumpPressed)
        {
            //make ball jump
            Vector3 JumpMovement = new Vector3(movementX, jumpHeight, movementY);
            rb.AddForce(JumpMovement);

            isJumpPressed = false;
        }
        RestartUIUpdate();
        gameIsGameOver = IsGameOver();//save state of game

    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("OnCollisionEnter");
        if (other.gameObject.CompareTag(InnerWallTag))
        {
            Vector3 PullMovement = new Vector3(movementX- jumpHeight, jumpHeight, movementY);
            //Debug.Log("x:" + movementX);
            //Debug.Log("y:" + movementY);
            rb.AddForce(PullMovement);

        }
    }
    private void OnTriggerEnter(Collider other)
    {//hide PickUP when bump into
        if (other.gameObject.CompareTag(pickUpTag) || other.gameObject.CompareTag(pickUpSpecialTag))
        {
            //pickup change location
            other.gameObject.SetActive(false);//hide PickUP
            score_cnt++;
            SetCountText();

            //change pickup position
            float x = (float)(randomDirection.NextDouble() * MIN_X + randomDirection.NextDouble() * MAX_X);
            float y = (float)(randomDirection.NextDouble() * MIN_Y + randomDirection.NextDouble() * MAX_Y);


            Vector3 newLocation = new Vector3(x, 0.5f, y);
            other.gameObject.transform.position = newLocation;
            other.gameObject.SetActive(true);//show PickUP
            gv.gameObject.SetActive(false);

            //show partical effect when picking up the obj
            other.gameObject.SetActiveRecursively(true);


        }
        if (other.gameObject.CompareTag(pickUpSpecialTag))
        {
            Debug.Log("pickUpSpecialTag");
            gv.gameObject.SetActive(true);
        }
    }

}
