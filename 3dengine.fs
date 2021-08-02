\ 3dengine.fs -- A Doom-like 3D renderer, written from scratch.
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

require sdl.fs
require gl.fs
require stbi.fs

variable WIDTH
variable HEIGHT
variable RUNNING

100e fconstant INTERNALW
100e fconstant INTERNALH

variable TEXTURE
variable WALLS

variable WINDOW
variable LOCAL_CONTEXT

\ Gameplay variables
variable MAPTYPE
variable MWIREFRAME

\ player variables
variable PX
variable PY
variable PANGLE

\ Transform variables
variable tx1
variable ty1
variable tx2
variable ty2
variable tz1
variable tz2
variable x1
variable y1a
variable y1b
variable x2
variable y2a
variable y2b
variable ix1
variable iz1
variable ix2
variable iz2

require render.fs
require input.fs
require utils.fs


( Initialization )

: init-game ( -- )
  600 WIDTH !
  600 HEIGHT !
  0 MAPTYPE !
  false INPFWD !
  false INPBACK !
  false INPLSTRF !
  false INPRSTRF !
  false INPLROT !
  false INPRROT !
  50e PX f!
  50e PY f!
  0e PANGLE f!
  true RUNNING !
  false MWIREFRAME ! ;

: open-window ( -- )
  SDL_INIT_VIDEO SDL_INIT_GAMECONTROLLER OR
  sdl-init

  s" Teste 3D" to-c-str
  SDL_WINDOWPOS_CENTERED SDL_WINDOWPOS_CENTERED
  WIDTH @ HEIGHT @
  SDL_WINDOW_SHOWN SDL_WINDOW_OPENGL OR
  sdl-createwindow WINDOW !

  WINDOW @ sdl-gl-createcontext LOCAL_CONTEXT !
  1 sdl-gl-setswapinterval drop

  \ Attributes
  SDL_GL_CONTEXT_PROFILE_MASK SDL_GL_CONTEXT_PROFILE_CORE
  SDL_GL_CONTEXT_MAJOR_VERSION 3
  SDL_GL_CONTEXT_MINOR_VERSION 1
  SDL_GL_RED_SIZE      8
  SDL_GL_GREEN_SIZE    8
  SDL_GL_BLUE_SIZE     8
  SDL_GL_ALPHA_SIZE    8
  SDL_GL_BUFFER_SIZE  32
  SDL_GL_DOUBLEBUFFER  1
  SDL_GL_DEPTH_SIZE   24
  SDL_GL_SHARE_WITH_CURRENT_CONTEXT 1
  11 0 do sdl-gl-setattribute drop loop drop

  setup-viewport ;

40 constant WALLFLOATS#
: generate-walls ( v -- )
  dup WALLFLOATS# floats allocate 0 = if
    swap ! @
   ( start   end      tex-t    color )
   ( ix iy   fx  fy   ft    r  g  b )
    70e 20e  70e 70e  50e   1e 1e 0e \ Yellow
    40e 90e  70e 70e  36e   1e 0e 1e \ Magenta
    20e 80e  40e 90e  22e   1e 0e 0e \ Red
    20e 80e  50e 10e  76e   0e 1e 1e \ Cyan
    50e 10e  70e 20e  22e   0e 1e 0e \ Green
    WALLFLOATS# store-fvec
  else 2drop drop then ;

: init-assets ( -- )
  s" texture/wall.png" load-texture TEXTURE !
  WALLS generate-walls ;



( Game running )

: clear-window ( -- )
  GL_COLOR_BUFFER_BIT GL_DEPTH_BUFFER_BIT OR gl-clear ;

: swap-window ( -- )
  WINDOW @ sdl-gl-swapwindow ;

: paint-window ( -- )
  clear-window
  MAPTYPE @ case
    0 of draw-map-topdown     endof
    1 of draw-map-transformed endof
    2 of draw-map-perspective endof
    3 of
      draw-map-perspective
      draw-overlay
      draw-map-transformed endof
  endcase
  swap-window ;

: move-player ( -- )
  INPFWD @ if
    PX dup f@ PANGLE f@ fcos f+ f!
    PY dup f@ PANGLE f@ fsin f+ f! then
  INPBACK @ if
    PX dup f@ PANGLE f@ fcos f- f!
    PY dup f@ PANGLE f@ fsin f- f! then
  INPLSTRF @ if
    PX dup f@ PANGLE f@ fsin f+ f!
    PY dup f@ PANGLE f@ fcos f- f! then
  INPRSTRF @ if
    PX dup f@ PANGLE f@ fsin f- f!
    PY dup f@ PANGLE f@ fcos f+ f! then
  INPLROT @ if PANGLE f@ 0.1e f- PANGLE f! then
  INPRROT @ if PANGLE f@ 0.1e f+ PANGLE f! then ;

: gameloop ( -- )
  begin RUNNING @ while    
    update-input
    move-player
    paint-window
  repeat ;


( Dispose game )

: dispose-assets ( -- )
  TEXTURE dup @ gl-disposetexture -1 swap ! ;

: close-window ( -- )
  LOCAL_CONTEXT @ sdl-gl-deletecontext
  WINDOW @ sdl-destroywindow
  0 WINDOW 0 LOCAL_CONTEXT ! !
  sdl-quit ;

: dispose-game ( -- )
  WALLS dup @  free drop 0 swap ! ;



( Entry point )

: run ( -- )
  init-game open-window init-assets
  gameloop
  dispose-assets close-window dispose-game ;

