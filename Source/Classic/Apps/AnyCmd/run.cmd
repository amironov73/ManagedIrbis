@echo off

cd bin\Debug

rem AnyCmd.exe "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=A;" "2" "IBIS" "v200^a,10,100,1" "" "0" "0" "" ""
rem AnyCmd.exe "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=A;" "7" "IBIS" "@tabf1w" ""  "" "T=A$" "0" "0" "" ""
rem AnyCmd.exe "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=A;" "M" "2.IBIS.IBIS.mst" "C:\ibis.bak" 
rem AnyCmd.exe "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=A;" "+1"
rem AnyCmd.exe "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=A;" "V" "IBIS" "2" "1" "2"
rem AnyCmd.exe "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=A;" "Z" "IBIS"
rem AnyCmd.exe "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=A;" "H" "IBIS" "K=" "6" "@brief"
rem AnyCmd.exe "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=A;" "I" "IBIS" "6" "1" "@brief" "K=&"
rem AnyCmd.exe "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=C;" "G" "IBIS" "&uf('+0')" "1" "1"
rem AnyCmd.exe "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=C;" "J" "IBIS" "0" "!0ADD4000*OGO!" "0#00#0700#^aИванов^bИ. И.701#^aПетров^bП. П.200#^aЗаглавие^eподзаголовочное^fИ. И. Иванов, П. П. Петров210#^aИркутск^d2016215#^a123300#Первое примечание300#Второе примечание300#Третье примечание920#PAZK"
rem AnyCmd.exe "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=C;" "J" "IBIS" "0" "@none" "0#00#0700#^aИванов^bИ. И.701#^aПетров^bП. П.200#^aЗаглавие^eподзаголовочное^fИ. И. Иванов, П. П. Петров210#^aИркутск^d2016215#^a123300#Первое примечание300#Второе примечание300#Третье примечание920#PAZK"

rem 0#00#0700#^aИванов^bИ. И.701#^aПетров^bП. П.200#^aЗаглавие^eподзаголовочное^fИ. И. Иванов, П. П. Петров210#^aИркутск^d2016215#^a123300#Первое примечание300#Второе примечание300#Третье примечание920#PAZK
rem !0//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

rem AnyCmd.exe "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=A;" "L" "0..@logo.gif"

rem AnyCmd.exe "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=A;" "G" "IBIS" "!mpl,'999',/,v999" "-2" "0#00#0700#^aИванов^bИ. И.701#^aПетров^bП. П.200#^aЗаглавие^eподзаголовочное^fИ. И. Иванов, П. П. Петров210#^aИркутск^d2016215#^a123300#Первое примечание300#Второе примечание300#Третье примечание920#PAZK999#100500" "0#00#0700#^aИванов^bИ. И.701#^aПетров^bП. П.200#^aЗаглавие^eподзаголовочное^fИ. И. Иванов, П. П. Петров210#^aИркутск^d2016215#^a123300#Первое примечание300#Второе примечание300#Третье примечание920#PAZK999#100600"

rem AnyCmd.exe "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=A;" "K" "IBIS" "T=A$" 0 1 "@brief" "if p(v102) then '1' else '0' fi"

rem AnyCmd.exe "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=C;" "C" "IBIS" "1" "0" "v200"

AnyCmd.exe "host=127.0.0.1;port=6666;user=1;password=1;db=IBIS;arm=A;" "X" "RQST" "1" 

cd ..\..