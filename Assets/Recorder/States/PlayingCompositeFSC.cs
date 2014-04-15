using System;
using FSM4U3D;

public class PlayingCompositeFSC : FiniteStateComponent<RecorderFSMC.States>
{
	public override RecorderFSMC.States Id {
		get {
			return RecorderFSMC.States.PLAYING;
		}
	}

	public override bool OnCreate ()
	{
		
		if( base.OnCreate () ) 
		{
			PlayingFSMC.Instance.TurnOff();
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public override void OnEnter ()
	{
		base.OnEnter ();
		
		PlayingFSMC.Instance.TurnOn();
		
	}

	protected override string NextStateName ()
	{
		RecorderFSMC.States nextState = this.Id;
		
		switch( Inputs.Instance.CurrentEvent )
		{
		case Inputs.Events.ON_POWER_OFF:
			nextState = RecorderFSMC.States.OFF;
			break;
		case Inputs.Events.ON_STOP:
			nextState = RecorderFSMC.States.IDLE;
			break;
		case Inputs.Events.NONE:
		default:
			break;
		}

		return nextState.ToString();
	}
	
	public override void OnExit ()
	{
		base.OnExit ();
		
		PlayingFSMC.Instance.TurnOff();
	}

}

