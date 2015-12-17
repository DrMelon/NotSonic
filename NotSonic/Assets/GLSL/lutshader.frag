#version 120

uniform sampler2D texture;
uniform sampler2D lut;
uniform float time;
uniform float belowwater;
uniform float cutoff;

vec4 sampleAs3Dtexture2D(sampler2D texture, vec3 uv, float width) {
    float sliceSize = 1.0 / width;              // space of 1 slice
    float slicePixelSize = sliceSize / width;           // space of 1 pixel
    float sliceInnerSize = slicePixelSize * (width - 1.0);  // space of width pixels
    float zSlice0 = min(floor(uv.z * width), width - 1.0);
    float zSlice1 = min(zSlice0 + 1.0, width - 1.0);
    float xOffset = slicePixelSize * 0.5 + uv.x * sliceInnerSize;
    float s0 = xOffset + (zSlice0 * sliceSize);
    float s1 = xOffset + (zSlice1 * sliceSize);
    vec4 slice0Color = texture2D(texture, vec2(s0, uv.y));
    vec4 slice1Color = texture2D(texture, vec2(s1, uv.y));
    float zOffset = mod(uv.z * width, 1.0);
    vec4 result = mix(slice0Color, slice1Color, zOffset);
    return result;
}

float floorpr(float f, float p)
{
	f *= pow(10, p);
	f = ceil(f);
	f /= pow(10, p);
	return f;
}
 
void main() {
    vec2 pos = gl_TexCoord[0].xy;
	vec2 wavepos = pos;
	if(belowwater - pos.y > cutoff)
	{
		wavepos.x += 0.01 * sin(time * 0.03f + wavepos.y * 10);
	}
	vec4 pixel = texture2D(texture, wavepos);
	vec4 gradpix = sampleAs3Dtexture2D(lut, pixel.rgb, 16.0f);
	gradpix.a = pixel.a;
	

	if(belowwater - pos.y > cutoff)
	{
		gl_FragColor = gradpix;
	}
	else
	{
		gl_FragColor = pixel;
	}

}