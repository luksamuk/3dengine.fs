\ gl.fs -- OpenGL bindings for 3dengine.fs.
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
c-function gl-pushmatrix   glPushMatrix   -- void
c-function gl-popmatrix    glPopMatrix    -- void
c-function gl-translatef   glTranslatef   r r r -- void
c-function gl-rotatef      glRotatef      r r r r -- void
c-function gl-pointsize    glPointSize    r -- void
c-function gl-polygonmode  glPolygonMode  n n -- void

c-function gl-gentextures    glGenTextures    n a -- void
c-function gl-bindtexture    glBindTexture    n n -- void
c-function gl-texparameteri  glTexParameteri  n n n -- void
c-function gl-teximage2d     glTexImage2D     n n n n n n n n a -- void
c-function gl-deletetextures glDeleteTextures n a -- void
c-function gl-texcoord2f     glTexCoord2f     r r -- void

c-function gl-genbuffers     glGenBuffers     n a -- void
c-function gl-bindbuffer     glBindBuffer     n n -- void
c-function gl-bufferdata     glBufferData     n n a n -- void

c-function gl-createshader     glCreateShader      n -- n
c-function gl-shadersource     glShaderSource      n n a a -- void
c-function gl-compileshader    glCompileShader     n -- void
c-function gl-getshaderiv      glGetShaderiv       n n a -- void
c-function gl-getshaderinfolog glGetShaderInfoLog  n n a a -- void

c-function gl-createprogram        glCreateProgram         -- void
c-function gl-attachshader         glAttachShader          n n -- void
c-function gl-bindfragdatalocation glBindFragDataLocation  n n a -- void
c-function gl-linkprogram          glLinkProgram           n -- void
c-function gl-useprogram           glUseProgram            n -- void
c-function gl-getattriblocation    glGetAttribLocation     n a -- n
c-function gl-vertexattribpointer  glVertexAttribPointer   n n n n n n -- void
c-function gl-enablevertexattribarray glEnableVertexAttribArray n -- void

c-function gl-genvertexarrays      glGenVertexArrays       n a -- void
c-function gl-bindvertexarray      glBindVertexArray       n -- void
c-function gl-drawarrays           glDrawArrays            n n n -- void

c-function gl-getuniformlocation   glGetUniformLocation    n a -- n
c-function gl-uniform3f            glUniform3f             n r r r -- void
c-function gl-uniform4f            glUniform4f             n r r r r -- void

c-function gl-drawelements         glDrawElements          n n n n -- void

c-function gl-geterror      glGetError       -- n
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

0408 CONSTANT GL_FRONT_AND_BACK

1B01 CONSTANT GL_LINE
1B02 CONSTANT GL_FILL

0DE1 CONSTANT GL_TEXTURE_2D
2802 CONSTANT GL_TEXTURE_WRAP_S
2803 CONSTANT GL_TEXTURE_WRAP_T
2800 CONSTANT GL_TEXTURE_MAG_FILTER
2801 CONSTANT GL_TEXTURE_MIN_FILTER
2600 CONSTANT GL_NEAREST
2601 CONSTANT GL_LINEAR
812F CONSTANT GL_CLAMP_TO_EDGE
2901 CONSTANT GL_REPEAT

1907 CONSTANT GL_RGB
1908 CONSTANT GL_RGBA

1401 CONSTANT GL_UNSIGNED_BYTE

0000 CONSTANT GL_NO_ERROR
0500 CONSTANT GL_INVALID_ENUM
0501 CONSTANT GL_INVALID_VALUE
0502 CONSTANT GL_INVALID_OPERATION
0503 CONSTANT GL_STACK_OVERFLOW
0504 CONSTANT GL_STACK_UNDERFLOW
0505 CONSTANT GL_OUT_OF_MEMORY

DECIMAL

: gl-printerror ( -- )
  gl-geterror
  dup GL_NO_ERROR = if drop exit then
  ." OpenGL error: "
  case
    GL_INVALID_ENUM  of ." Invalid enumeration" endof
    GL_INVALID_VALUE of ." Invalid value" endof
    GL_INVALID_OPERATION of ." Invalid operation" endof
    GL_STACK_OVERFLOW of ." Stack overflow" endof
    GL_STACK_UNDERFLOW of ." Stack underflow" endof
    GL_OUT_OF_MEMORY of ." Out of memory" endof
  endcase cr ;

: gl-createtexture ( -- n )
  1 cells allot
  1 here -1 cells + gl-gentextures
  here -1 cells + @
  -1 cells allot ;

: gl-disposetexture ( n -- )
  1 cells allot
  here -1 cells + !
  1 here -1 cells + gl-deletetextures
  -1 cells allot ;

