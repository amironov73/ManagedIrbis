//
// ArsMagna project http://arsmagna.ru
//
// Shim for converting IRBIS64.DLL fastcall exports into stdcall.
//

//
// http://wiki.elnit.org/index.php/IRBIS64.dll
//

library Irbis65;

{$R *.res}
{$WARN UNSAFE_CODE OFF}
{$WARN UNSAFE_TYPE OFF}

// ===========================================================================

// функция инициализации Space вызывается первой!!!!!!
function IrbisInit: integer; external 'IRBIS64.dll' name 'Irbisinit';

function IrbisInit65: integer; stdcall; export;
begin
  Result := IrbisInit;
end;

// ===========================================================================

// скрыто выполняет все закрытия файлов и освобождение памяти
// irbisclosemst, irbiscloseterm
function IrbisClose
  (
    space: integer
  ): integer; external 'IRBIS64.dll' name 'Irbisclose';

function IrbisClose65
  (
    space: integer
  ): integer; stdcall; export;
begin
  Result := IrbisClose(space);
end;

// ===========================================================================

procedure IrbisDLLVersion
  (
    buffer: Pchar;
    bufsize: integer
  ); external 'IRBIS64.dll';

procedure IrbisDllVersion65
  (
    buffer: Pchar;
    bufsize: integer
  ); stdcall; export;
begin
  IrbisDLLVersion(buffer, bufsize);
end;

// ===========================================================================

// создание 5 файлов новой БД
function IrbisInitNewDB
  (
    path: PChar
  ):integer; external 'IRBIS64.dll' name 'IrbisinitNewDB';

function IrbisInitNewDB65
  (
    path: PChar
  ):integer; stdcall; export;
begin
  Result := IrbisInitNewDB(path);
end;

// ===========================================================================

function irbis_uatab_init
  (
    uctab,
    lctab,
    actab,
    aExecDir,
    aDataPath: PChar
  ): integer; external 'IRBIS64.dll' name 'irbis_uatab_init';

function irbis_uatab_init65
  (
    uctab,
    lctab,
    actab,
    aExecDir,
    aDataPath: PChar
  ): integer; stdcall; export;
begin
  Result := irbis_uatab_init(uctab, lctab, actab, aExecDir, aDataPath);
end;

// ===========================================================================

function IrbisInitDeposit
  (
    path: PChar
  ): integer; external 'IRBIS64.dll' name 'irbis_init_DepositPath';

function IrbisInitDeposit65
  (
    path: PChar
  ): integer; stdcall; export;
begin
  Result := IrbisInitDeposit(path);
end;

// ===========================================================================

function IrbisNewRec
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll' name 'Irbisnewrec';

function IrbisNewRec65
  (
    space,
    shelf: integer
  ): integer; stdcall; export;
begin
  Result := IrbisNewRec(space, shelf);
end;

// ===========================================================================

function IrbisFldAdd
  (
    space,
    shelf,
    met,
    nf: integer;
    pole: Pchar
  ): integer; external 'IRBIS64.dll' name 'Irbisfldadd';

function IrbisFldAdd65
  (
    space,
    shelf,
    met,
    nf: integer;
    pole: Pchar
  ): integer; stdcall; export;
begin
  Result := IrbisFldAdd(space, shelf, met, nf, pole);
end;

// ===========================================================================

function IrbisInitPft
  (
    space: integer;
    line: PChar
  ): integer; external 'IRBIS64.dll' name 'Irbis_InitPFT';

function IrbisInitPft65
  (
    space: integer;
    line: PChar
  ): integer; stdcall; export;
begin
  Result := IrbisInitPft(space, line);
end;

// ===========================================================================

function Irbis_Format
  (
    space,
    shelf,
    alt_shelf,
    trm_shelf,
    LwLn: integer;
    FmtExitDLL : PChar
  ): integer; external 'IRBIS64.dll' name 'Irbis_Format';

function Irbis_Format65
  (
    space,
    shelf,
    alt_shelf,
    trm_shelf,
    LwLn: integer;
    FmtExitDLL : PChar
  ): integer; stdcall; export;
begin
  Result := Irbis_Format(space, shelf, alt_shelf, trm_shelf, LwLn, FmtExitDLL);
end;

// ===========================================================================

