unit IRBIS64_CLIENT;
{файл прототипов функций, констант и кодов возврата для irbis64_client.dll}
interface

const
{коды возвратов}
ERR_USER = -1; {ошибка завершения - прервано пользователем или общая ошибка}
ERR_BUSY = -2; {не завершена обработка предыдущего запроса}
ERR_UNKNOWN = -3; {неизвестная ошибка}
ERR_BUFSIZE = -4; {выходной буфер мал}
ZERO = 0;{0 - нормальное завершение}
ERR_DBEWLOCK = - 300;{монопольная блокировка БД}
ERR_RECLOCKED = -602;{запись заблокирована на ввод}
VERSION_ERROR = -608;//при записи обнаружено несоответстивие версий
READ_WRONG_MFN = -140;{-1 - заданный MFN вне пределов БД}
REC_DELETE = -603;{1 - запись логически удалена}
REC_PHYS_DELETE = -605;{2 - запись физически удалена}
ERROR_CLIENT_FMT=999;
SERVER_EXECUTE_ERROR = -1111;
WRONG_PROTOCOL = -2222;
CLIENT_NOT_IN_LIST = -3333;{незарегистрированный клиент}
CLIENT_NOT_IN_USE = -3334;{незарегистрированный клиент не сделал irbis-reg}
CLIENT_IDENTIFIER_WRONG = -3335;{непрв уникальный идентификатор}
CLIENT_LIST_OVERLOAD = -3336;{зарегистрировано максимально допустимое количество клиентов}
CLIENT_ALREADY_EXISTS = -3337;{клиент уже зарегистрирован}
CLIENT_NOT_ALLOWED = -3338;{нет доступа к командам АРМа}
WRONG_PASSWORD = -4444;{неверный пароль}
FILE_NOT_EXISTS = -5555;
SERVER_OVERLOAD = -6666;{сервер перегружен достигнуто максимальное число потоков обработки}
PROCESS_ERROR = -7777;{не удалось запустить/прервать поток или процесс администратора}
GLOBAL_ERROR = -8888;//gbl обрушилась
ANSWER_LENGTH_ERROR = -1112;//несоответсвие полученной и реальной длины
TERM_NOT_EXISTS = -202;
TERM_LAST_IN_LIST = -203;
TERM_FIRST_IN_LIST = -204;

{коды путей ИРБИС}
SYSPATH = 0;
DATAPATH = 1;
DBNPATH2 = 2;
DBNPATH3 = 3;
DBNPATH10 = 10;
FULLTEXTPATH = 11;
INTERNALRESOURCEPATH = 12;

{коды АРМов}
IRBIS_READER = 'R';
IRBIS_ADMINISTRATOR = 'A';
IRBIS_CATALOG = 'C';
IRBIS_COMPLECT = 'M';
IRBIS_BOOKLAND = 'B';
IRBIS_BOOKPROVD = 'K';

MAX_POSTINGS_IN_PACKET = 32758;{максимальное число ссылок в пакете с сервера}

type TBuffer = packed record
   size:integer;
   capacity:integer;
   data:PChar;
   end;
type PBuffer = ^TBuffer;

{инициализация и регистрация клиента для работы с сервером}
function IC_reg(aserver_host: Pchar;
                           aserver_port: Pchar;
                           arm:char;
                           user_name,password: Pchar;
                           var answer: Pchar; abufsize: integer):integer; stdcall;
{раз-регистрация или автоматически в течении времени CLIENT_TIME_LIVE которое приходит с сервера
после удачной регистрации среди других параметров file.ini}
function IC_unreg(user_name: Pchar): integer; stdcall;


{функции установки основных параметров}
function IC_set_webserver(Aopt: integer): integer; stdcall;
function IC_set_webcgi(Acgi: Pchar): integer; stdcall;
function IC_set_blocksocket(Aopt: integer): integer; stdcall;
function IC_set_show_waiting(Aopt: integer): integer; stdcall;
function IC_set_client_time_live(Aopt: integer): integer; stdcall;
{функция проверки состояния}
function IC_isbusy: integer; stdcall;


