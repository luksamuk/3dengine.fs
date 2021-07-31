c-library stbi
\c #define STB_IMAGE_IMPLEMENTATION
\c #include "stb_image.h"

c-function stbi-load       stbi_load        a a a a n -- a
c-function stbi-image-free stbi_image_free  a -- void

end-c-library

0 constant STBI_default
1 constant STBI_grey
2 constant STBI_grey_alpha
3 constant STBI_rgb
4 constant STBI_rgb_alpha