// открывает мастер файл на чтение-запись
// database - полный путь на мастер файл БЕЗ РАСШИРЕНИЯ!!!
function IrbisInitMst
  (
    space: integer;
    database: Pchar;
    numberShelfs: integer
  ): integer; external 'IRBIS64.dll' name 'Irbisinitmst';

function IrbisInitMst65
  (
    space: integer;
    database: Pchar;
    numberShelfs: integer
  ): integer; stdcall; export;
begin
  Result := IrbisInitMst(space, database, numberShelfs);
end;

// ===========================================================================

// открывает инверсный файл на чтение-запись
// database - полный путь на инверсный файл БЕЗ РАСШИРЕНИЯ!!!
function IrbisInitTerm
  (
    space: integer;
    database: Pchar
  ): integer;external 'IRBIS64.dll' name 'Irbisinitterm';

function IrbisInitTerm65
  (
    space: integer;
    database: Pchar
  ): integer; stdcall; export;
begin
  Result := IrbisInitTerm(space, database);
end;

// ===========================================================================

function IrbisMaxMfn
  (
    space: integer
  ): integer; external 'IRBIS64.dll' name 'Irbismaxmfn';

function IrbisMaxMfn65
  (
    space: integer
  ): integer; stdcall; export;
begin
  Result := IrbisMaxMfn(space);
end;

// ===========================================================================

function IrbisCloseMst
  (
    space: integer
  ): integer; external 'IRBIS64.dll' name 'Irbisclosemst';

function IrbisCloseMst65
  (
    space: integer
  ): integer; stdcall; export;
begin
  Result := IrbisCloseMst(space);
end;

// ===========================================================================

function IrbisRecord
  (
    space,
    shelf,
    mfn: integer
  ): integer; external 'IRBIS64.dll' name 'IrbisRecord';

function IrbisRecord65
  (
    space,
    shelf,
    mfn: integer
  ): integer; stdcall; export;
begin
  Result := IrbisRecord(space, shelf, mfn);
end;

// ===========================================================================

function IrbisMfn
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll' name 'Irbismfn';

function IrbisMfn65
  (
    space,
    shelf: integer
  ): integer; stdcall; export;
begin
  Result := IrbisMfn(space, shelf);
end;

// ===========================================================================

function IrbisNFields
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll' name 'Irbisnfields';

function IrbisNFields65
  (
    space,
    shelf: integer
  ): integer;  stdcall; export;
begin
  Result := IrbisNFields(space, shelf);
end;

// ===========================================================================

// чтение копии с шагом назад step без блокировки
function IrbisReadVersion
  (
    space,
    mfn: integer
  ):integer; external 'IRBIS64.dll' name 'IrbisReadVersion';

function IrbisReadVersion65
  (
    space,
    mfn: integer
  ):integer; stdcall; export;
begin
  Result := IrbisReadVersion(space, mfn);
end;

// ===========================================================================

// откат до старой копии step шагов
function IrbisRecordBack
  (
    space,
    shelf,
    mfn,
    step:integer
  ): integer; external 'IRBIS64.dll' name 'IrbisRecordBack';

function IrbisRecordBack65
  (
    space,
    shelf,
    mfn,
    step:integer
  ): integer; stdcall; export;
begin
  Result := IrbisRecordBack(space, shelf, mfn, step);
end;
// ===========================================================================

function IrbisRecLock0
  (
    space,
    shelf,
    mfn: integer
  ): integer; external 'IRBIS64.dll' name 'IrbisRecLock0';

function IrbisRecLock065
  (
    space,
    shelf,
    mfn: integer
  ): integer; stdcall; export;
begin
  Result := IrbisRecLock0(space, shelf, mfn);
end;

// ===========================================================================

function IrbisRecUnLock0
  (
    space,
    mfn:integer
  ): integer; external 'IRBIS64.dll' name 'IrbisRecUnLock0';

function IrbisRecUnLock065
  (
    space,
    mfn:integer
  ): integer; stdcall; export;
begin
  Result := IrbisRecUnLock0(space, mfn);
end;

// ===========================================================================

function IrbisRecUpdate0
  (
    space,
    shelf,
    keepLock:integer
  ): integer; external 'IRBIS64.dll' name 'IrbisRecUpdate0';

function IrbisRecUpdate065
  (
    space,
    shelf,
    keepLock:integer
  ): integer; stdcall; export;
begin
  Result := IrbisRecUpdate0(space, shelf, keepLock);
end;

// ===========================================================================

