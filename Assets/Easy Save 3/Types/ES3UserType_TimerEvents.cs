using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("IsRunning", "IsPaused", "ElapsedTime", "Ended")]
	public class ES3UserType_TimerEvents : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_TimerEvents() : base(typeof(FireRingStudio.TimerEvents)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (FireRingStudio.TimerEvents)obj;
			
			writer.WriteProperty("IsRunning", instance.IsRunning, ES3Type_bool.Instance);
			writer.WriteProperty("IsPaused", instance.IsPaused, ES3Type_bool.Instance);
			writer.WriteProperty("ElapsedTime", instance.ElapsedTime, ES3Type_float.Instance);
			writer.WriteProperty("Ended", instance.Ended, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (FireRingStudio.TimerEvents)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "IsRunning":
						instance.IsRunning = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "IsPaused":
						instance.IsPaused = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "ElapsedTime":
						instance.ElapsedTime = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "Ended":
						instance.Ended = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}

            if (instance.Ended && instance.ReplayEnd)
            {
                instance.OnEndTimer();
            }

            if (instance.IsRunning)
            {
                instance.StopAllCoroutines();
                instance.StartCoroutine(instance.TimerRoutine(true));
            }
        }
    }


	public class ES3UserType_TimerEventsArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_TimerEventsArray() : base(typeof(FireRingStudio.TimerEvents[]), ES3UserType_TimerEvents.Instance)
		{
			Instance = this;
		}
	}
}