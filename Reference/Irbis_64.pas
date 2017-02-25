unit Irbis_64;

interface

uses irbis_01,classes;


{
function IrbisSearch_Range(CurSpace:PIrbisSpace; MTerms: TStringList; MLogic: byte; MPortion: integer; MResultList: TIntList): integer;export;
function IrbisFreeIrbisSearch(MIf: PChar; MSex: PChar; MResultList: TIntList): integer;export;
 }
                             {
function  ThreadFormatGroup(ResultList:TStringList;
                       DBname:PChar;
                       PFT_Line_: PChar;
                       AltShelf_:integer;
                       MFNList:TIntList;
                       AddFields_:TStringList;
                       uctab,lctab,actab,aExecDir,aDataPath:PChar;
                       const ADepositPath:PChar;
                       ACashable,AUnifor6_Is_Precompiled,AERROR_FIRST_BREAK:boolean;
                       AIniFile:TIniFile
                       ):integer;far;

function  ThreadFormat(CurSpace:PIrbisSpace;
                       DBname:PChar;
                       PFT_Line_: PChar;
                       AltShelf_:integer;
                       MFN:integer;
                       AddFields_:TStringList;
                       uctab,lctab,actab,aExecDir,aDataPath:PChar;
                       const ADepositPath:PChar;
                       ACashable,AUnifor6_Is_Precompiled,AERROR_FIRST_BREAK:boolean;
                       AIniFile:TIniFile
                       ):integer;far
                              }
{функция инициализации Space вызывается первой!!!!!!}
function Irbisinit:PIrbisSpace;far;
{создание 5 файлов новой БД}
function IrbisinitNewDB(Path:PChar):longint;far;
{скрыто выполняет все закрытия файлов и освобождение памяти irbisclosemst,irbisclose term}
procedure Irbisclose(SP:PIrbisSpace);far;
{открывает мастер файл на чтение-запись DataBase - полный путь на мастер файл БЕЗ РАСШИРЕНИЯ!!!}
function Irbisinitmst(SP:PIrbisSpace;DataBase: Pchar; ANumberShelfs: integer): integer; far;{открытие файлов MST & XRF}
procedure Irbisclosemst(SP:PIrbisSpace);far;
{открывает инверсный файл на чтение-запись DataBase - полный путь на инверсный файл БЕЗ РАСШИРЕНИЯ!!!}
function Irbisinitterm(PS:PIrbisSpace;line: Pchar): integer;far;
//function IrbisinittermIndex(PS:PIrbisSpace;line: Pchar;If_Index:integer): integer;far;
procedure Irbiscloseterm(PS: PIrbisSpace);far;
procedure IrbisInitUACTAB(PS: PIrbisSpace);far;
procedure IrbisInitInvContext(PS: PIrbisSpace;fst,stw,uctab,actab:PChar;deflex:boolean);far;
//путь вместе с расширением!    =    unicode.tab
//procedure IrbisInitUnicode(PS:PIrbisSpace;unicodetab:PChar);far;

function IrbisRecord(PS: PIrbisSpace; Shelf,mfn: longint): integer; far;{аналог RECORD}
function IrbisReadVersion(PS:PIrbisSpace;mfn:integer):integer;far;

{чтение копии с шагом назад step без блокировки}
function IrbisRecordBack(PS: PIrbisSpace; shelf,mfn: longint;step:integer): integer; far;{откат до старой копии step шагов}


{чтение с блокировкой записи}
function IrbisRecLock0(PS: PIrbisSpace; Shelf,mfn: longint): integer; far;
function IrbisRecUnLock0(PS:PIrbisSpace;mfn:integer):integer;far;
function IrbisRecUpdate0(PS:PIrbisSpace;Shelf,KeepLock:integer):integer;far;
function IrbisRecIfUpdate0(PS:PIrbisSpace;AShelf, mfn:integer):integer;far;