{функции работы с ресурсами}
{функция обновления INI-файла клиента на сервере}
function IC_update_ini(inifile: Pchar): integer; stdcall;
{функция чтения текстового ресурса}
function IC_getresourse(Apath: integer; Adbn,Afilename: Pchar; var answer: Pchar; abufsize: integer): integer; stdcall;
{очистка кэша ресурсов}
function IC_clearresourse: integer; stdcall;
{функция группового чтения текстовых ресурсов}
function IC_getresoursegroup(var acontext: Pchar; var answer: Pchar; abufsize: integer): integer; stdcall;
{функция чтения двоичного ресурса}
function IC_getbinaryresourse(Apath: integer; Adbn,Afilename: Pchar; var Abuffer: PBuffer): integer; stdcall;
{функция записи текстового ресурса}
function IC_putresourse(Apath: integer; Adbn,Afilename,Aresourse: Pchar): integer; stdcall;

{функции работы с мастер-файлом базы данных}
{чение записи}
function IC_read(Adbn: Pchar; Amfn,Alock: integer; var answer: Pchar; abufsize: integer): integer; stdcall;
{чение записи с расформатированием}
function IC_readformat(Adbn: Pchar; Amfn,Alock: integer; Aformat: Pchar; var answer: Pchar; abufsize: integer; var answer1: Pchar; abufsize1: integer): integer;  stdcall;
{редактирование записи}
function IC_update(Adbn: Pchar; Alock,Aifupdate: integer; var answer: Pchar; abufsize: integer): integer; stdcall;
{групповое редактирование записи}
function IC_updategroup(Adbn: Pchar; Alock,Aifupdate: integer; var answer: Pchar; abufsize: integer): integer; stdcall;
{связанное редактирование группы записей - возможно из разных БД}
function IC_updategroup_sinhronize(Alock,Aifupdate: integer; Adbnames: Pchar; var answer: Pchar; abufsize: integer):integer; stdcall;
{разблокировать запись}
function IC_runlock(Adbn: Pchar; Amfn: integer): integer; stdcall;
{актуализировать запись}
function IC_ifupdate(Adbn: Pchar; Amfn: integer): integer; stdcall;
{получит максимальный MFN базы данных}
function IC_maxmfn(Adbn: Pchar): integer; stdcall;

{функции для работы с записью}
{определить порядковый номер поля в записи}
function IC_fieldn(Arecord: Pchar; Amet,Aocc: integer): integer; stdcall;
{прочитать заданное поле/подполе}
function IC_field(Arecord: Pchar; nf: integer; delim: char; answer: Pchar; abufsize: integer): integer; stdcall;
{добавить поле в запись}
function IC_fldadd(Arecord: Pchar; Amet,nf: integer; pole: Pchar; abufsize: integer): integer; stdcall;
{заменить поле}
function IC_fldrep(Arecord: Pchar; nf: integer; pole: Pchar; abufsize: integer): integer; stdcall;
{определить кол-во полей в записи}
function IC_nfields(Arecord: Pchar): integer; stdcall;
{определить кол-во повторений поля с заданной меткой}
function IC_nocc(Arecord: Pchar; Amet: integer): integer; stdcall;
{определить метку поля с заданным порядковым номером}
function IC_fldtag(Arecord: Pchar; nf: integer): integer; stdcall;
{опустошить запись}
function IC_fldempty(Arecord: Pchar): integer; stdcall;
{поменять mfn записи}
function IC_changemfn(Arecord: Pchar; newmfn: integer): integer; stdcall;
{установить признак логически удаленной записи}
function IC_recdel(Arecord: Pchar): integer; stdcall;
{снять признак логически удаленной записи}
function IC_recundel(Arecord: Pchar): integer; stdcall;
{снять признак заблокированности}
function IC_recunlock(Arecord: Pchar): integer; stdcall;

