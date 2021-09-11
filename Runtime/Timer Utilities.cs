using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace FreyrEssentials
{
	public enum Normalization { Linear, In, Out, InOut, Unclamped }

	public abstract class Bar
	{
		public float Inverse => 1f - this;
		/// <summary>
		/// Gives you a normal from 0 to 1 then back to 0
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
		/// Whether to linear the progress or let it exceed 1. By default it's true.
		/// </summary>
		public Normalization normalization = Normalization.Linear;
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
        {
			CurrentValue = 0f;
			Reached = false;
        }

		public static implicit operator float(Bar bar)
		{
			float time = bar.CurrentValue / bar.TopValue;
			float normTime = Math.Clamp(time, 0f, 1f);

            return bar.normalization switch
            {
                Normalization.Unclamped => time,
                Normalization.Linear => normTime,
                Normalization.InOut => (normTime *= 2f) < 1f 
										? 0.5f * normTime * normTime 
										: -0.5f * ((normTime -= 1f) * (normTime - 2f) - 1f),
                Normalization.In => normTime * normTime,
                Normalization.Out => normTime * (2f - normTime),
                
                _ => normTime,
            };

			//static float Bezier(float k, float c)
			//{
			//	return c * 2 * k * (1 - k) + k * k;
			//}
		}

		public static implicit operator bool(Bar bar)
			=> bar.Reached || bar.CurrentValue >= bar.TopValue;
    }

    /// <summary>
    /// Timer that ticks towards a set time and can be used as a float: the timer's normalized progression.
    /// It can also be used as a bool: true if finished.
    /// </summary>
    public class Timer : Bar
	{
		public float TimeStarted { get; private set; }
		public float SetTime => base.TopValue;
		public override float CurrentValue => GetTime() - TimeStarted;

        readonly Func<float> GetTime = () => (float)DateTime.Now.TimeOfDay.TotalSeconds;

    #region Statics👁

        public static Timer Create(float setTime, Normalization normalization = 0)
            => Create(setTime, null, normalization);
        public static Timer Create(float setTime, Func<float> timeSource, Normalization normalization = 0)
        {
            Timer timer = new Timer(normalization, timeSource);
            timer.Start(setTime);
            return timer;
        }


        public static Timer Finished => new Timer { Reached = true }; 
    #endregion

		/// <param name="time">The time to count towards.</param>
		/// <param name="linear">Whether to linear the progress or let it exceed 1. By default it's true</param>
		public Timer() : base() { }

		public Timer(Normalization normalization) : base(normalization) { }
		public Timer(Normalization normalization, Func<float> timeSource) : base(normalization)
        {
			GetTime ??= timeSource;
        }

		/// <summary>
		/// Start/reset the clock.
		/// </summary>
		public void Start(float setTime)
		{
			TimeStarted = GetTime();
			Reached = false;
			TopValue = setTime;
		}

		/// <summary>
		/// Runs until timer is complete
		/// </summary>
        public IEnumerator GetRoutine(Action<Timer> Routine)
		{
			if (Routine == null) yield break;
			Restart();
			while (!this)
			{
				yield return null;
				Routine(this);
			}
		}

		public IEnumerator GetRoutine(Action OnComplete)
		{
			if (OnComplete == null) yield break;
			Restart();
			while (!this) yield return null;
			OnComplete();
		}

		//TODO: Pause()/Resume()
		//Can be restarted to resume as well

		public override void Restart()
        {
			base.Restart();
			TimeStarted = GetTime();
        }

		public static implicit operator Timer(bool boolValue)
			=> new Timer { Reached = boolValue };
	}

	public class Charger : Bar
	{
		public float deltaDecline;
		public Func<float> deltaTime;
		public void Charge(float amount)
		{
			CurrentValue += amount;
			CurrentValue = Math.Clamp(CurrentValue, 0f, TopValue);
		}


		public Charger(float topValue, Func<float> deltaTime) : this(topValue, deltaTime, 0f) { }
		public Charger(float topValue, Func<float> deltaTime, float deltaDecline)
			=> (this.TopValue, this.deltaTime, this.deltaDecline) = (topValue, deltaTime, deltaDecline);


		public static implicit operator float(Charger charger)
		{
			charger.CurrentValue -= charger.deltaTime() * charger.deltaDecline;
			return (Bar)charger;
		}
	}
}