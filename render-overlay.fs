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

