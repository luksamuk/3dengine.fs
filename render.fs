\ render.fs -- General rendering code for 3dengine.fs.
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

: setup-viewport ( -- )
  0e 0e 0e 1e gl-clearcolor
  GL_LEQUAL gl-depthfunc
  GL_SRC_ALPHA GL_ONE_MINUS_SRC_ALPHA gl-blendfunc
  GL_DEPTH_TEST gl-enable
  GL_BLEND gl-enable
  GL_TEXTURE_2D gl-enable
  0 0 WIDTH @ HEIGHT @ gl-viewport
  GL_PROJECTION gl-matrixmode
  gl-loadidentity
  0e INTERNALW INTERNALH 0e -1e 1e gl-ortho
  GL_MODELVIEW gl-matrixmode
  gl-loadidentity ;

require render-topdown.fs
require render-transformed.fs

\ TODO: Replace temporary values with temporary
\ allotted spaces
variable texw
variable texh
variable texcomp
variable pimg
variable mtex
: load-texture ( a -- n )
  to-c-str texw texh texcomp STBI_rgb_alpha stbi-load pimg !
  pimg @ 0= if -1 ." Failed loading texture" exit then
  gl-createtexture mtex !
  mtex @ 0 < if -1 ." Failed texture generation." cr gl-printerror exit then
  GL_TEXTURE_2D mtex @ gl-bindtexture
  GL_TEXTURE_2D GL_TEXTURE_MIN_FILTER GL_LINEAR  gl-texparameteri
  GL_TEXTURE_2D GL_TEXTURE_MAG_FILTER GL_NEAREST gl-texparameteri
  GL_TEXTURE_2D GL_TEXTURE_WRAP_S     GL_REPEAT  gl-texparameteri
  GL_TEXTURE_2D GL_TEXTURE_WRAP_T     GL_REPEAT  gl-texparameteri
  GL_TEXTURE_2D 0 GL_RGBA texw @ texh @ 0 GL_RGBA GL_UNSIGNED_BYTE pimg @ gl-teximage2d
  gl-printerror
  GL_TEXTURE_2D 0 gl-bindtexture
  pimg @ stbi-image-free
  mtex @ ;

require render-perspective.fs
require render-overlay.fs

