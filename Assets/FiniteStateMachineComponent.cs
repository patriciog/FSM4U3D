//---------------------------------------------------------------------------
// FiniteStateMachine.cs
//---------------------------------------------------------------------------

/**
 * @file FiniteStateMachine.cs
 * Declara la clase FiniteStateMachine. 'Port' del código original (C++)
 * desarrollado por Pedro Pablo Gómez Martín en 2009 para MDVC.
 *
 * @see FiniteStateMachine
 * @see FiniteState
 *
 * @author Patricio González Sevilla, 0ctubre 2012
 */

using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Clase que implementa el comportamiento básico de una máquina de estados
 * finitos (FSM - Finite State Machine).
 * <br>
 * La máquina de estados es un contenedor de estados (definidos mediante la
 * clase FiniteState). Cada estado define un comportamiento, y tiene un identificador.
 * Los estados (y no la máquina) son los que deciden el siguiente estado.
 * Para indicar el siguiente estado proporcionan un identificador.
 * Eso obliga a que todos los estados tengan que poseer uno (un enum),
 * que no puede compartirse entre estados dentro de la misma máquina de estados.
 * Sin embargo desacopla un poco el conocimiento que los estados deben tener
 * entre ellos, y no es necesaria una organización en memoria en tiempo de
 * ejecución tan complicada de construir.
 * <br>
 * Como se ha dicho, cada estado define un comportamiento de alguna entidad superior.
 * Para poder definir esos comportamientos es necesario que cada estado tenga
 * acceso a esa entidad superior que están controlando. Para dar soporte a eso,
 * los estados heredan de MonoBehaviour. Por tanto, esta clase y los estados,
 * se adhieren a esa entidad superior.
 * A través de ella se supone que los estados accederan para crear el comportamiento,
 * y para decidir qué transición seguir.
 *
 * @todo Ahora mismo no puede ocurrir que un estado ocasione una transición
 * a un estado que no exista. Si esto sucede, la FSM se detiene.
 * Si se desea añadir esa posibilidad ver:
 * (FiniteState.nextState() o FiniteStateMachine.update()).
 *
 * @see FiniteStateComponent
 *
 * @author Patricio González Sevilla, 0ctubre 2012
 */

namespace FSM4U3D
{
	
