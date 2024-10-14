using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("_on")]
	public class ES3UserType_ToggleEventsInput : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_ToggleEventsInput() : base(typeof(FireRingStudio.Input.ToggleEventsInput)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (FireRingStudio.Input.ToggleEventsInput)obj;
			
			writer.WritePrivateField("_on", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (FireRingStudio.Input.ToggleEventsInput)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "_on":
					instance = (FireRingStudio.Input.ToggleEventsInput)reader.SetPrivateField("_on", reader.Read<System.Boolean>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}

			instance.InvokeEvent();
		}
	}


	public class ES3UserType_ToggleEventsInputArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_ToggleEventsInputArray() : base(typeof(FireRingStudio.Input.ToggleEventsInput[]), ES3UserType_ToggleEventsInput.Instance)
		{
			Instance = this;
		}
	}
}