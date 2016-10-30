//============================================================
// MX64 commands grammar for ANTLR 4.2
// Author: Alexey Mironov
// Version: 0.0.4
//============================================================

grammar MxScript;

program
        : statement (DELIMITER+ statement)* EOF
        ;

statement
        : connectCommand
        | disconnectCommand
        | hostCommand
        | portCommand
        | userCommand
        | passwordCommand
        | armCommand
        | dbCommand
        | reconnectCommand
        | searchCommand
        | sequentialCommand
        | formatCommand
        | sortCommand
        | printCommand
        | transformCommand
        | createCommand
        | deleteCommand
        | emptyCommand
        | globalCommand
        ;

//================================
//          COMMANDS
//===============================

connectCommand
        : CONNECT_ALIAS STRING?
        ;

disconnectCommand
        : DISCONNECT_ALIAS
        ;

hostCommand
        : HOST_ALIAS                # hostInfoCommand1
        | HOST_ALIAS INFO           # hostInfoCommand2
        | HOST_ALIAS STRING         # hostSetCommand1
        | SET HOST_ALIAS TO? STRING # hostSetCommand2
        ;

portCommand
        : PORT INTEGER              # portSetCommand1
        | SET PORT TO? INTEGER      # portSetCommand2
        ;

userCommand
        : USER_ALIAS                # userInfoCommand1
        | USER_ALIAS INFO           # userInfoCommand2
        | USER_ALIAS STRING         # userSetCommand1
        | SET USER_ALIAS TO? STRING # userSetCommand2
        ;

passwordCommand
        : PASSWORD_ALIAS STRING
        ;

armCommand
        : ARM                       # armInfoCommand
        | ARM STRING                # armSetCommand
        ;

dbCommand
        : DB_ALIAS                  # dbInfoCommand1
        | DB_ALIAS INFO             # dbInfoCommand2
        | DB_ALIAS STRING           # dbSetCommand1
        | SET DB_ALIAS TO? STRING   # dbSetCommand2
        | USE DB_ALIAS STRING       # dbSetCommand3
        ;

reconnectCommand
        : RECONNECT
        ;

searchCommand
        : SEARCH_ALIAS              # searchInfoCommand1
        | SEARCH_ALIAS INFO         # searchInfoCommand2
        | SEARCH_ALIAS STRING       # searchFindCommand
        ;

sequentialCommand
        : SEQUENTIAL STRING
        ;

formatCommand
        : FORMAT                    # formatInfoCommand1
        | FORMAT INFO               # formatInfoCommand2
        | FORMAT STRING             # formatSetCommand1
        | SET FORMAT TO? STRING     # formatSetCommand2
        ;

sortCommand
        : SORT_ALIAS                # sortInfoCommand1
        | SORT_ALIAS INFO           # sortInfoCommand2
        | SORT_ALIAS STRING         # sortSetCommand1
        | SET SORT_ALIAS TO? STRING # sortSetComamnd2
        | SORT RESET                # sortResetCommand1
        | RESET SORT                # sortResetCommand2
        ;

printCommand
        : PRINT                     # printCommand1
        | PRINT (TO FILE?)? STRING  # printCommand2
        ;

transformCommand
        : TRANSFORM_ALIAS           # transformInfoCommand
        | TRANSFORM_ALIAS STRING    # transformSetCommand
        | TRANSFORM_ALIAS RESET     # transformResetCommand1
        | RESET TRANSFORM_ALIAS     # transofrmResetCommand2
        ;

createCommand
        : CREATE DB_ALIAS? STRING
        ;

deleteCommand
        : DELETE DB_ALIAS? STRING
        ;

emptyCommand
        : EMPTY DB_ALIAS? STRING?
        ;

globalCommand
        : GLOBAL_ALIAS STRING
        ;

//================================
//        INTEGER NUMBER
//===============================

INTEGER
        : [0-9]+
        ;

//================================
//          STRINGS
//===============================

STRING
        : '"' .*? '"'
        | '\''  .*? '\''
        | '`'  .*? '`'
        | '<<<' .*? '>>>'
        ;

//===============================
//         ALIASES
//===============================

BOOLEAN
        : ON | OFF | YES | NO
        ;

CONNECT_ALIAS
        : CONNECT
        | OPEN
        ;

DISCONNECT_ALIAS
        : DISCONNECT
        | CLOSE
        ;

HOST_ALIAS
        : HOST
        | SERVER
        | ADDRESS
        ;

USER_ALIAS
        : USER
        | LOGIN
        | USERNAME
        | NAME
        ;

PASSWORD_ALIAS
        : PASSWORD
        | PWD
        ;

