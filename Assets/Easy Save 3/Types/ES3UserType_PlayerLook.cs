using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("_targetRotation", "_initialLook")]
	public class ES3UserType_PlayerLook : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_PlayerLook() : base(typeof(FireRingStudio.Movement.PlayerLook)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (FireRingStudio.Movement.PlayerLook)obj;
			
			writer.WritePrivateField("_targetRotation", instance);
			writer.WritePrivateField("_initialLook", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (FireRingStudio.Movement.PlayerLook)obj;

			instance.Restore();

			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "_targetRotation":
					instance = (FireRingStudio.Movement.PlayerLook)reader.SetPrivateField("_targetRotation", reader.Read<UnityEngine.Vector2>(), instance);
					break;
					case "_initialLook":
					instance = (FireRingStudio.Movement.PlayerLook)reader.SetPrivateField("_initialLook", reader.Read<UnityEngine.Vector2>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_PlayerLookArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_PlayerLookArray() : base(typeof(FireRingStudio.Movement.PlayerLook[]), ES3UserType_PlayerLook.Instance)
		{
			Instance = this;
		}
	}
}