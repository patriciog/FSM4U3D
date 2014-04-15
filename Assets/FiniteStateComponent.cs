//---------------------------------------------------------------------------
// FiniteState.cs
//---------------------------------------------------------------------------

/**
 * @file FiniteState.cs
 * Declara la clase FiniteState. 'Port' del código original (C++)
 * desarrollado por Pedro Pablo Gómez Martín en 2009 para MDVC.
 *
 * @see FiniteState
 * @see FiniteStateMachine
 *
 * @author Patricio González Sevilla, 0ctubre 2012
 */

using System;
using UnityEngine;

/**
 * Clase abstracta de la que deben heredar todos los estados de la máquina de estados.
 * <br>
 * Además es una clase genérica. El parámetro de tipo EnumClass es un
 * enumerado que identifica el estado (comportamiento concreto).
 * Para más información, ver la documentación de la máquina de estados.
 *
 * @see FiniteStateMachine
 *
 * @author Patricio González Sevilla, 0ctubre 2012
 */

namespace FSM4U3D
{
	
	public abstract class FiniteStateComponentBase<EnumClass> : MonoSingleton<FiniteStateComponentBase<EnumClass>>
		where EnumClass : struct, System.IComparable, System.IConvertible, System.IFormattable
	{
		// To avoid Start implement on FSC
		public abstract void Start();

		// To avoid override NextState
		public abstract EnumClass NextState();
		
	}
		
	/** Execution order for any given FiniteStateComponent FSC
	 * All Awake calls
	 * All OnCreate Calls (use OnCreate instead Start)
	 * Only on FSMC.CurrentState:
	 * 		while (stepping towards variable delta time)
	 * 			All FixedUpdate functions
	 * 			Physics simulation
	 * 			OnEnter/Exit/Stay trigger functions
	 * 			OnEnter/Exit/Stay collision functions
	 * 		Rigidbody interpolation applies transform.position and rotation
	 * 		OnMouseDown/OnMouseUp etc. events
	 * 		All NextStateName Calls
	 * 		If FSMC.CurrentState is diferent to previous
	 * 			All OnExit Calls in previous states
	 * 			All OnEnter Calls in current states
	 * 		All Update functions
	 * 		Animations are advanced, blended and applied to transform
	 * 		All LateUpdate functions
	 * 		Rendering
	 * If FSMC.Stopped
	 * 		All OnExit Calls in current state
	 * 		All OnDestroy Calls in all states
	 */
	public abstract class FiniteStateComponent<EnumClass> : FiniteStateComponentBase<EnumClass>
		where EnumClass : struct, System.IComparable, System.IConvertible, System.IFormattable
	{
		
		// data members
		// ------------
	
		// Always ignore first Update: To avoid stray clicks from previous state...
		private bool _isReady;

		/**
		 * Identificador del estado. Cada estado de una máquina de estados
		 * posee su propio identificador. El método del estado que realiza la
		 * función de transición devuelve el identificador de estado, en lugar
		 * de un objeto al nuevo estado.
		 * Ver member functions getter Id
		 */
		// private EnumClass id;
		
		// member functions (getter/setter)
		// --------------------------------
		public abstract EnumClass Id { get; }
		
		// member functions
		// ----------------
		
		/**
		 * Devuelve el identificador del siguiente estado. Si se devuelve un identificador
		 * de estado inválido (que no existe en la máquina de estados en la que se
		 * encuentra el estado de este objeto), la máquina virtual no cambiará el estado.
		 */
		protected abstract string NextStateName();
		
		public override sealed EnumClass NextState()
		{
			if( !typeof(EnumClass).IsEnum )
   			{
   			    throw new NotSupportedException( "EnumClass must be an Enum" );
   			}
			
			return (EnumClass)Enum.Parse( typeof(EnumClass), this.NextStateName() );
		}
		
		public override sealed void Start()
		{
			// Use of the sealed modifier prevents a derived class from further overriding the method.
			// use OnCreate instead Start
		}

		/**
		 * Función llamada cuando se "engancha" el estado en una maquina de estados.
		 *
		 * @return Si el "enganche" se ha realizado correctamente.
		 */
		public virtual bool OnCreate()
		{
			return true;
		}
		 
		/**
		 * Método llamado cuando se establece el estado como nuevo.
		 * @note El comportamiento por defecto no hace nada.
		 */
		public virtual void OnEnter()
		{
			// Default empty
		
			_isReady = false;
			
		}
		
		/**
		 * Método llamado cuando se sale del estado.
		 * @note El comportamiento por defecto no hace nada.
		 */
		public virtual void OnExit()
		{
			// Default empty	
		}

		/**
		 * Función llamada cuando se "desengancha" el estado de una maquina de estados.
		 */
		public virtual void OnDestroy()
		{
			// Default empty
		}
		
		// If it alters current previous state or not
		public virtual bool ReplacePreviousStateOnExit()
		{
			return true;	
		}
		
		public virtual void Update()
		{
			if( !_isReady )
			{
				_isReady = !_isReady;
				return;
			}	
		}

	}; // class State
	
	/**
	 * Indica si es un estado desechable (un solo uso). En este tipo de
	 * estados es apropiado implementar el método IsComplete, que cuando
	 * pasa a ser "true", la propietaria se encarga de devolvernos al
	 * estado anterior. Util en casos como la definición de IA's donde
	 * hay un estado NO SUMIDERO, pero que actua como tal: todos los
	 * estados pueden acceder a uno en concreto (p.e. recibir impacto)
	 * y hasta que no se sale de ese estado no podemos regresar al que
	 * estabamos.
	 * Por defecto false.
	 */
	public abstract class DisposableFiniteStateComponent<EnumClass> : FiniteStateComponent<EnumClass>
		where EnumClass : struct, System.IComparable, System.IConvertible, System.IFormattable
	{
		
		protected abstract bool IsComplete();
		
		protected virtual string NextStateNameIsNoComplete()
		{
			return this.Id.ToString();
		}
		
		protected override sealed string NextStateName()
		{
			
			if( IsComplete() )
			{
				FiniteStateMachineComponent<EnumClass> owner =
					this.gameObject.GetComponent<FiniteStateMachineComponent<EnumClass>>()
						as FiniteStateMachineComponent<EnumClass>;				
				return owner.PreviousStateId.ToString();
			}
			else
				return NextStateNameIsNoComplete();
			
		}
		
		public override bool ReplacePreviousStateOnExit()
		{
			return false;	
		}
		
	}
	
}