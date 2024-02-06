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

public sealed class DripSystem : SharedDripSystem
{

    protected override void OnContainerModified(EntityUid uid, DripComponent drip, ContainerModifiedMessage args)
    {
        if (!TryComp<ChildrenSolutionVisualsComponent>(uid, out var childrenVisualComp))
        {
            return;
        }

        if (drip.DripPackedSlot.ContainerSlot is null)
        {
            return;
        }
        Logger.Error("Exchange");
        childrenVisualComp.СhildrenItem = drip.DripPackedSlot.ContainerSlot.ContainedEntity;
        return;
    }
}
