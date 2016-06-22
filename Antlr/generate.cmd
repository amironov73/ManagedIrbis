@echo off
rem java -jar antlr4-csharp-4.3-complete.jar -Dlanguage=CSharp_v3_5 -package ManagedClient.Search IrbisSearchQuery.g4
rem java -jar antlr4-csharp-4.3-complete.jar -Dlanguage=CSharp_v4_0 -package ManagedClient.Search IrbisSearchQuery.g4
java -jar antlr4-csharp-4.3-complete.jar -Dlanguage=CSharp_v4_5 -package ManagedClient.Search IrbisSearchQuery.g4


rem java -jar antlr4-csharp-4.3-complete.jar -Dlanguage=CSharp_v3_5 -package ManagedClient.Pft Pft.g4
rem java -jar antlr4-csharp-4.3-complete.jar -Dlanguage=CSharp_v3_5 -package ManagedClient.Quality FieldSpec.g4
rem java -jar antlr4-csharp-4.3-complete.jar -Dlanguage=CSharp_v3_5 -package AM.Text.Ranges NumberRanges.g4