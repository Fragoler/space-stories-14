using Content.Shared.Containers.ItemSlots;
using Content.Shared.Interaction;
using Content.Shared.Lock;
using Content.Shared.Verbs;
using Content.Shared.Stories.Drip;
using Content.Client.Stories.Drip;
using Content.Shared.Tag;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Timing;
using Robust.Shared.Utility;
using Content.Shared.Chemistry.Components;

namespace Content.Client.Stories.Drip;

public abstract class DripSystem : SharedDripSystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly ItemSlotsSystem _itemSlots = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly TagSystem _tag = default!;
    public override void Initialize()
    {

    }

    public override void OnComponentInit(EntityUid uid, DripComponent drip, ComponentInit args)
    {

    }

    public override void OnComponentRemove(EntityUid uid, DripComponent drip, ComponentRemove args)
    {

    }

    public override void OnComponentStartup(EntityUid uid, DripComponent drip, ComponentStartup args)
    {

    }

    public override void OnContainerModified(EntityUid uid, DripComponent drip, ContainerModifiedMessage args)
    {
        if (!TryComp<ChildrenSolutionVisualsComponent>(uid, out var childrenVisualComp))
        {
            return;
        }

        if (drip.DripPackedSlot.ContainerSlot is null || drip.DripPackedSlot.ContainerSlot.ContainedEntity is null)
        {
            return;
        }

        var innerEnt = (EntityUid) drip.DripPackedSlot.ContainerSlot.ContainedEntity;

        if (!TryComp<SolutionContainerVisualsComponent>(innerEnt, out var visualComp))
        {
            return;
        }

        childrenVisualComp.
    }
}
