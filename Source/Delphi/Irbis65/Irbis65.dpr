library Irbis65;

{ Important note about DLL memory management: ShareMem must be the
  first unit in your library's USES clause AND your project's (select
  Project-View Source) USES clause if your DLL exports any procedures or
  functions that pass strings as parameters or function results. This
  applies to all strings passed to and from your DLL--even those that
  are nested in records and classes. ShareMem is the interface unit to
  the BORLNDMM.DLL shared memory manager, which must be deployed along
  with your DLL. To avoid using BORLNDMM.DLL, pass string information
  using PChar or ShortString parameters. }

uses
  SysUtils,
  Classes;

{$R *.res}
{$WARN UNSAFE_CODE OFF}
{$WARN UNSAFE_TYPE OFF}

//
// http://wiki.elnit.org/index.php/IRBIS64.dll
//

// функция инициализации Space вызывается первой!!!!!!
function IrbisInit: integer; external 'IRBIS64.dll';

// скрыто выполняет все закрытия файлов и освобождение памяти
// irbisclosemst, irbiscloseterm
function IrbisClose
  (
    space: integer
  ): integer; external 'IRBIS64.dll';

procedure IrbisDLLVersion
  (
    buffer: Pchar;
    bufsize: integer
  ); external 'IRBIS64.dll';

// создание 5 файлов новой БД
function IrbisInitNewDB
  (
    path: PChar
  ):integer; external 'IRBIS64.dll';

function irbis_uatab_init
  (
    uctab,
    lctab,
    actab,
    aExecDir,
    aDataPath: PChar
  ): integer; external 'IRBIS64.dll';

function irbis_init_DepositPath
  (
    path: PChar
  ): integer; external 'IRBIS64.dll';

function IrbisNewRec
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll';

function IrbisFldAdd
  (
    space,
    shelf,
    met,
    nf: integer;
    pole: Pchar
  ): integer; external 'IRBIS64.dll';

function Irbis_InitPFT
  (
    space: integer;
    line: PChar
  ): integer; external 'IRBIS64.dll';

function Irbis_Format
  (
    space,
    shelf,
    alt_shelf,
    trm_shelf,
    LwLn: integer;
    FmtExitDLL : PChar
  ): integer; external 'IRBIS64.dll';

// открывает мастер файл на чтение-запись
// database - полный путь на мастер файл БЕЗ РАСШИРЕНИЯ!!!
function IrbisInitMst
  (
    space: integer;
    database: Pchar;
    numberShelfs: integer
  ): integer; external 'IRBIS64.dll';

// открывает инверсный файл на чтение-запись
// database - полный путь на инверсный файл БЕЗ РАСШИРЕНИЯ!!!
function IrbisInitTerm
  (
    space: integer;
    database: Pchar
  ): integer;external 'IRBIS64.dll';

function IrbisMaxMfn
  (
    space: integer
  ): integer; external 'IRBIS64.dll';

function IrbisCloseMst
  (
    space: integer
  ): integer; external 'IRBIS64.dll';

function IrbisRecord
  (
    space,
    shelf,
    mfn: integer
  ): integer; external 'IRBIS64.dll';

function IrbisMfn
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll';

function IrbisNFields
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll';

// чтение копии с шагом назад step без блокировки
function IrbisReadVersion
  (
    space,
    mfn: integer
  ):integer; external 'IRBIS64.dll';

// откат до старой копии step шагов
function IrbisRecordBack
  (
    space,
    shelf,
    mfn,
    step:integer
  ): integer; external 'IRBIS64.dll';

function IrbisRecLock0
  (
    space,
    shelf,
    mfn: integer
  ): integer; external 'IRBIS64.dll';

function IrbisRecUnLock0
  (
    space,
    mfn:integer
  ): integer; external 'IRBIS64.dll';

function IrbisRecUpdate0
  (
    space,
    shelf,
    keepLock:integer
  ): integer; external 'IRBIS64.dll';

function IrbisRecIfUpdate0
  (
    space,
    shelf,
    mfn:integer
  ): integer; external 'IRBIS64.dll';

function IrbisIsDBLocked
  (
    space: integer
  ): integer; external 'IRBIS64.dll';

// запись заблокирована? - без чтения!!!!!!!! только проверка флага в XRF
function IrbisIsRealyLocked
  (
    space,
    mfn: integer
  ): integer; external 'IRBIS64.dll';

function IrbisIsRealyActualized
  (
    space,
    mfn: integer
  ): integer; external 'IRBIS64.dll';

function IrbisIsLocked
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll';

function IrbisIsDeleted
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll';

function IrbisIsActualized
  (
    space,
    shelf: integer
  ): integer; external 'IRBIS64.dll';



begin
end.
