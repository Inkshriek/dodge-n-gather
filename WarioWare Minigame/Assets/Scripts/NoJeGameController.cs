using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

//This and the player controller are probably THE most messy programming I've ever done but it ~functions~ I guess, lol.
//If these scripts got reused over a maybe bigger version of this minigame with multiple levels or somethin', I'll clean it up and make it so it isn't literal spaghetti.

public class NoJeGameController : MonoBehaviour {

    public NoJePlayerController player;
    public GameObject pickup;

    public int pickupCount;
    public float pickupSpacing;

    public SpriteShapeController upperWall;
    public SpriteShapeController lowerWall;

    public float minimumWallSpacing;
    public float startPoint;
    public float vertexSpacing;
    public int vertexCount;
    public float vertexRange;
    public float scrollingSpeed;
    public float timeLimit;

    public Text startText;
    public Text winText;
    public Text failText;
    public Text scoreText;

    private Spline upperSpline;
    private Spline lowerSpline;
    private float timeLeft;

    [HideInInspector] public bool gameRunning = false;
    [HideInInspector] public bool gameFinished = false;

    void Start() {
        //-----Begin generating the walls 
        upperSpline = upperWall.spline;
        lowerSpline = lowerWall.spline;
        timeLeft = timeLimit;

        for (int i = 1; i <= vertexCount; i++) {
            float vertexH = startPoint + (vertexSpacing * i);
            float vertexV1;
            float vertexV2;
            float spacing;

            int tries = 0;

            do {
                vertexV1 = Random.Range(vertexRange, 0f);
                vertexV2 = Random.Range(vertexRange, 0f);

                float vertex1Ref = upperWall.transform.position.y - vertexV1;
                float vertex2Ref = lowerWall.transform.position.y + vertexV2;

                spacing = vertex1Ref - vertex2Ref;

                tries += 1;
            }
            while (spacing < minimumWallSpacing && tries < 1000);


            Vector2 vertex1 = new Vector2(vertexH, -vertexV1);
            Vector2 vertex2 = new Vector2(vertexH, -vertexV2);
            upperSpline.InsertPointAt(0, vertex1);
            lowerSpline.InsertPointAt(0, vertex2);
        }

        upperSpline.InsertPointAt(0, new Vector2(vertexCount * vertexSpacing + startPoint, 2));
        lowerSpline.InsertPointAt(0, new Vector2(vertexCount * vertexSpacing + startPoint, 2));
        lowerWall.transform.localScale = new Vector3(lowerWall.transform.localScale.x, -Mathf.Abs(lowerWall.transform.localScale.y), lowerWall.transform.localScale.z);

        
        upperWall.BakeCollider();
        lowerWall.BakeCollider();

        //-----Begin generating the pickup locations
        for (int i = 1; i <= pickupCount; i++) {

            Vector2 pos;
            RaycastHit2D overlap;
            int tries = 0;
            do {
                pos = new Vector2(player.transform.position.x + (pickupSpacing * i) + startPoint, Random.Range(upperWall.transform.position.y, lowerWall.transform.position.y));
                overlap = Physics2D.BoxCast(pos, pickup.transform.localScale, 0f, Vector2.zero);
                tries += 1;
            }
            while (overlap && tries < 1000);

            GameObject.Instantiate(pickup, pos, Quaternion.identity, gameObject.transform);
        }

        StartCoroutine(StartGame());
    }

    void Update () {
        if (gameRunning) {
            timeLeft -= Time.deltaTime;
            gameObject.transform.position += new Vector3(-scrollingSpeed * Time.deltaTime, 0, 0);
        }

        if (timeLeft <= 0 && !gameFinished) {
            StartCoroutine(EndGame(true));
        }
    }

    public IEnumerator StartGame() {


        float oldSize = startText.fontSize;
        float targetSize = startText.fontSize / 2;
        startText.color = new Color(startText.color.r, startText.color.g, startText.color.b, 0f);
        while (startText.fontSize > targetSize) {
            startText.color = new Color(startText.color.r, startText.color.g, startText.color.b, (oldSize - startText.fontSize) / targetSize);
            startText.fontSize -= 1;
            yield return new WaitForSeconds(.01f);
        }
        gameRunning = true;
        yield return new WaitForSeconds(1f);
        while (startText.fontSize < oldSize) {
            startText.color = new Color(startText.color.r, startText.color.g, startText.color.b, (oldSize - startText.fontSize) / targetSize);
            startText.fontSize += 1;
            yield return new WaitForSeconds(.01f);
        }
        startText.color = new Color(startText.color.r, startText.color.g, startText.color.b, 0f);

    }

