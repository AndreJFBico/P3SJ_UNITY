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

    }

    public void pickupKey()
    {
        hasKey = true;
    }

    public IEnumerator decreaseScoreTimely()
    {
        while (true)
        {
            decreaseScore(1);
            yield return new WaitForSeconds(1);
        }        
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
