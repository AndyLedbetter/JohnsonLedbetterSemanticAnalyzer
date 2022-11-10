using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS426.analysis
{
    public abstract class Definition
    {
        public string name = "";
        public string toString()
        {
            return name;
        }
    }

    public class TypeDefinition : Definition { }
    public class NumberDefinition : TypeDefinition { }
    public class StringDefinition: TypeDefinition { }
    public class BooleanDefinition : TypeDefinition { }
    public class VariableDefinition : Definition {
        public TypeDefinition variableType;
    }

    public class FunctionDefinition : Definition
    {
        public List<VariableDefinition> parameters = new List<VariableDefinition>();
    }




}
