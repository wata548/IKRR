using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Extension;

namespace Character.Skill {
    public static partial class SkillInterpreter {


        private const char IGNORE_SYMBOL = '`'; 
        private static readonly ReadOnlyDictionary<char, int> CalculatePriority = new(
            new Dictionary<char, int> {
                {'*', 2},
                {'+', 1},
                {'[', 0},
            }
        );
        
        public static ISkill Interpret(string pInput) {

            if (string.IsNullOrWhiteSpace(pInput))
                return null;
            
            var result = new Stack<INoSymbolToken>();
            var symbolBuffer = new Stack<OperatorToken>();
            var buffer = new StringBuilder();

            var prev = ' ';
            foreach (var c in pInput) {
                
                switch (c) {
                    case ']': 
                        AddToken(buffer.ToString());
                        buffer.Clear();
                        while(symbolBuffer.Peek().Symbol != '[')
                            SymbolAddToResult();
                        symbolBuffer.Pop();
                        break;
                    
                    case '[':
                        symbolBuffer.Push(new(c, CalculatePriority[c]));
                        break;
                    
                    default:
                        if (prev != '`' && CalculatePriority.TryGetValue(c, out var priority)) {
                            AddToken(buffer.ToString());
                            buffer.Clear();
                            AddSymbol(c);
                        }
                        else {
                            if(c != IGNORE_SYMBOL)
                                buffer.Append(c);
                            prev = c;
                        }
                        break;
                }
            }
            //remain on buffers
            AddToken(buffer.ToString());
            while (symbolBuffer.Count > 0) {
                SymbolAddToResult();
            }

            return (result.Peek() as CommandToken)!.Skill;
                    
            //<=============| SubMethods |=============>\\
            void AddToken(string pContext) {

                pContext = pContext.Trim();
                if (string.IsNullOrEmpty(pContext))
                    return;
                
                if(int.TryParse(pContext, out var value))
                    result.Push(new NumberToken(value));
                else
                    result.Push(new CommandToken(Generate(pContext))); 
            }
            void AddSymbol(char c) {
                if (!CalculatePriority.TryGetValue(c, out var value)) 
                    return;
                
                var token = new OperatorToken(c, value);
                while (symbolBuffer.Count > 0 && symbolBuffer.Peek().Priority >= token.Priority) {
                    SymbolAddToResult();
                }
                symbolBuffer.Push(token);
            }

            void SymbolAddToResult() {

                var targetSymbol = symbolBuffer.Pop();
                var b = result.Pop();
                var a = (result.Peek() as CommandToken)!;
                
                if (a.Skill is SkillBase command) {
                    result.Pop();
                    CommandToken temp = null;
                                        
                    switch (targetSymbol.Symbol) {
                        case '*':
                            var count = (b as NumberToken)!.Amount;
                            var composite = new SkillComposite(count, command);
                            temp = new CommandToken(composite);
                            break;
                        case '+':
                            var target = (b as CommandToken)!.Skill;
                            composite = new SkillComposite(command, target);
                            temp = new CommandToken(composite);
                            break;
                        default:
                            throw new Exception();
                    }
                                        
                    result.Push(temp);
                }
                else if (a.Skill is SkillComposite composite) {
                    switch (targetSymbol.Symbol) {
                        case '*':
                            var count = (b as NumberToken)!.Amount;
                            composite.SetRepeatCount(count);
                            break;
                        case '+':
                            var target = (b as CommandToken)!.Skill;
                            if (composite.RepeatCount == 1) {
                                composite.AddSkill(target);
                            }
                            else {
                                result.Pop();
                                composite = new SkillComposite(composite, target);
                                var temp = new CommandToken(composite); 
                                result.Push(temp);
                            }
                            break;
                        
                        default:
                            throw new Exception();
                    }
                }
            }
        }
    }
}