
#version 120
#extension GL_ARB_arrays_of_arrays : enable
uniform sampler2D texture;
uniform float scale = 720;


float find_closest(int x, int y, float cvalue)
{
	int dither[8][8] = int[8][8](
	int[8]( 0, 32, 8, 40, 2, 34, 10, 42), 
	int[8](48, 16, 56, 24, 50, 18, 58, 26), 
	int[8](12, 44, 4, 36, 14, 46, 6, 38), 
	int[8](60, 28, 52, 20, 62, 30, 54, 22), 
	int[8]( 3, 35, 11, 43, 1, 33, 9, 41), 
	int[8](51, 19, 59, 27, 49, 17, 57, 25),
	int[8](15, 47, 7, 39, 13, 45, 5, 37),
	int[8](63, 31, 55, 23, 61, 29, 53, 21) ); 
	
	float limit = 0.0;
	if(x < 8)
	{
		limit = (dither[x][y]+1)/64.0;
	}
	
	if(cvalue < limit)
	{
		return 0.0;
	}
	
	return 1.0;
}


void main() {
    vec2 pos = gl_TexCoord[0].xy;

	vec4 pixel = texture2D(texture, pos);
	
	vec4 finalcol;
	vec2 xy = pos * scale;
	int x = int(mod(xy.x, 8));
	int y = int(mod(xy.y, 8));
	
	finalcol.r = find_closest(x, y, pixel.r);
	finalcol.g = find_closest(x, y, pixel.g);
	finalcol.b = find_closest(x, y, pixel.b);
	finalcol.a = 1.0;
	

	gl_FragColor = finalcol;
	

}