using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class StartMenu : MonoBehaviour {

	public GUISkin _menuskin;
	public GUIStyle _title;
	public GUIStyle _nosoundbutton;
	public GUIStyle _unmutebutton;
	public GUIStyle _mutebutton;
	public GUIStyle _soundbutton;
	private Rect _creditwindow = new Rect((Screen.width*4)/5, Screen.height/10, Screen.width/5, (Screen.height*4)/5);
	private bool _showcredits = false;
	public Texture2D _mute;
	public Texture2D _unmute;
	public Texture2D _sound;
	public Texture2D _nosound;
    private string name = "Enter Name Here";
	
	void Start()
    {
        Debug.Log("Entered StartMenu Start");
        if(!File.Exists("Scoreboard.txt"))
        {
            Debug.Log("Creating File");
            File.Create("Scoreboard.txt");
        }
    }

	void OnGUI(){
		
		//The title box
		GUI.Box(new Rect(Screen.width/4, Screen.height/5, Screen.width/2, Screen.width/10), "Platformer 5000", _title);

        name = GUI.TextArea((new Rect(0, (Screen.height * 13) / 15, Screen.width / 4, Screen.height / 15)), name);
        PlayerPrefs.SetString("CurrentPlayer", name);

		//The "Game developed by" box
		GUI.Box(new Rect(0, (Screen.height*14)/15, Screen.width/4, Screen.height/15), "Game developed By: João, Sebastião, André", _menuskin.box);
		
		//The mute sound and music button
		if(GUI.Button(new Rect((Screen.width*8/10), 0, Screen.width/30, Screen.height/20), "", _mutebutton)){
			if(audio.mute == false){	
				audio.mute = true;
				_mutebutton.normal.background = _mute;
			}
			else{ 
				audio.mute = false;
				_mutebutton.normal.background = _unmute;
			}
		}
		
		if(GUI.Button(new Rect((Screen.width*17/20), 0, Screen.width/30, Screen.height/20), "", _soundbutton))
			if(audio.mute == false){	
				audio.mute = true;
				_soundbutton.normal.background = _nosound;
			}
			else{ 
				audio.mute = false;
				_soundbutton.normal.background = _sound;
			}
		
		
		//Start button
		if(GUI.Button(new Rect((Screen.width/2) - Screen.height/10, (Screen.height/2) - Screen.height/10, Screen.width/10, Screen.height/10), new GUIContent("Start", "Click to start"), _menuskin.button)){

            PlayerPrefs.Save();
			Application.LoadLevel("Scene");
			
		}
		
		//Options Button
		GUI.Button(new Rect((Screen.width/2) - Screen.height/10, (Screen.height/2), Screen.width/10, Screen.height/10), new GUIContent("Options", "Click to mute, change volume settings or game quality"), _menuskin.button);
		
		
		//Credits Window
		if(GUI.Button(new Rect((Screen.width/2) - Screen.height/10, (Screen.height/2) + Screen.height/10, Screen.width/10, Screen.height/10), new GUIContent("Credits", "Click to view the credits"), _menuskin.button)){
			if(_showcredits == true)
				_showcredits = false;
			else _showcredits = true;

		}
		
		if(_showcredits)
			_creditwindow = GUI.Window(1, _creditwindow, doMyWindow, "Credits\n\n Music\n SoundFx\n Special Thanks to\n", _menuskin.window);
				
		GUI.Label (new Rect (Input.mousePosition.x + 10,Screen.height-(Input.mousePosition.y) - 10,400,20), GUI.tooltip, _menuskin.label);
	}
	
	void doMyWindow (int windowid) {
		Debug.Log("doMyWindow");
	}
}
