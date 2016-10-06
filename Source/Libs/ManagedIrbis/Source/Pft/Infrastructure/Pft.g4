//============================================================
// УПРОЩЕННАЯ ГРАММАТИКА ЯЗЫКА ФОРМАТИРОВАНИЯ ИРБИС
// грамматика для ANTLR 4.2
// Автор: А. В. Миронов
// Версия: 0.2.26
//============================================================

grammar Pft;

// Стартовый символ
program         :   pftInfoBlock?
                    compositeElement*
                    EOF
                ;

// Составной элемент
compositeElement
                :   (   simpleFormat
                    |   commaOperator
                    |   assignment
                    |   forLoop
                    |   whileLoop
                    |   selectStatement
                    |   procedureStatement
                    |   callStatement
                    |   groupStatement
                    |   extendedGroupStatement
                    |   conditionalStatement
                    |   unused
                    )
                ;

compositeList   : compositeElement*
                ;

simpleFormat    :   primaryElement+
                ;

//============================================================
// БЛОК ИНФОРМАЦИИ О СКРИПТЕ
//============================================================

pftInfoBlock    :   PFT
                    LPAREN
                    (   authorInfo
                    |   titleInfo
                    |   versioInfo
                    |   formatInfo
                    )+
                    RPAREN
                ;

authorInfo      :   AUTHOR
                    EQUALS
                    UNCONDITIONAL
                    SEMICOLON
                ;

titleInfo       :   TITLE
                    EQUALS
                    UNCONDITIONAL
                    SEMICOLON
                ;

versioInfo      :   VERSION
                    EQUALS
                    UNCONDITIONAL
                    SEMICOLON
                ;

formatInfo      :   FORMAT
                    EQUALS
                    (   PLAIN
                    |   HTML
                    |   RTF
                    )
                    SEMICOLON
                ;

//============================================================
// ОПЕРАТОР "ЗАПЯТАЯ"
//============================================================

commaOperator   :   COMMA
                ;

//============================================================
// ПРИСВАИВАНИЕ
//============================================================

assignment      : arithAssignment
                | stringAssignment
                ;

// Присваивание числа
arithAssignment
                :   ID
                    EQUALS
                    arithExpr
                    SEMICOLON
                ;

// Присваивание строки
stringAssignment
                :   ID
                    EQUALS
                    (   primaryElement+
                    |   groupStatement
                    |   extendedGroupStatement
                    )
                    SEMICOLON
                ;

//============================================================
// ЦИКЛЫ
//============================================================

// Цикл ДЛЯ
forLoop         :   FOR
                    LPAREN
                    assignment
                    condition
                    SEMICOLON
                    assignment
                    RPAREN
                    (   simpleFormat
                    |   commaOperator
                    |   assignment
                    |   forLoop
                    |   whileLoop
                    |   selectStatement
                    |   callStatement
                    |   groupStatement
                    |   extendedGroupStatement
                    |   conditionalStatement
                    )*
                    NEXT
                ;

// Цикл ПОКА
whileLoop       :   WHILE
                    LPAREN
                    condition
                    RPAREN
                    (   simpleFormat
                    |   commaOperator
                    |   assignment
                    |   forLoop
                    |   whileLoop
                    |   selectStatement
                    |   callStatement
                    |   groupStatement
                    |   extendedGroupStatement
                    |   conditionalStatement
                    )*
                    NEXT
                ;

//============================================================
// ВЫБОР
//============================================================

// Оператор выбора (рудиментарный)
selectStatement :   SELECT selectValue=simpleFormat
                    (   CASE
                        optionValue=UNCONDITIONAL
                        COLON
                        optionFormat=compositeList
                    )+
                    (   ELSE
                        elseFormat=compositeList
                    )?
                    END
                ;

//============================================================
// ПРОЦЕДУРА
//============================================================

// Объявление процедуры
procedureStatement
                :   PROCEDURE
                    name=ID
                    LPAREN
                    args=procedureArguments
                    RPAREN
                    body=procedureBody
                    END
                ;

// Перечисление аргументов процедуры
procedureArguments
                :   (   ID
                        (   COMMA
                            ID
                        )*
                    )?
                ;

// Тело процедуры
procedureBody   :   (   simpleFormat
                    |   commaOperator
                    |   assignment
                    |   forLoop
                    |   whileLoop
                    |   selectStatement
                    |   groupStatement
                    |   extendedGroupStatement
                    |   callStatement
                    |   conditionalStatement
                    )*
                ;

// Вызов процедуры
callStatement   :   CALL
                    ID
                    (   LPAREN
                        compositeList
                        RPAREN
                    )
                ;

//============================================================
// ПЕРВИЧНЫЕ ЭЛЕМЕНТЫ ФОРМАТИРОВАНИЯ
//============================================================

primaryElement  :   fieldReference
                |   globalReference
                |   variableReference

                |   appDirFunction
                |   appendFileFunction
                |   appSettingFunction
                |   askFunction
                |   beepOperator
                |   boldFunction
                |   breakOperator
                |   catFunction
                |   center
                |   changeDbFunction
                |   chooseFunction
                |   chrFunction
                |   clientVersionFunction
                |   cmdlineFunction
                |   colorFunction
                |   combineFunction
                |   commandC
                |   commandX
                |   coutFunction
                |   createDbFunction
                |   cpuFunction
                |   curdirFunction
                |   databaseFunction
                |   dateFunction
                |   debugBreak
                |   debugStatement
                |   deleteDbFunction
                |   deleteFileFunction
                |   delRecFunction
                |   eatFunction
                |   editTextFunction
                |   errorStatement
                |   escapedLiteral
                |   expandEnvFunction
                |   extractDirFunction
                |   extractDriveFunction
                |   extractExtFunction
                |   extractNameFunction
                |   fatalStatement
                |   f2Function
                |   fFunction
                |   fontNameFunction
                |   fontSizeFunction
                |   formatExitStatement
                |   getenvFunction
                |   hashOperator
                |   header1Function
                |   header2Function
                |   header3Function
                |   hostFunction
                |   iffFunction
                |   includeFunction
                |   includeStatement
                |   incrementFunction
                |   italicFunction
                |   leftFunction
                |   lineBreak
                |   localIPFunction
                |   longLiteral
                |   machineNameFunction
                |   mapFunction
                |   messageFunction
                |   mfnOperator
                |   midFunction
                |   modeSwitch
                |   msgFunction
                |   mstnameFunction
                |   newrecFunction
                |   nlOperator
                |   nowFunction
                |   organizationFunction
                |   osFunction
                |   padFunction
                |   padLeftFunction
                |   padRightFunction
                |   pageBreak
                |   paraOperator
                |   percentOperator
                |   platformFunction
                |   popModeOperator
                |   portFunction
                |   procFunction
                |   pushModeOperator
                |   putenvFunction
                |   readFileFunction
                |   readLineFunction
                |   refFunction
                |   replaceFunction
                |   requireClientFunction
                |   requireServerFunction
                |   revertFunction
                |   rightFunction
                |   runtimeFunction
                |   serverVersionFunction
                |   sFunction
                |   slashOperator
                |   sysdirFunction
                |   systemFunction
                |   tempdirFunction
                |   timeFunction
                |   tolowerFunction
                |   touchFileFunction
                |   toupperFunction
                |   traceStatement
                |   trimFunction
                |   trimLeftFunction
                |   trimRightFunction
                |   truncateFileFunction
                |   typeFunction
                |   unconditionalLiteral
                |   undelRecFunction
                |   underlineFunction
                |   userFunction
                |   warningStatement
                |   writeFunction
                |   writeFileFunction
                ;

