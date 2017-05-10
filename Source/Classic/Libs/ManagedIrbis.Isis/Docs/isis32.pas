{~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
'   Copyright (c) 1996 by

'   United Nations Educational Scientific and Cultural Organization.
'                                &
'   Latin American and Caribbean Center on Health Sciences Information /
'   PAHO-WHO.

'   All Rights Reserved.
'~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~}

unit ISIS32;

interface
uses Isis001;

{-------------------- ISIS_DLL application functions --------------------}

 Function IsisAppAcTab(AppHandle:LongInt;AcTab:PChar):LongInt;far;stdcall;
 Function IsisAppDebug(AppHandle:LongInt;Flag:LongInt):LongInt;far;stdcall;
 Function IsisAppDelete(AppHandle:LongInt):LongInt;far;stdcall;
 Function IsisAppLogFile(AppHandle:LongInt;FileName:PChar):LongInt;far;stdcall;
 Function IsisAppNew:LongInt;far;stdcall;
 Function IsisAppParGet(AppHandle:LongInt;ParIn:PChar;ParOut:PChar;AreaSize:LongInt):LongInt;far;stdcall;
 Function IsisAppParSet(AppHandle:LongInt;AppArea:PChar):LongInt;far;stdcall;
 Function IsisAppUcTab(AppHandle:LongInt;UcTab:PChar):LongInt;far;stdcall;


{-------------------- ISIS_DLL dll functions ----------------------------}

 Function IsisDllVersion:single;far;stdcall;


{------------------- ISIS_DLL link functions ----------------------------}

 Function IsisLnkIfLoad(Handle:LongInt):LongInt;far;stdcall;
 Function IsisLnkIfLoadEx(Handle:LongInt;Reset:LongInt;Posts:LongInt;Balan:LongInt):LongInt;far;stdcall;
 Function IsisLnkSort(Handle:LongInt):LongInt;far;stdcall;


{-------------------- ISIS_DLL record functions -------------------------}

 Function IsisRecControlMap(Handle:LongInt;var P:IsisRecControl):LongInt;far;stdcall;
 Function IsisRecCopy(HandleFrom:LongInt;IndexFrom:LongInt;HandleTo:LongInt;IndexTo:LongInt):LongInt;far;stdcall;
 Function IsisRecDirMap(Handle:LongInt;Index:LongInt;var P:IsisRecDir):LongInt;far;stdcall;
 Function IsisRecDummy(Handle:LongInt;Index:LongInt):LongInt;far;stdcall;
 Function IsisRecDump(Handle:LongInt;Index:LongInt;FieldArea:PChar;AreaSize:LongInt):LongInt;far;stdcall;
 Function IsisRecField(Handle:LongInt;Index:LongInt;Tag:LongInt;Occ:LongInt;FieldArea:PChar;AreaSize:LongInt):LongInt;far;stdcall;
 Function IsisRecFieldN(Handle:LongInt;Index:LongInt;Pos:LongInt;FieldArea:PChar;AreaSize:LongInt):LongInt;far;stdcall;
 Function IsisRecFieldOcc(Handle:LongInt;Index:LongInt;Tag:LongInt):LongInt;far;stdcall;
 Function IsisRecFieldUpdate(Handle:LongInt;Index:LongInt;FldUpd:PChar):LongInt;far;stdcall;
 Function IsisRecFormat(Handle:LongInt;Index:LongInt;Farea:PChar;AreaSize:LongInt):LongInt;far;stdcall;
 Function IsisRecFormatCisis(Handle:LongInt;Index:LongInt;Farea:PChar;FareaSize:LongInt):LongInt;far;stdcall;
 Function IsisRecFormatCisisEx(Handle:LongInt;Index:LongInt;LineSize:LongInt;Farea:PChar;FareaSize:LongInt):LongInt;far;stdcall;
 Function IsisRecFormatEx(Handle:LongInt;Index:LongInt;LineSize:LongInt;Farea:PChar;AreaSize:LongInt):LongInt;far;stdcall;
 Function IsisRecIfUpdate(Handle:LongInt;Mfn:LongInt):LongInt;far;stdcall;
 Function IsisRecIfUpdateEx(Handle:LongInt;BeginMfn:LongInt;EndMfn:LongInt;KeepPending:LongInt):LongInt;far;stdcall;
 Function IsisRecIsoRead(Handle:LongInt;Index:LongInt):LongInt;far;stdcall;
 Function IsisRecIsoWrite(Handle:LongInt; Index:LongInt):LongInt;far;stdcall;
 Function IsisRecLeaderMap(Handle:LongInt;Index:LongInt;var P:IsisRecLeader):LongInt;far;stdcall;
 Function IsisRecLnk(Handle:LongInt;BeginMfn:LongInt;EndMfn:LongInt):LongInt;far;stdcall;
 Function IsisRecMerge(HandleFrom:LongInt;IndexFrom:LongInt;HandleTo:LongInt;IndexTo:LongInt):LongInt;far;stdcall;
 Function IsisRecMfn(Handle:LongInt;Index:LongInt):LongInt;far;stdcall;
 Function IsisRecMfnChange(Handle:LongInt;Index:LongInt;Mfn:LongInt):LongInt;far;stdcall;
 Function IsisRecNew(Handle:LongInt;Index:LongInt):LongInt;far;stdcall;
 Function IsisRecNewLock(Handle:LongInt;Index:LongInt):LongInt;far;stdcall;
 Function IsisRecNvf(Handle:LongInt;Index:LongInt):LongInt;far;stdcall;
 Function IsisRecRead(Handle:LongInt;Index:LongInt;Mfn:LongInt):LongInt;far;stdcall;
 Function IsisRecReadLock(Handle:LongInt;Index:LongInt;Mfn:LongInt):LongInt;far;stdcall;
 Function IsisRecShelfSize(Handle:LongInt;Index:LongInt;Memory:LongInt):LongInt;far;stdcall;
 Function IsisRecSubField(Handle:LongInt;Index:LongInt;Tag:LongInt;FldOcc:LongInt;SubField:PChar;
			  SubFieldArea:PChar;AreaSize:LongInt):LongInt;far;stdcall;
 Function IsisRecSubFieldEx(Handle:LongInt;Index:LongInt;Tag:LongInt;FldOcc:LongInt;SubField:PChar;
			    SubFldOcc:LongInt;SubFieldArea:PChar;AreaSize:LongInt):LongInt;far;stdcall;
 Function IsisRecUndelete(Handle:LongInt;Index:LongInt):LongInt;far;stdcall;
 Function IsisRecUnlock(Handle:LongInt;Index:LongInt):LongInt;far;stdcall;
 Function IsisRecUnlockForce(Handle:LongInt;Index:LongInt):LongInt;far;stdcall;
 Function IsisRecUpdate(Handle:LongInt;Index:LongInt;FieldArea:PChar):LongInt;far;stdcall;
 Function IsisRecWrite(Handle:LongInt;Index:LongInt):LongInt;far;stdcall;
 Function IsisRecWriteLock(Handle:LongInt;Index:LongInt):LongInt;far;stdcall;
 Function IsisRecWriteUnlock(Handle:LongInt;Index:LongInt):LongInt;far;stdcall;


{-------------------- ISIS_DLL space functions -------------------------}

 Function IsisSpaDb(Handle:LongInt;NameBD:PChar):LongInt;far;stdcall;
 Function IsisSpaDf(Handle:LongInt;NameDf:PChar):LongInt;far;stdcall;
 Function IsisSpaDelete(Handle:LongInt):LongInt;far;stdcall;
 Function IsisSpaFmt(Handle:LongInt;NameFmt:PChar):LongInt;far;stdcall;
 Function IsisSpaFst(Handle:LongInt;NameFst:PChar):LongInt;far;stdcall;
 Function IsisSpaGf(Handle:LongInt;NameGf:PChar):LongInt;far;stdcall;
 Function IsisSpaHeaderMap(Handle:LongInt;var P:IsisSpaHeader):LongInt;far;stdcall;
 Function IsisSpaIf(Handle:LongInt;NameIf:PChar):LongInt;far;stdcall;
 Function IsisSpaIfCreate(Handle:LongInt):LongInt;far;stdcall;
 Function IsisSpaIsoDelim(Handle:LongInt;RecDelim:PChar;FieldDelim:PChar):LongInt;far;stdcall;
 Function IsisSpaIsoIn(Handle:LongInt;FileName:PChar):LongInt;far;stdcall;
 Function IsisSpaIsoOut(Handle:LongInt;FileName:PChar):LongInt;far;stdcall;
 Function IsisSpaIsoOutCreate(Handle:LongInt):LongInt;far;stdcall;
 Function IsisSpaLnkFix(Handle:LongInt;IFix,OFix:LongInt):LongInt;far;stdcall;
 Function IsisSpaMf(Handle:LongInt;NameMst:PChar):LongInt;far;stdcall;
 Function IsisSpaMfCreate(Handle:LongInt):LongInt;far;stdcall;
 Function IsisSpaMfUnlockForce(Handle:LongInt):LongInt;far;stdcall;
 Function IsisSpaMultiOff(Handle:LongInt):LongInt;far;stdcall;
 Function IsisSpaMultiOn(Handle:LongInt):LongInt;far;stdcall;
 Function IsisSpaName(Handle:LongInt;NameSpace:PChar):LongInt;far;stdcall;
 Function IsisSpaNew(AppHandle:LongInt):LongInt;far;stdcall;
 Function IsisSpaPft(Handle:LongInt;NamePft:PChar):LongInt;far;stdcall;
 Function IsisSpaPftCisis(Handle:LongInt;NamePft:PChar):LongInt;far;stdcall;
 Function IsisSpaRecDelim(Handle:LongInt;BeginDelim:PChar;EndDelim:PChar):LongInt;far;stdcall;
 Function IsisSpaRecShelves(Handle:LongInt;MaxMst:LongInt):LongInt;far;stdcall;
 Function IsisSpaStw(Handle:LongInt;NameStw:PChar):LongInt;far;stdcall;
 Function IsisSpaTrmShelves(Handle:LongInt;MaxMst:LongInt):LongInt;far;stdcall;


{-------------------- ISIS_DLL search functions -------------------------}

 Function IsisSrcHeaderMap(AppHandle:LongInt;TSFNum:LongInt;SearchNo:LongInt;var P:IsisSrcHeader):LongInt;far;stdcall;
 Function IsisSrcHitMap(AppHandle:LongInt;TSFNum:LongInt;SearchNo:LongInt;FirstPos:LongInt;LastPos:LongInt;
			var P:IsisSrcHit):LongInt;far;stdcall;
 Function IsisSrcLogFileFlush(AppHandle:LongInt;TSFNum:LongInt):LongInt;far;stdcall;
 Function IsisSrcLogFileSave(AppHandle:LongInt;TSFNum:LongInt;FileName:PChar):LongInt;far;stdcall;
 Function IsisSrcLogFileUse(AppHandle:LongInt;TSFNum:LongInt;FileName:PChar):LongInt;far;stdcall;
 Function IsisSrcMfnMap(AppHandle:LongInt;TSFNum:LongInt;SearchNo:LongInt;FirstPos:LongInt;
			LastPos:LongInt;var P:IsisSrcMfn):LongInt;far;stdcall;
 Function IsisSrcSearch(Handle:LongInt;TSFNum:LongInt;Boolean:PChar;var P:IsisSrcHeader):LongInt;far;stdcall;


{-------------------- ISIS_DLL term functions -------------------------}

 Function IsisTrmMfnMap(Handle:LongInt;Index:LongInt;FirstPos:LongInt;LastPos:LongInt;var P:IsisTrmMfn):LongInt;far;stdcall;
 Function IsisTrmPostingMap(Handle:LongInt;Index:LongInt;FirstPos:LongInt;LastPos:LongInt;
			    var P:IsisTrmPosting):LongInt;far;stdcall;
 Function IsisTrmReadMap(Handle:LongInt;Index:LongInt;var P:IsisTrmRead):LongInt;far;stdcall;
 Function IsisTrmReadNext(Handle:LongInt;Index:LongInt;var P:IsisTrmRead):LongInt;far;stdcall;
 Function IsisTrmReadPrevious(Handle:LongInt;Index:LongInt;Prefix:Pchar;var P:IsisTrmRead):LongInt;far;stdcall;
 Function IsisTrmShelfSize(Handle:LongInt;Index:LongInt;Memory:LongInt):LongInt;far;stdcall;


{-------------------- General functions -------------------------------}

 Function OemToCharBuff(lpszSrc:PChar;lpszDst:PChar;cchDstLength:LongInt):LongInt;far;stdcall;
 Function CharToOemBuff(lpszSrc:PChar;lpszDst:PChar;cchDstLength:LongInt):LongInt;far;stdcall;
 Function SetHandleCount(wNumber:LongInt):LongInt;far;stdcall;



implementation

{-------------------- ISIS_DLL application functions --------------------}

 Function IsisAppAcTab(AppHandle:LongInt;AcTab:PChar):LongInt;
	  external 'isis32';
 Function IsisAppDebug(AppHandle:LongInt;Flag:LongInt):LongInt;
	  external 'isis32';
 Function IsisAppDelete(AppHandle:LongInt):LongInt;
	  external 'isis32';
 Function IsisAppLogFile(AppHandle:LongInt;FileName:PChar):LongInt;
	  external 'isis32';
 Function IsisAppNew:LongInt;
	  external 'isis32';
 Function IsisAppParGet(AppHandle:LongInt;ParIn:PChar;ParOut:PChar;AreaSize:LongInt):LongInt;
	  external 'isis32';
 Function IsisAppParSet(AppHandle:LongInt;AppArea:PChar):LongInt;
	  external 'isis32';
 Function IsisAppUcTab(AppHandle:LongInt;UcTab:PChar):LongInt;
	  external 'isis32';


{------------------- ISIS_DLL dll functions ----------------------------}

 Function IsisDllVersion:single;
	  external 'isis32';


{------------------- ISIS_DLL link functions ----------------------------}

 Function IsisLnkIfLoad(Handle:LongInt):LongInt;
	  external 'isis32';
 Function IsisLnkIfLoadEx(Handle:LongInt;Reset:LongInt;Posts:LongInt;Balan:LongInt):LongInt;
	  external 'isis32';
 Function IsisLnkSort(Handle:LongInt):LongInt;
	  external 'isis32';


{------------------- ISIS_DLL record functions -------------------------}

 Function IsisRecControlMap(Handle:LongInt;var P:IsisRecControl):LongInt;
	  external 'isis32';
 Function IsisRecCopy(HandleFrom:LongInt;IndexFrom:LongInt;HandleTo:LongInt;IndexTo:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecDirMap(Handle:LongInt;Index:LongInt;var P:IsisRecDir):LongInt;
	  external 'isis32';
 Function IsisRecDummy(Handle:LongInt;Index:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecDump(Handle:LongInt;Index:LongInt;FieldArea:PChar;AreaSize:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecField(Handle:LongInt;Index:LongInt;Tag:LongInt;Occ:LongInt;FieldArea:PChar;AreaSize:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecFieldN(Handle:LongInt;Index:LongInt;Pos:LongInt;FieldArea:PChar;AreaSize:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecFieldOcc(Handle:LongInt;Index:LongInt;Tag:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecFieldUpdate(Handle:LongInt;Index:LongInt;FldUpd:PChar):LongInt;
	  external 'isis32';
 Function IsisRecFormat(Handle:LongInt;Index:LongInt;Farea:PChar;AreaSize:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecFormatCisis(Handle:LongInt;Index:LongInt;Farea:PChar;FareaSize:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecFormatCisisEx(Handle:LongInt;Index:LongInt;LineSize:LongInt;Farea:PChar;FareaSize:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecFormatEx(Handle:LongInt;Index:LongInt;LineSize:LongInt;Farea:PChar;AreaSize:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecIfUpdate(Handle:LongInt;Mfn:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecIfUpdateEx(Handle:LongInt;BeginMfn:LongInt;EndMfn:LongInt;KeepPending:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecIsoRead(Handle:LongInt;Index:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecIsoWrite(Handle:LongInt; Index:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecLeaderMap(Handle:LongInt;Index:LongInt;var P:IsisRecLeader):LongInt;
	  external 'isis32';
 Function IsisRecLnk(Handle:LongInt;BeginMfn:LongInt;EndMfn:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecMerge(HandleFrom:LongInt;IndexFrom:LongInt;HandleTo:LongInt;IndexTo:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecMfn(Handle:LongInt;Index:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecMfnChange(Handle:LongInt;Index:LongInt;Mfn:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecNew(Handle:LongInt;Index:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecNewLock(Handle:LongInt;Index:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecNvf(Handle:LongInt;Index:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecRead(Handle:LongInt;Index:LongInt;Mfn:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecReadLock(Handle:LongInt;Index:LongInt;Mfn:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecShelfSize(Handle:LongInt;Index:LongInt;Memory:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecSubField(Handle:LongInt;Index:LongInt;Tag:LongInt;FldOcc:LongInt;Subfield:PChar;SubFieldArea:PChar;AreaSize:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecSubFieldEx(Handle:LongInt;Index:LongInt;Tag:LongInt;FldOcc:LongInt;Subfield:PChar;
			    SubFldOcc:LongInt;SubFieldArea:PChar;AreaSize:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecUndelete(Handle:LongInt;Index:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecUnlock(Handle:LongInt;Index:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecUnlockForce(Handle:LongInt;Index:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecUpdate(Handle:LongInt;Index:LongInt;FieldArea:PChar):LongInt;
	  external 'isis32';
 Function IsisRecWrite(Handle:LongInt;Index:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecWriteLock(Handle:LongInt;Index:LongInt):LongInt;
	  external 'isis32';
 Function IsisRecWriteUnlock(Handle:LongInt;Index:LongInt):LongInt;
	  external 'isis32';


{-------------------- ISIS_DLL space functions -------------------------}

 Function IsisSpaDb(Handle:LongInt;NameBD:PChar):LongInt;
	  external 'isis32';
 Function IsisSpaDf(Handle:LongInt;NameDf:PChar):LongInt;
	  external 'isis32';
 Function IsisSpaDelete(Handle:LongInt):LongInt;
	  external 'isis32';
 Function IsisSpaFmt(Handle:LongInt;NameFmt:PChar):LongInt;
	  external 'isis32';
 Function IsisSpaFst(Handle:LongInt;NameFst:PChar):LongInt;
	  external 'isis32';
 Function IsisSpaGf(Handle:LongInt;NameGf:PChar):LongInt;
	  external 'isis32';
 Function IsisSpaHeaderMap(Handle:LongInt;var P:IsisSpaHeader):LongInt;
	  external 'isis32';
 Function IsisSpaIf(Handle:LongInt;NameIf:PChar):LongInt;
	  external 'isis32';
 Function IsisSpaIfCreate(Handle:LongInt):LongInt;
	  external 'isis32';
 Function IsisSpaIsoDelim(Handle:LongInt;RecDelim:PChar;FieldDelim:PChar):LongInt;
	  external 'isis32';
 Function IsisSpaIsoIn(Handle:LongInt;FileName:PChar):LongInt;
	  external 'isis32';
 Function IsisSpaIsoOut(Handle:LongInt;FileName:PChar):LongInt;
	  external 'isis32';
 Function IsisSpaIsoOutCreate(Handle:LongInt):LongInt;
	  external 'isis32';
 Function IsisSpaLnkFix(Handle:LongInt;IFix,OFix:LongInt):LongInt;
	  external 'isis32';
 Function IsisSpaMf(Handle:LongInt;NameMst:PChar):LongInt;
	  external 'isis32';
 Function IsisSpaMfCreate(Handle:LongInt):LongInt;
	  external 'isis32';
 Function IsisSpaMfUnlockForce(Handle:LongInt):LongInt;
	  external 'isis32';
 Function IsisSpaMultiOff(Handle:LongInt):LongInt;
	  external 'isis32';
 Function IsisSpaMultiOn(Handle:LongInt):LongInt;
	  external 'isis32';
 Function IsisSpaName(Handle:LongInt;NameSpace:PChar):LongInt;
	  external 'isis32';
 Function IsisSpaNew(AppHandle:LongInt):LongInt;
	  external 'isis32';
 Function IsisSpaPft(Handle:LongInt;NamePft:PChar):LongInt;
	  external 'isis32';
 Function IsisSpaPftCisis(Handle:LongInt;NamePft:PChar):LongInt;
	  external 'isis32';
 Function IsisSpaRecDelim(Handle:LongInt;BeginDelim:PChar;EndDelim:PChar):LongInt;
	  external 'isis32';
 Function IsisSpaRecShelves(Handle:LongInt;MaxMst:LongInt):LongInt;
	  external 'isis32';
 Function IsisSpaStw(Handle:LongInt;NameStw:PChar):LongInt;
	  external 'isis32';
 Function IsisSpaTrmShelves(Handle:LongInt;MaxMst:LongInt):LongInt;
	  external 'isis32';


{-------------------- ISIS_DLL search functions -------------------------}

 Function IsisSrcHeaderMap(AppHandle:LongInt;TSFNum:LongInt;SearchNo:LongInt;var P:IsisSrcHeader):LongInt;
	  external 'isis32';
 Function IsisSrcHitMap(AppHandle:LongInt;TSFNum:LongInt;SearchNo:LongInt;FirstPos:LongInt;LastPos:LongInt;
			var P:IsisSrcHit):LongInt;
	  external 'isis32';
 Function IsisSrcLogFileFlush(AppHandle:LongInt;TSFNum:LongInt):LongInt;
	  external 'isis32';
 Function IsisSrcLogFileSave(AppHandle:LongInt;TSFNum:LongInt;FileName:PChar):LongInt;
	  external 'isis32';
 Function IsisSrcLogFileUse(AppHandle:LongInt;TSFNum:LongInt;FileName:PChar):LongInt;
	  external 'isis32';
 Function IsisSrcMfnMap(AppHandle:LongInt;TSFNum:LongInt;SearchNo:LongInt;FirstPos:LongInt;
			LastPos:LongInt;var P:IsisSrcMfn):LongInt;
	  external 'isis32';
 Function IsisSrcSearch(Handle:LongInt;TSFNum:LongInt;Boolean:PChar;var P:IsisSrcHeader):LongInt;
	  external 'isis32';


{-------------------- ISIS_DLL term functions -------------------------}

 Function IsisTrmMfnMap(Handle:LongInt;Index:LongInt;FirstPos:LongInt;LastPos:LongInt;var P:IsisTrmMfn):LongInt;
	  external 'isis32';
 Function IsisTrmPostingMap(Handle:LongInt;Index:LongInt;FirstPos:LongInt;LastPos:LongInt;
			    var P:IsisTrmPosting):LongInt;
	  external 'isis32';
 Function IsisTrmReadMap(Handle:LongInt;Index:LongInt;var P:IsisTrmRead):LongInt;
	  external 'isis32';
 Function IsisTrmReadNext(Handle:LongInt;Index:LongInt;var P:IsisTrmRead):LongInt;
	  external 'isis32';
 Function IsisTrmReadPrevious(Handle:LongInt;Index:LongInt;Prefix:Pchar;var P:IsisTrmRead):LongInt;
	  external 'isis32';
 Function IsisTrmShelfSize(Handle:LongInt;Index:LongInt;Memory:LongInt):LongInt;
	  external 'isis32';


{-------------------- General functions -------------------------------}

 Function OemToCharBuff(lpszSrc:PChar;lpszDst:PChar;cchDstLength:LongInt):LongInt;
	 external  'user32';
 Function CharToOemBuff(lpszSrc:PChar;lpszDst:PChar;cchDstLength:LongInt):LongInt;
	 external  'user32';
 Function SetHandleCount(wNumber:LongInt):LongInt;
	 external  'kernel32';

 end.


