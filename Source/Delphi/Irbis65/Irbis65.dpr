library Irbis65;

{$R *.res}
{$WARN UNSAFE_CODE OFF}
{$WARN UNSAFE_TYPE OFF}

//
// http://wiki.elnit.org/index.php/IRBIS64.dll
//

// ===========================================================================

// функция инициализации Space вызывается первой!!!!!!
function IrbisInit: integer; external 'IRBIS64.dll' name 'Irbisinit';

// скрыто выполняет все закрытия файлов и освобождение памяти
// irbisclosemst, irbiscloseterm
function IrbisClose
  (
    space: integer
  ): integer; external 'IRBIS64.dll' name 'Irbisclose';

procedure IrbisDLLVersion
  (
    buffer: Pchar;
    bufsize: integer
  ); external 'IRBIS64.dll';

// создание 5 файлов новой БД
function IrbisInitNewDB
  (
    path: PChar
  ):integer; external 'IRBIS64.dll' name 'IrbisinitNewDB';

function irbis_uatab_init
  (
    uctab,
    lctab,
    actab,
    aExecDir,
    aDataPath: PChar
  ): integer; external 'IRBIS64.dll' name 'irbis_uatab_init';

function IrbisInitDeposit
  (
    path: PChar
  ): integer; external 'IRBIS64.dll' name 'irbis_init_DepositPath';

function IrbisNewRec
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll' name 'Irbisnewrec';

function IrbisFldAdd
  (
    space,
    shelf,
    met,
    nf: integer;
    pole: Pchar
  ): integer; external 'IRBIS64.dll' name 'Irbisfldadd';

function IrbisInitPft
  (
    space: integer;
    line: PChar
  ): integer; external 'IRBIS64.dll' name 'Irbis_InitPFT';

function Irbis_Format
  (
    space,
    shelf,
    alt_shelf,
    trm_shelf,
    LwLn: integer;
    FmtExitDLL : PChar
  ): integer; external 'IRBIS64.dll' name 'Irbis_Format';

// открывает мастер файл на чтение-запись
// database - полный путь на мастер файл БЕЗ РАСШИРЕНИЯ!!!
function IrbisInitMst
  (
    space: integer;
    database: Pchar;
    numberShelfs: integer
  ): integer; external 'IRBIS64.dll' name 'Irbisinitmst';

// открывает инверсный файл на чтение-запись
// database - полный путь на инверсный файл БЕЗ РАСШИРЕНИЯ!!!
function IrbisInitTerm
  (
    space: integer;
    database: Pchar
  ): integer;external 'IRBIS64.dll' name 'Irbisinitterm';

function IrbisMaxMfn
  (
    space: integer
  ): integer; external 'IRBIS64.dll' name 'Irbismaxmfn';

function IrbisCloseMst
  (
    space: integer
  ): integer; external 'IRBIS64.dll' name 'Irbisclosemst';

function IrbisRecord
  (
    space,
    shelf,
    mfn: integer
  ): integer; external 'IRBIS64.dll' name 'IrbisRecord';

function IrbisMfn
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll' name 'Irbismfn';

function IrbisNFields
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll' name 'Irbisnfields';

// чтение копии с шагом назад step без блокировки
function IrbisReadVersion
  (
    space,
    mfn: integer
  ):integer; external 'IRBIS64.dll' name 'IrbisReadVersion';

// откат до старой копии step шагов
function IrbisRecordBack
  (
    space,
    shelf,
    mfn,
    step:integer
  ): integer; external 'IRBIS64.dll' name 'IrbisRecordBack';

function IrbisRecLock0
  (
    space,
    shelf,
    mfn: integer
  ): integer; external 'IRBIS64.dll' name 'IrbisRecLock0';

function IrbisRecUnLock0
  (
    space,
    mfn:integer
  ): integer; external 'IRBIS64.dll' name 'IrbisRecUnLock0';

function IrbisRecUpdate0
  (
    space,
    shelf,
    keepLock:integer
  ): integer; external 'IRBIS64.dll' name 'IrbisRecUpdate0';

function IrbisRecIfUpdate0
  (
    space,
    shelf,
    mfn:integer
  ): integer; external 'IRBIS64.dll' name 'IrbisRecIfUpdate0';

function IrbisIsDBLocked
  (
    space: integer
  ): integer; external 'IRBIS64.dll' name 'IrbisIsDBLocked';

