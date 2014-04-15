using System;
using FSM4U3D;

public class StopFSC : FiniteStateComponent<IdleFSMC.States>
{
	public override IdleFSMC.States Id {
		get {
			return IdleFSMC.States.STOP;
		}
	}

	protected override string NextStateName ()
	{
		IdleFSMC.States nextState = this.Id;

		switch( Inputs.Instance.CurrentEvent )
		{
			case Inputs.Events.ON_FF:
				nextState = IdleFSMC.States.FF;
				break;
			case Inputs.Events.ON_RW:
				nextState = IdleFSMC.States.RW;
				break;
			case Inputs.Events.NONE:
			default:
				break;
		}

		return nextState.ToString();
	}

}