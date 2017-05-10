/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//   Copyright (c) 1996 by

//   United Nations Educational Scientific and Cultural Organization.
//                                &
//   Latin American and Caribbean Center on Health Sciences Information /
//   PAHO-WHO.

//   All Rights Reserved.
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

#ifndef ISISDLL_H
#define ISISDLL_H

#include <windows.h>

/*-----------------------------------------------------------
//------------------- DEFINES -------------------------------
//-----------------------------------------------------------*/

#define TOCHAR(x) (char *)&x

#ifndef ErrorCode
#define ErrorCode long
#endif /* ErrorCode */

/*-----------------------------------------------------------*/

#ifdef __cplusplus
extern "C" {
#endif

/*-------------------- ISIS_DLL application functions --------------------*/

ErrorCode
CALLBACK
         IsisAppAcTab (long  apphandle,
                       char *actab);

long
CALLBACK
         IsisAppDebug (long apphandle,
                       long debugflag);

ErrorCode
CALLBACK
         IsisAppDelete (long apphandle);

ErrorCode
CALLBACK
         IsisAppLogFile (long  apphandle,
                         char *filename);

long
CALLBACK
         IsisAppNew (void);


ErrorCode
CALLBACK
         IsisAppParGet (long  apphandle,
                        char *parinp,
                        char *paroutp,
                        long  areasize);

long
CALLBACK
         IsisAppParSet (long  apphandle,
                        char *appareap);

ErrorCode
CALLBACK
         IsisAppUcTab (long  apphandle,
                       char *uctab);


/*-------------------- ISIS_DLL dll functions ----------------------------*/

float
CALLBACK
         IsisDllVersion (void);


/*-------------------- ISIS_DLL link functions -------------------------*/

long
CALLBACK
         IsisLnkIfLoad (long handle);


long
CALLBACK
         IsisLnkIfLoadEx (long handle,
                          long reset,
                          long posts,
                          long balan);

ErrorCode
CALLBACK
         IsisLnkSort (long handle);



/*-------------------- ISIS_DLL record functions -------------------------*/

long
CALLBACK
         IsisRecControlMap (long  handle,
                            char *ctrl);

ErrorCode
CALLBACK
         IsisRecCopy (long handle_from,
                      long index_from,
                      long handle_to,
                      long index_to);

long
CALLBACK
         IsisRecDirMap (long  handle,
                        long  index,
                        long  firstpos,
                        long  lastpos,
                        char *dir);

long
CALLBACK
         IsisRecDummy (long handle,
                       long index);

long
CALLBACK
         IsisRecDump (long  handle,
                      long  index,
                      char *dump,
                      long  areasize);

long
CALLBACK
         IsisRecField (long  handle,
                       long  index,
                       long  tag,
                       long  occ,
                       char *field_area,
                       long  areasize);

long
CALLBACK
         IsisRecFieldN (long  handle,
                        long  index,
                        long  pos,
                        char *field_area,
                        long  areasize);

long
CALLBACK
         IsisRecFieldOcc (long handle,
                          long index,
                          long tag);

ErrorCode
CALLBACK
         IsisRecFieldUpdate (long  handle,
                             long  index,
                             char *fldupd);

long
CALLBACK
         IsisRecFormat (long  handle,
                        long  index,
                        char *fareap,
                        long  areasize);

long
CALLBACK
         IsisRecFormatCisis (long  handle,
                             long  index,
                             char *fareap,
                             long  fareasize);

long
CALLBACK
         IsisRecFormatCisisEx (long  handle,
                               long  index,
                               long  linesize,
                               char *fareap,
                               long  fareasize);

long
CALLBACK
         IsisRecFormatEx (long  handle,
                          long  index,
                          long  linesize,
                          char *fareap,
                          long  areasize);

ErrorCode
CALLBACK
         IsisRecIfUpdate (long handle,
                          long mfn);

ErrorCode
CALLBACK
         IsisRecIfUpdateEx (long handle,
                            long beginmfn,
                            long endmfn,
                            long keeppending);

long
CALLBACK
         IsisRecIsoRead (long handle,
                         long index);

long
CALLBACK
         IsisRecIsoWrite (long handle,
                          long index);

long
CALLBACK
         IsisRecLeaderMap (long  handle,
                           long  index,
                           char *leader);

long
CALLBACK
         IsisRecLnk (long handle,
                     long beginmfn,
                     long endmfn);

ErrorCode
CALLBACK
         IsisRecLockRecall (long  handle,
                            long  index,
                            long  mfn,
                            long  tag,
                            char* password);

ErrorCode
CALLBACK
         IsisRecMerge (long handle_from,
                       long index_from,
                       long handle_to,
                       long index_to);

long
CALLBACK
         IsisRecMfn (long handle,
                     long index);

long
CALLBACK
         IsisRecMfnChange (long handle,
                           long index,
                           long mfn);

long
CALLBACK
         IsisRecNew (long handle,
                     long index);

long
CALLBACK
         IsisRecNewLock (long handle,
                         long index);

long
CALLBACK
         IsisRecNvf (long handle,
                     long index);

ErrorCode
CALLBACK
         IsisRecRead (long handle,
                      long index,
                      long mfn);

ErrorCode
CALLBACK
         IsisRecReadLock (long handle,
                          long index,
                          long mfn);

ErrorCode
CALLBACK
         IsisRecShelfSize (long handle,
                           long index,
                           long mem);

long
CALLBACK
         IsisRecSubField (long  handle,
                          long  index,
                          long  tag,
                          long  fldocc,
                          char *subfield,
                          char *subfield_area,
                          long  areasize);

long
CALLBACK
         IsisRecSubFieldEx (long  handle,
                            long  index,
                            long  tag,
                            long  fldocc,
                            char *subfield,
                            long  subfldocc,
                            char *subfield_area,
                            long  areasize);

ErrorCode
CALLBACK
         IsisRecUndelete (long handle,
                          long index);

ErrorCode
CALLBACK
         IsisRecUnlock (long handle,
                        long index);

ErrorCode
CALLBACK
         IsisRecUnlockForce (long handle,
                             long index);

long
CALLBACK
         IsisRecUpdate (long  handle,
                        long  index,
                        char *sparser);

ErrorCode
CALLBACK
         IsisRecWrite (long  handle,
                       long  index);

ErrorCode
CALLBACK
         IsisRecWriteLock (long handle,
                           long index);

ErrorCode
CALLBACK
         IsisRecWriteUnlock (long handle,
                             long index);


/*-------------------- ISIS_DLL space functions -------------------------*/

ErrorCode
CALLBACK
         IsisSpaDb (long  handle,
                    char *dbname);

ErrorCode
CALLBACK
         IsisSpaDf (long  handle,
                    char *decname);

ErrorCode
CALLBACK
         IsisSpaDelete (long handle);


ErrorCode
CALLBACK
         IsisSpaFdt (long  handle,
                     char *fdtname);

ErrorCode
CALLBACK
         IsisSpaFmt (long  handle,
                     char *fmtarea);

ErrorCode
CALLBACK
         IsisSpaFst (long  handle,
                     char *fstname);

ErrorCode
CALLBACK
         IsisSpaGf (long  handle,
                    char *gizname);

long
CALLBACK
         IsisSpaHeaderMap (long  handle,
                           char *header);

ErrorCode
CALLBACK
         IsisSpaIf (long  handle,
                    char *ifname);

long
CALLBACK
         IsisSpaIfCreate (long  handle);

ErrorCode
CALLBACK
         IsisSpaIsoDelim (long  handle,
                          char *recdelim,
                          char *fielddelim);

ErrorCode
CALLBACK
         IsisSpaIsoIn (long  handle,
                       char *filename);

ErrorCode
CALLBACK
         IsisSpaIsoOut (long  handle,
                        char *filename);

ErrorCode
CALLBACK
         IsisSpaIsoOutCreate (long handle);

ErrorCode
CALLBACK
         IsisSpaLnkFix (long handle,
                        long ifix,
                        long ofix);

ErrorCode
CALLBACK
         IsisSpaMf (long  handle,
                    char *mfname);

long
CALLBACK
         IsisSpaMfCreate (long handle);

ErrorCode
CALLBACK
         IsisSpaMfUnlockForce (long handle);

ErrorCode
CALLBACK
         IsisSpaMultiOff (long handle);

ErrorCode
CALLBACK
         IsisSpaMultiOn (long handle);

long
CALLBACK
         IsisSpaName (long  handle,
                      char *sname);

long
CALLBACK
         IsisSpaNew (long  apphandle);

ErrorCode
CALLBACK
         IsisSpaPft (long  handle,
                     char *format);

ErrorCode
CALLBACK
         IsisSpaPftCisis (long  handle,
                          char *format);

ErrorCode
CALLBACK
         IsisSpaRecDelim (long  handle,
                          char *begindelim,
                          char *enddelim);

ErrorCode
CALLBACK
         IsisSpaRecShelves (long handle,
                            long max_mst);

ErrorCode
CALLBACK
         IsisSpaStw (long  handle,
                     char *stwname);

ErrorCode
CALLBACK
         IsisSpaTrmShelves (long handle,
                            long max_trm);


/*-------------------- ISIS_DLL search functions -------------------------*/

long
CALLBACK
         IsisSrcHeaderMap (long  apphandle,
                           long  tsfnum,
                           long  searchnum,
                           char *sstrup);

long
CALLBACK
         IsisSrcHitMap (long  apphandle,
                        long  tsfnum,
                        long  searchnum,
                        long  firstpos,
                        long  lastpos,
                        char *hstrup);

ErrorCode
CALLBACK
         IsisSrcLogFileFlush (long  apphandle,
                              long  tsfnum);

ErrorCode
CALLBACK
         IsisSrcLogFileSave (long  apphandle,
                             long  tsfnum,
                             char *filename);

ErrorCode
CALLBACK
         IsisSrcLogFileUse (long  apphandle,
                            long  tsfnum,
                            char *filename);

long
CALLBACK
         IsisSrcMfnMap (long  apphandle,
                        long  tsfnum,
                        long  searchnum,
                        long  firstpos,
                        long  lastpos,
                        char *mfnareap);

long
CALLBACK
         IsisSrcSearch (long  handle,
                        long  tsfnum,
                        char *express,
                        char *areap);


/*-------------------- ISIS_DLL term functions -------------------------*/

long
CALLBACK
         IsisTrmMfnMap (long  handle,
                        long  index_trm,
                        long  firstpos,
                        long  lastpos,
                        char *mfnareap);

long
CALLBACK
         IsisTrmPostingMap (long  handle,
                            long  index_trm,
                            long  firstpos,
                            long  lastpos,
                            char *postings);

long
CALLBACK
         IsisTrmReadMap (long  handle,
                         long  index_trm,
                         char *key);

long
CALLBACK
         IsisTrmReadNext (long  handle,
                          long  index_trm,
                          char *key);

long
CALLBACK
         IsisTrmReadPrevious (long  handle,
                              long  index_trm,
                              char *prefix,
                              char *key);

ErrorCode
CALLBACK
         IsisTrmShelfSize (long  handle,
                           long  index_trm,
                           long  mem);


#ifdef __cplusplus
}
#endif /* extern "C" */

#endif /* ISISDLL_H */

