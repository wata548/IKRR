using System;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace Data.Event {
    public class SingleScript {

        public readonly string Context;
        public readonly Sprite TargetImage;
        public readonly Button[] Buttons;
        
        public SingleScript(string pContext) {
            var args = pContext
                .Split('|')
                .Select(arg => arg.Trim())
                .ToList();
            
            var imgCandidate = args.First();
            if (imgCandidate[0] == '#') {
                args.RemoveAt(0);
                TargetImage = Resources.Load<Sprite>(imgCandidate[1..]);
            }
            Context = args[0];
            args.RemoveAt(0);
            
            if (args.Count == 0) {
                Buttons = Array.Empty<Button>();
                return;
            }
                        

            Buttons = args.Select(arg => new Button(arg)).ToArray();
        }

        public override string ToString() {
            var builder = new StringBuilder();
            builder.AppendLine("Context");
            builder.AppendLine($"\t{Context}");
            builder.AppendLine("TargetImage");
            builder.AppendLine($"\t{TargetImage?.name ?? "Null"}");
            builder.AppendLine("Buttons");
            foreach (var button in Buttons) {
                builder.AppendLine($"\t{button}");
            }    
            
            return builder.ToString();
        }
    }
}