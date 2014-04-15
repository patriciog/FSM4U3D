using System;
using FSM4U3D;

public class RWFSC : FiniteStateComponent<IdleFSMC.States>
{
	public override IdleFSMC.States Id {
		get {
			return IdleFSMC.States.RW;
		}
	}

	protected override string NextStateName ()
	{
		IdleFSMC.States nextState = this.Id;

		switch( Inputs.Instance.CurrentEvent )
		{
		case Inputs.Events.ON_STOP:
			nextState = IdleFSMC.States.STOP;
			break;
		case Inputs.Events.NONE:
		default:
			break;
		}

		return nextState.ToString();
	}

}