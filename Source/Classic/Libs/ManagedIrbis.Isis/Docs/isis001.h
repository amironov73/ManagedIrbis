/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//   Copyright (c) 1996 by

//   United Nations Educational Scientific and Cultural Organization.
//                                &
//   Latin American and Caribbean Center on Health Sciences Information /
//   PAHO-WHO.

//   All Rights Reserved.
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

#ifndef ISIS001_H
#define ISIS001_H


/*-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_

//                       ISIS_DLL Global Constants

//-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_*/


/* ----------- Debug Flags ---------------------- */

const SHOW_NEVER       =   0;
const SHOW_FATAL       =   1;
const SHOW_ALWAYS      =   3;

const EXIT_NEVER       =   0;
const EXIT_FATAL       =  16;
const EXIT_ALWAYS      =  48;

const DEBUG_VERY_LIGHT =   0;         /* SHOW_NEVER   | EXIT_NEVER  */
const DEBUG_LIGHT      =  17;         /* SHOW_FATAL   | EXIT_FATAL  */
const DEBUG_HARD       =  19;         /* SHOW_ALWAYS  | EXIT_FATAL  */
const DEBUG_VERY_HARD  =  51;         /* SHOW_ALWAYS  | EXIT_ALWAYS */

/* ---------------------------------------------- */

#define KEY_LENGTH         30
const IFBSIZE          =  512;

#define SRC_EXPR_LENGTH   512
#define MAXPATHLEN         63

const MAXMFRL          = 8192;         /* max record length. */  


/*-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_

//                            ISIS_DLL Structures

//-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_*/

/* NOTE: The structures DOES NOT necessarily reflects the actual
	disk file layout. */


struct IsisRecControl
{
 long    ctlmfn;                       /*gdb ctlmfn.*/
 long    nxtmfn;                       /*gdb nxtmfn.*/
 long    nxtmfb;                       /*gdb nxtmfb.*/
 long    nxtmfp;                       /*gdb nxtmfp - offset.*/
 long    mftype;                       /*gdb mftypedef.*/
 long    reccnt;                       /*gdb reccnt.*/
 long    mfcxx1;                       /*gdb mfcxx1.*/
 long    mfcxx2;                       /*gdb mfcxx2 - MULTI: Data entry lock.*/
 long    mfcxx3;                       /*gdb mfcxx3 - MULTI: Exclusive write lock.*/
};

struct IsisRecDir
{
 long    tag;                          /*field tag entry.*/
 long    pos;                          /*field position.*/
 long    len;                          /*field length entry.*/
};

struct IsisRecLeader
{
 long    mfn;                          /*gdb mfn.*/
 long    mfrl;                         /*gdb mfrl - MULTI: record being updated.*/
 long    mfbwb;                        /*gdb mfbwb.*/
 long    mfbwp;                        /*gdb mfbwp - offset.*/
 long    base;                         /*gdb base (MSNVSPLT).*/
 long    nvf;                          /*gdb nvf.*/
 long    status;                       /*gdb status.*/
};

struct IsisSpaHeader
{
 long    handle;                       /*pointer ISIS_SPACE.*/
 char    name[MAXPATHLEN+1];           /*ISIS_SPACE name.*/
 char    cipar[MAXPATHLEN+1];          /*cipar file name.*/
 char    mf[MAXPATHLEN+1];             /*master file name.*/
 char    ifi[MAXPATHLEN+1];            /*inverted file name.*/
 char    isoin[MAXPATHLEN+1];          /*import iso file name.*/
 char    isoout[MAXPATHLEN+1];         /*export iso file name.*/
 long    rec;                          /*number of RECSTRU shelves.*/
 long    trm;                          /*number of TRMSTRU shelves.*/
 long    filestatus;                   /*file status - bit mask.*/
};

struct IsisSrcHeader
{
 long  number;                         /*search number (start in 1).*/
 long  hits;                           /*total posting retrieved.*/
 long  recs;                           /*total records retrieved.  */
 long  segmentpostings;                /*number of hits.*/
 char  dbname[MAXPATHLEN+1];           /*data base name.*/
 char  booleanexpr[SRC_EXPR_LENGTH+1]; /*search expression.*/
};

struct IsisSrcHit
{
 long  mfn;                          /*current hit mfn.*/
 long  tag;                          /*current hit tag.*/
 long  occ;                          /*current hit occ.*/
 long  cnt;                          /*current hit cnt.*/
};

struct IsisSrcMfn
{
 long  mfn;                          /*hit mfn component.*/
};

struct IsisTrmMfn
{
 long  mfn;                          /*posting mfn component.*/
};

struct IsisTrmPosting
{
 long    posting;                      /*current posting order.*/
 long    mfn;                          /*current posting pmfn.*/
 long    tag;                          /*current posting ptag.*/
 long    occ;                          /*current posting pocc.*/
 long    cnt;                          /*current posting pcnt.*/
};

struct IsisTrmRead
{
 char  key[KEY_LENGTH+1];           /*term key.*/
};


/*-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_

//                       ISIS_DLL Error Codes

//-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_*/


const  ZERO          =       0;


/*--------------------------------------------------------------------------
//------------------ Data Base Errors --------------------------------------
//--------------------------------------------------------------------------*/

const ERR_DBDELOCK     =  -101;   /* Data Base access denied (data entry lock).*/
const ERR_DBEWLOCK     =  -102;   /* Data Base access denied (probably exclusive write lock).*/
const ERR_DBMONOUSR    =  -103;   /* Data Base access is single-user.*/
const ERR_DBMULTUSR    =  -104;   /* Data Base access is multi-user.*/


/*--------------------------------------------------------------------------
//------------------ File Manipulation Erros -------------------------------
//--------------------------------------------------------------------------*/

