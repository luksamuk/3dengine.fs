\ render-topdown.fs -- Top-down rendering for 3dengine.fs.
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

: draw-wall-topdown ( n -- )
  8 *
  gl-pushmatrix
  dup 5 + wall@ dup 6 + wall@ dup 7 + wall@ 1e gl-color4f
  GL_LINES gl-begin
  dup wall@ dup 1+ wall@ gl-vertex2f
  dup 2 + wall@ dup 3 + wall@ gl-vertex2f
  gl-end
  gl-popmatrix
  drop ;

: draw-map-topdown ( -- )
  3e gl-pointsize
  5 0 do I draw-wall-topdown loop
  gl-pushmatrix
  0.3e 0.3e 0.3e 1e gl-color4f
  GL_LINES gl-begin
  PX f@ PY f@ gl-vertex2f
  PANGLE f@ fcos 5e f* PX f@ f+
  PANGLE f@ fsin 5e f* PY f@ f+ gl-vertex2f
  gl-end
  1e 1e 1e 1e gl-color4f
  GL_POINTS gl-begin
  PX f@ PY f@ gl-vertex2f
  gl-end
  gl-popmatrix
  1e gl-pointsize ;

