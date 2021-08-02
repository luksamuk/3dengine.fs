\ overlay.fs -- Overlay rendering bits for 3dengine.fs.
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

: draw-overlay ( -- )
  MWIREFRAME @ if GL_FRONT_AND_BACK GL_FILL gl-polygonmode then
  gl-pushmatrix
  0e 0e 0e 0.7e gl-color4f
  GL_QUADS gl-begin
  0e 0e gl-vertex2f
  INTERNALW 0e gl-vertex2f
  INTERNALW INTERNALH gl-vertex2f
  0e INTERNALH gl-vertex2f
  gl-end
  gl-popmatrix
  MWIREFRAME @ if GL_FRONT_AND_BACK GL_LINE gl-polygonmode then ;

