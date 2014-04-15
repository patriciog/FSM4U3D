using System;
using FSM4U3D;

public class RecordFSC : FiniteStateComponent<RecordingFSMC.States>
{
	public override RecordingFSMC.States Id {
		get {
			return RecordingFSMC.States.RECORD;
		}
	}

	protected override string NextStateName ()
	{
		RecordingFSMC.States nextState = this.Id;
		
		switch( Inputs.Instance.CurrentEvent )
		{
			case Inputs.Events.ON_PAUSE:
				nextState = RecordingFSMC.States.PAUSE_RECORD;
				break;
			case Inputs.Events.NONE:
			default:
				break;
		}

		return nextState.ToString();
	}

}