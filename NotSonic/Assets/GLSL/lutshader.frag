

sampler2D texture;
sampler2D lut;



vec4 sampleAs3DTexture(sampler2D texture, vec3 uv, float width) {
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
 
void main() {
    vec2 pos = gl_TexCoord[0];
	vec4 pixel = texture2D(texture, pos);
	vec4 gradpix = sampleAs3DTexture(lut, pixel.rgb, 16.0f);
	gradpix.a = pixel.a;
	pixel = gradpix;

 
    gl_FragColor = gradpix;
}