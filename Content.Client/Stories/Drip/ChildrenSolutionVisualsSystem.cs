using Content.Shared.Chemistry;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Rounding;
using Robust.Client.GameObjects;
using Robust.Shared.Prototypes;

namespace Content.Client.Chemistry.Visualizers;

public sealed class ChildrenSolutionVisualsSystem : VisualizerSystem<ChildrenSolutionVisualsComponent>
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ChildrenSolutionVisualsComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(EntityUid uid, ChildrenSolutionVisualsComponent component, MapInitEvent args)
    {
        var meta = MetaData(uid);
        component.InitialName = meta.EntityName;
        component.InitialDescription = meta.EntityDescription;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = AllEntityQuery<ChildrenSolutionVisualsComponent>();

        while (query.MoveNext(out var uid, out var childrenVisual))
        {

            if (!TryComp<SpriteComponent>(uid, out var spriteComp))
                continue;


            if (!spriteComp.LayerMapTryGet(childrenVisual.Layer, out var fillLayer))
                continue;


            if (childrenVisual.СhildrenItem is null)
            {
                childrenVisual.Fraction = null;
                if (childrenVisual.EmptySpriteName == null)
                    spriteComp.LayerSetVisible(fillLayer, false);
                else
                {
                    spriteComp.LayerSetState(fillLayer, childrenVisual.EmptySpriteName);
                    if (childrenVisual.ChangeColor)
                        spriteComp.LayerSetColor(fillLayer, childrenVisual.EmptySpriteColor);
                }
                continue;
            }

            var item = (EntityUid) childrenVisual.СhildrenItem;


            if (!AppearanceSystem.TryGetData<float>(item, SolutionContainerVisuals.FillFraction, out var fraction) || childrenVisual.Fraction == fraction)
                continue;

            // Currently some solution methods such as overflowing will try to update appearance with a
            // volume greater than the max volume. We'll clamp it so players don't see
            // a giant error sign and error for debug.
            if (fraction > 1f)
            {
                fraction = 1f;
            }

            childrenVisual.Fraction = fraction;

            if (childrenVisual.Metamorphic)
            {
                if (spriteComp.LayerMapTryGet(childrenVisual.BaseLayer, out var baseLayer))
                {
                    var hasOverlay = spriteComp.LayerMapTryGet(childrenVisual.OverlayLayer, out var overlayLayer);

                    if (AppearanceSystem.TryGetData<string>(item, SolutionContainerVisuals.BaseOverride, out var baseOverride))
                    {
                        _prototype.TryIndex<ReagentPrototype>(baseOverride, out var reagentProto);

                        if (reagentProto?.MetamorphicSprite is { } sprite)
                        {
                            spriteComp.LayerSetSprite(baseLayer, sprite);
                            spriteComp.LayerSetVisible(fillLayer, false);
                            if (hasOverlay)
                                spriteComp.LayerSetVisible(overlayLayer, false);
                            continue;
                        }
                        else
                        {
                            if (hasOverlay)
                                spriteComp.LayerSetVisible(overlayLayer, true);
                            if (childrenVisual.MetamorphicDefaultSprite != null)
                                spriteComp.LayerSetSprite(baseLayer, childrenVisual.MetamorphicDefaultSprite);
                        }
                    }
                }
            }

            var closestFillSprite = ContentHelpers.RoundToLevels(fraction, 1, childrenVisual.MaxFillLevels + 1);

            if (closestFillSprite > 0)
            {
                if (childrenVisual.FillBaseName == null)
                    return;

                spriteComp.LayerSetVisible(fillLayer, true);

                var stateName = childrenVisual.FillBaseName + closestFillSprite;
                spriteComp.LayerSetState(fillLayer, stateName);

                if (childrenVisual.ChangeColor && AppearanceSystem.TryGetData<Color>(item, SolutionContainerVisuals.Color, out var color))
                    spriteComp.LayerSetColor(fillLayer, color);
            }
            else
            {
                if (childrenVisual.EmptySpriteName == null)
                    spriteComp.LayerSetVisible(fillLayer, false);
                else
                {
                    spriteComp.LayerSetState(fillLayer, childrenVisual.EmptySpriteName);
                    if (childrenVisual.ChangeColor)
                        spriteComp.LayerSetColor(fillLayer, childrenVisual.EmptySpriteColor);
                }
            }
        }
    }

}
