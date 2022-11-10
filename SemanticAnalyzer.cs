using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS426.node;

namespace CS426.analysis
{
    class SemanticAnalyzer : DepthFirstAdapter
    {
        Dictionary<string, Definition> globalSymbolTable = new Dictionary<string, Definition>();

        Dictionary<string, Definition> localSymbolTable = new Dictionary<string, Definition>();

        Dictionary<string, Definition> functionSymbolTable = new Dictionary<string, Definition>();

        Dictionary<Node, Definition> decoratedParseTree = new Dictionary<Node, Definition>();

        List<VariableDefinition> tmpParams = new List<VariableDefinition>();

        List<TypeDefinition> tmpParamsCompare = new List<TypeDefinition>();

        public void PrintWarning(Token t, string message)
        {
            Console.WriteLine(t.Line + "," + t.Pos + ":" + message);
        }

        public override void InAProgram(AProgram node)
        {
            Definition intDefinition = new NumberDefinition();
            intDefinition.name = "num";

            Definition strDefinition = new NumberDefinition();
            strDefinition.name = "s";

            globalSymbolTable.Add("num", intDefinition);
            globalSymbolTable.Add("s", strDefinition);
        }

        //////////////////////Operands///////////////////////////////////////////////

        public override void OutAIntegerOneConstant(AIntegerOneConstant node)
        {

            Definition intDefinition = new NumberDefinition();
            intDefinition.name = "const num";

            //add to global

            globalSymbolTable.Add("const num", intDefinition);

            // Don't need to add to parse tree
            //decoratedParseTree.Add(node, intDefinition);
        }

        public override void OutAFloatsOneConstant(AFloatsOneConstant node)
        {
            Definition floatDefinition = new NumberDefinition();
            floatDefinition.name = "const fp";

            globalSymbolTable.Add("const fp", floatDefinition);

        }

        //////////////////////Operands///////////////////////////////////////////////
        public override void OutAIntOperand(AIntOperand node)
        {
            Definition intDefinition = new NumberDefinition();
            intDefinition.name = "num";

            //Add to tree
            decoratedParseTree.Add(node, intDefinition);
        }

        public override void OutAFloatOperand(AFloatOperand node)
        {
            Definition floatDefinition = new NumberDefinition();
            floatDefinition.name = "fp";

            decoratedParseTree.Add(node, floatDefinition);

        }

        public override void OutAStringOperand(AStringOperand node)
        {
            Definition strDefinition = new StringDefinition();
            strDefinition.name = "s";

            decoratedParseTree.Add(node, strDefinition);
        }

        public override void OutAVariableOperand(AVariableOperand node)
        {
            string varname = node.GetId().Text;
            Definition varDefinition;

            if (!localSymbolTable.TryGetValue(varname, out varDefinition))
            {
                PrintWarning(node.GetId(), "Identifier " + varname + " does not exist");
            }
            else if (!(varDefinition is VariableDefinition))
            {
                PrintWarning(node.GetId(), "Identifier " + varname + " is not a variable");
            }
            else
            {
                VariableDefinition v = (VariableDefinition)varDefinition;

                decoratedParseTree.Add(node, v.variableType);
            }
        }
        /////////////////////////////////END OF OPERANDS///////////////////////////////////////////

        ///////////////////////////Expression 7////////////////////////////////////////////////////
        public override void OutAPassExpression7(APassExpression7 node)
        {
            Definition expression7Def;

            if (!decoratedParseTree.TryGetValue(node.GetOperand(), out expression7Def))
            {

            }
            else
            {
                decoratedParseTree.Add(node, expression7Def);
            }
        }

        public override void OutAParenthesisExpression7(AParenthesisExpression7 node)
        {
            Definition expressionDef;

            if (!decoratedParseTree.TryGetValue(node.GetExpression(), out expressionDef))
            {

            }
            else
            {
                decoratedParseTree.Add(node, expressionDef);
            }
        }

        ///////////////////////////Expression 6////////////////////////////////////////////////////

