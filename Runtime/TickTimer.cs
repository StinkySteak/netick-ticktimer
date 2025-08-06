using UnityEngine;
using Netick.Unity;

namespace StinkySteak.Netick.Timer
{
    /// <summary>
    /// A Networkable Lightweight Timer
    /// </summary>
    public struct TickTimer
    {
        public int EstablishedTick { get; private set; }
        public int TargetTick { get; private set; }
        public float TickDuration => TargetTick - EstablishedTick;
        public bool IsRunning => TargetTick > 0;
        public static TickTimer None => default;

        public int RemainingTick(NetworkSandbox sandbox)
        {
            return TargetTick - sandbox.Tick.TickValue;
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
        /// Used to Create a Simple TickTimer by getting duration parameter
        /// </summary>
        /// <param name="sandbox">Active Network Sandbox, used to get current running tick</param>
        /// <param name="duration">Duration in Realtime seconds</param>
        /// <returns></returns>
        public static TickTimer CreateFromSeconds(NetworkSandbox sandbox, float duration)
        {
            float tickRate = 1 / sandbox.FixedDeltaTime;

            return CreateFromTicks(sandbox, Mathf.RoundToInt(duration * tickRate));
        }

        /// <summary>
        /// Used to Pauseable TickTimer by getting tick duration parameter
        /// </summary>
        /// <param name="sandbox">Active Network Sandbox, used to get current running tick</param>
        /// <param name="tickDuration">How many Ticks are required simulated</param>
        /// <returns></returns>
        public static TickTimer CreateFromTicks(NetworkSandbox sandbox, int tickDuration)
        {
            int currentTick = sandbox.Tick.TickValue;

            int targetTick = currentTick + tickDuration;

            return new TickTimer()
            {
                EstablishedTick = currentTick,
                TargetTick = targetTick,
            };
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
            return $"[TickTimer] From: {EstablishedTick} To: {TargetTick}";
        }
    }
}