// запись заблокирована? - без чтения!!!!!!!! только проверка флага в XRF
function IrbisIsRealyLocked
  (
    space,
    mfn: integer
  ): integer; external 'IRBIS64.dll' name 'IrbisIsRealyLocked';

function IrbisIsLocked
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll' name 'IrbisIsLocked';

function IrbisIsDeleted
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll' name 'IrbisIsDeleted';

function IrbisIsActualized
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll' name 'IrbisIsActualized';

procedure IrbisSetOptions
  (
    cashable,
    precompiled,
    errorFirstBreak :boolean
  ); external 'IRBIS64.dll' name 'irbis_set_options';

procedure IrbisMainIniInit
  (
    iniFile:PChar
  ); external 'IRBIS64.dll' name 'irbis_MainIni_Init';

procedure IrbisInitUactab
  (
    space: integer
  ); external 'IRBIS64.dll' name 'IrbisInitUACTAB';

function IrbisFind
  (
    space: integer;
    term: Pchar
  ): integer; external 'IRBIS64.dll' name 'Irbisfind';

function IrbisNextTerm
  (
    space: integer;
    term: Pchar
  ): integer; external 'IRBIS64.dll' name 'Irbisnxtterm';

function IrbisPrevTerm
  (
    space: integer;
    term: Pchar
  ): integer; external 'IRBIS64.dll' name 'Irbisprevterm';

function IrbisNPosts
  (
    space: integer
  ): longint; external 'IRBIS64.dll' name 'Irbisnposts';

function IrbisPosting
  (
    space: integer;
    opt: smallint
  ): longint; external 'IRBIS64.dll' name 'Irbisposting';

function IrbisNextPost
  (
    space: integer
  ): integer; external 'IRBIS64.dll' name 'Irbisnxtpost';

// ===========================================================================

// функция инициализации Space вызывается первой!!!!!!
function IrbisInit65: integer; stdcall;
begin
  Result := IrbisInit;
end;

// скрыто выполняет все закрытия файлов и освобождение памяти
// irbisclosemst, irbiscloseterm
function IrbisClose65
  (
    space: integer
  ): integer; stdcall;
begin
  Result := IrbisClose(space);
end;

procedure IrbisDllVersion65
  (
    buffer: Pchar;
    bufsize: integer
  ); stdcall;
begin
  IrbisDLLVersion(buffer, bufsize);
end;

// создание 5 файлов новой БД
function IrbisInitNewDB65
  (
    path: PChar
  ):integer; stdcall;
begin
  Result := IrbisInitNewDB(path);
end;

function irbis_uatab_init65
  (
    uctab,
    lctab,
    actab,
    aExecDir,
    aDataPath: PChar
  ): integer; stdcall;
begin
  Result := irbis_uatab_init(uctab, lctab, actab, aExecDir, aDataPath);
end;

function IrbisInitDeposit65
  (
    path: PChar
  ): integer; stdcall;
begin
  Result := IrbisInitDeposit(path);
end;

function IrbisNewRec65
  (
    space,
    shelf: integer
  ): integer; stdcall;
begin
  Result := IrbisNewRec(space, shelf);
end;

function IrbisFldAdd65
  (
    space,
    shelf,
    met,
    nf: integer;
    pole: Pchar
  ): integer; stdcall;
begin
  Result := IrbisFldAdd(space, shelf, met, nf, pole);
end;

function IrbisInitPft65
  (
    space: integer;
    line: PChar
  ): integer; stdcall;
begin
  Result := IrbisInitPft(space, line);
end;

function Irbis_Format65
  (
    space,
    shelf,
    alt_shelf,
    trm_shelf,
    LwLn: integer;
    FmtExitDLL : PChar
  ): integer; stdcall;
begin
  Result := Irbis_Format(space, shelf, alt_shelf, trm_shelf, LwLn, FmtExitDLL);
end;

// открывает мастер файл на чтение-запись
// database - полный путь на мастер файл БЕЗ РАСШИРЕНИЯ!!!
function IrbisInitMst65
  (
    space: integer;
    database: Pchar;
    numberShelfs: integer
  ): integer; stdcall;
begin
  Result := IrbisInitMst(space, database, numberShelfs);
end;

// открывает инверсный файл на чтение-запись
// database - полный путь на инверсный файл БЕЗ РАСШИРЕНИЯ!!!
function IrbisInitTerm65
  (
    space: integer;
    database: Pchar
  ): integer; stdcall;
begin
  Result := IrbisInitTerm(space, database);