function IrbisRecIfUpdate0
  (
    space,
    shelf,
    mfn:integer
  ): integer; external 'IRBIS64.dll' name 'IrbisRecIfUpdate0';

function IrbisRecIfUpdate065
  (
    space,
    shelf,
    mfn:integer
  ): integer; stdcall; export;
begin
  Result := IrbisRecIfUpdate0(space, shelf, mfn);
end;

// ===========================================================================

function IrbisIsDBLocked
  (
    space: integer
  ): integer; external 'IRBIS64.dll' name 'IrbisIsDBLocked';

function IrbisIsDBLocked65
  (
    space: integer
  ): integer; stdcall; export;
begin
  Result := IrbisIsDBLocked(space);
end;

// ===========================================================================

// запись заблокирована? - без чтения!!!!!!!! только проверка флага в XRF
function IrbisIsRealyLocked
  (
    space,
    mfn: integer
  ): integer; external 'IRBIS64.dll' name 'IrbisIsRealyLocked';

function IrbisIsRealyLocked65
  (
    space,
    mfn: integer
  ): integer; stdcall; export;
begin
  Result := IrbisIsRealyLocked(space, mfn);
end;

// ===========================================================================

function IrbisIsLocked
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll' name 'IrbisIsLocked';

function IrbisIsLocked65
  (
    space,
    shelf: integer
  ): integer; stdcall; export;
begin
  Result := IrbisIsLocked(space, shelf);
end;

// ===========================================================================

function IrbisIsDeleted
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll' name 'IrbisIsDeleted';

function IrbisIsDeleted65
  (
    space,
    shelf: integer
  ): integer; stdcall; export;
begin
  Result := IrbisIsDeleted(space, shelf);
end;

// ===========================================================================

function IrbisIsActualized
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll' name 'IrbisIsActualized';

function IrbisIsActualized65
  (
    space,
    shelf: integer
  ): integer; stdcall; export;
begin
  Result := IrbisIsActualized(space, shelf);
end;

// ===========================================================================

procedure IrbisSetOptions
  (
    cashable,
    precompiled,
    errorFirstBreak :boolean
  ); external 'IRBIS64.dll' name 'irbis_set_options';

procedure IrbisSetOptions65
  (
    cashable,
    precompiled,
    errorFirstBreak :boolean
  ); stdcall; export;
begin
  IrbisSetOptions(cashable, precompiled, errorFirstBreak);
end;

// ===========================================================================

procedure IrbisMainIniInit
  (
    iniFile:PChar
  ); external 'IRBIS64.dll' name 'irbis_MainIni_Init';

procedure IrbisMainIniInit65
  (
    iniFile:PChar
  ); stdcall; export;
begin
  IrbisMainIniInit(iniFile);
end;

// ===========================================================================

procedure IrbisInitUactab
  (
    space: integer
  ); external 'IRBIS64.dll' name 'IrbisInitUACTAB';

procedure IrbisInitUactab65
  (
    space: integer
  ); stdcall; export;
begin
  IrbisInitUactab(space);
end;

// ===========================================================================

function IrbisFind
  (
    space: integer;
    term: Pchar
  ): integer; external 'IRBIS64.dll' name 'Irbisfind';

function IrbisFind65
  (
    space: integer;
    term: Pchar
  ): integer; stdcall; export;
begin
  Result := IrbisFind(space, term);
end;

// ===========================================================================

function IrbisNextTerm
  (
    space: integer;
    term: Pchar
  ): integer; external 'IRBIS64.dll' name 'Irbisnxtterm';

function IrbisNextTerm65
  (
    space: integer;
    term: Pchar
  ): integer; stdcall; export;
begin
  Result := IrbisNextTerm(space, term);
end;

// ===========================================================================

function IrbisPrevTerm
  (
    space: integer;
    term: Pchar
  ): integer; external 'IRBIS64.dll' name 'Irbisprevterm';

function IrbisPrevTerm65
  (
    space: integer;
    term: Pchar
  ): integer; stdcall; export;
begin
  Result := IrbisPrevTerm(space, term);
end;

// ===========================================================================

function IrbisNPosts
  (
    space: integer
  ): longint; external 'IRBIS64.dll' name 'Irbisnposts';

function IrbisNPosts65
  (
    space: integer
  ): longint; stdcall; export;
begin
  Result := IrbisNPosts(space);
end;

// ===========================================================================

