using System;
using System.Collections.Generic;
using MythicEmpire.Enums;
using UnityEngine;
using MythicEmpire.CommonScript;

namespace MythicEmpire.Manager
{

	namespace MythicEmpire.Manager
	{

		public class EventManager : MonoBehaviour
		{
			public static EventManager Instance;

			void Awake()
			{
				// check if there's another instance already exist in scene
				if (Instance != null && Instance.GetInstanceID() != this.GetInstanceID())
				{
					// Common.Log("An instance of EventManager already exist : <{1}>, So destroy this instance : <{2}>!!", Instance.name, name);
					Destroy(Instance.gameObject);
					Instance = this;
				}
				else
				{
					// set instance
					Instance = this;
					DontDestroyOnLoad(this);
				}
			}


			void OnDestroy()
			{
				// reset this static var to null if it's the singleton instance
				if (Instance == this)
				{
					ClearAllListener();
					Instance = null;
				}
			}



			#region Fields

			Dictionary<EventID, Action<object>> _listeners = new Dictionary<EventID, Action<object>>();

			#endregion


			#region Add Listeners, Post events, Remove listener

			/// <summary>
			/// Register to listen for eventID
			/// </summary>
			/// <param name="eventID">EventID that object want to listen</param>
			/// <param name="callback">Callback will be invoked when this eventID be raised</param>
			public void RegisterListener(EventID eventID, Action<object> callback)
			{

				Common.Assert(callback != null, "AddListener, event {0}, callback = null !!", eventID.ToString());
				Common.Assert(eventID != EventID.None, "RegisterListener, event = None !!");

				// check if listener exist in dictionary
				if (_listeners.ContainsKey(eventID))
				{
					// add callback to our collection
					_listeners[eventID] += callback;
				}
				else
				{
					// add new key-value pair
					_listeners.Add(eventID, null);
					_listeners[eventID] += callback;
				}
			}

			/// <summary>
			/// Posts the event. This will notify all listener that register for this event
			/// </summary>
			/// <param name="eventID">EventID.</param>
			/// <param name="sender">Sender, in some case, the Listener will need to know who send this message.</param>
			/// <param name="param">Parameter. Can be anything (struct, class ...), Listener will make a cast to get the data</param>
			public void PostEvent(EventID eventID, object param = null)
			{
				if (!_listeners.ContainsKey(eventID))
				{
					Common.Log("No listeners for this event : {0}", eventID);
					return;
				}

				// posting event
				var callbacks = _listeners[eventID];
				// if there's no listener remain, then do nothing
				if (callbacks != null)
				{
					callbacks(param);
				}
				else
				{
					Common.Log("PostEvent {0}, but no listener remain, Remove this key", eventID);
					_listeners.Remove(eventID);
				}
			}

			/// <summary>
			/// Removes the listener. Use to Unregister listener
			/// </summary>
			/// <param name="eventID">EventID.</param>
			/// <param name="callback">Callback.</param>
			public void RemoveListener(EventID eventID, Action<object> callback)
			{
				// checking params
				Common.Assert(callback != null, "RemoveListener, event {0}, callback = null !!", eventID.ToString());
				Common.Assert(eventID != EventID.None, "AddListener, event = None !!");

				if (_listeners.ContainsKey(eventID))
				{
					_listeners[eventID] -= callback;
				}
				else
				{
					Common.Warning(false, "RemoveListener, not found key : " + eventID);
				}
			}

			/// <summary>
			/// Clears all the listener.
			/// </summary>
			private void ClearAllListener()
			{
				_listeners.Clear();
			}

			#endregion
		}
	}
}
