using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("_object")]
	public class ES3UserType_SaveObject : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_SaveObject() : base(typeof(FireRingStudio.Save.SaveObject)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (FireRingStudio.Save.SaveObject)obj;
			
			writer.WritePrivateFieldByRef("_object", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (FireRingStudio.Save.SaveObject)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "_object":
					instance = (FireRingStudio.Save.SaveObject)reader.SetPrivateField("_object", reader.Read<UnityEngine.Object>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_SaveObjectArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SaveObjectArray() : base(typeof(FireRingStudio.Save.SaveObject[]), ES3UserType_SaveObject.Instance)
		{
			Instance = this;
		}
	}
}