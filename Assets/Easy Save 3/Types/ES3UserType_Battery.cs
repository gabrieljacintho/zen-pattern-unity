using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("_batteryLevel")]
	public class ES3UserType_Battery : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Battery() : base(typeof(FireRingStudio.Energy.Battery)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (FireRingStudio.Energy.Battery)obj;
			
			writer.WritePrivateField("_batteryLevel", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (FireRingStudio.Energy.Battery)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "_batteryLevel":
					instance = (FireRingStudio.Energy.Battery)reader.SetPrivateField("_batteryLevel", reader.Read<System.Single>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}

			instance.OnBatteryLevelChanged?.Invoke(instance.BatteryLevel);
		}
	}


	public class ES3UserType_BatteryArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_BatteryArray() : base(typeof(FireRingStudio.Energy.Battery[]), ES3UserType_Battery.Instance)
		{
			Instance = this;
		}
	}
}