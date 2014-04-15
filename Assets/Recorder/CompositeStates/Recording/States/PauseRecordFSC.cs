using System;
using FSM4U3D;

public class PauseRecordFSC : FiniteStateComponent<RecordingFSMC.States>
{
	public override RecordingFSMC.States Id {
		get {
			return RecordingFSMC.States.PAUSE_RECORD;
		}
	}

	protected override string NextStateName ()
	{
		RecordingFSMC.States nextState = this.Id;
		
		switch( Inputs.Instance.CurrentEvent )
		{
			case Inputs.Events.ON_REC:
				nextState = RecordingFSMC.States.RECORD;
				break;
			case Inputs.Events.NONE:
			default:
				break;
		}

		return nextState.ToString();
	}

}