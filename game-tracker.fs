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
\ delimit with ,
variable tags

: PROMPT-USER ( -- ) ;

\ title: ▊ ▍▏name ;
\ games per year: ▊ ▍▏ name;

\ determine how to place these strings directly next to each other in memory...
\ count out length when using user input? then store length, use to access variable later?
\ spaces will need to be added in

: ARGUMENT ( char # -- addr ) CELLS command-buffer + ;

: FETCH-COMMAND ( -- ) command-buffer command-buffer-ptr @ ;

: PRINT-COMMAND ( -- ) CR CR command-buffer command-buffer-ptr @ type ;

: SHOW-INPUT ( -- ) command-buffer command-buffer-ptr @ + 1 type ;

: APPEND-BUFFER ( u -- ) command-buffer-ptr @ ARGUMENT ! ;

\ : APPEND-BUFFER ( u -- ) command-buffer command-buffer-ptr @ + ! ;

: MOVE-PTR ( u -- ) command-buffer-ptr @ + command-buffer-ptr ! ;

: ADD-QUOTE ( -- )
    1 MOVE-PTR
    39 APPEND-BUFFER ;

: SPACE-ARGUMENT ( -- )
    1 MOVE-PTR
    32 APPEND-BUFFER ;

: STORE ( addr u -- ) 
    APPEND-BUFFER
    SHOW-INPUT ;

: ADD ( -- )
    SPACE-ARGUMENT
    ADD-QUOTE
    1 MOVE-PTR
    begin
        key dup dup
        \ ." before store"
        \ CR
        \ .s
        STORE
        \ ." after store"
        \ CR
        \ .s
        \ CR
        1 MOVE-PTR
        13 = 
    until drop ADD-QUOTE PRINT-COMMAND ;

: ADD-NAME ( -- )
    ADD ;

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

 : INITIALIZE-BUFFER ( -- ) 
    \ 110 111 100 101 32 114 101 113 117 101 115 116 46 106 115
    115 106 46 116 115 101 117 113 101 114 32 101 100 111 110
    15 0 DO APPEND-BUFFER 1 MOVE-PTR CR .s LOOP ;

INITIALIZE-BUFFER
PRINT-COMMAND
." add game (hit enter to submit each value; type isn't working right now): " CR CR
ADD-GAME