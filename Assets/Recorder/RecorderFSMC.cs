using UnityEngine;
using System.Collections;
using FSM4U3D;

[RequireComponent(typeof(OffFSC))]
[RequireComponent(typeof(IdleCompositeFSC))]
[RequireComponent(typeof(PlayingCompositeFSC))]
[RequireComponent(typeof(RecordingCompositeFSC))]
public class RecorderFSMC : FiniteStateMachineComponent<RecorderFSMC.States>
{

	public enum States
	{
		OFF,
		IDLE,
		PLAYING,
		RECORDING
	}

	protected override bool Setup ()
	{
		
		if( !this.AddState(this.gameObject.GetComponent<OffFSC>()) )
			return false;
		else
			this.gameObject.GetComponent<OffFSC>().enabled = false;
		
		if( !this.AddState(this.gameObject.GetComponent<PlayingCompositeFSC>()) )
			return false;
		else
			this.gameObject.GetComponent<PlayingCompositeFSC>().enabled = false;
		
		if( !this.AddState(this.gameObject.GetComponent<IdleCompositeFSC>()) )
			return false;
		else
			this.gameObject.GetComponent<IdleCompositeFSC>().enabled = false;
		
		if( !this.AddState(this.gameObject.GetComponent<RecordingCompositeFSC>()) )
			return false;
		else
			this.gameObject.GetComponent<RecordingCompositeFSC>().enabled = false;
		
		return true;
		
	}

}