{прочитать mfn записи}
function IC_getmfn(Arecord: Pchar): integer; stdcall;
{создать пустую запись}
function IC_recdummy(Arecord: Pchar; abufsize: integer): integer; stdcall;
{прочитать в статусе записи признак АКТУАЛИЗИРОВАННОСТИ}
function IC_isActualized(Arecord: Pchar): integer; stdcall;
{прочитать в статусе записи признак ЗАБЛОКИРОВАННОСТИ}
function IC_isLocked(Arecord: Pchar): integer; stdcall;
{прочитать в статусе записи признак ЛОГИЧЕСКОЙ УДАЛЕННОСТИ}
function IC_isDeleted(Arecord: Pchar): integer; stdcall;

{функции для работы со словарем базы данных}
{прочитать список терминов, начиная с заданного}
function IC_nexttrm(Adbn,Aterm: Pchar; Anumb: integer; answer: Pchar; abufsize: integer): integer; stdcall;
{прочитать список терминов, начиная с заданного, и расформатировать документы по первой ссылке}
function IC_nexttrmgroup(Adbn,Aterm: Pchar; Anumb: integer; Aformat: Pchar; answer: Pchar; abufsize: integer): integer; stdcall;
{прочитать список терминов, начиная с заданного, в обратном порядке}
function IC_prevtrm(Adbn,Aterm: Pchar; Anumb: integer; answer: Pchar; abufsize: integer): integer; stdcall;
{прочитать список терминов, начиная с заданного, в обратном порядке и расформатировать документы по первой ссылке}
function IC_prevtrmgroup(Adbn,Aterm: Pchar; Anumb: integer; Aformat: Pchar; answer: Pchar; abufsize: integer): integer; stdcall;
{прочитать список ссылок для заданного термина}
function IC_posting(Adbn,Aterm: Pchar; Anumb,Afirst: integer; answer: Pchar; abufsize: integer): integer; stdcall;
{прочитать список первых ссылок для заданного списка терминов}
function IC_postinggroup(Adbn,Aterms,answer: Pchar; abufsize: integer): integer; stdcall;
{прочитать список ссылок для заданного термина и расформатировать записи им соответствующие}
function IC_postingformat(Adbn,Aterm: Pchar; Anumb,Afirst: integer; Aformat: Pchar; answer1: Pchar; abufsize1: integer; answer: Pchar; abufsize: integer): integer; stdcall;

{функции поиска}
{прямой поиск записей по заданному поисковому выражению}
function IC_search(Adbn,Asexp: Pchar; Anumb,Afirst: integer; Aformat: Pchar; answer: Pchar; abufsize: integer): integer; stdcall;
{последовательный поиск по результату прямого поиска и/или по заданному диапазону записей}
function IC_searchscan(Adbn,Asexp: Pchar; Anumb,Afirst: integer; Aformat: Pchar; Amin,Amax: integer; Aseq: Pchar; answer: Pchar; abufsize: integer): integer; stdcall;

{функции форматирования}
{форматирование записи по MFN}
function IC_sformat(Adbn: Pchar; Amfn: integer; Aformat: Pchar; answer: Pchar; abufsize: integer): integer; stdcall;
{форматирование записи в клиентском представлении}
function IC_record_sformat(Adbn, Aformat,Arecord: Pchar; answer: Pchar; abufsize: integer): integer; stdcall;
{форматирование группы записей}
function IC_sformatgroup(Adbn,Amfnlist,Aformat: Pchar; answer: Pchar; abufsize: integer): integer; stdcall;

{пакетные функции}
{получить выходную табличную форму по заданному набору записей}
function IC_print(Adbn,Atab,Ahead,Amod,Asexp: Pchar; Amin,Amax: integer; Aseq,Amfnlist: Pchar; answer: Pchar; abufsize: integer): integer; stdcall;
{получить выходную форму статистических распределений заданных параметров по заданному набору записей}
function IC_stat(Adbn,Astat,Asexp: Pchar; Amin,Amax: integer; Aseq,Amfnlist: Pchar; answer: Pchar; abufsize: integer): integer; stdcall;
{выполнить глобальную корректировку заданного набора записей}
function IC_gbl(Adbn: Pchar; Aifupdate: integer; Agbl,Asexp: Pchar; Amin,Amax: integer; Aseq,Amfnlist: Pchar; answer: Pchar; abufsize: integer): integer;  stdcall;

