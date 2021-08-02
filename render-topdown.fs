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

