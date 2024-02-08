using Content.Shared.Stories.Drip;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Server.Fluids.EntitySystems;
using Content.Server.Forensics;
using Content.Shared.Alert;
using Content.Shared.Chemistry.Components;
using Content.Shared.Damage;
using Content.Shared.Drunk;
using Content.Shared.FixedPoint;
using Content.Shared.Mobs.Systems;
using Content.Shared.Speech.EntitySystems;
using Robust.Server.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Content.Server.Popups;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Chemistry;
using Content.Shared.Forensics;

namespace Content.Server.Stories.Drip;

public sealed class DripSystem : SharedDripSystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IRobustRandom _robustRandom = default!;
    [Dependency] private readonly AudioSystem _audio = default!;
    [Dependency] private readonly DamageableSystem _damageableSystem = default!;
    [Dependency] private readonly PopupSystem _popupSystem = default!;
    [Dependency] private readonly PuddleSystem _puddleSystem = default!;
    [Dependency] private readonly MobStateSystem _mobStateSystem = default!;
    [Dependency] private readonly SharedDrunkSystem _drunkSystem = default!;
    [Dependency] private readonly SolutionContainerSystem _solutionContainerSystem = default!;
    [Dependency] private readonly SharedStutteringSystem _stutteringSystem = default!;
    [Dependency] private readonly AlertsSystem _alertsSystem = default!;
    [Dependency] private readonly ForensicsSystem _forensicsSystem = default!;
    [Dependency] private readonly ReactiveSystem _reactiveSystem = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<DripComponent>();
        while (query.MoveNext(out var uid, out var drip))
        {
            drip.AccumulatedFrametime += frameTime;

            if (drip.AccumulatedFrametime < drip.UpdateInterval)
                continue;

            drip.AccumulatedFrametime -= frameTime;

            if (drip.DripPackedSlot.ContainerSlot is null || drip.DripPackedSlot.ContainerSlot.ContainedEntity is null)
            {
                continue;
            }

            var packedItem = (EntityUid) drip.DripPackedSlot.ContainerSlot.ContainedEntity;

            if (drip.ConnectedEnt is null)
            {
                continue;
            }

            TryInject(uid, drip, packedItem, (EntityUid) drip.ConnectedEnt);
        }
    }

    private void TryInject(EntityUid uid, DripComponent drip, EntityUid packedItem, EntityUid target)
    {
        if (!_solutionContainerSystem.ResolveSolution(packedItem, drip.SolutionPackName, ref drip.PackSolution, out var packSolution))
            return;

        if (!_solutionContainerSystem.TryGetInjectableSolution(target, out var targetSoln, out var targetSolution))
        {
            return;
        }

        if (targetSoln is null || drip.PackSolution is null)
        {
            return;
        }

        var realTransferAmount = FixedPoint2.Min(drip.TransferAmount, targetSolution.AvailableVolume);

        var removedSolution = _solutionContainerSystem.SplitSolution((Entity<SolutionComponent>) drip.PackSolution, realTransferAmount);
        if (targetSolution.CanAddSolution(removedSolution))
            return;
        _reactiveSystem.DoEntityReaction(target, removedSolution, ReactionMethod.Injection);
        _solutionContainerSystem.TryAddSolution((Entity<SolutionComponent>) targetSoln, removedSolution);

        var ev = new TransferDnaEvent { Donor = packedItem, Recipient = target };
        RaiseLocalEvent(target, ref ev);
    }
}