//-------------------------------------------

// Папка, в которую установлено исполняющееся приложение
// (если имеет смысл, иначе пустая строка)
appDirFunction  :   APPDIR ( LPAREN RPAREN )?
                ;

// Добавление текста в локальный файл
// Если файл не существует, он будет создан
appendFileFunction
                :   APPENDFILE
                    LPAREN
                    name=simpleFormat
                    COMMA
                    text=compositeList
                    RPAREN
                ;

// Получение настроек программы
appSettingFunction
                :   APPSETTING
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Запрос информации у пользователя
askFunction     :   ASK
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Сигнал динамика
beepOperator    :   BEEP
                ;

// Выделение текста жирным
boldFunction    :   BOLD
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Выход из группы, цикла или скрипта
breakOperator   :   BREAK
                ;

// Чтение файла с сервера
catFunction     :   CAT
                    LPAREN
                    text=simpleFormat
                    RPAREN
                ;

// Центрирование параграфа
center          :   CENTER
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Переключение на другую базу данных
changeDbFunction
                :   CHANGEDB
                    LPAREN
                    name=simpleFormat
                    LPAREN
                ;

// Выбор пользователем одного из вариантов
// (если позволяет платформа)
chooseFunction  :   CHOOSE
                    LPAREN
                    prompt=compositeList
                    RPAREN
                ;

// Получение символа по его UNICODE-коду
chrFunction     :   CHR
                    LPAREN
                    code=arithExpr
                    RPAREN
                ;

// Версия клиентской библиотеки
clientVersionFunction
                :   CLIENTVERSION ( LPAREN RPAREN )?
                ;

// Получение аргументов командной строки
cmdlineFunction :   CMDLINE ( LPAREN RPAREN )?
                ;

// Вывод текста указанным цветом (если позволяет формат)
colorFunction   :   COLOR
                    LPAREN
                    color=simpleFormat
                    COMMA
                    text=compositeList
                    RPAREN
                ;

// Получение полного пути к файлу из фрагментов
combineFunction :   COMBINE
                    LPAREN
                    arg1=compositeList
                    COMMA
                    arg2=compositeList
                    RPAREN
                ;

// Табуляция в указанную позицию
commandC        :   COMMANDC
                ;

// Вставка указанного числа пробелов
commandX        :   COMMANDX
                ;

// Вывод строки в консоль
// (если позволяет платформа)
coutFunction    :   COUT
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Создание пустой библиографической базы данных
createDbFunction
                :   CREATEDB
                    LPAREN
                    name=simpleFormat
                    RPAREN
                ;

// Количество установленных процессоров
cpuFunction     :   CPU ( LPAREN RPAREN )?
                ;

// Текущая директория
curdirFunction  :   CURDIR ( LPAREN RPAREN )?
                ;

// Описание текущей базы данных
// (из dbnam.mnu)
databaseFunction
                :   DATABASE ( LPAREN RPAREN )?
                ;

// Текущая дата
dateFunction    :   DATE
                    (   LPAREN
                        format=compositeList
                        RPAREN
                    )?
                ;

// Остановка в отладчике скрипта (если подключен)
debugBreak      :   BANG
                ;

// Отладочная печать (видна в VS)
debugStatement  :   DEBUG
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Удаление указанной базы данных
deleteDbFunction
                :   DELETEDB
                    LPAREN
                    name=simpleFormat
                    RPAREN
                ;

deleteFileFunction
                :   DELETEFILE
                    LPAREN
                    name=compositeList
                    RPAREN
                ;

// Удаление текущей или указанной записи
delRecFunction  :   DELREC
                    (   LPAREN
                        mfn=arithExpr?
                        RPAREN
                    )?
                ;

// Съедает переданный текст
eatFunction     :   EAT
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Редактирование текста в окне
// (если позволяет платформа)
editTextFunction
                :   EDITTEXT
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Сообщение об ошибке
errorStatement  :   ERROR
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Литерал, могущий содержать одинарную и двойные кавычки
escapedLiteral  :   ESCAPED
                ;

// Расширение переменных
expandEnvFunction
                :   EXPANDENV
                    LPAREN
                    text=simpleFormat
                    RPAREN
                ;

// Извлечение из полного пути к файлу директории
extractDirFunction
                :   EXTRACTDIR
                    LPAREN
                    path=compositeList
                    RPAREN
                ;

// Извлечение из полного пути к файлу диска
extractDriveFunction
                :   EXTRACTDRIVE
                    LPAREN
                    path=compositeList
                    RPAREN
                ;

// Извлечение из полного пути к файлу расширения
extractExtFunction
                :   EXTRACTEXT
                    LPAREN
                    path=compositeList
                    RPAREN
                ;

// Извлечение из полного пути к файлу имени
extractNameFunction
                :   EXTRACTNAME
                    LPAREN
                    path=compositeList
                    RPAREN
                ;

