\ input.fs -- Input-related bits for 3dengine.fs.
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

sdl-event E

variable INPFWD   \ Forward
variable INPBACK  \ Backward
variable INPLSTRF \ Strafe left
variable INPRSTRF \ Strafe right
variable INPLROT  \ Rotate left
variable INPRROT  \ Rotate right

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
  repeat ;

