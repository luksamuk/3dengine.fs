\ render-transformed.fs -- Transformed rendering for 3dengine.fs.
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

: draw-wall-transformed ( n -- )
  8 *
  dup wall@ PX f@ f- tx1 f!     \ tx1
  dup 1+ wall@ PY f@ f- ty1 f!  \ ty1
  dup 2 + wall@ PX f@ f- tx2 f! \ tx2
  dup 3 + wall@ PY f@ f- ty2 f! \ ty2
  tx1 f@ PANGLE f@ fcos f* ty1 f@ PANGLE f@ fsin f* f+ tz1 f!
  tx2 f@ PANGLE f@ fcos f* ty2 f@ PANGLE f@ fsin f* f+ tz2 f!
  tx1 f@ PANGLE f@ fsin f* ty1 f@ PANGLE f@ fcos f* f- tx1 f!
  tx2 f@ PANGLE f@ fsin f* ty2 f@ PANGLE f@ fcos f* f- tx2 f!

  gl-pushmatrix
  dup 5 + wall@ dup 6 + wall@ dup 7 + wall@ 1e gl-color4f
  GL_LINES gl-begin
  50e tx1 f@ f- 50e tz1 f@ f- gl-vertex2f
  50e tx2 f@ f- 50e tz2 f@ f- gl-vertex2f
  gl-end
  gl-popmatrix
  drop ;

: draw-map-transformed ( -- )
  3e gl-pointsize
  5 0 do I draw-wall-transformed loop
  gl-pushmatrix
  0.3e 0.3e 0.3e 1e gl-color4f
  GL_LINES gl-begin
  50e 50e gl-vertex2f
  50e 45e gl-vertex2f
  gl-end
  1e 1e 1e 1e gl-color4f
  GL_POINTS gl-begin
  50e 50e gl-vertex2f
  gl-end
  gl-popmatrix
  1e gl-pointsize ;

