

sampler2D texture;
uniform float freezetime;
uniform float comboamt;
uniform float maxfreeze;

float floorpr(float f, float p)
{
	f *= pow(10, p);
	f = ceil(f);
	f /= pow(10, p);
	return f;
}

vec3 rgb2hsv(vec3 c)
{
    vec4 K = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    vec4 p = mix(vec4(c.bg, K.wz), vec4(c.gb, K.xy), step(c.b, c.g));
    vec4 q = mix(vec4(p.xyw, c.r), vec4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

vec3 hsv2rgb(vec3 c)
{
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}
 
void main() {
    vec2 pos = gl_TexCoord[0];
	vec2 scrpos = gl_FragCoord.xy;

	vec4 pixel = texture2D(texture, pos);

	float freezecoeff = (freezetime / maxfreeze);

	if(comboamt < 5.0f)
	{
		pixel.rgb *= 0.5f;
	}
	if(comboamt >= 5.0f)
	{
		pixel.rgb *= freezecoeff;
	}
	if(comboamt >= 10.0f)
	{
		// hue shifting
		vec3 hsvcol = rgb2hsv(pixel.rgb);
		hsvcol.r += comboamt*0.3f;
		pixel.rgb = (pixel.rgb * (1.0 - freezecoeff)) + hsv2rgb(hsvcol) * freezecoeff;
	}
	if(comboamt >= 15.0f)
	{
		vec3 hsvcol = rgb2hsv(pixel.rgb);
		hsvcol.g = 1.0 - hsvcol.g;
		pixel.rgb = hsv2rgb(hsvcol);
	}
	if(comboamt >= 20.0f)
	{
		pixel.rgb = 1.0 - pixel.rgb;
	}


	gl_FragColor = pixel;
	

}