// Сообщение о фатальной ошибке и завершение работы программы
fatalStatement  :   FATAL
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Правильное форматирование числа с плавающей точкой
f2Function      :   F2
                    LPAREN
                    value=arithExpr
                    (   COMMA
                        format=compositeList
                    )?
                    RPAREN
                ;

// Форматирование числа с плавающей точкой по-ИРБИСовски
fFunction       :   F
                    LPAREN
                    arg1=arithExpr
                    (   COMMA
                        arg2=arithExpr
                        (   COMMA
                            arg3=arithExpr
                        )?
                    )?
                    RPAREN
                ;

// Смена шрифта
fontNameFunction
                :   FONTNAME
                    LPAREN
                    name=compositeElement
                    COMMA
                    text=compositeList
                    RPAREN
                ;

// Изменение размера шрифта
fontSizeFunction
                :   FONTSIZE
                    LPAREN
                    size=arithExpr
                    COMMA
                    text=compositeList
                    RPAREN
                ;

// Форматный выход
formatExitStatement
                :   FORMATEXIT
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Получение значения переменной
getenvFunction  :   GETENV
                    LPAREN
                    name=compositeList
                    RPAREN
                ;

// Переход на новую строку
hashOperator    :   HASH
                ;

// Заголовок 1 уровня
header1Function :   HEADER1
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Заголовок 2 уровня
header2Function :   HEADER2
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Заголовок 3 уровня
header3Function :   HEADER3
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Адрес хоста, с которым установлено подключение
// Если нет подключения, то пустая строка
hostFunction    :   HOST ( LPAREN RPAREN )?
                ;

// Замена тернарному оператору
iffFunction     :   IFF
                    LPAREN
                    condition
                    COMMA
                    thenBranch=simpleFormat
                    COMMA
                    elseBranch=simpleFormat
                    RPAREN
                ;

// Вложенный формат
includeFunction :   INCLUDE
                    LPAREN
                    name=compositeList
                    RPAREN
                ;

// Вложенный формат
includeStatement
                :   INCLUSION
                ;

// Инкрементация числа в строке
incrementFunction
                :   INCREMENT
                    LPAREN
                    text=simpleFormat
                    RPAREN
                ;

// Получение значения из INI-файла (серверного)
iniFunction     :   INI
                    LPAREN
                    section=simpleFormat
                    COMMA
                    name=simpleFormat
                    (   COMMA
                        defaultValue=simpleFormat
                    )?
                    RPAREN
                ;

// Вывод текста курсивом
italicFunction  :   ITALIC
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Начальная часть строки
leftFunction    :   LEFT
                    LPAREN
                    text=simpleFormat
                    COMMA
                    len=arithExpr
                    RPAREN
                ;

// Перевод строки (но не абзаца!)
lineBreak       :   LINEBREAK
                ;

// Клиентский IP-адрес
localIPFunction :   LOCALIP ( LPAREN RPAREN )?
                ;

// Длинный литерал, могущий содержать переводы строки,
// кавычки и прочие символы
longLiteral     :   LONGLITERAL
                ;

// Имя клиентской машины
machineNameFunction
                :   MACHINENAME ( LPAREN RPAREN )?
                ;

// Раскодировка по указанному справочнику
mapFunction     :   MAP
                    LPAREN
                    menu=simpleFormat
                    COMMA
                    key=compositeList
                    RPAREN
                ;

// Показ окна с сообщением (если доступно)
messageFunction :   MESSAGE
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// MFN текущей записи (может быть 0)
mfnOperator     :   MFN
                |   MFNWITHLENGTH
                ;

// Получение фрагмента строки
midFunction     :   MID
                    LPAREN
                    text=simpleFormat
                    COMMA
                    offset=arithExpr
                    COMMA
                    len=arithExpr
                    RPAREN
                ;

// Переключение режима вывода полей
modeSwitch      :   MODESWITCH
                ;

// Получение текста из IRBISMSG.TXT
msgFunction     :   MSG
                    LPAREN
                    index=arithExpr
                    RPAREN
                ;

// Имя мастер-файла текущей базы
mstnameFunction :   MSTNAME ( LPAREN RPAREN )?
                ;

// Создание новой пустой записи
newrecFunction  :   NEWREC ( LPAREN RPAREN )?
                ;

// Перевод строки, специфичный для платформы
nlOperator      :   NL
                ;

// Текущая дата/время
nowFunction     :   NOW
                    (   LPAREN
                        format=compositeList
                        RPAREN
                    )?
                ;

// Организация, на которую зарегистрирован сервер
organizationFunction
                :   ORGANIZATION ( LPAREN RPAREN )?
                ;

// Операционная система (с версией и сервис-паком, если есть)
osFunction      :   OS ( LPAREN RPAREN )?
                ;

// Дополнение строки пробелами до указанной длины
padFunction     :   PAD
                    LPAREN
                    text=compositeElement
                    COMMA
                    len=arithExpr
                    RPAREN
                ;

// Дополнение строки пробелами слева
padLeftFunction
                :   PADLEFT
                    LPAREN
                    text=compositeElement
                    COMMA
                    len=arithExpr
                    RPAREN
                ;

// Дополнение строки пробелами справа
padRightFunction
                :   PADRIGHT
                    LPAREN
                    text=compositeElement
                    COMMA
                    len=arithExpr
                    RPAREN
                ;

// Переход на новую страницу (если доступен в формате)
pageBreak       :   PAGEBREAK
                ;

// Начало нового параграфа (если доступно в формате)
paraOperator    :   PARA
                ;

// Удаление пустой строки
percentOperator :   PERCENT
                ;

// Имя платформы: windows, unix, android, silverlight,
// wince, osx и т. д.
platformFunction
                :   PLATFORM ( LPAREN RPAREN )?
                ;

// Восстановление ранее сохраненного режима вывода полей
popModeOperator :   POPMODE
                ;

// Порт сервера, к которому выполнено подключение
// Если нет подключения, то 0
portFunction    :   PORT ( LPAREN RPAREN )?
                ;

// Модификация текущей записи
// (взято из CISIS)
procFunction    :   PROC
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Запоминание режима вывода полей
pushModeOperator
                :   PUSHMODE
                ;

