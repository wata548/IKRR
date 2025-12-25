using Character.Skill;

namespace Character.Skill {
    public static partial class SkillInterpreter {
        private interface IToken { }

        private interface INoSymbolToken: IToken {}
        private class CommandToken: INoSymbolToken {
            public readonly ISkill Skill;
            
            public CommandToken(string pInput) => Skill = Generate(pInput);
            public CommandToken(ISkill pInput) => Skill = pInput;
        }
        private class OperatorToken: IToken {
            public readonly char Symbol;
            public readonly int Priority;
        
            public OperatorToken(char pSymbol, int pPriority) {
                Symbol = pSymbol;
                Priority = pPriority;
            }
        }
        private class NumberToken : INoSymbolToken {
            public readonly int Amount;
            public NumberToken(int pAmount) => Amount = pAmount;
        }
    }
}