using UnityEngine;
using System.Collections;
using FSM4U3D;

[RequireComponent(typeof(RecordFSC))]
[RequireComponent(typeof(PauseRecordFSC))]
public class RecordingFSMC : FiniteStateMachineComponent<RecordingFSMC.States> {

	public enum States
	{
		RECORD,
		PAUSE_RECORD
	}

	protected override bool Setup ()
	{
		
		if( !this.AddState(this.gameObject.GetComponent<RecordFSC>()) )
			return false;
		else
			this.gameObject.GetComponent<RecordFSC>().enabled = false;
		
		if( !this.AddState(this.gameObject.GetComponent<PauseRecordFSC>()) )
			return false;
		else
			this.gameObject.GetComponent<PauseRecordFSC>().enabled = false;
		
		return true;
		
	}

}
