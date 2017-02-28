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

//
// http://wiki.elnit.org/index.php/IRBIS64.dll
//

function IrbisInit: integer; external 'IRBIS64.dll';

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

function IrbisInitMst
  (
    space: integer;
    database: Pchar;
    aNumberShelfs: integer
  ): integer; external 'IRBIS64.dll';

function IrbisInitTerm
(
    space: integer;
    line: Pchar
  ): integer;external 'IRBIS64.dll';

function IrbisMaxMfn
  (
    space: integer
  ): integer; external 'IRBIS64.dll';

function IrbisClose
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



begin
end.
