using UnityEngine;

namespace FreyrEssentials
{
    /// <summary>
    /// Sign value of float
    /// </summary>
    public struct Sign
    {
        readonly bool Value;

		public readonly static Sign POSITIVE = true;
		public readonly static Sign NEGATIVE = false;

		public Sign(float value) : this()
			=> Value = value >= 0;

		public Sign(int value) : this()
			=> Value = value >= 0;

		public Sign(bool value) : this()
			=> Value = value;

		public static implicit operator Sign(float value)
			=> new Sign(value);

		public static implicit operator float(Sign sign)
			=> sign.Value ? 1f : -1f;

		public static implicit operator int(Sign sign)
			=> sign.Value ? 1 : -1;

		public static implicit operator double(Sign sign)
			=> sign.Value ? 1.0 : -1.0;

		public static implicit operator Sign(bool value)
			=> new Sign(value);
	}

	/// <summary>
	/// Absolute value of float
	/// </summary>
	public struct Abs
	{
		private readonly float Value;

		public Abs(float value) : this()
			=> Value = value < 0 ? -value : value;

		public static implicit operator Abs(float value)
			=> new Abs(value);

		public static implicit operator float(Abs value)
			=> value.Value;
	}
}