{функции администратора}
{перезапустить сервер}
function IC_adm_restartserver:  integer;  stdcall;
{получить список удаленных документов}
function IC_adm_getDeletedList(Adbn: Pchar; answer: Pchar; abufsize: integer):integer; stdcall;
function IC_adm_getallDeletedLists(Adbn: Pchar; answer: Pchar; abufsize: integer):integer; stdcall;
{опустошение БД}
function IC_adm_dbempty(Adbn: Pchar):integer; stdcall;
{удаление БД}
function IC_adm_dbdelete(Adbn: Pchar):integer; stdcall;
{создание новой БД}
function IC_adm_newdb(Adbn,Adef: Pchar; AReader: integer):integer; stdcall;
{снять монопольную блокировку БД}
function IC_adm_DBunlock(Adbn: Pchar):integer; stdcall;
{снять блокировку записей (MFN)}
function IC_adm_DBunlockMFN(Adbn: Pchar; Amfnlist: Pchar):integer; stdcall;
{создать словарь заново}
function IC_adm_DBStartCreateDictionry(Adbn: Pchar):integer; stdcall;
{реорганизовать файл словаря}
function IC_adm_DBStartReorgDictionry(Adbn: Pchar):integer;  stdcall;
{реорганизовать файл документов}
function IC_adm_DBStartReorgMaster(Adbn: Pchar):integer;  stdcall;
{список зарегистрированных клиентов}
function IC_adm_getClientlist(answer: Pchar; abufsize: integer):integer; stdcall;
{список клиентов для доступа к серверу}
function IC_adm_getClientslist(answer: Pchar; abufsize: integer):integer; stdcall;
{список запущенных процессов}
function IC_adm_getProcessList(answer: Pchar; abufsize: integer):integer; stdcall;
{обновить список клиентов для доступа к серверу}
function IC_adm_SetClientslist(AClientMnu: Pchar):integer; stdcall;



{вспомогательные функции}
{подтвержение регистрации}
function IC_nooperation:integer; stdcall
{получить элемент ссылки}
function IC_getposting(APost: Pchar; AType: integer): integer; stdcall;
{заменить реальные разделители строк $0D0A на псевдоразделители $3130}
function IC_reset_delim(Aline: Pchar): Pchar; stdcall;
{заменить псевдоразделители разделители строк $3130 на реальные $0D0A}
function IC_delim_reset(Aline: Pchar): Pchar; stdcall;

implementation


{инициализация и регистрация клиента для работы с сервером}
function IC_reg(aserver_host: Pchar;
                           aserver_port: Pchar;
                           arm:char;
                           user_name,password: Pchar;
                           var answer: Pchar; abufsize: integer):integer;  external 'irbis64_client.dll';
{раз-регистрация или автоматически в течении времени CLIENT_TIME_LIVE которое приходит с сервера
после удачной регистрации среди других параметров file.ini}
function IC_unreg(user_name: Pchar): integer;  external 'irbis64_client.dll';


{функции установки основных параметров}
function IC_set_webserver(Aopt: integer): integer;   external 'irbis64_client.dll';
function IC_set_webcgi(Acgi: Pchar): integer;   external 'irbis64_client.dll';
function IC_set_blocksocket(Aopt: integer): integer;   external 'irbis64_client.dll';
function IC_set_show_waiting(Aopt: integer): integer;   external 'irbis64_client.dll';
function IC_set_client_time_live(Aopt: integer): integer;   external 'irbis64_client.dll';
{функция проверки состояния}
function IC_isbusy: integer;  external 'irbis64_client.dll';


