\ sdl.fs -- SDL 2.0 bindings for 3dengine.fs.
\ Copyright (C) 2015 Braden Shepherdson
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

\ Modelled around SDL2 v2.0.14
c-library sdl2
\c #include <SDL2/SDL.h>
\c #include <SDL2/SDL_opengl.h>

s" SDL2" add-lib

c-function sdl-init               SDL_Init               n -- n
c-function sdl-createwindow       SDL_CreateWindow       a n n n n n -- a
c-function sdl-quit               SDL_Quit               -- void
c-function sdl-createrenderer     SDL_CreateRenderer     a n n -- a
c-function sdl-createtexture      SDL_CreateTexture      a n n n n -- a
c-function sdl-updatetexture      SDL_UpdateTexture      a a a n -- n
c-function sdl-destroywindow      SDL_DestroyWindow      a -- void
c-function sdl-destroyrenderer    SDL_DestroyRenderer    a -- void
c-function sdl-destroytexture     SDL_DestroyTexture     a -- void
c-function sdl-getError           SDL_GetError           -- a
c-function sdl-renderclear        SDL_RenderClear        a -- n
c-function sdl-rendercopy         SDL_RenderCopy         a a a a -- n
c-function sdl-renderpresent      SDL_RenderPresent      a -- void
c-function sdl-getticks           SDL_GetTicks           -- n
c-function sdl-delay              SDL_Delay              n -- void
c-function sdl-gl-setattribute    SDL_GL_SetAttribute    n n -- n
c-function sdl-gl-createcontext   SDL_GL_CreateContext   a -- a
c-function sdl-gl-deletecontext   SDL_GL_DeleteContext   a -- void
c-function sdl-gl-setswapinterval SDL_GL_SetSwapInterval n -- n
c-function sdl-gl-swapwindow      SDL_GL_SwapWindow      a -- void
c-function sdl-pollevent          SDL_PollEvent          a -- n

end-c-library

HEX

0001 CONSTANT SDL_INIT_TIMER
0010 CONSTANT SDL_INIT_AUDIO
0020 CONSTANT SDL_INIT_VIDEO
0200 CONSTANT SDL_INIT_JOYSTICK
1000 CONSTANT SDL_INIT_HAPTIC
2000 CONSTANT SDL_INIT_GAMECONTROLLER
4000 CONSTANT SDL_INIT_EVENTS
7231 CONSTANT SDL_INIT_EVERYTHING

1fff0000 CONSTANT SDL_WINDOWPOS_UNDEFINED
2fff0000 CONSTANT SDL_WINDOWPOS_CENTERED

1 CONSTANT SDL_RENDERER_SOFTWARE
2 CONSTANT SDL_RENDERER_ACCELERATED
4 CONSTANT SDL_RENDERER_PRESENTVSYNC
8 CONSTANT SDL_RENDERER_TARGETTEXTURE

00 CONSTANT SDL_GL_RED_SIZE
01 CONSTANT SDL_GL_GREEN_SIZE
02 CONSTANT SDL_GL_BLUE_SIZE
03 CONSTANT SDL_GL_ALPHA_SIZE
04 CONSTANT SDL_GL_BUFFER_SIZE
05 CONSTANT SDL_GL_DOUBLEBUFFER
06 CONSTANT SDL_GL_DEPTH_SIZE
07 CONSTANT SDL_GL_STENCIL_SIZE
08 CONSTANT SDL_GL_ACCUM_RED_SIZE
09 CONSTANT SDL_GL_ACCUM_GREEN_SIZE
0A CONSTANT SDL_GL_ACCUM_BLUE_SIZE
0B CONSTANT SDL_GL_ACCUM_ALPHA_SIZE
0C CONSTANT SDL_GL_STEREO
0D CONSTANT SDL_GL_MULTISAMPLEBUFFERS
0E CONSTANT SDL_GL_MULTISAMPLESAMPLES
0F CONSTANT SDL_GL_ACCELERATED_VISUAL
10 CONSTANT SDL_GL_RETAINED_BACKING
11 CONSTANT SDL_GL_CONTEXT_MAJOR_VERSION
12 CONSTANT SDL_GL_CONTEXT_MINOR_VERSION
13 CONSTANT SDL_GL_CONTEXT_EGL
14 CONSTANT SDL_GL_CONTEXT_FLAGS
15 CONSTANT SDL_GL_CONTEXT_PROFILE_MASK
16 CONSTANT SDL_GL_SHARE_WITH_CURRENT_CONTEXT
17 CONSTANT SDL_GL_FRAMEBUFFER_SRGB_CAPABLE
18 CONSTANT SDL_GL_CONTEXT_RELEASE_BEHAVIOR
19 CONSTANT SDL_GL_CONTEXT_RESET_NOTIFICATION
20 CONSTANT SDL_GL_CONTEXT_NO_ERROR