//new function for writing
function IrbisDBEmptyTime(IrbisSpace:PIrbisSpace;seconds:integer):integer;far;
function IrbisLockDBTime(IrbisSpace:PIrbisSpace;seconds:integer):integer;far;
function IrbisUnLockDBTime(IrbisSpace:PIrbisSpace;seconds:integer):integer;far;
function IrbisRecUpdateTime(IrbisSpace:PIrbisSpace;Shelf:integer;KeepLock:integer;Updif:boolean;seconds:integer;var result_update:integer;var result_updif:integer):integer;far;
function IrbisRecIfUpdateTime(IrbisSpace:PIrbisSpace;Shelf, mfn:integer;seconds:integer):integer;far;
//15.07.2015
function IrbisRecIfUpdateFullTextTime(IrbisSpace:PIrbisSpace;seconds:integer;TermsFileName:PChar;Delete:integer):integer;far;
//IRBIS+
//ставит или снимает бит неактуализированности записи по полным текстам
function IrbisSetFullTextActualizedBitTime(IrbisSpace:PIrbisSpace;mfn,FullTextAcualized,seconds:integer):integer;far;


function IrbisRecLockTime(IrbisSpace: PIrbisSpace; Shelf,mfn: longint;seconds:integer): integer;{аналог RECORD}far;
function IrbisRecUnLockTime(IrbisSpace:PIrbisSpace;mfn:integer;seconds:integer):integer;far;
//function IrbisRecPhysDelTime(IrbisSpace:PIrbisSpace;mfn:integer;seconds:integer):integer;far;

procedure IrbisNotActList(PS:PIrbisSpace;List:TIntList);far;
//function IrbisLockDB(PS:PIrbisSpace):integer;far;
//procedure IrbisUnLockDB(PS:PIrbisSpace);far;
{разблокировать запись}
//function IrbisRecUnLock(PS:PIrbisSpace;mfn:integer):integer;far;
function IrbisIsDBLocked(PS:PIrbisSpace):integer;far;
{запись заблокирована? - без чтения!!!!!!!! только проверка флага в XRF}
function IrbisIsRealyLocked(PS:PIrbisSpace; mfn:integer):integer;far;
function IrbisIsRealyActualized(PS:PIrbisSpace; mfn:integer):integer;far;

function IrbisIsLocked(PS:PIrbisSpace; Shelf:integer):integer;far;
function IrbisIsDeleted(PS:PIrbisSpace; Shelf:integer):integer;far;
function IrbisIsActualized(PS:PIrbisSpace; Shelf:integer):integer;far;

procedure Irbisrecdel(PS: PIrbisSpace; Shelf: longint);far;
procedure Irbisrecundelete(PS: PIrbisSpace; Shelf: longint);far;

