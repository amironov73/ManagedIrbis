/* Интервалы чисел с префиксами.
*/

grammar NumberRanges;

program: item+ EOF;

item: range | one;

range:   start=NUMBER MINUS stop=NUMBER;

one:    NUMBER;

NUMBER: [0-9A-Za-zА-Яа-я/:_]+;
MINUS: '-';
DELIMITER: [ \t\r\n,;] -> skip;