function IrbisPosting
  (
    space: integer;
    opt: smallint
  ): longint; external 'IRBIS64.dll' name 'Irbisposting';

function IrbisPosting65
  (
    space: integer;
    opt: smallint
  ): longint; stdcall; export;
begin
  Result := IrbisPosting(space, opt);
end;

// ===========================================================================

function IrbisNextPost
  (
    space: integer
  ): integer; external 'IRBIS64.dll' name 'Irbisnxtpost';

function IrbisNextPost65
  (
    space: integer
  ): integer; stdcall; export;
begin
  Result := IrbisNextPost(space);
end;

// ===========================================================================

procedure IrbisRecDelete
  (
    space,
    shelf: integer
  ); external 'IRBIS64.dll' name 'Irbisrecdel';

procedure IrbisRecDelete65
  (
    space,
    shelf: integer
  ); stdcall; export;
begin
  IrbisRecDelete(space, shelf);
end;

// ===========================================================================

procedure IrbisRecUndelete
  (
    space,
    shelf: integer
  ); external 'IRBIS64.dll' name 'Irbisrecundelete';

procedure IrbisRecUndelete65
  (
    space,
    shelf: integer
  ); stdcall; export;
begin
  IrbisRecUndelete(space, shelf);
end;

// ===========================================================================

function Unifor
  (
    space,
    currentShelf,
    termShelf,
    lwExit,
    occExit: integer;
    sp1,
    sp2: Pchar
  ): integer; external 'IRBIS64.dll' name 'UNIFOR';

function Unifor65
  (
    space,
    currentShelf,
    termShelf,
    lwExit,
    occExit: integer;
    sp1,
    sp2: Pchar
  ): integer; stdcall; export;
begin
  Result := Unifor(space, currentShelf, termShelf, lwExit, occExit, sp1, sp2);
end;

// ===========================================================================

function Umarci
  (
    space,
    currentShelf,
    termShelf,
    lwExit,
    occExit: integer;
    sp1,
    sp2: Pchar
  ): integer; external 'IRBIS64.dll' name 'UMARCI';

function Umarci65
  (
    space,
    currentShelf,
    termShelf,
    lwExit,
    occExit: integer;
    sp1,
    sp2: Pchar
  ): integer; stdcall; export;
begin
  Result := Umarci(space, currentShelf, termShelf, lwExit, occExit, sp1, sp2);
end;

// ===========================================================================

function IrbisDbEmptyTime
  (
    space,
    seconds: integer
  ): integer; external 'IRBIS64.DLL' name 'IrbisDBEmptyTime';

function IrbisDbEmptyTime65
  (
    space,
    seconds: integer
  ): integer; stdcall; export;
begin
  Result := IrbisDbEmptyTime(space, seconds);
end;

// ===========================================================================

function IrbisLockDbTime
  (
    space,
    seconds: integer
  ): integer; external 'IRBIS64.DLL' name 'IrbisLockDBTime';

function IrbisLockDbTime65
  (
    space,
    seconds: integer
  ): integer; stdcall; export;
begin
  Result := IrbisLockDbTime(space, seconds);
end;

// ===========================================================================

function IrbisUnLockDbTime
  (
    space,
    seconds: integer
  ): integer; external 'IRBIS64.DLL' name 'IrbisUnLockDBTime';

function IrbisUnLockDbTime65
  (
    space,
    seconds: integer
  ): integer; stdcall; export;
begin
  Result := IrbisUnLockDbTime(space, seconds);
end;

// ===========================================================================

function IrbisRecUpdateTime
  (
    space,
    shelf,
    keepLock: integer;
    updif :boolean;
    seconds :integer;
    var resultUpdate :integer;
    var resultUpdif :integer
  ): integer; external 'IRBIS64.DLL' name 'IrbisRecUpdateTime';

function IrbisRecUpdateTime65
  (
    space,
    shelf,
    keepLock: integer;
    updif :boolean;
    seconds :integer;
    var resultUpdate :integer;
    var resultUpdif :integer
  ): integer; stdcall; export;
begin
  Result := IrbisRecUpdateTime(space, shelf, keepLock, updif, seconds,
    resultUpdate, resultUpdif);
end;

// ===========================================================================

function IrbisRecIfUpdateTime
  (
    space,
    shelf,
    mfn,
    seconds: integer
  ): integer; external 'IRBIS64.DLL' name 'IrbisRecIfUpdateTime';

