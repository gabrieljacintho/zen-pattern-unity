using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("CurrentHealth")]
	public class ES3UserType_Health : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Health() : base(typeof(FireRingStudio.Vitals.Health)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (FireRingStudio.Vitals.Health)obj;
			
			writer.WriteProperty("CurrentHealth", instance.CurrentHealth, ES3Type_float.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (FireRingStudio.Vitals.Health)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "CurrentHealth":
						instance.CurrentHealth = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
    }


	public class ES3UserType_HealthArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_HealthArray() : base(typeof(FireRingStudio.Vitals.Health[]), ES3UserType_Health.Instance)
		{
			Instance = this;
		}
	}
}