const ERR_FILECREATE   =  -201;   /* File create error.*/
const ERR_FILEDELETE   =  -202;   /* File delete error.*/
const ERR_FILEEMPTY    =  -203;   /* File (empty).*/
const ERR_FILEFLUSH    =  -204;   /* File flush error.*/
const ERR_FILEFMT      =  -205;   /* File does not exist (fmt).*/
const ERR_FILEFST      =  -206;   /* File does not exist (fst).*/
const ERR_FILEINVERT   =  -207;   /* File does not exist (inverted).*/
const ERR_FILEISO      =  -208;   /* File does not exist (ISO).*/
const ERR_FILEMASTER   =  -209;   /* File does not exist (master).*/
const ERR_FILEMISSING  =  -210;   /* File missing.*/
const ERR_FILEOPEN     =  -211;   /* File open error.*/
const ERR_FILEPFT      =  -212;   /* File does not exist (pft).*/
const ERR_FILEREAD     =  -213;   /* File read error.*/
const ERR_FILERENAME   =  -214;   /* File rename error.*/
const ERR_FILESTW      =  -215;   /* File does not exist (stw).*/
const ERR_FILEWRITE    =  -216;   /* File write error.*/


/*--------------------------------------------------------------------------
//------------------ Low Level Engine Errors -------------------------------
//--------------------------------------------------------------------------*/

const ERR_LLCISISETRAP =  -301;   /* Cisis Low Level Error Trap.*/
const ERR_LLISISETRAP  =  -302;   /* Isis  Low Level Error Trap.*/


/*--------------------------------------------------------------------------
//------------------ Memory Manipulation Errors ---------------------------
//--------------------------------------------------------------------------*/

const ERR_MEMALLOCAT   =  -401;   /* Memory Allocation Error.*/


/*--------------------------------------------------------------------------
//------------------ Parameter Specification Errors ------------------------
//--------------------------------------------------------------------------*/

const ERR_PARAPPHAND   =  -501;   /* Invalid application handle.*/
const ERR_PARFILNSIZ   =  -502;   /* Invalid file name size.*/
const ERR_PARFLDSYNT   =  -503;   /* Syntax Error (field update).*/
const ERR_PARFMTSYNT   =  -504;   /* Syntax Error (format).*/
const ERR_PARNULLPNT   =  -505;   /* NULL pointer.*/
const ERR_PARNULLSTR   =  -506;   /* String with zero size.*/
const ERR_PAROUTRANG   =  -507;   /* Parameter out of range.*/
const ERR_PARSPAHAND   =  -508;   /* Invalid space handle.*/
const ERR_PARSRCSYNT   =  -509;   /* Syntax Error (search).*/
const ERR_PARSUBFSPC   =  -510;   /* Invalid subfield specification.*/
const ERR_PARUPDSYNT   =  -511;   /* Syntax Error (record update).*/


/*--------------------------------------------------------------------------
//------------------ Record Errors -----------------------------------------
//--------------------------------------------------------------------------*/

const ERR_RECEOF       =  -601;   /* Record eof: found eof in data base.*/
const ERR_RECLOCKED    =  -602;   /* Record locked.*/
const ERR_RECLOGIDEL   =  -603;   /* Record logically deleted.*/
const ERR_RECNOTNORM   =  -604;   /* Record condition is not RCNORMAL.*/
const ERR_RECPHYSDEL   =  -605;   /* Record physically deleted.*/


/*--------------------------------------------------------------------------
//------------------ Term Errors -------------------------------------------
//--------------------------------------------------------------------------*/

const ERR_TRMEOF       =  -701;   /* Term eof: found eof in data base.*/
const ERR_TRMNEXT      =  -702;   /* Term next: key not found.*/


/*--------------------------------------------------------------------------
//------------------ Unexpected Errors -------------------------------------
//--------------------------------------------------------------------------*/

const ERR_UNEXPECTED   =  -999;   /* Unexpected Error.*/




/*-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_

//                    IsisSpaHeader - filestatus

//-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_

//
// ---------------------- File Status ------------------------------
//
//  Máscara de bits indicando quais são os arquivos existentes.
//
//  00000000 00000000 00000000 00000000          0 = No open files.
//  00000000 00000000 00000000 00000001          1 = Master   *.mst
//  00000000 00000000 00000000 00000010          2 = Master   *.xrf
//  00000000 00000000 00000000 00000100          4 = Inverted *.cnt
//  00000000 00000000 00000000 00001000          8 = Inverted *.n01
//  00000000 00000000 00000000 00010000         16 = Inverted *.n02
//  00000000 00000000 00000000 00100000         32 = Inverted *.l01
//  00000000 00000000 00000000 01000000         64 = Inverted *.l02
//  00000000 00000000 00000000 10000000        128 = Inverted *.ifp

//  00000000 00000000 00000001 00000000        256 =          *.fst
//  00000000 00000000 00000010 00000000        512 =          *.pft
//  00000000 00000000 00000100 00000000       1024 =          *.pft (Cisis)
//  00000000 00000000 00001000 00000000       2048 =          *.fmt
//  00000000 00000000 00010000 00000000       4096 =          *.stw
//  00000000 00000000 00100000 00000000       8192 =          *.fdt
//  00000000 00000000 01000000 00000000      16384 = ISO in   *.iso
//  00000000 00000000 10000000 00000000      32738 = ISO out  *.iso

//  00000000 00000001 00000000 00000000      65536 = Cipar    *.par
//  00000000 00000010 00000000 00000000     131072 = Gizmo
//  00000000 00000100 00000000 00000000     262144 = Decode

//  00000001 00000000 00000000 00000000   16777216 =          *.any   */

#endif /* ISIS001_H */

