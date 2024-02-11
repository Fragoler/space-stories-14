using Content.Shared.Containers.ItemSlots;
using Content.Shared.Chemistry.Components;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Content.Shared.FixedPoint;



namespace Content.Shared.Stories.Drip;

/// <summary>
///     Used for entities that can be opened, closed, and can hold one item. E.g., fire extinguisher cabinets.
/// </summary>
[RegisterComponent]
public sealed partial class CapillaryHoldComponent : Component
{
    [DataField, ViewVariables]
    public Dictionary<EntityUid, Capillary> Capillars = null;
}

public struct Capillary
{
    [DataField]
    public bool InjectedInBody = false;

    public float BleedOnEject = 10.0f;

    public Capillary(bool injectedInBody = false, float bleedOnEject = 10.0f)
    {
        BleedOnEject = bleedOnEject;
        InjectedInBody = injectedInBody;
    }
}
