#ifndef GODSPVZ_RECOVERED_GAME_UNIT_INCLUDED
#define GODSPVZ_RECOVERED_GAME_UNIT_INCLUDED

// Bound to PC DXBC programs 1/3 and Android GLES program 1 (path ID 886).
// Angles are radians. The original vertex program ignores input Z and W.
float2 RecoveredGameUnitXY(float2 position, float angleX, float angleY, float scaleX, float scaleY)
{
    float2 scaled = position * float2(scaleX, scaleY);
    float skewX = scaled.y * sin(angleX - angleY) + scaled.x;
    float skewY = cos(angleX - angleY) * scaled.y;
    float sinY = sin(-angleY);
    float cosY = cos(-angleY);
    return float2(skewX * cosY - skewY * sinY, skewX * sinY + skewY * cosY);
}

float4 RecoveredGameUnitColor(half4 texel, float4 color, float exposure, float alpha, float isVisible)
{
    // The hidden branch returns sampled RGB with alpha 0; it does not discard.
    if (isVisible == -1.0)
        return float4(texel.rgb, 0.0);
    return texel * color * float4(exposure, exposure, exposure, alpha);
}
#endif
