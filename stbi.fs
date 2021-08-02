\ stbi.fs -- Bindings for stb_image.h for 3dengine.fs.
\ Copyright (C) 2021 Lucas S. Vieira
\
\ This program is free software: you can redistribute it and/or modify
\ it under the terms of the GNU General Public License as published by
\ the Free Software Foundation, either version 3 of the License, or
\ (at your option) any later version.
\
\ This program is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied warranty of
\ MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
\ GNU General Public License for more details.
\
\ You should have received a copy of the GNU General Public License
\ along with this program.  If not, see <https://www.gnu.org/licenses/>.

\ Modeled after stb_image.h v2.27
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