// Изменение значения переменной окружения
putenvFunction  :   PUTENV
                    LPAREN
                    name=simpleFormat
                    COMMA
                    value=compositeList
                    RPAREN
                ;

// Чтение локального файла
readFileFunction
                :   READFILE
                    LPAREN
                    name=compositeList
                    RPAREN
                ;

// Считывание строки с консоли, если доступно
// Если недоступно, возвращается пустая строка
readLineFunction
                :   READLINE
                    (   LPAREN
                        prompt=compositeList
                        RPAREN
                    )
                ;

// Позволяет извлечь данные из альтернативной записи
// файла документов (той же самой БД)
refFunction     :   REF
                    LPAREN
                    arg1=arithExpr
                    COMMA
                    arg2=compositeList
                    RPAREN
                ;

// Замена подстроки в строке
replaceFunction :   REPLACE
                    LPAREN
                    arg1=simpleFormat
                    COMMA
                    arg2=simpleFormat
                    COMMA
                    arg3=simpleFormat
                    RPAREN
                ;

// Требование минимальной версии клиентской библиотеки
// При невыполнении аварийное завершение программы
requireClientFunction
                :   REQUIRECLIENT
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Требование минимальной версии сервера
// При невыполнении аварийное завершение программы
requireServerFunction
                :   REQUIRESERVER
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Восстановление предыдущей версии записи
revertFunction  :   REVERT
                    (   LPAREN
                        version=arithExpr
                        RPAREN
                    )?
                ;

// Получение указанного числа символов в конце строки
rightFunction   :   RIGHT
                    LPAREN
                    text=compositeElement
                    COMMA
                    len=arithExpr
                    RPAREN
                ;

// Версия .NET Framework или аналогичного рантайма
runtimeFunction :   RUNTIME ( LPAREN RPAREN )?
                ;

// Версия сервера
serverVersionFunction
                :   SERVERVERSION ( LPAREN RPAREN  )?
                ;

// Склейка строк
sFunction       :   S
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Переход на новую строку
slashOperator   :   SLASH
                ;

// Путь к системной папке Windows
// На других платформах пустая строка
sysdirFunction  :   SYSDIR ( LPAREN RPAREN )?
                ;

// Выполнение команды и вставка её вывода
systemFunction  :   SYSTEM
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Путь до временной папки
tempdirFunction :   TEMPDIR ( LPAREN RPAREN )?
                ;

// Текущее время
timeFunction    :   TIME
                    (   LPAREN
                        format=compositeList
                        RPAREN
                    )?
                ;

// Перевод в нижний регистр
tolowerFunction :   TOLOWER
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Создание пустого лоакального файла,
// если файла с указанным именем не существует
// Установка у файла текущей даты модификации
touchFileFunction
                :   TOUCHFILE
                    LPAREN
                    name=compositeList
                    RPAREN
                ;

// Перевод в верхний регистр
toupperFunction :   TOUPPER
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Трассировочное сообщение (видно в VS)
traceStatement  :   TRACE
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Удаление пробелов в начале и в конце строки
trimFunction    :   TRIM
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Удаление пробелов в начале строки
trimLeftFunction
                :   TRIMLEFT
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Удаление пробелов в конце строки
trimRightFunction
                :   TRIMRIGHT
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Обнуление длины локального файла.
// Если файл не существует, он будет создан
truncateFileFunction
                :   TRUNCATEFILE
                    LPAREN
                    name=compositeList
                    RPAREN
                ;

// Вычисление типа выражения (взято из CISIS)
typeFunction    :   TYPE
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Безусловный литерал
unconditionalLiteral
                :   UNCONDITIONAL
                ;

// Восстановление удаленной записи
undelRecFunction
                :   UNDELREC ( LPAREN RPAREN )?
                ;

// Вывод текста с подчеркиванием
underlineFunction
                :   UNDERLINE
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Логин текущего пользователя
userFunction
                :   USER ( LPAREN RPAREN )?
                ;

// Предупреждение
warningStatement
                :   WARNING
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Запись в файл на сервере
writeFunction   :   WRITE
                    LPAREN
                    name=simpleFormat
                    COMMA
                    text=compositeList
                    RPAREN
                ;

// Запись текста в локальный файл
writeFileFunction
                :   WRITEFILE
                    LPAREN
                    name=simpleFormat
                    COMMA
                    text=compositeList
                    RPAREN
                ;

//============================================================
// ССЫЛКА НА ПОЛЕ/ПОДПОЛЕ/ГЛОБАЛЬНУЮ ПЕРЕМЕННУЮ
//============================================================

// Ссылка на поле (с префиксной и постфиксной частью, если есть)
fieldReference  :   leftHand
                    FIELD
                    rightHand
                ;

// Ссылка на глобальную переменную (с префиксной и постфиксной
// частью, если есть)
globalReference :   leftHand
                    GLOBALVAR
                    rightHand
                ;

// Префиксная часть
leftHand        :   (   conditionalLiteral
                        (   conditionalLiteral
                        |   commandC
                        |   commandX
                        |   modeSwitch
                        |   slashOperator
                        |   hashOperator
                        |   percentOperator
                        )*
                    )?
                    (   repeatableLiteral
                        PLUS?
                    )?
                ;

// Постфиксная часть
rightHand       :   (   PLUS?
                        repeatableLiteral
                    )?
                    conditionalLiteral?
                ;

// Список полей/подполей через запятую
// Например: v200^a, v461^c
fieldList       :   fieldReference
                    (
                        COMMA
                        fieldReference
                    )*
                ;

conditionalLiteral
                :   CONDITIONAL
                ;

repeatableLiteral
                :   REPEATABLE
                ;

//============================================================
// ОБЫЧНАЯ ГРУППА
//============================================================

groupStatement  :   LPAREN
                    (   simpleFormat
                    |   commaOperator
                    |   assignment
                    |   forLoop
                    |   whileLoop
                    |   selectStatement
                    |   callStatement
                    |   conditionalStatement
                    )*
                    RPAREN
                ;

//============================================================
// РАСШИРЕННАЯ ГРУППА
//============================================================

extendedGroupStatement
                :   LCURLY
                    fieldList
                    COLON
                    (   simpleFormat
                    |   commaOperator
                    |   assignment
                    |   forLoop
                    |   whileLoop
                    |   selectStatement
                    |   callStatement
                    |   conditionalStatement
                    )*
                    RCURLY
                ;

