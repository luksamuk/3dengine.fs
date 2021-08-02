\ render-perspective.fs -- Perspective rendering for 3dengine.fs.
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

require utils.fs
require gl.fs

15e fconstant WALLTILE#
0.5e fconstant WALLTEXEL_S

variable vtmp \ temp vector addr

: draw-wall-perspective ( n -- )
  8 *
  dup wall@ PX f@ f- tx1 f!     \ tx1
  dup 1+ wall@ PY f@ f- ty1 f!  \ ty1
  dup 2 + wall@ PX f@ f- tx2 f! \ tx2
  dup 3 + wall@ PY f@ f- ty2 f! \ ty2
  tx1 f@ PANGLE f@ fcos f* ty1 f@ PANGLE f@ fsin f* f+ tz1 f!
  tx2 f@ PANGLE f@ fcos f* ty2 f@ PANGLE f@ fsin f* f+ tz2 f!
  tx1 f@ PANGLE f@ fsin f* ty1 f@ PANGLE f@ fcos f* f- tx1 f!
  tx2 f@ PANGLE f@ fsin f* ty2 f@ PANGLE f@ fcos f* f- tx2 f!

  tz1 f@ 0e f> tz2 f@ 0e f> or if
    here vtmp ! 8 floats allot
    tx1 f@ tz1 f@ tx2 f@ tz2 f@ -0.0001e 0.0001e -20e 5e
    vtmp 8 store-fvec
    vtmp intersect fdrop
    intersect-x f@ ix1 f!
    intersect-y f@ iz1 f!
    
    tx1 f@ tz1 f@ tx2 f@ tz2 f@ 0.0001e 0.0001e 20e 5e
    vtmp 8 store-fvec
    vtmp intersect fdrop
    intersect-x f@ ix2 f!
    intersect-y f@ iz2 f!
    -8 floats allot

    tz1 f@ 0e f<= if
      iz1 f@ 0e f>
      if   ix1 f@ tx1 f! iz1 f@ tz1 f!
      else ix2 f@ tx1 f! iz2 f@ tz1 f! then
    then

    tz2 f@ 0e f<= if
      iz1 f@ 0e f>
      if   ix1 f@ tx2 f! iz1 f@ tz2 f!
      else ix2 f@ tx2 f! iz2 f@ tz2 f! then
    then

    tx1 f@ -1e f* 16e f* tz1 f@ f/ x1 f!
    -50e tz1 f@ f/ y1a f!
    50e tz1 f@ f/ y1b f!
    tx2 f@ -1e f* 16e f* tz2 f@ f/ x2 f!
    -50e tz2 f@ f/ y2a f!
    50e tz2 f@ f/ y2b f!

    gl-pushmatrix
    MAPTYPE @ 3 = if
      dup 5 + wall@ dup 6 + wall@ dup 7 + wall@ 1e gl-color4f
    else
      1e 1e 1e 1e gl-color4f then
    GL_TEXTURE_2D TEXTURE @ gl-bindtexture
    GL_QUADS gl-begin

    \ bottom left
    0e
    WALLTEXEL_S
    gl-texcoord2f
    
    50e x1 f@ f+
    50e y1a f@ f+
    gl-vertex2f

    \ bottom right
    dup 4 + wall@ WALLTILE# f/ 1.0e f* ( todo: change to percent )
    WALLTEXEL_S
    gl-texcoord2f
    
    50e x2 f@ f+
    50e y2a f@ f+
    gl-vertex2f

    \ top right
    dup 4 + wall@ WALLTILE# f/ 1.0e f* ( todo: change to percent )
    0e
    gl-texcoord2f
    
    50e x2 f@ f+
    50e y2b f@ f+
    gl-vertex2f

    \ top left
    0e
    0e
    gl-texcoord2f

    50e x1 f@ f+
    50e y1b f@ f+
    gl-vertex2f

    gl-end
    GL_TEXTURE_2D 0 gl-bindtexture
    gl-popmatrix
  then drop ;

: draw-map-perspective ( -- )
  5 0 do I draw-wall-perspective loop ;

