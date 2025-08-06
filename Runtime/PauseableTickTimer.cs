using Netick.Unity;
using UnityEngine;

namespace StinkySteak.Netick.Timer 
{
    /// <summary>
    /// A Networkable Lightweight Timer with Addition of Pause Functionality
    /// </summary>
    public struct PauseableTickTimer
    {
        public int EstablishedTick { get; set; }
        public int TargetTick { get; set; }
        public float TickDuration => TargetTick - EstablishedTick;
        public bool IsPaused { get; set; }
        public int LastPausedTick { get; set; }
        public bool IsRunning => TargetTick > 0;
        public static PauseableTickTimer None => default;

        public int RemainingTick(NetworkSandbox sandbox)
        {
            if (!IsPaused)
                return TargetTick - sandbox.Tick.TickValue;
            else
                return TargetTick - sandbox.Tick.TickValue + sandbox.Tick.TickValue - LastPausedTick;
        }
        public float RemainingSecond(NetworkSandbox sandbox)
        {
            return Mathf.Max(RemainingTick(sandbox) / (1 / sandbox.FixedDeltaTime), 0f);
        }

        public float RemainingSecondSmoothed(NetworkSandbox sandbox)
        {
            float detailedRemainingTick = RemainingTick(sandbox) - sandbox.LocalInterpolation.Alpha;

            float tickRate = 1f / sandbox.FixedDeltaTime;

            float seconds = detailedRemainingTick / tickRate;

            return seconds;
        }

        /// <summary>
        /// Used to Pauseable TickTimer by getting tick duration parameter
        /// </summary>
        /// <param name="sandbox">Active Network Sandbox, used to get current running tick</param>
        /// <param name="tickDuration">How many Ticks are required simulated</param>
        /// <returns></returns>
        public static PauseableTickTimer CreateFromTicks(NetworkSandbox sandbox, int tickDuration)
        {
            int currentTick = sandbox.Tick.TickValue;

            int targetTick = currentTick + tickDuration;

            return new PauseableTickTimer()
            {
                EstablishedTick = currentTick,
                TargetTick = targetTick,
            };
        }

        /// <summary>
        /// Used to Pauseable TickTimer by getting duration parameter
        /// </summary>
        /// <param name="sandbox">Active Network Sandbox, used to get current running tick</param>
        /// <param name="duration">Duration in Realtime seconds</param>
        /// <returns></returns>
        public static PauseableTickTimer CreateFromSeconds(NetworkSandbox sandbox, float duration)
        {
            float tickRate = 1 / sandbox.FixedDeltaTime;

            return CreateFromTicks(sandbox, Mathf.RoundToInt(duration * tickRate));
        }

        /// <summary>
        /// This method will compare if running tick has pass the target tick <br/>
        /// This will only works if the timer has been set
        /// </summary>
        public bool IsExpired(NetworkSandbox sandbox)
            => RemainingTick(sandbox) <= 0 && IsRunning;

        /// <summary>
        /// This method will compare if running tick has pass the target tick
        /// </summary>
        public bool IsExpiredOrNotRunning(NetworkSandbox sandbox)
            => RemainingTick(sandbox) <= 0;

        public float GetAlpha(NetworkSandbox sandbox)
        {
            int startTick = EstablishedTick;
            int targetTick = TargetTick;

            int currentTick = sandbox.Tick.TickValue;

            int start = currentTick - startTick;
            int end = targetTick - startTick;

            float alpha = start / (float)end;

            return alpha;
        }

        public float GetAlphaClamped(NetworkSandbox sandbox)
        {
            return Mathf.Clamp01(GetAlpha(sandbox));
        }

        public override string ToString()
        {
            return $"[PauseableTickTimer] From: {EstablishedTick} To: {TargetTick} Is Paused: {IsPaused}";
        }
    }
}