{функции работы с ресурсами}
{функция обновления INI-файла клиента на сервере}
function IC_update_ini(inifile: Pchar): integer;  external 'irbis64_client.dll';
{функция чтения текстового ресурса}
function IC_getresourse(Apath: integer; Adbn,Afilename: Pchar; var answer: Pchar; abufsize: integer): integer;  external 'irbis64_client.dll';
{очистка кэша ресурсов}
function IC_clearresourse: integer;  external 'irbis64_client.dll';
{функция группового чтения текстовых ресурсов}
function IC_getresoursegroup(var acontext: Pchar; var answer: Pchar; abufsize: integer): integer;  external 'irbis64_client.dll';
{функция чтения двоичного ресурса}
function IC_getbinaryresourse(Apath: integer; Adbn,Afilename: Pchar; var Abuffer: PBuffer): integer;   external 'irbis64_client.dll';
{функция записи текстового ресурса}
function IC_putresourse(Apath: integer; Adbn,Afilename,Aresourse: Pchar): integer;   external 'irbis64_client.dll';

{функции работы с мастер-файлом базы данных}
{чение записи}
function IC_read(Adbn: Pchar; Amfn,Alock: integer; var answer: Pchar; abufsize: integer): integer;  external 'irbis64_client.dll';
{чение записи с расформатированием}
function IC_readformat(Adbn: Pchar; Amfn,Alock: integer; Aformat: Pchar; var answer: Pchar; abufsize: integer; var answer1: Pchar; abufsize1: integer): integer;    external 'irbis64_client.dll';
{редактирование записи}
function IC_update(Adbn: Pchar; Alock,Aifupdate: integer; var answer: Pchar; abufsize: integer): integer;  external 'irbis64_client.dll';
{групповое редактирование записи}
function IC_updategroup(Adbn: Pchar; Alock,Aifupdate: integer; var answer: Pchar; abufsize: integer): integer;   external 'irbis64_client.dll';
{связанное редактирование группы записей - возможно из разных БД}
function IC_updategroup_sinhronize(Alock,Aifupdate: integer; Adbnames: Pchar; var answer: Pchar; abufsize: integer):integer;  external 'irbis64_client.dll';
{разблокировать запись}
function IC_runlock(Adbn: Pchar; Amfn: integer): integer;  external 'irbis64_client.dll';
{актуализировать запись}
function IC_ifupdate(Adbn: Pchar; Amfn: integer): integer;  external 'irbis64_client.dll';
{получит максимальный MFN базы данных}
function IC_maxmfn(Adbn: Pchar): integer;  external 'irbis64_client.dll';

{функции для работы с записью}
{определить порядковый номер поля в записи}
function IC_fieldn(Arecord: Pchar; Amet,Aocc: integer): integer;    external 'irbis64_client.dll';
{прочитать заданное поле/подполе}
function IC_field(Arecord: Pchar; nf: integer; delim: char; answer: Pchar; abufsize: integer): integer;     external 'irbis64_client.dll';
{добавить поле в запись}
function IC_fldadd(Arecord: Pchar; Amet,nf: integer; pole: Pchar; abufsize: integer): integer;     external 'irbis64_client.dll';
{заменить поле}
function IC_fldrep(Arecord: Pchar; nf: integer; pole: Pchar; abufsize: integer): integer;     external 'irbis64_client.dll';
{определить кол-во полей в записи}
function IC_nfields(Arecord: Pchar): integer;     external 'irbis64_client.dll';
{определить кол-во повторений поля с заданной меткой}
function IC_nocc(Arecord: Pchar; Amet: integer): integer;     external 'irbis64_client.dll';
{определить метку поля с заданным порядковым номером}
function IC_fldtag(Arecord: Pchar; nf: integer): integer;     external 'irbis64_client.dll';
{опустошить запись}
function IC_fldempty(Arecord: Pchar): integer;     external 'irbis64_client.dll';
{поменять mfn записи}
function IC_changemfn(Arecord: Pchar; newmfn: integer): integer;     external 'irbis64_client.dll';
{установить признак логически удаленной записи}
function IC_recdel(Arecord: Pchar): integer;     external 'irbis64_client.dll';
{снять признак логически удаленной записи}
function IC_recundel(Arecord: Pchar): integer;     external 'irbis64_client.dll';
{снять признак заблокированности}
function IC_recunlock(Arecord: Pchar): integer;    external 'irbis64_client.dll';