1 CONSTANT SDL_GL_CONTEXT_PROFILE_CORE
2 CONSTANT SDL_GL_CONTEXT_PROFILE_COMPATIBILITY
4 CONSTANT SDL_GL_CONTEXT_PROFILE_ES

0001 CONSTANT SDL_WINDOW_FULLSCREEN
0002 CONSTANT SDL_WINDOW_OPENGL
0004 CONSTANT SDL_WINDOW_SHOWN
0008 CONSTANT SDL_WINDOW_HIDDEN
0010 CONSTANT SDL_WINDOW_BORDERLESS
0020 CONSTANT SDL_WINDOW_RESIZABLE
0040 CONSTANT SDL_WINDOW_MINIMIZED
0080 CONSTANT SDL_WINDOW_MAXIMIZED
0100 CONSTANT SDL_WINDOW_INPUT_GRABBED
0200 CONSTANT SDL_WINDOW_INPUT_FOCUS
0400 CONSTANT SDL_WINDOW_MOUSE_FOCUS
0800 CONSTANT SDL_WINDOW_FOREIGN
2000 CONSTANT SDL_WINDOW_ALLOW_HIGHDPI
1001 CONSTANT SDL_WINDOW_FULLSCREEN_DESKTOP

0 CONSTANT SDL_TEXTUREACCESS_STATIC
1 CONSTANT SDL_TEXTUREACCESS_STREAMING
2 CONSTANT SDL_TEXTUREACCESS_TARGET

16362004 CONSTANT SDL_PIXELFORMAT_ARGB8888

0 CONSTANT SDL_RELEASED
1 CONSTANT SDL_PRESSED

100 CONSTANT SDL_QUIT
101 CONSTANT SDL_APP_TERMINATING
102 CONSTANT SDL_APP_LOWMEMORY
103 CONSTANT SDL_APP_WILLENTERBACKGROUND
104 CONSTANT SDL_APP_DIDENTERBACKGROUND
105 CONSTANT SDL_APP_WILLENTERFOREGROUND
106 CONSTANT SDL_APP_DIDENTERFOREGROUND
107 CONSTANT SDL_LOCALECHANGED
150 CONSTANT SDL_DISPLAYEVENT
200 CONSTANT SDL_WINDOWEVENT
201 CONSTANT SDL_SYSWMEVENT
300 CONSTANT SDL_KEYDOWN
301 CONSTANT SDL_KEYUP
302 CONSTANT SDL_TEXTEDITING
303 CONSTANT SDL_TEXTINPUT
304 CONSTANT SDL_KEYMAPCHANGED
400 CONSTANT SDL_MOUSEMOTION
401 CONSTANT SDL_MOUSEBUTTONDOWN
402 CONSTANT SDL_MOUSEBUTTONUP
403 CONSTANT SDL_MOUSEWHEEL
600 CONSTANT SDL_JOYAXISMOTION
601 CONSTANT SDL_JOYBALLMOTION
602 CONSTANT SDL_JOYHATMOTION
603 CONSTANT SDL_JOYBUTTONDOWN
604 CONSTANT SDL_JOYBUTTONUP
605 CONSTANT SDL_JOYDEVICEADDED
606 CONSTANT SDL_JOYDEVICEREMOVED
650 CONSTANT SDL_CONTROLLERAXISMOTION
651 CONSTANT SDL_CONTROLLERBUTTONDOWN
652 CONSTANT SDL_CONTROLLERBUTTONUP
653 CONSTANT SDL_CONTROLLERDEVICEADDED
DECIMAL

