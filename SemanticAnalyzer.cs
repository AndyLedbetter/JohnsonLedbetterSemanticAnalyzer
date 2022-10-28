using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS426.node;

namespace CS426.analysis
{
    class SemanticAnalyzer : DepthFirstAdapter
    {
        Dictionary<string, Definition> globalSymbolTable;

        Dictionary<string, Definition> localSymbolTable;

        Dictionary<Node, Definition> decoratedParseTree = new Dictionary<Node, Definition>();

        public void PrintWarning(Token t, string message)
        {
            Console.WriteLine(t.Line + "," + t.Pos + ":" + message);
        }

        public override void InAProgram(AProgram node)
        {
            Definition intDefinition = new NumberDefinition();
            intDefinition.name = "$int";

            Definition strDefinition = new NumberDefinition();
            strDefinition.name = "$string";

            globalSymbolTable.Add("$int", intDefinition);
            globalSymbolTable.Add("$string", strDefinition);
        }

        //Operands
        public override void OutAIntOperand(AIntOperand node)
        {
            Definition intDefinition= new NumberDefinition();
            intDefinition.name = "$int";

            //Add to tree
            decoratedParseTree.Add(node, intDefinition);
        }

        public override void OutA(AcharOperand node)
        {
            Definition strDefinition = new StringDefinition();
            strDefinition.name = "string";

            decoratedParseTree.Add(node, strDefinition);
        }

        public override void OutAVariableOperand(AVariableOperand node)
        {
            string varname = node.GetId().Text;
            Definition varDefinition;

            if(!localSymbolTable.TryGetValue(varname, out varDefinition))
            {
                PrintWarning(node.GetId(), "Identifier " + varname + " does not exist");
            }
            else if(!(varDefinition is VariableDefinition))
            {
                PrintWarning(node.GetId(), "Identifier " + varname + " is not a variable");
            }
            else
            {
                VariableDefinition v = (VariableDefinition) varDefinition;

                decoratedParseTree.Add(node, v.variableType);
            }
        }
        //Expression 11
        public override void OutAPassExpression11(APassExpression11 node)
        {
            Definition operandDefinition;

            if(!decoratedParseTree.TryGetValue(node.GetOperand(), out operandDefinition))
            {

            }
            else
            {
                decoratedParseTree.Add(node, operandDefinition);
            }
        }

        public override void OutANegativeExpression11(ANegativeExpression11 node)
        {
            Definition operandDefinition;

            if(!decoratedParseTree.TryGetValue(node.GetOperand(), out operandDefinition))
            {

            }
            else if(!(operandDefinition is NumberDefinition))
            {
                PrintWarning(node.GetMinus(), "Only a number can be negated");
            }
            else
            {
                decoratedParseTree.Add(node, operandDefinition);
            }
        }

        //Expression 10
        public override void OutAPassExpression10(APassExpression10 node)
        {
            Definition expression3Def;

            if(!decoratedParseTree.TryGetValue(node.GetExpression11(), out expression3Def))
            {

            }
            else
            {
                decoratedParseTree.Add(node, expression3Def);
            }
        }

        //Expression 1

        public override void OutAPassExpression(APassExpression node)
        {
            Definition expressionDef2;

            if(!decoratedParseTree.TryGetValue(node.GetExpression2(), out expressionDef2))
            {

            }
            else
            {
                decoratedParseTree.Add(node, expressionDef2);
            }
        }

        //Declare
        public override void OutAIntAndFloatDeclareDeclareStatement(AIntAndFloatDeclareDeclareStatement node)
        {
            Definition typeDef;
            Definition idDef;

            if(!globalSymbolTable.TryGetValue(node.GetType().Text, out typeDef))
            {
                PrintWarning(node.GetType(), "Type " + node.GetType().Text, out idDef);
            }
            else if(localSymbolTable.TryGetValue(node.GetVarname().Text, out idDef))
            {
                    PrintWarning(node.GetVarname(), "ID " + node.GetVarname().Text + " had already been declared");
            
            }
            else
            {
                VariableDefinition newVariableDefinition = new VariableDefinition();
                newVariableDefinition.name = node.GetVarname().Text;
                newVariableDefinition.variableType = (TypeDefinition)typeDef;

                localSymbolTable.Add(node.getVarname().Text, newVariableDefinition);
            }
        }

        //Assignment

        public override void OutAAssignmentStatement(AAssignmentStatement node)
        {
            Definition idDef;
            Definition expressionDef;

            if(!localSymbolTable.TryGetValue(node.GetId().Text, out idDef))
            {
                PrintWarning(node.GetId(), "ID " + node.GetId().Text + " does not exist");

            }
            else if(!(idDef is VariableDefinition))
            {
                PrintWarning(node.GetId(), " ID " + node.GetId().Text + " is not a variable");
            }
            else if(!decoratedParseTree.TryGetValue(node.GetExpression(), out expressionDef))
            {

            }
            else if(((VariableDefinition)idDef).variableType.name != expressionDef.name)
            {
                PrintWarning(node.GetId(), "Cannot assign value of type " + expressionDef.name + " to variable of type " + ((VariableDefinition)idDef).variableType.name);
            }
            else
            {

            }
        }
    }

}
