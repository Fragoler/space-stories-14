using Content.Shared.Containers.ItemSlots;
using Content.Shared.Chemistry.Components;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Content.Shared.FixedPoint;


namespace Content.Shared.Stories.Drip;

/// <summary>
///     Used for entities that can be opened, closed, and can hold one item. E.g., fire extinguisher cabinets.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class DripComponent : Component
{
    [DataField("dripSlot"), ViewVariables]
    public ItemSlot DripPackedSlot = new();

    public string DripTag = "DripBag";

    public string SolutionPackName = "pack";

    public Entity<SolutionComponent>? PackSolution = null;

    [DataField]
    public bool HaveCapillar = true;

    [DataField, ViewVariables]
    public EntityUid? ConnectedEnt = null;

    [ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 TransferAmount { get; set; } = FixedPoint2.New(2);

    [ViewVariables(VVAccess.ReadWrite)]
    public bool CanChangeTransferAmount { get; set; } = false;

    public float AccumulatedFrametime = 0.0f;

    /// <summary>
    ///     How frequently should this drip update, in seconds?
    /// </summary>
    [DataField]
    public float UpdateInterval = 1.0f;

}