	public abstract class FiniteStateMachineComponent<EnumClass> : MonoSingleton<FiniteStateMachineComponent<EnumClass>>
		where EnumClass : struct, System.IComparable, System.IConvertible, System.IFormattable
	{
		
		public EnumClass turnOnState;
		
		// data members

		/**
		 * Lista con todos los estados de la máquina de estados actual.
		 */
		protected Dictionary<EnumClass,FiniteStateComponent<EnumClass>> _states;

		/**
		 * Estado actual.
		 */
		protected FiniteStateComponent<EnumClass> _currentState;

		/**
		 * Si esta arrancada o parada.
		 */
		protected bool _stopped;
		
		protected EnumClass _previousStateId;
		
		// constructors
		// ------------
		
		/**
		 * Constructor vacío.
		 */
		public void Awake()
		{
			
			_states = new Dictionary<EnumClass, FiniteStateComponent<EnumClass>>();
			_currentState = null;
			_stopped = true;
			
		}
		
		public virtual void Start()
		{
			
			if( !this.Setup() )
			{
				Debug.Log ("FSM setup error.");
			}
			
			if( !this.TurnOn() )
			{
				Debug.Log ("FSM turn-on error.");
			}
			
		}
		
		// member functions (getter/setters)
		// ---------------------------------
		
		/**
		 * Establece un nuevo estado como el actual. Si previamente existía un estado
		 * establecido, se llamará a su método onExit. También se llama al método onEnter
		 * del nuevo estado.
		 * @param state Identificador del nuevo estado a establecer.
		 * @return Cierto si pudo establecerse el estado como actual (existe un estado
		 * con ese identificador). En caso contrario, devuelve falso y no se invocará
		 * a onExit del estado actual. En concreto, devuelve falso si el estado
		 * actual es el mismo que el que se intenta establecer.
		 */
		public virtual bool SetCurrentState(EnumClass targetStateId)
    	{
			
			if ( _currentState != null && _currentState.Id.Equals(targetStateId) )
			{
				_currentState.enabled = true;
				// El estado objetivo es el mismo que ahora. No hacemos nada
				return true;
			}
				
			if ( !_stopped && _currentState != null)
			{
				_currentState.OnExit();
				_currentState.enabled = false;
				
				// Fix previous state id
				if( _currentState != null )
				{
					if( _currentState.ReplacePreviousStateOnExit() )
					{
						
						Debug.Log("Previous State: " + _previousStateId.ToString());
						Debug.Log("State: " + targetStateId.ToString());
						
						_previousStateId = _currentState.Id;
						
					}
					// else Keep current previous state
				}
				else
					_previousStateId = targetStateId;
			}
			
			FiniteStateComponent<EnumClass> newState = _states[targetStateId];
			
			if( newState == null )
			{
				// No existe un identificador con ese estado.
				// Se detiene la maquina de estados.
				return false;	
			}
			
			_currentState = newState;
			_currentState.enabled = true;
			_currentState.OnEnter();

			return true;
			
    	}
		
		public FiniteStateComponent<EnumClass> CurrentState
    	{
	        get { return this._currentState; }
    	}
		
		public EnumClass PreviousStateId
    	{
	        get { return this._previousStateId; }
    	}
		
		public bool IsStopped
    	{
	        set { this._stopped = value; }
	        get { return this._stopped; }
    	}
		
		// member functions
		// ----------------
		
		protected abstract bool Setup ();
		
		[Obsolete("Use Setup() instead. GetComponents causes performance problems!")]
		protected virtual bool AutomaticSetup ()
		{
			
			FiniteStateComponent<EnumClass>[] states = this.gameObject.GetComponents<FiniteStateComponent<EnumClass>>();
			
			for( int i = 0; i < states.Length; i++)
			{
				if( !this.AddState(states[i]) )
			 		return false;
				else
					states[i].enabled = false;
			}
			
			return true;
			
		}
		
		/**
		 * Añade un nuevo estado a la máquina de estados. El estado debe haber sido
		 * creado previamente. El identificador del estado (establecido de algún modo
		 * en su constructor) debe ser correcto y no estar repetido en ningun estado
		 * añadido previamente a la máquina de estados.
		 *
		 * @param state Nuevo estado a añadir. No se hace copia del estado.
		 * @return Cierto si el estado pudo añadirse (no se están repitiendo
		 * identificadores).
		 */
		public bool AddState(FiniteStateComponent<EnumClass> newState)
		{
		
			// Comprobamos si el estado se ha instanciado y si no existe ya
			if (newState == null || _states.ContainsKey(newState.Id) )
				return false;

			_states.Add(newState.Id, newState);
			
			Debug.Log ("Added new state: " + newState.Id.ToString());
			
			return newState.OnCreate();
			
		}
		
		/**
		 * Pone a nulo el estado actual. La máquina de estados deja de tener un estado
		 * actual. Si existía un estado actual, invoca a su método onExit().
		 */
		public void ClearCurrentState()
		{
			if ( _currentState != null )
			{
				_currentState.OnExit();
				_currentState.enabled = false;
			}
			_currentState = null;
		}

		/**
		 * Si hay que intercambiar el estado actual, lo cambia.
		 */
		public void CheckCurrentState()
		{
		
			if ( !_stopped && _currentState != null &&
					!SetCurrentState(_currentState.NextState()) ) {
				// Apagamos la FSM si no podemos seguir.
				_stopped = true;	
			}
			
		}


		/**
		 * Actualiza la máquina de estados. Si hay estado actual, lo actualiza.
		 * @param Milisegundos transcurridos desde la última llamada.
		 */
		public void Update()
		{
			
			// Simulamos y actualizamos si podemos.
			if( !_stopped )
			{
				// FIRST: Check-in state.
				this.CheckCurrentState();
				
			}
			else
			{
				if( _currentState != null )
					_currentState.enabled = false;	
			}
				
		}
		
		/**
		 * Método llamado cuando se arranca la máquina de estados.
		 * @note El comportamiento por defecto no hace nada.
		 */
		public virtual void OnTurnOn()
		{
			// Default empty
		}
		
		/**
		 * Método que inicia la maquina de estados.
		 * @return	Si se ha inicializado bien o no.
		 * @note:	Debe ser reimplementado en las clases hijas
		 *			para definir el estado inicial y _stopped => false.
		 */
		public bool TurnOn()
		{
			
			this.OnTurnOn();
			
			_stopped = false;

			if( !this.SetCurrentState( turnOnState ) )
				return false;
			
			return true;
		}

		/**
		 * Método que apaga la maquina de estados.
		 * @note:	Puede ser que queramos reimplementarlo
		 *			por eso lo dejamos como virtual.
		 */
		public void TurnOff()
		{
			
			if( !this._stopped ) {
				ClearCurrentState();
				_stopped = true;
			}
			
			this.OnTurnOff();
			
		}
		
		/**
		 * Método llamado cuando se apaga la máquina de estados.
		 * @note El comportamiento por defecto no hace nada.
		 */
		public virtual void OnTurnOff()
		{
			// Default empty	
		}
		
		/**
		 * Destructor. Independientemente de que haya o no un estado activo válido,
		 * no se llamará a su método onExit(). Como se destruyen todos los estados
		 * establecidos con AddState, los estados aún tienen la oportunidad de liberar
		 * los recursos en el destructor.
		 */
		public void OnDestroy()
		{
			
			if( _states != null )
			{
			
				foreach(  KeyValuePair< EnumClass, FiniteStateComponent<EnumClass> > kvp in _states )
				{
					kvp.Value.OnDestroy();
				}
				
			}
			
		}
		
	}
	
}