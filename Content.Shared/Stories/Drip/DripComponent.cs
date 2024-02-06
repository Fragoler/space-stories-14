using Content.Shared.Containers.ItemSlots;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Stories.Drip;

/// <summary>
///     Used for entities that can be opened, closed, and can hold one item. E.g., fire extinguisher cabinets.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class DripComponent : Component
{
    /// <summary>
    ///     The <see cref="ItemSlot"/> that stores the actual item. The entity whitelist, sounds, and other
    ///     behaviours are specified by this <see cref="ItemSlot"/> definition.
    /// </summary>
    [DataField("dripSlot"), ViewVariables]
    public ItemSlot DripPackedSlot = new();

    public string DripTag = "DripBag";
}