end;

function IrbisMaxMfn65
  (
    space: integer
  ): integer; stdcall;
begin
  Result := IrbisMaxMfn(space);
end;

function IrbisCloseMst65
  (
    space: integer
  ): integer; stdcall;
begin
  Result := IrbisCloseMst(space);
end;

function IrbisRecord65
  (
    space,
    shelf,
    mfn: integer
  ): integer; stdcall;
begin
  Result := IrbisRecord(space, shelf, mfn);
end;

function IrbisMfn65
  (
    space,
    shelf: integer
  ): integer; stdcall;
begin
  Result := IrbisMfn(space, shelf);
end;

function IrbisNFields65
  (
    space,
    shelf: integer
  ): integer;  stdcall;
begin
  Result := IrbisNFields(space, shelf);
end;

function IrbisReadVersion65
  (
    space,
    mfn: integer
  ):integer; stdcall;
begin
  Result := IrbisReadVersion(space, mfn);
end;

// откат до старой копии step шагов
function IrbisRecordBack65
  (
    space,
    shelf,
    mfn,
    step:integer
  ): integer; stdcall;
begin
  Result := IrbisRecordBack(space, shelf, mfn, step);
end;

function IrbisRecLock065
  (
    space,
    shelf,
    mfn: integer
  ): integer; stdcall;
begin
  Result := IrbisRecLock0(space, shelf, mfn);
end;

function IrbisRecUnLock065
  (
    space,
    mfn:integer
  ): integer; stdcall;
begin
  Result := IrbisRecUnLock0(space, mfn);
end;

function IrbisRecUpdate065
  (
    space,
    shelf,
    keepLock:integer
  ): integer; stdcall;
begin
  Result := IrbisRecUpdate0(space, shelf, keepLock);
end;

function IrbisRecIfUpdate065
  (
    space,
    shelf,
    mfn:integer
  ): integer; stdcall;
begin
  Result := IrbisRecIfUpdate0(space, shelf, mfn);
end;

function IrbisIsDBLocked65
  (
    space: integer
  ): integer; stdcall;
begin
  Result := IrbisIsDBLocked(space);
end;

// запись заблокирована? - без чтения!!!!!!!! только проверка флага в XRF
function IrbisIsRealyLocked65
  (
    space,
    mfn: integer
  ): integer; stdcall;
begin
  Result := IrbisIsRealyLocked(space, mfn);
end;

function IrbisIsLocked65
  (
    space,
    shelf: integer
  ): integer; stdcall;
begin
  Result := IrbisIsLocked(space, shelf);
end;

function IrbisIsDeleted65
  (
    space,
    shelf: integer
  ): integer; stdcall;
begin
  Result := IrbisIsDeleted(space, shelf);
end;

function IrbisIsActualized65
  (
    space,
    shelf: integer
  ): integer; stdcall;
begin
  Result := IrbisIsActualized(space, shelf);
end;

procedure IrbisSetOptions65
  (
    cashable,
    precompiled,
    errorFirstBreak :boolean
  ); stdcall;
begin
  IrbisSetOptions(cashable, precompiled, errorFirstBreak);
end;

procedure IrbisMainIniInit65
  (
    iniFile:PChar
  ); stdcall;
begin
  IrbisMainIniInit(iniFile);
end;

procedure IrbisInitUactab65
  (
    space: integer
  ); stdcall;
begin
  IrbisInitUactab(space);
end;

function IrbisFind65
  (
    space: integer;
    term: Pchar
  ): integer; stdcall;
begin
  Result := IrbisFind(space, term);
end;

function IrbisNextTerm65
  (
    space: integer;
    term: Pchar
  ): integer; stdcall;
begin
  Result := IrbisNextTerm(space, term);
end;

function IrbisPrevTerm65
  (
    space: integer;
    term: Pchar
  ): integer; stdcall;
begin
  Result := IrbisPrevTerm(space, term);
end;

function IrbisNPosts65
  (
    space: integer
  ): longint; stdcall;
begin
  Result := IrbisNPosts(space);
end;

function IrbisPosting65
  (
    space: integer;
    opt: smallint
  ): longint; stdcall;
begin
  Result := IrbisPosting(space, opt);
end;

function IrbisNextPost65
  (
    space: integer
  ): integer; stdcall;
begin
  Result := IrbisNextPost(space);
end;

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
InteropVersion name 'InteropVersion';

// ===========================================================================

begin
end.
