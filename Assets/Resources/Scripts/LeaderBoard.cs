using UnityEngine;
using System.Collections;

public class LeaderBoard : MonoBehaviour {

    private string[] lines;
	// Use this for initialization
	void Start () {
        lines = System.IO.File.ReadAllLines("Scoreboard.txt");
	}
	
    public void OnGUI()
    {
        int i = 0;
        foreach (string s in lines)
        {
            GUI.Box(new Rect(Screen.width/2 - 40, Screen.height/2 + i*40 - 100, 200, 40), s);
            i++;
        }

        if (GUI.Button(new Rect(10, 10, 200, 50), new GUIContent("Restart", "Click to restart")))
        {
            PlayerPrefs.Save();
            Application.LoadLevel("Scene");
        }

        if (GUI.Button(new Rect(10, 70, 200, 50), new GUIContent("New Player", "Click to restart")))
        {
            PlayerPrefs.Save();
            Application.LoadLevel("StartMenu");
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
