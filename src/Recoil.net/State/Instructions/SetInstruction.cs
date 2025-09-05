using System;
using System.Collections.Generic;
using System.Text;

namespace RecoilNet.State.Instructions
{
    internal class SetInstruction : Instruction
    {
        public object? Value { get; }

        public SetInstruction(Primitive primitive, object? value) : base(primitive)
        {
            Value = value;
        }

    }
}
