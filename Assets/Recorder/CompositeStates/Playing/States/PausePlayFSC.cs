using System;
using FSM4U3D;

public class PausePlayFSC : FiniteStateComponent<PlayingFSMC.States>
{
	public override PlayingFSMC.States Id {
		get {
			return PlayingFSMC.States.PAUSE_PLAY;
		}
	}

	protected override string NextStateName ()
	{
		PlayingFSMC.States nextState = this.Id;
		
		switch( Inputs.Instance.CurrentEvent )
		{
			case Inputs.Events.ON_RESUME:
				nextState = PlayingFSMC.States.PLAY;
				break;
			case Inputs.Events.NONE:
			default:
				break;
		}

		return nextState.ToString();
	}

}