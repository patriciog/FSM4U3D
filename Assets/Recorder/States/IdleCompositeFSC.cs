using System;
using FSM4U3D;

public class IdleCompositeFSC : FiniteStateComponent<RecorderFSMC.States>
{

	public override RecorderFSMC.States Id {
		get {
			return RecorderFSMC.States.IDLE;
		}
	}
	
	protected override string NextStateName ()
	{
		RecorderFSMC.States nextState = this.Id;
		
		switch( Inputs.Instance.CurrentEvent )
		{
			case Inputs.Events.ON_POWER_OFF:
				nextState = RecorderFSMC.States.OFF;
				break;
			case Inputs.Events.ON_PLAY:
				if( IdleFSMC.States.STOP.Equals( IdleFSMC.Instance.CurrentState.Id ) )
					nextState = RecorderFSMC.States.PLAYING;
				break;
			case Inputs.Events.ON_REC:
				if( IdleFSMC.States.STOP.Equals( IdleFSMC.Instance.CurrentState.Id ) )
					nextState = RecorderFSMC.States.RECORDING;
				break;
			case Inputs.Events.NONE:
			default:
				break;
		}
		
		return nextState.ToString();
	}

	public override bool OnCreate ()
	{

		if( base.OnCreate () ) 
		{
			IdleFSMC.Instance.TurnOff();
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

		IdleFSMC.Instance.TurnOn();

	}

	public override void OnExit ()
	{
		base.OnExit ();

		IdleFSMC.Instance.TurnOff();
	}

}

