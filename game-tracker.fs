variable command-buffer 256 CELLS ALLOT
variable command-buffer-ptr
\ s" node request.js" .s command-buffer-ptr ! command-buffer !

\ store in memory surrounded by single quotes
variable name
variable genre
variable platform
variable cover
variable franchise
variable developer
variable language
\ delimit with , ?
variable tags

\ will be opening command once helper functions are complete
: PROMPT-USER ( -- ) ;

\ title: ▊ ▍▏name ;
\ games per year: ▊ ▍▏ name;

\ determine how to place these strings directly next to each other in memory...
\ count out length when using user input? then store length, use to access variable later?
\ spaces will need to be added in

\ used to append cells to the command-buffer array
: ARGUMENT ( char # -- addr ) CELLS command-buffer + ;

\ fetches the command (for use in construct-command)
: FETCH-COMMAND ( -- ) command-buffer command-buffer-ptr @ ;

\ prints what's currently stored in the command-buffer
: PRINT-COMMAND ( -- ) CR CR command-buffer command-buffer-ptr @ type ;

\ types out the input to the screen as user types
: SHOW-INPUT ( -- ) command-buffer command-buffer-ptr @ + 1 type ;

\ appends to the buffer
: APPEND-BUFFER ( u -- ) command-buffer-ptr @ ARGUMENT ! ;

\ : APPEND-BUFFER ( u -- ) command-buffer command-buffer-ptr @ + ! ;

\ moves ptr over by 1
: MOVE-PTR ( u -- ) command-buffer-ptr @ + command-buffer-ptr ! ;

: ADD-QUOTE ( -- )
    \ move pointer over 1
    1 MOVE-PTR
    \ append single quote to buffer
    39 APPEND-BUFFER ;

: SPACE-ARGUMENT ( -- )
    \ move pointer over 1
    1 MOVE-PTR
    \ append space to buffer; used to separate commands
    32 APPEND-BUFFER ;

: STORE ( addr u -- ) 
    \ append user input to buffer
    APPEND-BUFFER
    \ type input to screen
    SHOW-INPUT ;

: ADD ( -- )
    \ add space between arguments
    SPACE-ARGUMENT
    \ add a quote before next argument to allow for spaces
    ADD-QUOTE
    \ move ptr to account for new char
    1 MOVE-PTR
    \ begin reading user input until the user hits enter
    begin
        \ key will read value; add 2 dups to the stack
        key dup dup
        \ ." before store"
        \ CR
        \ .s
        \ store value at the current location of the pointer
        STORE
        \ ." after store"
        \ CR
        \ .s
        \ CR
        \ move pointer over 1
        1 MOVE-PTR
        \ check for "enter"
        13 = 
    \ drop value from stack, add closing quote, print command to verify input
    until drop ADD-QUOTE PRINT-COMMAND ;

\ a wrapper for ADD. used to make the code more readable. called for adding name.
: ADD-NAME ( -- )
    ADD ;

\ a wrapper for ADD. used to make the code more readable. called for adding genre.
: ADD-GENRE ( -- )
    ADD ;

\ will prompt user for name and genre currently
: CONSTRUCT-COMMAND ( -- ) ADD-NAME CR ADD-GENRE CR FETCH-COMMAND ;
\ ADD-PLATFORM ADD-COVER ADD-FRANCHISE ADD-DEV ADD-LANG ADD-TAGS ;

\ name genre platform cover franchise developer language tags: F for favorite, 1-5 for rating;
: ADD-GAME ( u1 u2 u3 u4 u5 -- ) 
    CONSTRUCT-COMMAND PRINT-COMMAND ;

: RETRIEVE-TEMPLATE ( -- ) 
\ retrieves a page and prints to an outfile; needed to set template for JSON object later
    s" curl 'https://api.notion.com/v1/pages/14ac93c4e9b880a0ad6ee791b269f41f' -H 'Notion-Version: 2022-06-28' -H 'Authorization: Bearer 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' | json > output.json" system
 ;

\ initializes the buffer. these ascii values spell out "node request.js"
 : INITIALIZE-BUFFER ( -- ) 
    \ 110 111 100 101 32 114 101 113 117 101 115 116 46 106 115
    \ put ascii values on stack
    115 106 46 116 115 101 117 113 101 114 32 101 100 111 110
    \ append to the command-buffer 15 times, move the pointer once each time
    15 0 DO APPEND-BUFFER 1 MOVE-PTR CR .s LOOP ;

\ initializes the buffer
INITIALIZE-BUFFER
\ prints the command
PRINT-COMMAND
." add game (hit enter to submit each value; type isn't working right now): " CR CR
\ add game will take 2 inputs currently (add-name & add-genre)
ADD-GAME