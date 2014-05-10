using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class ScoreBoard : MonoBehaviour {

    private string name;


	// Use this for initialization
	void Start () {
        name = PlayerPrefs.GetString("CurrentPlayer");
        PlayerPrefs.SetInt(name, 0);
	}
	
    public void updateScores(int scored)
    {
        PlayerPrefs.SetInt(name, scored);
        PlayerPrefs.Save();
        StreamWriter file2 = new StreamWriter("Scoreboard.txt", true);
        file2.WriteLine(name + ": " + scored);
        file2.Close();
    }

	// Update is called once per frame
	void Update () {
	
	}
}
