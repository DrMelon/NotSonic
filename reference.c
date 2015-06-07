int obstacle_get_height_at(const obstacle_t *obstacle, int position_on_base_axis, obstaclebaselevel_t base_level)
{
    uint32 mask = video_get_maskcolor();
    int w = obstacle_get_width(obstacle);
    int h = obstacle_get_height(obstacle);
    const image_t *image = obstacle_get_image(obstacle);
    int x, y;

    if(NULL == image)
        return 0;

    switch(base_level) {

        /* will return the height counting from the left side to the right side of the obstacle */
        /* +---------------+ */
        /* |               / */
        /* | ----->        \ */
        /* |               / */
        /* +--------------+  */
        case FROM_LEFT:
            if(position_on_base_axis < 0 || position_on_base_axis >= h)
                return 0;

            y = position_on_base_axis;
            for(x=w-1; x>=0; x--) {
                if(image_getpixel(image, x, y) != mask)
                    break;
            }

            return x;

        /* will return the height counting from the top side to the bottom side of the obstacle */
        /*  +-----------------+  */
        /*  |         |       |  */
        /*  |        \|/      |  */
        /*  |                 |  */
        /*  |   __      ____  |  */
        /*  \__/  \_/\_/    \_/  */
        case FROM_TOP:
            if(position_on_base_axis < 0 || position_on_base_axis >= w)
                return 0;

            x = position_on_base_axis;
            for(y=h-1; y>=0; y--) {
                if(image_getpixel(image, x, y) != mask)
                    break;
            }

            return y;

        /* will return the height counting from the right side to the left side of the obstacle */
        /* +---------------+ */
        /* \               | */
        /* /        <----- | */
        /* \               | */
        /* +---------------+ */
        case FROM_RIGHT:
            if(position_on_base_axis < 0 || position_on_base_axis >= h)
                return 0;

            y = position_on_base_axis;
            for(x=0; x<w; x++) {
                if(image_getpixel(image, x, y) != mask)
                    break;
            }

            return w-x;

        /* will return the height counting from the bottom side to the top side of the obstacle */
        /*   __    __     _  _   */
        /*  /  \__/  \___/ \/ \  */
        /*  |                 |  */
        /*  |                 |  */
        /*  |      /|\        |  */
        /*  |       |         |  */
        /*  +-----------------+  */
        case FROM_BOTTOM:
            if(position_on_base_axis < 0 || position_on_base_axis >= w)
                return 0;

            x = position_on_base_axis;
            for(y=0; y<h; y++) {
                if(image_getpixel(image, x, y) != mask)
                    break;
            }

            return h-y;
    }

    /* this shouldn't happen */
    return 0;
}