using UnityEngine;
using System.Collections;
using FSM4U3D;

[RequireComponent(typeof(PlayFSC))]
[RequireComponent(typeof(PausePlayFSC))]
public class PlayingFSMC : FiniteStateMachineComponent<PlayingFSMC.States> {

	public enum States
	{
		PLAY,
		PAUSE_PLAY
	}

	protected override bool Setup ()
	{
		
		if( !this.AddState(this.gameObject.GetComponent<PlayFSC>()) )
			return false;
		else
			this.gameObject.GetComponent<PlayFSC>().enabled = false;
		
		if( !this.AddState(this.gameObject.GetComponent<PausePlayFSC>()) )
			return false;
		else
			this.gameObject.GetComponent<PausePlayFSC>().enabled = false;
		
		return true;
		
	}

}
