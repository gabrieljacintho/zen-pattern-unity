using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("_poolTag", "_autoRelease", "_currentAutoReleaseDelay", "_currentAutoReleaseInRealTime", "_autoReleaseTime", "_releaseWhenInvisible", "_releaseWhenInvisibleDelay", "_releaseWhenInvisibleTime", "IsActive")]
	public class ES3UserType_PooledObject : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_PooledObject() : base(typeof(FireRingStudio.Pool.PooledObject)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (FireRingStudio.Pool.PooledObject)obj;
			
			writer.WritePrivateField("_poolTag", instance);
			writer.WritePrivateField("_autoRelease", instance);
			writer.WritePrivateField("_currentAutoReleaseDelay", instance);
			writer.WritePrivateField("_currentAutoReleaseInRealTime", instance);
			writer.WritePrivateField("_autoReleaseTime", instance);
			writer.WritePrivateField("_releaseWhenInvisible", instance);
			writer.WritePrivateField("_releaseWhenInvisibleDelay", instance);
			writer.WritePrivateField("_releaseWhenInvisibleTime", instance);
			writer.WriteProperty("IsActive", instance.IsActive, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (FireRingStudio.Pool.PooledObject)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "_poolTag":
					instance = (FireRingStudio.Pool.PooledObject)reader.SetPrivateField("_poolTag", reader.Read<System.String>(), instance);
					break;
					case "_autoRelease":
					instance = (FireRingStudio.Pool.PooledObject)reader.SetPrivateField("_autoRelease", reader.Read<System.Boolean>(), instance);
					break;
					case "_currentAutoReleaseDelay":
					instance = (FireRingStudio.Pool.PooledObject)reader.SetPrivateField("_currentAutoReleaseDelay", reader.Read<System.Single>(), instance);
					break;
					case "_currentAutoReleaseInRealTime":
					instance = (FireRingStudio.Pool.PooledObject)reader.SetPrivateField("_currentAutoReleaseInRealTime", reader.Read<System.Boolean>(), instance);
					break;
					case "_autoReleaseTime":
					instance = (FireRingStudio.Pool.PooledObject)reader.SetPrivateField("_autoReleaseTime", reader.Read<System.Single>(), instance);
					break;
					case "_releaseWhenInvisible":
					instance = (FireRingStudio.Pool.PooledObject)reader.SetPrivateField("_releaseWhenInvisible", reader.Read<System.Boolean>(), instance);
					break;
					case "_releaseWhenInvisibleDelay":
					instance = (FireRingStudio.Pool.PooledObject)reader.SetPrivateField("_releaseWhenInvisibleDelay", reader.Read<System.Single>(), instance);
					break;
					case "_releaseWhenInvisibleTime":
					instance = (FireRingStudio.Pool.PooledObject)reader.SetPrivateField("_releaseWhenInvisibleTime", reader.Read<System.Single>(), instance);
					break;
					case "IsActive":
						instance.IsActive = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_PooledObjectArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_PooledObjectArray() : base(typeof(FireRingStudio.Pool.PooledObject[]), ES3UserType_PooledObject.Instance)
		{
			Instance = this;
		}
	}
}