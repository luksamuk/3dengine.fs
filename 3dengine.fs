require sdl.fs
require gl.fs
require stbi.fs

100e fconstant INTERNALW
100e fconstant INTERNALH

variable TEXTURE
variable WALLS

variable WIDTH
variable HEIGHT

variable WINDOW
variable LOCAL_CONTEXT
variable RUNNING
sdl-event E

\ input
variable INPFWD   \ Forward
variable INPBACK  \ Backward
variable INPLSTRF \ Strafe left
variable INPRSTRF \ Strafe right
variable INPLROT  \ Rotate left
variable INPRROT  \ Rotate right

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

: reload ( -- )
  s" 3dengine.fs" included ;

: to-c-str ( a u -- a )
  pad swap dup >r move 0 pad r> + c! pad ;

: from-c-str ( a -- a u )
  0 begin 2dup + c@ while 1+ repeat ;

\ Store floats on allotted vector from
\ top to bottom, backwards, so they can
\ be written sequentially
: store-fvec ( a n -- ) ( r1 ... rn -- )
  swap over 1- floats + swap
  0 do dup f! float - loop drop ;

: generate-walls ( v -- )
  dup 40 floats allocate 0 = if
    swap ! @
   ( start   end      tex-t    color )
   ( ix iy   fx  fy   ft    r  g  b )
    70e 20e  70e 70e  50e   1e 1e 0e \ Yellow
    40e 90e  70e 70e  36e   1e 0e 1e \ Magenta
    20e 80e  40e 90e  22e   1e 0e 0e \ Red
    20e 80e  50e 10e  76e   0e 1e 1e \ Cyan
    50e 10e  70e 20e  22e   0e 1e 0e \ Green
    40 store-fvec
  else 2drop drop then ;


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


: close-window ( -- )
  LOCAL_CONTEXT @ sdl-gl-deletecontext
  WINDOW @ sdl-destroywindow
  0 WINDOW 0 LOCAL_CONTEXT ! !
  sdl-quit ;

: clear-window ( -- )
  GL_COLOR_BUFFER_BIT GL_DEPTH_BUFFER_BIT OR gl-clear ;
  
: swap-window ( -- )
  WINDOW @ sdl-gl-swapwindow ;


: fvec@ ( a i -- ) ( -- r )
  floats + f@ ;

: wall@ ( i -- ) ( -- r )
  WALLS @ swap fvec@ ;

\ Top down map
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

\ Transformed map
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

\ Perspective map
: cross ( -- ) ( rx1 ry1 rx2 ry2 -- r )
  fswap frot f* frot frot f* fswap f- ;

variable intersectx  \ intersect x point output
variable intersecty  \ intersect y point output
: intersect ( a -- ) ( -- r )
  \ a is a ptr to float vector containing:
  \ x1, y1, x2, y2, x3, y3, x4, y4
  dup 0 fvec@ \ x1
  dup 1 fvec@ \ y1
  dup 2 fvec@ \ x2
  dup 3 fvec@ \ y2
  cross intersectx f!

  dup 4 fvec@  \ x3
  dup 5 fvec@  \ y3
  dup 6 fvec@  \ x4
  dup 7 fvec@  \ y4
  cross intersecty f!

  dup 0 fvec@ dup 2 fvec@ f-  \ x1 - x2
  dup 1 fvec@ dup 3 fvec@ f-  \ y1 - y2
  dup 4 fvec@ dup 6 fvec@ f-  \ x3 - x4
  dup 5 fvec@ dup 7 fvec@ f-  \ y3 - y4
  cross \ determinant pushed onto float stack

  fdup
  intersectx f@                \ x
  dup 0 fvec@ dup 2 fvec@ f-   \ x1 - x2
  intersecty f@                \ y
  dup 4 fvec@ dup 6 fvec@ f-   \ x3 - x4
  cross fswap f/ intersectx f!

  fdup
  intersectx f@                \ x
  dup 1 fvec@ dup 3 fvec@ f-   \ y1 - y2
  intersecty f@                \ y
  dup 5 fvec@ dup 7 fvec@ f-   \ y3 - y4
  cross fswap f/ intersecty f!
  
  \ drop vector pointer on stack
  drop ;

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
    intersectx f@ ix1 f!
    intersecty f@ iz1 f!
    
    tx1 f@ tz1 f@ tx2 f@ tz2 f@ 0.0001e 0.0001e 20e 5e
    vtmp 8 store-fvec
    vtmp intersect fdrop
    intersectx f@ ix2 f!
    intersecty f@ iz2 f!
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

