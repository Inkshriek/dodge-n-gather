using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoJePlayerController : MonoBehaviour {

    public NoJeGameController gameController;
    public GameObject pickupEffect;
    public GameObject explosionEffect;
    public float speed;

    private Rigidbody2D rb2d;
    private SpriteRenderer sprite;
    private PolygonCollider2D col;
    private AudioSource aud;
    private bool dead;

    [HideInInspector] public int score;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<PolygonCollider2D>();
        aud = GetComponent<AudioSource>();
        score = 0;
        dead = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!dead && gameController.gameRunning) {
            float inputHorizontal = Input.GetAxis("Horizontal");
            float inputVertical = Input.GetAxis("Vertical");

            float inputOverall = Mathf.Min(Mathf.Max(inputVertical - inputHorizontal, -1), 1);

            float movement = inputOverall * speed;
            float tilt = (inputOverall * 15) - 90;

            rb2d.position += new Vector2(0, movement) * Time.deltaTime;
            rb2d.transform.rotation = Quaternion.Euler(0f, 0f, tilt);
        }
        
        if (!dead && gameController.gameFinished) {
            rb2d.velocity += new Vector2(5, 0) * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp")) {
            Destroy(other.gameObject);
            GameObject.Instantiate(pickupEffect, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -3), Quaternion.identity, gameObject.transform);

            score += 1;
            GameLoader.AddScore(1);
        }
        else if (other.gameObject.CompareTag("Wall")) {
            GameObject.Instantiate(explosionEffect, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -3), Quaternion.identity);
            KillPlayer();
            Debug.Log("Rest in pieces!");
        }
    }

    void KillPlayer() {
        dead = true;
        rb2d.bodyType = RigidbodyType2D.Static;
        sprite.enabled = false;
        col.enabled = false;
        StartCoroutine(gameController.EndGame(false));
    }
}
