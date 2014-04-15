using System;
using FSM4U3D;

public class OffFSC : FiniteStateComponent<RecorderFSMC.States>
{

	public override RecorderFSMC.States Id {
		get {
			return RecorderFSMC.States.OFF;
		}
	}

	protected override string NextStateName ()
	{
		RecorderFSMC.States nextState = this.Id;

		switch( Inputs.Instance.CurrentEvent )
		{
			case Inputs.Events.ON_POWER_ON:
				nextState = RecorderFSMC.States.IDLE;
				break;
			case Inputs.Events.NONE:
			default:
				break;
		}

		return nextState.ToString();
	}

}