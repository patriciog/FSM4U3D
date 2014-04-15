using UnityEngine;
using System.Collections;

public class Inputs : MonoSingleton<Inputs> {

	public bool buttonPower = false;

	public bool buttonStop = false;

	public bool buttonFF = false;

	public bool buttonRW = false;

	public bool buttonPlay = false;

	public bool buttonRecord = false;

	public bool buttonPause = false;

	public enum Events
	{
		NONE,
		ON_POWER_ON,
		ON_POWER_OFF,
		ON_FF,
		ON_STOP,
		ON_RW,
		ON_PLAY,
		ON_PAUSE,
		ON_RESUME,
		ON_REC,
	};
	
	private Events _currentEvent;
	
	public Events CurrentEvent {
		get { return _currentEvent; }
		set { _currentEvent = value; }
	}

	void OnGUI () {

		float groupWidth = 250;
		float groupHeight = 250;
		
		float screenWidth = Screen.width;
		float screenHeight = Screen.height;
		
		float groupX = ( screenWidth - groupWidth ) / 2;
		float groupY = ( screenHeight - groupHeight ) / 2;

		_currentEvent = Events.NONE;

		GUI.BeginGroup( new Rect( groupX, groupY, groupWidth, groupHeight ) );

		float buttonX = 10;
		float buttonY = 0;
		bool oldValue = false;

		GUI.Box( new Rect( 0, buttonY, groupWidth, groupHeight ), "Recorder buttons" );
		buttonY += 30;

		// POWER BUTTON
		oldValue = buttonPower;
		buttonPower = GUI.Toggle( new Rect( buttonX, buttonY, 100, 30 ), oldValue, "POWER" );
		if( buttonPower != oldValue )
		{
			if( buttonPower ) _currentEvent = Events.ON_POWER_ON;
			else _currentEvent = Events.ON_POWER_OFF;
		}
		buttonY += 30;

		// FAST FORWARD BUTTON
		if( GUI.Button( new Rect( buttonX, buttonY, 100, 30 ), "FF" ) )
		{
			_currentEvent = Events.ON_FF;
		}
		buttonY += 30;

		// STOP BUTTON
		if( GUI.Button( new Rect( buttonX, buttonY, 100, 30 ), "STOP" ) )
		{
			_currentEvent = Events.ON_STOP;
		}
		buttonY += 30;

		// REWIND BUTTON
		if( GUI.Button( new Rect( buttonX, buttonY, 100, 30 ), "RW" ) )
		{
			_currentEvent = Events.ON_RW;
		}
		buttonY += 30;

		// PLAY BUTTON
		if( GUI.Button( new Rect( buttonX, buttonY, 100, 30 ), "PLAY" ) )
		{
			_currentEvent = Events.ON_PLAY;
		}
		buttonY += 30;

		// PAUSE/RESUME BUTTON
		oldValue = buttonPause;
		buttonPause = GUI.Toggle( new Rect( buttonX, buttonY, 100, 30 ), oldValue, "||" );
		if( buttonPause != oldValue )
		{
			if( buttonPause ) _currentEvent = Events.ON_PAUSE; // PAUSE
			else _currentEvent = Events.ON_RESUME;	// RESUME
		}
		buttonY += 30;

		// REC BUTTON
		if( GUI.Button( new Rect( buttonX, buttonY, 100, 30 ), "REC" ) )
		{
			_currentEvent = Events.ON_REC;
		}
		buttonY += 30;

		GUI.EndGroup();
	}

}
