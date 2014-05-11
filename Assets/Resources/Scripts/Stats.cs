using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {

    private int lives = 3;
    private int score = 1000;
    private string playerName = "";
    private bool hasKey = false;

    public Texture2D tex;

    public void OnGUI()
    {
        GUI.Box(new Rect(Screen.width - 120, 10, 100, 20), "Score: " + score);
        for (int i = 0; i < lives; i++ )
            GUI.Label(new Rect(10 + 20*i, 10, 20, 20), tex);
    }

    public void endGame()
    {
        transform.parent.gameObject.GetComponent<ScoreBoard>().updateScores(score);
        Destroy(transform.parent.gameObject);
        Application.LoadLevel("LeaderBoard");
    }

    public void pickupKey()
    {
        hasKey = true;
    }

    public bool haveKey()
    {
        return hasKey;
    }

    public IEnumerator decreaseScoreTimely()
    {
        while (true)
        {
            decreaseScore(1);
            yield return new WaitForSeconds(1);
        }        
    }

    public void fellOff()
    {
        decreaseHealth();
        transform.position = GameObject.FindWithTag("StartLevel").GetComponent<Transform>().position;
    }

    public void decreaseScore(int amount)
    {
        score -= amount;
    }

    public void decreaseHealth()
    {
        if (lives == 0)
            endGame();
        lives -= 1;
        decreaseScore(100);
    }

	// Use this for initialization
	void Start () {
        StartCoroutine("decreaseScoreTimely");
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