    public IEnumerator EndGame(bool win) {
        gameFinished = true;

        if (win) {
            float oldSize = winText.fontSize;
            float targetSize = winText.fontSize / 2;
            winText.color = new Color(winText.color.r, winText.color.g, winText.color.b, 0f);
            while (winText.fontSize > targetSize) {
                winText.color = new Color(winText.color.r, winText.color.g, winText.color.b, (oldSize - winText.fontSize) / targetSize);
                winText.fontSize -= 1;
                yield return new WaitForSeconds(.01f);
            }
            yield return new WaitForSeconds(1f);


            float oldScoreSize = scoreText.fontSize;
            float targetScoreSize = scoreText.fontSize / 2;
            scoreText.text = "[ Score: " + player.score + " / " + pickupCount + " ]";

            if (player.score == pickupCount) {
                scoreText.color = new Color(0.9f, 1f, 0.04f, 0f);
            }
            else {
                scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 0f);
            }

            while (scoreText.fontSize > targetScoreSize) {
                scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, (oldScoreSize - scoreText.fontSize) / targetScoreSize);
                scoreText.fontSize -= 1;
                yield return new WaitForSeconds(.01f);
            }

            while (!Input.GetButton("Submit"))
                yield return null;


            while (winText.fontSize < oldSize) {
                winText.color = new Color(winText.color.r, winText.color.g, winText.color.b, (oldSize - winText.fontSize) / targetSize);
                winText.fontSize += 1;
                yield return new WaitForSeconds(.01f);
            }
            winText.color = new Color(failText.color.r, failText.color.g, failText.color.b, 0f);
            while (scoreText.fontSize < oldScoreSize) {
                scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, (oldScoreSize - scoreText.fontSize) / targetScoreSize);
                scoreText.fontSize += 1;
                yield return new WaitForSeconds(.01f);
            }
            scoreText.color = new Color(failText.color.r, failText.color.g, failText.color.b, 0f);
        }

        if (!win) {
            float oldSize = failText.fontSize;
            float targetSize = failText.fontSize / 2;
            failText.color = new Color(failText.color.r, failText.color.g, failText.color.b, 0f);
            while (failText.fontSize > targetSize) {
                failText.color = new Color(failText.color.r, failText.color.g, failText.color.b, (oldSize - failText.fontSize) / targetSize);
                failText.fontSize -= 1;
                yield return new WaitForSeconds(.01f);
            }
            yield return new WaitForSeconds(1f);


            float oldScoreSize = scoreText.fontSize;
            float targetScoreSize = scoreText.fontSize / 2;
            scoreText.text = "[ Score: " + player.score + " / " + pickupCount + " ]";

            if (player.score == pickupCount) {
                scoreText.color = new Color(0.9f, 1f, 0.04f, 0f);
            }
            else {
                scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, 0f);
            }

            while (scoreText.fontSize > targetScoreSize) {
                scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, (oldScoreSize - scoreText.fontSize) / targetScoreSize);
                scoreText.fontSize -= 1;
                yield return new WaitForSeconds(.01f);
            }

            while (!Input.GetButton("Submit"))
                yield return null;


            while (failText.fontSize < oldSize) {
                failText.color = new Color(failText.color.r, failText.color.g, failText.color.b, (oldSize - failText.fontSize) / targetSize);
                failText.fontSize += 1;
                yield return new WaitForSeconds(.01f);
            }
            failText.color = new Color(failText.color.r, failText.color.g, failText.color.b, 0f);
            while (scoreText.fontSize < oldScoreSize) {
                scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, (oldScoreSize - scoreText.fontSize) / targetScoreSize);
                scoreText.fontSize += 1;
                yield return new WaitForSeconds(.01f);
            }
            scoreText.color = new Color(failText.color.r, failText.color.g, failText.color.b, 0f);
        }


        GameLoader.gameOn = false;
    }
}
