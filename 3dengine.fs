include sdl.fs
include gl.fs

800 constant WIDTH
600 constant HEIGHT

variable WINDOW
variable CONTEXT
variable RUNNING
sdl-event E

: to-c-str ( a u -- a )
  pad swap dup >r move 0 pad r> + c! pad ;

: from-c-str ( a -- a u )
  0 begin 2dup + c@ while 1+ repeat ;

: open-window ( -- )
  SDL_INIT_VIDEO SDL_INIT_GAMECONTROLLER OR
  sdl-init

  s" Teste 3D" to-c-str
  SDL_WINDOWPOS_CENTERED SDL_WINDOWPOS_CENTERED
  WIDTH HEIGHT
  SDL_WINDOW_SHOWN SDL_WINDOW_OPENGL OR
  sdl-createwindow WINDOW !

  WINDOW @ sdl-gl-createcontext CONTEXT !
  1 sdl-gl-setswapinterval drop

  \ Atributos
  SDL_GL_CONTEXT_PROFILE_MASK SDL_GL_CONTEXT_PROFILE_CORE
  SDL_GL_CONTEXT_MAJOR_VERSION 2
  SDL_GL_CONTEXT_MINOR_VERSION 2
  SDL_GL_RED_SIZE      8
  SDL_GL_GREEN_SIZE    8
  SDL_GL_BLUE_SIZE     8
  SDL_GL_ALPHA_SIZE    8
  SDL_GL_BUFFER_SIZE  32
  SDL_GL_DOUBLEBUFFER  1
  SDL_GL_DEPTH_SIZE   24
  SDL_GL_SHARE_WITH_CURRENT_CONTEXT 1
  11 0 do sdl-gl-setattribute drop loop drop

  0e 0e 0e 1e gl-clearcolor ;


: close-window ( -- )
  CONTEXT @ sdl-gl-deletecontext
  WINDOW @ sdl-destroywindow ;

: clear-window ( -- )
  GL_COLOR_BUFFER_BIT GL_DEPTH_BUFFER_BIT OR gl-clear ;
  
: swap-window ( -- )
  WINDOW @ sdl-gl-swapwindow ;

: draw-triangle ( -- )
  GL_TRIANGLES gl-begin
  1e 0e 0e    gl-color3f
  0.0e 0.5e   gl-vertex2f

  0e 1e 0e    gl-color3f
  -0.5e -0.5e gl-vertex2f

  0e 0e 1e    gl-color3f
  0.5e -0.5e  gl-vertex2f
  gl-end ;

: paint-window
  clear-window
  draw-triangle
  swap-window ;

: gameloop ( -- )
  begin RUNNING @ while    
    begin E sdl-pollevent 0 > while
      E sdl-eventtype@ SDL_QUIT =
      if false RUNNING ! then
      E sdl-eventtype@ SDL_KEYDOWN =
      if
	E sdl-keyboardevent-keysym@ sdl-keysym-scancode 20 ~~ =
	if false RUNNING ! then
      then
    repeat
    \ update
    paint-window
  repeat ;

: run ( -- )
  true RUNNING !
  open-window
  gameloop
  close-window ;
