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

        SubscribeLocalEvent<DripComponent, EntInsertedIntoContainerMessage>(OnContainerModified);
        SubscribeLocalEvent<DripComponent, EntRemovedFromContainerMessage>(OnContainerModified);

        SubscribeLocalEvent<DripComponent, GetVerbsEvent<AlternativeVerb>>(AddCapillarVerb);
    }

    protected virtual void OnComponentInit(EntityUid uid, DripComponent drip, ComponentInit args)
    {
        _itemSlots.AddItemSlot(uid, "DripPacked", drip.DripPackedSlot);
    }

    protected virtual void OnComponentRemove(EntityUid uid, DripComponent drip, ComponentRemove args)
    {
        _itemSlots.RemoveItemSlot(uid, drip.DripPackedSlot);
    }

    private void AddCapillarVerb(EntityUid uid, DripComponent drip, GetVerbsEvent<AlternativeVerb> args)
    {
        var isBringingCapillar = true;
        var userCapillars = EnsureComp<CapillaryHoldComponent>(args.User);

        if (!drip.HaveCapillar)
        {
            if (!userCapillars.Capillars.TryGetValue(uid, out var capillary) || capillary.InjectedInBody)
                return;
            isBringingCapillar = false;
        }

        AlternativeVerb verb = new()
        {
            Act = isBringingCapillar ?
                () => BringCapillary(uid, drip, args.User) :
                () => PutCapillary(uid, drip, args.User),
            Text = Loc.GetString(isBringingCapillar ? "capillar-verb-bring" : "capillar-verb-put"),
            Icon = isBringingCapillar ?
                new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/drink.svg.192dpi.png")) :
                new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/drink.svg.192dpi.png")),
        };

        args.Verbs.Add(verb);
    }

    private void PutCapillary(EntityUid uid, DripComponent drip, EntityUid target)
    {
        var capilComp = EnsureComp<CapillaryHoldComponent>(target);

        capilComp.Capillars.Remove(uid);
        drip.HaveCapillar = true;
    }

    private void BringCapillary(EntityUid uid, DripComponent drip, EntityUid target)
    {
        var capilComp = EnsureComp<CapillaryHoldComponent>(target);
        var capillar = new Capillary(false);

        capilComp.Capillars.Add(uid, capillar);
        drip.HaveCapillar = false;
    }

    protected virtual void OnContainerModified(EntityUid uid, DripComponent cabinet, ContainerModifiedMessage args)
    {
        // Only in client
    }
}
