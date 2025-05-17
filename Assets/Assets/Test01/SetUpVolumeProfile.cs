using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SetUpVolumeProfile : MonoBehaviour
{
    public VolumeProfile profile;

    void Start()
    {
        if (!profile) return;

        if (!profile.TryGet(out Bloom bloom))
            bloom = profile.Add<Bloom>(true);
        bloom.intensity.Override(0.8f);
        bloom.threshold.Override(1.1f);

        if (!profile.TryGet(out Tonemapping tone))
            tone = profile.Add<Tonemapping>(true);
        tone.mode.Override(TonemappingMode.ACES);

        if (!profile.TryGet(out Vignette vignette))
            vignette = profile.Add<Vignette>(true);
        vignette.intensity.Override(0.4f);
        vignette.smoothness.Override(0.8f);
        vignette.center.Override(new Vector2(0.5f, 0.5f));

        if (!profile.TryGet(out ColorAdjustments color))
            color = profile.Add<ColorAdjustments>(true);
        color.postExposure.Override(-0.5f);
    }
}
