using System;

namespace SaveSystem
{

	/// <summary>
	/// Mark a field or property as savable.
	/// </summary>
	[AttributeUsage (
		AttributeTargets.Property | AttributeTargets.Field,
		Inherited = false,
		AllowMultiple = false )]
	public sealed class Savable : Attribute
	{

		public Savable ()
		{
		}
		
	}

}