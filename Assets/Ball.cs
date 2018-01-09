using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public GameObject playerObj;
    public BoxCollider playerCol;
    public BoxCollider ballCol;
    public BoxHandler BoxHandler;
    public GameHandler gamehandler;

    float baseSpeed;
    float speedIncrements;
    float currMaxSpeed;

    public float speedY = 0.0f;
    public float speedX = 0.0f;

    public float reverseXTimer = 0.0f;
    public float reverseYTimer = 0.0f;

    public int hp = 1;

    public int _damage;

    public bool _selfReplicating = false;
    public float _selfReplicationCDMax;
    public float _selfReplicationCDCurrent;
    public float _selfDestructCDMax;
    public float _selfDestructCDCurrent;

    public float _scoreMultiplier = 1.0f;

    // Use this for initialization
    void Start () {
        baseSpeed = 0.05f;
        speedIncrements = 0.005f;
        currMaxSpeed = baseSpeed + (speedIncrements * gamehandler.level * 2);

        //gamehandler = FindObjectOfType<GameHandler>();
        //BoxHandler = FindObjectOfType<BoxHandler>();
        //ballCol = this.GetComponent<BoxCollider>();
        //playerObj = GameObject.FindGameObjectWithTag("Player");
        //playerCol = playerObj.GetComponent<BoxCollider>();
	}

    private void Awake()
    {
        gamehandler = FindObjectOfType<GameHandler>();
        BoxHandler = FindObjectOfType<BoxHandler>();
        ballCol = this.GetComponent<BoxCollider>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerCol = playerObj.GetComponent<BoxCollider>();

        _selfReplicationCDMax = 5.0f;
        _selfReplicationCDCurrent = 0.0f;

        _selfDestructCDMax = 15.0f;
        _selfDestructCDCurrent = 0.0f;
    }

    // Update is called once per frame
    void Update () {
        
        if (gamehandler.gamestate != GameHandler.GameState.Playing)
            return;

        if(transform.position.y < -7)
        {
            gamehandler.DestroyBall(this.gameObject);
        }

        if (_selfReplicating)
        {
            _selfReplicationCDCurrent += Time.deltaTime;
            if(_selfReplicationCDCurrent >= _selfReplicationCDMax)
            {
                _selfReplicationCDCurrent = 0;
                gamehandler.SpawnBall(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z));
            }

            _selfDestructCDCurrent += Time.deltaTime;
            if(_selfDestructCDCurrent >= _selfDestructCDMax)
            {
                _selfDestructCDCurrent = 0;
                gamehandler.DestroyBall(this.gameObject);
            }
        }

        transform.Translate(speedX, speedY, 0.0f);

        if(speedY <= 0)
            speedY = -currMaxSpeed;

        float maxSpeed = currMaxSpeed;
        
        if (reverseXTimer > 0)
            reverseXTimer -= Time.deltaTime;
        if (reverseYTimer > 0)
            reverseYTimer -= Time.deltaTime;

        if (ballCol.bounds.Intersects(playerCol.bounds))
        {
            //Debug.Log("Ball collided with Player");
            speedY = 0.5f;

            Vector3 intersectPoint = ballCol.ClosestPointOnBounds(playerCol.transform.position);

            //Debug.Log("Intersect point: " + intersectPoint);

            float start = playerObj.transform.position.x - playerCol.size.x / 2;
            float end = playerObj.transform.position.x + playerCol.size.x / 2;

            //Debug.Log("Orig Start: " + start + " Orig End: " + end + " Size: " + playerCol.size.x);

            //Now to make start become a 0 value so we can count using %
            //and make the new hit pos
            float hitPos = intersectPoint.x;// + playerCol.size.x / 2;
            end -= start;
            hitPos -= start;
            start = 0;
            

            //Debug.Log("Start: " + start + " End: " + end);

            float percentage = hitPos / end;
            //Debug.Log("HitPos: " + hitPos + " Percentage: " + percentage * 100);

            //speedX = 0;
            //speedX = -0.5f * percentage;

            float speed = 0.0f;
            if (hitPos < 5.01f && hitPos > 4.99f)
            {
                speedX = 0.0f;
            }
            else if (hitPos < 5.0f)
            {
                speed = (-1 * hitPos) - 4.0f;
                speedX = maxSpeed * speed / 2;
            }
            else
            {
                speed = hitPos - 4.0f;
                speedX = maxSpeed * speed / 2;
            }

            //Debug.Log("Speed: " + speed + " speedX: " + speedX);
            
        }

        if (this.transform.position.x < 0.4f || this.transform.position.x > 15.0f)
        {
            if(transform.position.x > 15.0f)
            {
                transform.position = new Vector3(14.5f, transform.position.y, transform.position.z);
            }
            if (reverseXTimer <= 0)
            {
                speedX *= -1;
                reverseXTimer = 0.2f;
            }
        }
        if(this.transform.position.y >= 5.0f)
        {
            if (reverseYTimer <= 0)
            {
                speedY *= -1;
                reverseYTimer = 0.2f;
            }
        }

        List<GameObject> box = BoxHandler.GetBoxes();
        for (int i = 0; i < box.Count; i++)
        {
            Bounds bounds = box[i].GetComponent<BoxCollider>().bounds;
            if (box[i].activeSelf)
            {
                if (ballCol.bounds.Intersects(bounds))
                {

                    //box[i].SetActive(false);
                    bool damageBall = false;
                    if (box[i].GetComponent<Box>().Hit(_damage, out damageBall))
                    {
                        gamehandler.IncreaseScore(_scoreMultiplier);

                        if (damageBall)
                        {
                            hp--;
                            if(hp <= 0)
                            {
                                gamehandler.DestroyBall(this.gameObject);
                            }
                        }
                    }
                    else
                    {
                        if (reverseXTimer <= 0)
                        {
                            speedX *= -1;
                            reverseXTimer = 0.2f;
                        }
                    }

                    //BoxHandler.lowerNrOfBoxes();

                    if (reverseYTimer <= 0)
                    {
                        speedY *= -1;
                        reverseYTimer = 0.2f;
                    }
                }

            }
        }

        if (speedY < -maxSpeed)
            speedY = -maxSpeed;
        if (speedY > maxSpeed)
            speedY = maxSpeed;
        if (speedX < -maxSpeed)
            speedX = -maxSpeed;
        if (speedX > maxSpeed)
            speedX = maxSpeed;
    }

    public void Reset(bool increaseDifficulty)
    {
        if (increaseDifficulty)
        {
            currMaxSpeed = baseSpeed + speedIncrements * gamehandler.level;
        }
        else
        {
            currMaxSpeed = baseSpeed;
        }

        transform.position = new Vector3(8.5f, -3.0f, 0.0f);
        speedX = 0.0f;
        speedY = 0.0f;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public bool IsBeingShown()
    {
        return this.gameObject.activeSelf;
    }
}
