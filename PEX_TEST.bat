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
echo Testing Correct assign variables>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\assignVariableCorrect.txt >> %file%
echo. >> %file%

echo Testing Correct assign variables>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\ConditionalsCorrect.txt >> %file%
echo. >> %file%

echo Testing Correct assign variables>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\declareFunctionTestCorrect.txt >> %file%
echo. >> %file%

echo Testing Correct assign variables>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\declareVariableCorrect.txt >> %file%
echo. >> %file%

echo Testing Correct assign variables>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\functionsCallCorrect.txt >> %file%
echo. >> %file%

echo Testing Correct assign variables>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\iterativeCorrect.txt >> %file%
echo. >> %file%

echo Testing Correct assign variables>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\OrderOfOperationsCorrect.txt >> %file%
echo. >> %file%

echo Testing Correct assign variables>> %file%
bin\Debug\ConsoleApplication.exe testcases\pex2\extraTestFileCorrect.txt >> %file%
echo. >> %file%

pause