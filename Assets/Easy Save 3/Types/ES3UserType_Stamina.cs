using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("CurrentStamina", "IsTired")]
	public class ES3UserType_Stamina : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Stamina() : base(typeof(FireRingStudio.Vitals.Stamina)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (FireRingStudio.Vitals.Stamina)obj;
			
			writer.WriteProperty("CurrentStamina", instance.CurrentStamina, ES3Type_float.Instance);
			writer.WriteProperty("IsTired", instance.IsTired, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (FireRingStudio.Vitals.Stamina)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "CurrentStamina":
						instance.CurrentStamina = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "IsTired":
						instance.IsTired = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}

			instance.UpdateAudio();
        }
	}


	public class ES3UserType_StaminaArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_StaminaArray() : base(typeof(FireRingStudio.Vitals.Stamina[]), ES3UserType_Stamina.Instance)
		{
			Instance = this;
		}
	}
}