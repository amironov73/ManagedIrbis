unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  irbis64_client, StdCtrls, Buttons, ExtCtrls, TntStdCtrls,TntSystem,TntSysUtils;

type
  TForm1 = class(TForm)
    Panel1: TPanel;
    TntMemo1: TTntMemo;
    TntMemo2: TTntMemo;
    TntMemo3: TTntMemo;
    Splitter1: TSplitter;
    Splitter2: TSplitter;
    RegBitBtn: TBitBtn;
    BitBtn2: TBitBtn;
    Read1BitBtn: TBitBtn;
    Write1BitBtn: TBitBtn;
    Write1_2BitBtn: TBitBtn;
    Read2BitBtn: TBitBtn;
    BitBtn1: TBitBtn;
    BitBtn3: TBitBtn;
    procedure RegBitBtnClick(Sender: TObject);
    procedure BitBtn2Click(Sender: TObject);
    procedure Read1BitBtnClick(Sender: TObject);
    procedure Write1BitBtnClick(Sender: TObject);
    procedure Read2BitBtnClick(Sender: TObject);
    procedure Write1_2BitBtnClick(Sender: TObject);
    procedure BitBtn1Click(Sender: TObject);
    procedure BitBtn3Click(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

{$R *.DFM}

procedure TForm1.RegBitBtnClick(Sender: TObject);
var rp: Pchar;
    ri: integer;
begin
GetMem(rp,32000);
ri:=IC_reg('127.0.0.1','6666',IRBIS_ADMINISTRATOR,'1','1',rp,32000);
Caption:=IntToStr(ri);
TntMemo1.Lines.Text:=String(rp);
FreeMem(rp);
end;

procedure TForm1.BitBtn2Click(Sender: TObject);
begin
Caption:=IntToStr(IC_unreg('1'));
end;

procedure TForm1.Read1BitBtnClick(Sender: TObject);
var rp: Pchar;
    ri: integer;
begin
GetMem(rp,32000);
ri:=IC_read('IBIS',10,0,rp,32000);
Caption:=IntToStr(ri);
TntMemo1.Lines.Text:=Utf8ToWideString(String(rp));
FreeMem(rp);
end;

procedure TForm1.Write1BitBtnClick(Sender: TObject);
var rp: Pchar;
    ri: integer;
begin
GetMem(rp,32000);
StrCopy(rp,Pchar(WideStringToUtf8(TntMemo1.Lines.Text)));
ri:=IC_update('IBIS',0,1,rp,32000);
Caption:=IntToStr(ri);
TntMemo1.Lines.Text:=Utf8ToWideString(String(rp));
FreeMem(rp);
end;

procedure TForm1.Read2BitBtnClick(Sender: TObject);
var rp: Pchar;
    ri: integer;
begin
GetMem(rp,32000);
ri:=IC_read('IBIS',20,0,rp,32000);
Caption:=IntToStr(ri);
TntMemo2.Lines.Text:=Utf8ToWideString(String(rp));
FreeMem(rp);
end;

procedure TForm1.Write1_2BitBtnClick(Sender: TObject);
var rp: Pchar;
    ri: integer;
    rs: string;
    TL: TStringList;
begin
GetMem(rp,32000);
TL:=TStringList.Create;
rs:=WideStringToUtf8(TntMemo1.Lines.Text);
StrCopy(rp,IC_reset_delim(Pchar(rs)));
StrCat(rp,Pchar(chr(13)+chr(10)));
rs:=WideStringToUtf8(TntMemo2.Lines.Text);
StrCat(rp,IC_reset_delim(Pchar(rs)));

ri:=IC_updategroup('IBIS',0,1,rp,32000);
Caption:=IntToStr(ri);
TL.Text:=String(rp);
TntMemo3.Lines.Text:=Utf8ToWideString(String(rp));
TntMemo1.Text:=Utf8ToWideString(String(IC_delim_reset(Pchar(TL.Strings[0]))));
TntMemo2.Text:=Utf8ToWideString(String(IC_delim_reset(Pchar(TL.Strings[1]))));
FreeMem(rp);
TL.Free;
end;

procedure TForm1.BitBtn1Click(Sender: TObject);
var rp,pole: Pchar;
    ri,nf: integer;
begin
GetMem(rp,32000);
GetMem(pole,32000);
ri:=IC_searchscan('IBIS','',0,1,'',0,0,Pchar(chr(39)+'0067dhfsdhfsd1 sdkfjskdfj'+chr(39)),rp,32000);

Caption:=IntToStr(ri);
TntMemo3.Lines.Text:=String(rp);

FreeMem(rp);
FreeMem(pole);

end;

procedure TForm1.BitBtn3Click(Sender: TObject);
var rp,rp1: Pchar;
    ri,nf: integer;
    TL: TStringList;
begin
GetMem(rp,32000);
GetMem(rp1,32000);
TL:=TStringList.Create;
ri:=IC_adm_getalldeletedlists('IBIS',rp,32000);
Caption:=IntToStr(ri);
//IC_sformat('IBIS',10,'@',rp1,32000);
TntMemo3.Lines.Text:=String(rp);
//TntMemo2.Lines.Text:=Utf8ToWideString(String(rp1));
FreeMem(rp);
FreeMem(rp1);
TL.Free;
end;

end.