\ overlay
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

: update-input ( -- )
  begin E sdl-pollevent 0 > while
    E sdl-eventtype@ case    
      SDL_QUIT of
	false RUNNING !
      endof
      SDL_WINDOWEVENT of
	E sdl-windowevent-event case
	  SDL_WINDOWEVENT_RESIZED of
	    E sdl-windowevent-data1
	    E sdl-windowevent-data2
	    HEIGHT ! WIDTH ! setup-viewport
	  endof
	endcase
      endof   
      SDL_KEYDOWN of
	E sdl-event-kbdscancode case
	  SDL_SCANCODE_1 of 0 MAPTYPE ! endof
	  SDL_SCANCODE_2 of 1 MAPTYPE ! endof
	  SDL_SCANCODE_3 of 2 MAPTYPE ! endof
	  SDL_SCANCODE_4 of 3 MAPTYPE ! endof
	  SDL_SCANCODE_Z of
	    GL_FRONT_AND_BACK GL_FILL gl-polygonmode
	    false MWIREFRAME ! endof
	  SDL_SCANCODE_X of
	    GL_FRONT_AND_BACK GL_LINE gl-polygonmode
	    true MWIREFRAME ! endof
	  
	  SDL_SCANCODE_Q     of false RUNNING  ! endof
	  SDL_SCANCODE_W     of true  INPFWD   ! endof
	  SDL_SCANCODE_UP    of true  INPFWD   ! endof
	  SDL_SCANCODE_S     of true  INPBACK  ! endof
	  SDL_SCANCODE_DOWN  of true  INPBACK  ! endof
	  SDL_SCANCODE_A     of true  INPLSTRF ! endof
	  SDL_SCANCODE_D     of true  INPRSTRF ! endof
	  SDL_SCANCODE_LEFT  of true  INPLROT  ! endof
	  SDL_SCANCODE_RIGHT of true  INPRROT  ! endof
	endcase
      endof      
      SDL_KEYUP of
	E sdl-event-kbdscancode case
	  SDL_SCANCODE_W     of false INPFWD   ! endof
	  SDL_SCANCODE_UP    of false INPFWD   ! endof
	  SDL_SCANCODE_S     of false INPBACK  ! endof
	  SDL_SCANCODE_DOWN  of false INPBACK  ! endof
	  SDL_SCANCODE_A     of false INPLSTRF ! endof
	  SDL_SCANCODE_D     of false INPRSTRF ! endof
	  SDL_SCANCODE_LEFT  of false INPLROT  ! endof
	  SDL_SCANCODE_RIGHT of false INPRROT  ! endof
	endcase
      endof      
    endcase
  repeat ( print-input ) ;

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
  false MWIREFRAME !
  
  WALLS generate-walls ;

: init-assets ( -- )
  s" texture/wall.png" load-texture TEXTURE !
  TEXTURE @ drop ;

: dispose-assets ( -- )
  TEXTURE dup @ gl-disposetexture -1 swap ! ;

: dispose-game ( -- )
  ." Disposing walls" cr
  WALLS dup @ ~~ free drop 0 swap !
  ." Disposed" cr ;

: run ( -- )
  init-game open-window init-assets gameloop dispose-assets close-window ( dispose-game ) ;

\ run bye