DB_ALIAS
        : DB
        | DATABASE
        | CATALOG
        ;

SEARCH_ALIAS
        : SEARCH
        | FIND
        ;

SORT_ALIAS
        : SORT
        | ORDER
        ;

TRANSFORM_ALIAS
        : TRANSFORM
        | TVP
        | FST
        ;

GLOBAL_ALIAS
        : GLOBAL
        | GLOBAL CORRECTION
        | GBL
        ;

//===============================
//       RESERVED WORDS
//===============================

ACTUALIZE : [Aa][Cc][Tt][Uu][Aa][Ll][Ii][Zz][Ee];
ADDRESS   : [Aa][Dd][Dd][Rr][Ee][Ss][Ss];
ALIAS     : [Aa][Ll][Ii][Aa][Ss];
ALL       : [Aa][Ll][Ll];
AM        : [Aa][Mm];
ARM       : [Aa][Rr][Mm];
BASE      : [Bb][Aa][Ss][Ee];
BREAK     : [Bb][Rr][Ee][Aa][Kk];
CANCEL    : [Cc][Aa][Nn][Cc][Ee][Ll];
CATALOG   : [Cc][Aa][Tt][Aa][Ll][Oo][Gg];
CHANGE    : [Cc][Hh][Aa][Nn][Gg][Ee];
CLEAR     : [Cc][Ll][Ee][Aa][Rr];
CLOSE     : [Cc][Ll][Oo][Ss][Ee];
CONNECT   : [Cc][Oo][Nn][Nn][Ee][Cc][Tt];
CONNECTED : [Cc][Oo][Nn][Nn][Ee][Cc][Tt][Ee][Dd];
CORRECTION: [Cc][Oo][Rr][Rr][Ee][Cc][Tt][Ii][Oo][Nn];
CREATE    : [Cc][Rr][Ee][Aa][Tt][Ee];
DATABASE  : [Dd][Aa][Tt][Aa][Bb][Aa][Ss][Ee];
DB        : [Dd][Bb];
DELETE    : [Dd][Ee][Ll][Ee][Tt][Ee];
DELETED   : [Dd][Ee][Ll][Ee][Tt][Ee][Dd];
DESTROY   : [Dd][Ee][Ss][Tt][Rr][Oo][Yy];
DIAGNOSE  : [Dd][Ii][Aa][Gg][Nn][Oo][Ss][Ee];
DISCONNECT: [Dd][Ii][Ss][Cc][Oo][Nn][Nn][Ee][Cc][Tt];
DUMP      : [Dd][Uu][Mm][Pp];
ECHO      : [Ee][Cc][Hh][Oo];
EMPTY     : [Ee][Mm][Pp][Tt][Yy];
ERROR     : [Ee][Rr][Rr][Oo][Rr];
EXEC      : [Ee][Xx][Ee][Cc];
EXECUTE   : [Ee][Xx][Ee][Cc][Uu][Tt][Ee];
EXPORT    : [Ee][Xx][Pp][Oo][Rr][Tt];
EXT       : [Ee][Xx][Tt];
EXTERNAL  : [Ee][Xx][Tt][Ee][Rr][Nn][Aa][Ll];
FATAL     : [Ff][Aa][Tt][Aa][Ll];
FILE      : [Ff][Ii][Ll][Ee];
FIND      : [Ff][Ii][Nn][Dd];
FORMAT    : [Ff][Oo][Rr][Mm][Aa][Tt];
FOUND     : [Ff][Oo][Uu][Nn][Dd];
FROM      : [Ff][Rr][Oo][Mm];
FST       : [Ff][Ss][Tt];
GBL       : [Gg][Bb][Ll];
GLOBAL    : [Gg][Ll][Oo][Bb][Aa][Ll];
HOST      : [Hh][Oo][Ss][Tt];
I         : [Ii];
IGNORE    : [Ii][Gg][Nn][Oo][Rr][Ee];
IMPORT    : [Ii][Mm][Pp][Oo][Rr][Tt];
INCLUDE   : [Ii][Nn][Cc][Ll][Uu][Dd][Ee];
INDEX     : [Ii][Nn][Dd][Ee][Xx] | 'IF';
INFO      : [Ii][Nn][Ff][Oo];
IS        : [Ii][Ss];
ISO       : [Ii][Ss][Oo];
KEEP      : [Kk][Ee][Ee][Pp];
LAST      : [Ll][Aa][Ss][Tt];
LIST      : [Ll][Ii][Ss][Tt];
LOCK      : [Ll][Oo][Cc][Kk];
LOCKED    : [Ll][Oo][Cc][Kk][Ee][Dd];
LOCKS     : [Ll][Oo][Cc][Kk][Ss];
LOG       : [Ll][Oo][Gg];
LOGIN     : [Ll][Oo][Gg][Ii][Nn];
MAIL      : [Mm][Aa][Ii][Ll];
MASTER    : [Mm][Aa][Ss][Tt][Ee][Rr] | 'MF';
NAME      : [Nn][Aa][Mm][Ee];
NEW       : [Nn][Ee][Ww];
NO        : [Nn][Oo];
OBRZV     : [Oo][Bb][Rr][Zz][Vv];
OFF       : [Oo][Ff][Ff];
ON        : [Oo][Nn];
OPEN      : [Oo][Pp][Ee][Nn];
ORDER     : [Oo][Rr][Dd][Ee][Rr];
PASSWORD  : [Pp][Aa][Ss][Ww][Oo][Rr][Dd];
PEEK      : [Pp][Ee][Ee][Kk];
POP       : [Pp][Oo][Pp];
PORT      : [Pp][Oo][Rr][Tt];
PRINT     : [Pp][Rr][Ii][Nn][Tt];
PROCESSES : [Pp][Rr][Oo][Cc][Ee][Ss][Ss][Ee][Ss];
PROGRAM   : [Pp][Rr][Oo][Gg][Rr][Aa][Mm];
PUSH      : [Pp][Uu][Ss][Hh];
PUT       : [Pp][Uu][Tt];
PWD       : [Pp][Ww][Dd];
RECALL    : [Rr][Ee][Cc][Aa][Ll][Ll];
RECONNECT : [Rr][Ee][Cc][Oo][Nn][Nn][Ee][Cc][Tt];
RECORDS   : [Rr][Ee][Cc][Oo][Rr][Dd][Ss];
REMOVE    : [Rr][Ee][Mm][Oo][Vv][Ee];
REORGANIZE: [Rr][Ee][Oo][Rr][Gg][Aa][Nn][Ii][Zz][Ee];
RESET     : [Rr][Ee][Ss][Ee][Tt];
SCRIPT    : [Ss][Cc][Rr][Ii][Pp][Tt];
SEARCH    : [Ss][Ee][Aa][Rr][Cc][Hh];
SEND      : [Ss][Ee][Nn][Dd];
SEQUENTIAL: [Ss][Ee][Qq][Uu][Ee][Nn][Tt][Ii][Aa][Ll];
SERVER    : [Ss][Ee][Rr][Vv][Ee][Rr];
SET       : [Ss][Ee][Tt];
SORT      : [Ss][Oo][Rr][Tt];
START     : [Ss][Tt][Aa][Rr][Tt];
STOP      : [Ss][Tt][Oo][Pp];
TABLE     : [Tt][Aa][Bb][Ll][Ee];
TAKE      : [Tt][Aa][Kk][Ee];
TEXT      : [Tt][Ee][Xx][Tt];
TO        : [Tt][Oo];
TRANSFORM : [Tt][Rr][Aa][Nn][Ss][Ff][Oo][Rr][Mm];
TVP       : [Tt][Vv][Pp];
UNLOCK    : [Uu][Nn][Ll][Oo][Cc][Kk];
USE       : [Uu][Ss][Ee];
USER      : [Uu][Ss][Ee][Rr];
USERNAME  : [Uu][Ss][Ee][Rr][Nn][Aa][Mm][Ee];
USERS     : [Uu][Ss][Ee][Rr][Ss];
WARNING   : [Ww][Aa][Rr][Nn][Ii][Nn][Gg];
WHO       : [Ww][Hh][Oo];
WRITEABLE : [Ww][Rr][Ii][Tt][Ee][Aa][Bb][Ll][Ee];
YES       : [Yy][Ee][Ss];

//================================
//        DELIMITERS
//===============================

DELIMITER
        : ';'
        | [\r\n]+
        ;

//================================
//  MASTER RECORD NUMBERS (MFN)
//===============================

fragment
RECNUM
        : [0-9]+
        ;

fragment
RECRANGE
        : RECNUM MINUS (RECNUM|LAST)
        ;

RECLIST
        : (RECNUM|RECRANGE) (COMMA (RECNUM|RECRANGE))*
        ;

//===============================
//     SERVICE CHARACTERS
//===============================

BANG      : '!';
LPAREN    : '(';
RPAREN    : ')';
COMMA     : ',';
EQUAL     : '=';
QUESTION  : '?';
MINUS     : '-';
PLUS      : '+';
STAR      : '*';

//===============================
//         COMMENTS
//===============================

COMMENT
        : '#' .*? '\r'? '\n' -> skip
        ;

//===============================
//         WHITESPACE
//===============================

WS
        : [ \t]+ -> skip
        ;
