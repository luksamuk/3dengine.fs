include sdl.fs
include gl.fs

100e fconstant INTERNALW
100e fconstant INTERNALH

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

variable PX
variable PY
variable PANGLE

: reload-file ( -- )
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
  dup 35 floats allocate 0 = if
    swap ! @
    70e 20e  70e 70e  1e 1e 0e \ Yellow
    40e 90e  70e 70e  1e 0e 1e \ Magenta
    20e 80e  40e 90e  1e 0e 0e \ Red
    20e 80e  50e 10e  0e 1e 1e \ Cyan
    50e 10e  70e 20e  0e 1e 0e \ Green
    35 store-fvec
  else 2drop drop then ;


: setup-viewport ( -- )
  0e 0e 0e 1e gl-clearcolor
  GL_LEQUAL gl-depthfunc
  GL_SRC_ALPHA GL_ONE_MINUS_SRC_ALPHA gl-blendfunc
  GL_DEPTH_TEST gl-enable
  GL_BLEND gl-enable
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


: wall@ ( i -- ) ( -- r )
  floats WALLS @ + f@ ;

\ Top down map
: draw-wall-topdown ( n -- )
  7 *
  gl-pushmatrix
  dup 4 + wall@ dup 5 + wall@ dup 6 + wall@ 1e gl-color4f
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

: paint-window ( -- )
  clear-window
  draw-map-topdown
  swap-window ;


: print-input ( -- )
  cr cr s" Input" type cr s" =====" type cr
  s" Fwd: " type INPFWD   @ . cr
  s" Bck: " type INPBACK  @ . cr
  s" Lst: " type INPLSTRF @ . cr
  s" Rst: " type INPRSTRF @ . cr
  s" Lrt: " type INPLROT  @ . cr
  s" Rrt: " type INPRROT  @ . cr ;

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
  800 WIDTH  !
  600 HEIGHT !
  false INPFWD   !
  false INPBACK  !
  false INPLSTRF !
  false INPRSTRF !
  false INPLROT  !
  false INPRROT  !
  50e PX        f!
  50e PY        f!
  0e PANGLE     f!
  true RUNNING   !
  WALLS generate-walls ;

: dispose-game ( -- )
  WALLS dup @ free drop 0 swap ! ;

: run ( -- )
  init-game open-window gameloop close-window dispose-game ;

\ run bye