//============================================================
// УСЛОВНЫЙ ОПЕРАТОР
//============================================================

// Условный оператор
conditionalStatement
                :   IF condition
                    THEN thenBranch=compositeList
                    ( ELSE elseBranch=compositeList )?
                    FI
                ;

// Варианты условий
condition       :   condition
                    op=(AND|OR)
                    condition

                |   conditionNot
                |   conditionParen
                |   conditionString
                |   conditionBoolean
                |   conditionArith
                ;

// Отрицание выражения
conditionNot    :   NOT
                    condition
                ;

// Выражение в скобках
conditionParen  :   LPAREN
                    condition
                    RPAREN
                ;

// Сравнение двух строк
conditionString :   left=primaryElement
                    op=(COLON|EQUALS|NOTEQUALS|MORE|MOREEQ|LESS|LESSEQ)
                    right=primaryElement
                ;

// Булево условие
conditionBoolean
                :   fieldPresense
                |   startsWithFunction
                |   endsWithFunction
                |   interactiveFunction
                |   x64Function
                |   deletedFunction
                |   existFunction
                |   connectedFunction
                |   fileExistFunction
                ;

// Сравнение двух чисел
conditionArith  :   left=arithExpr
                    op=(EQUALS|NOTEQUALS|MORE|MOREEQ|LESS|LESSEQ)
                    right=arithExpr
                ;

//============================================================
// АРИФМЕТИКА
//============================================================

// Арифметическое выражение
arithExpr       :   left=arithExpr
                    op=(STAR|SLASH)
                    right=arithExpr

                |   left=arithExpr
                    op=(PLUS|MINUS)
                    right=arithExpr

                |   numericValue
                ;

// Возможные варианты значений
numericValue    :   floatValue
                |   minusValue
                |   parenValue
                |   arithFunction
                |   mfnValue
                |   variableReference
                ;

// Число с плавающей точкой без знака
// (может быть целым)
floatValue      :   FLOAT
                ;

// Минус-выражение
minusValue      :   MINUS
                    arithExpr
                ;

// Выражение в скобках
parenValue      :   LPAREN
                    arithExpr
                    RPAREN
                ;

// MFN текущей записи
// Может быть 0
mfnValue        :   MFN
                ;

// Ссылка на переменную
// Например: $a
variableReference
                :   ID
                ;

//============================================================
// АРИФМЕТИЧЕСКИЕ ФУНКЦИИ
//============================================================

arithFunction   :   rsumFunction
                |   rmaxFunction
                |   rminFunction
                |   ravrFunction
                |   valFunction
                |   lFunction
                |   npostFunction
                |   sizeFunction
                |   workingSetFunction
                |   ioccOperator
                |   noccFunction
                |   noccOperator
                |   licenseCountFunction
                |   licenseLeftFunction
                |   licenseUsedFunction
                |   ordFunction
                |   totalMemoryFunction
                |   freeMemoryFunction
                ;

// Сравнение двух чисел, представленных строками
compareFunction :   COMPARE
                    LPAREN
                    left=simpleFormat
                    COMMA
                    right=simpleFormat
                    RPAREN
                ;

fileSizeFunction
                :   FILESIZE
                    LPAREN
                    name=compositeList
                    RPAREN
                ;

// Количество свободной памяти, доступной программе
// (если возможно вычисление)
freeMemoryFunction
                :   FREEMEMORY ( LPAREN RPAREN )?
                ;

// Номер текущего повторения в группе
// Нумерация с 1
// Если не в группе, то 0
ioccOperator    :   IOCC
                ;

// Использует текст, полученный в результате вычисления аргумента,
// в качестве термина доступа для инвертированного файла и возвращает
// MFN первой ссылки на этот термин, если она есть
lFunction       :   L
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Количество пользовательских лицензий,
// установленных на сервере
// Если нет подключения, то 0
licenseCountFunction
                :   LICENSECOUNT ( LPAREN RPAREN )?
                ;

// Количество оставшихся свободных лицензий
// Если нет подключения, то 0
licenseLeftFunction
                :   LICENSELEFT ( LPAREN RPAREN )?
                ;

// Количество использованных лицензий
// Если нет подключения, то 0
licenseUsedFunction
                :   LICENSEUSED ( LPAREN RPAREN )?
                ;

// Максимальный MFN в базе + 1
maxMfnFunction  :   MAXMFN ( LPAREN RPAREN )?
                ;

// Число повторений указанного поля
noccFunction    :   NOCC
                    LPAREN
                    FIELD
                    RPAREN
                ;

// Прогнозируемое число повторений в группе
// Если не в группе, то 0
noccOperator    :   NOCC
                ;

// Число повторений указанных полей
npostFunction   :   NPOST
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// UNICODE-код первого символа в строке
// Если строка пустая, то 0
ordFunction     :   ORD
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Среднее значение
ravrFunction    :   RSUM
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Максимальное значение
rmaxFunction    :   RSUM
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Минимальное значение
rminFunction    :   RSUM
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Сумма
rsumFunction    :   RSUM
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Длина строки
sizeFunction    :   SIZE
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Общее количество памяти, доступное операционной системе, в байтах
// (если возможно вычисление)
totalMemoryFunction
                :   TOTALMEMORY ( LPAREN RPAREN )?
                ;

// Преобразование строки в число по ИРБИСовски
valFunction     :   VAL
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Правильное преобразование строки в число
val2Function    :   VAL2
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Текущий размер рабочего набора программы в байтах
// (если возможно вычисление)
workingSetFunction
                :   WORKINGSET ( LPAREN RPAREN )?
                ;

//============================================================
// ЛОГИЧЕСКИЕ ФУНКЦИИ
//============================================================

// Установлено ли в настоящее время подключение к серверу?
connectedFunction
                :   CONNECTED ( LPAREN RPAREN )?
                ;

// Помечена ли текущая запись как удаленная?
deletedFunction :   DELETED ( LPAREN RPAREN )?
                ;

// Заканчивается ли строка указанным фрагментом?
endsWithFunction
                :   ENDSWITH
                    LPAREN
                    arg1=primaryElement
                    COMMA
                    arg2=primaryElement
                    RPAREN
                ;