function Irbisfieldn(PS: PIrbisSpace; Shelf,met,occ: integer): integer;far;{аналог FIELDN}
{function Irbisfield(PS: PIrbisSpace; Shelf,nf: integer; subfields: string): Pchar;  {аналог FIELD}
function Irbisfield(PS: PIrbisSpace; Shelf,nf: integer; subfields: PChar): Pchar;far;  {аналог FIELD}
function Irbisfldadd(PS: PIrbisSpace; Shelf,met, nf: integer; pole: Pchar): integer;far;{аналог FLDADD}
function Irbisfldrep(PS: PIrbisSpace; Shelf,nf: integer; pole: Pchar): integer;far;{если pole=nil ('') - удаляет!! аналог FLDREP}
function Irbisnfields(PS: PIrbisSpace;Shelf:integer): integer;far;  {аналог NFIELDS}
function Irbisnocc(PS: PIrbisSpace; Shelf,met: integer): integer;far; {аналог NOCC}
function Irbisfldtag(PS: PIrbisSpace; Shelf,nf: integer): integer; far;{аналог FLDTAG}
function Irbisfldempty(PS:PIrbisSpace;Shelf:integer):integer;far;{опустошает запись}
function Irbisnewrec(PS: PIrbisSpace; shelf: integer):integer;far;
function Irbischangemfn(PS: PIrbisSpace; shelf,newmfn: integer):integer;far;


function Irbismfn(PS: PIrbisSpace;shelf:integer): longint;far;
function Irbismaxmfn(PS: PIrbisSpace): longint;far;

function IrbisDBEmpty(PS: PIrbisSpace): longint;far;
{trm}
function Irbisfind(PS: PIrbisSpace; term: Pchar): integer;far;
function Irbisnxtterm(PS: PIrbisSpace; term: Pchar): integer;far;
function Irbisnposts(PS: PIrbisSpace): longint;far;
function Irbisposting(PS: PIrbisSpace; opt: smallint): longint;far;
function IrbisFindPosting(PS:PIrbisSpace;const Term:PChar;const posting:TifpItemPosting):integer;far;
function Irbisnxtpost(PS: PIrbisSpace): integer;far;
function Irbisprevterm(PS: PIrbisSpace; term: Pchar): integer;far;
function Irbisinitpost(PS: PIrbisSpace):integer;far;
function InsertTerm(PS:PIrbisSpace;const Term:PChar;const posting:TifpItemPosting):integer;far;
procedure IrbisDLLVersion(Abuf: Pchar; Abufsize: integer);far; //ALIO
function Irbis_InitPFT(PS: PIrbisSpace; Line: PChar): integer; far;  { analog GETFMT }
//function Irbis_InitPFT_Utf8(PS: PIrbisSpace; Line: PChar): integer; far;  { analog GETFMT }
function irbis_uatab_init(uctab,lctab,actab,aExecDir,aDataPath:PChar):integer;far;
function irbis_init_DepositPath(const ADepositPath:PChar):integer;far;
//procedure irbis_MainIni_Init(AIniFile:TIniFile);far;
procedure irbis_MainIni_Init(AIniFile:PChar);export;
{
function irbis_GetGlobalVarList():PChar;far;
procedure irbis_SetGlobalVarList(GlobalVarList_Text:PChar);far;
}
procedure irbis_set_options(ACashable,AUnifor6_Is_Precompiled,AERROR_FIRST_BREAK:boolean);far;
function Irbis_Format(PS: PIrbisSpace; shelf,alt_shelf,trm_shelf,LwLn: Integer;FmtExitDLL : PChar): integer; far;{ analog FORMAT }
//сохранение формата в кэш на диск в рабочую директорию

Function UNIFOR(H  : PIrbisSpace; { Handle       }
                Curr_Shelf : LongInt; { Shelf of Current record }
                Trm_Shelf  : LongInt; { Shelf of Term           }
                LW_Exit: integer;
                OCC_Exit: integer;
                SP1,SP2: Pchar): integer; far;
Function UMARCI(H  : PIrbisSpace; { Handle       }
                Curr_Shelf : LongInt; { Shelf of Current record }
                Trm_Shelf  : LongInt; { Shelf of Term           }
                LW_Exit: integer;
                OCC_Exit: integer;
                SP1,SP2: Pchar
                ): integer;  far;

{function IrbisLnk_Creat(PS:PIrbisSpace;Path_ln:PChar):integer;far;
function IrbisLnk_Load(PS:PIrbisSpace;Path_lk:PChar):integer;far;}

{
.....эти функции содержатся в lnk_load.pas termsort.pas.....
function IrbisLnkCreate(PS:PIrbisSpace;Path_ln:PChar):integer;far;
function IrbisLnkLoad(PS:PIrbisSpace;Path_lk:PChar):integer;far;
procedure IrbisLnkSort(line:PChar);far;

procedure IrbisRecTerms(PS:PIrbisSpace;AShelf:integer;line:Pchar;MfnList:TIntList);far;
function IrbisCreateBKP(PS:PIrbisSpace;line:PChar):integer;far;
function IrbisRestoreBKP(PS:PIrbisSpace;line:PChar):integer;far;
}

implementation

function Irbisinit:PIrbisSpace;external 'IRBIS64.DLL';
{создание 5 файлов новой БД}
function IrbisinitNewDB(Path:PChar):longint;external 'IRBIS64.DLL';
procedure Irbisclose(SP:PIrbisSpace);external 'IRBIS64.DLL';
function Irbisinitmst(SP:PIrbisSpace;DataBase: Pchar; ANumberShelfs: integer): integer; external 'IRBIS64.DLL';{открытие файлов MST & XRF}
procedure Irbisclosemst(SP:PIrbisSpace);external 'IRBIS64.DLL';
function Irbisinitterm(PS:PIrbisSpace;line: Pchar): integer;external 'IRBIS64.DLL';
//function IrbisinittermIndex(PS:PIrbisSpace;line: Pchar;If_Index:integer): integer;external 'IRBIS64T.DLL';
procedure Irbiscloseterm(PS: PIrbisSpace);external 'IRBIS64.DLL';
procedure IrbisInitUACTAB(PS: PIrbisSpace);external 'IRBIS64.DLL';
procedure IrbisInitInvContext(PS: PIrbisSpace;fst,stw,uctab,actab:PChar;deflex:boolean);external 'IRBIS64.DLL';
//procedure IrbisInitUnicode(PS:PIrbisSpace;unicodetab:PChar);external 'IRBIS64.DLL';

function IrbisRecord(PS: PIrbisSpace; Shelf,mfn: longint): integer; external 'IRBIS64.DLL';{аналог RECORD}
function IrbisReadVersion(PS:PIrbisSpace;mfn:integer):integer; external 'IRBIS64.DLL';


function IrbisRecordBack(PS: PIrbisSpace; shelf,mfn: longint;step:integer): integer; external 'IRBIS64.DLL';{откат до старой копии step шагов}

function IrbisRecLock0(PS: PIrbisSpace; Shelf,mfn: longint): integer; external 'IRBIS64.DLL';
function IrbisRecUnLock0(PS:PIrbisSpace;mfn:integer):integer;external 'IRBIS64.DLL';
function IrbisRecUpdate0(PS:PIrbisSpace;Shelf,KeepLock:integer):integer;external 'IRBIS64.DLL';
function IrbisRecIfUpdate0(PS:PIrbisSpace;AShelf, mfn:integer):integer;external 'IRBIS64.DLL';

procedure IrbisNotActList(PS:PIrbisSpace;List:TIntList);external 'IRBIS64.DLL';
{
function IrbisLockDB(PS:PIrbisSpace):integer;external 'IRBIS64.DLL';
procedure IrbisUnLockDB(PS:PIrbisSpace);external 'IRBIS64.DLL';}

function IrbisIsDBLocked(PS:PIrbisSpace):integer;external 'IRBIS64.DLL';
function IrbisIsLocked(PS:PIrbisSpace; Shelf:integer):integer;external 'IRBIS64.DLL';
{запись заблокирована? - без чтения!!!!!!!! только проверка флага в XRF}
function IrbisIsRealyLocked(PS:PIrbisSpace; mfn:integer):integer;external 'IRBIS64.DLL';
function IrbisIsRealyActualized(PS:PIrbisSpace; mfn:integer):integer;external 'IRBIS64.DLL';

function IrbisIsDeleted(PS:PIrbisSpace; Shelf:integer):integer;external 'IRBIS64.DLL';
function IrbisIsActualized(PS:PIrbisSpace; Shelf:integer):integer;external 'IRBIS64.DLL';

procedure Irbisrecdel(PS: PIrbisSpace; Shelf: longint);external 'IRBIS64.DLL';
procedure Irbisrecundelete(PS: PIrbisSpace; Shelf: longint);external 'IRBIS64.DLL';

function Irbisfieldn(PS: PIrbisSpace; Shelf,met,occ: integer): integer;external 'IRBIS64.DLL';{аналог FIELDN}
{function Irbisfield(PS: PIrbisSpace; Shelf,nf: integer; subfields: string): Pchar;  {аналог FIELD}
function Irbisfield(PS: PIrbisSpace; Shelf,nf: integer; subfields: PChar): Pchar;external 'IRBIS64.DLL';  {аналог FIELD}
function Irbisfldadd(PS: PIrbisSpace; Shelf,met, nf: integer; pole: Pchar): integer;external 'IRBIS64.DLL';{аналог FLDADD}
function Irbisfldrep(PS: PIrbisSpace; Shelf,nf: integer; pole: Pchar): integer;external 'IRBIS64.DLL';{если pole=nil ('') - удаляет!! аналог FLDREP}
function Irbisnfields(PS: PIrbisSpace;Shelf:integer): integer;external 'IRBIS64.DLL';  {аналог NFIELDS}
function Irbisnocc(PS: PIrbisSpace; Shelf,met: integer): integer;external 'IRBIS64.DLL'; {аналог NOCC}
function Irbisfldtag(PS: PIrbisSpace; Shelf,nf: integer): integer; external 'IRBIS64.DLL';{аналог FLDTAG}
function Irbisfldempty(PS:PIrbisSpace;Shelf:integer):integer;external 'IRBIS64.DLL';{опустошает запись}
function Irbisnewrec(PS: PIrbisSpace; shelf: integer):integer;external 'IRBIS64.DLL';
function Irbischangemfn(PS: PIrbisSpace; shelf,newmfn: integer):integer;external 'IRBIS64.DLL';


function Irbismfn(PS: PIrbisSpace;shelf:integer): longint;external 'IRBIS64.DLL';
function Irbismaxmfn(PS: PIrbisSpace): longint;external 'IRBIS64.DLL';

function IrbisDBEmpty(PS: PIrbisSpace): longint;external 'IRBIS64.DLL';
{trm}
function Irbisfind(PS: PIrbisSpace; term: Pchar): integer;external 'IRBIS64.DLL';
function Irbisnxtterm(PS: PIrbisSpace; term: Pchar): integer;external 'IRBIS64.DLL';
function Irbisnposts(PS: PIrbisSpace): longint;external 'IRBIS64.DLL';
function Irbisposting(PS: PIrbisSpace; opt: smallint): longint;external 'IRBIS64.DLL';
function IrbisFindPosting(PS:PIrbisSpace;const Term:PChar;const posting:TifpItemPosting):integer;external 'IRBIS64.DLL';
function Irbisnxtpost(PS: PIrbisSpace): integer;external 'IRBIS64.DLL';
function Irbisprevterm(PS: PIrbisSpace; term: Pchar): integer;external 'IRBIS64.DLL';
function Irbisinitpost(PS: PIrbisSpace):integer;external 'IRBIS64.DLL';
function InsertTerm(PS:PIrbisSpace;const Term:PChar;const posting:TifpItemPosting):integer;external 'IRBIS64.DLL';

procedure IrbisDLLVersion(Abuf: Pchar; Abufsize: integer);external 'IRBIS64.DLL'; //ALIO


function Irbis_InitPFT(PS: PIrbisSpace; Line: PChar): integer; external 'IRBIS64.DLL';  { analog GETFMT }
//function Irbis_InitPFT_Utf8(PS: PIrbisSpace; Line: PChar): integer; external 'IRBIS64.DLL';  { analog GETFMT }
function irbis_uatab_init(uctab,lctab,actab,aExecDir,aDataPath:PChar):integer;external 'IRBIS64.DLL';
function irbis_init_DepositPath(const ADepositPath:PChar):integer;external 'IRBIS64.DLL';
//procedure irbis_MainIni_Init(AIniFile:TIniFile);external 'IRBIS64.DLL';
procedure irbis_MainIni_Init(AIniFile:PChar);external 'IRBIS64.DLL';
{
function irbis_GetGlobalVarList():PChar;external 'IRBIS64.DLL';
procedure irbis_SetGlobalVarList(GlobalVarList_Text:PChar);external 'IRBIS64.DLL';
}
procedure irbis_set_options(ACashable,AUnifor6_Is_Precompiled,AERROR_FIRST_BREAK:boolean);external 'IRBIS64.DLL';
function Irbis_Format(PS: PIrbisSpace; shelf,alt_shelf,trm_shelf,LwLn: Integer;FmtExitDLL : PChar): integer; external 'IRBIS64.DLL';{ analog FORMAT }

Function UNIFOR(H  : PIrbisSpace; { Handle       }
                Curr_Shelf : LongInt; { Shelf of Current record }
                Trm_Shelf  : LongInt; { Shelf of Term           }
                LW_Exit: integer;
                OCC_Exit: integer;
                SP1,SP2: Pchar): integer; external 'IRBIS64.DLL';
Function UMARCI(H  : PIrbisSpace; { Handle       }
                Curr_Shelf : LongInt; { Shelf of Current record }
                Trm_Shelf  : LongInt; { Shelf of Term           }
                LW_Exit: integer;
                OCC_Exit: integer;
                SP1,SP2: Pchar
                ): integer;  external 'IRBIS64.DLL';

{function IrbisLnk_Creat(PS:PIrbisSpace;Path_ln:PChar):integer; external 'IRBIS64.DLL';
function IrbisLnk_Load(PS:PIrbisSpace;Path_lk:PChar):integer; external 'IRBIS64.DLL';}

{function IrbisFormat(PS: PIrbisSpace; shelf:integer; LwLn: Integer): integer; external 'IRBIS64.DLL';{ analog FORMAT }
{
function IrbisLnkCreate(PS:PIrbisSpace;Path_ln:PChar):integer; external 'IRBIS64.DLL';
function IrbisLnkLoad(PS:PIrbisSpace;Path_lk:PChar):integer; external 'IRBIS64.DLL';
procedure IrbisLnkSort(line:PChar); external 'IRBIS64.DLL';
procedure IrbisRecTerms(PS:PIrbisSpace;AShelf:integer;line:Pchar;MfnList:TIntList);external 'IRBIS64.DLL';
function IrbisCreateBKP(PS:PIrbisSpace;line:PChar):integer;external 'IRBIS64.DLL';
function IrbisRestoreBKP(PS:PIrbisSpace;line:PChar):integer;external 'IRBIS64.DLL';
}


function IrbisDBEmptyTime(IrbisSpace:PIrbisSpace;seconds:integer):integer;external 'IRBIS64.DLL';
function IrbisLockDBTime(IrbisSpace:PIrbisSpace;seconds:integer):integer;external 'IRBIS64.DLL';
function IrbisUnLockDBTime(IrbisSpace:PIrbisSpace;seconds:integer):integer;external 'IRBIS64.DLL';
function IrbisRecUpdateTime(IrbisSpace:PIrbisSpace;Shelf:integer;KeepLock:integer;Updif:boolean;seconds:integer;var result_update:integer;var result_updif:integer):integer;external 'IRBIS64.DLL';
function IrbisRecIfUpdateTime(IrbisSpace:PIrbisSpace;Shelf, mfn:integer;seconds:integer):integer;external 'IRBIS64.DLL';
function IrbisRecLockTime(IrbisSpace: PIrbisSpace; Shelf,mfn: longint;seconds:integer): integer;{аналог RECORD}external 'IRBIS64.DLL';
function IrbisRecUnLockTime(IrbisSpace:PIrbisSpace;mfn:integer;seconds:integer):integer;external 'IRBIS64.DLL';
//15.07.2015
function IrbisRecIfUpdateFullTextTime(IrbisSpace:PIrbisSpace;seconds:integer;TermsFileName:PChar;Delete:integer):integer;external 'IRBIS64.DLL';
//IRBIS+
//ставит или снимает бит неактуализированности записи по полным текстам
function IrbisSetFullTextActualizedBitTime(IrbisSpace:PIrbisSpace;mfn,FullTextAcualized,seconds:integer):integer;external 'IRBIS64.DLL';


{
function IrbisSearch_Range(CurSpace:PIrbisSpace; MTerms: TStringList; MLogic: byte; MPortion: integer; MResultList: TIntList): integer;external 'IRBIS64.DLL';
function IrbisFreeIrbisSearch(MIf: PChar; MSex: PChar; MResultList: TIntList): integer;external 'IRBIS64.DLL';
 }


//function IrbisRecPhysDelTime(IrbisSpace:PIrbisSpace;mfn:integer;seconds:integer):integer;external 'IRBIS64.DLL';
{
function  ThreadFormatGroup(ResultList:TStringList;
                       DBname:PChar;
                       PFT_Line_: PChar;
                       AltShelf_:integer;
                       MFNList:TIntList;
                       AddFields_:TStringList;
                       uctab,lctab,actab,aExecDir,aDataPath:PChar;
                       const ADepositPath:PChar;
                       ACashable,AUnifor6_Is_Precompiled,AERROR_FIRST_BREAK:boolean;
                       AIniFile:TIniFile
                       ):integer;external 'IRBIS64.DLL';

function  ThreadFormat(CurSpace:PIrbisSpace;
                       DBname:PChar;
                       PFT_Line_: PChar;
                       AltShelf_:integer;
                       MFN:integer;
                       AddFields_:TStringList;
                       uctab,lctab,actab,aExecDir,aDataPath:PChar;
                       const ADepositPath:PChar;
                       ACashable,AUnifor6_Is_Precompiled,AERROR_FIRST_BREAK:boolean;
                       AIniFile:TIniFile
                       ):integer;external 'IRBIS64.DLL';
 }

end.
