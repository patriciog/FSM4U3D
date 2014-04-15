using UnityEngine;
using System.Collections;
using FSM4U3D;

[RequireComponent(typeof(StopFSC))]
[RequireComponent(typeof(FFFSC))]
[RequireComponent(typeof(RWFSC))]
public class IdleFSMC : FiniteStateMachineComponent<IdleFSMC.States> {

	public enum States
	{
		STOP,
		FF,
		RW
	}

	protected override bool Setup ()
	{
		
		if( !this.AddState(this.gameObject.GetComponent<StopFSC>()) )
			return false;
		else
			this.gameObject.GetComponent<StopFSC>().enabled = false;
		
		if( !this.AddState(this.gameObject.GetComponent<FFFSC>()) )
			return false;
		else
			this.gameObject.GetComponent<FFFSC>().enabled = false;
		
		if( !this.AddState(this.gameObject.GetComponent<RWFSC>()) )
			return false;
		else
			this.gameObject.GetComponent<RWFSC>().enabled = false;
		
		return true;
		
	}

}