// Существует ли указанная переменная
// или указанный файл (на сервере)?
existFunction   :   EXIST
                    LPAREN
                    (   ID              // существование переменной
                    |   simpleFormat    // существование файла на сервере
                    )
                    RPAREN
                ;

// Наличие/отсутствие поля/подполя
fieldPresense   :   P
                    LPAREN
                    FIELD
                    RPAREN
                |   A
                    LPAREN
                    FIELD
                    RPAREN
                ;

// Существование локального файла
fileExistFunction
                :   FILEEXIST
                    LPAREN
                    name=compositeList
                    RPAREN
                ;

// Есть ли хоть одно из перечисленных полей?
// Можно использовать регулярные выражения
// Например: if have('7[01][01]') then ...
haveFunction    :   HAVE
                    LPAREN
                    text=compositeList
                    RPAREN
                ;

// Доступны ли интерактивные возможности?
// По умолчанию - нет
interactiveFunction
                :   INTERACTIVE ( LPAREN RPAREN )?
                ;

// Начинается ли строка с указанного фрагмента?
startsWithFunction
                :   STARTSWITH
                    LPAREN
                    arg1=primaryElement
                    COMMA
                    arg2=primaryElement
                    RPAREN
                ;

// Программа выполняется на 64-битной машине
x64Function     :   X64 ( LPAREN RPAREN )?
                ;

//============================================================
// НЕИСПОЛЬЗУЕМЫЕ ЭЛЕМЕНТЫ
//============================================================

unused          :   conditionalLiteral
                |   repeatableLiteral
                ;

//============================================================
// ТЕРМИНАЛЫ
//============================================================

// Табуляция в указанную позицию
COMMANDC        :   [Cc]
                    [0-9]+
                ;

// Вставка указанного числа пробелов
COMMANDX        :   [Xx]
                    [0-9]+
                ;

// Условный литерал
CONDITIONAL     :   '"'
                    .*?
                    '"'
                ;

// Литерал, могущий содержать одинарные
// и двойные кавычки
ESCAPED         :   '`'
                    .*?
                    '`'
                ;

// Ссылка на поле/подполе
FIELD           :   [dvnDVN]
                    [0-9]+
                    ( '@' [0-9]+ )?
                    ( '^' [A-Za-z0-9*] )?
                    ( '[' ([0-9]+ | LAST) (MINUS [0-9]+)? ']')?
                    ( '*' [0-9]+ )?
                    ( '.' [0-9]+ )?
                    ( '#' [0-9]+ )?
                ;

// Число с плавающей запятой
FLOAT           :   ( // начинается с точки
                        '.'
                        [0-9]+
                        (   [Ee]
                            '-'?
                            [0-9]+
                        )?
                    )
                    |
                    ( // начинается с цифры
                        [0-9]+
                        (   '.'
                            [0-9]*
                        )?
                        (   [Ee]
                            '-'?
                            [0-9]+
                        )?
                    )
                ;

// Форматный выход
FORMATEXIT      :   [&]
                    [A-Za-z_]
                    [A-Za-z0-9_]*
                ;

// Ссылка на глобальную переменную
GLOBALVAR       :   [gG]
                    [0-9]+
                    ( '^' [A-Za-z0-9*] )?
                    ( '*' [0-9]+ )?
                    ( '.' [0-9]+ )?
                ;

// Переменная (число или строка)
ID              :   '$'
                    [0-9a-zA-Z_]+
                ;

// Включение файла
INCLUSION       :   AT
                    [A-Za-z0-9_.]+
                ;

// Длинный литерал с переводами строки
LONGLITERAL     :   '<<<'
                    .*?
                    '>>>'
                ;

// MFN с явным указанием длины
MFNWITHLENGTH   :   [Mm]
                    [Ff]
                    [Nn]
                    [(]
                    [0-9]+
                    [)]
                ;

// Переключатель режимов отображения полей
MODESWITCH      :   [Mm]
                    [PpHhDd]
                    [UuLl]
                ;

// Повторяющийся литерал
REPEATABLE      :   '|'
                    .*?
                    '|'
                ;

// Безусловный литерал
UNCONDITIONAL   :   '\''
                    .*?
                    '\''
                ;

// Зарезервированные слова и символы

