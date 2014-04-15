using System;
using FSM4U3D;

public class PlayFSC : FiniteStateComponent<PlayingFSMC.States>
{
	public override PlayingFSMC.States Id {
		get {
			return PlayingFSMC.States.PLAY;
		}
	}

	protected override string NextStateName ()
	{
		PlayingFSMC.States nextState = this.Id;
		
		switch( Inputs.Instance.CurrentEvent )
		{
		case Inputs.Events.ON_PAUSE:
			nextState = PlayingFSMC.States.PAUSE_PLAY;
			break;
		case Inputs.Events.NONE:
		default:
			break;
		}

		return nextState.ToString();
	}

}