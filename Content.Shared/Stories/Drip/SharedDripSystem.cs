using Content.Shared.Containers.ItemSlots;
using Content.Shared.Interaction;
using Content.Shared.Lock;
using Content.Shared.Verbs;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Shared.Stories.Drip;

public abstract class SharedDripSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly ItemSlotsSystem _itemSlots = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    public override void Initialize()
    {
        SubscribeLocalEvent<DripComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<DripComponent, ComponentRemove>(OnComponentRemove);
        SubscribeLocalEvent<DripComponent, ComponentStartup>(OnComponentStartup);

        SubscribeLocalEvent<DripComponent, EntInsertedIntoContainerMessage>(OnContainerModified);
        SubscribeLocalEvent<DripComponent, EntRemovedFromContainerMessage>(OnContainerModified);
    }

    public virtual void OnComponentInit(EntityUid uid, DripComponent drip, ComponentInit args)
    {
        _itemSlots.AddItemSlot(uid, "DripPacked", drip.DripPackedSlot);
    }

    public virtual void OnComponentRemove(EntityUid uid, DripComponent drip, ComponentRemove args)
    {
        _itemSlots.RemoveItemSlot(uid, drip.DripPackedSlot);
    }

    public virtual void OnComponentStartup(EntityUid uid, DripComponent drip, ComponentStartup args)
    {

    }

    public virtual void OnContainerModified(EntityUid uid, DripComponent cabinet, ContainerModifiedMessage args)
    {

    }
}
