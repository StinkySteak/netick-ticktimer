using Netick.Unity;

namespace StinkySteak.Netick.Timer 
{
    public static class PauseableTickTimerExtension
    {
        /// <summary>
        /// How to Use?
        /// <br/>
        /// TickTimer ExistingTimer = PauseableTickTimer.Pause(Sandbox);
        /// </summary>
        /// <param name="sandbox">Active Network Sandbox, used to get current running tick</param>
        /// <returns></returns>
        public static PauseableTickTimer Pause(this PauseableTickTimer timer, NetworkSandbox sandbox)
        {
            if (timer.IsPaused)
                return timer;

            return new PauseableTickTimer()
            {
                EstablishedTick = timer.EstablishedTick,
                TargetTick = timer.TargetTick,
                IsPaused = true,
                LastPausedTick = sandbox.Tick.TickValue
            };
        }

        /// <summary>
        /// How to Use?
        /// <br/>
        /// TickTimer ExistingTimer = PauseableTickTimer.Resume(Sandbox);
        /// </summary>
        /// <param name="sandbox">Active Network Sandbox, used to get current running tick</param>
        /// <returns></returns>
        public static PauseableTickTimer Resume(this PauseableTickTimer timer, NetworkSandbox sandbox)
        {
            if (!timer.IsPaused)
                return timer;

            int pausedDurationTick = sandbox.Tick.TickValue - timer.LastPausedTick;
            int finalTargetTick = timer.TargetTick + pausedDurationTick;

            return new PauseableTickTimer()
            {
                EstablishedTick = timer.EstablishedTick,
                LastPausedTick = 0,
                TargetTick = finalTargetTick,
                IsPaused = false
            };
        }
    }
}