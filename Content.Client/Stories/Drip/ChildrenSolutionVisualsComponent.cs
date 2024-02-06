using Robust.Shared.Utility;

namespace Content.Shared.Chemistry.Components
{
    [RegisterComponent]
    public sealed partial class ChildrenSolutionVisualsComponent : Component
    {
        [DataField]
        public EntityUid? СhildrenItem = null;
        public SolutionContainerVisuals? SolutionVisualComp = null;

        [DataField]
        public int MaxFillLevels = 0;
        [DataField]
        public string? FillBaseName = null;
        [DataField]
        public SolutionContainerLayers Layer = SolutionContainerLayers.Fill;
        [DataField]
        public SolutionContainerLayers BaseLayer = SolutionContainerLayers.Base;
        [DataField]
        public SolutionContainerLayers OverlayLayer = SolutionContainerLayers.Overlay;
        [DataField]
        public bool ChangeColor = true;
        [DataField]
        public string? EmptySpriteName = null;
        [DataField]
        public Color EmptySpriteColor = Color.White;
        [DataField]
        public bool Metamorphic = false;
        [DataField]
        public SpriteSpecifier? MetamorphicDefaultSprite;
        [DataField]
        public LocId MetamorphicNameFull = "transformable-container-component-glass";

        [DataField]
        public string InitialName = string.Empty;

        [DataField]
        public string InitialDescription = string.Empty;
    }
}