{прочитать mfn записи}
function IC_getmfn(Arecord: Pchar): integer;    external 'irbis64_client.dll';
{создать пустую запись}
function IC_recdummy(Arecord: Pchar; abufsize: integer): integer; external 'irbis64_client.dll';
{прочитать в статусе записи признак АКТУАЛИЗИРОВАННОСТИ}
function IC_isActualized(Arecord: Pchar): integer; external 'irbis64_client.dll';
{прочитать в статусе записи признак ЗАБЛОКИРОВАННОСТИ}
function IC_isLocked(Arecord: Pchar): integer;     external 'irbis64_client.dll';
{прочитать в статусе записи признак ЛОГИЧЕСКОЙ УДАЛЕННОСТИ}
function IC_isDeleted(Arecord: Pchar): integer;    external 'irbis64_client.dll';

{функции для работы со словарем базы данных}
{прочитать список терминов, начиная с заданного}
function IC_nexttrm(Adbn,Aterm: Pchar; Anumb: integer; answer: Pchar; abufsize: integer): integer; external 'irbis64_client.dll';
{прочитать список терминов, начиная с заданного, и расформатировать документы по первой ссылке}
function IC_nexttrmgroup(Adbn,Aterm: Pchar; Anumb: integer; Aformat: Pchar; answer: Pchar; abufsize: integer): integer; external 'irbis64_client.dll';
{прочитать список терминов, начиная с заданного, в обратном порядке}
function IC_prevtrm(Adbn,Aterm: Pchar; Anumb: integer; answer: Pchar; abufsize: integer): integer; external 'irbis64_client.dll';
{прочитать список терминов, начиная с заданного, в обратном порядке и расформатировать документы по первой ссылке}
function IC_prevtrmgroup(Adbn,Aterm: Pchar; Anumb: integer; Aformat: Pchar; answer: Pchar; abufsize: integer): integer; external 'irbis64_client.dll';
{прочитать список ссылок для заданного термина}
function IC_posting(Adbn,Aterm: Pchar; Anumb,Afirst: integer; answer: Pchar; abufsize: integer): integer; external 'irbis64_client.dll';
{прочитать список первых ссылок для заданного списка терминов}
function IC_postinggroup(Adbn,Aterms,answer: Pchar; abufsize: integer): integer; external 'irbis64_client.dll';
{прочитать список ссылок для заданного термина и расформатировать записи им соответствующие}
function IC_postingformat(Adbn,Aterm: Pchar; Anumb,Afirst: integer; Aformat: Pchar; answer1: Pchar; abufsize1: integer; answer: Pchar; abufsize: integer): integer; external 'irbis64_client.dll';

{функции поиска}
{прямой поиск записей по заданному поисковому выражению}
function IC_search(Adbn,Asexp: Pchar; Anumb,Afirst: integer; Aformat: Pchar; answer: Pchar; abufsize: integer): integer; external 'irbis64_client.dll';
{последовательный поиск по результату прямого поиска и/или по заданному диапазону записей}
function IC_searchscan(Adbn,Asexp: Pchar; Anumb,Afirst: integer; Aformat: Pchar; Amin,Amax: integer; Aseq: Pchar; answer: Pchar; abufsize: integer): integer; external 'irbis64_client.dll';

{функции форматирования}
{форматирование записи по MFN}
function IC_sformat(Adbn: Pchar; Amfn: integer; Aformat: Pchar; answer: Pchar; abufsize: integer): integer; external 'irbis64_client.dll';
{форматирование записи в клиентском представлении}
function IC_record_sformat(Adbn, Aformat,Arecord: Pchar; answer: Pchar; abufsize: integer): integer;  external 'irbis64_client.dll';
{форматирование группы записей}
function IC_sformatgroup(Adbn,Amfnlist,Aformat: Pchar; answer: Pchar; abufsize: integer): integer;  external 'irbis64_client.dll';

