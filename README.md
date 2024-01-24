# Netick TickTimer
Netick is a state transfer Unity netcode.
This TickTimer is inspired by Photon Fusion TickTimer

## 0.1.1
Please Use this version if your netick version is 0.9.7, otherwise use Netick TickTimer 0.1.0

### How to Use?
```csharp
[Networked] private TickTimer DisableTimer { get; set; }

private void NetworkStart()
{
    DisableTimer = TickTimer.CreateFromSeconds(_delay);
}

private void NetworkFixedUpdate()
{
    if(DisableTimer.IsExpired(Sandbox))
    {
        _gameObject.SetActive(false);
        _disableTimer = TickTimer.None;
    }
}
```

### TickTimer vs AuthTickTimer vs PauseableTickTimer
- `TickTimer`
Most recommended way to sync timers
- `AuthTickTimer`
Uses `sandbox.AuthoritativeTick` instead of `sandbox.Tick`. Should be used only by waiting for authorative ticks instead of predicted ticks.

- `PauseableTickTimer`
Can be paused accross network
