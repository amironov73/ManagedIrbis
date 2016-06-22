//============================================================
// ÂÑÒĞÎÅÍÍÛÉ ßÇÛÊ ÏÎÈÑÊÎÂÛÕ ÇÀÏĞÎÑÎÂ ÈĞÁÈÑ
// ãğàììàòèêà äëÿ ANTLR 4.2
// Àâòîğ: À. Â. Ìèğîíîâ
// Âåğñèÿ: 0.0.6
//============================================================
 
// Èñòî÷íèê íîğìàòèâíîé èíôîğìàöèè
// http://wiki.elnit.org/index.php/ßçûê_çàïğîñîâ_ÈĞÁÈÑ
 
grammar IrbisSearchQuery;

// Ñòàğòîâûé ñèìâîë
program
        : levelThree EOF
        ;
 
// Âåğõíèé óğîâåíü, íà êîòîğîì ìîãóò áûòü îòñûëêè ê ğåçóëüòàòàì
// ïğåäûäóùèõ ïîèñêîâ
levelThree
        : levelTwo                                          # levelTwoOuter
        | REFERENCE                                         # reference
        | left=levelThree op=(STAR|HAT) right=levelThree    # starOperator3
        | left=levelThree PLUS right=levelThree             # plusOperator3
        ;
 
// Âûğàæåíèÿ â ñêîáêàõ ìîãóò îáúåäèíÿòüñÿ òîëüêî îïåğàòîğàìè PLUS STAR HAT.
 
// Ïğîìåæóòî÷íûé óğîâåíü, íà êîòîğîì íåäîïóñòèìû êîíòåêñòíûå îïåğàòîğû,
// íî äîïóñòèìû ñêîáêè
levelTwo
        : levelOne                                      # levelOneOuter
        | LPAREN levelTwo RPAREN                        # parenOuter
        | left=levelTwo op=(STAR|HAT) right=levelTwo    # starOperator2
        | left=levelTwo PLUS right=levelTwo             # plusOperator2
        ;
 
// Îáğàòèòå âíèìàíèå íà ïğèîğèòåòû îïåğàòîğîâ:
// DOT
// F
// G
// STAR è HAT
// PLUS
 
// Ñàìûé íèæíèé óğîâåíü, íà êîòîğîì ğàáîòàşò êîíòåêñòíûå îïåğàòîğû
// (ïîıòîìó íåäîïóñòèìû ñêîáêè)
levelOne
        : ENTRY                                         # entry
        | left=levelOne DOT right=levelOne              # dotOperator
        | left=levelOne F   right=levelOne              # fOperator
        | left=levelOne G   right=levelOne              # gOperator
        | left=levelOne op=(STAR|HAT) right=levelOne    # starOperator1
        | left=levelOne PLUS right=levelOne             # plusOperator1
        ;
 
ENTRY
        : (QUOTED|NONQUOTED) (SLASH LPAREN TAGNUMBER (COMMA TAGNUMBER)* RPAREN)?
        ;
 
// Ñòğîêà â äâîéíûõ êàâû÷êàõ
QUOTED  : '"' .*? '"';
 
// Íåçàêàâû÷åííàÿ ñòğîêà
// \u0400-\u04FF - êèğèëëè÷åñêèå ñèìâîëû
NONQUOTED
        : [0-9A-Za-z\u0400-\u04FF=\[\]~!@$%&_'-] [0-9A-Za-z\u0400-\u04FF=\[\]~!@#$%&_'-]+
        ;
 
// Ññûëêà íà ğåçóëüòàòû ïğåäûäóùåãî ïîèñêà
REFERENCE : '#' [0-9]+;
 
// Ïîëå çàïèñè
TAGNUMBER : [0-9]+;
 
// Îïåğàòîğû
 
PLUS    : '+';   // Ëîãè÷åñêîå ÈËÈ
STAR    : '*';   // Ïğîñòîå ëîãè÷åñêîå È
HAT     : '^';   // Ëîãè÷åñêîå ÍÅ
G       : '(G)'; // Êîíòåêñòíîå È (â îäíîì ïîëå)
F       : '(F)'; // Êîíòåêñòíîå È (â îäíîì ïîâòîğåíèè)
DOT     : '.';   // Êîíòåêñòíîå È (ñëîâà ïîäğÿä)
 
// Ñëóæåáíûå ñèìâîëû
 
LPAREN  : '(';
RPAREN  : ')';
SLASH   : '/';
COMMA   : ',';
 
// Ïğîáåëû
WS: [ \t\r\n]+ -> skip;