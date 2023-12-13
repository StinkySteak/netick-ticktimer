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
        public static TickTimer None => default;

        public float RemainingTick(NetworkSandbox sandbox)
        {
            return TargetTick - sandbox.Tick.TickValue;
        }
        public float RemainingSecond(NetworkSandbox sandbox)
        {
            return Mathf.Max(RemainingTick(sandbox) / (1 / sandbox.FixedDeltaTime), 0f);
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
        /// This method will compare if running tick has pass the target tick
        /// </summary>
        public bool IsExpired(NetworkSandbox sandbox)
        {
            return RemainingTick(sandbox) <= 0;
        }
    }
}