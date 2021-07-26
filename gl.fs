c-library gl
\c #include <GL/gl.h>
\c #include <GL/glu.h>

s" GL" add-lib

c-function gl-viewport     glViewport     n n n n -- void
c-function gl-clear        glClear        n -- void
c-function gl-clearcolor   glClearColor   r r r r -- void
c-function gl-begin        glBegin        n -- void
c-function gl-end          glEnd          -- void
c-function gl-vertex2f     glVertex2f     r r -- void
c-function gl-color3f      glColor3f      r r r -- void
c-function gl-color4f      glColor4f      r r r r -- void
c-function gl-enable       glEnable       n -- void
c-function gl-depthfunc    glDepthFunc    n -- void
c-function gl-blendfunc    glBlendFunc    n n -- void
c-function gl-matrixmode   glMatrixMode   n -- void
c-function gl-loadidentity glLoadIdentity -- void
c-function gl-ortho        glOrtho        r r r r r r -- void

end-c-library

HEX
00000100 CONSTANT GL_DEPTH_BUFFER_BIT
00004000 CONSTANT GL_COLOR_BUFFER_BIT
00000203 CONSTANT GL_LEQUAL
00000B71 CONSTANT GL_DEPTH_TEST
00000BE2 CONSTANT GL_BLEND
00000302 CONSTANT GL_SRC_ALPHA
00000303 CONSTANT GL_ONE_MINUS_SRC_ALPHA

00001700 CONSTANT GL_MODELVIEW
00001701 CONSTANT GL_PROJECTION

0000 CONSTANT GL_POINTS
0001 CONSTANT GL_LINES
0002 CONSTANT GL_LINE_LOOP
0003 CONSTANT GL_LINE_STRIP
0004 CONSTANT GL_TRIANGLES
0005 CONSTANT GL_TRIANGLE_STRIP
0006 CONSTANT GL_TRIANGLE_FAN
0007 CONSTANT GL_QUADS
0008 CONSTANT GL_QUAD_STRIP
0009 CONSTANT GL_POLYGON

DECIMAL
