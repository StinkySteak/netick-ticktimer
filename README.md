# Netick TickTimer
Netick is a state transfer Unity netcode.
This TickTimer is inspired by Photon Fusion TickTimer

TickTimer is a plugin wrapper to count if the tick we are targeting has been passed, rather than decrementing a float with deltaTime.

## 0.1.1
Please Use this version if your netick version is 0.9.7, otherwise use Netick TickTimer 0.1.0

### How to Use?
```csharp
[Networked] private TickTimer _destroyTimer { get; set; }
private float _delay = 5f;

public override void NetworkStart()
{
    _destroyTimer = TickTimer.CreateFromSeconds(Sandbox, _delay);
}

public override void NetworkFixedUpdate()
{
    if (_destroyTimer.IsExpired(Sandbox))
    {
        _disableTimer = TickTimer.None;
        Sandbox.Destroy(Object);
    }
}
```

### TickTimer vs AuthTickTimer vs PauseableTickTimer
| Timers             | Description                                                            |
|--------------------|------------------------------------------------------------------------|
| TickTimer          | Uses predictive tick if available. Most recommended way to sync timers |
| AuthTickTimer      | Uses the server tick. Good for non predictive stuff                    |
| PauseableTickTimer | Can be paused across network                                          |

### API Reference

| API                   | Description                                        |
|-----------------------|----------------------------------------------------|
| IsRunning             | Has a valid Target Tick                            |
| IsExpired             | Has passed the target Tick                         |
| IsExpiredOrNotRunning | Either Has passed the target Tick or it wasn't set |
| GetAlpha              | Get the progress to the target tick                |
| GetAlphaClamed        | Get the progress to the target tick (clamped to 1) |                                                                    |