program FirstApp;

{$APPTYPE CONSOLE}

{$WARN UNSAFE_CODE OFF}
{$WARN UNSAFE_TYPE OFF}

uses
  SysUtils,
  Classes;

type PIrbisSpace = Pointer;

function Irbisinit: PIrbisSpace; external 'IRBIS64.DLL';
function IrbisinitNewDB(Path:PChar): longint; external 'IRBIS64.DLL';
procedure Irbisclose(SP:PIrbisSpace); external 'IRBIS64.DLL';
function Irbisinitmst(SP:PIrbisSpace;DataBase: Pchar;
  ANumberShelfs: integer): integer; external 'IRBIS64.DLL';
procedure Irbisclosemst(SP:PIrbisSpace);external 'IRBIS64.DLL';
function Irbisinitterm(PS:PIrbisSpace;
  line: Pchar): integer;external 'IRBIS64.DLL';
procedure Irbiscloseterm(PS: PIrbisSpace);external 'IRBIS64.DLL';

function IrbisRecLock0(PS: PIrbisSpace;
  Shelf, mfn: longint): integer; external 'IRBIS64.DLL';
function IrbisRecUnLock0(PS:PIrbisSpace;
  mfn:integer):integer;external 'IRBIS64.DLL';
function IrbisRecUpdate0(PS:PIrbisSpace;
  Shelf, KeepLock:integer): integer; external 'IRBIS64.DLL';
function IrbisRecIfUpdate0(PS:PIrbisSpace;
  AShelf, mfn: integer): integer; external 'IRBIS64.DLL';

function Irbisfldadd(PS: PIrbisSpace; Shelf, met, nf: integer;
  pole: Pchar): integer; external 'IRBIS64.DLL';
function Irbisfldempty(PS:PIrbisSpace;
  Shelf:integer): integer; external 'IRBIS64.DLL';
function Irbisnewrec (PS: PIrbisSpace;
  shelf: integer): integer; external 'IRBIS64.DLL';

function IrbisUnLockDBTime(IrbisSpace: PIrbisSpace;
  seconds: integer): integer; external 'IRBIS64.DLL';

function IrbisRecUpdateTime(IrbisSpace: PIrbisSpace;
  Shelf: integer; KeepLock: integer; Updif:boolean;
  seconds: integer; var result_update: integer;
  var result_updif: integer): integer; external 'IRBIS64.DLL';

function Irbischangemfn(PS: PIrbisSpace;
  shelf,  newmfn: integer): integer; external 'IRBIS64.DLL';

procedure CheckRC(functionName: string; rc: Integer);
begin
  if rc < 0 then
  begin
    Writeln(functionName, ' returns ', rc );
    Abort;
  end;
end;

var space: PIrbisSpace;
  curDir, dbPath, field: string;
  i, rc, updResult, ifResult: integer;

begin

  space := Irbisinit;

  curDir := GetCurrentDir;
  dbPath := curDir + '\TestDb\TestDb';

  rc := IrbisinitNewDB(PAnsiChar(dbPath));
  CheckRC('IrbisInitNewDB', rc);

  rc := Irbisinitmst(space, PAnsiChar(dbPath), 5);
  CheckRC('IrbisInitMst', rc);
  rc := Irbisinitterm(space, PAnsiChar(dbPath));
  CheckRC('IrbisInitTerm', rc);

  for i := 0 to 9 do
  begin
    rc := Irbisnewrec(space, 0);
    CheckRC('IrbisNewRec', rc);
    field := 'New record';
    rc := Irbisfldadd(space,0,100,-1, PAnsiChar(field));
    CheckRC('IrbisFldAdd', rc);
    //rc := IrbisRecUpdate0(space,0,0);
    //CheckRC('IrbisRecUpdate0', rc);
    Writeln(i);
    //rc := Irbischangemfn(space, 0, i+1);
    //CheckRC('IrbisChangeMfn', rc);
    rc := IrbisRecUpdateTime(space, 0, 0, false, 1, updResult, ifResult);
    CheckRC('IrbisRecUpdateTime', rc);
    rc := IrbisUnLockDBTime(space, 1);
    CheckRC('IrbisUnlockDbTime', rc);
  end;

  Irbisclose(space);

end.
