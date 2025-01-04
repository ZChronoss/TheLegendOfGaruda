using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnakeManager : MonoBehaviour, IStunnable
{
    public float speed = 5f;
    public bool ableToMove = true;
    public float maxHealth = 200f;
    public float health;
    private bool fleeingState = false;
    public bool chasingState = false;
    [SerializeField] float rotateSpeed = 200f; 
    [SerializeField] float distanceBetween = 2f;
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    [SerializeField] List<Sprite> bodySprites;
    private float speedThreshold1 = 30f; // Speed threshold for the first sprite change
    private float speedThreshold2 = 60f; // Speed threshold for the second sprite change
    List<GameObject> snakeBody = new List<GameObject>();
    public List<GameObject> target = new List<GameObject>();
    private Rigidbody2D rb;
    private float countUp = 0;
    [SerializeField] List<GameObject> bodySegments = new List<GameObject>(new GameObject[3]);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        target.Add(GameObject.FindGameObjectWithTag("Player"));
        createBodyParts();
        rb = snakeBody[0].GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
    }

    void Update(){
        UpdateBodyPartSprites();
    }

    // Update is called once per frame
    private void FixedUpdate(){
        ManageSnakeBody();
        if(fleeingState){
            movementHandler(false);
        }else if(ableToMove && target.Count != 0){
            movementHandler(true);
        }else{
            rb.angularVelocity = 0;
            
            rb.linearVelocity = Vector2.zero;

            if(snakeBody.Count > 1){
                for(int i=1; i<snakeBody.Count; i++){
                    MarkerManager markM = snakeBody[i-1].GetComponent<MarkerManager>();
                    markM.recording = false;
                }
            }
        }
    }

    private void movementHandler(bool towards){
        Vector2 direction = new Vector2();

        if (chasingState){
            direction = (snakeBody[0].transform.position - target[target.Count-1].transform.position).normalized;
        }else{
            direction = (snakeBody[0].transform.position - target[0].transform.position).normalized;
        }

        if (!towards)
        {
            direction = -direction;
        }

        float value = Vector3.Cross(direction, snakeBody[0].transform.right).z;

        // Add a bias or a corrective factor
        float correctiveFactor = 0.1f; // Adjust this value as needed
        if (Mathf.Abs(value) < correctiveFactor)
        {
            value = Mathf.Sign(value) * correctiveFactor;
        }

        rb.angularVelocity = rotateSpeed * value;
        
        rb.linearVelocity = snakeBody[0].transform.right * speed;

        if(snakeBody.Count > 1){
            for(int i=1; i<snakeBody.Count; i++){
                MarkerManager markM = snakeBody[i - 1].GetComponent<MarkerManager>();
                markM.recording = true;
                snakeBody[i].transform.position = markM.markerList[0].pos;
                snakeBody[i].transform.rotation = markM.markerList[0].rot;
                markM.markerList.RemoveAt(0);
            }
        }
    }

    public void Stun(float duration){
        ableToMove = false;
        StartCoroutine(stopStun(duration));
    }

    IEnumerator stopStun(float duration){
        yield return new WaitForSeconds(duration);
        ableToMove = true;
        fleeingState = true;
        yield return new WaitForSeconds(duration);
        fleeingState = false;
        chasingState = false;
    }

    private void createBodyParts(){
        if(snakeBody.Count == 0){
            GameObject temp1 = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            if (!temp1.GetComponent<MarkerManager>()) temp1.AddComponent<MarkerManager>();
            if (!temp1.GetComponent<Rigidbody2D>()){
                temp1.AddComponent<Rigidbody2D>();
                temp1.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            SpriteRenderer headRenderer = temp1.GetComponent<SpriteRenderer>();
            if (headRenderer != null)
            {
                headRenderer.sortingOrder = bodyParts.Count;
            }
            snakeBody.Add(temp1);
            bodyParts.RemoveAt(0);
        }
        MarkerManager markM = snakeBody[snakeBody.Count-1].GetComponent<MarkerManager>();
        if(countUp == 0){
            markM.clearMarkerList();
        }
        countUp += Time.deltaTime;
        if(countUp >= distanceBetween){
            GameObject temp = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            if (!temp.GetComponent<MarkerManager>()) temp.AddComponent<MarkerManager>();
            if (!temp.GetComponent<Rigidbody2D>()){
                temp.AddComponent<Rigidbody2D>();
                temp.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            SpriteRenderer headRenderer = temp.GetComponent<SpriteRenderer>();
            if (headRenderer != null)
            {
                headRenderer.sortingOrder = bodyParts.Count;
            }
            snakeBody.Add(temp);
            bodyParts.RemoveAt(0);
            temp.GetComponent<MarkerManager>().clearMarkerList();
            countUp = 0;
        }
    }

    void ManageSnakeBody(){
        if(bodyParts.Count>0){
            createBodyParts();
        }
        
        for(int i = 0; i<snakeBody.Count; i++){
            if(snakeBody[i]==null){
                snakeBody.RemoveAt(i);
                i = i - 1;
            }
        }

        if(snakeBody.Count == 0) Destroy(this);
    }

    public void AddBodyParts(){
        GameObject temp = snakeBody[snakeBody.Count-1];
        snakeBody.RemoveAt(snakeBody.Count-1);
        Destroy(temp);
        bodyParts.Add(bodySegments[1]);
        bodyParts.Add(bodySegments[2]);
    }

    private void UpdateBodyPartSprites()
    {
        Sprite newSprite;

        // Determine the sprite based on the speed
        if (speed < speedThreshold1)
        {
            newSprite = bodySprites[0]; // Normal speed sprite
        }
        else if (speed < speedThreshold2)
        {
            newSprite = bodySprites[1]; // Medium speed sprite
        }
        else
        {
            newSprite = bodySprites[2]; // High speed sprite
        }

        for(int i=1; i<snakeBody.Count-1; i++){
            if (snakeBody[i] != null)
            {
                SpriteRenderer spriteRenderer = snakeBody[i].GetComponent<SpriteRenderer>();
                if (spriteRenderer != null && spriteRenderer.sprite != newSprite)
                {
                    spriteRenderer.sprite = newSprite; // Update the sprite
                }
            }
        }
    }
}