function IrbisRecIfUpdateTime65
  (
    space,
    shelf,
    mfn,
    seconds: integer
  ): integer; stdcall; export;
begin
  Result := IrbisRecIfUpdateTime(space, shelf, mfn, seconds);
end;

// ===========================================================================

function IrbisRecLockTime
  (
    space,
    shelf,
    mfn,
    seconds: integer
  ): integer; external 'IRBIS64.DLL' name 'IrbisRecLockTime';

function IrbisRecLockTime65
  (
    space,
    shelf,
    mfn,
    seconds: integer
  ): integer; stdcall; export;
begin
  Result := IrbisRecLockTime(space, shelf, mfn, seconds);
end;

// ===========================================================================

function IrbisRecUnLockTime
  (
    space,
    mfn,
    seconds:integer
  ): integer; external 'IRBIS64.DLL' name 'IrbisRecUnLockTime';

function IrbisRecUnLockTime65
  (
    space,
    mfn,
    seconds:integer
  ): integer; stdcall; export;
begin
  Result := IrbisRecUnLockTime(space, mfn, seconds);
end;

// ===========================================================================

function InteropVersion: integer; stdcall;
begin
  Result := 100;
end;

// ===========================================================================

exports

IrbisInit65 name 'IrbisInit',
IrbisClose65 name 'IrbisClose',
IrbisDllVersion65 name 'IrbisDllVersion',
IrbisInitNewDB65 name 'IrbisInitNewDb',
irbis_uatab_init65 name 'IrbisUatabInit',
IrbisInitDeposit65 name 'IrbisInitDeposit',
IrbisNewRec65 name 'IrbisNewRec',
IrbisFldAdd65 name 'IrbisFldAdd',
IrbisInitPft65 name 'IrbisInitPft',
Irbis_Format65 name 'IrbisFormat',
IrbisInitMst65 name 'IrbisInitMst',
IrbisInitTerm65 name 'IrbisInitTerm',
IrbisMaxMfn65 name 'IrbisMaxMfn',
IrbisCloseMst65 name 'IrbisCloseMst',
IrbisRecord65 name 'IrbisRecord',
IrbisMfn65 name 'IrbisMfn',
IrbisNFields65 name 'IrbisNFields',
IrbisReadVersion65 name 'IrbisReadVersion',
IrbisRecordBack65 name 'IrbisRecordBack',
IrbisRecLock065 name 'IrbisRecLock0',
IrbisRecUnLock065 name 'IrbisRecUnlock0',
IrbisRecUpdate065 name 'IrbisRecUpdate0',
IrbisRecIfUpdate065 name 'IrbisRecIfUpdate0',
IrbisIsDBLocked65 name 'IrbisIsDbLocked',
IrbisIsRealyLocked65 name 'IrbisIsReallyLocked',
IrbisIsLocked65 name 'IrbisIsLocked',
IrbisIsDeleted65 name 'IrbisIsDeleted',
IrbisIsActualized65 name 'IrbisIsActualized',
IrbisSetOptions65 name 'IrbisSetOptions',
IrbisMainIniInit65 name 'IrbisMainIniInit',
IrbisInitUactab65 name 'IrbisInitUactab',
IrbisFind65 name 'IrbisFind',
IrbisNextTerm65 name 'IrbisNextTerm',
IrbisPrevTerm65 name 'IrbisPrevTerm',
IrbisNPosts65 name 'IrbisNPosts',
IrbisPosting65 name 'IrbisPosting',
IrbisNextPost65 name 'IrbisNextPost',
IrbisRecDelete65 name 'IrbisRecDelete',
IrbisRecUndelete65 name 'IrbisRecUndelete',
Unifor65 name 'Unifor',
Umarci65 name 'Umarci',
IrbisDbEmptyTime65 name 'IrbisDbEmptyTime',
IrbisLockDbTime65 name 'IrbisLockDbTime',
IrbisUnLockDbTime65 name 'IrbisUnLockDbTime',
IrbisRecUpdateTime65 name 'IrbisRecUpdateTime',
IrbisRecIfUpdateTime65 name 'IrbisRecIfUpdateTime',
IrbisRecLockTime65 name 'IrbisRecLockTime',
IrbisRecUnLockTime65 name 'IrbisRecUnLockTime',
InteropVersion name 'InteropVersion';

// ===========================================================================

begin
end.