{пакетные функции}
{получить выходную табличную форму по заданному набору записей}
function IC_print(Adbn,Atab,Ahead,Amod,Asexp: Pchar; Amin,Amax: integer; Aseq,Amfnlist: Pchar; answer: Pchar; abufsize: integer): integer;  external 'irbis64_client.dll';
{получить выходную форму статистических распределений заданных параметров по заданному набору записей}
function IC_stat(Adbn,Astat,Asexp: Pchar; Amin,Amax: integer; Aseq,Amfnlist: Pchar; answer: Pchar; abufsize: integer): integer; external 'irbis64_client.dll';
{выполнить глобальную корректировку заданного набора записей}
function IC_gbl(Adbn: Pchar; Aifupdate: integer; Agbl,Asexp: Pchar; Amin,Amax: integer; Aseq,Amfnlist: Pchar; answer: Pchar; abufsize: integer): integer;  external 'irbis64_client.dll';

{перезапустить сервер}
function IC_adm_restartserver:  integer;  external 'irbis64_client.dll';
function IC_adm_getDeletedList(Adbn: Pchar; answer: Pchar; abufsize: integer):integer;  external 'irbis64_client.dll';
function IC_adm_getallDeletedLists(Adbn: Pchar; answer: Pchar; abufsize: integer):integer; external 'irbis64_client.dll';
{опустошение БД}
function IC_adm_dbempty(Adbn: Pchar):integer; external 'irbis64_client.dll';
{удаление БД}
function IC_adm_dbdelete(Adbn: Pchar):integer; external 'irbis64_client.dll';
{создание новой БД}
function IC_adm_newdb(Adbn,Adef: Pchar; AReader: integer):integer; external 'irbis64_client.dll';
{снять монопольную блокировку БД}
function IC_adm_DBunlock(Adbn: Pchar):integer; external 'irbis64_client.dll';
{снять блокировку записей (MFN)}
function IC_adm_DBunlockMFN(Adbn: Pchar; Amfnlist: Pchar):integer; external 'irbis64_client.dll';
{создать словарь заново}
function IC_adm_DBStartCreateDictionry(Adbn: Pchar):integer;  external 'irbis64_client.dll';
{реорганизовать файл словаря}
function IC_adm_DBStartReorgDictionry(Adbn: Pchar):integer;  external 'irbis64_client.dll';
{реорганизовать файл документов}
function IC_adm_DBStartReorgMaster(Adbn: Pchar):integer;  external 'irbis64_client.dll';
{список зарегистрированных клиентов}
function IC_adm_getClientlist(answer: Pchar; abufsize: integer):integer;  external 'irbis64_client.dll';
{список клиентов для доступа к серверу}
function IC_adm_getClientslist(answer: Pchar; abufsize: integer):integer; external 'irbis64_client.dll';
{список запущенных процессов}
function IC_adm_getProcessList(answer: Pchar; abufsize: integer):integer; external 'irbis64_client.dll';
{обновить список клиентов для доступа к серверу}
function IC_adm_SetClientslist(AClientMnu: Pchar):integer; external 'irbis64_client.dll';


{вспомогательные функции}
{подтвержение регистрации}
function IC_nooperation:integer; external 'irbis64_client.dll';
{получить элемент ссылки}
function IC_getposting(APost: Pchar; AType: integer): integer;   external 'irbis64_client.dll';
{заменить реальные разделители строк $0D0A на псевдоразделители $3130}
function IC_reset_delim(Aline: Pchar): Pchar;    external 'irbis64_client.dll';
{заменить псевдоразделители разделители строк $3130 на реальные $0D0A}
function IC_delim_reset(Aline: Pchar): Pchar;    external 'irbis64_client.dll';
end.


