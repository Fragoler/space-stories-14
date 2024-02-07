using Content.Shared.Containers.ItemSlots;
using Content.Shared.Interaction;
using Content.Shared.Lock;
using Content.Shared.Verbs;
using Content.Shared.Stories.Drip;
using Content.Shared.Tag;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Timing;
using Robust.Shared.Utility;
using Content.Shared.Chemistry.Components;
using Robust.Client.GameObjects;

namespace Content.Client.Stories.Drip;

public sealed class DripSystem : SharedDripSystem
{
    [Dependency] private readonly TagSystem _tag = default!;

    protected override void OnComponentInit(EntityUid uid, DripComponent drip, ComponentInit args)
    {
        base.OnComponentInit(uid, drip, args);

        UpdateAppereance(uid, drip);
    }

    protected override void OnContainerModified(EntityUid uid, DripComponent drip, ContainerModifiedMessage args)
    {
        UpdateAppereance(uid, drip);
    }

    private void UpdateAppereance(EntityUid uid, DripComponent drip)
    {
        if (!TryComp<ChildrenSolutionVisualsComponent>(uid, out var childrenVisualComp))
        {
            return;
        }

        if (drip.DripPackedSlot.ContainerSlot is null)
        {
            return;
        }

        childrenVisualComp.СhildrenItem = drip.DripPackedSlot.ContainerSlot.ContainedEntity;

        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        if (childrenVisualComp.СhildrenItem is not null && _tag.HasTag((EntityUid) childrenVisualComp.СhildrenItem, drip.DripTag))
        {
            sprite.LayerSetVisible(DripLayers.Packed, true);
        }
        else
        {
            sprite.LayerSetVisible(DripLayers.Packed, false);
        }
    }
}


public enum DripLayers
{
    Packed
}
