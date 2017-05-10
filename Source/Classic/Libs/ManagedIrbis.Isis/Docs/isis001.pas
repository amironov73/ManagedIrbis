{~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
'   Copyright (c) 1996 by

'   United Nations Educational Scientific and Cultural Organization.
'                                &
'   Latin American and Caribbean Center on Health Sciences Information /
'   PAHO-WHO.

'   All Rights Reserved.
'~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~}

Unit Isis001;

Interface


{-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_

			 ISIS_DLL Global Constants

-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_}


{ ----------- Debug Flags ---------------------- }

Const SHOW_NEVER       =   0;
Const SHOW_FATAL       =   1;
Const SHOW_ALWAYS      =   3;

Const EXIT_NEVER       =   0;
Const EXIT_FATAL       =  16;
Const EXIT_ALWAYS      =  48;

Const DEBUG_VERY_LIGHT =  SHOW_NEVER  OR EXIT_NEVER;
Const DEBUG_LIGHT      =  SHOW_FATAL  OR EXIT_FATAL;
Const DEBUG_HARD       =  SHOW_ALWAYS OR EXIT_FATAL;
Const DEBUG_VERY_HARD  =  SHOW_ALWAYS OR EXIT_ALWAYS;

{ ---------------------------------------------- }

Const KEY_LENGTH       =   30;
Const IFBSIZE          =  512;
Const SRC_EXPR_LENGTH  =  512;
Const MAXPATHLEN       =   63;

Const KEY_LENGTH1 = KEY_LENGTH + 1;
Const SRC_EXPR_LENGTH1 = SRC_EXPR_LENGTH + 1;
Const MAXPATHLEN1 = MAXPATHLEN + 1;

Const MAXMFRL          = 8192;          {max record length.}



{-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_

			    ISIS_DLL Structures

-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_}

{ NOTE: The structures DOES NOT necessarily reflects the actual
	disk file layout. }


Type IsisRecControl = record

    ctlmfn          : LongInt;                 {gdb ctlmfn.}
    nxtmfn          : LongInt;                 {gdb nxtmfn.}
    nxtmfb          : LongInt;                 {gdb nxtmfb.}
    nxtmfp          : LongInt;                 {gdb nxtmfp - offset.}
    mftype          : LongInt;                 {gdb mftype.}
    reccnt          : LongInt;                 {gdb reccnt.}
    mfcxx1          : LongInt;                 {gdb mfcxx1.}
    mfcxx2          : LongInt;                 {gdb mfcxx2 - MULTI: Data entry lock.}
    mfcxx3          : LongInt;                 {gdb mfcxx3 - MULTI: Exclusive write lock.}

End;

Type IsisRecDir = record

    tag             : LongInt;                 {field tag entry.}
    pos             : LongInt;                 {field position.}
    len             : LongInt;                 {field length entry.}

End;

Type IsisRecLeader = record

    mfn             : LongInt;                 {gdb mfn.}
    mfrl            : LongInt;                 {gdb mfrl - MULTI: record being updated.}
    mfbwb           : LongInt;                 {gdb mfbwb.}
    mfbwp           : LongInt;                 {gdb mfbwp - offset.}
    base            : LongInt;                 {gdb base (MSNVSPLT).}
    nvf             : LongInt;                 {gdb nvf.}
    status          : LongInt;                 {gdb status.}

End;

Type IsisSpaHeader = record

    handle          : LongInt;		                {pointer to ISIS_SPACE.}
    name            : array [0..MAXPATHLEN] of Char;    {ISIS_SPACE name.}
    cipar           : array [0..MAXPATHLEN] of Char;    {cipar file name.}
    mf              : array [0..MAXPATHLEN] of Char;    {master file name.}
    ifi             : array [0..MAXPATHLEN] of Char;    {inverted file name.}
    isoin           : array [0..MAXPATHLEN] of Char;    {import iso file name.}
    isoout          : array [0..MAXPATHLEN] of Char;    {export iso file name.}
    rec             : LongInt;                          {number of RECSTRU shelves.}
    trm             : LongInt;                          {number of TRMSTRU shelves.}
    filestatus      : LongInt;                          {file status - bit mask.}

End;

Type IsisSrcHeader = record

    number          : LongInt;                             {search number (start in 1).}
    hits            : LongInt;                             {total posting retrieved.}
    recs            : LongInt;                             {total records retrieved.}
    segmentpostings : LongInt;                             {number of hits.}
    dbname          : array [0..MAXPATHLEN] of Char;       {data base name.}
    booleanexpr     : array [0..SRC_EXPR_LENGTH] of Char;  {search expression.}

End;

Type IsisSrcHit = record

    mfn             : LongInt;                 {current hit mfn.}
    tag             : LongInt;                 {current hit tag.}
    occ             : LongInt;                 {current hit occ.}
    cnt             : LongInt;                 {current hit cnt.}

End;

Type IsisSrcMfn = record

    mfn             : LongInt;                 {hit mfn component.}

End;

Type IsisTrmMfn = record

    mfn             : LongInt;                 {posting mfn component.}

End;

Type IsisTrmPosting = record

    posting         : LongInt;                 {current posting order.}
    mfn             : LongInt;                 {current posting pmfn.}
    tag             : LongInt;                 {current posting ptag.}
    occ             : LongInt;                 {current posting pocc.}
    cnt             : LongInt;                 {current posting pcnt.}

End;

Type IsisTrmRead = record

    key             : array [0..KEY_LENGTH] of Char;  {term key.}

End;



{-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_

		       ISIS_DLL Error Codes

-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_}


Const  ZERO            =    0;    { No error. }

{--------------------------------------------------------------------------
 ------------------ Data Base Errors --------------------------------------
 --------------------------------------------------------------------------}

Const ERR_DBDELOCK     =  -101;   { Data Base access denied (data entry lock). }
Const ERR_DBEWLOCK     =  -102;   { Data Base access denied (probably exclusive write lock). }
Const ERR_DBMONOUSR    =  -103;   { Data Base access is single-user. }
Const ERR_DBMULTUSR    =  -104;   { Data Base access is multi-user. }


{--------------------------------------------------------------------------
 ------------------ File Manipulation Erros -------------------------------
 -------------------------------------------------------------------------- }

Const ERR_FILECREATE   =  -201;   { File create error. }
Const ERR_FILEDELETE   =  -202;   { File delete error. }
Const ERR_FILEEMPTY    =  -203;   { File (empty). }
Const ERR_FILEFLUSH    =  -204;   { File flush error. }
Const ERR_FILEFMT      =  -205;   { File does not exist (fmt). }
Const ERR_FILEFST      =  -206;   { File does not exist (fst). }
Const ERR_FILEINVERT   =  -207;   { File does not exist (inverted). }
Const ERR_FILEISO      =  -208;   { File does not exist (ISO). }
Const ERR_FILEMASTER   =  -209;   { File does not exist (master). }
Const ERR_FILEMISSING  =  -210;   { File missing. }
Const ERR_FILEOPEN     =  -211;   { File open error. }
Const ERR_FILEPFT      =  -212;   { File does not exist (pft). }
Const ERR_FILEREAD     =  -213;   { File read error. }
Const ERR_FILERENAME   =  -214;   { File rename error. }
Const ERR_FILESTW      =  -215;   { File does not exist (stw). }
Const ERR_FILEWRITE    =  -216;   { File write error. }


{--------------------------------------------------------------------------
 ------------------ Low Level Engine Errors -------------------------------
 -------------------------------------------------------------------------- }

Const ERR_LLCISISETRAP =  -301;   { Cisis Low Level Error Trap. }
Const ERR_LLISISETRAP  =  -302;   { Isis  Low Level Error Trap. }


{--------------------------------------------------------------------------
 ------------------ Memory Manipulation Errors ----------------------------
 -------------------------------------------------------------------------- }

Const ERR_MEMALLOCAT   =  -401;   { Memory Allocation Error. }


{--------------------------------------------------------------------------
 ------------------ Parameter Specification Errors ------------------------
 -------------------------------------------------------------------------- }

Const ERR_PARAPPHAND   =  -501;   { Invalid application handle. }
Const ERR_PARFILNINV   =  -502;   { Invalid file name size. }
Const ERR_PARFLDSYNT   =  -503;   { Syntax Error (field update). }
Const ERR_PARFMTSYNT   =  -504;   { Syntax Error (format). }
Const ERR_PARNULLPNT   =  -505;   { NULL pointer. }
Const ERR_PARNULLSTR   =  -506;   { String with zero size. }
Const ERR_PAROUTRANG   =  -507;   { Parameter out of range. }
Const ERR_PARSPAHAND   =  -508;   { Invalid space handle. }
Const ERR_PARSRCSYNT   =  -509;   { Syntax Error (search). }
Const ERR_PARSUBFSPC   =  -510;   { Invalid subfield specification. }
Const ERR_PARUPDSYNT   =  -511;   { Syntax Error (record update). }


{--------------------------------------------------------------------------
 ------------------ Record Errors -----------------------------------------
 -------------------------------------------------------------------------- }

Const ERR_RECEOF       =  -601;   { Record eof: found eof in data base. }
Const ERR_RECLOCKED    =  -602;   { Record locked. }
Const ERR_RECLOGIDEL   =  -603;   { Record logically deleted. }
Const ERR_RECNOTNORM   =  -604;   { Record condition is not RCNORMAL. }
Const ERR_RECPHYSDEL   =  -605;   { Record physically deleted. }


{--------------------------------------------------------------------------
 ------------------ Term Errors -------------------------------------------
 -------------------------------------------------------------------------- }

Const ERR_TRMEOF       =  -701;   { Term eof: found eof in data base. }
Const ERR_TRMNEXT      =  -702;   { Term next: key not found. }


{--------------------------------------------------------------------------
 ------------------ Unexpected Errors -------------------------------------
 -------------------------------------------------------------------------- }

Const ERR_UNEXPECTED   =  -999;   { Unexpected Error. }




{-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_

		    IsisSpaHeader - filestatus

 -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_


 ---------------------- File Status ------------------------------

  Máscara de bits indicando quais são os arquivos existentes.

  00000000 00000000 00000000 00000000          0 = No open files.
  00000000 00000000 00000000 00000001          1 = Master   *.mst
  00000000 00000000 00000000 00000010          2 = Master   *.xrf
  00000000 00000000 00000000 00000100          4 = Inverted *.cnt
  00000000 00000000 00000000 00001000          8 = Inverted *.n01
  00000000 00000000 00000000 00010000         16 = Inverted *.n02
  00000000 00000000 00000000 00100000         32 = Inverted *.l01
  00000000 00000000 00000000 01000000         64 = Inverted *.l02
  00000000 00000000 00000000 10000000        128 = Inverted *.ifp

  00000000 00000000 00000001 00000000        256 =          *.fst
  00000000 00000000 00000010 00000000        512 =          *.pft
  00000000 00000000 00000100 00000000       1024 =          *.pft (Cisis)
  00000000 00000000 00001000 00000000       2048 =          *.fmt
  00000000 00000000 00010000 00000000       4096 =          *.stw
  00000000 00000000 00100000 00000000       8192 =          *.fdt
  00000000 00000000 01000000 00000000      16384 = ISO in   *.iso
  00000000 00000000 10000000 00000000      32738 = ISO out  *.iso

  00000000 00000001 00000000 00000000      65536 = Cipar    *.par
  00000000 00000010 00000000 00000000     131072 = Gizmo
  00000000 00000100 00000000 00000000     262144 = Decode

  00000001 00000000 00000000 00000000   16777216 =          *.any }



implementation

end.