A               :   [Aa];
AND             :   [Aa][Nn][Dd];
APPDIR          :   [Aa][Pp][Pp][Dd][Ii][Rr];
APPENDFILE      :   [Aa][Pp][Pp][Ee][Nn][Dd][Ff][Ii][Ll][Ee];
APPSETTING      :   [Aa][Pp][Pp][Ss][Ee][Tt][Tt][Ii][Nn][Gg];
ASK             :   [Aa][Ss][Kk];
AT              :   '@';
AUTHOR          :   [Aa][Uu][Tt][Hh][Oo][Rr];
BANG            :   '!';
BEEP            :   [Bb][Ee][Ee][Pp];
BOLD            :   [Bb][Oo][Ll][Dd];
BREAK           :   [Bb][Rr][Ee][Aa][Kk];
CALL            :   [Cc][Aa][Ll][Ll];
CASE            :   [Cc][Aa][Ss][Ee];
CAT             :   [Cc][Aa][Tt];
COLOR           :   [Cc][Oo][Ll][Oo][Rr];
CENTER          :   [Cc][Ee][Nn][Tt][Ee][Rr];
CHANGEDB        :   [Cc][Hh][Aa][Nn][Gg][Ee][Dd][Bb];
CHOOSE          :   [Cc][Hh][Oo][Oo][Ss][Ee];
CHR             :   [Cc][Hh][Rr];
CLIENTVERSION   :   [Cc][Ll][Ii][Ee][Nn][Tt][Vv][Ee][Rr][Ss][Ii][Oo][Nn];
CMDLINE         :   [Cc][Mm][Dd][Ll][Ii][Nn][Ee];
COLON           :   ':';
COMBINE         :   [Cc][Oo][Mm][Bb][Ii][Nn][Ee];
COMMA           :   ',';
COMPARE         :   [Cc][Oo][Mm][Pp][Aa][Rr][Ee];
CONNECTED       :   [Cc][Oo][Nn][Nn][Ee][Cc][Tt][Ee][Dd];
COUT            :   [Cc][Oo][Uu][Tt];
CPU             :   [Cc][Pp][Uu];
CREATEDB        :   [Cc][Rr][Ee][Aa][Tt][Ee][Dd][Bb];
CURDIR          :   [Cc][Uu][Rr][Dd][Ii][Rr];
DATABASE        :   [Dd][Aa][Tt][Aa][Bb][Aa][Ss][Ee];
DATE            :   [Dd][Aa][Tt][Ee];
DEBUG           :   [Dd][Ee][Bb][Uu][Gg];
DELETED         :   [Dd][Ee][Ll][Ee][Tt][Ee][Dd];
DELETEDB        :   [Dd][Ee][Ll][Ee][Tt][Ee][Dd][Bb];
DELETEFILE      :   [Dd][Ee][Ll][Ee][Tt][Ee][Ff][Ii][Ll][Ee];
DELREC          :   [Dd][Ee][Ll][Rr][Ee][Cc];
EAT             :   [Ee][Aa][Tt];
EDITTEXT        :   [Ee][Dd][Ii][Tt][Tt][Ee][Xx][Tt];
END             :   [Ee][Nn][Dd];
ELSE            :   [Ee][Ll][Ss][Ee];
ENDSWITH        :   [Ee][Nn][Dd][Ss][Ww][Ii][Tt][Hh];
EQUALS          :   '=';
ERROR           :   [Ee][Rr][Rr][Oo][Rr];
EXIST           :   [Ee][Xx][Ii][Ss][Tt];
EXPANDENV       :   [Ee][Xx][Pp][Aa][Nn][Dd][Ee][Nn][Vv];
EXTRACTDIR      :   [Ee][Xx][Tt][Rr][Aa][Cc][Tt][Dd][Ii][Rr];
EXTRACTDRIVE    :   [Ee][Xx][Tt][Rr][Aa][Cc][Tt][Dd][Rr][Ii][Vv][Ee];
EXTRACTEXT      :   [Ee][Xx][Tt][Rr][Aa][Cc][Tt][Ee][Xx][Tt];
EXTRACTNAME     :   [Ee][Xx][Tt][Rr][Aa][Cc][Tt][Nn][Aa][Mm][Ee];
F               :   [Ff];
F2              :   [Ff][2];
FATAL           :   [Ff][Aa][Tt][Aa][Ll];
FI              :   [Ff][Ii];
FILEEXIST       :   [Ff][Ii][Ll][Ee][Ee][Xx][Ii][Ss][Tt];
FILESIZE        :   [Ff][Ii][Ll][Ee][Ss][Ii][Zz][Ee];
FONTNAME        :   [Ff][Oo][Nn][Tt][Nn][Aa][Mm][Ee];
FONTSIZE        :   [Ff][Oo][Nn][Tt][Ss][Ii][Zz][Ee];
FOR             :   [Ff][Oo][Rr];
FORMAT          :   [Ff][Oo][Rr][Mm][Aa][Tt];
FREEMEMORY      :   [Ff][Rr][Ee][Ee][Mm][Ee][Mm][Oo][Rr][Yy];
GETENV          :   [Gg][Ee][Tt][Ee][Nn][Vv];
HASH            :   '#';
HAVE            :   [Hh][Aa][Vv][Ee];
HEADER1         :   [Hh][Ee][Aa][Dd][Ee][Rr][1];
HEADER2         :   [Hh][Ee][Aa][Dd][Ee][Rr][2];
HEADER3         :   [Hh][Ee][Aa][Dd][Ee][Rr][3];
HOST            :   [Hh][Oo][Ss][Tt];
HTML            :   [Hh][Tt][Mm][Ll];
IF              :   [Ii][Ff];
IFF             :   [Ii][Ff][Ff];
INCLUDE         :   [Ii][Nn][Cc][Ll][Uu][Dd][Ee];
INCREMENT       :   [Ii][Nn][Cc][Rr][Ee][Mm][Ee][Nn][Tt];
INI             :   [Ii][Nn][Ii];
INTERACTIVE     :   [Ii][Nn][Tt][Ee][Rr][Aa][Cc][Tt][Ii][Vv][Ee];
IOCC            :   [Ii][Oo][Cc][Cc];
ITALIC          :   [Ii][Tt][Aa][Ll][Ii][Cc];
L               :   [Ll];
LAST            :   [Ll][Aa][Ss][Tt];
LCURLY          :   '{';
LEFT            :   [Ll][Ee][Ff][Tt];
LESS            :   '<';
LESSEQ          :   '<=';
LICENSECOUNT    :   [Ll][Ii][Cc][Ee][Nn][Ss][Ee][Cc][Oo][Uu][Nn][Tt];
LICENSELEFT     :   [Ll][Ii][Cc][Ee][Nn][Ss][Ee][Ll][Ee][Ff][Tt];
LICENSEUSED     :   [Ll][Ii][Cc][Ee][Nn][Ss][Ee][Uu][Ss][Ee][Dd];
LINEBREAK       :   [Ll][Ii][Nn][Ee][Bb][Rr][Ee][Aa][Kk];
LOCALIP         :   [Ll][Oo][Cc][Aa][Ll][Ii][Pp];
LPAREN          :   '(';
MACHINENAME     :   [Mm][Aa][Cc][Hh][Ii][Nn][Ee][Nn][Aa][Mm][Ee];
MAP             :   [Mm][Aa][Pp];
MAXMFN          :   [Mm][Aa][Xx][Mm][Ff][Nn];
MESSAGE         :   [Mm][Ee][Ss][Aa][Gg][Ee];
MFN             :   [Mm][Ff][Nn];
MID             :   [Mm][Ii][Dd];
MINUS           :   '-';
MORE            :   '>';
MOREEQ          :   '>=';
MSG             :   [Mm][Ss][Gg];
MSTNAME         :   [Mm][Ss][Tt][Nn][Aa][Mm][Ee];
NEWREC          :   [Nn][Ee][Ww][Rr][Ee][Cc];
NEXT            :   [Nn][Ee][Xx][Tt];
NL              :   [Nn][Ll];
NOCC            :   [Nn][Oo][Cc][Cc];
NOT             :   [Nn][Oo][Tt];
NOTEQUALS       :   '<>';
NOW             :   [Nn][Oo][Ww];
NPOST           :   [Nn][Pp][Oo][Ss][Tt];
OR              :   [Oo][Rr];
ORD             :   [Oo][Rr][Dd];
ORGANIZATION    :   [Oo][Rr][Gg][Aa][Nn][Ii][Zz][Aa][Tt][Ii][Oo][Nn];
OS              :   [Oo][Ss];
P               :   [Pp];
PAD             :   [Pp][Aa][Dd];
PADLEFT         :   [Pp][Aa][Dd][Ll][Ee][Ff][Tt];
PADRIGHT        :   [Pp][Aa][Dd][Rr][Ii][Gg][Hh][Tt];
PAGEBREAK       :   [Pp][Aa][Gg][Ee][Bb][Rr][Ee][Aa][Kk];
PARA            :   [Pp][Aa][Rr][Aa];
PERCENT         :   '%';
PFT             :   [Pp][Ff][Tt];
PLAIN           :   [Pp][Ll][Aa][Ii][Nn];
PLATFORM        :   [Pp][Ll][Aa][Tt][Ff][Oo][Rr][Mm];
PLUS            :   '+';
POPMODE         :   [Pp][Oo][Pp][Mm][Oo][Dd][Ee];
PORT            :   [Pp][Oo][Rr][Tt];
PROC            :   [Pp][Rr][Oo][Cc];
PROCEDURE       :   [Pp][Rr][Oo][Cc][Ee][Dd][Uu][Rr][Ee];
PUSHMODE        :   [Pp][Uu][Ss][Hh][Mm][Oo][Dd][Ee];
PUTENV          :   [Pp][Uu][Tt][Ee][Nn][Vv];
RAVR            :   [Rr][Aa][Vv][Rr];
RCURLY          :   '}';
READFILE        :   [Rr][Ee][Aa][Dd][Ff][Ii][Ll][Ee];
READLINE        :   [Rr][Ee][Aa][Dd][Ll][Ii][Nn][Ee];
REF             :   [Rr][Ee][Ff];
REPLACE         :   [Rr][Ee][Pp][Ll][Aa][Cc][Ee];
REQUIRECLIENT   :   [Rr][Ee][Qq][Uu][Ii][Rr][Ee][Cc][Ll][Ii][Ee][Nn][Tt];
REQUIRESERVER   :   [Rr][Ee][Qq][Uu][Ii][Rr][Ee][Ss][Ee][Rr][Vv][Ee][Rr];
REVERT          :   [Rr][Ee][Vv][Ee][Rr][Tt];
RIGHT           :   [Rr][Ii][Gg][Hh][Tt];
RMAX            :   [Rr][Mm][Aa][Xx];
RMIN            :   [Rr][Mm][Ii][Nn];
RPAREN          :   ')';
RSUM            :   [Rr][Ss][Uu][Mm];
RTF             :   [Rr][Tt][Ff];
RUNTIME         :   [Rr][Uu][Nn][Tt][Ii][Mm][Ee];
S               :   [Ss];
SELECT          :   [Ss][Ee][Ll][Ee][Cc][Tt];
SEMICOLON       :   ';';
SERVERVERSION   :   [Ss][Ee][Rr][Vv][Ee][Rr][Vv][Ee][Rr][Ss][Ii][Oo][Nn];
SIZE            :   [Ss][Ii][Zz][Ee];
SLASH           :   '/';
STAR            :   '*';
STARTSWITH      :   [Ss][Tt][Aa][Rr][Tt][Ss][Ww][Ii][Tt][Hh];
SYSDIR          :   [Ss][Yy][Ss][Dd][Ii][Rr];
SYSTEM          :   [Ss][Yy][Ss][Tt][Ee][Mm];
TEMPDIR         :   [Tt][Ee][Mm][Pp][Dd][Ii][Rr];
THEN            :   [Tt][Hh][Ee][Nn];
TILDA           :   '~';
TIME            :   [Tt][Ii][Mm][Ee];
TITLE           :   [Tt][Ii][Tt][Ll][Ee];
TOLOWER         :   [Tt][Oo][Ll][Oo][Ww][Ee][Rr];
TOTALMEMORY     :   [Tt][Oo][Tt][Aa][Ll][Mm][Ee][Mm][Oo][Rr][Yy];
TOUCHFILE       :   [Tt][Oo][Uu][Cc][Hh][Ff][Ii][Ll][Ee];
TOUPPER         :   [Tt][Oo][Uu][Pp][Pp][Ee][Rr];
TRACE           :   [Tt][Rr][Aa][Cc][Ee];
TRIM            :   [Tt][Rr][Ii][Mm];
TRIMLEFT        :   [Tt][Rr][Ii][Mm][Ll][Ee][Ff][Tt];
TRIMRIGHT       :   [Tt][Rr][Ii][Mm][Rr][Ii][Gg][Hh][Tt];
TRUNCATEFILE    :   [Tt][Rr][Uu][Nn][Cc][Aa][Tt][Ee][Ff][Ii][Ll][Ee];
TYPE            :   [Tt][Yy][Pp][Ee];
UNDELREC        :   [Uu][Nn][Dd][Ee][Ll][Rr][Ee][Cc];
UNDERLINE       :   [Uu][Nn][Dd][Ee][Rr][Ll][Ii][Nn][Ee];
USER            :   [Uu][Ss][Ee][Rr];
VAL             :   [Vv][Aa][Ll];
VAL2            :   [Vv][Aa][Ll][2];
VERSION         :   [Vv][Ee][Rr][Ss][Ii][Oo][Nn];
WARNING         :   [Ww][Aa][Rr][Nn][Ii][Nn][Gg];
WHILE           :   [Ww][Hh][Ii][Ll][Ee];
WORKINGSET      :   [Ww][Oo][Rr][Kk][Ii][Nn][Gg][Ss][Ee][Tt];
WRITE           :   [Ww][Rr][Ii][Tt][Ee];
WRITEFILE       :   [Ww][Rr][Ii][Tt][Ee][Ff][Ii][Ll][Ee];
X64             :   [Xx][6][4];

//============================================================
// ПРОБЕЛЫ И КОММЕНТАРИИ
//============================================================

WS          : [ \t\r\n\u000C]+ -> skip;

COMMENT
            : '/*' .*? ('\n' | '*/') -> skip
            ;


