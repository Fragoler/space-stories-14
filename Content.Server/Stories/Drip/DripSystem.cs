using Content.Shared.Stories.Drip;
using Content.Server.Body.Components;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Server.Chemistry.ReactionEffects;
using Content.Server.Fluids.EntitySystems;
using Content.Server.Forensics;
using Content.Server.HealthExaminable;
using Content.Server.Popups;
using Content.Shared.Alert;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reaction;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Drunk;
using Content.Shared.FixedPoint;
using Content.Shared.IdentityManagement;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;
using Content.Shared.Rejuvenate;
using Content.Shared.Speech.EntitySystems;
using Robust.Server.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

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

            if (!_solutionContainerSystem.ResolveSolution(packedItem, drip.SolutionPackName, ref drip.BloodSolution, out var dripSolution))
                continue;


        }
    }

}
