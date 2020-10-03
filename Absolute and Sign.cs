namespace FreyrEssentials
{
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
	/// <summary>
	/// Sign value of float
	/// </summary>
	public struct Sign
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
	{
		readonly int Value;

		public readonly static Sign positive = new Sign(1);
		public readonly static Sign negative = new Sign(-1);

		public Sign(float value) : this()
			=> Value = value < 0 ? -1 : 1;

		public Sign(int value) : this()
			=> Value = value < 0 ? -1 : 1;

		public static implicit operator Sign(float value)
			=> new Sign(value);

		public static implicit operator float(Sign value)
			=> value.Value;

		public static implicit operator int(Sign value)
			=> value.Value;

		static bool IsPositive(float value) => value >= 0f;

		public static bool operator ==(float floatValue, Sign signValue)
			=> IsPositive(floatValue) == IsPositive(signValue.Value);
		public static bool operator !=(float floatValue, Sign signValue)
			=> IsPositive(floatValue) != IsPositive(signValue.Value);
		public static implicit operator bool(Sign sign)
			=> IsPositive(sign);

		public static Sign operator -(Sign value)
			=> -value.Value;
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