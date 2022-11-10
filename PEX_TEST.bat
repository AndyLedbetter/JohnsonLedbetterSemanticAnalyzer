:: Creates a Variable for the Output File
@SET file="pex_test_results.txt"

:: Erases Everything Currently In the Output File
type NUL>%file%

:: ----------------------------------------
:: TITLE
:: ----------------------------------------
echo PEX TEST CASES (C1C Unnamed) >> %file%

:: ----------------------------------------
:: GOOD EXAMPLES
:: ----------------------------------------
echo Assign Statement>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\AssignStatementTest.txt >> %file%
echo. >> %file%

echo Boolean Statement>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\BooleanStatementTest.txt >> %file%
echo. >> %file%

echo Constants>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\ConstantTest.txt >> %file%
echo. >> %file%

echo Declare Statement>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\DeclareStatementTest.txt >> %file%
echo. >> %file%

echo FunctionCall Statement>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\FunctionCallStatementTest.txt >> %file%
echo. >> %file%

echo Function Test>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\FunctionTest.txt >> %file%
echo. >> %file%

echo iff elsee>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\IfElseTest.txt >> %file%
echo. >> %file%

echo Main Test>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\MainTest.txt >> %file%
echo. >> %file%

echo Not Test>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\NotTest.txt >> %file%
echo. >> %file%

echo While Test>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex3\WhileTest.txt >> %file%
echo. >> %file%

pause