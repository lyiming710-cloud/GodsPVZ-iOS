#ifndef GODSPVZ_RECOVERED_BRIGHTNESS_INCLUDED
#define GODSPVZ_RECOVERED_BRIGHTNESS_INCLUDED

// PC path ID 871: PS programs 37..52, 56, 60. Android: 16..31, 33, 35.
// Keep the graph's float product before the half output; no UV scale/offset.
float4 RecoveredBrightnessColor(half4 texel, float4 color, float brightness, float transparent)
{
    return texel * float4(brightness, brightness, brightness, transparent) * color;
}

half3 RecoveredBrightnessLight(half3 light, half4 filter, half4 inverted)
{
    // The graph's mask is constant (1,1,1,1). Zero filter disables masking.
    if (dot(filter, filter) != 0.0)
        light *= dot(1.0 - inverted, filter);
    return light;
}

half4 RecoveredBrightnessLit(float4 color, half3 modulate, half3 additive, half hdrScale, half useLighting)
{
    if (color.a == 0.0)
        discard;
    #if USE_SHAPE_LIGHT_TYPE_0 || USE_SHAPE_LIGHT_TYPE_1 || USE_SHAPE_LIGHT_TYPE_2 || USE_SHAPE_LIGHT_TYPE_3
        half3 lit = hdrScale * (color.rgb * modulate + additive);
        half4 output = half4(lerp(color.rgb, lit, useLighting), color.a);
        return max(output, 0.0);
    #else
        return max(color, 0.0);
    #endif
}

half4 RecoveredBrightnessNormal(half4 texel, float3 viewNormal, float transparent)
{
    // Neither vertex alpha nor _RendererColor affects the original normals pass.
    half3 encodedNormal = (half3(viewNormal) + 1.0) * 0.5;
    return half4(encodedNormal, texel.a * transparent);
}
#endif
