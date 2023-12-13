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
        /// This method will compare if running tick has pass the target tick & checks wheter its not paused
        /// </summary>
        public bool IsExpired(NetworkSandbox sandbox)
        {
            return RemainingTick(sandbox) <= 0;
        }

    }

}