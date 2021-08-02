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

: fvec@ ( a i -- ) ( -- r )
  floats + f@ ;

: wall@ ( i -- ) ( -- r )
  WALLS @ swap fvec@ ;

: cross ( -- ) ( rx1 ry1 rx2 ry2 -- r )
  fswap frot f* frot frot f* fswap f- ;

variable intersect-x  \ intersect x point output
variable intersect-y  \ intersect y point output
: intersect ( a -- ) ( -- r )
  \ a is a ptr to float vector containing:
  \ x1, y1, x2, y2, x3, y3, x4, y4
  dup 0 fvec@ \ x1
  dup 1 fvec@ \ y1
  dup 2 fvec@ \ x2
  dup 3 fvec@ \ y2
  cross intersect-x f!

  dup 4 fvec@  \ x3
  dup 5 fvec@  \ y3
  dup 6 fvec@  \ x4
  dup 7 fvec@  \ y4
  cross intersect-y f!

  dup 0 fvec@ dup 2 fvec@ f-  \ x1 - x2
  dup 1 fvec@ dup 3 fvec@ f-  \ y1 - y2
  dup 4 fvec@ dup 6 fvec@ f-  \ x3 - x4
  dup 5 fvec@ dup 7 fvec@ f-  \ y3 - y4
  cross \ determinant pushed onto float stack

  fdup
  intersect-x f@                \ x
  dup 0 fvec@ dup 2 fvec@ f-   \ x1 - x2
  intersect-y f@                \ y
  dup 4 fvec@ dup 6 fvec@ f-   \ x3 - x4
  cross fswap f/ intersect-x f!

  fdup
  intersect-x f@                \ x
  dup 1 fvec@ dup 3 fvec@ f-   \ y1 - y2
  intersect-y f@                \ y
  dup 5 fvec@ dup 7 fvec@ f-   \ y3 - y4
  cross fswap f/ intersect-y f!
  
  \ drop vector pointer on stack
  drop ;