        public override void OutAPassExpression6(APassExpression6 node)
        {
            Definition expression6Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression7(), out expression6Def))
            {

            }
            else
            {
                decoratedParseTree.Add(node, expression6Def);
            }
        }

        public override void OutANotExpression6(ANotExpression6 node)
        {
            Definition expression7Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression7(), out expression7Def))
            {

            }
            else if (!(expression7Def is BooleanDefinition))
            {
                PrintWarning(node.GetNot(), "Can Only Not a Boolean Value");
            }
            else
            {
                decoratedParseTree.Add(node, expression7Def);
            }
        }

        public override void OutANegativeExpression6(ANegativeExpression6 node)
        {
            Definition expression7Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression7(), out expression7Def))
            {

            }
            else if (!(expression7Def is NumberDefinition))
            {
                PrintWarning(node.GetMinus(), "Only a number can be negated");
            }
            else
            {
                decoratedParseTree.Add(node, expression7Def);
            }
        }

        ///////////////////////////Expression 5////////////////////////////////////////////////////

        public override void OutAPassExpression5(APassExpression5 node)
        {
            Definition expression6Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression6(), out expression6Def))
            {

            }
            else
            {
                decoratedParseTree.Add(node, expression6Def);
            }
        }
        public override void OutAMultiplyExpression5(AMultiplyExpression5 node)
        {
            Definition expression5Def;
            Definition expression6Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression5(), out expression5Def))
            {

            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression6(), out expression6Def))
            {

            }
            else if (expression5Def.GetType() != expression6Def.GetType())
            {
                PrintWarning(node.GetMultiply(), "Cannot multiply " + expression5Def.name
                    + " by " + expression6Def.name);
            }
            else if (!(expression5Def is NumberDefinition))
            {
                PrintWarning(node.GetMultiply(), "Cannot multiply something of type "
                    + expression5Def.name);
            }
            else
            {
                // Decorate ourselves (either expression5def or expression6def would work)
                decoratedParseTree.Add(node, expression5Def);
            }
        }

        public override void OutADivideExpression5(ADivideExpression5 node)
        {
            Definition expression5Def;
            Definition expression6Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression5(), out expression5Def))
            {

            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression6(), out expression6Def))
            {

            }
            else if (expression5Def.GetType() != expression6Def.GetType())
            {
                PrintWarning(node.GetDivide(), "Cannot divide " + expression5Def.name
                    + " by " + expression6Def.name);
            }
            else if (!(expression5Def is NumberDefinition))
            {
                PrintWarning(node.GetDivide(), "Cannot divide something of type "
                    + expression5Def.name);
            }
            else
            {
                // Decorate ourselves (either expression5def or expression6def would work)
                decoratedParseTree.Add(node, expression5Def);
            }
        }

        ///////////////////////////Expression 4////////////////////////////////////////////////////
        public override void OutAPassExpression4(APassExpression4 node)
        {
            Definition expression5Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression5(), out expression5Def))
            {

            }
            else
            {
                decoratedParseTree.Add(node, expression5Def);
            }
        }
        public override void OutAAddExpression4(AAddExpression4 node)
        {
            Definition expression4Def;
            Definition expression5Def;
            Definition strDefinition = new StringDefinition();

            if (!decoratedParseTree.TryGetValue(node.GetExpression4(), out expression4Def))
            {

            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression5(), out expression5Def))
            {

            }
            else if (!(expression4Def is NumberDefinition))
            {
                PrintWarning(node.GetPlus(), "Cannot add something of type "
                    + expression4Def.name);
            }
            else if (!(expression5Def is NumberDefinition))
            {
                PrintWarning(node.GetPlus(), "Cannot add something of type "
                    + expression5Def.name);
            }
            else if (expression4Def.GetType() == strDefinition.GetType() || expression5Def.GetType() == strDefinition.GetType())
            {
                PrintWarning(node.GetPlus(), "Cannot add strings");
            }
            else if (expression4Def.GetType() != expression5Def.GetType())
            {
                //TO DO - Check to see variables are in const table
                PrintWarning(node.GetPlus(), "Cannot add " + expression4Def.name
                    + " by " + expression5Def.name);
            }
            else
            {
                // Decorate ourselves (either expression4def or expression5def would work)
                decoratedParseTree.Add(node, expression4Def);
            }
        }

        public override void OutASubtractExpression4(ASubtractExpression4 node)
        {
            Definition expression4Def;
            Definition expression5Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression4(), out expression4Def))
            {

            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression5(), out expression5Def))
            {

            }
            else if (expression4Def.GetType() != expression5Def.GetType())
            {
                PrintWarning(node.GetMinus(), "Cannot subtract " + expression4Def.name
                    + " by " + expression5Def.name);
            }
            else if (!(expression4Def is NumberDefinition))
            {
                PrintWarning(node.GetMinus(), "Cannot subtract something of type "
                    + expression4Def.name);
            }
            else if (!(expression5Def is NumberDefinition))
            {
                PrintWarning(node.GetMinus(), "Cannot subtract something of type "
                    + expression5Def.name);
            }
            else
            {
                // Decorate ourselves (either expression4def or expression5def would work)
                decoratedParseTree.Add(node, expression4Def);
            }
        }

        ///////////////////////////Expression 3////////////////////////////////////////////////////

        public override void OutAPassExpression3(APassExpression3 node)
        {
            Definition expression4Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression4(), out expression4Def))
            {

            }
            else
            {
                decoratedParseTree.Add(node, expression4Def);
            }
        }

        public override void OutAGreaterThanExpression3(AGreaterThanExpression3 node)
        {
            Definition expression3Def;
            Definition expression4Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression3(), out expression3Def))
            {

            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression4(), out expression4Def))
            {

            }
            else if ((expression3Def.GetType() != expression4Def.GetType()))
            {
                PrintWarning(node.GetGreaterthan(), "Cannot compare " + expression3Def.name
                    + " by " + expression4Def.name);
            }
            else if (!(expression3Def is NumberDefinition))
            {
                PrintWarning(node.GetGreaterthan(), "Cannot compare something of type "
                    + expression3Def.name);
            }
            else
            {
                // Decorate ourselves (either expression3def or expression4def would work)
                decoratedParseTree.Add(node, new BooleanDefinition());
            }
        }
        public override void OutALessThanExpression3(ALessThanExpression3 node)
        {
            Definition expression3Def;
            Definition expression4Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression3(), out expression3Def))
            {

            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression4(), out expression4Def))
            {

            }
            else if ((expression3Def.GetType() != expression4Def.GetType()))
            {
                PrintWarning(node.GetLessthan(), "Cannot compare " + expression3Def.name
                    + " by " + expression4Def.name);
            }
            else if (!(expression3Def is NumberDefinition))
            {
                PrintWarning(node.GetLessthan(), "Cannot compare something of type "
                    + expression3Def.name);
            }
            else
            {
                // Decorate ourselves (either expression3def or expression4def would work)
                decoratedParseTree.Add(node, new BooleanDefinition());
            }
        }

        public override void OutAGreaterThanOrEqualsExpression3(AGreaterThanOrEqualsExpression3 node)
        {
            Definition expression3Def;
            Definition expression4Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression3(), out expression3Def))
            {

            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression4(), out expression4Def))
            {

            }
            else if ((expression3Def.GetType() != expression4Def.GetType()))
            {
                PrintWarning(node.GetGreaterthanorequals(), "Cannot compare " + expression3Def.name
                    + " by " + expression4Def.name);
            }
            else if (!(expression3Def is NumberDefinition))
            {
                PrintWarning(node.GetGreaterthanorequals(), "Cannot compare something of type "
                    + expression3Def.name);
            }
            else
            {
                // Decorate ourselves (either expression3def or expression4def would work)
                decoratedParseTree.Add(node, new BooleanDefinition());
            }
        }

        public override void OutALessThanOrEqualsExpression3(ALessThanOrEqualsExpression3 node)
        {
            Definition expression3Def;
            Definition expression4Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression3(), out expression3Def))
            {

            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression4(), out expression4Def))
            {

            }
            else if ((expression3Def.GetType() != expression4Def.GetType()))
            {
                PrintWarning(node.GetLessthanorequals(), "Cannot compare " + expression3Def.name
                    + " by " + expression4Def.name);
            }
            else if (!(expression3Def is NumberDefinition))
            {
                PrintWarning(node.GetLessthanorequals(), "Cannot compare something of type "
                    + expression3Def.name);
            }
            else
            {
                // Decorate ourselves (either expression3def or expression4def would work)
                decoratedParseTree.Add(node, new BooleanDefinition());
            }
        }

        ///////////////////////////Expression 2////////////////////////////////////////////////////

        public override void OutAPassExpression2(APassExpression2 node)
        {
            Definition expression3Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression3(), out expression3Def))
            {

            }
            else
            {
                decoratedParseTree.Add(node, expression3Def);
            }
        }

        public override void OutAEqualsExpression2(AEqualsExpression2 node)
        {
            Definition expression2Def;
            Definition expression3Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression2(), out expression2Def))
            {

            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression3(), out expression3Def))
            {

            }
            else if ((expression2Def.GetType() != expression3Def.GetType()))
            {
                PrintWarning(node.GetEquals(), "Cannot equate " + expression2Def.name
                    + " by " + expression3Def.name);
            }
            else if (!(expression2Def is NumberDefinition))
            {
                PrintWarning(node.GetEquals(), "Cannot equate something of type "
                    + expression2Def.name);
            }
            else
            {
                // Decorate ourselves (either expression2def or expression3def would work)
                decoratedParseTree.Add(node, new BooleanDefinition());
            }
        }


        public override void OutANotEqualExpression2(ANotEqualExpression2 node)
        {
            Definition expression2Def;
            Definition expression3Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression2(), out expression2Def))
            {

            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression3(), out expression3Def))
            {

            }
            else if ((expression2Def.GetType() != expression3Def.GetType()))
            {
                PrintWarning(node.GetNotEquals(), "Cannot equate " + expression2Def.name
                    + " by " + expression3Def.name + " (Not Equals)");
            }
            else if (!(expression2Def is NumberDefinition))
            {
                PrintWarning(node.GetNotEquals(), "Cannot equate something of type "
                    + expression2Def.name + " (Not Equals)");
            }
            else
            {
                // Decorate ourselves (either expression2def or expression3def would work)
                decoratedParseTree.Add(node, new BooleanDefinition());
            }
        }

        ///////////////////////////Expression 1////////////////////////////////////////////////////


        public override void OutAPassExpression1(APassExpression1 node)
        {
            Definition expression2Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression2(), out expression2Def))
            {

            }
            else
            {
                decoratedParseTree.Add(node, expression2Def);
            }
        }

        public override void OutAAndExpression1(AAndExpression1 node)
        {
            Definition expression1Def;
            Definition expression2Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression1(), out expression1Def))
            {

            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression2(), out expression2Def))
            {

            }
            else if ((expression1Def.GetType() != expression2Def.GetType()))
            {
                PrintWarning(node.GetAnd(), "Cannot AND " + expression1Def.name
                    + " by " + expression2Def.name);
            }
            else if (!(expression1Def is BooleanDefinition))
            {
                PrintWarning(node.GetAnd(), "Cannot AND something of type "
                    + expression1Def.name);
            }
            else
            {
                // Decorate ourselves (either expression1def or expression2def would work)
                decoratedParseTree.Add(node, expression1Def);
            }
        }

        ///////////////////////////Expression 1////////////////////////////////////////////////////


        public override void OutAPassExpression(APassExpression node)
        {
            Definition expression1Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression1(), out expression1Def))
            {

            }
            else
            {
                decoratedParseTree.Add(node, expression1Def);
            }
        }

        public override void OutAOrExpression(AOrExpression node)
        {
            Definition expressionDef;
            Definition expression1Def;

            if (!decoratedParseTree.TryGetValue(node.GetExpression(), out expressionDef))
            {

            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression1(), out expression1Def))
            {

            }
            else if ((expressionDef.GetType() != expression1Def.GetType()))
            {
                PrintWarning(node.GetOr(), "Cannot OR " + expressionDef.name
                    + " by " + expression1Def.name);
            }
            else if (!(expressionDef is BooleanDefinition))
            {
                PrintWarning(node.GetOr(), "Cannot OR something of type "
                    + expressionDef.name);
            }
            else
            {
                // Decorate ourselves (either expressiondef or expression1def would work)
                decoratedParseTree.Add(node, expressionDef);
            }
        }

        /////////////////////////////////END OF EXPRESSIONS///////////////////////////////////////////

        ///////////////////////////////////VARIABLE DECLARE////////////////////////////////////////////////////

        public override void OutADeclareStatement(ADeclareStatement node)
        {
            Definition typeDef;
            Definition idDef;

            if (!globalSymbolTable.TryGetValue(node.GetType().Text, out typeDef))
            {
                PrintWarning(node.GetType(), "Type " + node.GetType().Text + " does not exist");
            }
            else if (localSymbolTable.TryGetValue(node.GetVarname().Text, out idDef))
            {
                PrintWarning(node.GetVarname(), "ID " + node.GetVarname().Text + " has already been declared");

            }
            else
            {
                VariableDefinition newVariableDefinition = new VariableDefinition();
                newVariableDefinition.name = node.GetVarname().Text;
                newVariableDefinition.variableType = (TypeDefinition)typeDef;

                localSymbolTable.Add(node.GetVarname().Text, newVariableDefinition);
            }
        }

        ///////////////////////////////////VARIABLE ASSIGN////////////////////////////////////////////////////

        public override void OutAAssignStatement(AAssignStatement node)
        {
            Definition idDef;
            Definition expressionDef;

            if (!localSymbolTable.TryGetValue(node.GetId().Text, out idDef) && (!functionSymbolTable.TryGetValue(node.GetId().Text, out idDef)))
            {
                PrintWarning(node.GetId(), "ID " + node.GetId().Text + " does not exist");

            }
            else if (!(idDef is VariableDefinition))
            {
                PrintWarning(node.GetId(), " ID " + node.GetId().Text + " is not a variable");
            }
            else if (!decoratedParseTree.TryGetValue(node.GetExpression(), out expressionDef))
            {

            }
            else if (((VariableDefinition)idDef).variableType.name != expressionDef.name)
            {
                PrintWarning(node.GetId(), "Cannot assign value of type " + expressionDef.name + " to variable of type " + ((VariableDefinition)idDef).variableType.name);
            }
            else
            {
                //All tests passed
            }
        }

        ///////////////////////////////////FUNCTION DECLARE//////////////////////////////////////////////////

        public override void CaseAFunction(AFunction node)
        {
            tmpParams.Clear();
            functionSymbolTable.Clear();

            InAFunction(node);

            if (node.GetDefine() != null)
            {
                node.GetDefine().Apply(this);
            }
            if (node.GetId() != null)
            {
                node.GetId().Apply(this);
            }
            if (node.GetOone() != null)
            {
                node.GetOone().Apply(this);
            }
            if (node.GetFunctionArguments() != null)
            {
                node.GetFunctionArguments().Apply(this);
                // Create a Function Definition
                Definition idDef;

                if (globalSymbolTable.TryGetValue(node.GetId().Text, out idDef))
                {
                    PrintWarning(node.GetId(), "Identifier " + node.GetId().Text + " is already being used");
                }
                else
                {
                    // Wipes out the local symbol table
                    //localSymbolTable = new Dictionary<string, Definition>();

                    FunctionDefinition newFunctionDefinition = new FunctionDefinition();

                    // TODO:  You will have to figure out how to populate this with parameters
                    // when you work on PEX 3
                    List<VariableDefinition> parameters = new List<VariableDefinition>();
                    List<TypeDefinition> types = new List<TypeDefinition>();

                    //newFunctionDefinition.parameters = tmpParams;

                    for (int i = 0; i < tmpParams.Count(); i++)
                    {
                        newFunctionDefinition.parameters.Add(tmpParams[i]);
                    }

                    // Adds the Function!
                    globalSymbolTable.Add(node.GetId().Text, newFunctionDefinition);

                }
            }
            if (node.GetCone() != null)
            {
                node.GetCone().Apply(this);
            }
            if (node.GetOtwo() != null)
            {
                node.GetOtwo().Apply(this);
            }
            if (node.GetStatements() != null)
            {
                node.GetStatements().Apply(this);
            }
            if (node.GetCtwo() != null)
            {
                node.GetCtwo().Apply(this);
            }
            if (node.GetEol() != null)
            {
                node.GetEol().Apply(this);
            }
            OutAFunction(node);
        }

        public override void OutAMultipleFunctionArguments(AMultipleFunctionArguments node)
        {
            //Arguments are added in argument node
        }

        public override void OutADecArguement(ADecArguement node)
        {

            VariableDefinition argument = new VariableDefinition();
            TypeDefinition type = new TypeDefinition();

            type.name = node.GetType().Text;

            argument.name = node.GetVarname().Text;
            argument.variableType = type;

            tmpParams.Add(argument);
            functionSymbolTable.Add(argument.name, argument);


        }

        public override void OutAFunction(AFunction node)
        {
            // Wipes out the local symbol table so that the next function doesn't have to deal with it
            //localSymbolTable = new Dictionary<string, Definition>();
        }

        ///////////////////////////////////FUNCTION CALL//////////////////////////////////////////////////

        public override void OutAParameter(AParameter node)
        {
            Definition expressionDef;

            if (!decoratedParseTree.TryGetValue(node.GetExpression(), out expressionDef))
            {
                // We are checking to see if the node below us was decorated.
                // We don't have to print an error, because if something bad happened
                // the error would have been printed at the lower node.
            }
            else if (!((expressionDef is NumberDefinition) || (expressionDef is StringDefinition)))
            {
                Console.WriteLine("Invalid Parameter: " + expressionDef);
            }

            TypeDefinition parameter = new TypeDefinition();
            NumberDefinition tmp = new NumberDefinition();

            tmp = (NumberDefinition)expressionDef;

            parameter.name = tmp.name;

            tmpParamsCompare.Add(parameter);
        }

        public override void OutAFunctionCallStatement(AFunctionCallStatement node)
        {
            Definition idDef = new FunctionDefinition();

            if (!globalSymbolTable.TryGetValue(node.GetId().Text, out idDef))
            {
                PrintWarning(node.GetId(), "ID " + node.GetId().Text + " not found");
            }
            else if (!(idDef is FunctionDefinition))
            {
                PrintWarning(node.GetId(), "ID " + node.GetId().Text + " is not a function");
            }
            else
            {
                FunctionDefinition newDef = (FunctionDefinition)idDef;
                List<VariableDefinition> parameters = new List<VariableDefinition>();
                List<TypeDefinition> parameterTypes = new List<TypeDefinition>();
                List<TypeDefinition> argumentTypes = new List<TypeDefinition>();

                //Compare lengths
                if (newDef.parameters.Count() != tmpParamsCompare.Count())
                {
                    PrintWarning(node.GetId(), "Parameter lists are not the same size");
                }

                for (int i = 0; i < tmpParams.Count(); i++)
                {
                    parameterTypes.Add(tmpParamsCompare[i]);
                }

                tmpParamsCompare.Clear();

                for (int i = 0; i < newDef.parameters.Count(); i++)
                {
                    argumentTypes.Add(newDef.parameters[i].variableType);
                }

                for (int i = 0; i < argumentTypes.Count(); i++)
                {
                    if (!(argumentTypes[i].name == parameterTypes[i].name))
                    {
                        PrintWarning(node.GetId(), "Parameter types are not the same for parameter " + i);
                    }
                }

            }
        }

        public override void OutAIfState(AIfState node)
        {
            Definition ifConditonal;

            if (!decoratedParseTree.TryGetValue(node.GetConditional(), out ifConditonal))
            {
                //PrintWarning(node.GetId(), "ID " + node.GetId().Text + " not found");
            }
            else if (!(ifConditonal is BooleanDefinition))
            {
                Console.WriteLine("If conditional is not of boolean type");
            }

        }

        public override void OutAWhileState(AWhileState node)
        {
            Definition whileConditonal;

            if (!decoratedParseTree.TryGetValue(node.GetConditional(), out whileConditonal))
            {
                //PrintWarning(node.GetId(), "ID " + node.GetId().Text + " not found");
            }
            else if (!(whileConditonal is BooleanDefinition))
            {
                Console.WriteLine("If conditional is not of boolean type");
            }

        }
    }
}
