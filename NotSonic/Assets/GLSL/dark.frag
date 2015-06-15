

sampler2D texture;

float floorpr(float f, float p)
{
	f *= pow(10, p);
	f = ceil(f);
	f /= pow(10, p);
	return f;
}
 
void main() {
    vec2 pos = gl_TexCoord[0];

	vec4 pixel = texture2D(texture, pos);

	pixel.rgb *= 0.3;


	gl_FragColor = pixel;
	

}