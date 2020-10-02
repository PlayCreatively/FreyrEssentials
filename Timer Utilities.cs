using System;
using System.Collections;
using UnityEngine;

namespace FreyrEssentials
{
	public enum Normalization { clamp, smoothClamp, unlimited }

	public abstract class Bar
	{
		public float Inverse => 1f - this;
		/// <summary>
		/// Gives you a normal from 0 to 1 and back to 0
		/// </summary>
		public float Midway
		{
			get
			{
				float val = this;
				float halfWay = val < .5f ? val : 1f - val;
				return halfWay * 2f;
			}
		}
		public float TopValue { get; protected set; }
		public virtual float CurrentValue { get; protected set; }
		/// <summary>
		/// Whether to clamp the progress or let it exceed 1. By default it's true.
		/// </summary>
		public Normalization normalization = Normalization.clamp;
		public bool Reached { get; protected set; }

		public Bar() { }

		public Bar(Normalization normalization)
			=> this.normalization = normalization;

		/// <summary>
		/// Set the timer to have reached the goal.
		/// </summary>
		public void Finish()
			=> Reached = true;
		public virtual void Restart()
			=> CurrentValue = 0f;

		public static implicit operator float(Bar bar)
		{
			float normalizedTime = bar.CurrentValue / bar.TopValue;

			switch (bar.normalization)
			{
				case Normalization.clamp:
					normalizedTime = Mathf.Clamp01(normalizedTime);
					break;
				case Normalization.smoothClamp:
					normalizedTime = Mathf.SmoothStep(0, 1, normalizedTime);
					break;
			}

			return normalizedTime;
		}

		public static implicit operator bool(Bar bar)
			=> bar.CurrentValue >= bar.TopValue;
	}

	/// <summary>
	/// Timer that ticks towards a set time and can be used as a float: the timer's normalized progression.
	/// It can also be used as a bool: true if finished.
	/// </summary>
	public class Timer : Bar
	{
		public float TimeStarted { get; private set; }
		public override float CurrentValue => Time.time - TimeStarted;

		/// <param name="time">The time to count towards.</param>
		/// <param name="clamp">Whether to clamp the progress or let it exceed 1. By default it's true</param>

		public Timer() : base() { }

		public Timer(Normalization normalization) : base(normalization) { }

		/// <summary>
		/// Start/reset the clock.
		/// </summary>
		public void Start(float setTime)
		{
			TimeStarted = Time.time;
			Reached = false;
			TopValue = setTime;
		}
		public static Timer Create(float setTime, Normalization normalization = 0)
		{
			Timer timer = new Timer(normalization);
			timer.Start(setTime);
			return timer;
		}
		public static Timer Finished => new Timer { Reached = true };

		public IEnumerator GetRoutine(Action<float> Routine)
		{
			if (Routine == null) yield break;
			Restart();
			do
			{
				Routine(this);
				yield return null;
			}
			while (!this);
		}

		public override void Restart()
			=> TimeStarted = Time.time;

		public static implicit operator Timer(bool boolValue)
			=> new Timer { Reached = boolValue };
	}

	public class Charger : Bar
	{
		public float deltaDecline;
		public void Charge(float amount)
		{
			CurrentValue += amount;
			CurrentValue = Mathf.Clamp(CurrentValue, 0f, TopValue);
		}


		public Charger(float topValue, float deltaDecline)
			=> (this.TopValue, this.deltaDecline) = (topValue, deltaDecline);


		public static implicit operator float(Charger charger)
		{
			charger.CurrentValue -= Time.deltaTime * charger.deltaDecline;
			return (Bar)charger;
		}
	}
}