\ See SDL_scancode.h
004 CONSTANT SDL_SCANCODE_A
007 CONSTANT SDL_SCANCODE_D
020 CONSTANT SDL_SCANCODE_Q
022 CONSTANT SDL_SCANCODE_S
026 CONSTANT SDL_SCANCODE_W
027 CONSTANT SDL_SCANCODE_X
029 CONSTANT SDL_SCANCODE_Z
030 CONSTANT SDL_SCANCODE_1
031 CONSTANT SDL_SCANCODE_2
032 CONSTANT SDL_SCANCODE_3
033 CONSTANT SDL_SCANCODE_4
079 CONSTANT SDL_SCANCODE_RIGHT
080 CONSTANT SDL_SCANCODE_LEFT
081 CONSTANT SDL_SCANCODE_DOWN
082 CONSTANT SDL_SCANCODE_UP

\ see SDL_video.h
02 CONSTANT SDL_WINDOWEVENT_SHOWN
03 CONSTANT SDL_WINDOWEVENT_HIDDEN
04 CONSTANT SDL_WINDOWEVENT_EXPOSED
05 CONSTANT SDL_WINDOWEVENT_MOVED
06 CONSTANT SDL_WINDOWEVENT_RESIZED
07 CONSTANT SDL_WINDOWEVENT_SIZE_CHANGED
08 CONSTANT SDL_WINDOWEVENT_MINIMIZED
09 CONSTANT SDL_WINDOWEVENT_MAXIMIZED
10 CONSTANT SDL_WINDOWEVENT_RESTORED
11 CONSTANT SDL_WINDOWEVENT_ENTER
12 CONSTANT SDL_WINDOWEVENT_LEAVE
13 CONSTANT SDL_WINDOWEVENT_FOCUS_GAINED
14 CONSTANT SDL_WINDOWEVENT_FOCUS_LOST
15 CONSTANT SDL_WINDOWEVENT_CLOSE
16 CONSTANT SDL_WINDOWEVENT_TAKE_FOCUS
17 CONSTANT SDL_WINDOWEVENT_HIT_TEST



\ See SDL_events.h
\ SDL_Event
\ type: 8 bytes
\ This is a union type, so memory layout depends on
\ other structs
: sdl-event create 10 cells allot ;
: sdl-eventtype@ ( a -- n )
  ul@ ;

\ SDL_KeyboardEvent struct
\ type: 8 bytes
\ timestamp: 8 bytes
\ windowid: 8 bytes
\ state: 1 byte
\ repeat: 1 byte
\ 2 bytes of padding
\ keysym: SDL_Keysym
: sdl-keyboardevent-timestamp ( a -- n )
  4 chars + ul@ ;
: sdl-keyboardevent-windowid ( a -- n )
  8 chars + ul@ ;
: sdl-keyboardevent-state ( a -- n )
  12 chars + c@ ;
: sdl-keyboardevent-repeat ( a -- n )
  13 chars + c@ ;
: sdl-keyboardevent-keysym@ ( a -- a )
  16 chars + ;

\ SDL_WindowEvent struct
\ type: 8 bytes
\ timestamp: 8 bytes
\ windowid: 8 bytes
\ event: SDL_WindowEventID, 1 byte
\ 3 bytes of padding
\ data1: 8 bytes
\ data2: 8 bytes
: sdl-windowevent-timestamp ( a -- n )
  4 chars + ul@ ;
: sdl-windowevent-windowid ( a -- n )
  8 chars + ul@ ;
: sdl-windowevent-event ( a -- n )
  12 chars + c@ ;
: sdl-windowevent-data1 ( a -- n )
  16 chars + ul@ ;
: sdl-windowevent-data2 ( a -- n )
  20 chars + ul@ ;

\ See SDL_keyboard.h
\ SDL_Keysym struct
\ scancode: SDL_Scancode, enum (8 bytes)
\ keycode: SDL_Keycode, enum (8 bytes)
\ mod: 2 bytes
\ 8 bytes of unused space
: sdl-keysym-scancode ( a -- n )
  ul@ ;
: sdl-keysym-keycode ( a -- n )
  4 chars + ul@ ;
: sdl-keysym-mod ( a -- n )
  2 chars + uw@ ;

\ SDL_Keysym helpers from an SDL_Event standpoint
: sdl-event-kbdscancode ( a -- n )
  sdl-keyboardevent-keysym@ sdl-keysym-scancode ;
